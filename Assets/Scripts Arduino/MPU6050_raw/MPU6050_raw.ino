// I2C device class (I2Cdev) demonstration Arduino sketch for MPU6050 class
// 10/7/2011 by Jeff Rowberg <jeff@rowberg.net>
// Updates should (hopefully) always be available at https://github.com/jrowberg/i2cdevlib
//
// Changelog:
//      2013-05-08 - added multiple output formats
//                 - added seamless Fastwire support
//      2011-10-07 - initial release

/* ============================================
I2Cdev device library code is placed under the MIT license
Copyright (c) 2011 Jeff Rowberg

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
===============================================
*/

// I2Cdev and MPU6050 must be installed as libraries, or else the .cpp/.h files
// for both classes must be in the include path of your project
#include "I2Cdev.h"
#include "MPU6050.h"

#include <SoftwareSerial.h>


const int16_t OFFSET_GRAVEDAD = 8291;
const float FACTOR_NORMALIZ = 8191;

// Arduino Wire library is required if I2Cdev I2CDEV_ARDUINO_WIRE implementation
// is used in I2Cdev.h
#if I2CDEV_IMPLEMENTATION == I2CDEV_ARDUINO_WIRE
    #include "Wire.h"
#endif

// class default I2C address is 0x68
// specific I2C addresses may be passed as a parameter here
// AD0 low = 0x68 (default for InvenSense evaluation board)
// AD0 high = 0x69
MPU6050 accelgyro;
//MPU6050 accelgyro(0x69); // <-- use for AD0 high

int16_t ax, ay, az;
int16_t gx, gy, gz;



#define LED_PIN 13

/*comentar para usar la version sin filtro de complemento*/
#define MODO_FILTRO_COMPLEMENTO


bool blinkState = false;


//variables para calcular orientacion a partir de datos del giroscopio
long tiempo_prev, dt;
float girosc_ang_z, girosc_ang_y;
float ang_z_prev, ang_y_prev;
float accelX, accelY, accelZ; //guardan la aceleracion procesada en rango 2, -2
float factor_gravedad_x, factor_gravedad_y, factor_gravedad_z; 

#ifdef MODO_FILTRO_COMPLEMENTO
  float angulo_z_filtro_comp, angulo_y_filtro_comp;
  float incremento_giroscopio_z, incremento_giroscopio_y;
#endif


float accel_ang_y, accel_ang_z;


void setup() {
    // join I2C bus (I2Cdev library doesn't do this automatically)
    #if I2CDEV_IMPLEMENTATION == I2CDEV_ARDUINO_WIRE
        Wire.begin();
    #elif I2CDEV_IMPLEMENTATION == I2CDEV_BUILTIN_FASTWIRE
        Fastwire::setup(400, true);
    #endif

    // initialize serial communication
    // (38400 chosen because it works as well at 8MHz as it does at 16MHz, but
    // it's really up to you depending on your project)
    Serial.begin(38400);

    // initialize device
    Serial.println("Initializing I2C devices...");
    accelgyro.initialize();

    // verify connection
    Serial.println("Testing device connections...");
    Serial.println(accelgyro.testConnection() ? "MPU6050 connection successful" : "MPU6050 connection failed");

    // use the code below to change accel/gyro offset values

 
    // configure Arduino LED for
    pinMode(LED_PIN, OUTPUT);

    #ifdef MODO_FILTRO_COMPLEMENTO
    //inicializa variables
    ang_z_prev = ang_y_prev = 0;
    #endif
}

