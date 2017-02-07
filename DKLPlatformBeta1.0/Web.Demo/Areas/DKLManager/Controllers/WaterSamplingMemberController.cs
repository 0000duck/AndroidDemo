using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Common;
using HYZK.FrameWork.Common;
using Web.Demo.Areas.DKLManager.Models;
using System.Drawing.Imaging;
using System.IO;
using HYZK.Core.Upload;
using OfficeDocGenerate;
using System.Text;
using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using System;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    
    public class WaterSamplingMemberController : AdminControllerBase
    {
        private static ProjectWholeInfoViewModel m_ProjectWholeInfoViewModel;
        //
        // GET: /DKLManager/WaterSamplingMember/
        public ActionResult Index(ProjectInfoRequest request)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            ViewData["Name"] = user.Name;
            request.ProjectStatus = (int)EnumProjectSatus.PJSumbit;
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            var result = this.IDKLManagerService.GetProjectInfoList(request);           
            ViewData.Add("ProjectPersonCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectPersonCategory>(), "Key", "Value"));
            return View(result);
        }
        public ActionResult Edit(int id, string projectNumber)
        {
            var parameters = this.IDKLManagerService.GetParameterList().Select(c => new { Name = c.ParameterName }).Distinct();
            //var users = this.AccountService.GetUserList(9).Select(c => new { Id = c.ID, Name = c.Name });
            //ViewData.Add("Person", new SelectList(users, "Name", "Name"));
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
        public ActionResult Edit(int id, string projectNumber, string sampleRegisterNumber, FormCollection collection)
        {
            ViewData.Add("ProjectPersonCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectPersonCategory>(), "Key", "Value"));
            m_ProjectWholeInfoViewModel.projectBasicinfo = this.IDKLManagerService.GetProjectInfo(id);
            if (m_ProjectWholeInfoViewModel.projectBasicinfo != null)
            {
                //m_ProjectWholeInfoViewModel.projectBasicImgFile = this.IDKLManagerService.GetProjectFile(m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber);                
                string fileName = CreateBarCode(m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber);
                if (fileName != null)
                {
                    string filePath = fileName.Substring(fileName.IndexOf("Upload") - 1);
                    ViewData.Add("ProjectBarCodeImg", filePath);
                }
            }
            //获取报告文档
           // HttpFileCollectionBase files = Request.Files;
           // HttpPostedFileBase file = files["docFile"];
          //  string fileName = "";
            //if (file != null && file.ContentLength > 0)
            //{
            //    fileName = GetFilePathByRawFile(file.FileName);
            //    file.SaveAs(fileName);
            //}
            //提交
            //判断所有数据不为空   
           // var model = new SampleRegisterTable();
            #region 判断物理化学是否填写完整部分
            //样品表添加项目编号字段
            //新增方法 根据项目编号获取样品model list
            //根据list判断样品状态是否为2 
            //projectNumber = m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber;
            //List<SampleRegisterTable> models = this.IDKLManagerService.GetSampleRegisterListByProjectNumber(projectNumber);
            //models = this.IDKLManagerService.GetSampleRegisterListByProjectNumber(projectNumber);
            //foreach (var item in models)
            //{
            //    if (item.SampleStates == (int)EnumSampleStates.DoneSample)
            //    {
            //        m_ProjectWholeInfoViewModel.sampleTable.SampleStates = item.SampleStates;
            //        m_ProjectWholeInfoViewModel.sampleTable = item;
            //        break;
            //    }
            //}
            #endregion        
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            ProjectInfoRequest request = new ProjectInfoRequest();
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;            
            request.ProjectCategory = (int)EnumProjectCategory.TestValue;
            request.ProjectCheif = user.Name;
            var result = this.IDKLManagerService.GetProjectInfoList(request);
            var model = new SampleRegisterTable();
            model = this.IDKLManagerService.GetSampleRegisterTable(id);
            model.AnalyzePeople = collection["Person"];
            model.SampleStates = (int)EnumSampleStates.NewSample;
            this.IDKLManagerService.UpDateSampleRegister(model);                      
            var models1 = new TimeInstructions();
            models1.TimeNode = DateTime.Now;         
            models1.Instructions = user.LoginName + "提交实验室";
            this.IDKLManagerService.InsertTimeInstructions(models1);
            return View("Index", result);

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
        [HttpPost]
        public ActionResult Return(int id, FormCollection collection)
        {

            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            var model = this.IDKLManagerService.GetProjectInfo(id);
            var nn = this.IDKLManagerService.SelectContractInfo(model.ProjectName);
            model.SignTime = nn.ContractDate;
            var models = new TimeInstructions();
            models.SignTime = model.SignTime.ToString();
            models.ProjectNumBer = model.ProjectNumber;
            models.ProjectName = model.ProjectName;
            models.TimeNode = DateTime.Now;
            models.Instructions = user.LoginName + "退回";
            this.IDKLManagerService.InsertTimeInstructions(models);
            model.ProjectPersonCategory = 4;
            this.TryUpdateModel<ProjectInfo>(model);
            model.ProjectStatus = (int)EnumProjectSatus.WaterS;
            this.IDKLManagerService.UpdateProjectInfo(model);
            //  return RedirectToAction("Index");
            return this.RefreshParent();
        }
        public ActionResult Return(int id)
        {

            var model = new ProjectInfo();

            return View(model);
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
            model.Factor = collection["projectTestChemicalReport.SampleProject"];
            model.SampleNumber = collection["projectTestChemicalReport.SampleNumber"];
            model.SampleProject = collection["projectTestChemicalReport.SampleProject"];
            m_ProjectWholeInfoViewModel.projectTestChemicalReportList.Add(model);
            this.IDKLManagerService.InsertTestChemicalReport(model);

            return View("Edit", m_ProjectWholeInfoViewModel);
        }
        public ActionResult UpdateChemical(ProjectInfoRequest request, string projectNumber)
        {
            var result = this.IDKLManagerService.GetSampleRegisterTableListEdit(projectNumber);
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
        [HttpPost]
        public ActionResult DownLoadTestDocFile(FormCollection collection)
        {
            //try
            //{
            string strFileName = "";

            //1.判断是否满足下载条件
            //2.从各个数据库获取数据
            //3.调用接口生成报告

            List<string> strc = new List<string>();
            ProjectInfo projectmodels = new ProjectInfo();
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
                strc.Add(item.SampleProject);
            }
            //strc = strc.Distinct().ToList();

            #region 基本信息
            projectmodels.CompaneName = m_ProjectWholeInfoViewModel.projectBasicinfo.CompaneName;
            projectmodels.CompanyAddress = m_ProjectWholeInfoViewModel.projectBasicinfo.CompanyAddress;
            projectmodels.ProjectName = m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectName;
            projectmodels.CompanyContact = m_ProjectWholeInfoViewModel.projectBasicinfo.CompanyContact;
            projectmodels.ZipCode = m_ProjectWholeInfoViewModel.projectBasicinfo.ZipCode;
            projectmodels.ContactTel = m_ProjectWholeInfoViewModel.projectBasicinfo.ContactTel;
            projectmodels.ProjectNumber = m_ProjectWholeInfoViewModel.projectBasicinfo.ProjectNumber;
            #endregion

            #region 报告生成+下载
            CreatWaterTestDoc cr = new CreatWaterTestDoc(projectmodels);
            if ((projectmodels != null) && (chemicalmodels != null))
            {
                List<string> appList = new List<string>();

                appList = cr.CreateReportWord(strc, projectmodels, chemicalmodels);


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
            }
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

            #endregion
                //}
                //catch (Exception e)
                //{
                //    return Back("1"+e.Message); 
                //}
                return Back("成功");
            
        }
        [HttpPost]
        



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
	}
}