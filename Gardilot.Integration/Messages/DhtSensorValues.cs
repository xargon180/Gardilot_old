using System;
using System.Collections.Generic;
using System.Text;

namespace Gardilot.Integration
{
    public class DhtSensorValues
    {
        public string SensorType { get; set; }
        public decimal Temperature { get; set; }
        public decimal Humidity { get; set; }
    }
}
