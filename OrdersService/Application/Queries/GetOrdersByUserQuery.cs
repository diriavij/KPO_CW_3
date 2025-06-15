using MediatR;
using Application.DTOs;
namespace Application.Queries {
    public record GetOrdersByUserQuery(Guid UserId) : IRequest<List<OrderDto>>;
}