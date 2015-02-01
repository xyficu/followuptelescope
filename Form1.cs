using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;

namespace Follow_Up_Telescope
{
    public partial class FormFUTMain : Form
    {
        private FutTcpServer fut;
        private DeviceStatus deviceStatus;
        private Thread futServerThread;

        public FormFUTMain()
        {
            InitializeComponent();

            //initial data
            deviceStatus = new DeviceStatus();
            fut = new FutTcpServer(deviceStatus);

            //start tcp server
            futServerThread = new Thread(new ThreadStart(StartFutServer));
            futServerThread.IsBackground = true;
            futServerThread.Start();

            //enable timer to update status per 100ms
            timerUpdateStatus.Enabled = true;
        }


        private void FormFUTMain_Load(object sender, EventArgs e)
        {

        }

        public void StartFutServer()
        {
            fut.StartServer();
        }


        private void UpdateWheelStatus()
        {
            if (fut != null && fut.mDeviceConnections.ContainsKey("WHEEL"))
            {
                labelWMovStat.ForeColor = Color.Green;
                labelWMovStat.Text = "Connected！";
            }
            else 
            {
                labelWMovStat.ForeColor = Color.Red;
                labelWMovStat.Text = "Not connected.";
            }


            labelWCurPos.Text = deviceStatus.wheelStatus.curPos.ToString();
            if (deviceStatus.wheelStatus.movStatus == 1)
            {
                labelWConStatus.ForeColor = Color.Red;
                labelWConStatus.Text = "Moving...";
            }
            else
            {
                labelWConStatus.ForeColor = Color.Black;
                labelWConStatus.Text = "Stopped.";
            }
            
        }

        private void UpdateCcdStatus()
        {

        }

        private void timerUpdateStatus_Tick(object sender, EventArgs e)
        {
            UpdateWheelStatus();
            UpdateCcdStatus();
        }


    }
}
