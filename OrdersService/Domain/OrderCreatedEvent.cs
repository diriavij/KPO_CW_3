namespace Domain {
    public record OrderCreatedEvent(Guid OrderId, Guid UserId, decimal Amount) : IDomainEvent;
}