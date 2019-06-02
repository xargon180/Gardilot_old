function onInit() {
  //Clear
  clearInterval();
  clearWatch();

  // MQTT settings
  var MQTT_SERVER_ADDRESS = "192.168.178.2";
  var MQTT_TEMPERATURE_TOPIC = "gardo1/temperature";
  var MQTT_HUMIDITY_TOPIC = "gardo1/humidity";
  var MQTT_SWITCH_ON_TOPIC = "gardo1/switchOn";
  var MQTT_SWITCH_OFF_TOPIC = "gardo1/switchOff";

  // RcSwitch settings
  var RCSWITCH_PROTOCOL = 1;
  var RCSWITCH_PIN = D13;
  var RCSWITCH_REPEAT = 10;

  // DHT22 settings
  var DHT22_PIN = D4;

  // RcSwitch
  var sw = require("RcSwitch").connect(RCSWITCH_PROTOCOL, RCSWITCH_PIN, RCSWITCH_REPEAT);

  // MQTT
  var mqtt = require("tinyMQTT").create(MQTT_SERVER_ADDRESS);
  mqtt.connect();

  mqtt.on("disconnected", () => {
    console.log("disconnected");
    mqtt.connect();
  });

  mqtt.on("connected", () => {
      console.log("connected");
      mqtt.subscribe(MQTT_SWITCH_ON_TOPIC);
      mqtt.subscribe(MQTT_SWITCH_OFF_TOPIC);
  });

  mqtt.on("message", msg => {
    console.log(msg.topic);
    console.log(msg.message);

    // {"group":"11110", "device":"10000"}
    var deviceId = JSON.parse(msg.message);
    if (msg.topic === MQTT_SWITCH_ON_TOPIC) {
      sw.switchOn(deviceId.group, deviceId.device);
    }

    if (msg.topic === MQTT_SWITCH_OFF_TOPIC) {
      sw.switchOff(deviceId.group, deviceId.device);
    }
  });

  // Temperature and humidity.
  var dht = require("DHT22").connect(DHT22_PIN);
  var i = setInterval(() => {
    dht.read(a => {
      print("Temp is "+a.temp.toString()+" and RH is "+a.rh.toString());
      mqtt.publish(MQTT_TEMPERATURE_TOPIC, a.temp.toString());
      mqtt.publish(MQTT_HUMIDITY_TOPIC, a.rh.toString());
    });
  }, 5000);
}
