using System;
using HYZK.FrameWork.Common;
namespace DKLManager.Contract.Model
{
    public class DeviceModelRequest : Request
    {
        private int checkState = -1;
        public int CheckState
        {
            get { return checkState; }
            set { checkState = value; }
        }
        private int deviceMold = -1;
        public int DeviceMold
        {
            get { return deviceMold; }
            set { deviceMold = value; }
        }
        public string Number { get; set; }
        public string DeviceName { get; set; }
    }
}
