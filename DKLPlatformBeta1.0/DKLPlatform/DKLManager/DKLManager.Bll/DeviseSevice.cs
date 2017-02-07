using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HYZK.FrameWork.DAL;
using DKLManager.Contract.Model;
using DKLManager.Contract;
using HYZK.Core.Config;
using HYZK.Core.Cache;
using DKLManager.Dal;
using HYZK.FrameWork.Common;
using HYZK.Account.Contract;
namespace DKLManager.Bll
{
    public class DKLManagerSevice : IDKLManager
    {
        public List<SampleRegisterTable> GetSampleRegisterTableListEdit(string projectNumber)
        {
            List<SampleRegisterTable> list = new List<SampleRegisterTable>();
            using (var dbContext = new DKLManagerDbContext())
            {

               dbContext.SampleRegisterDB.Where(u => u.ProjectNumber == projectNumber).ToList().ForEach(a => { list.Add(a); });
               return list;
            }

        }
        public IEnumerable<DetectionResult> GetDetectionResultList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.DetectionResultDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public DetectionResult SelectDetectionResult(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.DetectionResultDB.Where(u => u.ID == id).FirstOrDefault();
            }
        }
        public void InsertDetectionResult(DetectionResult DeteResult)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<DetectionResult>(DeteResult);
            }
        }
        public void UpDateDetectionResult(DetectionResult DeteResult)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Update<DetectionResult>(DeteResult);
            }
        }
        public void DeleteDetectionResult(List<int> id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.DetectionResultDB.Where(u => id.Contains(u.ID)).ToList().ForEach(a => { dbContext.DetectionResultDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }

        public List<TestBasicInfo> GetTestBasicInfoList(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                IQueryable<TestBasicInfo> TestBasicInfo = dbContext.TestBasicInfoDB;

                if (!string.IsNullOrEmpty(projectNumber))
                {
                    TestBasicInfo = TestBasicInfo.Where(u => u.ProjectNumber == projectNumber);
                }
                return TestBasicInfo.OrderByDescending(u => u.ID).ToList();
            }
        }
        public TestBasicInfo GetTestBasicInfo(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.TestBasicInfoDB.Where(c => c.ID == id).FirstOrDefault();
            }
        }
        public TestBasicInfo GetTestBasicInfo(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.TestBasicInfoDB.Where(c => c.ProjectNumber == projectNumber).FirstOrDefault();
            }
        }

        public List<TestPhysicalReport> GetPhysicalModels(string projectName)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.TestPhysicalReportDB.Where(u => u.ProjectNumber == projectName).ToList();
            }
        }
        public List<TestChemicalReport> GetChemicalReport(string projectName)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.TestChemicalReportDB.Where(u => u.ProjectName == projectName).ToList();
            }
        }
        public List<ProjectInfo> GetProjectInfoModels(string projectName)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectInfoDB.Where(u => u.ProjectName == projectName).ToList();
            }
        }
        public List<ProjectInfo> GetProjectInfos(string Year)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                int year = Convert.ToInt32(Year);
                return dbContext.ProjectInfoDB.Where(u=>u.CreateTime.Year == year).OrderBy(u=>u.ID).ToList();
            }
        }
       public List<ProjectInfoHistory> GetProjectInfosHistory(string Year)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                int year = Convert.ToInt32(Year);
                return dbContext.ProjectInfoHistoryDB.Where(u => u.ProjectClosingDate.Year == year).OrderBy(u => u.ID).ToList();
            }
        }

        public List<string> GetSampleList(string projectName)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                List<string> str = new List<string>();
                dbContext.TestChemicalReportDB.Where(u => u.ProjectName == projectName).ToList().ForEach(a => { str.Add(a.SampleProject); });
                str = str.Distinct<string>().ToList();
                return str;
            }
        }
      

        public List<string> GetTestList(string projectName)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                List<string> str = new List<string>();
                dbContext.TestPhysicalReportDB.Where(u => u.ProjectNumber == projectName).ToList().ForEach(a => { str.Add(a.TestContent); });
                str = str.Distinct<string>().ToList();
                return str;
            }
        }
        public IEnumerable<TestPhysicalReport> GetTestReportProjectList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.TestPhysicalReportDB.OrderBy(u => u.ProjectNumber).ToPagedList(request.PageIndex, request.PageSize);
            }
        }

        public IEnumerable<TestChemicalReport> GetTestTestChemicalReportList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.TestChemicalReportDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public List<TestChemicalReport> GetTestChemicalReportByProjectNumber(string ProjectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                List<TestChemicalReport> list = new List<TestChemicalReport>();
                dbContext.TestChemicalReportDB.Where(u => u.ProjectNumber == ProjectNumber).ToList().ForEach(a => { list.Add(a); });
                list = list.OrderBy(u => u.SampleNumber).ToList();
                return list;

            }
        }
        public TestChemicalReport GetTestTestChemicalReport(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.TestChemicalReportDB.Where(c => c.ProjectNumber == projectNumber).FirstOrDefault();
            }
        }
        public void InsertTestChemicalReport(TestChemicalReport testreport)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<TestChemicalReport>(testreport);
            }
        }
        public TestChemicalReport SelectTestChemicalReport(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.TestChemicalReportDB.Where(u => u.ID == id).FirstOrDefault();
            }
        }
        public void UpdateTestChemicalReport(TestChemicalReport testreport)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Update<TestChemicalReport>(testreport);
            }
        }
        public void DeleteTestChemicalReport(List<int> ids)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.TestChemicalReportDB.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.TestChemicalReportDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        public void DeleteTestChemicalReport(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.TestChemicalReportDB.Where(u => u.ProjectNumber == projectNumber).ToList().ForEach(a => { dbContext.TestChemicalReportDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        public IEnumerable<TestPhysicalReport> GetTestPhysicalReportList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.TestPhysicalReportDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public void InsertTestPhysicalReport(TestPhysicalReport testreport)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<TestPhysicalReport>(testreport);
            }
        }
        public TestPhysicalReport SelectTestPhysicalReport(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.TestPhysicalReportDB.Where(u => u.ID == id).FirstOrDefault();
            }
        }
        public void UpdateTestPhysicalReport(TestPhysicalReport testreport)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Update<TestPhysicalReport>(testreport);
            }
        }
        public void DeleteTestPhysicalReport(List<int> ids)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.TestPhysicalReportDB.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.TestPhysicalReportDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }


        public IEnumerable<ArgumentValueHistory> GetArgumentValueHistoryList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ArgumentValueHistoryDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }


        public ArgumentValueHistory SelectArgumentValueHistory(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ArgumentValueHistoryDB.Where(u => u.ID == id).FirstOrDefault();
            }
        }
        public void InsertArgumentValueHistory(ArgumentValueHistory ArgumentHistory)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<ArgumentValueHistory>(ArgumentHistory);
            }
        }
        public void UpDateArgumentValueHistory(ArgumentValueHistory ArgumentHistory)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Update<ArgumentValueHistory>(ArgumentHistory);
            }
        }
        public void DeleteArgumentValueHistory(List<int> id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ArgumentValueHistoryDB.Where(u => id.Contains(u.ID)).ToList().ForEach(a => { dbContext.ArgumentValueHistoryDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }


        public ArgumentValue GetArgumentValue(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ArgumentValueDB.Where(c => c.ID == id).SingleOrDefault();
            }
        }
        public List<ArgumentValue> GetArgumentList(string sampleRegisterNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ArgumentValueDB.Where(u => u.SampleRegisterNumber == sampleRegisterNumber).ToList();
            }
        }
        public IEnumerable<ArgumentValue> GetArgumentValueList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                IQueryable<ArgumentValue> ArgumentVal = dbContext.ArgumentValueDB;
                if (!string.IsNullOrEmpty(request.SampleRegisterNumber))
                {
                    ArgumentVal = ArgumentVal.Where(u => u.SampleRegisterNumber.Contains(request.SampleRegisterNumber));
                }
                if (!string.IsNullOrEmpty(request.Argument))
                {
                    ArgumentVal = ArgumentVal.Where(u => u.Argument.Contains(request.Argument));
                }
                return ArgumentVal.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public ArgumentValue SelectArgumentValue(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ArgumentValueDB.Where(u => u.ID == id).FirstOrDefault();
            }
        }
        public void InsertArgumentValue(ArgumentValue Argument)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<ArgumentValue>(Argument);
            }
        }
        public void DeleteArgumentValue(List<int> id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ArgumentValueDB.Where(u => id.Contains(u.ID)).ToList().ForEach(a => { dbContext.ArgumentValueDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        public void UpDateArgumentValue(ArgumentValue Argument)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Update<ArgumentValue>(Argument);
            }
        }
        
        public IEnumerable<ArgumentValue> GetArgumentValueList(string sampleRegisterNumber)
        {
            
            using (var dbContext = new DKLManagerDbContext())
            {             
                return dbContext.ArgumentValueDB.Where(u => u.SampleRegisterNumber == sampleRegisterNumber).ToList(); 
            }
        }

        public IEnumerable<AnalyzePerson> GetAnalyzePersonList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.AnalyzePersonDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public AnalyzePerson SelectAnalyzePerson(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.AnalyzePersonDB.Where(u => u.ID == id).FirstOrDefault();
            }
        }
        public void InsertAnalyzePerson(AnalyzePerson AnalyzePer)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<AnalyzePerson>(AnalyzePer);
            }
        }
        public void DeleteAnalyzePerson(List<int> id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.AnalyzePersonDB.Where(u => id.Contains(u.ID)).ToList().ForEach(a => { dbContext.AnalyzePersonDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        public void UpDateAnalyzePerson(AnalyzePerson AnalyzePer)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Update<AnalyzePerson>(AnalyzePer);
            }
        }



        public IEnumerable<Parameter> GetParameterList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ParameterDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }


        public void InsertParameter(Parameter Param)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<Parameter>(Param);
            }
        }

        public void DeleteParameter(List<int> id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ParameterDB.Where(u => id.Contains(u.ID)).ToList().ForEach(a => { dbContext.ParameterDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        /// <summary>
        /// 设备表改
        /// </summary>
        /// <param name="device"></param>
        public void UpDateParameter(Parameter Param)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Update<Parameter>(Param);
            }
        }
        public Parameter SelectParameter(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ParameterDB.Where(u => u.ID == id).FirstOrDefault();
            }
        }





        public IEnumerable<SampleHistory> GetSampleHistoryList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.SampleHistoryDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }


        public void AddSampleHistoryList(SampleHistory info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Insert<SampleHistory>(info);
                }
            }
        }


        public VisitRecord GetVisitRecord(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.VisitRecordDB.Where(c => c.ID == id).SingleOrDefault();
            }
        }


        public IEnumerable<VisitRecord> GetVisitRecordList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;

            using (var dbContext = new DKLManagerDbContext())
            {
                IQueryable<VisitRecord> VisitRecordDB = dbContext.VisitRecordDB;

                if (!string.IsNullOrEmpty(request.CustomerName))
                {
                    VisitRecordDB = VisitRecordDB.Where(u => u.CustomerName.Contains(request.CustomerName));
                }  
                return VisitRecordDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        
        public void AddVisitRecord(VisitRecord info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Insert<VisitRecord>(info);
                }
            }
        }

        public void UpdateVisitRecord(VisitRecord info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Update<VisitRecord>(info);
                }
            }
        }

        public void DeleteVisitRecord(List<int> ids)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.VisitRecordDB.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.VisitRecordDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        public CustomerModel GetCustomer(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.CustomerDB.Where(c => c.ID == id).SingleOrDefault();
            }
        }
        public IEnumerable<CustomerModel> GetCustomerList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                IQueryable<CustomerModel> CustomerDB = dbContext.CustomerDB;
                if (!string.IsNullOrEmpty(request.CustomerName))
                {
                    CustomerDB = CustomerDB.Where(u => u.CustomerName.Contains (request.CustomerName));
                }
                if (!string.IsNullOrEmpty(request.Tel))
                {
                    CustomerDB = CustomerDB.Where(u => u.Tel.Contains(request.Tel));
                }
                return CustomerDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        
        public void AddCustomer(CustomerModel info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Insert<CustomerModel>(info);
                }
            }
        }

        public void UpdateCustomer(CustomerModel info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Update<CustomerModel>(info);
                }
            }
        }

        public void DeleteCustomer(List<int> ids)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.CustomerDB.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.CustomerDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        public IEnumerable<SampleProjectGist> GetSampleProjectGistList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.SampleGistDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }

        public SampleProjectGist SelectSampleProject(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.SampleGistDB.Where(u => u.ID == id).FirstOrDefault();
            }
        }
        public void InsertSampleProject(SampleProjectGist SamplePro)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<SampleProjectGist>(SamplePro);
            }
        }
        public void DeleteSampleProject(List<int> id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.SampleGistDB.Where(u => id.Contains(u.ID)).ToList().ForEach(a => { dbContext.SampleGistDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        public void UpDateSampleProject(SampleProjectGist SamplePro)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Update<SampleProjectGist>(SamplePro);
            }
        }





        public IEnumerable<DetectionParameter> GetDetectionParameterList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.DetectionParametersDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public DetectionParameter SelectDetectionPar(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.DetectionParametersDB.Where(u => u.ID == id).FirstOrDefault();
            }
        }
        public void InsertDetectionParameter(DetectionParameter DetectionPar)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<DetectionParameter>(DetectionPar);
            }
        }
        public void DeleteDetectionParameter(List<int> id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.DetectionParametersDB.Where(u => id.Contains(u.ID)).ToList().ForEach(a => { dbContext.DetectionParametersDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        public void UpDateDetectionParameter(DetectionParameter DetectionPar)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Update<DetectionParameter>(DetectionPar);
            }
        }

        public SampleRegisterTable GetSampleRegisterTable(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.SampleRegisterDB.Where(c => c.ID == id).FirstOrDefault();
            }
        }

        public SampleRegisterTable SampleRegister(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.SampleRegisterDB.Where(c => c.ID == id).FirstOrDefault();
            }
        }


        public IEnumerable<SampleRegisterTable> GetSampleRegisterTableList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;

            using (var dbContext = new DKLManagerDbContext())
            {
                IQueryable<SampleRegisterTable> AnalyzePersons = dbContext.SampleRegisterDB;
                
                if (request.AnalyzePeople != -1)
                {
                    int m = request.AnalyzePeople;
                    string ad = m.ToString();
                    AnalyzePersons = AnalyzePersons.Where(u => u.AnalyzePeople == (ad));
                }
                return dbContext.SampleRegisterDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public IEnumerable<SampleRegisterTable> GetSampleRegisterTableList(string Name,ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;

            using (var dbContext = new DKLManagerDbContext())
            {
                IQueryable<SampleRegisterTable> AnalyzePersons = dbContext.SampleRegisterDB;

                if (request.AnalyzePeople != -1)
                {
                    int m = request.AnalyzePeople;
                    string ad = m.ToString();
                    AnalyzePersons = AnalyzePersons.Where(u => u.AnalyzePeople == (ad));
                }
                return dbContext.SampleRegisterDB.Where(u => u.AnalyzePeople == Name).OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            
            }
        }
        public SampleRegisterTable SelectSampleRegister(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.SampleRegisterDB.Where(u => u.ID == id).FirstOrDefault();
            }
        }
        public void InsertSampleRegister(SampleRegisterTable SampleRegister)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<SampleRegisterTable>(SampleRegister);
            }
        }
        public void DeleteSampleRegister(string sampleRegisterNumber)
        {
            if (!string.IsNullOrEmpty(sampleRegisterNumber))
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.SampleRegisterDB.Where(u => u.SampleRegisterNumber == sampleRegisterNumber).ToList().ForEach(a => { dbContext.SampleRegisterDB.Remove(a); });
                    dbContext.SaveChanges();
                }
            }
        }

        public void DeleteSampleRegister(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.SampleRegisterDB.Where(u => u.ID == id).ToList().ForEach(a => { dbContext.SampleRegisterDB.Remove(a); });
            }
 
        }
        public void DeleteSampleRegister(List<int> id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.SampleRegisterDB.Where(u => id.Contains(u.ID)).ToList().ForEach(a => { dbContext.SampleRegisterDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        public void UpDateSampleRegister(SampleRegisterTable SampleRegister)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Update<SampleRegisterTable>(SampleRegister);
            }
        }
        public IEnumerable<SampleRegisterTable> GetSampleRegisterTableList(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.SampleRegisterDB.Where(u => u.ID==id);
            }
        }
        public IEnumerable<SampleRegisterTable> GetSampleRegisterTableList(string sampleRegisterNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.SampleRegisterDB.Where(u => u.SampleRegisterNumber == sampleRegisterNumber).ToList();
            }
        }
        public SampleRegisterTable GetSampleRegisterTable(string sampleRegisterNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.SampleRegisterDB.Where(c => c.SampleRegisterNumber == sampleRegisterNumber).FirstOrDefault();
            }
        }
        public SampleRegisterTable GetSampleRegisterTableByProjectNumber(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.SampleRegisterDB.Where(c => c.ProjectNumber == projectNumber).FirstOrDefault();
            }
        }
        public List<SampleRegisterTable> GetSampleRegisterListByProjectNumber(string ProjectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                List<SampleRegisterTable> list = new List<SampleRegisterTable>();
                dbContext.SampleRegisterDB.Where(u => u.ProjectNumber == ProjectNumber).ToList().ForEach(a => { list.Add(a); });
                list =  list.OrderBy(u => u.ParameterName).ToList();
                return list;
                
            }
        }
        public List<SampleProjectGist> GetSampleProjectGistBySampleProject(List<string> str)
        {
            using (var dbContext = new DKLManagerDbContext())
            { 
                List<SampleProjectGist> list = new List<SampleProjectGist>();
                foreach (var item in str)
                {
		             list.Add (dbContext.SampleGistDB.Where(u => u.SampleProject == item ).ToList().SingleOrDefault());
	            }                       
                return list;
            }

        }
        public List<Parameter> GetParameterListBySampleProject(List<string> str, List<string> strc)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                List<Parameter> list = new List<Parameter>();
                foreach (var item in str)
                {
                    list.Add(dbContext.ParameterDB.Where(u => u.ParameterName == item).ToList().SingleOrDefault());
                }
                foreach (var item in strc)
                {
                    list.Add(dbContext.ParameterDB.Where(u => u.ParameterName == item).ToList().SingleOrDefault());
                }
                return list;
            }
        }
     
        public List<Parameter> GetParameterListBySampleProject(List<string>str)          //重载 只负责化学部分
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                List<Parameter> list = new List<Parameter>();
                foreach (var item in str)
                {
                    list.Add(dbContext.ParameterDB.Where(u => u.ParameterName == item).ToList().SingleOrDefault());
                }
             
                return list;
            }
        }

        public IEnumerable<OccupationaldiseaseHarm> GetOccupationaldiseaseHarmList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.OccupationaldiseaseDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public OccupationaldiseaseHarm SelectOccupationaldisease(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.OccupationaldiseaseDB.Where(u => u.ID == id).FirstOrDefault();
            }
        }

        public void InsertOccupationaldisease(OccupationaldiseaseHarm Occupationaldisease)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<OccupationaldiseaseHarm>(Occupationaldisease);
            }
        }
        public void DeleteOccupationaldisease(List<int> id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.OccupationaldiseaseDB.Where(u => id.Contains(u.ID)).ToList().ForEach(a => { dbContext.OccupationaldiseaseDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        public void UpDateOccupationaldisease(OccupationaldiseaseHarm Occupationaldisease)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Update<OccupationaldiseaseHarm>(Occupationaldisease);
            }
        }

        public int GetDeviceCanOrderNumber(string deviceName, DateTime orderDate)
        {
            int totalNumber = GetDeviceList().Where(d => d.DeivceName == deviceName).Count();
            int orderedNumber = 0;
            var items = GetDeviceOrderDetailList().Where(d => (d.OrderDate == orderDate) && (d.DeviceName == deviceName));
            foreach (var item in items)
            {
                orderedNumber += item.OrderNumber;
            }
            return totalNumber - orderedNumber;
        }



        public IEnumerable<DeviceStateModel> GetDeviceStateList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
           {
               return dbContext.DeviceState.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }


        public DeviceStateModel SelectDeviceState(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.DeviceState.Where(u => u.ID == id).FirstOrDefault();
            }
        }
        /// <summary>
        /// 设备状态表增
        /// </summary>
        /// <param name="state"></param>
        public void InsertDeviceState(DeviceStateModel state)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<DeviceStateModel>(state);
            }
        }
        /// <summary>
        /// 设备状态表删
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteDeviceState(List<int> ids)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.DeviceState.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.DeviceState.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        /// <summary>
        /// 设备状态表改
        /// </summary>
        /// <param name="state"></param>
        public void UpDateDeviceState(DeviceStateModel state)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Update<DeviceStateModel>(state);
            }
        }


        /// <summary>
        /// 设备表查
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 

        public IEnumerable<DeviceModel> GetDeviceList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.Device.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }

        /// <summary>
        /// 设备表增
        /// </summary>
        /// <param name="device"></param>
        public void InsertDevice(DeviceModel device)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<DeviceModel>(device);
            }
        }
        /// <summary>
        /// 设备表删
        /// </summary>
        /// <param name="device"></param>
        public void DeleteDevice(List<int> id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Device.Where(u => id.Contains(u.ID)).ToList().ForEach(a => { dbContext.Device.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        /// <summary>
        /// 设备表改
        /// </summary>
        /// <param name="device"></param>
        public void UpDateDevice(DeviceModel device)
        {
            using(var dbContext=new DKLManagerDbContext())
            {
                dbContext.Update<DeviceModel>(device);
            }
        }
        public DeviceModel SelectDevice(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.Device.Where(u => u.ID == id).FirstOrDefault();
            }
        }

        public IEnumerable<DeviceCalibrationRemarkModel> GetCalibretionRemarkList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.DeviceCalibrationRemark.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        /// <summary>
        /// 设备校准表记录表查
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DeviceCalibrationRemarkModel SelectCalibrationRemark(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.DeviceCalibrationRemark.Where(u => u.ID == id).FirstOrDefault();
            }
        }
        /// <summary>
        /// 设备校准记录表增
        /// </summary>
        /// <param name="calibrationRemark"></param>
        public void InsertCalibrationRemark(DeviceCalibrationRemarkModel calibrationRemark)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<DeviceCalibrationRemarkModel>(calibrationRemark);
            }
        }
        /// <summary>
        /// 设备校准记录表删
        /// </summary>
        /// <param name="calibrationRemark"></param>
        public void DelectCalibrationRemark( List<int> id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.DeviceCalibrationRemark.Where(u => id.Contains(u.ID)).ToList().ForEach(a => { dbContext.DeviceCalibrationRemark.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        /// <summary>
        /// 设备校准记录表改
        /// </summary>
        /// <param name="calibrationRemark"></param>
        public void UpdateCalibrationRemark(DeviceCalibrationRemarkModel calibrationRemark)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Update<DeviceCalibrationRemarkModel>(calibrationRemark);
            }
        }
        public IEnumerable<DeviceIdentityRemarkModel> GetIdentityRemarkList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.DeviceIdentityRemark.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        /// <summary>
        /// 设备鉴定记录表查
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DeviceIdentityRemarkModel SelectIdentityRemark(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.DeviceIdentityRemark.Where(u => u.ID == id).FirstOrDefault();
            }
        }
        /// <summary>
        /// 设备鉴定记录表增
        /// </summary>
        /// <param name="identityRemark"></param>
        public void InsertIdentityRemark(DeviceIdentityRemarkModel identityRemark)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<DeviceIdentityRemarkModel>(identityRemark);
            }
        }
        /// <summary>
        /// 设备鉴定记录表删
        /// </summary>
        /// <param name="identityRemark"></param>
        public void DeleteIdentityRemark(List<int> id)
        {
            using(var dbContext=new DKLManagerDbContext())
            {
                dbContext.DeviceIdentityRemark.Where(u => id.Contains(u.ID)).ToList().ForEach(a => { dbContext.DeviceIdentityRemark.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        /// <summary>
        /// 设备鉴定记录表改
        /// </summary>
        /// <param name="identityRemark"></param>
        public void UpdateIdentityRemark(DeviceIdentityRemarkModel identityRemark)
        {
            using(var dbContext=new DKLManagerDbContext())
            {
                dbContext.Update<DeviceIdentityRemarkModel>(identityRemark);
            }
        }

        public IEnumerable<DeviceOrderEntifyModel> GetOrderEntifyList(DeviceRequest request = null)
        {
            request = (request == null) ? new DeviceRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.OrderEntify.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        /// <summary>
        /// 预定申请表查
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DeviceOrderEntifyModel SelectOrderEntify(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.OrderEntify.Where(u => u.ID == id).FirstOrDefault();
            }
        }
        /// <summary>
        /// 预定申请表增
        /// </summary>
        /// <param name="OrderEntify"></param>
        public void InsertOrderEntify(DeviceOrderEntifyModel OrderEntify)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Insert<DeviceOrderEntifyModel>(OrderEntify);
            }
        }
        /// <summary>
        /// 预定申请表删
        /// </summary>
        /// <param name="id"></param>
        public void DeleteOrderEntify(List<int> id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.OrderEntify.Where(u => id.Contains(u.ID)).ToList().ForEach(a => { dbContext.OrderEntify.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        /// <summary>
        /// 预定申请表改
        /// </summary>
        /// <param name="OrderEntify"></param>
        public void UpdateOrderEntify(DeviceOrderEntifyModel OrderEntify)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.Update<DeviceOrderEntifyModel>(OrderEntify);
            }
        }
        public IEnumerable<DeviceServiceRemarkModel> GetSeviceRemarkList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.DeviceService.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        /// <summary>
        /// 设备维修记录表查
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DeviceServiceRemarkModel SelectServiceRemark(int id)
        {
            using(var dbContext=new DKLManagerDbContext())
            {
               return dbContext.DeviceService.Where(u => u.ID == id).FirstOrDefault();
            }
        }
        /// <summary>
        /// 设备维修记录表增
        /// </summary>
        /// <param name="serviceRemark"></param>
        public void InsertServiceRemark(DeviceServiceRemarkModel serviceRemark)
        {
            using(var dbContext=new DKLManagerDbContext())
            {
                dbContext.Insert<DeviceServiceRemarkModel>(serviceRemark);
            }
        }
        /// <summary>
        /// 设备维修记录表删
        /// </summary>
        /// <param name="serviceRemar"></param>
        public void DeleteServiceRemark(List<int> id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.DeviceService.Where(u => id.Contains(u.ID)).ToList().ForEach(a => { dbContext.DeviceService.Remove(a); });
                dbContext.SaveChanges();
            }

        }
        /// <summary>
        /// 设备维修记录表改
        /// </summary>
        /// <param name="serviceRemark"></param>
        public void UpdateServiceRemark(DeviceServiceRemarkModel serviceRemark)
        {
            using(var dbContext=new DKLManagerDbContext())
            {
                dbContext.Update<DeviceServiceRemarkModel>(serviceRemark);
            }
        }
        public IEnumerable<DeviceUseRecordModel> GetUserRecordList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.DeviceUserRecord.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public DeviceUseRecordModel SelectUserRecord(int id)
        {
            using(var dbContext=new DKLManagerDbContext())
            {
                return dbContext.DeviceUserRecord.Where(u => u.ID == id).FirstOrDefault();
            }
        }
        /// <summary>
        /// 设备使用记录表增
        /// </summary>
        /// <param name="userRecord"></param>
        public void InsertUserRecord(DeviceUseRecordModel userRecord)
        {
            using (var dbContext=new DKLManagerDbContext())
            {
                dbContext.Insert<DeviceUseRecordModel>(userRecord);
            }
        }
        /// <summary>
        /// 设备使用记录表删
        /// </summary>
        /// <param name="userRecord"></param>
        public void DeleteUserRecord(List<int> id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.DeviceUserRecord.Where(u => id.Contains(u.ID)).ToList().ForEach(a => { dbContext.DeviceUserRecord.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        /// <summary>
        /// 设备记录表改
        /// </summary>
        /// <param name="userRecord"></param>
        public void UpdateUserRecord(DeviceUseRecordModel userRecord)
        {
            using (var dbContext=new DKLManagerDbContext())
            {
                dbContext.Update<DeviceUseRecordModel>(userRecord);
            }
        }






        public ProjectFile GetProjectFile(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectFileDB.Where(c => c.ID == id).FirstOrDefault();
            }
        }
        public ProjectFile GetProjectFile(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectFileDB.Where(c => c.ProjectNumber == projectNumber).FirstOrDefault();
            }
        }
        public IEnumerable<ProjectFile> GetProjectFileList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectFileDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public void AddProjectFile(ProjectFile info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Insert<ProjectFile>(info);
                }
            }
        }
        public void UpdateProjectFile(ProjectFile info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Update<ProjectFile>(info);
                }
            }
        }
        public void DeleteProjectFile(List<int> ids)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ProjectFileDB.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.ProjectFileDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }

        public void DeleteProjectFile(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ProjectFileDB.Where(u => u.ProjectNumber == projectNumber).ToList().ForEach(a => { dbContext.ProjectFileDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }

        public ProjectDocFile GetProjectDocFile(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectDocFileDB.Where(c => c.ID == id).FirstOrDefault();
            }
        }
        public ProjectDocFile GetProjectDocFile(string projectNumber, int status)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectDocFileDB.Where(c => c.ProjectNumber == projectNumber && c.Status == status).OrderByDescending(c => c.ID).FirstOrDefault();
            }
        }
        public ProjectDocFile GetProjectDocFile(string projectNumber, int status, int idl)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectDocFileDB.Where(c => c.ProjectNumber == projectNumber && c.Status == status).OrderByDescending(c => c.ID).FirstOrDefault();
            }
        }
        public IEnumerable<ProjectDocFile> GetProjectDocFileList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectDocFileDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public IEnumerable<ProjectDocFileHistory> GetProjectDocFileHistoryList(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectDocFileHistoryDB.Where(p => p.ProjectNumber == projectNumber).OrderByDescending(u => u.ID).ToPagedList(1, 20);
            }
        }
        public IEnumerable<ProjectDocFile> GetProjectDocFileList(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectDocFileDB.Where(c => c.ProjectNumber == projectNumber).OrderByDescending(u => u.ID).ToList();
            }
        }

        public void AddProjectDocFile(ProjectDocFile info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Insert<ProjectDocFile>(info);
                }
            }
        }
        public void UpdateProjectDocFile(ProjectDocFile info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Update<ProjectDocFile>(info);
                }
            }
        }
        public void DeleteProjectDocFile(List<int> ids)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ProjectDocFileDB.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.ProjectDocFileDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        public void DeleteProjectDocFile(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ProjectDocFileDB.Where(u => u.ProjectNumber == projectNumber).ToList().ForEach(a => { dbContext.ProjectDocFileDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }



        public ProjectInfo GetProjectInfo(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectInfoDB.Where(c => c.ID == id).FirstOrDefault();
            }
        }

        public ProjectInfo GetProjectInfo(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectInfoDB.Where(c => c.ProjectNumber == projectNumber).FirstOrDefault();
            }
        }

        public List<ProjectInfo> GetProjectInfoList(string projectName)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectInfoDB.Where(u => u.ProjectNumber == projectName).ToList();
            }
        }
        public IEnumerable<ProjectInfo> GetProjectInfoLists( ProjectInfoRequest request)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                IQueryable<ProjectInfo> ProjectDoing = dbContext.ProjectInfoDB;
                return ProjectDoing.Where(u => u.ProjectCheif ==request.userName).OrderBy(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public IEnumerable<ProjectInfo> GetProjectInfoList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                IQueryable<ProjectInfo> ProjectDoing = dbContext.ProjectInfoDB;
                if ((request.ProjectCategory) != -1)
                {
                    if ((request.ProjectCategory) == 3)
                    {
                        ProjectDoing = ProjectDoing.Where(u => (u.ProjectCategory == 1 || u.ProjectCategory == 2));
                    }
                    else
                    {
                        ProjectDoing = ProjectDoing.Where(u => u.ProjectCategory == (request.ProjectCategory));
                    }
                }

                if ((request.ProjectStatus) != -1)
                {
                    if (request.ProjectStatus == (int)EnumProjectSatus.OneTwoThreeAssessor)
                    {
                        ProjectDoing = ProjectDoing.Where(u => (u.ProjectStatus == 10 || u.ProjectStatus == 12 || u.ProjectStatus == 15));
                    }
                    else
                    {
                        ProjectDoing = ProjectDoing.Where(u => u.ProjectStatus == (request.ProjectStatus));
                    }
                }

                if (!string.IsNullOrEmpty(request.ProjectName))
                    ProjectDoing = ProjectDoing.Where(u => u.ProjectName.Contains(request.ProjectName));  
               
                if (!string.IsNullOrEmpty(request.ProjectNumber))
                    ProjectDoing = ProjectDoing.Where(u => u.ProjectNumber.Contains(request.ProjectNumber));

                if (!string.IsNullOrEmpty(request.ProjectCheif))
                    ProjectDoing = ProjectDoing.Where(u => u.ProjectCheif.Contains(request.ProjectCheif));

                if (request.ProjectAlarmStatus == (int)EnumProjectAlarmStatus.Warning)
                    ProjectDoing = ProjectDoing.Where(u => u.ProjectClosingDate >= DateTime.Now);

                if (request.ProjectAlarmStatus == (int)EnumProjectAlarmStatus.Alert)
                    ProjectDoing = ProjectDoing.Where(u => u.ProjectClosingDate < DateTime.Now);

                return ProjectDoing.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);

            }    
        }
        public IEnumerable<ProjectInfo> GetProjectInfoList(string Name, ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                IQueryable<ProjectInfo> ProjectDoing = dbContext.ProjectInfoDB;
                if ((request.ProjectCategory) != -1)
                {
                    if ((request.ProjectCategory) == 3)
                    {
                        ProjectDoing = ProjectDoing.Where(u => (u.ProjectCategory == 1 || u.ProjectCategory == 2));
                    }
                    else
                    {
                        ProjectDoing = ProjectDoing.Where(u => u.ProjectCategory == (request.ProjectCategory));
                    }
                }

                if ((request.ProjectStatus) != -1)
                {
                    if (request.ProjectStatus == (int)EnumProjectSatus.OneTwoThreeAssessor)
                    {
                        ProjectDoing = ProjectDoing.Where(u => (u.ProjectStatus == 10 || u.ProjectStatus == 12 || u.ProjectStatus == 15));
                    }
                    else
                    {
                        ProjectDoing = ProjectDoing.Where(u => u.ProjectStatus == (request.ProjectStatus));
                    }
                }

                if (!string.IsNullOrEmpty(request.ProjectName))
                    ProjectDoing = ProjectDoing.Where(u => u.ProjectName.Contains(request.ProjectName));

                if (!string.IsNullOrEmpty(request.ProjectNumber))
                    ProjectDoing = ProjectDoing.Where(u => u.ProjectNumber.Contains(request.ProjectNumber));

                if (!string.IsNullOrEmpty(request.ProjectCheif))
                    ProjectDoing = ProjectDoing.Where(u => u.ProjectCheif.Contains(request.ProjectCheif));

                if (request.ProjectAlarmStatus == (int)EnumProjectAlarmStatus.Warning)
                    ProjectDoing = ProjectDoing.Where(u => u.ProjectClosingDate >= DateTime.Now);

                if (request.ProjectAlarmStatus == (int)EnumProjectAlarmStatus.Alert)
                    ProjectDoing = ProjectDoing.Where(u => u.ProjectClosingDate < DateTime.Now);

                return ProjectDoing.Where(u => u.ProjectLeader == Name).Where(u => u.ProjectStatus ==5).OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);

            }
        }
        public IEnumerable<ProjectInfo> GetProjectInfoList(string name, bool isLeader = false, ProjectInfoRequest request = null)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                if (isLeader)//组长
                {
                    return dbContext.ProjectInfoDB.Where(u => (u.ProjectLeader == name) || (u.ProjectPerson.Contains(name))).OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
                }
                else //负责人
                {
                    return dbContext.ProjectInfoDB.Where(u => (u.ProjectCheif == name) || (u.ProjectPerson.Contains(name))).OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
                }
                //return dbContext.ProjectInfoDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
           }
        }
        public void AddProjectInfo(ProjectInfo info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    var existpro = dbContext.FindAll<ProjectInfo>(u => u.ProjectNumber == info.ProjectNumber);
                    if (existpro.Count > 0)
                    {
                        throw new BusinessException("ProjectNumber", "此项目编号已存在！");
                    }
                    else
                    {
                        dbContext.Insert<ProjectInfo>(info);
                    }
                }
            }
        }
        public void UpdateProjectInfo(ProjectInfo info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Update<ProjectInfo>(info);
                }
            }
        }
        public void DeleteProjectInfo(List<int> ids)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ProjectInfoDB.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.ProjectInfoDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }

        public void DeleteProjectInfo(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ProjectInfoDB.Where(u => u.ProjectNumber == projectNumber).ToList().ForEach(a => { dbContext.ProjectInfoDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }


        public ValueBasicInfo GetVlaueProjectBasicInfo(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ValueProjectInfoDB.Where(c => c.ID == id).FirstOrDefault();
            }
        }

        public ValueBasicInfo GetVlaueProjectBasicInfo(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ValueProjectInfoDB.Where(c => c.ProjectNumber == projectNumber).FirstOrDefault();
            }
        }
        public IEnumerable<ValueBasicInfo> GetVlaueProjectBasicInfoList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ValueProjectInfoDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public void AddVlaueProjectBasicInfo(ValueBasicInfo info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Insert<ValueBasicInfo>(info);
                }
            }
        }
        public void UpdateVlaueProjectBasicInfo(ValueBasicInfo info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Update<ValueBasicInfo>(info);
                }
            }
        }
        public void DeleteVlaueProjectBasicInfo(List<int> ids)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ValueProjectInfoDB.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.ValueProjectInfoDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        public void DeleteVlaueProjectBasicInfo(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ValueProjectInfoDB.Where(u => u.ProjectNumber == projectNumber).ToList().ForEach(a => { dbContext.ValueProjectInfoDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }


        public TestBasicInfo GetProjectTestBasicInfo(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.TestBasicInfoDB.Where(c => c.ID == id).FirstOrDefault();
            }
        }
        public TestBasicInfo GetProjectTestBasicInfo(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.TestBasicInfoDB.Where(c => c.ProjectNumber == projectNumber).FirstOrDefault();
            }
        }
        public IEnumerable<TestBasicInfo> GetProjectTestBasicInfoList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                IQueryable<TestBasicInfo> ProjectTestBasicInfoList = dbContext.TestBasicInfoDB;
                
                if(!string.IsNullOrEmpty(request.ProjectNumber))//检测人员 submit确认信息，需要根据编号获取检测数据
                {
                    ProjectTestBasicInfoList = ProjectTestBasicInfoList.Where(p => p.ProjectNumber == request.ProjectNumber);
                }
                return ProjectTestBasicInfoList.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public void AddProjectTestBasicInfo(TestBasicInfo info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Insert<TestBasicInfo>(info);
                }
            }
        }
        public void UpdateProjectTestBasicInfo(TestBasicInfo info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Update<TestBasicInfo>(info);
                }
            }
        }
        public void DeleteProjectTestBasicInfo(List<int> ids)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.TestBasicInfoDB.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.TestBasicInfoDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }

        public void DeleteProjectTestBasicInfo(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.TestBasicInfoDB.Where(u => u.ProjectNumber == projectNumber).ToList().ForEach(a => { dbContext.TestBasicInfoDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }


        public IEnumerable<ConsultBasicInfo> GetConsultBasicInfoList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ConsultBasicInfoDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }

        public void AddConsultBasicInfo(ConsultBasicInfo info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Insert<ConsultBasicInfo>(info);
                }
            }
        }

        public void UpdateConsultBasicInfo(ConsultBasicInfo info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Update<ConsultBasicInfo>(info);
                }
            }
        }

        public void DeleteConsultBasicInfo(List<int> ids)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ConsultBasicInfoDB.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.ConsultBasicInfoDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }

        public void DeleteConsultBasicInfo(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ConsultBasicInfoDB.Where(u => u.ProjectNumber == projectNumber).ToList().ForEach(a => { dbContext.ConsultBasicInfoDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }


        public ConsultBasicInfo GetConsultBasicInfo(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ConsultBasicInfoDB.Where(c => c.ID == id).FirstOrDefault();
            }
        }
        public ConsultBasicInfo GetConsultBasicInfo(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ConsultBasicInfoDB.Where(c => c.ProjectNumber == projectNumber).FirstOrDefault();
            }
        }

        //历史记录表以下

        public List<TestChemicalReportListHistory> GetTestChemicalReportListHistory(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                IQueryable<TestChemicalReportListHistory> TestTestChemicalReport = dbContext.TestChemicalReportListHistoryDB;

                if (!string.IsNullOrEmpty(projectNumber))
                {
                    TestTestChemicalReport = TestTestChemicalReport.Where(u => u.ProjectNumber == projectNumber);
                }
                return TestTestChemicalReport.OrderByDescending(u => u.ID).ToList();
            }
        }
        public void AddTestChemicalReportListHistory(TestChemicalReportListHistory info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Insert<TestChemicalReportListHistory>(info);
                }
            }
        }
        public List<TestBasicInfoHistory> GetTestBasicInfoListHistory(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                IQueryable<TestBasicInfoHistory> TestBasicInfo = dbContext.TestBasicInfoHistoryDB;

                if (!string.IsNullOrEmpty(projectNumber))
                {
                    TestBasicInfo = TestBasicInfo.Where(u => u.ProjectNumber == projectNumber);
                }
                return TestBasicInfo.OrderByDescending(u => u.ID).ToList();
            }
        }
        public IEnumerable<TestBasicInfoHistory> GetProjectTestBasicInfoHistoryList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.TestBasicInfoHistoryDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }

        public void AddProjectTestBasicInfoHistory(TestBasicInfoHistory info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Insert<TestBasicInfoHistory>(info);
                }
            }
        }

        public IEnumerable<ValueBasicInfoHistory> GetVlaueProjectBasicInfoHistoryList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ValueBasicInfoHistoryDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }

        public void AddVlaueProjectBasicInfoHistory(ValueBasicInfoHistory info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Insert<ValueBasicInfoHistory>(info);
                }
            }
        }


        public IEnumerable<ConsultBasicInfoHistory> GetConsultBasicInfoHistoryList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ConsultBasicInfoHistoryDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }

        public void AddConsultBasicInfoHistory(ConsultBasicInfoHistory info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Insert<ConsultBasicInfoHistory>(info);
                }
            }
        }

        public ProjectInfoHistory GetProjectInfoHistory(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectInfoHistoryDB.Where(c => c.ID == id).FirstOrDefault();
            }
        }
        
        public IEnumerable<ProjectInfoHistory> GetProjectInfoHistoryList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                IQueryable<ProjectInfoHistory> ProjecInfo = dbContext.ProjectInfoHistoryDB;
                if ((request.ProjectCategory) != -1)
                {
                    ProjecInfo = ProjecInfo.Where(u => u.ProjectCategory == (request.ProjectCategory));
                }
                if (!string.IsNullOrEmpty(request.ProjectName))
                {
                    ProjecInfo = ProjecInfo.Where(u => u.ProjectName.Contains(request.ProjectName));
                }
                if (!string.IsNullOrEmpty(request.ProjectCheif))
                {
                    ProjecInfo = ProjecInfo.Where(u => u.ProjectName.Contains(request.ProjectCheif));
                }              
               if (!string.IsNullOrEmpty(request.ProjectNumber))
               {
                   ProjecInfo = ProjecInfo.Where(u => u.ProjectNumber.Contains(request.ProjectNumber));
               }

               return ProjecInfo.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
           }
        }
        public void AddProjectInfoHistory(ProjectInfoHistory info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Insert<ProjectInfoHistory>(info);
                }
            }
        }
        public ProjectFileHistory GetProjectFileHistory(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectFileHistoryDB.Where(c => c.ID == id).FirstOrDefault();
            }
        }
        public ProjectFileHistory GetProjectFileHistory(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectFileHistoryDB.Where(c => c.ProjectNumber == projectNumber).FirstOrDefault();
            }
        }
        public IEnumerable<ProjectFileHistory> GetProjectFileHistoryList(ProjectInfoRequest request = null)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectFileHistoryDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public void AddProjectFileHistory(ProjectFileHistory info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Insert<ProjectFileHistory>(info);
                }
            }
        }
        public void UpdateProjectFileHistory(ProjectFileHistory info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Update<ProjectFileHistory>(info);
                }
            }
        }
        public void DeleteProjectFileHistory(List<int> ids)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ProjectFileHistoryDB.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.ProjectFileHistoryDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        public void DeleteProjectFileHistory(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ProjectFileHistoryDB.Where(u => u.ProjectNumber == projectNumber).ToList().ForEach(a => { dbContext.ProjectFileHistoryDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }

        public List<ProjectDocFileHistory> GetProjectDocHistoryList(string Year)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                var year = Convert.ToInt32(Year);
                List<ProjectDocFileHistory> list = new List<ProjectDocFileHistory>();
                dbContext.ProjectDocFileHistoryDB.Where(u => u.CreateTime.Year == year).ToList().ForEach(a => { list.Add(a); });
                return list;
            }
        }
        public ProjectDocFileHistory GetProjectDocFileHistory(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectDocFileHistoryDB.Where(c => c.ID == id).FirstOrDefault();
            }
        }
        public ProjectDocFileHistory GetProjectDocFileHistory(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectDocFileHistoryDB.Where(c => c.ProjectNumber == projectNumber).FirstOrDefault();
            }
        }
        public IEnumerable<ProjectDocFileHistory> GetProjectDocFileHistoryList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectDocFileHistoryDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public void AddProjectDocFileHistory(ProjectDocFileHistory info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Insert<ProjectDocFileHistory>(info);
                }
            }
        }
        public void UpdateProjectDocFileHistory(ProjectDocFileHistory info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Update<ProjectDocFileHistory>(info);
                }
            }
        }
        public void DeleteProjectDocFileHistory(List<int> ids)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ProjectDocFileHistoryDB.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.ProjectDocFileHistoryDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        public void DeleteProjectDocFileHistory(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ProjectDocFileHistoryDB.Where(u => u.ProjectNumber == projectNumber).ToList().ForEach(a => { dbContext.ProjectDocFileHistoryDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        //项目审核表操作
        public ProjectChecker GetProjectChecker(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectCheckerDB.Where(c => c.ID == id).FirstOrDefault();
            }
        }
        public ProjectChecker GetProjectChecker(string checkerName)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectCheckerDB.Where(c => c.Name == checkerName).FirstOrDefault();
            }
        }
        public IEnumerable<ProjectChecker> GetProjectCheckerList(ProjectInfoRequest request = null)
        {
            request = (request == null) ? new ProjectInfoRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectCheckerDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public IEnumerable<ProjectChecker> GetProjectCheckerList(string checkerName, ProjectInfoRequest request = null)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.ProjectCheckerDB.Where(c => c.Name == checkerName).OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public void AddProjectChecker(ProjectChecker info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Insert<ProjectChecker>(info);
                }
            }
        }
        public void UpdateProjectChecker(ProjectChecker info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Update<ProjectChecker>(info);
                }
            }
        }
        public void DeleteProjectChecker(List<int> ids)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                dbContext.ProjectCheckerDB.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.ProjectCheckerDB.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        public DeviceOrderInfo GetDeviceOrderInfo(string projectNumber)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.DeviceOrderInfoDB.Where(c => c.ProjectNumber == projectNumber).SingleOrDefault();
            }
        }
        public DeviceOrderInfo GetDeviceOrderInfo(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.DeviceOrderInfoDB.Where(c => c.ID == id).SingleOrDefault();
            }
        }
        public IEnumerable<DeviceOrderInfo> GetDeviceOrderInfoList(DeviceRequest request = null)
        {
            request = (request == null) ? new DeviceRequest() : request;

            using (var dbContext = new DKLManagerDbContext())
            {
                IQueryable<DeviceOrderInfo> DeviceOrderInfoDB = dbContext.DeviceOrderInfoDB;

                if (!string.IsNullOrEmpty(request.ProjectNumber))
                {
                    DeviceOrderInfoDB = DeviceOrderInfoDB.Where(u => u.ProjectNumber.Contains(request.ProjectNumber));
                }
                if (!string.IsNullOrEmpty(request.OrderPerson))
                {
                    DeviceOrderInfoDB = DeviceOrderInfoDB.Where(u => u.OrderPerson.Contains(request.OrderPerson));
                }
                if (request.OrderState != 0)
                {
                    DeviceOrderInfoDB = DeviceOrderInfoDB.Where(u => u.OrderState == request.OrderState);
                }
                if (request.OrderDate != DateTime.MinValue)
                {
                    DeviceOrderInfoDB = DeviceOrderInfoDB.Where(u => u.OrderDate == request.OrderDate);
                }

                return DeviceOrderInfoDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public void AddDeviceOrderInfo(DeviceOrderInfo info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    var existpro = dbContext.FindAll<DeviceOrderInfo>(u => u.ProjectNumber == info.ProjectNumber);
                    if (existpro.Count > 0)
                    {
                        throw new BusinessException("ProjectNumber", "此项目编号已存在！");
                    }
                    else
                    {
                        dbContext.Insert<DeviceOrderInfo>(info);
                    }
                }
            }
        }
        public void UpdateDeviceOrderInfo(DeviceOrderInfo info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Update<DeviceOrderInfo>(info);
                }
            }
        }
        public void DeleteDeviceOrderInfo(List<int> ids)
        {
            if (ids != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.DeviceOrderInfoDB.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.DeviceOrderInfoDB.Remove(a); });
                    dbContext.SaveChanges();
                }
            }
        }
        public void DeleteDeviceOrderInfo(string projectNumber)
        {
            if (!string.IsNullOrEmpty(projectNumber))
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.DeviceOrderInfoDB.Where(u => u.ProjectNumber == projectNumber).ToList().ForEach(a => { dbContext.DeviceOrderInfoDB.Remove(a); });
                    dbContext.SaveChanges();
                }
            }
        }
        public DeviceOrderDetail GetDeviceOrderDetail(int id)
        {
            using (var dbContext = new DKLManagerDbContext())
            {
                return dbContext.DeviceOrderDetailDB.Where(c => c.ID == id).SingleOrDefault();
            }
        }
        public IEnumerable<DeviceOrderDetail> GetDeviceOrderDetailList(DeviceRequest request = null)
        {
            request = (request == null) ? new DeviceRequest() : request;
            using (var dbContext = new DKLManagerDbContext())
            {
                IQueryable<DeviceOrderDetail> DeviceOrderDetailDB = dbContext.DeviceOrderDetailDB;
                if (!string.IsNullOrEmpty(request.ProjectNumber))
                {
                    DeviceOrderDetailDB = DeviceOrderDetailDB.Where(u => u.ProjectNumber.Contains(request.ProjectNumber));
                }
                return DeviceOrderDetailDB.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public void AddDeviceOrderDetail(DeviceOrderDetail info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Insert<DeviceOrderDetail>(info);
                }
            }
        }
        public void UpdateDeviceOrderDetail(DeviceOrderDetail info)
        {
            if (info != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.Update<DeviceOrderDetail>(info);
                }
            }
        }
        public void DeleteDeviceOrderDetail(List<int> ids)
        {
            if (ids != null)
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.DeviceOrderDetailDB.Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { dbContext.DeviceOrderDetailDB.Remove(a); });
                    dbContext.SaveChanges();
                }
            }
        }
        public void DeleteDeviceOrderDetail(string projectNumber)
        {
            if (!string.IsNullOrEmpty(projectNumber))
            {
                using (var dbContext = new DKLManagerDbContext())
                {
                    dbContext.DeviceOrderDetailDB.Where(u => u.ProjectNumber == projectNumber).ToList().ForEach(a => { dbContext.DeviceOrderDetailDB.Remove(a); });
                    dbContext.SaveChanges();
                }
            }
        }
  
    }

}




       
































    