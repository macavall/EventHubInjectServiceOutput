using Azure.Messaging.EventHubs.Producer;
using Microsoft.Azure.EventHubs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventDataBatch2 = Azure.Messaging.EventHubs.Producer.EventDataBatch;
using EventData2 = Azure.Messaging.EventHubs.EventData;

public class EHService2 : IEHService2
{
    EventHubClient client = EventHubClient.Create(new EventHubsConnectionStringBuilder(Environment.GetEnvironmentVariable("ehconnstring2")));
    private bool _go = false;

    public EHService2()
    {
        _ = SendEvents();
    }

    private async Task SendEvents()
    {
        while(true)
        {
            if(_go)
            {
                try
                {
                    var eventDataList = new List<EventData>();

                    for (int i = 0; i < 100; i++)
                    {
                        string dateTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffff");
                        eventDataList.Add(new EventData(Encoding.UTF8.GetBytes(dateTime)));
                    }

                    foreach (var myEvent in eventDataList)
                    {
                        await client.SendAsync(eventDataList);
                    }
                }
                finally
                {
                }
            }
            else
            {
                await Task.Delay(1000);
            }
        }
    }

    public async Task StartSending()
    {

        _go = true;

        _ = Task.Factory.StartNew(async () =>
        {
            Console.WriteLine("Background task started.");
            await Task.Delay(TimeSpan.FromSeconds(600));
            _go = false;
            Console.WriteLine("Background task completed after 10 minutes.");
        });
    }

    public async Task StopSending()
    {
        _go = false;
    }
}

class Program2
{
    private const string connectionString = "Endpoint=sb://eventhub566ns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=your_access_key";
    private const string eventHubName = "myeventhub";

    static async Task TestMethod()
    {
        // Create a producer client that you can use to send events to an event hub
        await using (var producerClient = new EventHubProducerClient(connectionString, eventHubName))
        {
            // Create a batch of events 
            using EventDataBatch2 eventBatch = await producerClient.CreateBatchAsync();

            // Add events to the batch
            eventBatch.TryAdd(new EventData2(Encoding.UTF8.GetBytes("First event")));
            eventBatch.TryAdd(new EventData2(Encoding.UTF8.GetBytes("Second event")));
            eventBatch.TryAdd(new EventData2(Encoding.UTF8.GetBytes("Third event")));

            // Send the batch of events to the event hub
            await producerClient.SendAsync(eventBatch);

            Console.WriteLine("A batch of 3 events has been published.");
        }
    }
}