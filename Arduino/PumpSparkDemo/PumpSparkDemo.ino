/**
The MIT License (MIT)

PumpSpark Fountain Development Kit 

Copyright (c) [2013] [Gabriel Reyes]

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

**/

#include <SoftwareSerial.h>
SoftwareSerial pumpSerial(4, 5); // RX, TX

// Setup function
void setup()
{
  // Setting up serial monitor for debugging
  Serial.begin(9600);

  // Setting up the Software Serial port running on digital I/O pin
  pumpSerial.begin(9600);

  // Initialize with pump 0 turned off
  actuatePump(0, 0);
}

// Loop function (required)
void loop()
{
  // Example code running through pump values from max to min
  for(byte i = 254; i > 0; i--)
  {
    actuatePump(0, i);
    delay(100);
  }

  // Actuating pump 0 at max value of 254 for 5 seconds
  actuatePump(0,254);
  delay(5000);

  // Turning pump 0 off for 5 seconds
  actuatePump(0,0);
  delay(5000);
}

/*** 
 * Function to actuate the pumps 
 * pump = pump number => {0, 1, 2, 3, 4, 5, 6, 7}
 * value = pump flow value => {min to max} = {0 to 254}  
 ***/

void actuatePump(byte pump, byte value)
{
  // Send start byte = {255} or {0xff}
  // Required every time to actuate pumps
  pumpSerial.write(255);

  // Send pump number = {0, 1, 2, 3, 4, 5, 6, 7}
  pumpSerial.write(pump);

  // Set pump value {min to max} = {0 to 254}
  pumpSerial.write(value);
}

