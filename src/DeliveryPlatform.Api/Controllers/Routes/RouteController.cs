using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DeliveryPlatform.Application.Domain.Routes.Commands.BuildRoute;
using DeliveryPlatform.Api.Controllers.Routes.Requests;
using MediatR;
using DeliveryPlatform.Application.Domain.Routes.Queries.GetRoutes;
using DeliveryPlatform.Application.Domain.Routes.Queries.GetRouteDetails;
using DeliveryPlatform.Application.Domain.Routes.Queries.GetRouteForDriver;
using DeliveryPlatform.Application.Domain.Routes.Commands.SendRouteToDriver;
using DeliveryPlatform.Application.Domain.Routes.Commands.BuildManyRoutes;
using Microsoft.AspNetCore.Authorization;

namespace DeliveryPlatform.Api.Controllers.Routes;


[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Dispatcher,Driver")]
public class RoutesController : ControllerBase
{
    private readonly IMediator _mediator;
    public RoutesController(IMediator mediator) => _mediator = mediator;

    [HttpPost("build")]
    public async Task<IActionResult> Build([FromBody] BuildRouteRequest req, CancellationToken ct = default)
    {
        if (req is null) return BadRequest();

        var cmd = new BuildRouteCommand(
            req.DepotNodeId,
            req.VehicleId,
            req.DeliveryIds,
            DateOnly.FromDateTime(req.Date)
        );

        var routeId = await _mediator.Send(cmd, ct);

        return Ok(new { id = routeId });
    }

    [HttpGet]
    public async Task<IActionResult> GetRoutes([FromQuery] DateOnly? from, [FromQuery] DateOnly? to, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetRoutesQuery(from, to), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRouteDetails([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetRouteDetailsQuery(id), ct);
        return Ok(result);
    }

    [HttpGet("driver/{driverId:guid}")]
    public async Task<IActionResult> GetRouteForDriver([FromRoute] Guid driverId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetRouteForDriverQuery(driverId), ct));

    [HttpPost("{id:guid}/send-to-driver")]
    public async Task<IActionResult> SendToDriver([FromRoute] Guid id, [FromBody] SendRouteToDriverRequest req, CancellationToken ct)
    {
        await _mediator.Send(new SendRouteToDriverCommand(id, req.DriverId), ct);
        return Ok();
    }

    [HttpPost("build-many")]
    public async Task<IActionResult> BuildMany(
        [FromBody] BuildManyRoutesRequest req,
        CancellationToken ct = default)
    {
        if (req is null)
            return BadRequest();

        var cmd = new BuildManyRoutesCommand(
            req.DepotNodeId,
            req.VehicleIds,
            req.DeliveryIds,
            DateOnly.FromDateTime(req.Date)
        );

        var result = await _mediator.Send(cmd, ct);

        return Ok(result);
    }
}
