using MediatR;
using AutoMapper;
using Application.Interfaces;
using Application.DTOs;

namespace Application.Queries.QueryHandlers {
    public class GetOrdersByUserHandler : IRequestHandler<GetOrdersByUserQuery, List<OrderDto>> {
        private readonly IOrderRepository _repo;
        private readonly IMapper _mapper;
        public GetOrdersByUserHandler(IOrderRepository repo, IMapper mapper) {
            _repo = repo; _mapper = mapper;
        }
        public async Task<List<OrderDto>> Handle(GetOrdersByUserQuery q, CancellationToken ct) {
            var orders = await _repo.GetByUserAsync(q.UserId);
            return _mapper.Map<List<OrderDto>>(orders);
        }
    }
}