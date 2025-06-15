namespace Application.DTOs;

public record WithdrawMoneyRequest(Guid AccountId, decimal Amount);
