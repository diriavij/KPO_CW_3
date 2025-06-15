using System.Text.Json;

namespace Domain.Events
{
    public record MoneyDepositedEvent(
        Guid Id,
        Guid AccountId,
        decimal Amount,
        DateTime OccurredAt
    ) : IDomainEvent
    {
        public string EventType => nameof(MoneyDepositedEvent);
        public string Content   => JsonSerializer.Serialize(new { accountId = AccountId, amount = Amount, occurredAt = OccurredAt });
    }
}