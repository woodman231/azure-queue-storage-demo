using Azure.Storage.Blobs;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AzureQueueStorageDemo.Web.Services;
using AzureQueueStorageDemo.Web.Models;

namespace AzureQueueStorageDemo.Web.Controllers;

public class MessagesController : Controller
{
    private readonly IMyQueueItemsQueStorageService _myQueueItemsQueStorageService;
    private readonly BlobServiceClient _blobServiceClient;

    public MessagesController(IMyQueueItemsQueStorageService myQueueItemsQueStorageService, BlobServiceClient blobServiceClient)
    {
        _myQueueItemsQueStorageService = myQueueItemsQueStorageService;
        _blobServiceClient = blobServiceClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var results = new MessageLogViewModel();

        results.MessageLogContents = "";

        var containerClient = _blobServiceClient.GetBlobContainerClient("my-message-log-container");
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient("message-log.txt");

        if(blobClient.Exists()) {
            var blobContent = await blobClient.OpenReadAsync();

            var streamReader = new StreamReader(blobContent);

            results.MessageLogContents = await streamReader.ReadToEndAsync();            
        }        

        return View(results);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var newMessage = new MessageLogViewModel();

        return View(newMessage);
    }

    [HttpPost]
    public async Task<IActionResult> Create(MessageLogViewModel newMessage)
    {
        if(newMessage.MessageLogContents is not null) {
            await _myQueueItemsQueStorageService.SendMessageAsync(newMessage.MessageLogContents);
        }

        return RedirectToAction("Index");
    }
}