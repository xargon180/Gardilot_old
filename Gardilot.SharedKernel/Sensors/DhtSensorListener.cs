using Gardilot.SharedKernel.Sensors.Messages;
using Newtonsoft.Json;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Gardilot.SharedKernel.Sensors
{
    public class DhtSensorListener : ISensor<DhtSensorValues>
    {
        private MqttClient _client;
        private string _clientId;

        public DhtSensorListener()
        {
            _client = new MqttClient("gruenzeug-server");
            _clientId = Guid.NewGuid().ToString();
        }

        public IObservable<DhtSensorValues> StartListening()
        {
            _client.Connect(_clientId);

            // subscribe to the topic "/home/temperature" with QoS 2 
            _client.Subscribe(new string[] { "garden/house/kitchen/dht" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });

            return Observable.FromEventPattern<MqttMsgPublishEventArgs>(_client, nameof(_client.MqttMsgPublishReceived), Scheduler.Default).Select(ea =>
            {
                return JsonConvert.DeserializeObject<DhtSensorValues>(Encoding.UTF8.GetString(ea.EventArgs.Message));
            });
        }


        public void StopListening()
        {
            _client.Disconnect();
        }
    }
}
