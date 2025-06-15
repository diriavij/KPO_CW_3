using Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MassTransit;
using Application.Commands;
using Application.Interfaces;
using Infrastructure;
using Infrastructure.Handlers;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var startupLogger = LoggerFactory
    .Create(lb => lb.AddConsole().SetMinimumLevel(LogLevel.Information))
    .CreateLogger("Startup");
startupLogger.LogInformation("🔌 Starting Payments Service");

builder.Services.AddSingleton<IPublishObserver, LoggingPublishObserver>();
builder.Services.AddSingleton<IReceiveObserver, LoggingReceiveObserver>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Payments API", Version = "v1" });
});

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.MigrationsHistoryTable("__EFMigrationsHistory_Payments")
    )
);

builder.Services.AddMassTransit(x =>
{
    startupLogger.LogInformation("➕ Registering EF Outbox");
    x.AddEntityFrameworkOutbox<ApplicationDbContext>(o =>
    {
        o.UsePostgres();
        o.QueryDelay = TimeSpan.FromSeconds(1);
        o.UseBusOutbox();
    });

    startupLogger.LogInformation("➕ Registering consumer {Consumer}", nameof(ProcessPaymentConsumer));
    x.AddConsumer<ProcessPaymentConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Configuration["RabbitMq:Host"];
        startupLogger.LogInformation("🔌 Configuring RabbitMQ Host {Host}", host);
        cfg.Host(host!, h =>
        {
            h.Username(builder.Configuration["RabbitMq:User"]!);
            h.Password(builder.Configuration["RabbitMq:Pass"]!);
        });

        const string queueName = "payment-requested";
        startupLogger.LogInformation("⚙️ Configuring ReceiveEndpoint '{Queue}'", queueName);
        
        cfg.ConnectPublishObserver(context.GetRequiredService<IPublishObserver>());
        cfg.ConnectReceiveObserver(context.GetRequiredService<IReceiveObserver>());
        
        cfg.ReceiveEndpoint(queueName,
            e =>
            {
                e.UseEntityFrameworkOutbox<ApplicationDbContext>(context);

                e.ConfigureConsumer<ProcessPaymentConsumer>(context);
                
            }
        );
    });
});

builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateAccountCommand>());

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payments API V1");
    c.RoutePrefix = string.Empty;
});

app.MapControllers();

app.Run();
