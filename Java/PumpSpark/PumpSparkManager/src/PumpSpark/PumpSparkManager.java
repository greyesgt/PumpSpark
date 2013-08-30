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

package PumpSpark;

/* 
 * For Windows: import gnu.io.*
 * Download and install RxTx for serial communication: http://users.frii.com/jarvi/rxtx
 * 64-bit binaries can be found here: http://mfizz.com/oss/rxtx-for-java
 * For other platforms this can be used instead: import javax.comm.*;
 */
import gnu.io.CommPort;
import gnu.io.CommPortIdentifier;
import gnu.io.SerialPort;

import java.io.InputStream;
import java.io.OutputStream;

public class PumpSparkManager {
	static SerialPort _serialPort;
	static InputStream _serialInput;
	static OutputStream _serialOutput;
	static String _portName;

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

	/***
	 * Configure the serial port and specify COM Port
	 * 
	 * @param portName
	 *            Allow the user to set the appropriate portName (e.g. "COM1")
	 * @throws Exception
	 */
	public void configurePort(String portName) {
		_portName = portName;
	}

	public void connectPort() {
		try {
			CommPortIdentifier portIdentifier = CommPortIdentifier.getPortIdentifier(_portName);
			if (portIdentifier.isCurrentlyOwned()) {
				System.out.println("Error: Port is currently in use");
			} else {
				CommPort commPort = portIdentifier.open(this.getClass().getName(), 2000);
				
				if (commPort instanceof SerialPort) {
					_serialPort = (SerialPort) commPort;

					// Set appropriate properties (do not change these)
					_serialPort.setSerialPortParams(
							9600, 
							SerialPort.DATABITS_8,
							SerialPort.STOPBITS_1, 
							SerialPort.PARITY_NONE);

					_serialInput = _serialPort.getInputStream();
					_serialOutput = _serialPort.getOutputStream();
					System.out.println("Connected to port: " + _portName);
				} else {
					System.out.println("Error: Only serial ports are handled by this example.");
				}
			}
		} catch (Exception ex) {
			ex.printStackTrace();
		}		
	}

	/***
	 * Close the serial port
	 */
	public void disconnectPort() {
		try {
			System.out.println("Disconnecting from port: " + _portName);
			_serialInput.close();
			_serialOutput.close();
			_serialPort.close();
			
		} catch (Exception ex){
			ex.printStackTrace();
		}
	}

	/***
	 * Method to actuate pumps connected to the PumpSpark board
	 * 
	 * @param pumpNumber
	 *            => {0, 1, 2, 3, 4, 5, 6, 7}
	 * @param flowValue
	 *            => {min to max} = {0 to 254}
	 */
	public void actuatePump(byte pumpNumber, byte flowValue) {
		byte[] start = { (byte) 255 };
		byte[] pump = { pumpNumber };
		byte[] flow = { flowValue };

		try {
			_serialOutput.write(start, 0, 1);
			_serialOutput.write(pump, 0, 1);
			_serialOutput.write(flow, 0, 1);
		} catch (Exception ex) {
			return;
		}
	}
}
