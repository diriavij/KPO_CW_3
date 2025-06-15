namespace Domain
{
    public record PaymentRequestedEvent(Guid OrderId, Guid UserId, decimal Amount);
    public record PaymentResult(Guid OrderId, bool Success);
}