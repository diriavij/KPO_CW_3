using MediatR;

namespace Application.Commands;

public record WithdrawMoneyCommand(Guid AccountId, decimal Amount) : IRequest<Unit>;
