using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _db;
        public AccountRepository(ApplicationDbContext db) => _db = db;

        public async Task<Account?> GetByIdAsync(Guid accountId)
            => await _db.Accounts
                .FirstOrDefaultAsync(a => a.Id == accountId);

        public async Task<Account?> GetByUserIdAsync(Guid userId)
            => await _db.Accounts
                .FirstOrDefaultAsync(a => a.UserId == userId);

        public async Task AddAsync(Account account)
        {
            _db.Accounts.Add(account);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Account account)
        {
            _db.Accounts.Update(account);
            await _db.SaveChangesAsync();
        }
    }
}