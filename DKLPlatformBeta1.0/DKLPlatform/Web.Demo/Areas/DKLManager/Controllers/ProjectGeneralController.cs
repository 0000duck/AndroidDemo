using DKLManager.Contract.Model;
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
            request.ProjectStatus = (int)EnumProjectSatus.ProjectVerifyOne;
            request.ProjectCategory = (int)EnumProjectCategory.TestValue;
            request.ProjectCheif = user.Name;
            var result = this.IDKLManagerService.GetProjectInfoList(request);
            //var users = this.AccountService.GetUserList(6).Select(c => new { Id = c.ID, Name = c.Name });
            //ViewData.Add("ProjectGeneral", new SelectList(users, "Name", "Name"));
            return View(result);

        }

        public ActionResult UpdateChemical(ProjectInfoRequest request, string projectNumber)
        {
            var result = this.IDKLManagerService.GetSampleRegisterTableListEdit(projectNumber);
            request.SampleState = (int)EnumSampleStates.DoneSample;
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
        public ActionResult Edit(int id)
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
            m_ProjectWholeInfoViewModel = new ProjectWholeInfoViewModel();
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
        public ActionResult AddTestBasicinfo(FormCollection collection)
        {
            var model = new TestBasicInfo();
            model.ProjectNumber = m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber;
            model.TestContent = collection["projectTestBasicinfo.TestContent"];
            model.WordShop = collection["projectTestBasicinfo.WordShop"];
            model.Job = collection["projectTestBasicinfo.Job"];
            model.Location = collection["projectTestBasicinfo.Location"];
            model.SampleNumber = collection["projectTestBasicinfo.SampleNumber"];
            model.TouchTime = collection["projectTestBasicInfo.TouchTime"];
            model.NoiseStrength = collection["projectTestBasicinfo.NoiseStrength"];
            model.Lex8hLexw = collection["projectTestBasicinfo.Lex8hLexw"];
            m_ProjectWholeInfoViewModel.projectTestBasicinfoList.Add(model);

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
            model.ProjectNumber = m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber;
            model.WorkShop = collection["projectTestChemicalReport.WorkShop"];
            model.Job = collection["projectTestChemicalReport.Job"];
            model.Location = collection["projectTestChemicalReport.Location"];
            model.CSTEL = collection["projectTestChemicalReport.CSTEL"];
            model.CTWA = collection["projectTestChemicalReport.CTWA"];
            model.CMAC = collection["projectTestChemicalReport.CMAC"];
            //    model.Factor = collection["projectTestChemicalReport.Factor"];
            model.Factor = collection["projectTestChemicalReport.SampleProject"];
            model.SampleNumber = collection["projectTestChemicalReport.SampleNumber"];
            model.SampleProject = collection["projectTestChemicalReport.SampleProject"];
            m_ProjectWholeInfoViewModel.projectTestChemicalReportList.Add(model);
            this.IDKLManagerService.InsertTestChemicalReport(model);

            return View("Edit", m_ProjectWholeInfoViewModel);
        }
        [HttpPost]
        public ActionResult Edit(string projectNumber, string sampleRegisterNumber, FormCollection collection)
        {
            ViewData.Add("ProjectPersonCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectPersonCategory>(), "Key", "Value"));

            //获取报告文档
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = files["docFile"];
            string fileName = "";
            if (file != null && file.ContentLength > 0)
            {
                fileName = GetFilePathByRawFile(file.FileName);
                file.SaveAs(fileName);
            }
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
            var deviceInfo = this.IDKLManagerService.GetDeviceOrderInfo(projectNumber);

            //获取分析人员信息判断
            var sampleInfo = this.IDKLManagerService.GetSampleRegisterTableByProjectNumber(projectNumber);

            //判断是否符合条件
            if ((m_ProjectWholeInfoViewModel.projectTestBasicinfoList.Count > 0)
                && (deviceInfo.OrderState == (int)EnumOrderStateInfo.OrderSucceed)
                && (sampleInfo.SampleStates == (int)EnumSampleStates.DoneSample))
            {
                //更新图片信息到数据库
                m_ProjectWholeInfoViewModel.projectBasicImgFile.FilePath = collection["projectBasicImgFile.FilePath"];
                this.IDKLManagerService.UpdateProjectFile(m_ProjectWholeInfoViewModel.projectBasicImgFile);

                //更新检测信息列表到数据库
                //m_ProjectWholeInfoViewModel.projectTestBasicinfoList = this.IDKLManagerService.GetTestBasicInfoList(projectNumber);

                //更新项目信息到数据库
                m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectPersonCategory = 0;
                m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectStatus = (int)EnumProjectSatus.ProjectModifyFour;
                this.IDKLManagerService.UpdateProjectInfo(m_ProjectWholeInfoViewModel.projectBasicinfo);

                //更新文档信息到数据库
                var projectBasicDocFile = new ProjectDocFile();
                projectBasicDocFile.FilePath = fileName;
                projectBasicDocFile.CreateTime = DateTime.Now;
                projectBasicDocFile.ProjectNumber = m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber;
                projectBasicDocFile.Status = m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectStatus;
                this.IDKLManagerService.AddProjectDocFile(projectBasicDocFile);

                //添加检测的数据到数据库
                foreach (var data in m_ProjectWholeInfoViewModel.projectTestBasicinfoList)
                {
                    this.IDKLManagerService.AddProjectTestBasicInfo(data);
                }
            }
            else
            {
                if ((m_ProjectWholeInfoViewModel.projectTestBasicinfoList.Count == 0))
                {
                    return Back("请填写完整的物理信息");
                }
                else
                {
                    return Back("请联系样品人员填写样品信息");
                }
            }
            ///返回列表 
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            ProjectInfoRequest request = new ProjectInfoRequest();
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            request.ProjectStatus = (int)EnumProjectSatus.ProjectVerifyOne;
            request.ProjectCategory = (int)EnumProjectCategory.TestValue;
            request.ProjectCheif = user.Name;
            var result = this.IDKLManagerService.GetProjectInfoList(request);
         
            return View("Index",result);
            
        }

        //public ActionResult Submit(int id)
        //{
        //    m_ProjectWholeInfoViewModelUpdate = new ProjectWholeInfoViewModel();
        //    m_ProjectWholeInfoViewModelUpdate.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
        //    if (m_ProjectWholeInfoViewModelUpdate.projectBasicinfo != null)
        //    {
        //        m_ProjectWholeInfoViewModelUpdate.projectBasicImgFile = this.IDKLManagerService.GetProjectFile(m_ProjectWholeInfoViewModelUpdate.projectBasicinfo.ProjectNumber);
        //    }

        //    if ((m_ProjectWholeInfoViewModelUpdate.projectBasicImgFile != null) && (!string.IsNullOrEmpty(m_ProjectWholeInfoViewModelUpdate.projectBasicImgFile.FilePath)))
        //    {
        //        List<string> htmlFIles = new List<string>();
        //        m_ProjectWholeInfoViewModelUpdate.projectBasicImgFile.FilePath.Split(',').ToList().ForEach(f =>
        //        {
        //            if (!string.IsNullOrEmpty(f))
        //            {
        //                var picHtml = "<li class=\"span2\"><a> <img src=" + f + " > </a>";
        //                picHtml += "<div class=\"actions\"> <a  href=\"#\"><i class=\"icon-pencil\"></i></a>";
        //                picHtml += " </div></li>";
        //                htmlFIles.Add(picHtml);
        //            }
        //        });
        //        ViewData["picFiles"] = htmlFIles;
        //    }

        //    ProjectInfoRequest request = new ProjectInfoRequest();
        //    request.ProjectNumber = m_ProjectWholeInfoViewModelUpdate.projectBasicinfo.ProjectNumber;
        //    m_ProjectWholeInfoViewModelUpdate.projectTestBasicinfoList = this.IDKLManagerService.GetProjectTestBasicInfoList(request).ToList();

        //    return View("Edit", m_ProjectWholeInfoViewModelUpdate);
        //}
        //[HttpPost]
        //public ActionResult Submit(int id, FormCollection collection)
        //{
        //    HttpFileCollectionBase files = Request.Files;
        //    HttpPostedFileBase file = files["docFile"];
        //    string fileName = "";
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        fileName = GetFilePathByRawFile(file.FileName);
        //        file.SaveAs(fileName);
        //    }

        //    {
        //        m_ProjectWholeInfoViewModelUpdate.projectBasicinfo.ProjectStatus = (int)EnumProjectSatus.ProjectModifyFour;
        //        var projectBasicDocFile = new ProjectDocFile();
        //        projectBasicDocFile.CreateTime = DateTime.Now;
        //        projectBasicDocFile.FilePath = fileName;
        //        projectBasicDocFile.ProjectNumber = m_ProjectWholeInfoViewModelUpdate.projectBasicinfo.ProjectNumber;
        //        projectBasicDocFile.Status = m_ProjectWholeInfoViewModelUpdate.projectBasicinfo.ProjectStatus;
        //        this.IDKLManagerService.AddProjectDocFile(projectBasicDocFile);
        //        this.IDKLManagerService.UpdateProjectInfo(m_ProjectWholeInfoViewModelUpdate.projectBasicinfo);
        //    }
        //    //循环添加检测数据到数据库
        //    foreach (var data in m_ProjectWholeInfoViewModelUpdate.projectTestBasicinfoList)
        //    {
        //        this.IDKLManagerService.AddProjectTestBasicInfo(data);
        //    }

        //    ///返回列表
        //    var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
        //    ProjectInfoRequest request = new ProjectInfoRequest();
        //    request.UserAccountType = user.AccountType;
        //    request.userName = user.Name;
        //    request.ProjectStatus = (int)EnumProjectSatus.ProjectVerifyOne;
        //    request.ProjectCategory = (int)EnumProjectCategory.TestValue;
        //    var result = this.IDKLManagerService.GetProjectInfoList(request);
        //    return View("Index", result);
        //}
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

            //1.判断是否满足下载条件
            //2.从各个数据库获取数据
            //3.调用接口生成报告
            //目前报告均没有检测数据          
            List<TestPhysicalReport> physicalmodels = new List<TestPhysicalReport>();
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
                ProjectInfo projectmodels = new ProjectInfo();
                List<string> str = new List<string>();          //化学采样项目 控制打印表格时打印的项目
                foreach (var item in chemicalmodels)
                {
                    str.Add(item.SampleProject);
                }
                str = str.Distinct().ToList();
                List<string> strc = new List<string>();                          //物理检测项目(查询，制表用)               
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
                #endregion
                #region 物理检测部分录入
                foreach (var item in m_ProjectWholeInfoViewModel.projectTestBasicinfoList)
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
                    physicalmodels.Add(modeltemp);                   
                    strc.Add(item.TestContent);
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
                CreateWord cr = new CreateWord();

                List<string> appList = cr.CreateReportWord(str, strc, ProjectGistmodels, Parametermodels, ParmeterChemicalModels, projectmodels, physicalmodels, chemicalmodels);


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

                string strFileName = appList[1];                                        //报告下载
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
                return Back("成功");
            }
        }
    }



           
        
  
   
