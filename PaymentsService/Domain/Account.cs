using Domain.Events;

namespace Domain
{
    public class Account
    {
        private readonly List<IDomainEvent> _events = new();
        public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();

        public Guid   Id         { get; private set; }
        public Guid   UserId     { get; private set; }
        public Money  Balance    { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Account(Guid userId)
        {
            Id         = Guid.NewGuid();
            UserId     = userId;
            Balance    = Money.Zero;
            CreatedAt  = DateTime.UtcNow;

            _events.Add(new AccountCreatedEvent(
                Id:         Guid.NewGuid(),
                AccountId:  Id,
                OccurredAt: CreatedAt
            ));
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be positive", nameof(amount));

            var m = new Money(amount);
            Balance = Balance + m;

            _events.Add(new MoneyDepositedEvent(
                Id:         Guid.NewGuid(),
                AccountId:  Id,
                Amount:     amount,
                OccurredAt: DateTime.UtcNow
            ));
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be positive", nameof(amount));

            var m = new Money(amount);
            if (Balance < m) throw new InvalidOperationException("Insufficient funds");

            Balance = Balance - m;

            _events.Add(new MoneyWithdrawnEvent(
                Id:         Guid.NewGuid(),
                AccountId:  Id,
                Amount:     amount,
                OccurredAt: DateTime.UtcNow
            ));
        }
    }
}