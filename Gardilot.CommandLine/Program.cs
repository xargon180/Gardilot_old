using Gardilot.Integration;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace Gardilot.CommandLine
{
    public class Program
    {
        static void Main(string[] args)
        {
            var listener = new DhtSensorListener();


            var eventStream = listener.StartListening();

            var subscription = eventStream.Subscribe(v =>
            {
                Console.WriteLine($"SensorType: {v.SensorType}\tTemperature: {v.Temperature}\tHumidity: {v.Humidity}");
            });

            eventStream.Buffer(TimeSpan.FromSeconds(10)).Subscribe(v => 
            {
                Console.WriteLine($"Average for count: {v.Count}\tTemperature: {v.Average(av => av.Temperature)}\tHumidity: {v.Average(av => av.Humidity)}");
            });

            Console.WriteLine("ENTER to exit.");
            Console.ReadLine();



            subscription.Dispose();
            listener.StopListening();
        }
    }
}
