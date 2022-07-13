using System;

namespace Core.Messages.Events
{
    public interface IDomainEvent
    {
        Guid Correlation { get; }
    }
}
