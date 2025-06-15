namespace Application.DTOs {
    public record OrderDto(Guid Id, Guid UserId, decimal Amount, string Description, string Status, DateTime CreatedAt);
    public record PaymentRequested(Guid OrderId, Guid UserId, decimal Amount);
    //public record PaymentResult(Guid OrderId, bool Success, string? Reason = null);
}