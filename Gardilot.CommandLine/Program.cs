using Gardilot.SharedKernel;
using Gardilot.SharedKernel.Sensors;
using Gardilot.SharedKernel.Sensors.Messages;
using System;
using System.Linq;
using System.Reactive.Linq;
using LightInject;

namespace Gardilot.CommandLine
{
    public class Program
    {
        static void Main(string[] args)
        {
            var listener = DependencyInjection.ServiceFactory.GetInstance<ISensor<DhtSensorValues>>();

            var eventStream = listener.StartListening();

            var subscription = eventStream.Subscribe(v =>
            {
                Console.WriteLine($"SensorType: {v.SensorType}\tTemperature: {v.Temperature}\tHumidity: {v.Humidity}");
            });

            ShowAverageValues(eventStream);

            Console.WriteLine("ENTER to exit.");
            Console.ReadLine();

            subscription.Dispose();
            listener.StopListening();
        }

        private static void ShowAverageValues(IObservable<DhtSensorValues> eventStream)
        {
            eventStream.Buffer(TimeSpan.FromSeconds(10)).Subscribe(v =>
            {
                Console.WriteLine($"Average for count: {v.Count}\tTemperature: {v.Average(av => av.Temperature)}\tHumidity: {v.Average(av => av.Humidity)}");
            });
        }
    }
}
