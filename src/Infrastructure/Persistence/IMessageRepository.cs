using Core.Messages.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public interface IMessageRepository
    {
        Task Insert(IDomainEvent @event);
        Task UpdateState(Guid correlationId, bool isProcessed);
        Task<IEnumerable<IDomainEvent>> GetNextBatch(); 
    }
}
