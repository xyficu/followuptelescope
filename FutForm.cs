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
        // 自定义望远镜状态字
        /** 望远镜状态字
         * 0  : 初始状态, 断开连接
         * 1  : 静止
         * 2  : 搜索零点中
         * 3  : 搜索零点成功
         * 4  : 指向中
         * 5  : 跟踪中
         * 6  : 轴控制中
         * 7  : 复位中
         * 8  : 复位
         **/
        private const int TS_NONE = 0;
        private const int TS_Stopped = 1;
        private const int TS_Homing = 2;
        private const int TS_Homed = 3;
        private const int TS_Slewing = 4;
        private const int TS_Tracking = 5;
        private const int TS_Moving = 6;
        private const int TS_Parking = 7;
        private const int TS_Parked = 8;

        private FutTcpServer fut;
        private DeviceParams deviceParams;
        private Thread futServerThread;
        private StartObs frmStartObs;

        public FormFUTMain()
        {
            InitializeComponent();

            //initial data
            deviceParams = new DeviceParams();
            fut = new FutTcpServer(deviceParams);

            //start tcp server
            futServerThread = new Thread(new ThreadStart(StartFutServer));
            futServerThread.IsBackground = true;
            futServerThread.Start();

            //enable timer to update status per 100ms
            timerUpdateStatus.Enabled = true;

            //初始化观测界面窗口
            frmStartObs = new StartObs(fut.mDeviceConnections, deviceParams);
        }


        private void FormFUTMain_Load(object sender, EventArgs e)
        {
            groupBoxCCD.ForeColor = Color.Blue;
            groupBoxFocuser.ForeColor = Color.Blue;
            groupBoxMount.ForeColor = Color.Blue;
            groupBoxWheel.ForeColor = Color.Blue;
        }

        public void StartFutServer()
        {
            fut.StartServer();
        }


        private void UpdateWheelStatus()
        {
            if (fut != null && fut.mDeviceConnections.ContainsKey("WHEEL"))
            {
                labelWConStatus.ForeColor = Color.Green;
                labelWConStatus.Text = "connected";
            }
            else 
            {
                labelWConStatus.ForeColor = Color.Red;
                labelWConStatus.Text = "not connected";
            }


            labelWCurPos.Text = (deviceParams.wheelParams.curPos + 1).ToString();
            switch (deviceParams.wheelParams.curPos)
            {
                case 0:
                    labelWCurColor.Text = "U";
                    break;
                case 1:
                    labelWCurColor.Text = "B";
                    break;
                case 2:
                    labelWCurColor.Text = "V";
                    break;
                case 3:
                    labelWCurColor.Text = "R";
                    break;
                case 4:
                    labelWCurColor.Text = "I";
                    break;
                case 5:
                    labelWCurColor.Text = "L";
                    break;
                case 6:
                    labelWCurColor.Text = "Red";
                    break;
                case 7:
                    labelWCurColor.Text = "Green";
                    break;
                case 8:
                    labelWCurColor.Text = "Blue";
                    break;
                case 9:
                    labelWCurColor.Text = "empty";
                    break;
                case 10:
                    labelWCurColor.Text = "empty";
                    break;
                case 11:
                    labelWCurColor.Text = "empty";
                    break;
                default:
                    labelWCurColor.Text = "unknown";
                    break;
            }
            if (deviceParams.wheelParams.movStatus == 1)
            {
                labelWMovStat.ForeColor = Color.Red;
                labelWMovStat.Text = "moving...";
            }
            else
            {
                labelWMovStat.ForeColor = Color.Black;
                labelWMovStat.Text = "stopped";
            }
            
            
        }

        private void UpdateCcdStatus()
        {
            if (fut != null && fut.mDeviceConnections.ContainsKey("CCD"))
            {
                labelCConStat.ForeColor = Color.Green;
                labelCConStat.Text = "connected";
            }
            else
            {
                labelCConStat.ForeColor = Color.Red;
                labelCConStat.Text = "not connected";
            }
            labelCImgSavPath.Text = deviceParams.ccdParams.imgPath;
            labelCAmt.Text = deviceParams.ccdParams.amount.ToString();
            if (deviceParams.ccdParams.acqStat == 1)
            {
                labelCAcqStat.ForeColor = Color.Red;
                labelCAcqStat.Text = "acquiring...";
            }
            else
            {
                labelCAcqStat.ForeColor = Color.Black;
                labelCAcqStat.Text = "stopped";
            }
            labelCCurTemp.Text = deviceParams.ccdParams.temp.ToString();
            labelCCoolerStat.Text = deviceParams.ccdParams.coolerSwitch == true ? "ON" : "OFF";
            labelCCurNo.Text = deviceParams.ccdParams.curNumb.ToString();
            labelCAmt.Text = deviceParams.ccdParams.imgAmt.ToString();
            progressBarCAcq.Value = (int)deviceParams.ccdParams.acqProc > 100 ? 100 : (int)deviceParams.ccdParams.acqProc;
        }

        private void UpdateMountStatus()
        {
            if (fut != null && fut.mDeviceConnections.ContainsKey("MOUNT"))
            {
                labelMConStat.ForeColor = Color.Green;
                labelMConStat.Text = "connected";
            }
            else
            {
                labelMConStat.ForeColor = Color.Red;
                labelMConStat.Text = "not connected";
            }

            labelMRa.Text = deviceParams.mountParams.ra;
            labelMDec.Text = deviceParams.mountParams.dec;
            labelMAz.Text = deviceParams.mountParams.az;
            labelMAlt.Text = deviceParams.mountParams.alt;
            labelMDate.Text = deviceParams.mountParams.date;
            labelMUt.Text = deviceParams.mountParams.ut;
            labelMSt.Text = deviceParams.mountParams.st;
            switch (deviceParams.mountParams.stat)
            {
                case TS_Stopped:
                    labelMMovStat.Text = "Stopped";
                    break;
                case TS_Homing:
                    labelMMovStat.Text = "Homing...";
                    break;
                case TS_Homed:
                    labelMMovStat.Text = "Homed";
                    break;
                case TS_Slewing:
                    labelMMovStat.Text = "Slewing...";
                    break;
                case TS_Tracking:
                    labelMMovStat.Text = "Tracking...";
                    break;
                case TS_Parking:
                    labelMMovStat.Text = "Parking...";
                    break;
                case TS_Parked:
                    labelMMovStat.Text = "Parked";
                    break;
                default:
                    break;
            }
            labelMTarRa.Text = deviceParams.mountParams.targetRa;
            labelMTarDec.Text = deviceParams.mountParams.targetDec;

        }

        private void UpdateFocuserStatus()
        {
            if (fut != null && fut.mDeviceConnections.ContainsKey("FOCUSER"))
            {
                labelFConStat.ForeColor = Color.Green;
                labelFConStat.Text = "connected";
            }
            else
            {
                labelFConStat.ForeColor = Color.Red;
                labelFConStat.Text = "not connected";
            }
            labelFCurPos.Text = deviceParams.focuserParams.pos.ToString();
            labelFCurTemp.Text = deviceParams.focuserParams.temp.ToString();
            if (deviceParams.focuserParams.isMoving == 1)
            {
                labelFMovStat.ForeColor = Color.Red;
                labelFMovStat.Text = "moving...";
            }
            else
            {
                labelFMovStat.ForeColor = Color.Black;
                labelFMovStat.Text = "stopped";
            }
        }

        private void timerUpdateStatus_Tick(object sender, EventArgs e)
        {
            UpdateWheelStatus();
            UpdateCcdStatus();
            UpdateFocuserStatus();
            UpdateMountStatus();
        }

        private void 开始观测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmStartObs!=null)
            {
                frmStartObs.Show();
                frmStartObs.Focus();
            }
        }


    }
}
