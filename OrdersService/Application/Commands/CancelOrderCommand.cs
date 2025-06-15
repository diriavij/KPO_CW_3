using MediatR;
namespace Application.Commands {
    public record CancelOrderCommand(Guid OrderId) : IRequest<bool>;
}