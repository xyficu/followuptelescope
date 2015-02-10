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
        ObsTar obsTar;
        System.Timers.Timer tmrObs;
        FutTwin futTwin;
        public StartObs(Dictionary<string, Socket> _deviceConnections, DeviceParams _deviceParams, ObsTar _obsTar, FutTwin _futTwin)
        {
            InitializeComponent();
            deviceConnections = _deviceConnections;
            deviceParams = _deviceParams;
            obsTar = _obsTar;
            futTwin = _futTwin;

            textBoxRA.Text = "5:00:00";
            textBoxDEC.Text = "40:00:00";
            textBoxColor.Text = "10";
            textBoxExpTime.Text = "5";
            textBoxAmount.Text = "3";
            textBoxFileName.Text = "test";

            targetList = new List<ListViewItem>();
            //tarList = new Dictionary<string, ListViewItem>();
            //backgroundWorker = new BackgroundWorker();
            //backgroundWorker.WorkerReportsProgress = true;
            //backgroundWorker.DoWork += new DoWorkEventHandler(ObsTargetList);

            tmrObs = new System.Timers.Timer(500);
            tmrObs.Elapsed += new System.Timers.ElapsedEventHandler(CheckObsTar);
            tmrObs.AutoReset = true;
            tmrObs.Enabled = true;

            //listview setting
            listView1.GridLines = true;
            listView1.MultiSelect = true;
            listView1.FullRowSelect = true;
        }

        private void CheckObsTar(object o, System.Timers.ElapsedEventArgs e)
        {
            if (obsTar.type != 0 && deviceParams.teleStat == 0)
            {
                //type == 2 表示初始化用例，开启制冷器到-80，转台转到天顶位置，
                //创建以当天名字命名的目录（/data/yyyymmdd），拍摄一张3s的照片
                if (obsTar.type == 2)
                {
                    obsTar.type = 0;
                    Socket value;
                    byte[] buf = new byte[1024];
                    string cmd = "";
                    string lt = "";
                    string path="";
                    string res = "";
                    if (deviceConnections.TryGetValue("CCD", out value))
                    {
                        //做初始化
                        //设置制冷温度-80
                        lt = GetLocalTime();
                        cmd = "C,SETCOOLERTEMP,-80," + lt;
                        buf = Encoding.ASCII.GetBytes(cmd);
                        value.Send(buf);
                        Thread.Sleep(1000);
                        
                        //开启制冷器
                        cmd = "C,SETCOOLERSWITCH,1," + lt;
                        buf = Encoding.ASCII.GetBytes(cmd);
                        value.Send(buf);
                        Thread.Sleep(1000);
                        if (deviceParams.ccdParams.coolerSwitch == false)
                        {
                            res = "1";
                            //发送初始化失败消息，并返回
                            cmd = "R" + obsTar.deviceType + ",INIT," + res + "," + lt;

                            futTwin.SendMessage(cmd);
                            return;
                        }
                        //创建目录/data/yyyymmdd/
                        path = "/data/"+lt.Substring(0, 8);
                        cmd = "C,MKPATH," + path + "," + lt;
                        buf = Encoding.ASCII.GetBytes(cmd);
                        value.Send(buf);
                        Thread.Sleep(1000);
                        
                        //设置为图像存储目录
                        cmd = "C,PATH," + path + "," + lt;
                        buf = Encoding.ASCII.GetBytes(cmd);
                        value.Send(buf);
                        Thread.Sleep(1000);
                        if (deviceParams.ccdParams.imgPath != path)
                        {
                            res = "2";
                            //发送初始化失败消息，并返回
                            cmd = "R" + obsTar.deviceType + ",INIT," + res + "," + lt;
                            futTwin.SendMessage(cmd);
                            return;
                        }
                        //转动到天顶位置拍摄一幅3s的图
                        ra = obsTar.ra;
                        dec = obsTar.dec;
                        color = int.Parse(obsTar.color);
                        time = obsTar.expTime;
                        amount = obsTar.amount;
                        fileName = obsTar.fileName;
                        obsThd = new Thread(new ThreadStart(ObsTarget));
                        obsThd.IsBackground = true;
                        obsThd.Start();
                        res = "0";
                        try
                        {
                            //发送初始化成功消息，并返回
                            cmd = "R" + obsTar.deviceType + ",INIT," + res + "," + lt;
                            futTwin.SendMessage(cmd);
                            Thread.Sleep(1000);
                        }
                        catch (System.Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        
                    }
                }
                    //type==1表示通常目标，执行通常观测步骤
                else if (obsTar.type == 1)
                {
                    obsTar.type = 0;
                    Socket value;
                    byte[] buf = new byte[1024];
                    string cmd = "";
                    string lt = "";
                    string path = "";
                    string res = "";

                    ra = obsTar.ra;
                    dec = obsTar.dec;
                    color = int.Parse(obsTar.color);
                    time = obsTar.expTime;
                    amount = obsTar.amount;
                    fileName = obsTar.fileName;
                    obsThd = new Thread(new ThreadStart(ObsTarget));
                    obsThd.IsBackground = true;
                    obsThd.Start();
                    res = "0";
                    lt = GetLocalTime();
                    try
                    {
                        cmd = "R" + obsTar.deviceType + ",OBS," + obsTar.id.ToString() + "," + obsTar.ra
                         + "," + obsTar.dec + "," + obsTar.color + "," + obsTar.expTime.ToString()
                         + "," + obsTar.amount + "," + obsTar.fileName + "," + res + "," + lt;
                        futTwin.SendMessage(cmd);
                        Thread.Sleep(3000);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    
                }

            }
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
        string fileName = "";
        public void ObsTarget()
        {
            Socket value;
            byte[] buf = new byte[1024];
            string cmd = "";
            string lt="";

            //开启制冷器到-80摄氏度
            //if (deviceConnections.TryGetValue("CCD", out value))
            //{
            //    lt=GetLocalTime();
            //    cmd="C,SETCOOLERTEMP,-80,"+lt;
            //    buf = Encoding.ASCII.GetBytes(cmd);
            //    value.Send(buf);
            //}
            //if (deviceConnections.TryGetValue("CCD", out value))
            //{
            //    lt = GetLocalTime();
            //    cmd = "C,SETCOOLERSWITCH,1," + lt;
            //    buf = Encoding.ASCII.GetBytes(cmd);
            //    value.Send(buf);
            //}
            //检查制冷温度是否到位
            //while (deviceParams.ccdParams.temp>-75)
            //{
            //    System.Threading.Thread.Sleep(1000);
            //}

            //设置望远镜处于busy状态
            deviceParams.teleStat = 1;

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
                cmd = "C,GETIMG," + fileName + ",1," + time.ToString() + "," + amount + "," +
                      ra + "," + dec + "," + deviceParams.mountParams.ut + "," + deviceParams.mountParams.st + "," +
                      "S1" + "," + "OBJECT" + "," + (deviceParams.wheelParams.curPos + 1).ToString() + "," + lt;
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

            deviceParams.teleStat = 0;
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
            targetList.Clear();
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
            try
            {
                if (deviceParams.teleStat == 1)
                {
                    return;
                }
                foreach (ListViewItem item in targetList)
                {
                    //从list中取出一个源，赋值给变量

                    ra = item.SubItems[1].Text;
                    dec = item.SubItems[2].Text;
                    color = int.Parse(item.SubItems[3].Text) - 1;
                    time = double.Parse(item.SubItems[4].Text);
                    amount = int.Parse(item.SubItems[5].Text);
                    fileName = item.SubItems[6].Text;
                    ObsTarget();
                }
            }
            catch (System.Exception ex)
            {
                String err = ex.Message;
            }
                
        }

        private void AddTarget(string ra, string dec, int color, double time, int amount, string fileName)
        {
            ListViewItem record = new ListViewItem((listView1.Items.Count + 1).ToString());
            record.SubItems.Add(ra);
            record.SubItems.Add(dec);
            record.SubItems.Add(color.ToString());
            record.SubItems.Add(time.ToString());
            record.SubItems.Add(amount.ToString());
            record.SubItems.Add(fileName);
            listView1.Items.Add(record);
        }

        private void buttonAddTarget_Click(object sender, EventArgs e)
        {
            AddTarget(textBoxRA.Text,
                      textBoxDEC.Text,
                      int.Parse(textBoxColor.Text),
                      double.Parse(textBoxExpTime.Text),
                      int.Parse(textBoxAmount.Text),
                      textBoxFileName.Text);
        }

        //让窗体关闭按钮作用改为隐藏
        private void StartObs_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void buttonDelTarget_Click(object sender, EventArgs e)
        {
            int count = listView1.Items.Count;
            for (int i = count - 1; i >= 0;i-- )
            {
                if (listView1.Items[i].Selected)
                {
                    listView1.Items.RemoveAt(i);
                }
            }
        }

        private void buttonInit_Click(object sender, EventArgs e)
        {
            obsTar.id = 0;
            obsTar.ra = deviceParams.mountParams.st;
            obsTar.dec = "40:00:00";
            obsTar.color = "0";
            obsTar.expTime = 3;
            obsTar.amount = 1;
            obsTar.fileName = "initial";
            obsTar.type = 2;
        }


    }
}
