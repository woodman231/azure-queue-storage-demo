using Azure.Identity;
using Azure.Storage.Queues;
using AzureQueueStorageDemo.Web.Services;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Register Azure Clients
builder.Services.AddAzureClients(azureClientsBuilder => {
    azureClientsBuilder.AddQueueServiceClient(builder.Configuration.GetConnectionString("AzureStorage")).ConfigureOptions(queueOptions => {
        queueOptions.MessageEncoding = QueueMessageEncoding.Base64;        
    });

    azureClientsBuilder.AddBlobServiceClient(builder.Configuration.GetConnectionString("AzureStorage"));    
});

// Register Services
builder.Services.AddTransient<IMyQueueItemsQueStorageService, MyQueueItemsAzureQueueStorageService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
