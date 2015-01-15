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
        private DeviceStatus mDeviceStatus;
        private Socket mServer;

        public FutTcpServer(DeviceStatus dev)
        {
            mDeviceConnections = new Dictionary<string, Socket>();
            mDeviceStatus = dev;

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
                    ClientThread newClient = new ClientThread(client, mDeviceStatus, mDeviceConnections);
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
        private DeviceStatus mDeviceStatus;
        private Dictionary<string, Socket> mDeviceConnections;
        //member elements
        private static int mConnections = 0;
        private Socket mService;
        int i;
        //constructor
        public ClientThread(Socket clientSocket, DeviceStatus deviceStatus, Dictionary<string, Socket> deviceConnections)
        {
            //save client socket
            this.mService = clientSocket;
            this.mDeviceStatus = deviceStatus;
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

                    //register device 
                    RegisterDevice(data);

                    //deregister device
                    DeregisterDevice(data);

                    //Resolve commands
                    ResolveCmds(data);

                    //reply a message to client
                    data = "received: " + data;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                    mService.Send(msg);
                    Console.WriteLine("data sent: {0}", data);
                }
                

                mService.Close();
                mConnections--;
                //remove invalid socket  NEED FIX
                RemoveInvalidDevice();
                Console.WriteLine("client closed: {0} connection(s)", mConnections);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("ClientService error: {0}", ex.ToString());
            }
            
        }

        //remove invalid device
        private void RemoveInvalidDevice()
        {
            foreach (string key in mDeviceConnections.Keys)
            {
                if (!mDeviceConnections[key].Poll(-1, SelectMode.SelectRead))
                {
                    mDeviceConnections.Remove(key);
                }
            }
        }

        //register device if first connect
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
        private void DeregisterDevice(string message)
        {
            string deviceName = message.Substring(1);
            if (deviceName == "WHEEL" && mDeviceConnections.ContainsKey("WHEEL"))
            {
                //mDeviceConnections["WHEEL"].Close();
                mDeviceConnections.Remove("WHEEL");
                Console.WriteLine("wheel is deregistered.");
            }
            else if (deviceName == "CCD" && mDeviceConnections.ContainsKey("CCD"))
            {
                //mDeviceConnections["CCD"].Close();
                mDeviceConnections.Remove("CCD");
                Console.WriteLine("ccd is deregistered.");
            }
            else if (deviceName == "FOCUSER" && mDeviceConnections.ContainsKey("FOCUSER"))
            {
                //mDeviceConnections["FOCUSER"].Close();
                mDeviceConnections.Remove("FOCUSER");
                Console.WriteLine("focuser is deregistered.");
            }
            else if (deviceName == "MOUNT" && mDeviceConnections.ContainsKey("MOUNT"))
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
                string deviceType, cmd, pos, mov, lt;
                string[] cmdList = message.Split(',');
                deviceType = cmdList[0];

                if (deviceType != "RW")
                    return;
                cmd = cmdList[1];
                if (cmd == "STATUS")
                {
                    pos = cmdList[2];
                    mov = cmdList[3];
                    lt = cmdList[4];
                }
                else
                    return;

                mDeviceStatus.wheelStatus.curPos = Int32.Parse(pos);
                mDeviceStatus.wheelStatus.movStatus = Int32.Parse(mov);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("cmds error: {0}", ex.ToString());
            }

        }

        
    }
}
