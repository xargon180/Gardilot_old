using Gardilot.SharedKernel.Sensors;
using Gardilot.SharedKernel.Sensors.Messages;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Gardilot.SharedKernel
{
    public class DependencyInjection
    {
        private static Lazy<IServiceProvider> _container = new Lazy<IServiceProvider>(() =>
        {
            var collection = new ServiceCollection();
            ConfigureContainer(collection);
            return collection.BuildServiceProvider();
        });

        public static IServiceProvider ServiceFactory
        {
            get { return _container.Value; }
        }

        public static void ConfigureContainer(IServiceCollection container)
        {
            container.AddTransient<ISensor<DhtSensorValues>, DhtSensorListener>();
        }
    }
}
