using System;
using HYZK.FrameWork.Common;

namespace DKLManager.Contract.Model
{
    public class ProjectInfoRequest : Request
    {
        private int sampleState = -1;
        public int SampleState
        {
            get { return sampleState; }
            set { sampleState = value; }
        }
        private int analy = -1;
        public int AnalyzePeople
        {
            get { return analy; }
            set { analy = value; }
        }
        private int item = -1;
        public int ProjectStatus
        {
            get { return item; }
            set { item = value; }
        }
        private int projectAlarmStatus = -1;
        public int ProjectAlarmStatus 
        {
            get { return projectAlarmStatus; }
            set { projectAlarmStatus = value; }
        }
        private int projectCategory = -1;
        public int ProjectCategory
        {
            get { return projectCategory; }

            set { projectCategory = value; }
        }
        private int projectStatus =30;
        public int Status 
        {
            get { return projectStatus; }

            set { projectStatus = value; }
        }
        public DateTime ProjectClosingDate
        {
            get { return projectClosingDate; }
            set { projectClosingDate = value; }
        }
        public DateTime projectClosingDate = DateTime.Now;
        public string ProjectName { get; set; }
        private int deviceName = -1;
        public int DeviceName
        {
            get { return deviceName; }
            set { deviceName = value; }
        }
        public string Argument { get; set; }
        public string SampleRegisterNumber { get; set; }
        public string ProjectNumber { get; set; }
        public string ProjectCheif { get; set; }
        public string ProjectBarCode { get; set; }

        private int userAccountType = -1;
        public int UserAccountType  
        { 
            get{return userAccountType;}
            set { userAccountType = value; }
        }
        public string Tel { get; set; }
        public string userName { get; set; }
        public string CustomerName { get; set; }
    }
}

    
