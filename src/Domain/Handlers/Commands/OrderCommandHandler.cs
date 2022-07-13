using Core.Messages.Commands;
using Core.Persistence;
using Light.GuardClauses;
using MassTransit;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Handlers.Commands
{
    public class OrderCommandHandler : IRequestHandler<CreateOrderCommand>
    {
        private IBus BusClient { get; }
        private IUnitOfWork UnitOfWork { get; }

        public OrderCommandHandler(
            IBus bus,
            IUnitOfWork unitOfWork)
        {
            this.BusClient = bus.MustNotBeDefault(nameof(bus));
            this.UnitOfWork = unitOfWork.MustNotBeDefault(nameof(unitOfWork));
        }

        public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // TODO: atomically instantiate a new OrderAggregateRoot, persist it and send OrderCreatedEvent

            await UnitOfWork.Save(request);

            return new Unit();
        }
    }
}
