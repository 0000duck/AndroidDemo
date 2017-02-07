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
        void UpdateUserInputList(int type, string value);
        List<string> GetUserInputList(int type,string search);
        List<ProjectDocFile> GetProjectFilesByProjectNumber(string ProjectNumber);
       SampleRegisterTable GetSampleRegisterTableBySampleNumber(string SampleNumber);

       IEnumerable<ProjectContract> GetProjectAllContractListDoingPerson(ProjectInfoRequest request = null);
        IEnumerable<ProjectContract> GetProjectContractListDoingPerson(ProjectInfoRequest request = null);
       IEnumerable<ProjectInfoHistory> GetProjectInfoHistoryListPerson(ProjectInfoRequest request = null);
        IEnumerable<ProjectInfo> GetProjectInfoListPerson(ProjectInfoRequest request = null);
        List<OccupationaldiseaseHarm> GetOccupationaldiseaseHarmList(List<Parameter> models);
        IEnumerable<Parameter> GetParameterListAll(ProjectInfoRequest request = null);
        List<ProjectInfoHistory> GetProjectSearchByArea(string area, string year, string beginMonth, string endMonth);
        List<ProjectContract> GetMoneySearch(string Name, string Year,string BeginMonth, string EndMonth);
        List<Areas> GetAreasList();
        void TryToUpdateArea(string area);
        List<ProjectInfoHistory> GetProjectSearch(string JobType, string People, string ProjectType, string Year);

        IEnumerable<TimeInstructions> GetTimeInstructionsList(ProjectInfoRequest request);
        List<TimeInstructions> SelectTimeInstruction(string ProjectNumber);
        List<TimeInstructions> SelectTimeInstructions(string ProjectName, string SignTime);
        List<TimeInstructions> SelectTimeInstructionByCostingID(string costingID);
        TimeInstructions SelectTimeInstructions(int id);
        void UpDateTimeInstructions(TimeInstructions timeInstruions);
        void InsertTimeInstructions(TimeInstructions timeInstruions);
        TimeInstructions GetTimeInstruc(int id);

        void DeleteProjectContract(List<int> ids);
        ProjectContract SelectContractInfo(string projectName);
        ProjectContract GetProjectContractInfo(int id);
        void UpdateProjectContract(ProjectContract model);
        void InsertProjectContract(ProjectContract contract);
        IEnumerable<ProjectContract> GetProjectContractHistoryPerson(ProjectInfoRequest request = null);
        IEnumerable<ProjectContract> GetProjectContractHistory(ProjectInfoRequest request = null);
        IEnumerable<ProjectContract> GetProjectContractListPerson(ProjectInfoRequest request = null);
        IEnumerable<ProjectContract> GetProjectContractList(ProjectInfoRequest request = null);
        IEnumerable<ProjectContract> GetProjectContractListDoing(ProjectInfoRequest request = null);
        IEnumerable<ProjectContract> GetProjectContractListMarket(ProjectInfoRequest request = null);
        IEnumerable<ProjectContract> GetProjectContractListTest(ProjectInfoRequest request = null);
        IEnumerable<ProjectContract> GetProjectContractListQuality(ProjectInfoRequest request = null);
        IEnumerable<ProjectContract> GetProjectContractListWorker(ProjectInfoRequest request = null);
        IEnumerable<ProjectContract> GetProjectContractListFinancial(ProjectInfoRequest request = null);
        IEnumerable<ProjectContract> GetProjectContractListJob(ProjectInfoRequest request = null);

        IEnumerable<CostingHistory> GetProjectInfoHistoryListPerson(CostingRequest request = null);
        IEnumerable<CostingHistory> GetProjectInfoHistoryList(CostingRequest request = null);
        void AddCostingHistory(CostingHistory info);
        /// <summary>
        /// 成本分析表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IEnumerable<Costing> GetCostingList(CostingRequest request = null);
        void InsertCosting(Costing costing);
        void DeleteCosting(List<int> id);
        void UpDateCosting(Costing costing);
        Costing SelectCosting(int id);
        Costing SelectCosting(string projectName);

        //IEnumerable<ContractTable> GetContractTableList(ProjectInfoRequest request = null);
        //void InsertContractTable(ContractTable contract);
        //void DeleteContractTable(List<int> id);
        //void UpDateContractTable(ContractTable contract);
        //ContractTable SelectContractTable(int id);


        void CheckDevice();            
        Cookies GetCookies();                           //创建样品的时候记录上次信息
        void UpdateCookies(Cookies cookies);            //更新创建样品的记录
        List<DeviceOrderInfo> GetDeviceOrderInfo(string DeviceName, string year, string beginMonth, string endMonth);
        DeviceOrderInfo GetDeviceOrderInfo(string ProjectNumber,string DeviceName,DateTime OrderDate); ////
        IEnumerable<DeviceOrderInfo> GetDeviceOrderInfoFailedList(DeviceRequest request = null);
        List<ProjectInfo> GetProjectInfos(string Year);
        List<ProjectInfoHistory> GetProjectInfosHistory(string Year);
        List<Parameter> GetParameterListBySampleProject(List<string> str);
        IEnumerable<SampleRegisterTable> GetSampleRegisterTableEdit(string projectNumber);
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
        ProjectInfo SelectProjectInfo(string ProjectNumber);
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
        void DeleteArgumentValue();
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
        IEnumerable<Parameter> GetParameterListPhysical(ProjectInfoRequest request = null);
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
        void GetSampleRegisterTable1(List<int> ids,string str);
        void GetSampleRegisterTable1(List<int> ids);
        void DeleteSampleRegister(string sampleRegisterNumber);
        IEnumerable<SampleRegisterTable> GetSampleRegisterTableList(ProjectInfoRequest request = null);
        IEnumerable<SampleRegisterTable> GetSampleRegisterTableList(string Name, ProjectInfoRequest request = null);
        IEnumerable<SampleRegisterTable> GetSampleRegisterTableList(string projectNumber);
        List<SampleRegisterTable> GetSampleRegisterTableListEdit(string projectNumber);
        IEnumerable<SampleRegisterTable> GetSampleRegisterTableList(ProjectInfoRequest request, string projectNumber);
        void InsertSampleRegister(SampleRegisterTable SampleRegister);
        void DeleteSampleRegister(List<int> id);
        void DeleteSampleRegisterD(int id);
        void UpDateSampleRegister(SampleRegisterTable SampleRegister);
        SampleRegisterTable SelectSampleRegister(int id);
        SampleRegisterTable GetSampleRegisterTable(string sampleRegisterNumber);
        SampleRegisterTable GetSampleRegisterTables(string sampleRegisterNumber);
        SampleRegisterTable GetSampleRegisterTableByProjectNumber(string projectNumber);
        List<SampleRegisterTable> SelectSampleRegisterListByProjectNumber(string projectNumber);
        List<SampleRegisterTable> SelectAllSampleRegisterList();

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
        int GetDeviceNumber(string deviceName,int checkState);
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
        List<DeviceModel> GetDeviceListByDeviceNameStatic(string DeviceName);
        List<DeviceModel> GetDeviceListByDeviceName(string DeviceName);
        List<DeviceModel> GetDeviceListByNumber(string Number);
        IEnumerable<DeviceModel> GetDeviceList(DeviceModelRequest request = null);
        IEnumerable<DeviceModel> GetDeviceLists(DeviceModelRequest request = null);
        /// <summary>
        /// 设备表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void InsertDevice(DeviceModel device);
        void DeleteDevice(List<int> id);
        void UpDateDevice(DeviceModel device);
        DeviceModel SelectDevice(int id);
        DeviceModel SelectDeviceN(string number);

        List<DeviceModel> GetDeviceListByNuber(string nuber);

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


        List<DeviceOrderInfo> GetDeviceOrderInfoA(string projectNumber);
        DeviceOrderInfo GetDeviceOrderInfo(string projectNumber);
        DeviceOrderInfo GetDeviceOrderInfo(int id);
        DeviceOrderInfo GetDeviceOrderInfoByProjectNumberAndCreateTime(string str,DateTime createTime);
        DeviceOrderInfo GetDeviceOrderInfoByProjectNumber(string str);
        IEnumerable<DeviceOrderInfo> GetDeviceOrderInfoList(DeviceRequest request = null);
        void InsertDeviceOrderInfo(DeviceOrderInfo deviceName);
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
        List<ProjectDocFile> GetProjectDocFileLists(string projectNumber);
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
        IEnumerable<ProjectContract> GetProjectContractByProjectNumber(string projectNumber);
        IEnumerable<ProjectInfo> GetProjectInfoListByPerson(ProjectInfoRequest request = null);
        IEnumerable<ProjectInfo> SelectAllProjectInfo(ProjectInfoRequest request = null);
        IEnumerable<ProjectInfo> GetProjectInfoLista(ProjectInfoRequest request = null);
        IEnumerable<ProjectInfo> GetProjectInfoListP(ProjectInfoRequest request = null);
        IEnumerable<ProjectInfo> GetProjectInfoListT(ProjectInfoRequest request = null);
        IEnumerable<ProjectInfo> GetProjectInfoList(ProjectInfoRequest request = null);
        IEnumerable<ProjectInfo> GetAllProjectInfoList(ProjectInfoRequest request = null);
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

        List<TestBasicInfo> GetProectBasicInfoLists(string projectNumber);
        IEnumerable<TestBasicInfo> GetProjectTestBasicInfoList(ProjectInfoRequest request = null);
        IEnumerable<TestBasicInfo> GetProjectTestBasicInfoListBySampleStatus(ProjectInfoRequest request = null);
        
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
        ProjectInfoHistory GetProjectInfoHistory(string projectNumber);
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


        List<DeviceOrderDetail> GeDeviceOrderDetailq(string DeviceName, string year, string beginMonth, string endMonth);
        DeviceOrderDetail GetDeviceOrderDetail(int id);

        IEnumerable<DeviceOrderInfo> GetDeviceOrderInfoListD(DeviceRequest request = null);
        IEnumerable<DeviceOrderDetail> GetDeviceOrderDetailList(DeviceRequest request = null);
        IEnumerable<DeviceOrderDetail> GetDeviceOrderDetailLists(DeviceRequest request = null);
        DeviceOrderDetail GetDeviceOrderDetaislList(int id);
        List<DeviceOrderInfo> GetDeviceOrderInfoByProjectNumbert(string ProjectNumber);
        List<DeviceOrderDetail> GetDeviceOrderDetailsListByProjectNumber(string ProjectNumber);
        DeviceOrderInfo GetDeviceOrderInfoByNumberD(string ProjectNumber);
        DeviceOrderDetail GetDeviceOrderDetailByNumber(string ProjectNumber);
        DeviceDetail GetDeviceDetail(DateTime OrderTime, string DeviceNumber);
        void AddDeviceDetail(DeviceDetail info);
        void AddDeviceOrderDetail(DeviceOrderDetail info);

        void UpdateDeviceOrderDetail(DeviceOrderDetail info);

        void DeleteDeviceOrderDetail(List<int> ids);
        void DeleteDeviceOrderDetail(string projectNumber);
        DeviceOrderDetail SelectDeviceOrderDetail(int id);

    }
}
