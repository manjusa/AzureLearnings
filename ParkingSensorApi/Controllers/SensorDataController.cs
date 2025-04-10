using Microsoft.AspNetCore.Mvc;
using ParkingSensorApi.Models;

namespace ParkingSensorApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SensorDataController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<SensorDataController> _logger;

        public SensorDataController(ILogger<SensorDataController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult PostSensorData([FromBody] SensorData data)
        {
            if (data == null)
                return BadRequest("Invalid sensor data.");

            // TODO: Save to DB or trigger downstream logic
            Console.WriteLine($"Sensor: {data.SensorId}, Distance: {data.DistanceCm}cm, Time: {data.Timestamp}");

            return Ok(new { message = "Sensor data received successfully." });
        }
    }
}
