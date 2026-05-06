using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SerialCommunication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();
                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;

                comboBoxBaudrate.SelectedIndex = comboBoxBaudrate.Items.IndexOf("115200");
            }
            catch (Exception)
            { }
        }

        private void cboPoort_DropDown(object sender, EventArgs e)
        {
            try
            {
                string selected = (string)comboBoxPoort.SelectedItem;
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();

                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);

                comboBoxPoort.SelectedIndex = comboBoxPoort.Items.IndexOf(selected);
            }
            catch (Exception)
            {
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    // ik heb een verbinding -> de gerbuiker wil deze verbinden
                    serialPortArduino.Close();
                    radioButtonVerbonden.Checked = false;
                    buttonConnect.Text = "Connect";
                    labelStatus.Text = " status: Disconnected";

                }
                else
                {
                    // ik heb geen verbinding -> de gebruiker wil een verbinding maken
                    serialPortArduino.PortName = (string)comboBoxPoort.SelectedItem;
                    serialPortArduino.BaudRate = Int32.Parse((string)comboBoxBaudrate.SelectedItem);
                    serialPortArduino.DataBits = (int)numericUpDownDatabits.Value;

                    if (radioButtonParityEven.Checked) serialPortArduino.Parity = Parity.Even;
                    else if (radioButtonParityOdd.Checked) serialPortArduino.Parity = Parity.Odd;
                    else if (radioButtonParityNone.Checked) serialPortArduino.Parity = Parity.None;
                    else if (radioButtonParityMark.Checked) serialPortArduino.Parity = Parity.Mark;
                    else if (radioButtonParitySpace.Checked) serialPortArduino.Parity = Parity.Space;

                    if (radioButtonStopbitsNone.Checked) serialPortArduino.StopBits = StopBits.None;
                    else if (radioButtonStopbitsOne.Checked) serialPortArduino.StopBits = StopBits.One;
                    else if (radioButtonStopbitsOnePointFive.Checked) serialPortArduino.StopBits = StopBits.OnePointFive;
                    else if (radioButtonStopbitsTwo.Checked) serialPortArduino.StopBits = StopBits.Two;

                    if (radioButtonHandshakeNone.Checked) serialPortArduino.Handshake = Handshake.None;
                    else if (radioButtonHandshakeRTS.Checked) serialPortArduino.Handshake = Handshake.RequestToSend;
                    else if (radioButtonHandshakeRTSXonXoff.Checked) serialPortArduino.Handshake = Handshake.RequestToSendXOnXOff;
                    else if (radioButtonHandshakeXonXoff.Checked) serialPortArduino.Handshake = Handshake.XOnXOff;

                    serialPortArduino.RtsEnable = checkBoxRtsEnable.Checked;
                    serialPortArduino.DtrEnable = checkBoxDtrEnable.Checked;

                    serialPortArduino.Open();
                    string commando = "ping";
                    serialPortArduino.WriteLine(commando);
                    string antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.Trim(); // verwijdert spaties vooraan en achteraan maar ook speciale tekens zoals \r

                    if (antwoord == "pong")
                    {
                        radioButtonVerbonden.Checked = true;
                        buttonConnect.Text = "disconnect";
                        labelStatus.Text = "status: Connected";
                    }
                    else
                    {
                        serialPortArduino.Close();
                        labelStatus.Text = "error: Verkeerd antwoord";
                    }


                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void checkBoxDigital2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando; // set d2 high/low
                    if (checkBoxDigital2.Checked) commando = "set d2 high";
                    else commando = "set d2 low";
                    serialPortArduino.WriteLine(commando);

                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void checkBoxDigital3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando; // set d3 high/low
                    if (checkBoxDigital3.Checked) commando = "set d3 high";
                    else commando = "set d3 low";
                    serialPortArduino.WriteLine(commando);

                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void checkBoxDigital4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando; // set d4 high/low
                    if (checkBoxDigital4.Checked) commando = "set d4 high";
                    else commando = "set d4 low";
                    serialPortArduino.WriteLine(commando);

                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void trackBarPWM9_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando; // set pwm9 value 0....255
                    commando = "set pwm9 " + trackBarPWM9.Value;
                    serialPortArduino.WriteLine(commando);

                }

            }
            catch (Exception exception)
            {
                labelStatus.Text = "error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void trackBarPWM10_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando; // set pwm10 value 0....255
                    commando = "set pwm10 " + trackBarPWM10.Value;
                    serialPortArduino.WriteLine(commando);
                }

            }
            catch (Exception exception)
            {
                labelStatus.Text = "error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void trackBarPWM11_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando; // set pwm11 value 0....255
                    commando = "set pwm11 " + trackBarPWM11.Value;
                    serialPortArduino.WriteLine(commando);
                }

            }
            catch (Exception exception)
            {
                labelStatus.Text = "error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            timerOefening3.Enabled = tabControl.SelectedIndex == 3;
            timerOefening4.Enabled = tabControl.SelectedIndex == 4;
            timerOefening5.Enabled = tabControl.SelectedIndex == 5;
        }

        private void timerOefening3_Tick(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    serialPortArduino.ReadExisting();
                    string commando = "get d5";
                    serialPortArduino.WriteLine(commando);
                    string antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.Trim();
                    antwoord = antwoord.Substring(4);
                    radioButtonDigital5.Checked = (antwoord == "0");

                    commando = "get d6";
                    serialPortArduino.WriteLine(commando);
                    antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.Trim();
                    antwoord = antwoord.Substring(4);
                    radioButtonDigital6.Checked = (antwoord == "0");

                    commando = "get d7";
                    serialPortArduino.WriteLine(commando);
                    antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.Trim();
                    antwoord = antwoord.Substring(4);
                    radioButtonDigital7.Checked = (antwoord == "0");

                }

            }
            catch (Exception exception)
            {
                labelStatus.Text = "error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }


        private void timerOefening4_Tick(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    serialPortArduino.ReadExisting();
                    string commando = "get a0";
                    serialPortArduino.WriteLine(commando);
                    string antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.Trim();
                    antwoord = antwoord.Substring(4);
                    int value = Int32.Parse(antwoord);
                    labelAnalog0.Text = value.ToString();
                }

            }
            catch (Exception exception)
            {
                labelStatus.Text = "error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }


        private void timerOefening5_Tick(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    serialPortArduino.ReadExisting();
                    serialPortArduino.WriteLine("get a0");
                    string antwoord = serialPortArduino.ReadLine().Trim();
                    string[] delen = antwoord.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string token = delen.Length > 0 ? delen[delen.Length - 1] : antwoord;

                    if (!int.TryParse(token, out int rawA0))
                    {
                        labelStatus.Text = "error: ongeldig antwoord a0";
                        return;
                    }

                    // herschaal 0..10235 -> 5..45 °C
                    double slope = (45.0 - 5.0) / 1023.0; // richtingscoëfficiënt
                    double offset = 5.0;
                    double desired = slope * rawA0 + offset;
                    labelGewensteTemp.Text = Math.Round(desired, 1).ToString("0.0") + " °C";

                    // read analog 1 and rescale 0..1023 -> 0..500 °C
                    serialPortArduino.WriteLine("get a1");
                    string antwoordA1 = serialPortArduino.ReadLine().Trim();
                    string[] delenA1 = antwoordA1.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string tokenA1 = delenA1.Length > 0 ? delenA1[delenA1.Length - 1] : antwoordA1;

                    if (!int.TryParse(tokenA1, out int rawA1))
                    {
                        labelStatus.Text = "error: ongeldig antwoord a1";
                        return;
                    }

                    double slopeA1 = (26.0 - 18.0) / 1023.0; // richtingscoëfficiënt
                    double offsetA1 = 18.0; // minimum temperatuur
                    double current = slopeA1 * rawA1 + offsetA1;
                    labelHuidigeTemp.Text = Math.Round(current, 1).ToString("0.0") + " °C";

                    // read analog 2 as well and compare with a1
                    serialPortArduino.WriteLine("get a2");
                    string antwoordA2 = serialPortArduino.ReadLine().Trim();
                    string[] delenA2 = antwoordA2.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string tokenA2 = delenA2.Length > 0 ? delenA2[delenA2.Length - 1] : antwoordA2;

                    if (!int.TryParse(tokenA2, out int rawA2))
                    {
                        labelStatus.Text = "error: ongeldig antwoord a2";
                        return;
                    }

                    // bepaal LED-status: HIGH wanneer a1 == a2, anders HIGH wanneer current < desired
                    bool ledOn = (rawA1 == rawA2) || (current < desired);
                    try
                    {
                        checkBoxDigital2.CheckedChanged -= checkBoxDigital2_CheckedChanged;
                        checkBoxDigital2.Checked = ledOn;
                        checkBoxDigital2.CheckedChanged += checkBoxDigital2_CheckedChanged;

                        if (ledOn) serialPortArduino.WriteLine("set d2 high");
                        else serialPortArduino.WriteLine("set d2 low");
                    }
                    catch { }
                   

                }

            }
            catch (Exception exception)
            {
                labelStatus.Text = "error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }


    }
}

