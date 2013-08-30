/**

The MIT License (MIT)

PumpSpark Fountain Development Kit 

Copyright (c) [2013] [David Kim]

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

package PumpSparkConsoleDemo;

import PumpSpark.PumpSparkManager;

public class PumpSparkConsole {
	public static void main(String[] args) throws Exception {
		// Instantiate a new PumpSparkManager
		PumpSparkManager pumpSpark = new PumpSparkManager();
		
		// Configure serial port and specify COM port
		pumpSpark.configurePort("COM4");
		
		// Open the serial port
		pumpSpark.connectPort();
		
		// Looping through 10x times
		for (int i = 0; i < 10; i++) {
            // Output message to console
            System.out.println("Actuate pump 0 at flow 254");

            // Actuating pump 0 at max value of 254 for 5 seconds
            pumpSpark.actuatePump((byte)0, (byte)254);
            Thread.sleep(5000);

            // Output message to console
            System.out.println("Actuate pump 0 at flow 0");

            // Actuating pump 0 at min value of 0 for 5 seconds
            pumpSpark.actuatePump((byte)0, (byte)0);
            Thread.sleep(5000);			
		}
		
		pumpSpark.disconnectPort();
	}
}
