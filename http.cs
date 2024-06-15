using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace eventhubfamain
{
    public class http
    {
        private readonly ILogger<http> _logger;
        private readonly IEHService ehService;

        public http(ILogger<http> logger, IEHService _ehService)
        {
            _logger = logger;
            ehService = _ehService;
        }

        [Function("http")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            ehService.SendEvent(15);

            return new OkObjectResult(ehService.GetStats());
        }
    }

    public class httpStart
    {
        private readonly ILogger<http> _logger;
        private readonly IEHService2 ehService2;

        public httpStart(ILogger<http> logger, IEHService2 _ehService2)
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
        private readonly ILogger<http> _logger;
        private readonly IEHService2 ehService2;

        public httpStop(ILogger<http> logger, IEHService2 _ehService2)
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
