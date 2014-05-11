using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace FishTankControlCenter
{
    public partial class Form1 : Form
    {

        string RxString;
        byte[] data = new byte[1];

        public Form1()
        {
            InitializeComponent();
            foreach (string serialName in SerialPort.GetPortNames())
            {
                cmbPort.Items.Add(serialName);
            }
            //Put all the Possible Baud rates into the combo box for selection.
            cmbBaud.Items.Add("1200");
            cmbBaud.Items.Add("2400");
            cmbBaud.Items.Add("4800");
            cmbBaud.Items.Add("9600");
            cmbBaud.Items.Add("19200");
            cmbBaud.Items.Add("38400");
            cmbBaud.Items.Add("57600");
            cmbBaud.Items.Add("115200");
            cmbBaud.Items.Add("230400");
            cmbBaud.SelectedIndex = 7;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            SerialPort serialport1 = new SerialPort();
            serialPort1.PortName = cmbPort.SelectedItem.ToString();
            serialPort1.BaudRate = Convert.ToInt32(cmbBaud.SelectedItem);

            serialPort1.Open();
            if (serialPort1.IsOpen)
            {
                buttonStart.Enabled = false;
                buttonStop.Enabled = true;
                textBox1.ReadOnly = false;
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                buttonStart.Enabled = true;
                buttonStop.Enabled = false;
                textBox1.ReadOnly = true;
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen) serialPort1.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            /* If the port is closed, don't try to send a character.
            if (!serialPort1.IsOpen) return;

            // If the port is Open, declare a char[] array with one element.
            char[] buff = new char[1];

            // Load element 0 with the key character.
            buff[0] = e.KeyChar;

            // Send the one character buffer.
            serialPort1.Write(buff, 0, 1);

            // Set the KeyPress event as handled so the character won't
            // display locally. If you want it to display, omit the next line.
            //e.Handled = true; */
        }

        private void DisplayText(object sender, EventArgs e)
        {
            //textBox1.AppendText(RxString);
            textBox1.Text = RxString;
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            RxString = serialPort1.ReadExisting();
            this.Invoke(new EventHandler(DisplayText));
        }

        private void buttonLED_Click(object sender, EventArgs e)
        {
            data[0] = 255; // or 0 or the value from pwm
            serialPort1.Write(data, 0, 1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            data[0] = 0; // or 255 or the value from pwm
            serialPort1.Write(data, 0, 1);
        }

        private void trackBarPWM_Scroll(object sender, EventArgs e)
        {
            byte pwm;
            pwm = Convert.ToByte(trackBarPWM.Value);
            data[0] = pwm;
            serialPort1.Write(data, 0, 1);
            textBox2.Text = Convert.ToString(pwm);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

    }
}