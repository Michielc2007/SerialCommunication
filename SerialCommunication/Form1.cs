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
            {
                try
                {
                    if (serialPortArduino.IsOpen)
                    {
                        // --- 1. GEWENSTE TEMPERATUUR (Potmeter op A0) ---

                        serialPortArduino.ReadExisting();

                        serialPortArduino.WriteLine("get a0");

                        string antwoord0 = serialPortArduino.ReadLine().TrimEnd();

                        int ruweWaarde0 = Int32.Parse(antwoord0.Substring(4));

                        // Herschalen naar het bereik 5..45 °C

                        double gewensteTemp = (ruweWaarde0 * (40.0 / 1023.0)) + 5.0;

                        labelGewensteTemp.Text = gewensteTemp.ToString("0.0") + " °C";

                        // --- 2. HUIDIGE TEMPERATUUR (LM35 op A1) ---

                        serialPortArduino.WriteLine("get a1");

                        string antwoord1 = serialPortArduino.ReadLine().TrimEnd();

                        int ruweWaarde1 = Int32.Parse(antwoord1.Substring(4));

                        // We schalen de sensorwaarde zo dat het label ALTIJD tussen 18 en 26 graden blijft

                        double weergaveTemp = (ruweWaarde1 * (8.0 / 1023.0)) + 18.0;

                        labelHuidigeTemp.Text = weergaveTemp.ToString("0.0") + " °C";

                        // --- 3. LED LOGICA ---

                        if (gewensteTemp > weergaveTemp)
                        {
                            serialPortArduino.WriteLine("set d2 1"); // Lampje AAN
                        }

                        else
                        {
                            serialPortArduino.WriteLine("set d2 0"); // Lampje UIT
                        }
                    }
                    else
                    {
                        // Als de poort niet open is, reset de UI

                        labelStatus.Text = "Status: verbinding verbroken";

                        radioButtonVerbonden.Checked = false;

                        buttonConnect.Text = "Connect";
                    }
                }
                catch (Exception exception)
                {
                    // 1. Stop de timer direct om een herhalende lus van pop-ups te voorkomen

                    timerOefening5.Stop();

                    // 2. Toon de foutmelding aan de gebruiker

                    MessageBox.Show("Verbinding verloren: " + exception.Message, "USB Fout",

                                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // 3. Reset de interface

                    labelStatus.Text = "Error: " + exception.Message;

                    try { serialPortArduino.Close(); } catch { }

                    radioButtonVerbonden.Checked = false;

                    buttonConnect.Text = "Connect";

                }

            }

        }

    }


}


