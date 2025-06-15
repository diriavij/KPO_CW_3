namespace Application.DTOs;

public record AccountDto(Guid Id, Guid UserId, decimal Balance, DateTime CreatedAt);
