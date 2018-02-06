namespace Gardilot.SharedKernel.Sensors.Messages
{
    public class DhtSensorValues
    {
        public string SensorType { get; set; }
        public decimal Temperature { get; set; }
        public decimal Humidity { get; set; }
    }
}
