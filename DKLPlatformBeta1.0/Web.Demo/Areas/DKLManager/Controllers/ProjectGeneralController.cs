﻿using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Areas.DKLManager.Models;
using Web.Demo.Common;
using HYZK.FrameWork.Common;
using System.Drawing.Imaging;
using HYZK.Core.Upload;
using OfficeDocGenerate;
using System.Threading;


namespace Web.Demo.Areas.DKLManager.Controllers
{
    /// <summary>
    /// 评价监测组员
    /// </summary>
    public class ProjectGeneralController : AdminControllerBase
    {
        private static ProjectWholeInfoViewModel m_ProjectWholeInfoViewModel;
        //private static ProjectWholeInfoViewModel m_ProjectWholeInfoViewModelUpdate;
        //
        // GET: /DKLManager/ProjectGeneralControl/
        public ActionResult Index(ProjectInfoRequest request)
        {
            ViewData.Add("ProjectPersonCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectPersonCategory>(), "Key", "Value"));
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            request.ProjectStatus = (int)EnumProjectSatus.ZhiPingSubmit;
            request.ProjectCheif = user.Name;
            //request.ProjectCategory = (int)EnumProjectCategory.TestValue; 
            var result = this.IDKLManagerService.GetProjectInfoListByPerson(request);
            foreach (var m in result)
            {
                if (m.ProjectCheif != user.Name)
                {
                    m.ProjectCheif = user.Name;
                    this.IDKLManagerService.UpdateProjectInfo(m);
                }
            }
            //var result = this.IDKLManagerService.GetProjectInfoList(request);
            //var users = this.AccountService.GetUserList(6).Select(c => new { Id = c.ID, Name = c.Name });
            //ViewData.Add("ProjectGeneral", new SelectList(users, "Name", "Name"));
            return View(result);

        }
        public ActionResult Problem(int id)
        {
            var model = this.IDKLManagerService.GetProjectInfo(id);
            ViewData.Add("ProjectProblem", new SelectList(EnumHelper.GetItemValueList<EnumProjectProblem>(), "Key", "Value"));
            return View(model);

        }
        [HttpPost]
        public ActionResult Problem(int id, FormCollection collection)
        {

            var model = this.IDKLManagerService.GetProjectInfo(id);

            this.TryUpdateModel<ProjectInfo>(model);
            this.IDKLManagerService.UpdateProjectInfo(model);
            return this.RefreshParent();
        }
        public ActionResult Course(string ProjectName, string SignTime)
        {

            if (SignTime.Contains("?"))
            {
                SignTime = SignTime.Substring(0, SignTime.LastIndexOf("?"));
            }
            List<TimeInstructions> result = new List<TimeInstructions>();
            if (ProjectName != null && SignTime != null)
            {

                result = this.IDKLManagerService.SelectTimeInstructions(ProjectName, SignTime);
            }
            return View(result);
        }

        public ActionResult UpdateChemical(ProjectInfoRequest request, string projectNumber)
        {
            var result = this.IDKLManagerService.GetSampleRegisterTableList(request, projectNumber);
            //      request.SampleState = (int)EnumSampleStates.DoneSample;
            return View(result);
        }
        public ActionResult EditChemical(int id)
        {
            var model = new SampleRegisterTable();
            model = this.IDKLManagerService.GetSampleRegisterTable(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult EditChemical(int id, FormCollection collection)
        {
            var model = new SampleRegisterTable();
            model = this.IDKLManagerService.GetSampleRegisterTable(id);
            this.TryUpdateModel<SampleRegisterTable>(model);
            this.IDKLManagerService.UpDateSampleRegister(model);
            return this.RefreshParent();
        }
        public ActionResult Edit(int id, string projectNumber)
        {
            var parameters = this.IDKLManagerService.GetParameterList().Select(c => new { Name = c.ParameterName }).Distinct();
            #region 过滤掉样品登记表中的噪声选项
            int i = 0;
            var paraMeters = parameters.ToList();
            foreach (var item in parameters)
            {
                i++;
                if (item.Name == "噪声" || item.Name == "噪音")
                {
                    paraMeters.Remove(paraMeters[i - 1]);
                    i--;
                }
            }
            #endregion
            ViewData.Add("ParameterName", new SelectList(paraMeters, "Name", "Name"));
            var parameter = this.IDKLManagerService.GetParameterListPhysical().Select(c => new { Name = c.ParameterName }).Distinct();
            var users = this.AccountService.GetUserList(5).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Person", new SelectList(users, "Name", "Name"));
            ViewData.Add("ParameterPhysical", new SelectList(parameter, "Name", "Name"));
            ViewData.Add("LexCategory", new SelectList(EnumHelper.GetItemValueList<EnumLexCategory>(), "Key", "Value"));
            m_ProjectWholeInfoViewModel = new ProjectWholeInfoViewModel();
            m_ProjectWholeInfoViewModel.projectTestBasicinfoList = this.IDKLManagerService.GetProectBasicInfoLists(projectNumber);
            m_ProjectWholeInfoViewModel.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (m_ProjectWholeInfoViewModel.projectBasicinfo != null)
            {
                m_ProjectWholeInfoViewModel.projectBasicImgFile = this.IDKLManagerService.GetProjectFile(m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber);
                string fileName = CreateBarCode(m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber);
                if (fileName != null)
                {
                    string filePath = fileName.Substring(fileName.IndexOf("Upload") - 1);
                    ViewData.Add("ProjectBarCodeImg", filePath);
                }
            }
            this.RenderMyViewDatas(m_ProjectWholeInfoViewModel);
            return View(m_ProjectWholeInfoViewModel);
        }
        [HttpPost]
        public ActionResult AddTestBasicinfo(FormCollection collection, string projectNumber)
        {
            var parameter = this.IDKLManagerService.GetParameterListPhysical().Select(c => new { Name = c.ParameterName }).Distinct();
            var users = this.AccountService.GetUserList(5).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Person", new SelectList(users, "Name", "Name"));
            ViewData.Add("ParameterPhysical", new SelectList(parameter, "Name", "Name"));
            ViewData.Add("LexCategory", new SelectList(EnumHelper.GetItemValueList<EnumLexCategory>(), "Key", "Value"));
            var model = new TestBasicInfo();
            model.TestContent = collection["ParameterPhysical"];
            model.ProjectNumber = m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber;
            model.WordShop = collection["projectTestBasicinfo.WordShop"];
            model.Job = collection["projectTestBasicinfo.Job"];
            model.Location = collection["projectTestBasicinfo.Location"];
            model.SampleNumber = collection["projectTestBasicinfo.SampleNumber"];
            model.TouchTime = collection["projectTestBasicInfo.TouchTime"];
            model.NoiseStrength = collection["projectTestBasicinfo.NoiseStrength"];
            model.Lex8hLexw = collection["projectTestBasicinfo.Lex8hLexw"];
            model.LexCategory = collection["LexCategory"].ToInt();
            m_ProjectWholeInfoViewModel.projectTestBasicinfoList.Add(model);
            var list = this.IDKLManagerService.GetProectBasicInfoLists(projectNumber);
            this.IDKLManagerService.AddProjectTestBasicInfo(model);
            return View("Edit", m_ProjectWholeInfoViewModel);
        }
        [HttpPost]
        public ActionResult AddTestChemicalReport(FormCollection collection)
        {
            var model = new TestChemicalReport();
            var models = new SampleRegisterTable();
            var parameters = this.IDKLManagerService.GetParameterList().Select(c => new { Name = c.ParameterName }).Distinct();
            #region 过滤掉样品登记表中的噪声选项
            int i = 0;
            var paraMeters = parameters.ToList();
            foreach (var item in parameters)
            {
                i++;
                if (item.Name == "噪声" || item.Name == "噪音")
                {
                    paraMeters.Remove(paraMeters[i - 1]);
                    i--;
                }
            }
            #endregion
            ViewData.Add("ParameterName", new SelectList(paraMeters, "Name", "Name"));
            m_ProjectWholeInfoViewModel.projectBasicinfo.Person = collection["Person"];
            model.ProjectNumber = m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber;
            model.WorkShop = collection["projectTestChemicalReport.WorkShop"];
            model.Job = collection["projectTestChemicalReport.Job"];
            model.Location = collection["projectTestChemicalReport.Location"];
            model.CSTEL = collection["projectTestChemicalReport.CSTEL"];
            model.CTWA = collection["projectTestChemicalReport.CTWA"];
            model.CMAC = collection["projectTestChemicalReport.CMAC"];
            model.Factor = collection["projectTestChemicalReport.SampleProject"];
            model.SampleNumber = collection["projectTestChemicalReport.SampleNumber"];
            model.SampleProject = collection["projectTestChemicalReport.SampleProject"];

            m_ProjectWholeInfoViewModel.projectTestChemicalReportList.Add(model);
            this.IDKLManagerService.InsertTestChemicalReport(model);

            return View("Edit", m_ProjectWholeInfoViewModel);
        }
        [HttpPost]
        public ActionResult Edit(int id, string projectNumber, string sampleRegisterNumber, FormCollection collection)
        {
            ViewData.Add("ProjectPersonCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectPersonCategory>(), "Key", "Value"));
            m_ProjectWholeInfoViewModel.projectBasicinfo.Person = collection["Person"];
            ProjectInfo projectModel = this.IDKLManagerService.GetProjectInfo(id);
            var fileList = this.IDKLManagerService.GetProjectDocFileLists(projectModel.ProjectNumber);
            int flag = 0;
            foreach (var item in fileList)
            {
                if (item.Status == 4)
                {
                    flag = 1;
                    break;
                }
            }
            if (flag == 0)
            {
                return Back("未检测到上传文件，请上传文件！");
            }
            ////获取报告文档
            //HttpFileCollectionBase files = Request.Files;
            //HttpPostedFileBase file = files["docFile"];
            //string fileName = "";
            //if (file != null && file.ContentLength > 0)
            //{
            //    fileName = GetFilePathByRawFile(file.FileName);
            //    file.SaveAs(fileName);
            //}
            //提交
            //判断所有数据不为空   
            var model = new SampleRegisterTable();
            #region 判断物理化学是否填写完整部分
            //样品表添加项目编号字段
            //新增方法 根据项目编号获取样品model list
            //根据list判断样品状态是否为2 
            projectNumber = m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber;
            List<SampleRegisterTable> models = this.IDKLManagerService.GetSampleRegisterListByProjectNumber(projectNumber);
            models = this.IDKLManagerService.GetSampleRegisterListByProjectNumber(projectNumber);
            foreach (var item in models)
            {
                if (item.SampleStates == (int)EnumSampleStates.DoneSample)
                {
                    m_ProjectWholeInfoViewModel.sampleTable.SampleStates = item.SampleStates;
                    m_ProjectWholeInfoViewModel.sampleTable = item;
                    break;
                }
            }
            #endregion
            model.SampleStates = m_ProjectWholeInfoViewModel.sampleTable.SampleStates;

            //1。设备  分析人员的， 
            //获取设备信息，判断
            var deviceInfo = this.IDKLManagerService.GetDeviceOrderInfoA(projectNumber);

              


            //获取分析人员信息判断
            var sampleInfo = this.IDKLManagerService.GetSampleRegisterTableByProjectNumber(projectNumber);

            //判断是否符合条件
            foreach (var b in deviceInfo)
            {
                if ((m_ProjectWholeInfoViewModel.projectTestBasicinfoList.Count > 0)
                    && (b.OrderState == (int)EnumOrderStateInfo.OrderSucceed))
                    //&& (sampleInfo.SampleStates == (int)EnumSampleStates.Selec))
                {
                    //更新图片信息到数据库

                    if (m_ProjectWholeInfoViewModel.projectBasicImgFile != null)
                    {
                        m_ProjectWholeInfoViewModel.projectBasicImgFile.FilePath = collection["projectBasicImgFile.FilePath"];
                        this.IDKLManagerService.UpdateProjectFile(m_ProjectWholeInfoViewModel.projectBasicImgFile);
                    }

                    //更新检测信息列表到数据库
                    m_ProjectWholeInfoViewModel.projectTestBasicinfoList = this.IDKLManagerService.GetTestBasicInfoList(projectNumber);

                    //更新项目信息到数据库

                    m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectPersonCategory = 0;
                    m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectStatus = (int)EnumProjectSatus.ProjectModifyFour;
                    m_ProjectWholeInfoViewModel.projectBasicinfo.Person = collection["Person"];
                    m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectProblem = 0;
                    m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectProblemDescribe = "";
                    this.IDKLManagerService.UpdateProjectInfo(m_ProjectWholeInfoViewModel.projectBasicinfo);

                    //更新文档信息到数据库
                    //var projectBasicDocFile = new ProjectDocFile();
                    //projectBasicDocFile.FilePath = fileName;
                    //projectBasicDocFile.CreateTime = DateTime.Now;
                    //projectBasicDocFile.ProjectNumber = m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber;
                    //projectBasicDocFile.Status = m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectStatus;
                    //this.IDKLManagerService.AddProjectDocFile(projectBasicDocFile);

                    //添加检测的数据到数据库
                    //foreach (var data in m_ProjectWholeInfoViewModel.projectTestBasicinfoList)
                    //{
                    //    this.IDKLManagerService.AddProjectTestBasicInfo(data);
                    //}
                }

                else
                {
                    if ((m_ProjectWholeInfoViewModel.projectTestBasicinfoList.Count == 0))
                    {
                        return Back("请填写完整的物理信息");
                    }
                    foreach (var m in deviceInfo)
                    {
                        if (b.OrderState != (int)EnumOrderStateInfo.OrderSucceed)
                        {
                            return Back("请联系设备人员检查预约相关信息");
                        }
                    }
                    if (sampleInfo.SampleStates != (int)EnumSampleStates.Sumbit)
                    {
                        return Back("请联系实验室审核相关样品");
                    }
                }
            }
            ///返回列表 
            ///
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            
            ProjectInfoRequest request = new ProjectInfoRequest();
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            //request.ProjectStatus = (int)EnumProjectSatus.ProjectWorkFinish;
           // projectModel.Person = collection["Pserson"];
            request.ProjectCategory = (int)EnumProjectCategory.TestValue;
            request.ProjectCheif = user.Name;
            var result = this.IDKLManagerService.GetProjectInfoList(request);


            var nn = this.IDKLManagerService.SelectContractInfo(m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectName);
            model.SignTime = nn.ContractDate;
            var models1 = new TimeInstructions();
            models1.ProjectNumBer = model.ProjectNumber;
            models1.ProjectName = model.ProjectName;
            models1.TimeNode = DateTime.Now;
            models1.SignTime = model.SignTime.ToString();
            models1.Instructions = user.LoginName+"检测评价员工";
            this.IDKLManagerService.InsertTimeInstructions(models1);
            return View("Index", result);

        }
        [HttpPost]
        public ActionResult UploadFiles(FormCollection collection)
        {
            //获取报告文档
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = files["docFile"];
            string fileName = "";
            if (file != null && file.ContentLength > 0)
            {
                fileName = GetFilePathByRawFile(file.FileName);
                file.SaveAs(fileName);
            }
            if (file.ContentLength == 0)
                return Back("未检测到上传文件！");
            var projectBasicDocFile = new ProjectDocFile();
            projectBasicDocFile.FilePath = fileName;
            projectBasicDocFile.CreateTime = DateTime.Now;
            projectBasicDocFile.ProjectNumber = m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber;
            projectBasicDocFile.Status = m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectStatus;
            this.IDKLManagerService.AddProjectDocFile(projectBasicDocFile);

            return Back("上传成功");
        }

        private string CreateBarCode(string barcodeStrData)
        {
            string ret = null;
            System.Drawing.Image barcodeImage = null;
            try
            {
                BarcodeLib.Barcode barcode = new BarcodeLib.Barcode();
                barcode.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                barcode.IncludeLabel = true;

                //===== Encoding performed here =====
                barcodeImage = barcode.Encode(BarcodeLib.TYPE.CODE128, barcodeStrData.Trim(), System.Drawing.ColorTranslator.FromHtml("#000000"), System.Drawing.ColorTranslator.FromHtml("#FFFFFF"), 300, 150);
                //===================================
                string subFolder = "month_" + DateTime.Now.ToString("yyMM");

                string barImgFileFolder = Path.Combine(UploadConfigContext.UploadPath,
                    "barCodeFolder",
                    subFolder
                    );
                if (!System.IO.Directory.Exists(barImgFileFolder))
                {
                    Directory.CreateDirectory(barImgFileFolder);
                }
                string barImgFile = barImgFileFolder + "\\" + barcodeStrData + ".jpg";
                barcodeImage.Save(barImgFile, ImageFormat.Png);
                ret = barImgFile;
            }
            catch (Exception)
            {
                //TODO: find a way to return this to display the encoding error message
            }
            finally
            {
                if (barcodeImage != null)
                {
                    //Clean up / Dispose...
                    barcodeImage.Dispose();
                }
            }//finally

            return ret;
        }
        [HttpPost]
        public ActionResult UpdatePersonStatus(int id, FormCollection collection)
        {
            //string str = "";
            //str = collection["ProjectPersonCategory"]; 
            var model = this.IDKLManagerService.GetProjectInfo(id);
            //model.ProjectPersonCategory = Convert.ToInt32(str);
            this.TryUpdateModel<ProjectInfo>(model);
            this.IDKLManagerService.UpdateProjectInfo(model);
            return RedirectToAction("Index");

        }
        [HttpPost]
        public ActionResult DownLoadDocFile(FormCollection collection)
        {
            //try
            //{
            string strFileName = "";

            //1.判断是否满足下载条件
            //2.从各个数据库获取数据
            //3.调用接口生成报告

            List<string> strc = new List<string>();                          //物理检测项目(查询，制表用)               
            List<string> str = new List<string>();          //化学采样项目 控制打印表格时打印的项目
            ProjectInfo projectmodels = new ProjectInfo();
            List<TestPhysicalReport> physicalmodels = new List<TestPhysicalReport>();
            List<TestChemicalReport> chemicalmodels = new List<TestChemicalReport>();
            if ((m_ProjectWholeInfoViewModel == null) || (m_ProjectWholeInfoViewModel.projectBasicinfo == null))
            {
                return Back("123");
            }

            List<SampleRegisterTable> models = this.IDKLManagerService.GetSampleRegisterListByProjectNumber(m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber);

            #region 化学数据对接
            foreach (var item in models)
            {
                TestChemicalReport model = new TestChemicalReport();
                model.SampleNumber = item.SampleRegisterNumber;
                model.ProjectNumber = item.ProjectNumber;
                model.SampleProject = item.ParameterName;
                model.Factor = item.ParameterName;
                model.ID = item.ID;
                model.CreateTime = item.SamplingDay;
                model.WorkShop = item.WorkShop;
                model.Job = item.Job;
                model.Location = item.Location;
                model.CSTEL = item.CSTEL;
                model.CTWA = item.CTWA;
                model.CMAC = item.CMAC;
                chemicalmodels.Add(model);
            }
            #endregion

            foreach (var item in chemicalmodels)
            {
                str.Add(item.SampleProject);
            }
            str = str.Distinct().ToList();
            #region 采样项目打印
            List<SampleProjectGist> ProjectGistmodels = this.IDKLManagerService.GetSampleProjectGistBySampleProject(str);

            #endregion
            #region 基本信息
            projectmodels.CompaneName = m_ProjectWholeInfoViewModel.projectBasicinfo.CompaneName;
            projectmodels.CompanyAddress = m_ProjectWholeInfoViewModel.projectBasicinfo.CompanyAddress;
            projectmodels.ProjectName = m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectName;
            projectmodels.CompanyContact = m_ProjectWholeInfoViewModel.projectBasicinfo.CompanyContact;
            projectmodels.ZipCode = m_ProjectWholeInfoViewModel.projectBasicinfo.ZipCode;
            projectmodels.ContactTel = m_ProjectWholeInfoViewModel.projectBasicinfo.ContactTel;
            projectmodels.ProjectNumber = m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber;
            #endregion
            #region 物理检测部分录入
            foreach (var item in m_ProjectWholeInfoViewModel.projectTestBasicinfoList)
            {
                strc.Add(item.TestContent);
                if (item.TestContent == "噪声")
                {

                    var modeltemp = new TestPhysicalReport();
                    modeltemp.ProjectNumber = item.ProjectNumber;
                    modeltemp.WordShop = item.WordShop;
                    modeltemp.TestContent = item.TestContent;
                    modeltemp.Job = item.Job;
                    modeltemp.Location = item.Location;
                    modeltemp.SampleNumber = item.SampleNumber;
                    modeltemp.ContactTime = item.TouchTime;
                    modeltemp.NoiseIntensity = item.NoiseStrength;
                    modeltemp.Lex8hLexw = item.Lex8hLexw;
                    modeltemp.LexCategory = item.LexCategory;
                    physicalmodels.Add(modeltemp);

                }

            }

            strc = strc.Distinct().ToList();   //去重

            #endregion
            #region 检测部分打印（物理+化学）
            List<Parameter> Parametermodels = this.IDKLManagerService.GetParameterListBySampleProject(str, strc);
            #endregion
            #region 化学限值部分
            List<Parameter> ParmeterChemicalModels = this.IDKLManagerService.GetParameterListBySampleProject(str);  //重载

            #endregion
            #region 报告生成+下载


            CreateWord cr = new CreateWord(projectmodels);
            if ((ProjectGistmodels != null) && (Parametermodels != null) && (ParmeterChemicalModels != null) && (projectmodels != null) && (physicalmodels != null) && (chemicalmodels != null))
            {
                List<string> appList = new List<string>();

                appList = cr.CreateReportWord(str, strc, ProjectGistmodels, Parametermodels, ParmeterChemicalModels, projectmodels, physicalmodels, chemicalmodels);


                #region 判断报告生成运行状态
                if (appList[0] == "1")
                {
                    FileInfo fr = new FileInfo(appList[1]);
                    fr.Delete();
                    return Back("检测项目中缺少对应参数，请联系实验室人员添加相应参数");
                }
                if (appList[0] == "2")
                {
                    FileInfo fr = new FileInfo(appList[1]);
                    fr.Delete();
                    return Back("采样项目中缺少对应参数，请联系实验室人员添加相应参数");
                }
                if (appList[0] == "3")
                {
                    FileInfo fr = new FileInfo(appList[1]);
                    fr.Delete();
                    return Back("采样项目未检测到数据，请联系实验室人员添加相应数据");
                }

                #endregion

                strFileName = appList[1];
            }                                    //报告下载



            if (!string.IsNullOrEmpty(strFileName))
            {

                string fileNewName = strFileName.Substring(strFileName.LastIndexOf("\\") + 1);
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = Encoding.UTF8;
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileNewName, Encoding.UTF8));
                Response.WriteFile(strFileName);
                Response.End();
            }
            else
            {
                return Back("下载报告失败");
            }

            #endregion
            //}
            //catch (Exception e)
            //{
            //    return Back("1"+e.Message); 
            //}
            return Back("成功");


        }
        [HttpPost]


        public ActionResult DownLoadTsetDocFile(FormCollection collection)
        {
            //try
            //{
            string strFileName = "";

            //1.判断是否满足下载条件
            //2.从各个数据库获取数据
            //3.调用接口生成报告

            List<string> strc = new List<string>();                          //物理检测项目(查询，制表用)               
            List<string> str = new List<string>();          //化学采样项目 控制打印表格时打印的项目
            ProjectInfo projectmodels = new ProjectInfo();
            List<TestPhysicalReport> physicalmodels = new List<TestPhysicalReport>();
            List<TestChemicalReport> chemicalmodels = new List<TestChemicalReport>();
            if ((m_ProjectWholeInfoViewModel == null) || (m_ProjectWholeInfoViewModel.projectBasicinfo == null))
            {
                return Back("123");
            }

            List<SampleRegisterTable> models = this.IDKLManagerService.GetSampleRegisterListByProjectNumber(m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber);

            #region 化学数据对接
            foreach (var item in models)
            {
                TestChemicalReport model = new TestChemicalReport();
                model.SampleNumber = item.SampleRegisterNumber;
                model.ProjectNumber = item.ProjectNumber;
                model.SampleProject = item.ParameterName;
                model.Factor = item.ParameterName;
                model.ID = item.ID;
                model.CreateTime = item.SamplingDay;
                model.WorkShop = item.WorkShop;
                model.Job = item.Job;
                model.Location = item.Location;
                model.CSTEL = item.CSTEL;
                model.CTWA = item.CTWA;
                model.CMAC = item.CMAC;
                chemicalmodels.Add(model);
            }
            #endregion

            foreach (var item in chemicalmodels)
            {
                str.Add(item.SampleProject);
            }
            str = str.Distinct().ToList();
            #region 采样项目打印
            List<SampleProjectGist> ProjectGistmodels = this.IDKLManagerService.GetSampleProjectGistBySampleProject(str);

            #endregion
            #region 基本信息
            projectmodels.CompaneName = m_ProjectWholeInfoViewModel.projectBasicinfo.CompaneName;
            projectmodels.CompanyAddress = m_ProjectWholeInfoViewModel.projectBasicinfo.CompanyAddress;
            projectmodels.ProjectName = m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectName;
            projectmodels.CompanyContact = m_ProjectWholeInfoViewModel.projectBasicinfo.CompanyContact;
            projectmodels.ZipCode = m_ProjectWholeInfoViewModel.projectBasicinfo.ZipCode;
            projectmodels.ContactTel = m_ProjectWholeInfoViewModel.projectBasicinfo.ContactTel;
            projectmodels.ProjectNumber = m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber;
            #endregion
            #region 物理检测部分录入
            foreach (var item in m_ProjectWholeInfoViewModel.projectTestBasicinfoList)
            {
                strc.Add(item.TestContent);
                if (item.TestContent == "噪声"||item.TestContent=="噪音")
                {

                    var modeltemp = new TestPhysicalReport();
                    modeltemp.ProjectNumber = item.ProjectNumber;
                    modeltemp.WordShop = item.WordShop;
                    modeltemp.TestContent = item.TestContent;
                    modeltemp.Job = item.Job;
                    modeltemp.Location = item.Location;
                    modeltemp.SampleNumber = item.SampleNumber;
                    modeltemp.ContactTime = item.TouchTime;
                    modeltemp.NoiseIntensity = item.NoiseStrength;
                    modeltemp.Lex8hLexw = item.Lex8hLexw;
                    modeltemp.LexCategory = item.LexCategory;
                    physicalmodels.Add(modeltemp);

                }

            }

            strc = strc.Distinct().ToList();   //去重

            #endregion
            #region 检测部分打印（物理+化学）
            List<Parameter> Parametermodels = this.IDKLManagerService.GetParameterListBySampleProject(str, strc);
            #endregion
            #region 化学限值部分
            List<Parameter> ParmeterChemicalModels = this.IDKLManagerService.GetParameterListBySampleProject(str);  //重载

            #endregion
            #region 报告生成+下载


            CreatTestDoc cr = new CreatTestDoc(projectmodels);
            if ((ProjectGistmodels != null) && (Parametermodels != null) && (ParmeterChemicalModels != null) && (projectmodels != null) && (physicalmodels != null) && (chemicalmodels != null))
            {
                List<string> appList = new List<string>();

                appList = cr.CreateReportWord(str, strc, ProjectGistmodels, Parametermodels, ParmeterChemicalModels, projectmodels, physicalmodels, chemicalmodels);


                #region 判断报告生成运行状态
                if (appList[0] == "1")
                {
                    FileInfo fr = new FileInfo(appList[1]);
                    fr.Delete();
                    return Back("检测项目中缺少对应参数，请联系实验室人员添加相应参数");
                }
                if (appList[0] == "2")
                {
                    FileInfo fr = new FileInfo(appList[1]);
                    fr.Delete();
                    return Back("采样项目中缺少对应参数，请联系实验室人员添加相应参数");
                }
                if (appList[0] == "3")
                {
                    FileInfo fr = new FileInfo(appList[1]);
                    fr.Delete();
                    return Back("采样项目未检测到数据，请联系实验室人员添加相应数据");
                }

                #endregion

                strFileName = appList[1];
            }                                    //报告下载



            if (!string.IsNullOrEmpty(strFileName))
            {

                string fileNewName = strFileName.Substring(strFileName.LastIndexOf("\\") + 1);
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = Encoding.UTF8;
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileNewName, Encoding.UTF8));
                Response.WriteFile(strFileName);
                Response.End();
            }
            else
            {
                return Back("下载报告失败");
            }

            #endregion
            //}
            //catch (Exception e)
            //{
            //    return Back("1"+e.Message); 
            //}
            return Back("成功");


        }
        [HttpPost]
        public ActionResult WORDTEST(FormCollection collection)
        {
            #region 基本信息
            ProjectInfo projectmodels = new ProjectInfo();
            projectmodels.CompaneName = m_ProjectWholeInfoViewModel.projectBasicinfo.CompaneName;
            projectmodels.CompanyAddress = m_ProjectWholeInfoViewModel.projectBasicinfo.CompanyAddress;
            projectmodels.ProjectName = m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectName;
            projectmodels.CompanyContact = m_ProjectWholeInfoViewModel.projectBasicinfo.CompanyContact;
            projectmodels.ZipCode = m_ProjectWholeInfoViewModel.projectBasicinfo.ZipCode;
            projectmodels.ContactTel = m_ProjectWholeInfoViewModel.projectBasicinfo.ContactTel;
            projectmodels.ProjectNumber = m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber;
            #endregion
            #region 危害因素信息
            List<string> str = new List<string>();
            List<TestChemicalReport> chemicalmodels = new List<TestChemicalReport>();
            List<SampleRegisterTable> models = this.IDKLManagerService.GetSampleRegisterListByProjectNumber(m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber);
            #region 化学数据对接
            foreach (var item in models)
            {
                TestChemicalReport model = new TestChemicalReport();
                model.SampleNumber = item.SampleRegisterNumber;
                model.ProjectNumber = item.ProjectNumber;
                model.SampleProject = item.ParameterName;
                model.Factor = item.ParameterName;
                model.ID = item.ID;
                model.CreateTime = item.SamplingDay;
                model.WorkShop = item.WorkShop;
                model.Job = item.Job;
                model.Location = item.Location;
                model.CSTEL = item.CSTEL;
                model.CTWA = item.CTWA;
                model.CMAC = item.CMAC;
                chemicalmodels.Add(model);
            }
            #endregion

            foreach (var item in chemicalmodels)
            {
                str.Add(item.SampleProject);
            }
            str = str.Distinct().ToList();
            #region 化学限值部分
            List<Parameter> ParmeterChemicalModels = this.IDKLManagerService.GetParameterListBySampleProject(str);  //重载
            List<OccupationaldiseaseHarm> OccupationaldiseaseHarms = this.IDKLManagerService.GetOccupationaldiseaseHarmList(ParmeterChemicalModels);
            foreach (var item in OccupationaldiseaseHarms)
            {
                if (item == null)
                {
                    return Back("请核对参数跟职业病信息");
                }
            }
            #endregion
            #region 化学危害描述

            #endregion
            #endregion
            string strFileName = "";
            //   CreateContractTestingAndEvaluation cr = new CreateContractTestingAndEvaluation();                       
            CreatePresentEvaluationDoc cr = new CreatePresentEvaluationDoc(projectmodels, ParmeterChemicalModels, OccupationaldiseaseHarms);
            List<string> appList = new List<string>();
            appList = cr.CreateReportWord();
            #region 判断报告生成运行状态
            if (appList[0] == "1")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }
            if (appList[0] == "2")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }
            if (appList[0] == "3")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }

            #endregion

            strFileName = appList[1];
            //报告下载                                               
            if (!string.IsNullOrEmpty(strFileName))
            {

                string fileNewName = strFileName.Substring(strFileName.LastIndexOf("\\") + 1);
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = Encoding.UTF8;
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileNewName, Encoding.UTF8));
                Response.WriteFile(strFileName);
                Response.End();

            }
            else
            {
                return Back("下载报告失败");
            }
            return Back("成功");
        }           //评价报告 三江模板

        public ActionResult WordTest(ProjectContract Model)       //职业病危害预评价报告
        {
            string strFileName = "";
            //   CreateContractTestingAndEvaluation cr = new CreateContractTestingAndEvaluation();                       
            CreateOccupationalHarmDoc cr = new CreateOccupationalHarmDoc();
            List<string> appList = new List<string>();
            appList = cr.CreateReportWord();
            #region 判断报告生成运行状态
            if (appList[0] == "1")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }
            if (appList[0] == "2")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }
            if (appList[0] == "3")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }

            #endregion

            strFileName = appList[1];
            //报告下载                                               
            if (!string.IsNullOrEmpty(strFileName))
            {

                string fileNewName = strFileName.Substring(strFileName.LastIndexOf("\\") + 1);
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = Encoding.UTF8;
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileNewName, Encoding.UTF8));
                Response.WriteFile(strFileName);
                Response.End();

            }
            else
            {
                return Back("下载报告失败");
            }
            return Back("成功");
        }
        [HttpPost]
        public ActionResult CreateWordTest(FormCollection collection)
        {
            var model = new ProjectContract();
            WordTest(model);
            return this.RefreshParent();
        }
        public ActionResult WordTestTwo(ProjectContract Model)       // 职业病危害控制效果评价报告书
        {
            string strFileName = "";
            //   CreateContractTestingAndEvaluation cr = new CreateContractTestingAndEvaluation();                       
            CreateEvaluationDoc cr = new CreateEvaluationDoc();
            List<string> appList = new List<string>();
            appList = cr.CreateReportWord();
            #region 判断报告生成运行状态
            if (appList[0] == "1")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }
            if (appList[0] == "2")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }
            if (appList[0] == "3")
            {
                FileInfo fr = new FileInfo(appList[1]);
                fr.Delete();
                return Back("合同生成失败");
            }

            #endregion

            strFileName = appList[1];
            //报告下载                                               
            if (!string.IsNullOrEmpty(strFileName))
            {

                string fileNewName = strFileName.Substring(strFileName.LastIndexOf("\\") + 1);
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = Encoding.UTF8;
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileNewName, Encoding.UTF8));
                Response.WriteFile(strFileName);
                Response.End();

            }
            else
            {
                return Back("下载报告失败");
            }
            return Back("成功");
        }
        [HttpPost]
        public ActionResult CreateWordTestTwo(FormCollection collection)
        {
            var model = new ProjectContract();
            WordTestTwo(model);
            return this.RefreshParent();
        }
    }
}


           
        
  
   