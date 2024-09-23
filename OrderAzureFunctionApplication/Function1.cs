using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OrderAzureFunctionApplication
{
    public class Function1
    {
        private static readonly HttpClient httpClient = new HttpClient();

        [FunctionName("OrderReceiverAzFn")]
        public static async Task Run(
            [ServiceBusTrigger("orderqueue", Connection = "ServiceBusConnection")] string myQueueItem,
            ILogger log, ExecutionContext context)
        {
            // Build configuration
            var config = new ConfigurationBuilder()
           .SetBasePath(context.FunctionDirectory) // Use FunctionDirectory instead of FunctionAppDirectory
           .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
           .Build();

            // Access the Service Bus connection string
            var serviceBusConnection = config["Values:ServiceBusConnection"];

            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            // Deserialize the order
            var order = JsonConvert.DeserializeObject<Order>(myQueueItem);

            // Trigger the .NET Core API
            var apiUrl = "https://localhost:7110/api/Order";
            var json = JsonConvert.SerializeObject(order);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(apiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                log.LogInformation("Order processed successfully.");
            }
            else
            {
                log.LogError($"Failed to process order. Status Code: {response.StatusCode}");
            }
        }

        public class Order
        {
            public string Product { get; set; }
            public int Quantity { get; set; }
            public string CustId { get; set; }
        }
    }
}
