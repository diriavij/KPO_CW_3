using Application.Commands;
using MediatR;
using Application.Interfaces;
using Domain;
using MassTransit;

namespace Infrastructure.Handlers
{
    public class CreateAccountCommandHandler
        : IRequestHandler<CreateAccountCommand, Guid>
    {
        private readonly IAccountRepository _repo;
        private readonly ApplicationDbContext  _db;
        private readonly IPublishEndpoint   _publish;

        public CreateAccountCommandHandler(
            IAccountRepository repo,
            ApplicationDbContext  db,
            IPublishEndpoint   publish)
        {
            _repo    = repo;
            _db      = db;
            _publish = publish;
        }

        public async Task<Guid> Handle(CreateAccountCommand cmd, CancellationToken ct)
        {
            var existing = await _repo.GetByUserIdAsync(cmd.UserId);
            if (existing != null)
                throw new InvalidOperationException("Account already exists");

            var account = new Account(cmd.UserId);
            await _repo.AddAsync(account);

            foreach (var e in account.Events)
                await _publish.Publish(e, ct);
            
            await _db.SaveChangesAsync(ct);

            return account.Id;
        }
    }
}