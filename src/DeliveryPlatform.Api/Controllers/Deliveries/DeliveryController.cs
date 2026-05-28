using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DeliveryPlatform.Application.Domain.Deliveries.Commands.CreateDelivery;
using DeliveryPlatform.Application.Domain.Deliveries.Commands.UpdateDelivery;
using DeliveryPlatform.Application.Domain.Deliveries.Commands.ImportDeliveriesFromCsv;
using DeliveryPlatform.Application.Domain.Deliveries.Queries.GetDeliveries;
using DeliveryPlatform.Application.Domain.Deliveries.Queries.GetDeliveryById;
using DeliveryPlatform.Api.Controllers.Deliveries.Requests;
using DeliveryPlatform.Core.Common;
using DeliveryPlatform.Application.Domain.Deliveries.Commands.CancelDelivery;
using DeliveryPlatform.Application.Domain.Deliveries.Commands.DeleteDelivery;
using Microsoft.AspNetCore.Authorization;

namespace DeliveryPlatform.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Dispatcher")]
public class DeliveriesController : ControllerBase
{
    private readonly IMediator _mediator;
    public DeliveriesController(IMediator mediator) => _mediator = mediator;


    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken ct = default)
    {
        var q = new GetDeliveriesQuery(Guid.Empty, page, pageSize);
        var result = await _mediator.Send(q, ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var delivery = await _mediator.Send(new GetDeliveryByIdQuery(id), ct);
        if (delivery is null) return NotFound();
        return Ok(delivery);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
    [FromBody] CreateDeliveryRequest req,
    CancellationToken ct = default)
    {
        var cmd = new CreateDeliveryCommand(
            Name: req.Name,
            PickupNodeId: req.PickupNodeId,
            WeightKg: req.WeightKg,
            VolumeM3: req.VolumeM3,
            ProductGroup: (ProductGroup)req.ProductGroup,
            WindowStart: req.WindowStart,
            WindowEnd: req.WindowEnd,
            Priority: (DeliveryPriority)req.Priority
        );

        var id = await _mediator.Send(cmd, ct);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
    Guid id,
    [FromBody] UpdateDeliveryRequest req,
    CancellationToken ct = default)
    {
        var cmd = new UpdateDeliveryCommand(
            Id: id,
            Name: req.Name,
            PickupNodeId: req.PickupNodeId,
            WeightKg: req.WeightKg,
            VolumeM3: req.VolumeM3,
            ProductGroup: req.ProductGroup.HasValue
                ? (ProductGroup?)req.ProductGroup.Value
                : null,
            WindowStart: req.WindowStart,
            WindowEnd: req.WindowEnd,
            Priority: req.Priority.HasValue
                ? (DeliveryPriority?)req.Priority.Value
                : null
        );

        await _mediator.Send(cmd, ct);

        return NoContent();
    }

    [HttpPost("import")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Import(
        [FromForm] ImportDeliveriesRequest request)
    {
        if (request.File == null || request.File.Length == 0)
            return BadRequest("File is required");

        await using var stream = request.File.OpenReadStream();

        var command = new ImportDeliveriesFromCsvCommand(stream);

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new CancelDeliveryCommand(id), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteDeliveryCommand(id), ct);
        return NoContent();
    }
}
