namespace Domain {
    public record OrderCancelledEvent(Guid OrderId) : IDomainEvent;
}