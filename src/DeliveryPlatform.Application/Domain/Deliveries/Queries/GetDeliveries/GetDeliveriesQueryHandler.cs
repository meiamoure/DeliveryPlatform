using System;
using DeliveryPlatform.Application.Common;
using MediatR;
using DeliveryPlatform.Persistence.EFCore.DeliveryPlatformDb;
using Microsoft.EntityFrameworkCore;

namespace DeliveryPlatform.Application.Domain.Deliveries.Queries.GetDeliveries;

public class GetDeliveriesQueryHandler(DeliveryPlatformDbContext dbContext) : IRequestHandler<GetDeliveriesQuery, PageResponse<DeliveryDto[]>>
{
    public async Task<PageResponse<DeliveryDto[]>> Handle(GetDeliveriesQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Deliveries.AsQueryable();

        if (request.DeliveryId != Guid.Empty)
        {
            query = query.Where(d => d.Id == request.DeliveryId);
        }

        var total = await query.CountAsync(cancellationToken);

        var deliveries = await dbContext.Deliveries
    .Include(d => d.Node)
    .Skip((request.Page - 1) * request.PageSize)
    .Take(request.PageSize)
    .Select(d => new DeliveryDto(
        d.Id,
        d.OrderNumber,
        d.NodeId,
        d.Node.Name,
        d.PickupNodeId,
        d.WeightKg,
        d.VolumeM3,
        d.ProductGroup.ToString(),
        d.ServiceDate,
        d.WindowStart,
        d.WindowEnd,
        d.Priority,
        d.Status))
    .ToArrayAsync(cancellationToken);

        return new PageResponse<DeliveryDto[]>(total, deliveries);
    }
}
