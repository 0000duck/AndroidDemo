using DKLManager.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Demo.Areas.DKLManager.Models
{
    public class ProjectWholeInfoViewModel
    {
        public ProjectWholeInfoViewModel()
        {
            projectTime = new ProjectInfo();
            projectClosingDate = new ProjectInfo();
            projectBasicImgFile = new ProjectFile();
            device = new DeviceOrderInfo();
            projectBasicinfo = new ProjectInfo();
            projectConsultBasicinfo = new ConsultBasicInfo();
            projectBasicFile = new ProjectFile();
            projectTestBasicinfo = new TestBasicInfo();
            projectTestChemicalReport = new TestChemicalReport();
            projectTestChemicalReportList = new List<TestChemicalReport>();
            projectValueBasicinfo = new ValueBasicInfo();
            sampleTable = new SampleRegisterTable();
            projectTestBasicinfoList = new List<TestBasicInfo>();
            arguments = new ArgumentValue();

            

        }
     
        public ArgumentValue arguments;
        public ProjectInfo projectTime;
        public ProjectInfo projectClosingDate;
        public ProjectFile projectBasicImgFile;
        public DeviceOrderInfo device;
        public ProjectInfo projectBasicinfo;
        public ProjectFile projectBasicFile;
        public SampleRegisterTable sampleTable;
        public ArgumentValue argumentVal;
        public ConsultBasicInfo projectConsultBasicinfo;

        public TestBasicInfo projectTestBasicinfo;
        public TestChemicalReport projectTestChemicalReport;
        public ValueBasicInfo projectValueBasicinfo;

        public IList<TestBasicInfo> projectTestBasicinfoList;
        public IList<TestChemicalReport> projectTestChemicalReportList;

    }

    public class ProjectWholeInfoViewModelHis
    {
        public ProjectWholeInfoViewModelHis()
        {

            projectBasicinfoHistory = new ProjectInfoHistory();
            projectBasicFileHistory = new ProjectFileHistory();
            projectTestBasicinfoListHistory = new List<TestBasicInfoHistory>();
            projectTestChemicalReportListHistory = new List<TestChemicalReportListHistory>();
        }

        public ProjectInfoHistory projectBasicinfoHistory;
        public ProjectFileHistory projectBasicFileHistory;

        public IList<TestChemicalReportListHistory> projectTestChemicalReportListHistory;
        public IList<TestBasicInfoHistory> projectTestBasicinfoListHistory;
    }
}