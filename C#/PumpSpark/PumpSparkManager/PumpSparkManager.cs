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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows;
using System.Windows.Forms;

namespace PumpSpark
{
    public class PumpSparkManager
    {
        // Create a new SerialPort object with default settings.
        static SerialPort _serialPort = new SerialPort();        

        // Configure the serial port and specify COM Port
        public void ConfigurePort(string portName)
        {            
            // Allow the user to set the appropriate portName (e.g. "COM1")
            _serialPort.PortName = portName;

            // Set appropriate properties (do not change these)            
            _serialPort.BaudRate = 9600;
            _serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), "None");
            _serialPort.DataBits = 8;
            _serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "One");
            _serialPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), "None");

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
        }

        // Open the serial port
        public void ConnectPort()
        {
            try
            {
                _serialPort.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
                return;
            }
        }

        // Close the serial port
        public void DisconnectPort()
        {
            try
            {
                _serialPort.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Exception on serial port close");
                //MessageBox.Show(ex.Message);
                return;
            }
        }

        /* 
           ActuatePump(byte pumpNumber, byte flowValue)
           Method to actuate pumps connected to the PumpSpark board
           pumpNumber => {0, 1, 2, 3, 4, 5, 6, 7}
           flowValue  => {min to max} = {0 to 254} 
        */
        public void ActuatePump(byte pumpNumber, byte flowValue)
        {
            if (_serialPort.IsOpen == false)
            {
                ConnectPort();
            }

            byte[] start = new byte[] { 255 };
            byte[] pump = new byte[] { pumpNumber };
            byte[] flow = new byte[] { flowValue };

            try
            {                
                _serialPort.Write(start, 0, 1);
                _serialPort.Write(pump, 0, 1);
                _serialPort.Write(flow, 0, 1);
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Exception on serial port write");
                //MessageBox.Show(ex.Message);
                return;
            }
        }
    }
}