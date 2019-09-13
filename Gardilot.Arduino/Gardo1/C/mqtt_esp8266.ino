#include <ArduinoJson.h>
#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <DHTesp.h>
#include <RCSwitch.h>

// Network settings
const char* ssid = "FRITZ!Box 4040 FF";
const char* password = "76629251087878435790";
const char* mqtt_server = "192.168.178.2";

// Pin settings
const uint8_t led_pin = D4;
const uint8_t sensor_pin = D2;
const uint8_t transmitter_pin = D7;

// Sensor variables
DHTesp dht;
TempAndHumidity sensor_values;

// Communication variables
WiFiClient espClient;
PubSubClient client;

// Message variables
long lastMsg = 0;
long waitBetweenMsgs = 5000; // 5s
const int max_msg_length = 128;
char msg[max_msg_length];

// 433 Mhz Transmitter variables
RCSwitch mySwitch = RCSwitch();

// LED Status
bool is_led_on = false;

void setup() {
  Serial.begin(115200);
  setup_built_in_led();
  setup_dht_sensor();
  setup_wifi();
  setup_pub_sub_client();
  
  mySwitch.enableTransmit(transmitter_pin);
}

void setup_wifi() {
  delay(10);
  // We start by connecting to a WiFi network
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);

  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
}

void setup_built_in_led() {
  pinMode(led_pin, OUTPUT);     // Initialize the 2 pin as an output
}

void setup_dht_sensor() {
  dht.setup(sensor_pin, DHTesp::DHT22); // data pin 4
}

void setup_pub_sub_client() {
  client.setClient(espClient);
  client.setServer(mqtt_server, 1883);
  client.setCallback(callback);
}

void reconnect() {
  // Loop until we're reconnected
  while (!client.connected()) {
    Serial.print("Attempting MQTT connection...");
    // Attempt to connect
    if (client.connect("gardo1")) {
      Serial.println("connected");
      // Once connected, publish an announcement...
      client.publish("gardo1/status", "Ready to send environmental data.");
      // ... and resubscribe
      client.subscribe("gardo1/rc-switch");
    } else {
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println(" try again in 5 seconds");
      // Wait 5 seconds before retrying
      delay(5000);
    }
  }
}

void callback(char* topic, byte* payload, unsigned int length) {
  Serial.print("Message arrived [");
  Serial.print(topic);
  Serial.print("] ");

  // Convert to string
  String strData;
  for (int i = 0; i < length; i++) {
    strData += (char)payload[i];
  }
  
  Serial.println(strData);

  // Parse payload
  StaticJsonBuffer<200> jsonBuffer;
  JsonObject& root = jsonBuffer.parseObject(strData);
  if (!root.success()) {
    Serial.println("parseObject() of mqtt payload failed");
    return;
  }

  const char* systemCode = root["systemCode"] | "00000";
  const char* unitCode = root["unitCode"] | "00000";
  const char* switchTo = root["switchTo"] | "off";

  if (strcmp(switchTo, "on") == 0)
  {
    Serial.print("Switch systemCode=");
    Serial.print(systemCode);
    Serial.print(" unitCode=");
    Serial.print(unitCode);
    Serial.println(" on");
    mySwitch.switchOn(systemCode, unitCode);
  }
  
  if (strcmp(switchTo, "off") == 0)
  {
    Serial.print("Switch systemCode=");
    Serial.print(systemCode);
    Serial.print(" unitCode=");
    Serial.print(unitCode);
    Serial.println(" off");
    mySwitch.switchOff(systemCode, unitCode);
  }
  delay(1000);
}

const char* getSensorName(DHTesp::DHT_MODEL_t model) 
{
   switch (model) 
   {
      case DHTesp::AUTO_DETECT: return "AUTO_DETECT";
      case DHTesp::DHT11: return "DHT11";
      case DHTesp::DHT22: return "DHT22";
      case DHTesp::AM2302: return "AM2302";
      case DHTesp::RHT03: return "RHT03";
   }

   return "N/A";
}

void loop() {

  if (!client.connected()) {
    reconnect();
  }
  client.loop();

  long now = millis();
  long msBettwenLastMsg = now - lastMsg;
  if (msBettwenLastMsg > dht.getMinimumSamplingPeriod() && msBettwenLastMsg > waitBetweenMsgs) {
    lastMsg = now;

    // Turn LED on or OFF
    if(is_led_on)
    {
      digitalWrite(led_pin, HIGH);  // Turn the LED off by making the voltage HIGH
      is_led_on = false;
    }
    else
    {
      digitalWrite(led_pin, LOW);   // Turn the LED on (Note that LOW is the voltage level
      is_led_on = true;
    }

    // Read sensor data
    sensor_values = dht.getTempAndHumidity();

    // Create json from data
    StaticJsonBuffer<max_msg_length> jsonBuffer;
    JsonObject& root = jsonBuffer.createObject();
    root["sensorType"] = getSensorName(dht.getModel());
    root["temperature"] = sensor_values.temperature;
    root["humidity"] = sensor_values.humidity;

    // Print json to serial
    Serial.println();
    root.prettyPrintTo(Serial);

    // Publish mqtt message with json content
    root.printTo(msg);
    client.publish("gardo1/temp_and_hum", msg);
  }
}
