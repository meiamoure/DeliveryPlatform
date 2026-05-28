using DeliveryPlatform.Core.Domain.Drivers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryPlatform.Core.Domain.Drivers.Common;

public interface IDriverRepository
{
    Task <Driver?> GetDriverByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Driver>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Driver driver, CancellationToken cancellationToken = default);
    Task UpdateAsync(Driver driver, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
