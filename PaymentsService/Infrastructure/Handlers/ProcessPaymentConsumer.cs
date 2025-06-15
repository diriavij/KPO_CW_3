using MassTransit;
using Application.Interfaces;
using Domain;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Handlers;

public class ProcessPaymentConsumer : IConsumer<PaymentRequestedEvent>
{
    private readonly IAccountRepository _accounts;
    private readonly ApplicationDbContext _db;
    private readonly IPublishEndpoint     _publish;
    private readonly ILogger<ProcessPaymentConsumer> _logger;

    public ProcessPaymentConsumer(
        IAccountRepository accounts,
        ApplicationDbContext db,
        IPublishEndpoint publish, ILogger<ProcessPaymentConsumer> logger)
    {
        _accounts = accounts;
        _db       = db;
        _publish  = publish;
        _logger  = logger;
    }

    public async Task Consume(ConsumeContext<PaymentRequestedEvent> ctx)
    {
        _logger.LogInformation("📥 Received PaymentRequestedEvent: Order={OrderId}, User={UserId}, Amount={Amount}",
            ctx.Message.OrderId, ctx.Message.UserId, ctx.Message.Amount);
        
        var evt = ctx.Message;
        bool success; string? reason = null;

        var account = await _accounts.GetByUserIdAsync(evt.UserId);
        var paymentAmount = new Money(evt.Amount);
        if (account == null)
        {
            success = false;
            reason  = "Account not found";
        }
        else if (account.Balance < paymentAmount)
        {
            success = false;
            reason  = "Insufficient funds";
        }
        else
        {
            account.Withdraw(evt.Amount);
            await _db.SaveChangesAsync(ctx.CancellationToken);
            success = true;
            _logger.LogInformation("💳 Processing payment for Order {OrderId}...", ctx.Message.OrderId);
        }

        _logger.LogInformation("📤 Publishing PaymentResult: Order={OrderId}, Success={Success}",
            ctx.Message.OrderId, success);
        await ctx.Publish(new PaymentResult(ctx.Message.OrderId, success));
    }
}