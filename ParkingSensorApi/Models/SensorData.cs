namespace ParkingSensorApi.Models
{
    public class SensorData
    {
        public string SensorId { get; set; }
        public int DistanceCm { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
