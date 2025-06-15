using Domain;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public class PaymentResultConsumer : IConsumer<PaymentResult>
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<PaymentResultConsumer> _logger;
    
    public PaymentResultConsumer(ApplicationDbContext db, ILogger<PaymentResultConsumer> logger)
    {
        _db     = db;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<PaymentResult> ctx)
    {
        _logger.LogInformation("📥 Received PaymentResult for Order {OrderId}: Success={Success}",
            ctx.Message.OrderId, ctx.Message.Success);
        
        var order = await _db.Orders.FindAsync(ctx.Message.OrderId);
        if (order is null)
        {
            _logger.LogWarning("⚠️ Order {OrderId} not found in DB", ctx.Message.OrderId);
            return;
        }

        order.Status = ctx.Message.Success
            ? OrderStatus.Finished
            : OrderStatus.Cancelled;

        await _db.SaveChangesAsync(ctx.CancellationToken);
        _logger.LogInformation("✅ Order {OrderId} status updated to {Status}",
            order.Id, order.Status);
    }
}