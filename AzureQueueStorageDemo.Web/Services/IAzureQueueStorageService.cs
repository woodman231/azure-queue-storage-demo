namespace AzureQueueStorageDemo.Web.Services;

public interface IAzureQueueStorageService
{
    public Task SendMessageAsync(string message);
}