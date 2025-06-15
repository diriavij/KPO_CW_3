namespace Domain.Events
{
    public interface IDomainEvent
    {
        Guid Id { get; }
        string EventType { get; }
        string Content { get; }
        DateTime OccurredAt { get; }
    }
}