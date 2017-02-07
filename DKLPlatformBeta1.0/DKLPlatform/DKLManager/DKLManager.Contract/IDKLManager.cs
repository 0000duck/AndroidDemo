using DKLManager.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKLManager.Contract
{

    public interface IDKLManager
    {
       
        List<ProjectInfo> GetProjectInfos(string Year);
        List<ProjectInfoHistory> GetProjectInfosHistory(string Year);
        List<Parameter> GetParameterListBySampleProject(List<string> str); 
        List<Parameter> GetParameterListBySampleProject(List<string>str,List<string> strc);            //获取检测依据集合（物理和化学）
        List<SampleProjectGist> GetSampleProjectGistBySampleProject(List<string> str);  //获取采样依据model集合
       List<SampleRegisterTable> GetSampleRegisterListByProjectNumber(string ProjectNumber);   //获取采样项目集合
        IEnumerable<DetectionResult> GetDetectionResultList(ProjectInfoRequest request = null);
        void InsertDetectionResult(DetectionResult DeteResult);
        void DeleteDetectionResult(List<int> id);
        void UpDateDetectionResult(DetectionResult DeteResult);
        DetectionResult SelectDetectionResult(int id);

        IEnumerable<TestChemicalReport> GetTestTestChemicalReportList(ProjectInfoRequest request = null);
        List<TestChemicalReport> GetTestChemicalReportByProjectNumber(string ProjectNumber);
        TestChemicalReport GetTestTestChemicalReport(string projectNumber);
        void InsertTestChemicalReport(TestChemicalReport testreport);
        TestChemicalReport SelectTestChemicalReport(int id);
        void UpdateTestChemicalReport(TestChemicalReport testreport);
        void DeleteTestChemicalReport(List<int> ids);
        void DeleteTestChemicalReport(string projectNumber);
        //自动生成报告项目名称
        IEnumerable<TestPhysicalReport> GetTestReportProjectList(ProjectInfoRequest request = null);
        //自动生成检测报告表
        IEnumerable<TestPhysicalReport> GetTestPhysicalReportList(ProjectInfoRequest request = null);
        void InsertTestPhysicalReport(TestPhysicalReport testreport);
        TestPhysicalReport SelectTestPhysicalReport(int id);
        void UpdateTestPhysicalReport(TestPhysicalReport testreport);
        void DeleteTestPhysicalReport(List<int> ids);


        List<string> GetTestList(string projectName);   //获得检测项目（物理部分）
        List<string> GetSampleList(string projectName);       //获得采样项目（化学部分）
        List<ProjectInfo> GetProjectInfoModels(string projectName);  //项目信息
        List<TestPhysicalReport> GetPhysicalModels(string projectName);
        List<TestChemicalReport> GetChemicalReport(string projectName);


        IEnumerable<ArgumentValueHistory> GetArgumentValueHistoryList(ProjectInfoRequest request = null);
        void InsertArgumentValueHistory(ArgumentValueHistory ArgumentHistory);
        void DeleteArgumentValueHistory(List<int> id);
        void UpDateArgumentValueHistory(ArgumentValueHistory ArgumentHistory);
        ArgumentValueHistory SelectArgumentValueHistory(int id);

        ArgumentValue GetArgumentValue(int id);
        List<ArgumentValue> GetArgumentList(string sampleRegisterNumber);
        IEnumerable<ArgumentValue> GetArgumentValueList(ProjectInfoRequest request = null);
        void InsertArgumentValue(ArgumentValue Argument);
        void DeleteArgumentValue(List<int> id);
        void UpDateArgumentValue(ArgumentValue Argument);
        ArgumentValue SelectArgumentValue(int id);
        IEnumerable<ArgumentValue> GetArgumentValueList(string sampleRegisterNumber);

        /// <summary>
        /// 分析人员
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        IEnumerable<AnalyzePerson> GetAnalyzePersonList(ProjectInfoRequest request = null);
        void InsertAnalyzePerson(AnalyzePerson AnalyzePer);
        void DeleteAnalyzePerson(List<int> id);
        void UpDateAnalyzePerson(AnalyzePerson AnalyzePer);
        AnalyzePerson SelectAnalyzePerson(int id);


        IEnumerable<Parameter> GetParameterList(ProjectInfoRequest request = null);
        void InsertParameter(Parameter Param);
        void DeleteParameter(List<int> id);
        void UpDateParameter(Parameter Param);
        Parameter SelectParameter(int id);



        IEnumerable<SampleHistory> GetSampleHistoryList(ProjectInfoRequest request = null);
        void AddSampleHistoryList(SampleHistory info);



        IEnumerable<OccupationaldiseaseHarm> GetOccupationaldiseaseHarmList(ProjectInfoRequest request = null);
        void InsertOccupationaldisease(OccupationaldiseaseHarm Occupationaldisease);
        void DeleteOccupationaldisease(List<int> id);
        void UpDateOccupationaldisease(OccupationaldiseaseHarm Occupationaldisease);
        OccupationaldiseaseHarm SelectOccupationaldisease(int id);

        SampleRegisterTable GetSampleRegisterTable(int id);
        void DeleteSampleRegister(string sampleRegisterNumber);
        IEnumerable<SampleRegisterTable> GetSampleRegisterTableList(ProjectInfoRequest request = null);
        IEnumerable<SampleRegisterTable> GetSampleRegisterTableList(string Name, ProjectInfoRequest request = null);
        List<SampleRegisterTable> GetSampleRegisterTableListEdit(string projectNumber);
        void InsertSampleRegister(SampleRegisterTable SampleRegister);
        void DeleteSampleRegister(List<int> id);
        void UpDateSampleRegister(SampleRegisterTable SampleRegister);
        SampleRegisterTable SelectSampleRegister(int id);
        IEnumerable<SampleRegisterTable> GetSampleRegisterTableList(string sampleRegisterNumber);
        SampleRegisterTable GetSampleRegisterTable(string sampleRegisterNumber);
        SampleRegisterTable GetSampleRegisterTableByProjectNumber(string projectNumber);


        IEnumerable<DetectionParameter> GetDetectionParameterList(ProjectInfoRequest request = null);
        void InsertDetectionParameter(DetectionParameter DetectionPar);
        void DeleteDetectionParameter(List<int> id);
        void UpDateDetectionParameter(DetectionParameter DetectionPar);
        DetectionParameter SelectDetectionPar(int id);


        IEnumerable<SampleProjectGist> GetSampleProjectGistList(ProjectInfoRequest request = null);
        void InsertSampleProject(SampleProjectGist SamplePro);
        void DeleteSampleProject(List<int> id);
        void UpDateSampleProject(SampleProjectGist SamplePro);
        SampleProjectGist SelectSampleProject(int id);

        int GetDeviceCanOrderNumber(string device, DateTime orderDate);
        /// <summary>
        /// 设备状态表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<DeviceStateModel> GetDeviceStateList(ProjectInfoRequest request = null);       
        void InsertDeviceState(DeviceStateModel state);
        void DeleteDeviceState(List<int> id);
        void UpDateDeviceState(DeviceStateModel state);
        DeviceStateModel SelectDeviceState(int id);


        IEnumerable<DeviceModel> GetDeviceList(ProjectInfoRequest request = null);
        /// <summary>
        /// 设备表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void InsertDevice(DeviceModel device);
        void DeleteDevice(List<int> id);
        void UpDateDevice(DeviceModel device);
        DeviceModel SelectDevice(int id);



        IEnumerable<DeviceCalibrationRemarkModel> GetCalibretionRemarkList(ProjectInfoRequest request = null);
        /// <summary>
        /// 设备校准表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void InsertCalibrationRemark(DeviceCalibrationRemarkModel devicecalibrationremark);
        void DelectCalibrationRemark(List<int> id);
        void UpdateCalibrationRemark(DeviceCalibrationRemarkModel calibrationRemar);
        DeviceCalibrationRemarkModel SelectCalibrationRemark(int id);



        IEnumerable<DeviceIdentityRemarkModel> GetIdentityRemarkList(ProjectInfoRequest request = null);
        /// <summary>
        /// 设备鉴定表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void InsertIdentityRemark(DeviceIdentityRemarkModel identityRemark);
        void DeleteIdentityRemark(List<int> id);
        void UpdateIdentityRemark(DeviceIdentityRemarkModel identityRemark);
        DeviceIdentityRemarkModel SelectIdentityRemark(int id);



        IEnumerable<DeviceOrderEntifyModel> GetOrderEntifyList(DeviceRequest request = null);
        /// <summary>
        /// 预约申请表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void InsertOrderEntify(DeviceOrderEntifyModel orderEntify);
        void DeleteOrderEntify(List<int> id);
        void UpdateOrderEntify(DeviceOrderEntifyModel orderEntify);
        DeviceOrderEntifyModel SelectOrderEntify(int id);

        DeviceOrderInfo GetDeviceOrderInfo(string projectNumber);
        DeviceOrderInfo GetDeviceOrderInfo(int id);
        IEnumerable<DeviceOrderInfo> GetDeviceOrderInfoList(DeviceRequest request = null);

        void AddDeviceOrderInfo(DeviceOrderInfo info);

        void UpdateDeviceOrderInfo(DeviceOrderInfo info);

        void DeleteDeviceOrderInfo(List<int> ids);
        void DeleteDeviceOrderInfo(string projectNumber);




        IEnumerable<DeviceServiceRemarkModel> GetSeviceRemarkList(ProjectInfoRequest request = null);
        /// <summary>
        /// 设备维修表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void InsertServiceRemark(DeviceServiceRemarkModel serviceRemark);
        void DeleteServiceRemark(List<int> id);
        void UpdateServiceRemark(DeviceServiceRemarkModel serviceremark);
        DeviceServiceRemarkModel SelectServiceRemark(int id);




        IEnumerable<DeviceUseRecordModel> GetUserRecordList(ProjectInfoRequest request = null);
        /// <summary>
        /// 设备使用表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        void InsertUserRecord(DeviceUseRecordModel useRecord);
        void DeleteUserRecord(List<int> id);
        void UpdateUserRecord(DeviceUseRecordModel useRecord);
        DeviceUseRecordModel SelectUserRecord(int id);


        ProjectFile GetProjectFile(int id);
        ProjectFile GetProjectFile(string projectNumber);
        IEnumerable<ProjectFile> GetProjectFileList(ProjectInfoRequest request = null);
        void AddProjectFile(ProjectFile file);
        void UpdateProjectFile(ProjectFile file);
        void DeleteProjectFile(List<int> ids);
        void DeleteProjectFile(string projectNumber);


        ProjectDocFile GetProjectDocFile(int id);
        ProjectDocFile GetProjectDocFile(string projectNumber, int status);
        ProjectDocFile GetProjectDocFile(string projectNumber, int status,int id = 1);
        IEnumerable<ProjectDocFile> GetProjectDocFileList(ProjectInfoRequest request = null);
        IEnumerable<ProjectDocFile> GetProjectDocFileList(string projectNumber);
        void AddProjectDocFile(ProjectDocFile file);
        void UpdateProjectDocFile(ProjectDocFile file);
        void DeleteProjectDocFile(List<int> ids);
        void DeleteProjectDocFile(string projectNumber);
         
        ProjectInfo GetProjectInfo(int id);
        ProjectInfo GetProjectInfo(string projectNumber);
        List<ProjectInfo> GetProjectInfoList(string projectName);
        IEnumerable<ProjectInfo> GetProjectInfoList(ProjectInfoRequest request = null);
        IEnumerable<ProjectInfo> GetProjectInfoList(string Name,ProjectInfoRequest request = null);
        IEnumerable<ProjectInfo> GetProjectInfoLists( ProjectInfoRequest request);
        IEnumerable<ProjectInfo> GetProjectInfoList(string name, bool isLeader = false, ProjectInfoRequest request = null);
        void AddProjectInfo(ProjectInfo info);
        void UpdateProjectInfo(ProjectInfo info);
        void DeleteProjectInfo(List<int> ids);
        void DeleteProjectInfo(string projectNumber);



        IEnumerable<ConsultBasicInfo> GetConsultBasicInfoList(ProjectInfoRequest request = null);
        void AddConsultBasicInfo(ConsultBasicInfo info);
        void UpdateConsultBasicInfo(ConsultBasicInfo info);
        void DeleteConsultBasicInfo(List<int> ids);
        void DeleteConsultBasicInfo(string projectNumber);
        ConsultBasicInfo GetConsultBasicInfo(int id);
        ConsultBasicInfo GetConsultBasicInfo(string projectNumber);


        ValueBasicInfo GetVlaueProjectBasicInfo(int id);
        ValueBasicInfo GetVlaueProjectBasicInfo(string projectNumber);
        IEnumerable<ValueBasicInfo> GetVlaueProjectBasicInfoList(ProjectInfoRequest request = null);
        void AddVlaueProjectBasicInfo(ValueBasicInfo info);
        void UpdateVlaueProjectBasicInfo(ValueBasicInfo info);
        void DeleteVlaueProjectBasicInfo(List<int> ids);
        void DeleteVlaueProjectBasicInfo(string projectNumber);

        List<TestBasicInfo> GetTestBasicInfoList(string projectNumber);
        TestBasicInfo GetTestBasicInfo(int id);
        TestBasicInfo GetTestBasicInfo(string projectNumber);
        TestBasicInfo GetProjectTestBasicInfo(int id);
        TestBasicInfo GetProjectTestBasicInfo(string projectNumber);

        IEnumerable<TestBasicInfo> GetProjectTestBasicInfoList(ProjectInfoRequest request = null);
        void AddProjectTestBasicInfo(TestBasicInfo info);
        void UpdateProjectTestBasicInfo(TestBasicInfo info);
        void DeleteProjectTestBasicInfo(List<int> ids);
        void DeleteProjectTestBasicInfo(string projectNumber);

        //历史记录表以下
        List<TestBasicInfoHistory> GetTestBasicInfoListHistory(string projectNumber);
        List<TestChemicalReportListHistory> GetTestChemicalReportListHistory(string projectNumber);
        void AddTestChemicalReportListHistory(TestChemicalReportListHistory info);
        IEnumerable<TestBasicInfoHistory> GetProjectTestBasicInfoHistoryList(ProjectInfoRequest request = null);
        void AddProjectTestBasicInfoHistory(TestBasicInfoHistory info);

        IEnumerable<ValueBasicInfoHistory> GetVlaueProjectBasicInfoHistoryList(ProjectInfoRequest request = null);
        void AddVlaueProjectBasicInfoHistory(ValueBasicInfoHistory info);

        IEnumerable<ConsultBasicInfoHistory> GetConsultBasicInfoHistoryList(ProjectInfoRequest request = null);
        void AddConsultBasicInfoHistory(ConsultBasicInfoHistory info);
        ProjectInfoHistory GetProjectInfoHistory(int ID);
        IEnumerable<ProjectInfoHistory> GetProjectInfoHistoryList(ProjectInfoRequest request = null);
        void AddProjectInfoHistory(ProjectInfoHistory info);

        ProjectFileHistory GetProjectFileHistory(int id);
        ProjectFileHistory GetProjectFileHistory(string projectNumber);
        IEnumerable<ProjectFileHistory> GetProjectFileHistoryList(ProjectInfoRequest request = null);
        void AddProjectFileHistory(ProjectFileHistory file);
        void UpdateProjectFileHistory(ProjectFileHistory file);
        void DeleteProjectFileHistory(List<int> ids);
        void DeleteProjectFileHistory(string projectNumber);


        ProjectDocFileHistory GetProjectDocFileHistory(int id);
        List<ProjectDocFileHistory> GetProjectDocHistoryList(string Year);
        ProjectDocFileHistory GetProjectDocFileHistory(string projectNumber);
        IEnumerable<ProjectDocFileHistory> GetProjectDocFileHistoryList(ProjectInfoRequest request = null);
        IEnumerable<ProjectDocFileHistory> GetProjectDocFileHistoryList(string projectNumber);
        void AddProjectDocFileHistory(ProjectDocFileHistory file);
        void UpdateProjectDocFileHistory(ProjectDocFileHistory file);
        void DeleteProjectDocFileHistory(List<int> ids);
        void DeleteProjectDocFileHistory(string projectNumber);

        //项目审核表操作
        ProjectChecker GetProjectChecker(int id);
        ProjectChecker GetProjectChecker(string checkerName);

        IEnumerable<ProjectChecker> GetProjectCheckerList(ProjectInfoRequest request = null);
        IEnumerable<ProjectChecker> GetProjectCheckerList(string checkerName, ProjectInfoRequest request = null);

        void AddProjectChecker(ProjectChecker file);
        void UpdateProjectChecker(ProjectChecker file);
        void DeleteProjectChecker(List<int> ids);



        //客户管理操作
        CustomerModel GetCustomer(int id);
        IEnumerable<CustomerModel> GetCustomerList(ProjectInfoRequest request = null);
        void AddCustomer(CustomerModel file);
        void UpdateCustomer(CustomerModel file);
        void DeleteCustomer(List<int> ids);

        //访问记录操作
        VisitRecord GetVisitRecord(int id);
        IEnumerable<VisitRecord> GetVisitRecordList(ProjectInfoRequest request = null);
        void AddVisitRecord(VisitRecord file);
        void UpdateVisitRecord(VisitRecord file);
        void DeleteVisitRecord(List<int> ids);

        DeviceOrderDetail GetDeviceOrderDetail(int id);

        IEnumerable<DeviceOrderDetail> GetDeviceOrderDetailList(DeviceRequest request = null);

        void AddDeviceOrderDetail(DeviceOrderDetail info);

        void UpdateDeviceOrderDetail(DeviceOrderDetail info);

        void DeleteDeviceOrderDetail(List<int> ids);
        void DeleteDeviceOrderDetail(string projectNumber);

    }
}
