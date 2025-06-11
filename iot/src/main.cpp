#include <WiFi.h>
#include <WiFiClientSecure.h>
#include <PubSubClient.h>
#include <ArduinoJson.h>
#include <map>

#include "Preferences.h"

#define DEVICE_ID "locker-device-001"

WiFiClientSecure wifiClient;
PubSubClient mqttClient(wifiClient);

std::map<String, int> lockerPinMap;

void configureLockers(JsonDocument &doc)
{
  Serial.println("Configuring lockers...");
  for (JsonObject locker : doc["lockers"].as<JsonArray>())
  {
    String id = locker["lockerId"];
    int pin = locker["pin"];

    Serial.print("Configuring locker: ");
    Serial.print(id);
    Serial.print(" on pin: ");
    Serial.println(pin);

    lockerPinMap[id] = pin;
    pinMode(pin, OUTPUT);
    digitalWrite(pin, LOW);
  }
}

void openLocker(String id)
{
  if (!lockerPinMap.count(id))
    return;
  int pin = lockerPinMap[id];
  digitalWrite(pin, HIGH);
  delay(10000);
  digitalWrite(pin, LOW);
}

void closeLocker(String id)
{
  if (!lockerPinMap.count(id))
    return;
  int pin = lockerPinMap[id];
  digitalWrite(pin, LOW);
}

void handleMessage(char *topic, byte *payload, unsigned int length)
{
  JsonDocument doc;
  DeserializationError err = deserializeJson(doc, payload, length);
  if (err)
  {
    Serial.print("Failed to parse JSON: ");
    Serial.println(err.c_str());
    return;
  }

  String topicStr = String(topic);
  Serial.print("Received message on topic: ");
  Serial.println(topicStr);
  if (topicStr.endsWith("/configure"))
  {
    configureLockers(doc);
  }
  else if (topicStr.endsWith("/open"))
  {
    openLocker(doc["lockerId"].as<String>());
  }
  else if (topicStr.endsWith("/close"))
  {
    closeLocker(doc["lockerId"].as<String>());
  }
}

void connectToWiFi()
{
  Serial.println("Connecting to WiFi...");
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED)
    delay(500);
  Serial.println("Connected to WiFi");
}

void connectToMQTT()
{
  mqttClient.setServer(mqttServer, mqttPort);
  mqttClient.setCallback(handleMessage);
  wifiClient.setInsecure();

  while (!mqttClient.connected())
  {
    Serial.print("Connecting to MQTT...");
    if (mqttClient.connect(DEVICE_ID, mqttUser, mqttPass))
    {
      Serial.println("Connected");
      mqttClient.subscribe("locker/" DEVICE_ID "/configure");
      mqttClient.subscribe("locker/" DEVICE_ID "/open");
      mqttClient.subscribe("locker/" DEVICE_ID "/close");
    }
    else
    {
      Serial.print("Failed, rc=");
      Serial.println(mqttClient.state());
      delay(2000);
    }
  }
}

void setup()
{
  Serial.begin(115200);
  connectToWiFi();
  connectToMQTT();
}

void loop()
{
  if (!mqttClient.connected())
    connectToMQTT();
  mqttClient.loop();
}