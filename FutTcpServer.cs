using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Follow_Up_Telescope
{
    /// <summary>
    /// a tcp server listen at local port, max connection is 10,
    /// which is used for controlling telescope by communicating 
    /// with mount, focuser, ccd and filter wheel
    /// </summary>
    class FutTcpServer
    {
        //save device connections
        public Dictionary<string, Socket> mDeviceConnections;
        //save device status
        private DeviceParams mDeviceParams;
        private Socket mServer;
        private Thread refreshThread;
        private int refreshFreq;
        private Thread removeInvalidDeviceThread;
        private int removeFreq;
        Timer m_tmrRmDev;
        ClientThread newClient;

        public FutTcpServer(DeviceParams dev)
        {
            mDeviceConnections = new Dictionary<string, Socket>();
            mDeviceParams = dev;

            refreshFreq = 200;
            refreshThread = new Thread(new ThreadStart(RefreshStatus));
            refreshThread.IsBackground = true;
            refreshThread.Start();
            
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


        /// <summary>
        /// 从连接字典中取出socket连接，如果存在，就向他请求STATUS
        /// </summary>
        private void RefreshStatus()
        {
            Socket value;
            string lt = "";
            string data = "";
            try
            {
                while (true)
                {
                    lt = GetLocalTime();
                    //更新Wheel状态
                    if (mDeviceConnections.TryGetValue("WHEEL", out value))
                    {
                        data = "W,STATUS," + lt;
                        Console.WriteLine(data);
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                        mDeviceConnections["WHEEL"].Send(msg);
                    }
                    //更新CCD状态
                    if (mDeviceConnections.TryGetValue("CCD", out value))
                    {
                        data = "C,STATUS," + lt;
                        Console.WriteLine(data);
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                        mDeviceConnections["CCD"].Send(msg);
                    }
                    //更新Focuser状态
                    if (mDeviceConnections.TryGetValue("FOCUSER", out value))
                    {
                        data = "F,STATUS," + lt;
                        Console.WriteLine(data);
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                        mDeviceConnections["FOCUSER"].Send(msg);
                    }
                    //更新Mount状态
                    if (mDeviceConnections.TryGetValue("MOUNT", out value))
                    {
                        data = "M,STATUS," + lt;
                        Console.WriteLine(data);
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                        mDeviceConnections["MOUNT"].Send(msg);
                    }
                    System.Threading.Thread.Sleep(refreshFreq);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
                char c = data[0];
                switch (c)
                {
                    case 'C':
                        newClient.DeregisterDevice("RCCD");
                        break;
                    case 'F':
                        newClient.DeregisterDevice("RFOCUSER");
                        break;
                    case 'M':
                        newClient.DeregisterDevice("RMOUNT");
                        break;
                    case 'W':
                        newClient.DeregisterDevice("RWHEEL");
                        break;
                    default:
                        break;
                }
                RefreshStatus();
            }

        }

        public void StartServer()
        {
            try
            {
                //initial ip address
                //以后写成配置文件IP, Port
                IPAddress local = IPAddress.Parse(GetLocalIP());
                IPEndPoint iep = new IPEndPoint(local, 30001);
                mServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                    ProtocolType.Tcp);
                //bind ip end point to socket
                mServer.Bind(iep);
                //start listening at local port 30001
                mServer.Listen(10);
                Console.WriteLine("start listening at {0}:30001, wait for connecting...", local.ToString());
                while (true)
                {
                    //get client socket
                    Socket client = mServer.Accept();
                    //create message thread object
                    newClient = new ClientThread(client, mDeviceParams, mDeviceConnections);
                    //pass client method to thread
                    Thread newthread = new Thread(new ThreadStart(newClient.ClientService));
                    //start thread message service
                    newthread.Start();
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        private string GetLocalIP()
        {
            try
            {
                string hostName = Dns.GetHostName();
                Console.WriteLine("host name: " + hostName);
                IPHostEntry ipEntry = Dns.GetHostEntry(hostName);
                foreach (IPAddress ip in ipEntry.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                        
                    }
                }
                return "";
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("get local ip address error: ", ex.ToString());
                return "";
            }
        }




    }

    /// <summary>
    /// a tcp client service
    /// use to control client device via tcp connections
    /// </summary>
    public class ClientThread
    {
        //save device status
        private DeviceParams mDeviceParams;
        private Dictionary<string, Socket> mDeviceConnections;
        //member elements
        private static int mConnections = 0;
        private Socket mService;
        int i;
        //constructor
        public ClientThread(Socket clientSocket, DeviceParams deviceParams, Dictionary<string, Socket> deviceConnections)
        {
            //save client socket
            this.mService = clientSocket;
            this.mDeviceParams = deviceParams;
            this.mDeviceConnections = deviceConnections;
        }
        public void ClientService()
        {
            try
            {
                //directly use member data device status

                String data = null;
                byte[] bytes = new byte[1024];

                //if socket is not null, then connections plus 1, save connections
                if (mService != null)
                {
                    mConnections++;
                }
                Console.WriteLine("new client is set up: {0} connection(s)",
                    mConnections);

                while ((i = mService.Receive(bytes)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("data received: {0}", data);

                    //////////////////////////////////////////////////////////////////////////
                    //解析收到的字符串
                    //////////////////////////////////////////////////////////////////////////
                    //注册设备
                    RegisterDevice(data);

                    //注销设备
                    DeregisterDevice(data);

                    //解析指令
                    ResolveCmds(data);

                    //reply a message to client
                    data = "received: " + data;
                    //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                    //mService.Send(msg);
                    //Console.WriteLine("data sent: {0}", data);
                }
                

                mService.Close();
                mConnections--;
                Console.WriteLine("client closed: {0} connection(s)", mConnections);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("ClientService error: {0}", ex.ToString());
            }
            
        }

        //register device if first connect
        /// <summary>
        /// 注册设备，设备连接后会发送设备名字，以区别连接种类，后将其放入连接字典
        /// </summary>
        /// <param name="message"></param>
        private void RegisterDevice(string message)
        {
            string deviceName = message;
            if (deviceName == "WHEEL" && !mDeviceConnections.ContainsKey("WHEEL"))
            {
                mDeviceConnections.Add("WHEEL", mService);
                Console.WriteLine("wheel is registered.");
            }
            else if (deviceName == "CCD" && !mDeviceConnections.ContainsKey("CCD"))
            {
                mDeviceConnections.Add("CCD", mService);
                Console.WriteLine("ccd is registered.");
            }
            else if (deviceName == "FOCUSER" && !mDeviceConnections.ContainsKey("FOCUSER"))
            {
                mDeviceConnections.Add("FOCUSER", mService);
                Console.WriteLine("focuser is registered.");
            }
            else if (deviceName == "MOUNT" && !mDeviceConnections.ContainsKey("MOUNT"))
            {
                mDeviceConnections.Add("MOUNT", mService);
                Console.WriteLine("mount is registered.");
            }
        }

        //deregister device
        /// <summary>
        /// 注销设备，设备断开时会发送设备名称，此名称与注册时不同，如果收到则将连接从字典中移除
        /// </summary>
        /// <param name="message"></param>
        public void DeregisterDevice(string message)
        {
            if (message == "RWHEEL" && mDeviceConnections.ContainsKey("WHEEL"))
            {
                //mDeviceConnections["WHEEL"].Close();
                mDeviceConnections.Remove("WHEEL");
                Console.WriteLine("wheel is deregistered.");
            }
            else if (message == "RCCD" && mDeviceConnections.ContainsKey("CCD"))
            {
                //mDeviceConnections["CCD"].Close();
                mDeviceConnections.Remove("CCD");
                Console.WriteLine("ccd is deregistered.");
            }
            else if (message == "RFOCUSER" && mDeviceConnections.ContainsKey("FOCUSER"))
            {
                //mDeviceConnections["FOCUSER"].Close();
                mDeviceConnections.Remove("FOCUSER");
                Console.WriteLine("focuser is deregistered.");
            }
            else if (message == "RMOUNT" && mDeviceConnections.ContainsKey("MOUNT"))
            {
                //mDeviceConnections["MOUNT"].Close();
                mDeviceConnections.Remove("MOUNT");
                Console.WriteLine("mount is deregistered.");
            }
        }

        private void ResolveCmds(string message)
        {
            try
            {
                //resolve message
                string deviceType, cmd;
                string pos, mov, res, lt;
                string ra = "", dec = "", az = "", alt = "", date = "", ut = "", st = "", movStat = "";
                string temp = "", coolerSwitch = "", acqStat = "", gain = "", binx = "", biny = "", imgPath = "";
                string fileName = "", shutter = "", expTime = "", amount = "", curNumb = "", acqProc = "", imgAmt = "";
                string targetRa = "", targetDec = "";
                string[] cmdList = message.Split(',');
                deviceType = cmdList[0];
                string reply = "";

                if (cmdList.Length < 3)
                    return;
                cmd = cmdList[1];
                lt = cmdList.Last();

                //解析滤光片转轮消息
                if (deviceType == "RW")
                {
                    if (cmd == "STATUS")
                    {
                        pos = cmdList[2];
                        mov = cmdList[3];
                        //更新状态
                        mDeviceParams.wheelParams.curPos = Int32.Parse(pos);
                        mDeviceParams.wheelParams.movStatus = Int32.Parse(mov);
                    }
                    else if (cmd == "MOVE")
                    {
                        pos = cmdList[2];
                        res = cmdList[3];
                        //接收消息策略
                    }
                    else if (cmd == "HOUSEKEEPING")
                    {
                        //housekeeping策略

                    }
                    else
                        return;

                }
                //解析CCD消息
                else if (deviceType == "RC")
                {
                    if (cmd=="STATUS")
                    {
                        temp = cmdList[2];
                        coolerSwitch = cmdList[3];
                        acqStat = cmdList[4];
                        gain = cmdList[5];
                        binx = cmdList[6];
                        biny = cmdList[7];
                        imgPath = cmdList[8];
                        acqProc = cmdList[9];
                        curNumb = cmdList[10];
                        imgAmt = cmdList[11];
                        //
                        mDeviceParams.ccdParams.temp = Double.Parse(temp);
                        mDeviceParams.ccdParams.coolerSwitch = coolerSwitch == "1" ? true : false;
                        mDeviceParams.ccdParams.acqStat = Int32.Parse(acqStat);
                        mDeviceParams.ccdParams.gain = Int32.Parse(gain);
                        mDeviceParams.ccdParams.binx = Int32.Parse(binx);
                        mDeviceParams.ccdParams.biny = Int32.Parse(biny);
                        mDeviceParams.ccdParams.imgPath = imgPath;
                        mDeviceParams.ccdParams.acqProc = Double.Parse(acqProc);
                        mDeviceParams.ccdParams.curNumb = Int32.Parse(curNumb);
                        mDeviceParams.ccdParams.imgAmt = Int32.Parse(imgAmt);
                    }
                    else if (cmd == "GETIMG")
                    {
                        fileName = cmdList[2];
                        shutter = cmdList[3];
                        expTime = cmdList[4];
                        amount = cmdList[5];
                        res = cmdList[6];
                        //
                        mDeviceParams.ccdParams.fileName = fileName;
                        mDeviceParams.ccdParams.shutter = shutter == "1" ? true : false;
                        mDeviceParams.ccdParams.expTime = Double.Parse(expTime);
                        mDeviceParams.ccdParams.amount = Int32.Parse(amount);
                    }
                    else if (cmd == "SEGGAIN")
                    {
                        gain = cmdList[2];
                        res = cmdList[3];
                        //
                        mDeviceParams.ccdParams.gain = Int32.Parse(gain);
                    }
                    else if (cmd == "SETBIN")
                    {
                        binx = cmdList[2];
                        biny = cmdList[3];
                        res = cmdList[4];
                        //
                        mDeviceParams.ccdParams.binx = Int32.Parse(binx);
                        mDeviceParams.ccdParams.biny = Int32.Parse(biny);
                    }
                    else if (cmd == "SETCOOLERTEMP")
                    {
                        temp = cmdList[2];
                        res = cmdList[4];
                        //
                        mDeviceParams.ccdParams.temp = Double.Parse(temp);
                    }
                    else if (cmd == "SETCOOLERSWITCH")
                    {
                        coolerSwitch = cmdList[2];
                        res = cmdList[3];
                        //
                        mDeviceParams.ccdParams.coolerSwitch = coolerSwitch == "1" ? true : false;
                    }
                    else if (cmd == "HOUSEKEEPING")
                    {
                        //housekeeping 策略
                    }


                }
                //解析Mount消息
                else if (deviceType == "RM")
                {
                    if (cmd == "STATUS")
                    {
                        ra = cmdList[2];
                        dec = cmdList[3];
                        az = cmdList[4];
                        alt = cmdList[5];
                        date = cmdList[6];
                        ut = cmdList[7];
                        st = cmdList[8];
                        movStat = cmdList[9];
                        //更新状态

                        mDeviceParams.mountParams.ra = ra;
                        mDeviceParams.mountParams.dec = dec;
                        mDeviceParams.mountParams.az = az;
                        mDeviceParams.mountParams.alt = alt;
                        mDeviceParams.mountParams.date = date;
                        mDeviceParams.mountParams.ut = ut;
                        mDeviceParams.mountParams.st = st;
                        mDeviceParams.mountParams.stat = Int32.Parse(movStat);

                    }
                    else if (cmd == "SLEW")
                    {
                        targetRa = cmdList[2];
                        targetDec = cmdList[3];
                        res = cmdList[4];
                        //
                        mDeviceParams.mountParams.targetRa = targetRa;
                        mDeviceParams.mountParams.targetDec = targetDec;

                    }
                    else if (cmd=="STOP")
                    {
                        res = cmdList[2];
                    }
                    else if (cmd=="HOME")
                    {
                        res = cmdList[2];
                    }
                    else if (cmd == "PARK")
                    {
                        res = cmdList[2];
                    }
                    else if (cmd == "HOUSEKEEPING")
                    {
                        //housekeeping策略

                    }
                    else
                        return;

                }
                //解析Focuser消息
                else if (deviceType == "RF")
                {
                    if (cmd == "STATUS")
                    {
                        pos = cmdList[2];
                        temp = cmdList[3];
                        mov = cmdList[4];
                        //
                        mDeviceParams.focuserParams.pos = Double.Parse(pos);
                        mDeviceParams.focuserParams.temp = double.Parse(temp);
                        mDeviceParams.focuserParams.isMoving = Int32.Parse(mov);
                    }
                    else if (cmd=="MOVE")
                    {
                        res = cmdList[2];
                    }
                    else if (cmd=="STOP")
                    {
                        res = cmdList[2];
                    }
                    else if (cmd == "STEP")
                    {
                        res = cmdList[2];
                    }
                    else if (cmd == "HOUSEKEEPING")
                    {
                        //housekeeping 策略
                    }
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("cmds error: {0}", ex.ToString());
            }

        }

        
    }
}
