using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace eventhubfamain
{
    public class httpStart
    {
        private readonly ILogger<httpStart> _logger;
        private readonly IEHService2 ehService2;

        public httpStart(ILogger<httpStart> logger, IEHService2 _ehService2)
        {
            _logger = logger;
            ehService2 = _ehService2;
        }

        [Function("httpStart")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            ehService2.StartSending();

            return new OkObjectResult("STARTED!");
        }
    }

    public class httpStop
    {
        private readonly ILogger<httpStop> _logger;
        private readonly IEHService2 ehService2;

        public httpStop(ILogger<httpStop> logger, IEHService2 _ehService2)
        {
            _logger = logger;
            ehService2 = _ehService2;
        }

        [Function("httpStop")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            ehService2.StopSending();

            return new OkObjectResult("STOPPED!");
        }
    }
}
