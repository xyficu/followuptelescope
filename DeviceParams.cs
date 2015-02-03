using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Follow_Up_Telescope
{

    public class MountParams
    {
        public MountParams()
        {
            ra = "";
            dec = "";
            az = "";
            alt = "";
            longitude = "";
            latitude = "";
            connected = false;
            errMsg = "";
            st = "";
            ut = "";
            date = "";
            stat = 0;
            targetDec = "";
            targetRa = "";
        }
        public string ra;
        public string dec;
        public string az;
        public string alt;
        public string longitude;
        public string latitude;
        public string st;
        public string date;
        public string ut;
        public bool connected;
        public string errMsg;
        public int stat;
        public string targetRa;
        public string targetDec;

    }

    public class FocuserParams
    {
        public FocuserParams()
        {
            connected = false;
            pos = 0.0;
            temp = 0.0;
            isMoving = 0;
        }
        public bool connected;
        public double pos;
        public double temp;
        public int isMoving;

    }

    public class CcdParams
    {
        public CcdParams()
        {
            temp = 0.0;
            coolerSwitch = false;
            acqStat = 0;
            gain = 0;
            binx = 1;
            biny = 1;
            imgPath = "./";
            fileName = "";
            shutter = false;
            expTime = 0;
            amount = 0;
            acqProc = 0.0;
            curNumb = 0;
            imgAmt = 0;
        }
        public double temp;
        public bool coolerSwitch;
        public int acqStat;
        public int gain;
        public int binx;
        public int biny;
        public string imgPath;
        public string fileName;
        public bool shutter;
        public double expTime;
        public int amount;
        public double acqProc;
        public int curNumb;
        public int imgAmt;
    }

    public class WheelParams
    {
        public WheelParams()
        {
            conn = false;
            curPos = -1;
            movStatus = 0;
        }
        public bool conn;
        public int curPos;
        public int movStatus;
    }

    public class DeviceParams
    {
        public DeviceParams()
        {
            mountParams = new MountParams();
            ccdParams = new CcdParams();
            focuserParams = new FocuserParams();
            wheelParams = new WheelParams();
        }
        public MountParams mountParams;
        public CcdParams ccdParams;
        public FocuserParams focuserParams;
        public WheelParams wheelParams;//wheel status
    }

}
