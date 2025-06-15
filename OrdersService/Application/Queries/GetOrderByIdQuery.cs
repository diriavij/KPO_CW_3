using MediatR;
using Application.DTOs;
namespace Application.Queries {
    public record GetOrderByIdQuery(Guid OrderId) : IRequest<OrderDto?>;
}