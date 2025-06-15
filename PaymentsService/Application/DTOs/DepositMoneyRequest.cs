namespace Application.DTOs;

public record DepositMoneyRequest(Guid AccountId, decimal Amount);
