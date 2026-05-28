using DeliveryPlatform.Core.Domain.Users.Common;
using MediatR;
using DeliveryPlatform.Application.Auth.Dtos;
using DeliveryPlatform.Application.Common.Security;


namespace DeliveryPlatform.Application.Auth.Commands.Login;
public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        IUserRepository users,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _users = users;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await _users.GetByLoginAsync(request.Login, ct);

        if (user is null)
            throw new UnauthorizedAccessException("Неверный логин или пароль");

        var isValidPassword = _passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!isValidPassword)
            throw new UnauthorizedAccessException("Неверный логин или пароль");

        var token = _jwtTokenService.CreateToken(user);

        return new LoginResponseDto(
            AccessToken: token,
            Login: user.Login,
            FullName: user.FullName,
            Role: user.Role.ToString()
        );
    }
}
