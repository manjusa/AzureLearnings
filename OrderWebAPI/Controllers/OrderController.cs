using Microsoft.AspNetCore.Mvc;

namespace OrderWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        [HttpPost]
        public IActionResult ProcessOrder([FromBody] Order order)
        {
            // Process the order- Save to DB etc
            return Ok($"Order for {order.Product} processed successfully.");
        }
        public class Order
        {
            public string Product { get; set; }
            public int Quantity { get; set; }
            public string CustomerId { get; set; }
        }
    }
}
