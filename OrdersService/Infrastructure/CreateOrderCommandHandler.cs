using Application.Commands;
using Domain;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly ApplicationDbContext _db;
    private readonly IPublishEndpoint      _publish;
    private readonly ILogger<CreateOrderCommandHandler> _logger;


    public CreateOrderCommandHandler(ApplicationDbContext db, IPublishEndpoint publish, ILogger<CreateOrderCommandHandler> logger)
    {
        _db      = db;
        _publish = publish;
        _logger  = logger;
    }

    public async Task<Guid> Handle(CreateOrderCommand cmd, CancellationToken ct)
    {
        var order = new Order(cmd.UserId, new Money(cmd.Amount), cmd.Description);
        _db.Orders.Add(order);
        _logger.LogInformation("⏳ Creating new Order {OrderId} for User {UserId}, Amount={Amount}",
            order.Id, cmd.UserId, cmd.Amount);
        _logger.LogInformation("📤 Publishing PaymentRequestedEvent for Order {OrderId}", order.Id);

        await _publish.Publish(
            new PaymentRequestedEvent(
                order.Id,
                cmd.UserId,
                cmd.Amount),
            ct);        
        _logger.LogInformation("💾 Saving Order and outbox record to DB (in TX)");
        await _db.SaveChangesAsync(ct);
        _logger.LogInformation("✅ Order {OrderId} persisted; PaymentRequestedEvent enqueued", order.Id);
        return order.Id;
    }
}