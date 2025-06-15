using MediatR;
using AutoMapper;
using Application.Interfaces;
using Application.DTOs;

namespace Application.Queries.QueryHandlers {
    public class GetOrderByIdHandler(IOrderRepository repo, IMapper mapper)
        : IRequestHandler<GetOrderByIdQuery, OrderDto?>
    {
        public async Task<OrderDto?> Handle(GetOrderByIdQuery q, CancellationToken ct) {
            var order = await repo.GetByIdAsync(q.OrderId);
            return order is null ? null : mapper.Map<OrderDto>(order);
        }
    }
}