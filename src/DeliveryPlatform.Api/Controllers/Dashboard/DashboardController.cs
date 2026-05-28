using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DeliveryPlatform.Application.Dashboard.Queries.GetDashboardStats;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace DeliveryPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Dispatcher")]
public sealed class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;
    public DashboardController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] DateOnly date, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDashboardStatsQuery(date), ct));
}
