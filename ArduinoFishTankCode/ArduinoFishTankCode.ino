#include <XBee.h>
#include <SoftwareSerial.h>
#include <math.h>
#include <Arduino.h>

#define LED 9

byte commandIn;
SoftwareSerial xbee(2, 3); // RX, TX xbee pins

long previousMillis = 0; 
long currentMillis = 100;
long interval = 500;

double Thermister(int RawADC) 
{
	double Temp;
	Temp = log(((10240000/RawADC) - 10000));
	Temp = 1 / (0.001129148 + (0.000234125 + (0.0000000876741 * Temp * Temp ))* Temp );
	Temp = Temp - 273.15;            // Convert Kelvin to Celcius
	Temp = (Temp * 9.0)/ 5.0 + 32.0; // Convert Celcius to Fahrenheit
	return Temp;
}

void setup()
{
	xbee.begin(115200);
	pinMode(LED, OUTPUT);	// declare pin 9 to be an output:
}

void loop()
{
	currentMillis = millis(); 
	if((currentMillis - previousMillis) > interval)
	{  
		previousMillis += interval; // save the last time you sent data
		xbee.println(int(Thermister(analogRead(0))));
	} 


	if (xbee.available() > 0)
	{
		//read byte from the data sent by the pc
		commandIn = xbee.read();
		analogWrite(LED, int(commandIn));
	}

}
