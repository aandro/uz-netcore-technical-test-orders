using Core.Messages.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Models
{
    public class OrderAggregateRoot
    {
        public Guid Id { get; set; }

        public IDictionary<string, decimal> Items { get; set; }

        public List<CreateOrderCommand> Events { get; set; }

        public OrderAggregateRoot(CreateOrderCommand command)
        {
            this.Events = new List<CreateOrderCommand>();

            this.Apply(command);
        }

        private void Apply(CreateOrderCommand command)
        {
            this.Events.Add(command);

            this.Id = command.Id;
            this.Items = command.Items;

            // TODO: with Event Sourcing we can emit Domain Event here;
            // then use 3rd party package to handle storing events and propagating changes to ReadModels (for read store part of CQRS)
        }
    }
}
