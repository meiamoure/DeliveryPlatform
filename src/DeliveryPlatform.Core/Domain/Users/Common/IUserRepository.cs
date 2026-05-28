using DeliveryPlatform.Core.Domain.Users.Models;

namespace DeliveryPlatform.Core.Domain.Users.Common;
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<User?> GetByLoginAsync(string login, CancellationToken ct);
    Task<bool> ExistsByLoginAsync(string login, CancellationToken ct);
    Task AddAsync(User user, CancellationToken ct);
}
