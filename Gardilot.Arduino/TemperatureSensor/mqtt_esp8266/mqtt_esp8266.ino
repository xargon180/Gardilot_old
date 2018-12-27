#include <ArduinoJson.h>
#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <DHTesp.h>

// Network settings
const char* ssid = "FRITZ!Box 4040 FF";
const char* password = "76629251087878435790";
const char* mqtt_server = "192.168.178.2";

// Pin settings
// uint8_t LED_Pin = D4;
const int led_pin = 2;
const int sensor_pin = 4;

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

// LED Status
bool is_led_on = false;

void setup() {
  Serial.begin(115200);
  setup_built_in_led();
  setup_dht_sensor();
  setup_wifi();
  setup_pub_sub_client();
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
    } else {
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println(" try again in 5 seconds");
      // Wait 5 seconds before retrying
      delay(5000);
    }
  }
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
