using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Follow_Up_Telescope
{
    public class MountStatus
    {
        public MountStatus()
        {
            busy = false;
        }
        public bool busy;
    }
    public class CcdStatus
    {
        public CcdStatus()
        {
            busy = false;
        }
        public bool busy;
    }
    public class FocuserStatus
    {
        public FocuserStatus()
        {
            busy = false;
        }
        public bool busy;
    }
    public class WheelStatus
    {
        public WheelStatus()
        {
            conn = false;
            curPos = -1;
            movStatus = 0;
        }
        public bool conn;
        public int curPos;
        public int movStatus;
    }

    public class DeviceStatus
    {
        public DeviceStatus()
        {
            mountStatus = new MountStatus();
            ccdStatus = new CcdStatus();
            focuserStatus = new FocuserStatus();
            wheelStatus = new WheelStatus();
        }
        public MountStatus mountStatus;
        public CcdStatus ccdStatus;
        public FocuserStatus focuserStatus;
        public WheelStatus wheelStatus;//wheel status
    }
}
