namespace Domain {
    public class Order {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Money Amount { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; private set; }

        private readonly List<IDomainEvent> _events = new();
        public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();

        private Order() { }

        public Order(Guid userId, Money amount, string description) {
            if (amount.Value <= 0) throw new InvalidOrderStateException("Amount must be positive");
            Id = Guid.NewGuid();
            UserId = userId;
            Amount = amount;
            Description = description;
            Status = OrderStatus.New;
            CreatedAt = DateTime.UtcNow;
            _events.Add(new OrderCreatedEvent(Id, UserId, Amount.Value));
        }

        public void Finish() {
            if (Status != OrderStatus.New)
                throw new InvalidOrderStateException($"Cannot finish from {Status}");
            Status = OrderStatus.Finished;
            _events.Add(new OrderFinishedEvent(Id));
        }

        public void Cancel() {
            if (Status != OrderStatus.New)
                throw new InvalidOrderStateException($"Cannot cancel from {Status}");
            Status = OrderStatus.Cancelled;
            _events.Add(new OrderCancelledEvent(Id));
        }
    }
}