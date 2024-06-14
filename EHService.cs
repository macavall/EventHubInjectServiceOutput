//using Azure.Messaging.EventHubs;
//using Azure.Messaging.EventHubs.Producer;
using System;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;

public class EHService : IEHService
{
    private static SemaphoreSlim _semaphore = new SemaphoreSlim(10); // Limit to 10 concurrent tasks
    private static BlockingCollection<Func<Task>> _taskQueue = new BlockingCollection<Func<Task>>();
    private int activeTasks = 0;
    //private const string eventHubName = "eventhub1test";
    //private EventHubProducerClient producerClient = new EventHubProducerClient(connectionString);
    EventHubClient client = EventHubClient.Create(new EventHubsConnectionStringBuilder(Environment.GetEnvironmentVariable("ehconnstring2")));
    private int counter = 0;

    public EHService()
    {
        // Start consumer tasks
        for (int i = 0; i < 1; i++)
        {
            Task.Run(TaskConsumer);
        }
    }

    public async Task<string> GetStats()
    {
        return $"Task completed. Queue length: {_taskQueue.Count}. Active tasks: {activeTasks}";
    }

    public async Task SendEvent(int count = 1)
    {
        if(_taskQueue.Count <= 200)
        {
            _taskQueue.Add(async () => await SendEventInternal(count));
        }
        else
        {
            Console.WriteLine("_taskQueue at the limit of 200!!!!");
        }
        
    }

    private async Task SendEventInternal(int count)
    {
        await _semaphore.WaitAsync();

        try
        {
            var eventDataList = new List<EventData>();

            for (int i = 0; i < count; i++)
            {
                string dateTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffff");
                eventDataList.Add(new EventData(Encoding.UTF8.GetBytes(dateTime)));
            }

            foreach(var myEvent in eventDataList)
            {
                await client.SendAsync(eventDataList);
            }
        }
        finally
        {
            _semaphore.Release();
            Interlocked.Decrement(ref activeTasks);
        }
    }

    private async Task TaskConsumer()
    {
        foreach (var taskFunc in _taskQueue.GetConsumingEnumerable())
        {
            await taskFunc();
        }
    }
}