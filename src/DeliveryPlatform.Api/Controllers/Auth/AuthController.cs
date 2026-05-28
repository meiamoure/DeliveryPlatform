using MediatR;
using DeliveryPlatform.Application.Auth.Commands.Login;
using Microsoft.AspNetCore.Mvc;
using DeliveryPlatform.Application.Auth.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace DeliveryPlatform.Api.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }
}
