using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

namespace Follow_Up_Telescope
{
    public partial class StartObs : Form
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

        Dictionary<string, Socket> deviceConnections;
        DeviceParams deviceParams;
        Thread obsThd;
        List<ListViewItem> targetList;
        public StartObs(Dictionary<string, Socket> _deviceConnections, DeviceParams _deviceParams)
        {
            InitializeComponent();
            deviceConnections = _deviceConnections;
            deviceParams = _deviceParams;

            textBoxRA.Text = "5:00:00";
            textBoxDEC.Text = "40:00:00";
            textBoxColor.Text = "10";
            textBoxExpTime.Text = "5";
            textBoxAmount.Text = "3";

            targetList = new List<ListViewItem>();
            //tarList = new Dictionary<string, ListViewItem>();
            //backgroundWorker = new BackgroundWorker();
            //backgroundWorker.WorkerReportsProgress = true;
            //backgroundWorker.DoWork += new DoWorkEventHandler(ObsTargetList);
        }

        const int U = 0;
        const int B = 1;
        const int V = 2;
        const int R = 3;
        const int I = 4;
        const int L = 5;
        const int RED = 6;
        const int GREEN = 7;
        const int BLUE = 8;
        const int empty = 9;

        string ra = "", dec = "";
        int color = 0;
        double time = 0.0;
        int amount = 1;

        public void ObsTarget()
        {
            Socket value;
            byte[] buf = new byte[1024];
            string cmd = "";
            string lt="";

            //开启制冷器到-80摄氏度
            if (deviceConnections.TryGetValue("CCD", out value))
            {
                lt=GetLocalTime();
                cmd="C,SETCOOLERTEMP,-80,"+lt;
                buf = Encoding.ASCII.GetBytes(cmd);
                value.Send(buf);
            }
            if (deviceConnections.TryGetValue("CCD", out value))
            {
                lt = GetLocalTime();
                cmd = "C,SETCOOLERSWITCH,1," + lt;
                buf = Encoding.ASCII.GetBytes(cmd);
                value.Send(buf);
            }
            //检查制冷温度是否到位
            while (deviceParams.ccdParams.temp>-75)
            {
                System.Threading.Thread.Sleep(1000);
            }


            //转动转台
            if (deviceConnections.TryGetValue("MOUNT", out value))
            {
                lt = GetLocalTime();
                cmd = "M,SLEW," + ra + "," + dec + "," + lt;
                buf = Encoding.ASCII.GetBytes(cmd);
                value.Send(buf);
            }

            //转动滤光片
            if (deviceConnections.TryGetValue("WHEEL", out value))
            {
                lt = GetLocalTime();
                cmd = "W,MOVE," + color.ToString() + "," + lt;
                buf = Encoding.ASCII.GetBytes(cmd);
                value.Send(buf);
            }
            Thread.Sleep(1000);
            //转台是否到位
            int count = 0;
            while (deviceParams.mountParams.stat!=TS_Tracking || count <3)
            {
                System.Threading.Thread.Sleep(1000);
                if (deviceParams.mountParams.stat == TS_Tracking) count++;
            }

            //检查滤光片是否到位

            while (deviceParams.wheelParams.curPos != color || deviceParams.wheelParams.movStatus != 0)
            {
                System.Threading.Thread.Sleep(1000);
            }

            //CCD开始拍摄
            if (deviceConnections.TryGetValue("CCD", out value))
            {
                lt = GetLocalTime();
                cmd = "C,GETIMG," + ra + "_" + dec + ",1," + time.ToString() + "," + amount + "," + lt;
                buf = Encoding.ASCII.GetBytes(cmd);
                value.Send(buf);
            }
            //Thread.Sleep((int)time * amount * 1000);
            //等待拍摄结束
            count = 0;
            while (deviceParams.ccdParams.acqStat != 0 || count < 3)
            {
                Thread.Sleep(1000);
                if (deviceParams.ccdParams.acqStat == 0) count++;
            }
        }

        //获取local time
        private string GetLocalTime()
        {
            return DateTime.Now.Year.ToString() +
                        DateTime.Now.Month.ToString("d2") +
                        DateTime.Now.Day.ToString("d2") +
                        "T" +
                        DateTime.Now.Hour.ToString("d2") +
                        DateTime.Now.Minute.ToString("d2") +
                        DateTime.Now.Second.ToString("d2") +
                        "." +
                        DateTime.Now.Millisecond.ToString("d3");
        }

       
        private void buttonStartObs_Click(object sender, EventArgs e)
        {

            //取出listview中所有的item
            foreach (ListViewItem item in listView1.Items)
            {
                targetList.Add(item);
            }
            obsThd = new Thread(new ThreadStart(ObsTargetList));
            obsThd.IsBackground = true;
            obsThd.Start();
        }

        /// <summary>
        /// 开始列表观测
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ObsTargetList()
        {

                foreach (ListViewItem item in targetList)
                {
                    //从list中取出一个源，赋值给变量

                    ra = item.SubItems[1].Text;
                    dec = item.SubItems[2].Text;
                    color = int.Parse(item.SubItems[3].Text);
                    time = double.Parse(item.SubItems[4].Text);
                    amount = int.Parse(item.SubItems[5].Text);
                    ObsTarget();
                }
                   

        }

        private void AddTarget(string ra, string dec, int color, double time, int amount)
        {
            ListViewItem record = new ListViewItem(listView1.Items.Count.ToString());
            record.SubItems.Add(ra);
            record.SubItems.Add(dec);
            record.SubItems.Add(color.ToString());
            record.SubItems.Add(time.ToString());
            record.SubItems.Add(amount.ToString());
            listView1.Items.Add(record);
        }

        private void buttonAddTarget_Click(object sender, EventArgs e)
        {
            AddTarget(textBoxRA.Text,
                      textBoxDEC.Text,
                      int.Parse(textBoxColor.Text),
                      double.Parse(textBoxExpTime.Text),
                      int.Parse(textBoxAmount.Text));
        }

        //让窗体关闭按钮作用改为隐藏
        private void StartObs_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }


    }
}
