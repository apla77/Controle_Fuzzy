#include <AFMotor.h>
 
AF_DCMotor motor(1); //Seleciona o motor 1

const int analogInPin = A7;
int sensorValue = 0;
float potenciaMotor = 255;
float potencia = 0;
//int cont = 30;

  
void setup() { 
  Serial.begin(9600);
  motor.setSpeed(potenciaMotor); //Define a velocidade maxima
  motor.run(FORWARD); //Gira o motor sentido horario
}
  
void loop() {
   motor.setSpeed(potencia);
  sensorValue = analogRead(analogInPin);
  if (Serial.available()){
    potenciaMotor = Serial.parseInt();
    Serial.read();
    if(potenciaMotor > 0)
      potencia = 85 + (potenciaMotor * 1.7);
    else if (potenciaMotor < 0)
      potencia = 0;
  }  
   
  String x = String(sensorValue);
  Serial.print(x);
  /*Serial.print(cont++);

  if (cont == 40)
  {
    cont = 30;
  }*/
  delay(1000);
}
