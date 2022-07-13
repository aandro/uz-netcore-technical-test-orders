using Core.Messages.Events;
using Infrastructure.Persistence;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Publisher
{
    public class Worker : BackgroundService
    {
        private readonly IBusControl _bus;
        private readonly IMessageRepository _messageRepository;
        private readonly ILogger<Worker> _logger;

        public Worker(
            IBusControl bus,
            IMessageRepository messageRepository,
            ILogger<Worker> logger
            )
        {
            _bus = bus;
            _messageRepository = messageRepository;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //TODO: poll db once in (timeinterval)

                var messages = await _messageRepository.GetNextBatch();
                var tasks = messages.Select(m => Publish(m));

                await Task.WhenAll(tasks);
            }
        }

        private async Task Publish(IDomainEvent message)
        {
            await _bus.Publish(message);

            // at-least-once: if error occurs here, message might be delivered multiple times (at least twice)
            await _messageRepository.UpdateState(message.Correlation, isProcessed: true); 
        }
    }
}
