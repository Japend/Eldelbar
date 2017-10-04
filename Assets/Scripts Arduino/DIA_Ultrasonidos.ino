
#define DEBUG

#define PIN_ECHO_1      2
#define PIN_TRIGGER_1   4
#define PIN_BUZZER_1    3

#define PIN_ECHO_2      7
#define PIN_TRIGGER_2   8
#define PIN_BUZZER_2    6

#define PIN_ECHO_3      10
#define PIN_TRIGGER_3   11
#define PIN_BUZZER_3    9

#define PIN_ECHO_4      12
#define PIN_TRIGGER_4   13
#define PIN_BUZZER_4    5

#define NIVEL_INF_DIST_CM 3
#define NIVEL_SUP_DIST_CM 45
#define NIVEL_INF_BUZZER 0
#define NIVEL_SUP_BUZZER 255

void setup() {

  pinMode(PIN_ECHO_1, INPUT);
  pinMode(PIN_TRIGGER_1, OUTPUT);
  pinMode(PIN_BUZZER_1, OUTPUT);

  pinMode(PIN_ECHO_2, INPUT);
  pinMode(PIN_TRIGGER_2, OUTPUT);
  pinMode(PIN_BUZZER_2, OUTPUT);

  pinMode(PIN_ECHO_3, INPUT);
  pinMode(PIN_TRIGGER_3, OUTPUT);
  pinMode(PIN_BUZZER_3, OUTPUT);

  pinMode(PIN_ECHO_4, INPUT);
  pinMode(PIN_TRIGGER_4, OUTPUT);
  pinMode(PIN_BUZZER_4, OUTPUT);

  #ifdef DEBUG
    Serial.begin(38400);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }
  //Serial.println(" MODO DEBUG: ");
  #endif
}

void loop() {

  distancia_ultrasonidos(PIN_ECHO_1, PIN_TRIGGER_1, PIN_BUZZER_1);
  distancia_ultrasonidos(PIN_ECHO_2, PIN_TRIGGER_2, PIN_BUZZER_2);
  distancia_ultrasonidos(PIN_ECHO_3, PIN_TRIGGER_3, PIN_BUZZER_3);
  distancia_ultrasonidos(PIN_ECHO_4, PIN_TRIGGER_4, PIN_BUZZER_4);  
}

unsigned int distancia_ultrasonidos(unsigned char echo_pin, unsigned char trigger_pin, unsigned char buzzer_pin){

  unsigned long duration;
  unsigned int distance_cm, buzzer_value;

  digitalWrite(trigger_pin, LOW);
  delayMicroseconds(2);
  digitalWrite(trigger_pin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigger_pin, LOW);

  duration = pulseIn(echo_pin,HIGH);
  distance_cm = duration / 58 ;

  if(distance_cm > NIVEL_SUP_DIST_CM) distance_cm=NIVEL_SUP_DIST_CM ;
  
  //if(echo_pin == PIN_ECHO_3) distance_cm -= 3;
  buzzer_value=map(distance_cm, NIVEL_INF_DIST_CM, NIVEL_SUP_DIST_CM, NIVEL_SUP_BUZZER, NIVEL_INF_BUZZER);
  
  if(buzzer_value < NIVEL_INF_BUZZER) buzzer_value=0 ;

  //analogWrite(buzzer_pin, buzzer_value);

  #ifdef DEBUG
  
  //Serial.print("Distance");
/*
  if(echo_pin == PIN_ECHO_1) Serial.print(1);
  if(echo_pin == PIN_ECHO_2) Serial.print(2);
  if(echo_pin == PIN_ECHO_3) Serial.print(3);
  if(echo_pin == PIN_ECHO_4) Serial.print(4);

  */
  Serial.print(distance_cm);
  if(echo_pin == PIN_ECHO_4) Serial.println("");
  else Serial.print("_");  
  //Serial.println(buzzer_value);
  
  delay(50);
  #endif
  
  }
  
