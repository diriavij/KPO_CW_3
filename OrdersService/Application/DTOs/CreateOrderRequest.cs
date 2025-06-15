namespace Application.DTOs {
    public record CreateOrderRequest(Guid UserId, decimal Amount, string Description);
}