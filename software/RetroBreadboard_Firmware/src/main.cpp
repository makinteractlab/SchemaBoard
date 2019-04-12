#include <Arduino.h>
#include "constants.hpp"
#include "Breadboard.hpp"
#include <ArduinoJson.h>
#include <MsTimer2.h>

// Declarations
void initialize();
void blink(uint16_t times, uint16_t durationMs);
void parse(const String &buffer);
void ack(const String &buffer);
void blink();

// Globals
Breadboard br(LATCH, CLOCK, DATA);
String buffer = "";

void setup()
{
  initialize();
  br.resetAll();

  // set timer 
  MsTimer2::set(500, blink);
  MsTimer2::start();
}

void loop()
{
  while (Serial.available())
  {
    char inChar = (char)Serial.read();
    if (inChar == '\n')
    {
      parse(buffer);
      buffer = "";
    }
    else
    {
      buffer += inChar;
    }
  }
}

// Helpers

void initialize()
{
  Serial.begin(115200);
  pinMode(LED, OUTPUT);
}

void blink(uint16_t times, uint16_t durationMs)
{
  digitalWrite(LED, HIGH);
  delay(durationMs);
  digitalWrite(LED, LOW);
  delay(durationMs);
}

void parse(const String &buffer)
{
  StaticJsonDocument<BUFFER_SIZE> json;
  DeserializationError error = deserializeJson(json, buffer);

  // Test if parsing succeeds.
  if (error)
  {
    ack("JSON parse fail");
    return;
  }

  // Parse JSON
  if (json["cmd"] == F("on"))
  {
    br.setOn (json["data"].as<long>());
    br.update();
    ack("OK");

  }else if (json["cmd"] == F("off"))
  {
    br.setOff (json["data"].as<long>());
    br.update();
    ack("OK");

  }else if (json["cmd"] == F("toggle"))
  {
    br.toggle (json["data"].as<long>());
    br.update();
    ack("OK");

  }else if (json["cmd"] == F("set"))
  {
    br.set (json["left"].as<long>(), json["right"].as<long>());
    br.update();
    ack("OK");

  }else
  {
    ack("Invalid command");
  }
}

void ack(const String& msg)
{
  String result= "{\"ack\"=\"" + msg + "\"}\n\r";
  Serial.println (result);
}

void blink()
{
  //Serial.println("blink");
}