void loop() {

    Serial.flush();
    // read raw accel/gyro measurements from device
    accelgyro.getMotion6(&ax, &ay, &az, &gx, &gy, &gz);
    
    updateGiro();
    rotacionAcelerometro();
    procesarDatosAcelerometro();
    calcularFactoresGravedad();
    
        // display tab-separated accel/gyro x/y/z values

        #ifdef MODO_FILTRO_COMPLEMENTO
        filtroComplemento();
        Serial.print(angulo_y_filtro_comp); Serial.print(",");
        Serial.print(angulo_z_filtro_comp); Serial.print("|");
        Serial.print(accelX); Serial.print(",");
        Serial.print(accelY); Serial.print(",");
        Serial.println(accelZ);

        #else
          Serial.print("ANGULOS SIN FILTRO COMPEMENTO: \t")
          Serial.print(ax); Serial.print(",");
          Serial.print(accel_ang_y); Serial.print(",");
          Serial.print(accel_ang_z); Serial.print(",");
          Serial.print(girosc_ang_y); Serial.print(",");
          Serial.println(girosc_ang_z);
        //Serial.print(",");
        //Serial.println(gz);

    #endif


    // blink LED to indicate activity
    blinkState = !blinkState;
    digitalWrite(LED_PIN, blinkState);
    delay(100);
}

 /*Esta funcion calcula la rotacion en los ejes a partir de la informacion del acalerometro,
   * concretamente, de la aceleracion producida por la gravedad
   */
  void rotacionAcelerometro()
  {
    accel_ang_y=atan((az)/sqrt(pow(ay/* - OFFSET_GRAVEDAD*/,2) + pow(ax,2)))*(180.0/3.14);
    accel_ang_z=atan(ay/sqrt(pow(az,2) + pow(ax,2)))*(180.0/3.14);
  }

  void procesarDatosAcelerometro()
  {
    accelX = (ax  - (OFFSET_GRAVEDAD * factor_gravedad_x)) / FACTOR_NORMALIZ; //evitar aceleracion de gravedad
    accelY = (ay  /*- (OFFSET_GRAVEDAD * factor_gravedad_y)*/) / FACTOR_NORMALIZ;
    accelZ = (az  + (OFFSET_GRAVEDAD * factor_gravedad_z)) / FACTOR_NORMALIZ;
  }

  //esta funcion calcula el porcentaje del offset gravedad que debe ser aplicado en cada eje
  void calcularFactoresGravedad()
  {
    //factor de gravedad en eje x (a 1 cuando y 180 y 0)
    if(abs(angulo_y_filtro_comp) < 90)
    {
      factor_gravedad_x = ((90.0 - abs(angulo_y_filtro_comp)) / 80.0); //la division del final es para el signo
    }
   /* if(angulo_y_filtro_comp > 0)
      factor_gravedad_x = -factor_gravedad_x;*/
    else if(abs(angulo_y_filtro_comp) < 180)
    {
      factor_gravedad_x = (abs(angulo_y_filtro_comp) - 90) / 80 * angulo_y_filtro_comp/abs(angulo_y_filtro_comp);
    }

    if(abs(angulo_y_filtro_comp) > 5 && abs(angulo_y_filtro_comp) < 85)
      factor_gravedad_z = 1.4 - factor_gravedad_x;
    else
      factor_gravedad_z = 1.0 - factor_gravedad_x;

    if(angulo_y_filtro_comp > 0)
      factor_gravedad_z *= -0.8;
    
  }
/*CODIGO PARA USAR EN MODO FILTRO*/

#ifdef MODO_FILTRO_COMPLEMENTO
  void updateGiro()
  {
     dt = millis() - tiempo_prev;
     tiempo_prev = millis();
  
     incremento_giroscopio_z = (gz / 131)*dt / 1000.0;
     incremento_giroscopio_y = (gy / 131)*dt / 1000.0;
  }
  
  /*Esta funcion mplea un filtro de complemento para obtener unos valores mas exactos en cuanto a la orientacion del dispositivo*/
  void filtroComplemento()
  {
    /*angulo = 0.98(anguloPrevioGiroscopio + incrementoGiroscopio) + 0.02 (anguloAcelerometro)
     * los valores 0.98 y 0.2 pueden cambiarse, pero deben sumas siempre 1.0
     */
    angulo_z_filtro_comp = 0.98*(incremento_giroscopio_z + ang_z_prev) + 0.02 * accel_ang_z;
    angulo_y_filtro_comp = 0.98*(incremento_giroscopio_y + ang_y_prev) + 0.02 * accel_ang_y;

    ang_z_prev = angulo_z_filtro_comp;
    ang_y_prev = angulo_y_filtro_comp;
  }
/*FIN DEL CODIGO EN MODO FILTRO*/


#else
/*CODIGO EN MODO NO FILTRO*/
 void updateGiro()
  {
     dt = millis() - tiempo_prev;
     tiempo_prev = millis();
  
     girosc_ang_z = (gz / 131)*dt / 1000.0 + ang_z_prev;
     girosc_ang_y = (gy / 131)*dt / 1000.0 + ang_y_prev;
   
     ang_z_prev = girosc_ang_z;
     ang_y_prev = girosc_ang_y;
  }
  

#endif


