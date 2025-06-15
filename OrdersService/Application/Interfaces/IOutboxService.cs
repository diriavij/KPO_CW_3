using Domain;
namespace Application.Interfaces {
    public interface IOutboxService { Task AddAsync(IDomainEvent @event); }
}