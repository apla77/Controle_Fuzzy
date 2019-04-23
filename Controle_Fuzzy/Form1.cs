using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports; // para ter acesso as portas.

namespace Controle_Fuzzy
{
    public partial class Form1 : Form
    {
        String RxString;
        int motor;
        Double num = -1;

        public Form1()
        {
            InitializeComponent();
            timerCOM.Enabled = true;
            if(num >= 0)
                timerLeitura.Enabled = true;
        }

        private void atualizaListaCOMs()
        {
            int i;
            bool quantDiferente; //flag para sinalizar que a quantidade de portas mudou

            i = 0;
            quantDiferente = false;

            //se a quantidade de portas mudou
            if (comboBox1.Items.Count == SerialPort.GetPortNames().Length)
            {
                foreach (string s in SerialPort.GetPortNames())
                {
                    if (comboBox1.Items[i++].Equals(s) == false)
                    {
                        quantDiferente = true;
                    }
                }
            }
            else
            {
                quantDiferente = true;
            }

            //Se não foi detectado diferença
            if (quantDiferente == false)
            {
                return;                     //retorna
            }

            //limpa comboBox
            comboBox1.Items.Clear();

            //adiciona todas as COM diponíveis na lista
            foreach (string s in SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(s);
            }
            //seleciona a primeira posição da lista
            comboBox1.SelectedIndex = 0;
        }

        private void btConectar_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == false)
            {
                try
                {
                    serialPort1.PortName = comboBox1.Items[comboBox1.SelectedIndex].ToString();
                    serialPort1.Open();

                }
                catch
                {
                    return;

                }
                if (serialPort1.IsOpen)
                {
                    btConectar.Text = "Desconectar";
                    comboBox1.Enabled = false;

                }
            }
            else
            {
                try
                {
                    serialPort1.Close();
                    comboBox1.Enabled = true;
                    btConectar.Text = "Conectar";
                }
                catch
                {
                    return;
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort1.IsOpen == true)  // se porta aberta
                serialPort1.Close();         //fecha a porta
        }

        private void timerCOM_Tick(object sender, EventArgs e)
        {
            atualizaListaCOMs();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lbNivel_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            lbBomba.Text = hScrollBar1.Value.ToString() + " %";
            motor = (int)hScrollBar1.Value;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void lbBomba_Click(object sender, EventArgs e)
        {

        }


        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e) {

            RxString = serialPort1.ReadExisting();              //le o dado disponível na serial
            this.Invoke(new EventHandler(trataDadoRecebido));   //chama outra thread para escrever o dado no text box
        }

        private void trataDadoRecebido(object sender, EventArgs e) {
            lbNivel.Text = "";
            lbNivel.Text = RxString + " cm";
            plotarSensor();

        }

        private void plotarSensor()
        {
            num = Double.Parse(RxString);
            chrtNivel.Series[0].Points.AddY(num);
        }

        private void plotarBomba()
        {
            Double num2 = Double.Parse(RxString);
            chartMotor.Series[0].Points.AddY(num2);
        }

        private void btnEnviar_Click(object sender, EventArgs e) {
            if (serialPort1.IsOpen) {
                serialPort1.Write(lbBomba.Text);
            }
        }

        private void timerLeitura_Tick(object sender, EventArgs e) {
            plotarSensor();
            plotarBomba();
        }
    }
}
