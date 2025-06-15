using System.Text.Json;

namespace Domain.Events
{
    public record AccountCreatedEvent(
        Guid Id,
        Guid AccountId,
        DateTime OccurredAt
    ) : IDomainEvent
    {
        public string EventType => nameof(AccountCreatedEvent);
        public string Content   => JsonSerializer.Serialize(new { accountId = AccountId, occurredAt = OccurredAt });
    }
}