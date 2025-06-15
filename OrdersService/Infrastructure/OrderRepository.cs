using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure {
    public class OrderRepository : IOrderRepository {
        private readonly ApplicationDbContext _db;
        public OrderRepository(ApplicationDbContext db) => _db = db;
        public async Task AddAsync(Order order) => _db.Orders.Add(order);
        public async Task<Order?> GetByIdAsync(Guid id) => await _db.Orders.FindAsync(id);
        public async Task<List<Order>> GetByUserAsync(Guid uid) => await _db.Orders.Where(o=>o.UserId==uid).ToListAsync();
        public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
    }
}