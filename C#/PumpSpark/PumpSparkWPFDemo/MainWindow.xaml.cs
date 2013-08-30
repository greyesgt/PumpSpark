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


using PumpSpark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PumpWPFController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String PUMPSPARK_PORT = "COM4";
        private byte PUMPSPARK_MAX_VALUE = 254;
        private byte PUMPSPARK_MIN_VALUE = 0;
        private byte PUMPSPARK_INIT_VALUE = 100;
        private int PUMPSPARK_NUM_CHANNELS = 8;        
        private PumpSparkManager pumpSpark = new PumpSparkManager();
        private List<Slider> sliders = new List<Slider>();

        public MainWindow()
        {
            InitializeComponent();
            InitializePumpSpark();
            InitializePumpSparkUI();
        }

        private void InitializePumpSpark()
        {
            // Instantiate a new PumpSparkManager
            pumpSpark = new PumpSparkManager();

            // Configure serial port and specify COM Port
            pumpSpark.ConfigurePort(PUMPSPARK_PORT);

            // Open the serial port
            pumpSpark.ConnectPort();
        }

        private void InitializePumpSparkUI()
        {
            for (byte i = 0; i < PUMPSPARK_NUM_CHANNELS + 1; i++)
            {
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Vertical;

                if (i < PUMPSPARK_NUM_CHANNELS)
                {
                    panel.Background = Brushes.LightGreen;
                }
                else
                {
                    panel.Background = Brushes.Salmon;
                }

                TextBlock title = new TextBlock();
                if (i < PUMPSPARK_NUM_CHANNELS)
                    title.Text = "Ch. " + i;
                else
                    title.Text = "MASTER";

                Button resendBtn = new Button();
                resendBtn.Content = "Resend";
                resendBtn.Tag = i;
                resendBtn.Click += resendBtn_Click;

                Button maxValBtn = new Button();
                maxValBtn.Content = "Max Val";
                maxValBtn.Tag = i;
                if (i < PUMPSPARK_NUM_CHANNELS)
                    maxValBtn.Click += maxValBtn_Click;
                else
                    maxValBtn.Click += allMaxValBtn_Click;

                TextBlock valueTBk = new TextBlock();                
                valueTBk.Text = "0";

                Slider slider = new Slider();                
                slider.Tag = i;
                slider.Height = 400;
                slider.Orientation = Orientation.Vertical;
                slider.Minimum = 0;
                slider.Maximum = PUMPSPARK_MAX_VALUE;
                slider.AutoToolTipPlacement = System.Windows.Controls.Primitives.AutoToolTipPlacement.TopLeft;
                slider.IsSnapToTickEnabled = true;
                slider.AutoToolTipPrecision = 0;
                slider.TickFrequency = 1;
                slider.TickPlacement = System.Windows.Controls.Primitives.TickPlacement.BottomRight;
                if (i < PUMPSPARK_NUM_CHANNELS)
                    slider.ValueChanged += slider_ValueChanged;
                else
                    slider.ValueChanged += masterSlider_ValueChanged;

                sliders.Add(slider);
                
                Binding binding = new Binding();
                binding.Source = slider;
                binding.Mode = BindingMode.OneWay;
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                binding.Path = new PropertyPath("Value", slider.Value);
                valueTBk.SetBinding(TextBlock.TextProperty, binding);

                Button initValBtn = new Button();
                initValBtn.Content = "Init Val";
                initValBtn.Tag = i;
                if (i < PUMPSPARK_NUM_CHANNELS)
                    initValBtn.Click += initValBtn_Click;
                else
                    initValBtn.Click += allInitValBtn_Click;

                Button minValBtn = new Button();
                minValBtn.Content = "Min Val";
                minValBtn.Tag = i;
                if (i < PUMPSPARK_NUM_CHANNELS)
                    minValBtn.Click += minValBtn_Click;
                else
                    minValBtn.Click += allMinValBtn_Click;

                panel.Children.Add(title);
                panel.Children.Add(resendBtn);
                panel.Children.Add(maxValBtn);
                panel.Children.Add(valueTBk);
                panel.Children.Add(slider);
                panel.Children.Add(initValBtn);
                panel.Children.Add(minValBtn);

                SliderStackPanel.Children.Add(panel);
            }

            for (byte i = 0; i < PUMPSPARK_NUM_CHANNELS; i++)
            {
                sliders[i].Value = PUMPSPARK_MIN_VALUE;
            }
        }
             
        void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            if (slider != null)
            {
                byte id = (byte)slider.Tag;
                byte val = (byte)e.NewValue;
                pumpSpark.ActuatePump(id, val);                
            }
        }

        void masterSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            if (slider != null)
            {
                byte val = (byte)e.NewValue;
                for (byte i = 0; i < PUMPSPARK_NUM_CHANNELS; i++)
                {
                    sliders[i].Value = val;
                }
            }
        }

        void initValBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                byte i = (byte)btn.Tag;
                sliders[i].Value = PUMPSPARK_INIT_VALUE;
            }
        }

        void minValBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                byte i = (byte)btn.Tag;
                sliders[i].Value = PUMPSPARK_MIN_VALUE;
            }
        }

        void maxValBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                byte i = (byte)btn.Tag;
                sliders[i].Value = PUMPSPARK_MAX_VALUE;
            }
        }

        void resendBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                byte id = (byte)btn.Tag;
                byte val = (byte)sliders[id].Value;
                pumpSpark.ActuatePump(id, val);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            for (byte i = 0; i < PUMPSPARK_NUM_CHANNELS; i++)
            {
                pumpSpark.ActuatePump(i, 0);
            }
            pumpSpark.DisconnectPort();
        }

        private void allMinValBtn_Click(object sender, RoutedEventArgs e)
        {
            for (byte i = 0; i < PUMPSPARK_NUM_CHANNELS; i++)
            {
                sliders[i].Value = PUMPSPARK_MIN_VALUE;
            }
        }

        private void allInitValBtn_Click(object sender, RoutedEventArgs e)
        {
            for (byte i = 0; i < PUMPSPARK_NUM_CHANNELS; i++)
            {
                sliders[i].Value = PUMPSPARK_INIT_VALUE;
            }
        }

        private void allMaxValBtn_Click(object sender, RoutedEventArgs e)
        {
            for (byte i = 0; i < PUMPSPARK_NUM_CHANNELS; i++)
            {
                sliders[i].Value = PUMPSPARK_MAX_VALUE;
            }
        }
    }
}
