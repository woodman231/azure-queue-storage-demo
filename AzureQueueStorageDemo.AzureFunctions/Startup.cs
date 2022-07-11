using Azure.Identity;
using Microsoft.Extensions.Azure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AzureQueueStorageDemo.Startup))]

namespace AzureQueueStorageDemo;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddAzureClients(azureClientsBuilder => {
            azureClientsBuilder.AddBlobServiceClient("DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;");

            azureClientsBuilder.UseCredential(new DefaultAzureCredential());
        });                
    }

    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {        
        base.ConfigureAppConfiguration(builder);
    }
}