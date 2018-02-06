using Gardilot.SharedKernel.Sensors;
using Gardilot.SharedKernel.Sensors.Messages;
using LightInject;
using System;

namespace Gardilot.SharedKernel
{
    public class DependencyInjection
    {
        private static Lazy<IServiceContainer> _container = new Lazy<IServiceContainer>(() =>
        {
            var container = new ServiceContainer();
            ConfigureContainer(container);
            return container;
        });

        public static IServiceFactory ServiceFactory
        {
            get { return _container.Value; }
        }

        private static void ConfigureContainer(IServiceContainer container)
        {
            container.Register<ISensor<DhtSensorValues>, DhtSensorListener>();
        }
    }
}
