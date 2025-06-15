using MediatR;

namespace Application.Commands;

public record DepositMoneyCommand(Guid AccountId, decimal Amount) : IRequest<Unit>;
