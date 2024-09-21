using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration configuration = builder.Build();
var connectionString= configuration.GetConnectionString("ServiceBus");
var queueName = "orderqueue";

await SendOrdersAsync();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();


 async Task SendOrdersAsync()
{
    var client = new ServiceBusClient(connectionString);
    ServiceBusSender sender = client.CreateSender(queueName);

    for (int i = 1; i <= 5; i++)
    {
        var order = new Order
        {
            Product = $"Product-{i}",
            Quantity = i * 5,
            CustId =  $"Cust-{i}"
        };

        var message = new ServiceBusMessage(System.Text.Json.JsonSerializer.Serialize(order))
        {
            ContentType = "application/json"
        };

        await sender.SendMessageAsync(message);
        Console.WriteLine($"Sent order: {System.Text.Json.JsonSerializer.Serialize(order)}");
    }

    await sender.DisposeAsync();
    await client.DisposeAsync();
}
public class Order
{
    public string Product { get; set; }
    public int Quantity { get; set; }
    public string CustId { get; set; }
}
