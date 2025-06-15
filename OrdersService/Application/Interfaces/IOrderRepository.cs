using Domain;
namespace Application.Interfaces {
    public interface IOrderRepository {
        Task AddAsync(Order order);
        Task<Order?> GetByIdAsync(Guid id);
        Task<List<Order>> GetByUserAsync(Guid userId);
        Task SaveChangesAsync();
    }
}