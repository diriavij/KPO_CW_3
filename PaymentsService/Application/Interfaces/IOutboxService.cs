using Domain.Events;

namespace Application.Interfaces
{
    public interface IOutboxService
    {
        Task SaveAsync(IDomainEvent domainEvent);
    }
}