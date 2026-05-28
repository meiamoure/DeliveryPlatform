using DeliveryPlatform.Core.Domain.Users.Models;

namespace DeliveryPlatform.Application.Common.Security;
public interface IJwtTokenService
{
    string CreateToken(User user);
}
