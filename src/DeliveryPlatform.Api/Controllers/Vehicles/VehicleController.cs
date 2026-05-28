using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using DeliveryPlatform.Application.Domain.Vehicles.Commands.CreateVehicle;
using DeliveryPlatform.Application.Domain.Vehicles.Commands.SetVehicleStatus;
using DeliveryPlatform.Application.Domain.Vehicles.Commands.UnassignVehicle;
using DeliveryPlatform.Application.Domain.Vehicles.Queries.GetVehicles;
using DeliveryPlatform.Api.Controllers.Vehicles.Requests;
using Microsoft.AspNetCore.Authorization;


namespace DeliveryPlatform.Api.Controllers.Vehicles;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Dispatcher")]
public sealed class VehiclesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehiclesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetVehiclesQuery(), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateVehicleRequest request,
        CancellationToken ct)
    {
        var command = new CreateVehicleCommand(
            request.Plate,
            request.MaxWeightKg,
            request.MaxVolumeM3,
            request.SpeedKmh,
            request.DepotNodeId
        );

        var id = await _mediator.Send(command, ct);

        return CreatedAtAction(
            nameof(GetAll),
            new { id },
            new { id });
    }

    /*[HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateVehicleRequest request,
        CancellationToken ct)
    {
        var command = new UpdateVehicleCommand(
            id,
            request.Plate,
            request.MaxWeightKg,
            request.MaxVolumeM3,
            request.SpeedKmh,
            request.DepotNodeId
        );

        await _mediator.Send(command, ct);
        return NoContent();
    }*/

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> SetStatus(
        Guid id,
        [FromBody] SetVehicleStatusRequest request,
        CancellationToken ct)
    {
        await _mediator.Send(new SetVehicleStatusCommand(id, request.Status), ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/unassign")]
    public async Task<IActionResult> Unassign(
        Guid id,
        CancellationToken ct)
    {
        var command = new UnassignVehicleCommand(id);

        await _mediator.Send(command, ct);
        return NoContent();
    }
}

