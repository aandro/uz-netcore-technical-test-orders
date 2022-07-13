﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Messages.Events
{
    public class OrderCreatedEvent: INotification, IDomainEvent
    {
        public Guid Id { get; set; }

        public Guid Correlation { get; set; }

    }
}
