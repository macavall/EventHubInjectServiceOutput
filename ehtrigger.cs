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
        private readonly IEHService2 _ehService2;

        public ehtrigger(ILogger<ehtrigger> logger, IEHService2 ehService2)
        {
            _logger = logger;
            _ehService2 = ehService2;
        }

        [Function(nameof(ehtrigger))]
        public void Run([EventHubTrigger("eventhub1test2", Connection = "ehconnstring", IsBatched = true)] EventData[] events)
        {

            foreach (EventData @event in events)
            {
                _logger.LogInformation("Event Body: {body}", @event.Body);
                _logger.LogInformation("Event Content-Type: {contentType}", @event.ContentType);
            }

            //_ = Task.Factory.StartNew(() => {
            //    _ehService2.StartSending();
            //});
        }
    }
}
