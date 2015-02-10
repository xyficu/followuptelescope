using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Follow_Up_Telescope
{
    public class FutTwin
    {
        private IPAddress m_ip;
        private IPEndPoint m_ep;
        public Socket m_sktFut;
        string m_errMsg;
        bool m_connected;
        DeviceParams deviceParams;
        Timer m_timerHousekeep;
        ObsTar obsTar;
        public FutTwin(DeviceParams dev,ObsTar _obsTar)
        {
            deviceParams = dev;
            obsTar = _obsTar;
            m_ip = IPAddress.Parse("190.168.1.205");  //将来改为190.168.1.205
            m_ep = new IPEndPoint(m_ip, 30002);       //总控机端口为30002
            m_sktFut = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                ProtocolType.IP);
            m_errMsg = "";
            m_connected = false;

            //启动housekeeping线程
            m_timerHousekeep = new Timer(new TimerCallback(HouseKeeping), null, 0, 10000);

        }

        ~FutTwin()
        {
            try
            {
                //发送注销消息
                SendMessage("RS2");
                if (m_sktFut!=null)
                {
                    m_sktFut.Shutdown(SocketShutdown.Both);
                    m_sktFut.Close();
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Dispose error: " + ex.Message);
            }
        }

        //设备端反向连接
        public void ConnectToHost()
        {
            try
            {
                while (m_connected == false)
                {
                    Console.WriteLine("try to connect to host ...");
                    m_sktFut.Connect(m_ep);

                    m_connected = true;
                    //连接成功后发送注册消息
                    SendMessage("S2");

                    //连接成功后启动消息接收线程
                    Thread thd = new Thread(new ThreadStart(ReceiveMessage));
                    thd.IsBackground = true;
                    thd.Start();

                }

            }
            catch (System.Exception ex)
            {
                Thread.Sleep(5000);
                m_errMsg = ex.Message;
                m_connected = false;
                ConnectToHost();
            }
        }

        public void SendMessage(string msg)
        {
            try
            {
                byte[] buf = Encoding.ASCII.GetBytes(msg);
                if (true == m_connected)
                {
                    m_sktFut.Send(buf);

                }
            }
            catch (System.Exception ex)
            {

                Console.Write("send message error: " + ex.Message);
                m_sktFut.Close();
                m_sktFut = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                                        ProtocolType.IP);
                m_connected = false;
                ConnectToHost();
            }

        }

        //接收远程指令
        public void ReceiveMessage()
        {
            try
            {
                string msg = "";
                byte[] buf = new byte[1024];
                int length = 0;
                while ((length = m_sktFut.Receive(buf)) != 0)
                {
                    msg = Encoding.ASCII.GetString(buf, 0, length);
                    Console.WriteLine("message received: {0}", msg);
                    ResolveCmds(msg);
                }
            }
            catch (System.Exception ex)
            {
                m_errMsg = ex.Message;
                Console.WriteLine("resolve message error: " + ex.Message);
                //ReceiveMessage();
            }
        }

        //解析远程指令
        public void ResolveCmds(string msg)
        {
            try
            {
                string deviceType, cmd, lt;
                string[] cmdList = msg.Split(',');
                string reply = "";
                deviceType = cmdList[0];
                if (cmdList.Length < 3)
                    return;
                if (deviceType != "T1" && deviceType != "T2")
                    return;
                cmd = cmdList[1];
                lt = cmdList.Last();

                          
                if (cmd == "STATUS")
                {
                    //control device
                    //m_mountUser.GetAllStat(ref ra, ref dec, ref az, ref alt, ref date, ref ut, ref st, ref movStat);
                    //pos = m_mountUser.GetCurPos().ToString("f3");
                    //temp = m_mountUser.GetCurTemp().ToString("f1");
                    //ismoving = (m_mountUser.GetMoveStatus() ? 1 : 0).ToString();


                    //send reply message
                    reply = "R" + string.Join(",", cmdList, 0, cmdList.Length - 1);
                    //mount status
                    reply += "," + deviceParams.mountParams.ra;
                    reply += "," + deviceParams.mountParams.dec;
                    reply += "," + deviceParams.mountParams.targetRa;
                    reply += "," + deviceParams.mountParams.targetDec;
                    reply += "," + deviceParams.mountParams.az;
                    reply += "," + deviceParams.mountParams.alt;
                    reply += "," + deviceParams.mountParams.date;
                    reply += "," + deviceParams.mountParams.ut;
                    reply += "," + deviceParams.mountParams.st;
                    reply += "," + deviceParams.mountParams.stat.ToString();
                    //focuser status
                    reply += "," + deviceParams.focuserParams.pos.ToString();
                    reply += "," + deviceParams.focuserParams.isMoving.ToString();
                    reply += "," + deviceParams.focuserParams.temp.ToString();
                    //ccd status
                    reply += "," + deviceParams.ccdParams.temp.ToString();
                    reply += "," + (deviceParams.ccdParams.coolerSwitch == true ? 1 : 0).ToString();
                    reply += "," + deviceParams.ccdParams.acqStat.ToString();
                    reply += "," + deviceParams.ccdParams.gain.ToString();
                    reply += "," + deviceParams.ccdParams.binx.ToString();
                    reply += "," + deviceParams.ccdParams.biny.ToString();
                    reply += "," + deviceParams.ccdParams.imgPath;
                    reply += "," + deviceParams.ccdParams.acqProc.ToString();
                    reply += "," + deviceParams.ccdParams.curNumb.ToString();
                    reply += "," + deviceParams.ccdParams.imgAmt.ToString();
                    //wheel status
                    reply += "," + deviceParams.wheelParams.curPos.ToString();
                    reply += "," + deviceParams.wheelParams.movStatus.ToString();
                    reply += "," + deviceParams.teleStat.ToString();
                    //时间戳
                    reply += "," + lt;
                    SendMessage(reply);
                }
                else if (cmd == "INIT")
                {
                    obsTar.id = 0;
                    obsTar.ra = deviceParams.mountParams.st;
                    obsTar.dec = "40:00:00";
                    obsTar.color = "0";
                    obsTar.expTime = 3;
                    obsTar.amount = 1;
                    obsTar.type = 2;
                    obsTar.fileName = "initial";
                    obsTar.deviceType = deviceType;
                }
                else if (cmd == "OBS")
                {

                    obsTar.id = int.Parse(cmdList[2]);
                    obsTar.ra = cmdList[3];
                    obsTar.dec = cmdList[4];
                    obsTar.color = cmdList[5];
                    obsTar.expTime = double.Parse(cmdList[6]);
                    obsTar.amount = int.Parse(cmdList[7]);
                    obsTar.fileName = cmdList[8];
                    obsTar.type = 1;
                    obsTar.deviceType = deviceType;
                    
                }
                Console.WriteLine("send: {0}", reply);

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("cmds error: {0}", ex.Message);
            }
        }

        private void HouseKeeping(object o)
        {
            //获取系统时间
            string lt = GetLocalTime();
            //每隔10s由设备端发给服务器
            SendMessage("RS2,HOUSEKEEPING," + lt);
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

    }
}
