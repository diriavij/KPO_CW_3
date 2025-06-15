namespace Domain {
    public record OrderFinishedEvent(Guid OrderId) : IDomainEvent;
}