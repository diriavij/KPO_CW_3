using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using API;
using Application.Interfaces;
using Application.Queries.QueryHandlers;
using Infrastructure;
using MassTransit.EntityFrameworkCoreIntegration;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory_Orders")
    )
);

builder.Services.AddMassTransit(x =>
{
    x.AddEntityFrameworkOutbox<ApplicationDbContext>(o =>
    {
        o.UsePostgres();
        o.QueryDelay = TimeSpan.FromSeconds(1);
        o.UseBusOutbox();
    });
    
    x.AddConsumer<PaymentResultConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(
            builder.Configuration["RabbitMq:Host"],
            h =>
            {
                h.Username(builder.Configuration["RabbitMq:User"]);
                h.Password(builder.Configuration["RabbitMq:Pass"]);
            });
        
        cfg.ConfigureEndpoints(context);
    });
    
    x.AddConfigureEndpointsCallback((context, _, cfg) =>
    {
        cfg.UseEntityFrameworkOutbox<ApplicationDbContext>(context);
    });

});

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CreateOrderCommandHandler>();
    cfg.RegisterServicesFromAssemblyContaining<GetOrdersByUserHandler>();
});

builder.Services.AddAutoMapper(cfg =>
    cfg.AddProfile<MappingProfile>()
);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders API", Version = "v1" });
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orders API V1");
});

app.MapControllers();

app.Run();