using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public class LoggingPublishObserver : IPublishObserver
{
    private readonly ILogger<LoggingPublishObserver> _logger;
    public LoggingPublishObserver(ILogger<LoggingPublishObserver> logger) => _logger = logger;

    public Task PrePublish<T>(PublishContext<T> context) where T : class
    {
        _logger.LogInformation("📤 [Publish] {MessageType} MessageId={MessageId}", typeof(T).Name, context.MessageId);
        return Task.CompletedTask;
    }
    public Task PostPublish<T>(PublishContext<T> context) where T : class
    {
        _logger.LogInformation("✅ [Publish] {MessageType} MessageId={MessageId}", typeof(T).Name, context.MessageId);
        return Task.CompletedTask;
    }
    public Task PublishFault<T>(PublishContext<T> context, Exception exception) where T : class
    {
        _logger.LogError(exception, "❌ [PublishFault] {MessageType} MessageId={MessageId}", typeof(T).Name, context.MessageId);
        return Task.CompletedTask;
    }
}

public class LoggingSendObserver : ISendObserver
{
    private readonly ILogger<LoggingSendObserver> _logger;
    public LoggingSendObserver(ILogger<LoggingSendObserver> logger) => _logger = logger;

    public Task PreSend<T>(SendContext<T> context) where T : class
    {
        _logger.LogInformation("➡️ [Send] {MessageType} Destination={DestinationAddress}", typeof(T).Name, context.DestinationAddress);
        return Task.CompletedTask;
    }
    public Task PostSend<T>(SendContext<T> context) where T : class
    {
        _logger.LogInformation("✅ [Send] {MessageType} Destination={DestinationAddress}", typeof(T).Name, context.DestinationAddress);
        return Task.CompletedTask;
    }
    public Task SendFault<T>(SendContext<T> context, Exception exception) where T : class
    {
        _logger.LogError(exception, "❌ [SendFault] {MessageType} Destination={DestinationAddress}", typeof(T).Name, context.DestinationAddress);
        return Task.CompletedTask;
    }
}

public class LoggingReceiveObserver : IReceiveObserver
{
    private readonly ILogger<LoggingReceiveObserver> _logger;
    public LoggingReceiveObserver(ILogger<LoggingReceiveObserver> logger) => _logger = logger;

    public Task PreReceive(ReceiveContext context)
    {
        _logger.LogInformation("🛬 [Receive] InputAddress={InputAddress}", context.InputAddress);
        return Task.CompletedTask;
    }
    public Task PostReceive(ReceiveContext context)
    {
        _logger.LogInformation("✅ [Receive] InputAddress={InputAddress}", context.InputAddress);
        return Task.CompletedTask;
    }

    public Task PostConsume<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType)
        where T : class
    {
        _logger.LogInformation(
            "✅ [Consume] MessageType={MsgType} MessageId={MsgId} Consumer={Consumer} Duration={Elapsed}ms",
            typeof(T).Name, context.MessageId, consumerType, duration.TotalMilliseconds);
        return Task.CompletedTask;
    }

    public Task ConsumeFault<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType, Exception exception)
        where T : class
    {
        _logger.LogError(
            exception,
            "❌ [ConsumeFault] MessageType={MsgType} MessageId={MsgId} Consumer={Consumer} Duration={Elapsed}ms",
            typeof(T).Name, context.MessageId, consumerType, duration.TotalMilliseconds);
        return Task.CompletedTask;
    }

    public Task ReceiveFault(ReceiveContext context, Exception exception)
    {
        _logger.LogError(exception, "❌ [ReceiveFault] InputAddress={InputAddress}", context.InputAddress);
        return Task.CompletedTask;
    }
}

public class LoggingConsumeObserver : IConsumeObserver
{
    private readonly ILogger<LoggingConsumeObserver> _logger;
    public LoggingConsumeObserver(ILogger<LoggingConsumeObserver> logger) => _logger = logger;

    public Task PreConsume<T>(ConsumeContext<T> context) where T : class
    {
        _logger.LogInformation("🛬 [Consume] {MessageType} MessageId={MessageId}", typeof(T).Name, context.MessageId);
        return Task.CompletedTask;
    }
    public Task PostConsume<T>(ConsumeContext<T> context) where T : class
    {
        _logger.LogInformation("✅ [Consume] {MessageType} MessageId={MessageId}", typeof(T).Name, context.MessageId);
        return Task.CompletedTask;
    }
    public Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
    {
        _logger.LogError(exception, "❌ [ConsumeFault] {MessageType} MessageId={MessageId}", typeof(T).Name, context.MessageId);
        return Task.CompletedTask;
    }
}
