using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HYZK.FrameWork.DAL;
using HYZK.Core.Config;
using HYZK.Core.Log;
using System.Data.Entity;
using DKLManager.Contract.Model;

namespace DKLManager.Dal
{
    public class DKLManagerDbContext : DbContextBase
    {


        public DKLManagerDbContext()
            : base(CachedConfigContext.Current.DaoConfig.DKLManager)
        {
            //1.       CreateDatabaseIfNotExists：这是默认的策略。如果数据库不存在，那么就创建数据库。但是如果数据库存在了，而且实体发生了变化，就会出现异常。
            //2.       DropCreateDatabaseIfModelChanges：此策略表明，如果模型变化了，数据库就会被重新创建，原来的数据库被删除掉了。
            //3.       DropCreateDatabaseAlways：此策略表示，每次运行程序都会重新创建数据库，这在开发和调试的时候非常有用。
            //4.       自定制数据库策略：可以自己实现IDatabaseInitializer来创建自己的策略。或者从已有的实现了IDatabaseInitializer接口的类派生。
             Database.SetInitializer<DKLManagerDbContext>(null);
          //     Database.SetInitializer<DKLManagerDbContext>(new DropCreateDatabaseAlways<DKLManagerDbContext>());

        }
        public DbSet<DetectionResult> DetectionResultDB { get; set; }
        public DbSet<TestChemicalReportListHistory> TestChemicalReportListHistoryDB { get; set; }
        public DbSet<TestChemicalReport> TestChemicalReportDB { get; set; }
        public DbSet<TestPhysicalReport> TestPhysicalReportDB { get; set; }
        public DbSet<ArgumentValueHistory> ArgumentValueHistoryDB { get; set; }
        public DbSet<ArgumentValue> ArgumentValueDB { get; set; }
        public DbSet<AnalyzePerson> AnalyzePersonDB { get; set; }
        public DbSet<Parameter> ParameterDB { get; set; }
        public DbSet<SampleHistory > SampleHistoryDB { get; set; }
        public DbSet<SampleRegisterTable> SampleRegisterDB { get; set; }
        public DbSet<DetectionParameter> DetectionParametersDB { get; set; }
        public DbSet<SampleProjectGist> SampleGistDB { get; set; }
        public DbSet<OccupationaldiseaseHarm> OccupationaldiseaseDB { get; set; }

        public DbSet<DeviceStateModel> DeviceState { get; set; }
        public DbSet<DeviceOrderEntifyModel> OrderEntify { get; set; }
        public DbSet<DeviceUseRecordModel> DeviceUserRecord { get; set; }
        public DbSet<DeviceIdentityRemarkModel> DeviceIdentityRemark { get; set; }
        public DbSet<DeviceCalibrationRemarkModel> DeviceCalibrationRemark { get; set; }
        public DbSet<DeviceModel> Device { get; set; }
        public DbSet<DeviceServiceRemarkModel> DeviceService{get;set;}


        public DbSet<ProjectDocFile> ProjectDocFileDB { get; set; }
        public DbSet<ProjectFile> ProjectFileDB { get; set; }
        public DbSet<ProjectInfo> ProjectInfoDB { get; set; }
        public DbSet<ValueBasicInfo> ValueProjectInfoDB { get; set; }
        public DbSet<TestBasicInfo> TestBasicInfoDB { get; set; }
        public DbSet<ConsultBasicInfo> ConsultBasicInfoDB { get; set; }


        public DbSet<ProjectDocFileHistory> ProjectDocFileHistoryDB { get; set; }
        public DbSet<ProjectFileHistory> ProjectFileHistoryDB { get; set; }

        public DbSet<ProjectInfoHistory> ProjectInfoHistoryDB { get; set; }
        public DbSet<ValueBasicInfoHistory> ValueBasicInfoHistoryDB { get; set; }
        public DbSet<TestBasicInfoHistory> TestBasicInfoHistoryDB { get; set; }
        public DbSet<ConsultBasicInfoHistory> ConsultBasicInfoHistoryDB { get; set; }
        public DbSet<ProjectChecker> ProjectCheckerDB { get; set; }
        public DbSet<CustomerModel> CustomerDB { get; set; }
        public DbSet<VisitRecord> VisitRecordDB { get; set; }
        public DbSet<DeviceOrderInfo> DeviceOrderInfoDB { get; set; }
        public DbSet<DeviceOrderDetail> DeviceOrderDetailDB { get; set; }

    }
}


    

