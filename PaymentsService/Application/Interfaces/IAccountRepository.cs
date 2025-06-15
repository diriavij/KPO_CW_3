using Domain;

namespace Application.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(Guid accountId);
        Task<Account?> GetByUserIdAsync(Guid userId);
        Task AddAsync(Account account);
        Task UpdateAsync(Account account);
    }
}