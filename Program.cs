using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        var host = new HostBuilder()
        .ConfigureFunctionsWebApplication()
        .ConfigureServices(services =>
        {
            services.AddApplicationInsightsTelemetryWorkerService();
            services.ConfigureFunctionsApplicationInsights();
            services.AddSingleton<IEHService, EHService>();
        })
        .Build();

        host.Run();
    }
}
