using Azure.Storage.Blobs;
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs.Specialized;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace AzureQueueStorageDemo.AzureFunctions
{
    public class AddMessageLogRequest
    {
        private readonly BlobServiceClient _blobServiceClient;        

        public AddMessageLogRequest(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;            
        }        

        [FunctionName("AddMessageLogRequest")]
        public void Run([QueueTrigger("myqueue-items")]string myQueueItem, ILogger log)
        {            
            log.LogInformation($"Blob Service Client Connected To {_blobServiceClient.Uri}");            

            log.LogInformation("Trying to create a container client");
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient("my-message-log-container");
            containerClient.CreateIfNotExists();
            
            log.LogInformation("Trying to create a blobClient");
            AppendBlobClient appendBlobClient = containerClient.GetAppendBlobClient("message-log.txt");
            appendBlobClient.CreateIfNotExists();

            // Convert myQueueItem to a stream
            Stream contentStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(contentStream);
            streamWriter.Write(myQueueItem + Environment.NewLine);
            streamWriter.Flush();
            contentStream.Position = 0;            

            log.LogInformation("Trying to write to the blob");
            appendBlobClient.AppendBlock(contentStream);            

            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
