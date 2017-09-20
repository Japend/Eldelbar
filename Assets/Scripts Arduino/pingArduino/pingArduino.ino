#include <SoftwareSerial.h>
#include <SerialCommand.h>
SerialCommand sCmd;

void setup() {
  Serial.begin(9600);
  while (!Serial);

  sCmd.addCommand("PING", pingHandler);
}

void loop () {
  if (Serial.available() > 0)
    sCmd.readSerial();
}


void pingHandler (const char *command) {
  Serial.println("PONG"); //Enviar datos de Arduino a Unity
}


void echoHandler () {
  char *arg;
  arg = sCmd.next();
  if (arg != NULL)
    Serial.println(arg);
  else
    Serial.println("nothing to echo");
}
