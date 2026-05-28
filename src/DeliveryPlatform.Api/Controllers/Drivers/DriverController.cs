using DeliveryPlatform.Application.Domain.Routes.Commands.AcceptRoute;
using DeliveryPlatform.Application.Domain.Routes.Commands.CompleteRoute;
using DeliveryPlatform.Application.Domain.Routes.Commands.StartRoute;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DeliveryPlatform.Application.Domain.Drivers.Queries.GetMyRoute;
using DeliveryPlatform.Application.Domain.Drivers.Queries.GetDriverRouteHistory;

namespace DeliveryPlatform.Api.Controllers.Drivers;


[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Driver")]
public class DriverController : ControllerBase
{

    private readonly IMediator _mediator;
    public DriverController(IMediator mediator) => _mediator = mediator;

    [HttpGet("my-route")]
    public async Task<IActionResult> GetMyRoute(CancellationToken ct)
    {
        var driverIdClaim = User.FindFirst("driverId")?.Value;

        if (string.IsNullOrWhiteSpace(driverIdClaim))
            return Unauthorized();

        var driverId = Guid.Parse(driverIdClaim);

        var route = await _mediator.Send(new GetMyRouteQuery(driverId), ct);

        if (route == null)
            return NoContent();

        return Ok(route);
    }

    [HttpPost("{id:guid}/accept")]
    public async Task<IActionResult> Accept(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new AcceptRouteCommand(id), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/start")]
    public async Task<IActionResult> Start(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new StartRouteCommand(id), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new CompleteRouteCommand(id), ct);
        return NoContent();
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory(CancellationToken ct)
    {
        var driverIdClaim = User.FindFirst("driverId")?.Value;

        if (string.IsNullOrWhiteSpace(driverIdClaim))
            return Unauthorized();

        var driverId = Guid.Parse(driverIdClaim);

        var routes = await _mediator.Send(new GetDriverRouteHistoryQuery(driverId), ct);

        return Ok(routes);
    }
}
