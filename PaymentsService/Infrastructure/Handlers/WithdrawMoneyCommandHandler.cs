using Application.Commands;
using MediatR;
using Application.Interfaces;
using MassTransit;

namespace Infrastructure.Handlers
{
    public class WithdrawMoneyCommandHandler
        : IRequestHandler<WithdrawMoneyCommand, Unit>
    {
        private readonly IAccountRepository _repo;
        private readonly ApplicationDbContext  _db;
        private readonly IPublishEndpoint   _publish;

        public WithdrawMoneyCommandHandler(
            IAccountRepository repo,
            ApplicationDbContext  db,
            IPublishEndpoint   publish)
        {
            _repo    = repo;
            _db      = db;
            _publish = publish;
        }

        public async Task<Unit> Handle(WithdrawMoneyCommand cmd, CancellationToken ct)
        {
            var account = await _repo.GetByIdAsync(cmd.AccountId);
            if (account == null)
                throw new KeyNotFoundException($"Account {cmd.AccountId} not found.");

            account.Withdraw(cmd.Amount);
            await _repo.UpdateAsync(account);

            foreach (var e in account.Events)
                await _publish.Publish(e, ct);
            
            await _db.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}