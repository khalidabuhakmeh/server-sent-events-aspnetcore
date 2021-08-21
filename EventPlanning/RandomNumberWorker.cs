using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Lib.AspNetCore.ServerSentEvents;
using Microsoft.Extensions.Hosting;

namespace EventPlanning
{
    public class RandomNumberWorker : IHostedService
    {
        private readonly IServerSentEventsService _sse;

        public RandomNumberWorker(IServerSentEventsService sse)
        {
            _sse = sse;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var clients = _sse.GetClients();
                if (clients.Any())
                {
                    Number.Value = RandomNumberGenerator.GetInt32(1, 100);
                    await _sse.SendEventAsync(
                        new ServerSentEvent {
                            Id = "number",
                            Type = "number",
                            Data = new List<string> {
                                Number.Value.ToString()
                            }
                        },
                        cancellationToken
                    );
                }
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
    
    public static class Number {
        public static int Value { get; set; } = 1;
    }
}