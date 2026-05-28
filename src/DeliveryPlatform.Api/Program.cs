using DeliveryPlatform.Persistence.EFCore.DeliveryPlatformDb;
using Microsoft.EntityFrameworkCore;
using DeliveryPlatform.Application;
using DeliveryPlatform.Infrastructure;
using DeliveryPlatform.Core.Domain.Deliveries.Common;
using DeliveryPlatform.Core.Domain.Routes.Common;
using DeliveryPlatform.Core.Domain.Nodes.Common;
using DeliveryPlatform.Core.Domain.Vehicles.Common;
using DeliveryPlatform.Core.Domain.Drivers.Common;
using DeliveryPlatform.Core.Common;
using DeliveryPlatform.Infrastructure.Core.Domain.Deliveries.Common;
using DeliveryPlatform.Infrastructure.Core.Domain.Nodes.Common;
using DeliveryPlatform.Infrastructure.Core.Domain.Routes.Common;
using DeliveryPlatform.Infrastructure.Core.Domain.Vehicles.Common;
using DeliveryPlatform.Infrastructure.Core.Domain.Drivers.Common;
using DeliveryPlatform.Infrastructure.Core.Common;
using DeliveryPlatform.Infrastructure.Routing.Osm;
using DeliveryPlatform.Application.Routing;
using DeliveryPlatform.Application.Routing.Models;
using DeliveryPlatform.Application.Routing.DistanceMatrixBuilder;
using DeliveryPlatform.Application.Common.Geocoding;
using DeliveryPlatform.Infrastructure.Geocoding;
using Microsoft.Extensions.FileProviders;
using FluentValidation;
using MediatR;
using Hellang.Middleware.ProblemDetails;
using System.Reflection;
using DeliveryPlatform.Application.Common;
using System.Text.Json.Serialization;
using System.Text;
using DeliveryPlatform.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Add DbContext
builder.Services.AddDbContext<DeliveryPlatformDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DeliveryPlatform"))
);

// Core App & Infrastructure
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);

// 2. Repositories and UoW
builder.Services.AddScoped<INodeRepository, NodeRepository>();
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDistanceMatrixBuilder, OsmDistanceMatrixBuilder>();
builder.Services.AddScoped<IOsmRoutingService, OsmRoutingService>();
builder.Services.AddHttpClient<IGeocodingService, NominatimGeocodingService>();

// JWT Authentication
var jwtSection = builder.Configuration.GetSection(JwtOptions.SectionName);
var jwtOptions = jwtSection.Get<JwtOptions>()
    ?? throw new InvalidOperationException("JWT settings are not configured");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpClient("osrm", client =>
{
    var baseUrl = builder.Configuration["Osrm:BaseUrl"]
        ?? throw new Exception("OSRM BaseUrl not configured");

    client.BaseAddress = new Uri(baseUrl);
});

// 3. ProblemDetails
builder.Services.AddProblemDetails(opts =>
{
    opts.IncludeExceptionDetails = (ctx, ex) => builder.Environment.IsDevelopment();
});

builder.Services.Configure<RouteOptions>(opts =>
{
    opts.LowercaseUrls = true;
    opts.LowercaseQueryStrings = true;
    opts.AppendTrailingSlash = false;
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DeliveryPlatform.Api",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введи JWT токен в формате: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

// --- Middleware chain ---

// Migration (optional)
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<DeliveryPlatformDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration error: {ex.Message}");
    }
}

// Serve static files & SPA
app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath, "uploads", "csv")
    ),
    RequestPath = "/uploads/csv"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseProblemDetails();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("Frontend");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
