using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using OfficeDocGenerate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web.Demo.Areas.DKLManager.Models;
using Web.Demo.Common;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class ThressPersonCheckController : SampleBaseController
    {
        public static IEnumerable<SampleRegisterTable> Sample;
        //
        // GET: /DKLManager/ThressPersonCheck/
        public ActionResult Index(ProjectInfoRequest request)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            request.UserAccountType = user.AccountType;
            request.userName = user.Name;
            request.SampleStates = (int)EnumSampleStates.ThreeCheck;
            var parameNew = this.IDKLManagerService.GetSampleRegisterTableList(user.Name, request);
            Sample = parameNew;
            return View(parameNew);
        }
        public ActionResult ExportExcelFile(FormCollection collection)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", Type.GetType("System.Int32"));
            dt.Columns[0].AutoIncrement = true;
            dt.Columns[0].AutoIncrementSeed = 1;
            dt.Columns[0].AutoIncrementStep = 1;
            dt.Columns.Add("样品登记号", Type.GetType("System.String"));
            dt.Columns.Add("采样日期", Type.GetType("System.DateTime"));
            dt.Columns.Add("样品状态", Type.GetType("System.String"));
            dt.Columns.Add("保存条件", Type.GetType("System.String"));
            dt.Columns.Add("备注", Type.GetType("System.String"));
            dt.Columns.Add("状态", Type.GetType("System.String"));
            dt.Columns.Add("审核人", Type.GetType("System.String"));
            dt.Columns.Add("样品结果", Type.GetType("System.String"));




            foreach (var m in Sample)
            {
                DataRow dtRow = dt.NewRow();

                dtRow["样品登记号"] = "" + m.SampleRegisterNumber;
                dtRow["采样日期"] = "" + m.SamplingDay;
                dtRow["样品状态"] = "" + m.SampleState;
                dtRow["保存条件"] = "" + m.SaveCondition;
                dtRow["备注"] = "" + m.Remark;
                dtRow["状态"] = "" + HYZK.FrameWork.Utility.EnumHelper.GetEnumTitle((EnumSampleStates)@m.SampleStates);
                dtRow["审核人"] = "" + m.AnalyzePeople;
                dtRow["样品结果"] = "" + m.ArgumentPrice;
                dt.Rows.Add(dtRow);
            }


            string fileName = "d://DKLdownload" + Web.Demo.Common.AdminUserContext.Current.LoginInfo.LoginName + "(" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + ").xls";
            string tabName = "采样结果";
            string reMsg = string.Empty;
            bool result = ExcelOP.DataTableExportToExcel(dt, fileName, tabName, ref reMsg);



            string strFileName = fileName;
            if (result && !string.IsNullOrEmpty(strFileName))
            {

                string fileNewName = strFileName.Substring(strFileName.LastIndexOf("\\") + 1);
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = Encoding.UTF8;
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileNewName, Encoding.UTF8));
                Response.WriteFile(strFileName);
                Response.End();
                return Back("成功");
            }
            else
            {
                return Back("导出失败");
            }
        }
        public ActionResult CheckDoc(int id)
        {
            var infomodel = new ProjectInfo();
            var model = new ProjectDocFile();
            var users = this.AccountService.GetUserListQ(5).Select(c => new { Id = c.ID, Name = c.Name });
            ViewData.Add("Person", new SelectList(users, "Name", "Name"));
            ViewData.Add("Status", new SelectList(EnumHelper.GetItemValueList<EnumProjectAgree>(), "Key", "Value"));
            infomodel = this.IDKLManagerService.GetProjectInfo(id);
            if (infomodel != null)
            {
                //根据项目编号和状态查找文件Doc
                model = this.IDKLManagerService.GetProjectDocFile(infomodel.ProjectNumber, infomodel.ProjectStatus, infomodel.ID);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult CheckDoc(int id, FormCollection collection, ProjectInfoRequest request)
        {
            ProjectInfo projectModel = this.IDKLManagerService.GetProjectInfo(id);
            var fileList = this.IDKLManagerService.GetProjectDocFileLists(projectModel.ProjectNumber);
            int flag = 0;
            foreach (var item in fileList)
            {
                if (item.Status == 17)
                {
                    flag = 1;
                    break;
                }
            }
            ////upload doc file
            string retInfo = null;
            //HttpFileCollectionBase files = Request.Files;
            //HttpPostedFileBase file = files["DocFileForUpload"];
            //string fileName = "";
            //if (file != null && file.ContentLength > 0)
            //{
            if (flag != 0)
            {
                //fileName = GetFilePathByRawFile(file.FileName);
                //file.SaveAs(fileName);
                //更新项目信息状态
                var infomodel = new ProjectInfo();
                infomodel = this.IDKLManagerService.GetProjectInfo(id);
                if (request.Status == (int)EnumProjectAgree.DisAgree)
                {
                    var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
                    infomodel.ProjectStatus = (int)EnumProjectSatus.ZhiPingSubmit;
                    infomodel.Person = collection["Person"];
                    var nn = this.IDKLManagerService.SelectContractInfo(infomodel.ProjectName);
                    infomodel.SignTime = nn.ContractDate;
                    var models1 = new TimeInstructions();
                    models1.ProjectNumBer = infomodel.ProjectNumber;
                    models1.ProjectName = infomodel.ProjectName;
                    models1.TimeNode = DateTime.Now;
                    models1.SignTime = infomodel.SignTime.ToString();
                    models1.Instructions = user.LoginName + "未通过";
                    this.IDKLManagerService.InsertTimeInstructions(models1);
                }
                else if (request.Status == (int)EnumProjectAgree.Agree)
                {
                    //    var model = this.IDKLManagerService.GetSampleRegisterTable(id);
                    //    this.TryUpdateModel<SampleRegisterTable>(model);
                    //    this.IDKLManagerService.UpDateSampleRegister(model);
                    var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
                    infomodel.ProjectStatus = (int)EnumProjectSatus.PersonW;
                    infomodel.Person = collection["Person"];
                    var nn = this.IDKLManagerService.SelectContractInfo(infomodel.ProjectName);
                    infomodel.SignTime = nn.ContractDate;
                    var models1 = new TimeInstructions();
                    models1.ProjectNumBer = infomodel.ProjectNumber;
                    models1.ProjectName = infomodel.ProjectName;
                    models1.TimeNode = DateTime.Now;
                    models1.SignTime = infomodel.SignTime.ToString();
                    models1.Instructions = user.LoginName + "通过";
                    this.IDKLManagerService.InsertTimeInstructions(models1);
                }
                this.IDKLManagerService.UpdateProjectInfo(infomodel);
                //上传审核修改后doc文件记录
                //var model = new ProjectDocFile();
                //this.TryUpdateModel<ProjectDocFile>(model);
                //model.ProjectNumber = infomodel.ProjectNumber;
                //model.Status = infomodel.ProjectStatus;
                //model.FilePath = fileName;
                //this.IDKLManagerService.AddProjectDocFile(model);
            }
            else
            {
                retInfo = GlobalData.warningInfo1;
            }
            return this.RefreshParent(retInfo);
        }
        public ActionResult Check(int id, ProjectInfoRequest request)
        {
            var model = this.IDKLManagerService.GetSampleRegisterTable(id);
            return View("Edit",model);
        }
        [HttpPost]
        public ActionResult Check(int id, FormCollection collection)
        {
            var model = this.IDKLManagerService.GetSampleRegisterTable(id);
            this.TryUpdateModel<SampleRegisterTable>(model);
            this.IDKLManagerService.UpDateSampleRegister(model);
            return this.RefreshParent();
        }
	}
}