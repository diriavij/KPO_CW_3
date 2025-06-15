using MediatR;

namespace Application;

public record GetBalanceQuery(Guid UserId) : IRequest<decimal>;
