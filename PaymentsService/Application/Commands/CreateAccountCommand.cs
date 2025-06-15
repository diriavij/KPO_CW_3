using MediatR;

namespace Application.Commands;

public record CreateAccountCommand(Guid UserId) : IRequest<Guid>;
