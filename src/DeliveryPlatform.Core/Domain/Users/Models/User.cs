using DeliveryPlatform.Core.Common;

namespace DeliveryPlatform.Core.Domain.Users.Models;
public sealed class User : IAggregateRoot
{
    private User() { }

    private User(
        Guid id,
        string login,
        string passwordHash,
        UserRole role,
        string fullName,
        Guid? driverId)
    {
        Id = id;
        Login = login;
        PasswordHash = passwordHash;
        Role = role;
        FullName = fullName;
        DriverId = driverId;
    }

    public Guid Id { get; private set; }
    public string Login { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public UserRole Role { get; private set; }
    public string FullName { get; private set; } = default!;
    public Guid? DriverId { get; private set; }

    public static User CreateDispatcher(
        string login,
        string passwordHash,
        string fullName)
    {
        return new User(
            id: Guid.NewGuid(),
            login: login,
            passwordHash: passwordHash,
            role: UserRole.Dispatcher,
            fullName: fullName,
            driverId: null
        );
    }

    public static User CreateDriver(
        string login,
        string passwordHash,
        string fullName,
        Guid driverId)
    {
        return new User(
            id: Guid.NewGuid(),
            login: login,
            passwordHash: passwordHash,
            role: UserRole.Driver,
            fullName: fullName,
            driverId: driverId
        );
    }

    public void ChangePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void UpdateProfile(string fullName)
    {
        FullName = fullName;
    }
}
