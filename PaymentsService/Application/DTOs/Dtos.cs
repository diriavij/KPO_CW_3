namespace Application.DTOs
{
    public class CreateAccountResponse
    {
        public Guid AccountId { get; set; }
    }

    public class GetBalanceResponse
    {
        public decimal Balance { get; set; }
    }
    
    public record PaymentRequested(Guid OrderId, Guid UserId, decimal Amount);
    public record PaymentResult(Guid OrderId, bool Success, string? Reason = null);
}