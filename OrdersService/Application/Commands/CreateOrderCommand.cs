using MediatR;
namespace Application.Commands {
    public record CreateOrderCommand(Guid UserId, decimal Amount, string Description) : IRequest<Guid>;
}