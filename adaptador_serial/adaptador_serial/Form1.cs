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

namespace adaptador_serial
{
    public partial class Form1 : Form
    {
        private delegate void DelegadoAcceso(string accion);

        private string strBufferIn;
        private string strBufferOut;
        public Form1()
        {
            InitializeComponent();
        }

        private void AccesoForm(string accion)
        {
            strBufferIn = accion;
            TxtDatosRecibidos.Text = strBufferIn;
        }

        private void AccesoInterrupcion(string accion)
        {
            DelegadoAcceso Var_DelegadoAcceso;
            Var_DelegadoAcceso = new DelegadoAcceso(AccesoForm);
            object[] arg = { accion };
            base.Invoke(Var_DelegadoAcceso,arg);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            strBufferIn = " ";
            strBufferOut = " ";
            BtnConectar.Enabled = false;
            BtnEnviarDatos.Enabled = false;


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (BtnConectar.Text=="CONECTAR")
                {
                    SpPuertos.BaudRate = Int32.Parse(CboBaudRate.Text);
                    SpPuertos.DataBits = 8;
                    SpPuertos.Parity = Parity.None;
                    SpPuertos.StopBits = StopBits.One;
                    SpPuertos.Handshake = Handshake.None;
                    SpPuertos.PortName = CboPuertos.Text;

                    try
                    {
                        SpPuertos.Open();
                        BtnConectar.Text = "DESCONECTAR";
                        BtnEnviarDatos.Enabled = true;
                    }
                    catch(Exception exc)
                    {
                        MessageBox.Show(exc.Message.ToString());
                    }
                }
                else if (BtnConectar.Text == "DESCONECTAR")
                {
                    SpPuertos.Close();
                    BtnConectar.Text = "CONECTAR";
                    BtnEnviarDatos.Enabled = false;
                }
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message.ToString());
            }

        }

        private void LblBaudRate_Click(object sender, EventArgs e)
        {

        }

        private void BtnBuscarPuertos_Click(object sender, EventArgs e)
        {
            string[] PuertosDisponibles = SerialPort.GetPortNames();
            CboPuertos.Items.Clear();

            foreach(string puerto_simple in PuertosDisponibles)
            {
                CboPuertos.Items.Add(puerto_simple);
            }

            if (CboPuertos.Items.Count > 0)
            {
                CboPuertos.SelectedIndex = 0;
                MessageBox.Show("SELECCIONAR EL PUERTO DE TRABAJO");
                BtnConectar.Enabled = true;
            }
            else
            {
                MessageBox.Show("NINGUN PUERTO DETECTADO");
                CboPuertos.Items.Clear();
                CboPuertos.Text = "                    ";
                strBufferIn = " ";
                strBufferOut = " ";
                BtnConectar.Enabled = false;
                BtnEnviarDatos.Enabled = false;
            }

        }

        private void BtnEnviarDatos_Click(object sender, EventArgs e)
        {
            try
            {
                SpPuertos.DiscardOutBuffer();
                strBufferOut = TxtDatos_a_Enviar.Text;
                SpPuertos.Write(strBufferOut);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString());
            }
        }

        private void DatoRecibido(object sender, SerialDataReceivedEventArgs e)
        {
            AccesoInterrupcion(SpPuertos.ReadExisting());
            /*string Data_in = SpPuertos.ReadExisting();
            MessageBox.Show(Data_in);
            TxtDatosRecibidos.Text = Data_in;*/
        }
    }
}
