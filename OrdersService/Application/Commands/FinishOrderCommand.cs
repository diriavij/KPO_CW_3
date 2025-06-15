using MediatR;
namespace Application.Commands {
    public record FinishOrderCommand(Guid OrderId) : IRequest<bool>;
}