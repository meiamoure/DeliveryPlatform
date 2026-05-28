using DeliveryPlatform.Core.Domain.Deliveries.Common;
using DeliveryPlatform.Core.Domain.Routes.Common;
using DeliveryPlatform.Core.Domain.Vehicles.Common;
using DeliveryPlatform.Core.Domain.Nodes.Common;
using Microsoft.Extensions.DependencyInjection;
using DeliveryPlatform.Infrastructure.Core.Domain.Nodes.Common;
using DeliveryPlatform.Infrastructure.Core.Domain.Routes.Common;
using DeliveryPlatform.Infrastructure.Core.Domain.Vehicles.Common;
using DeliveryPlatform.Infrastructure.Core.Domain.Deliveries.Common;
using DeliveryPlatform.Infrastructure.Core.Common;
using DeliveryPlatform.Core.Common;
using DeliveryPlatform.Application.Routing.Models;
using DeliveryPlatform.Application.Routing;
using DeliveryPlatform.Application.Routing.DistanceMatrixBuilder;
using System.Reflection;
using DeliveryPlatform.Application.Routing.ShortestPath;
using DeliveryPlatform.Infrastructure.Routing.ShortestPath;
using DeliveryPlatform.Infrastructure.Routing.Graph;
using DeliveryPlatform.Application.Routing.Vrp;
using DeliveryPlatform.Infrastructure.Routing.Osm;
using DeliveryPlatform.Core.Domain.Users.Common;
using DeliveryPlatform.Infrastructure.Core.Domain.Users.Common;
using DeliveryPlatform.Application.Common.Security;
using DeliveryPlatform.Infrastructure.Auth;
using DeliveryPlatform.Infrastructure.Security;
using Microsoft.Extensions.Configuration;

namespace DeliveryPlatform.Infrastructure;

public static class InfrasrtuctureRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddScoped<IDeliveryRepository, DeliveryRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IRouteRepository, RouteRepository>();
        services.AddScoped<INodeRepository, NodeRepository>();
        //      services.AddScoped<IDistanceMatrixBuilder, BuildDistanceMatrixService>();
        services.AddScoped<RouteBuilder>();
        //      services.AddScoped<IShortestPathService, DijkstraShortestPathService>();
        services.AddScoped<IRoadGraphProvider, DbRoadGraphProvider>();
        services.AddScoped<IVrpSolver, ClarkeWrightSolver>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddHttpClient<IOsmRoutingService, OsmRoutingService>(c =>
        {
            c.BaseAddress = new Uri("http://localhost:5000/");
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.Configure<JwtOptions>(
        configuration.GetSection(JwtOptions.SectionName));

        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
