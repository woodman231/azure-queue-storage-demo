using Azure.Storage.Queues;

namespace AzureQueueStorageDemo.Web.Services;

public class MyQueueItemsAzureQueueStorageService : AzureQueueStorageService, IMyQueueItemsQueStorageService
{
    public MyQueueItemsAzureQueueStorageService(QueueServiceClient queueServiceClient) : base (queueServiceClient, "myqueue-items")
    {        
    }
}