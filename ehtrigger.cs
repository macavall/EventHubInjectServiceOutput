using System;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace eventhubfamain
{
    public class ehtrigger
    {
        private readonly ILogger<ehtrigger> _logger;
        private readonly IEHService _ehService;

        public ehtrigger(ILogger<ehtrigger> logger, IEHService ehService)
        {
            _logger = logger;
            _ehService = ehService;
        }

        [Function(nameof(ehtrigger))]
        public void Run([EventHubTrigger("eventhub1test2", Connection = "ehconnstring", IsBatched = true)] EventData[] events)
        {

            foreach (EventData @event in events)
            {
                _logger.LogInformation("Event Body: {body}", @event.Body);
                _logger.LogInformation("Event Content-Type: {contentType}", @event.ContentType);
            }

            _ = Task.Factory.StartNew(() => {
                _ehService.SendEvent(events.Length * 2);
            });
        }
    }
}
