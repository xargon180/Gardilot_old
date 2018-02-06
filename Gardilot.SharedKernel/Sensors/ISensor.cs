using System;

namespace Gardilot.SharedKernel.Sensors
{
    public interface ISensor<TMessage>
    {
        IObservable<TMessage> StartListening();
        void StopListening();
    }
}