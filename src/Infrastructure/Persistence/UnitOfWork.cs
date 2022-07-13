using Core.Messages.Commands;
using Core.Messages.Events;
using Core.Persistence;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class UnitOfWork: IUnitOfWork //TODO: IDisposable
    {
        private IDbConnection Connection { get; }
        private IOrderAggregateRootRepository OrderAggregateRootRepository { get; }
        private IMessageRepository MessageRepository { get; }

        public UnitOfWork(
            IDbConnection connection,
            IOrderAggregateRootRepository orderAggregateRootRepository,
            IMessageRepository messageRepository)
        {
            this.Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            this.OrderAggregateRootRepository = orderAggregateRootRepository ?? throw new ArgumentNullException(nameof(orderAggregateRootRepository));
            this.MessageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
        }

        public async Task Save(CreateOrderCommand model) // TODO: substitute for appropriate db model / primitive types
        {
            var transaction =  Connection.BeginTransaction();

            try
            {
                await OrderAggregateRootRepository.Insert(model);

                var @event = new OrderCreatedEvent() { Id = model.Id, Correlation = Guid.NewGuid() };
                await MessageRepository.Insert(@event);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Dispose();
            }
        }
    }
}
