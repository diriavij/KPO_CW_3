using Application;
using MediatR;
using Application.Interfaces;

namespace Infrastructure.Handlers
{
    public class GetBalanceQueryHandler : IRequestHandler<GetBalanceQuery, decimal>
    {
        private readonly IAccountRepository _repo;
        public GetBalanceQueryHandler(IAccountRepository repo) => _repo = repo;

        public async Task<decimal> Handle(GetBalanceQuery q, CancellationToken ct)
        {
            var account = await _repo.GetByUserIdAsync(q.UserId);
            return account?.Balance.Value ?? 0m;
        }
    }
}