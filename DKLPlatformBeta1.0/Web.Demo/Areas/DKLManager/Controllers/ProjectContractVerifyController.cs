using DKLManager.Contract.Model;
using HYZK.FrameWork.Utility;
using System;
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
using HYZK.Account.Contract;

namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class ProjectContractVerifyController : AdminControllerBase
    {
        //
        // GET: /DKLManager/ProjectContractVerify/
        public static ProjectContract UploadModel = new ProjectContract();
        public ActionResult Index(ProjectInfoRequest request)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            if (user.SecondStatus == true)
            {
                request.UserAccountType = user.SecondAccountType;
            }
            else
            {
                request.UserAccountType = user.AccountType;
            }

            request.userName = user.Name;
            switch (request.UserAccountType)
            {
                case 11:
                    {
                        var result = this.IDKLManagerService.GetProjectContractListMarket(request);
                        return View(result);
                    }
                case 5:
                    {
                        var result = this.IDKLManagerService.GetProjectContractListTest(request);
                        return View(result);
                    }
                case 17:
                    {
                        var result = this.IDKLManagerService.GetProjectContractListWorker(request);
                        return View(result);
                    }
                case 4:
                    {
                        var result = this.IDKLManagerService.GetProjectContractListQuality(request);
                        return View(result);
                    }
                case 12:
                    {
                        var result = this.IDKLManagerService.GetProjectContractListFinancial(request);
                        return View(result);
                    }
                case 13:
                    {
                        var result = this.IDKLManagerService.GetProjectContractListJob(request);
                        return View(result);
                    }
                default:
                    return Back("请求错误，请切换账户");

            }
            
        }
        //    if( request.UserAccountType ==11)
        //    {
        //        var result = this.IDKLManagerService.GetProjectContractListMarket(request);
        //        return View(result);    
        //    }
        //    if( request.UserAccountType == 5)
        //    {
        //        var result = this.IDKLManagerService.GetProjectContractListTest(request);
        //        return View(result);    
        //    }
        //    if(request.UserAccountType == 17)
        //    {
        //        var result = this.IDKLManagerService.GetProjectContractListWorker(request);
        //        return View(result);    
        //    }
        //    if (request.UserAccountType == 4)
        //    {
        //        var result = this.IDKLManagerService.GetProjectContractListQuality(request);
        //        return View(result);    
        //    }
        //    if (request.UserAccountType == 12)
        //    {
        //        var result = this.IDKLManagerService.GetProjectContractListFinancial(request);
        //        return View(result);
        //    }
        //    if (request.UserAccountType == 13)
        //    {
        //        var result = this.IDKLManagerService.GetProjectContractListJob(request);
        //        return View(result);
        //    }
           
        //    return View();           
        //}
        public ActionResult Edit(int id)
        {
            var model = new ProjectContract();
            model = this.IDKLManagerService.GetProjectContractInfo(id);
            var problem = new ContractProblem();
            problem.MarketStatus = model.MarketStatus;
            problem.WorkerStatus = model.WorkerStatus;
            problem.TestStatus = model.TestStatus;
            problem.QualityStatus = model.QualityStatus;
            problem.GeneralAccountingDepartmentStatus = model.GeneralAccountingDepartmentStatus;
            problem.EstimateStatus = model.EstimateStatus;
            if (model.ProjectCategory != 5)
            {
                if (model.EstimateStatusProblem == null || model.GeneralAccountingDepartmentStatusProblem == null || model.MarketStatusProblem == null || model.TestStatusProblem == null || model.QualityStatusProblem == null || model.WorkerStatusProblem == null)
                {
                    string alert = "本合同未审核的部门有：";
                    if (model.EstimateStatus == 0)
                        alert += "职评部  ";
                    if (model.GeneralAccountingDepartmentStatus == 0)
                        alert += "财务部  ";
                    if (model.MarketStatus == 0)
                        alert += "市场部  ";
                    if (model.TestStatus == 0)
                        alert += "检测部  ";
                    if (model.QualityStatus == 0)
                        alert += "质管部  ";
                    if (model.WorkerStatus == 0)
                        alert += "技术负责人  ";
                    alert += "请联系相关部门参与审核";
                    return this.RefreshParent(alert);
                }
            }
            else
            {
                if (model.GeneralAccountingDepartmentStatusProblem == null || model.MarketStatusProblem == null || model.TestStatusProblem == null || model.QualityStatusProblem == null || model.WorkerStatusProblem == null)
                {
                    string alert = "本合同未审核的部门有：";
                    if (model.EstimateStatus == 0)
                        alert += "职评部  ";
                    if (model.GeneralAccountingDepartmentStatus == 0)
                        alert += "财务部  ";
                    if (model.MarketStatus == 0)
                        alert += "市场部  ";
                    if (model.TestStatus == 0)
                        alert += "检测部  ";
                    if (model.QualityStatus == 0)
                        alert += "质管部  ";
                    if (model.WorkerStatus == 0)
                        alert += "技术负责人  ";
                    alert += "请联系相关部门参与审核";
                    return this.RefreshParent(alert);
                }
            }
            problem.listMarket = new List<string>(model.MarketStatusProblem.Split(','));
            problem.listTest = new List<string>(model.TestStatusProblem.Split(','));
            problem.listWorker = new List<string>(model.WorkerStatusProblem.Split(','));
            problem.listQuality = new List<string>(model.QualityStatusProblem.Split(','));
            problem.listGeneralAccountingDepartment = new List<string>(model.GeneralAccountingDepartmentStatusProblem.Split(','));
            problem.listEstimate = new List<string>(model.EstimateStatusProblem.Split(','));
            return View(problem);
        }
        [HttpPost]
        public ActionResult Edit()
        {
            return this.RefreshParent();
        }
        public ActionResult Submit(int id)
        {

            var model = this.IDKLManagerService.GetProjectContractInfo(id);
            ViewData.Add("ProjectCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectCategory>(), "Key", "Value", model.ProjectCategory));

            if (model.WorkerStatus == 2 && model.TestStatus == 2 && model.QualityStatus == 2 && model.MarketStatus == 2 && model.EstimateStatus == 2 && model.GeneralAccountingDepartmentStatus == 2)
            {

                return View("Refer", model);
            }
            else
                return this.RefreshParent("审核工作未完成或未通过审核，请点击查询审核状态了解更多");
        }
        [HttpPost]
        public ActionResult Submit(int id, FormCollection collection)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            var model = this.IDKLManagerService.GetProjectContractInfo(id);
            model.ProjectStatus = -2;
            ProjectInfo project = new ProjectInfo();
            project.ProjectCategory = model.ProjectCategory;
            project.ProjectNumber = model.ProjectNumber;
            project.ProjectName = model.ProjectName;
            project.CompaneName = model.CompaneName;
            project.CompanyAddress = model.CompanyAddress;
            project.CompanyContact = model.CompanyContact;
            project.ContactTel = model.ContactTel;
            project.ZipCode = model.ZipCode;
            project.ProjectClosingDate = model.ProjectClosingDate;
            project.CreateTime = DateTime.Now;
            project.ProjectStatus = 2;
            project.Area = model.Area;
            project.MakeOutAnInvoiceTime = DateTime.Now;
            project.Person = user.Name;



            var nn = this.IDKLManagerService.SelectContractInfo(model.ProjectName);
            project.SignTime = nn.ContractDate;
            project.ProjectName = nn.ProjectName;
            project.ProjectNumber = nn.ProjectNumber;
            var models = new TimeInstructions();
            models.SignTime = project.SignTime.ToString();
            models.ProjectNumBer = project.ProjectNumber;
            models.ProjectName = project.ProjectName;
            models.Instructions = user.LoginName + "项目已创建成功到质管部";
            models.SignTime = project.SignTime.ToLongDateString();
            models.TimeNode = DateTime.Now;
            this.IDKLManagerService.InsertTimeInstructions(models);
            this.IDKLManagerService.AddProjectInfo(project);
            ProjectFile file = new ProjectFile();
            file.ProjectNumber = project.ProjectNumber;
            file.FilePath = "";
            file.CreateTime = project.CreateTime;


            this.IDKLManagerService.AddProjectFile(file);
            this.IDKLManagerService.UpdateProjectContract(model);
            return this.RefreshParent("提交成功,项目已创建,如需上传文件请通知质管部");
        }
        public ActionResult Course(string ProjectName, string SignTime)
        {

            List<TimeInstructions> result = new List<TimeInstructions>();
            if (ProjectName != null && SignTime != null)
            {

                result = this.IDKLManagerService.SelectTimeInstructions(ProjectName, SignTime);
            }
            return View(result);
        }
        public ActionResult Verify(ProjectInfoRequest request,int id)
        {
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            if (user.SecondStatus == true)
            {
                request.UserAccountType = user.SecondAccountType;
            }
            else
            {
                request.UserAccountType = user.AccountType;
            }
          
            var model = this.IDKLManagerService.GetProjectContractInfo(id);
            if (model.ProjectCategory != 5)
            {
                switch (request.UserAccountType)
                {
                    case 11:
                        return View("VerifyMarket", model);
                    case 5:
                        return View("VerifyTest", model);

                    case 4:
                        return View("VerifyQuality", model);
                    case 17:
                        return View("VerifyWorker", model);
                    case 12:
                        return View("VerifyGeneralAccountingDepartment", model);
                    case 13:
                        return View("VerifyEstimate", model);
                    default:
                        return Back("此用户不支持审核合同，请切换第二职位");
                    
                }
            }
            else
            {
                switch (request.UserAccountType)
                {
                    case 11:
                        return View("VerifyMarket", model);
                    case 5:
                        return View("VerifyNew", model);

                    case 4:
                        return View("VerifyQuality", model);
                    case 17:
                        return View("VerifyNew", model);
                    case 12:
                        return View("VerifyGeneralAccountingDepartment", model);
                    default:
                        return Back("此用户不支持审核合同，请切换第二职位");
                }
            }
           
        }
        [HttpPost]
        public ActionResult Verify(ProjectInfoRequest request, int id, List<int> ids,FormCollection collection)
        {
            var project = this.IDKLManagerService.GetProjectContractInfo(id);
            List<int> list = new List<int>();         
            var user = this.AccountService.GetUser(this.LoginInfo.LoginName);
            if (user.SecondStatus == true)
            {
                request.UserAccountType = user.SecondAccountType;
            }
            else
            {
                request.UserAccountType = user.AccountType;
            }
          
            if (collection["ProjectNumber"] == "" || collection["ProjectNumber"] == null)
                if (request.UserAccountType == 11)
                    return this.RefreshParent("请填写项目编号！");
            if (request.UserAccountType == 11)
            {
                list.Add(1);
                list.Add(2);
                list.Add(3);
                var model = this.IDKLManagerService.GetProjectContractInfo(id);
                this.TryUpdateModel(model);
                if (ids == null || ids.Count != 3)
                {
                    model.MarketStatus = 1;
                    if (ids != null)
                    {
                        foreach (var item in ids)
                        {
                            list.Remove(item);
                        }
                        model.MarketStatusProblem = string.Join(",", list);
                        this.IDKLManagerService.UpdateProjectContract(model);
                    }
                    else
                    {
                        model.MarketStatusProblem = string.Join(",", list);
                        this.IDKLManagerService.UpdateProjectContract(model);
                    }
                }
                else
                {
                    if (model.EstimateStatus == 2 && model.QualityStatus == 2 && model.GeneralAccountingDepartmentStatus == 2 && model.TestStatus == 2 && model.WorkerStatus == 2)
                    {
                        Sumbit(id, project, user, model);
                        return this.RefreshParent("提交成功,项目已创建,如需上传文件请通知质管部");
                    }
                    else
                    {
                        model.MarketStatus = 2;
                        model.MarketStatusProblem = "";
                        this.IDKLManagerService.UpdateProjectContract(model);
                    }
                }
                var models = new TimeInstructions();
                models.ProjectNumBer = model.ProjectNumber;
                models.TimeNode = DateTime.Now;
                models.SignTime = model.ContractDate.ToString();
                int m = request.UserAccountType;
                if (m == 11)
                {
                    string s = m.ToString();
                    models.Instructions = "市场主管审核完毕";
                    models.ProjectName = model.ProjectName;
                    this.IDKLManagerService.InsertTimeInstructions(models);
                }
            }
            if (request.UserAccountType == 4)
            {
                list.Add(1);
                list.Add(2);
                list.Add(3);
                var model = this.IDKLManagerService.GetProjectContractInfo(id);
                if (ids == null || ids.Count != 3)
                {
                    model.QualityStatus = 1;
                    if (ids != null)
                    {
                        foreach (var item in ids)
                        {
                            list.Remove(item);
                        }
                        model.QualityStatusProblem = string.Join(",", list);
                        this.IDKLManagerService.UpdateProjectContract(model);
                    }
                    else
                    {
                        model.QualityStatusProblem = string.Join(",", list);
                        this.IDKLManagerService.UpdateProjectContract(model);
                    }
                }
                else
                {
                    if (model.MarketStatus == 2 && model.EstimateStatus == 2 && model.GeneralAccountingDepartmentStatus == 2 && model.TestStatus == 2 && model.WorkerStatus == 2)
                    {
                        Sumbit(id, project, user, model);
                        return this.RefreshParent("提交成功,项目已创建,如需上传文件请通知质管部");
                    }
                    else
                    {
                        model.QualityStatus = 2;
                        model.QualityStatusProblem = "";
                        this.IDKLManagerService.UpdateProjectContract(model);
                    }
                }
                var models = new TimeInstructions();
                models.ProjectNumBer = model.ProjectNumber;
                models.TimeNode = DateTime.Now;
                models.SignTime = model.ContractDate.ToString();
                int m = request.UserAccountType;
                if (m == 4)
                {
                    string s = m.ToString();
                    models.Instructions = "质管部审核完毕";
                    models.ProjectName = model.ProjectName;
                    this.IDKLManagerService.InsertTimeInstructions(models);
                }
            }
            if (request.UserAccountType == 12)
            {
                list.Add(1);
                list.Add(2);

                var model = this.IDKLManagerService.GetProjectContractInfo(id);
                if (ids == null || ids.Count != 2)
                {
                    model.GeneralAccountingDepartmentStatus = 1;
                    if (ids != null)
                    {
                        foreach (var item in ids)
                        {
                            list.Remove(item);
                        }
                        model.GeneralAccountingDepartmentStatusProblem = string.Join(",", list);
                        this.IDKLManagerService.UpdateProjectContract(model);
                    }
                    else
                    {
                        model.GeneralAccountingDepartmentStatusProblem = string.Join(",", list);
                        this.IDKLManagerService.UpdateProjectContract(model);
                    }
                }
                else
                {
                    if (model.MarketStatus == 2 && model.EstimateStatus == 2 && model.QualityStatus == 2 && model.TestStatus == 2 && model.WorkerStatus == 2)
                    {
                        Sumbit(id, project, user, model);
                        return this.RefreshParent("提交成功,项目已创建,如需上传文件请通知质管部");
                    }
                    else
                    {
                        model.GeneralAccountingDepartmentStatus = 2;
                        model.GeneralAccountingDepartmentStatusProblem = "";
                        this.IDKLManagerService.UpdateProjectContract(model);
                    }
                }
                var models = new TimeInstructions();
                models.ProjectNumBer = model.ProjectNumber;
                models.TimeNode = DateTime.Now;
                models.SignTime = model.ContractDate.ToString();
                int m = request.UserAccountType;
                if (m == 12)
                {
                    string s = m.ToString();
                    models.Instructions = "财务部审核完毕";
                    models.ProjectName = model.ProjectName;
                    this.IDKLManagerService.InsertTimeInstructions(models);
                }
            }
            if (project.ProjectCategory != 5)
            {
                if (request.UserAccountType == 5)
                {
                    list.Add(1);
                    list.Add(2);
                    list.Add(3);
                    list.Add(4);
                    list.Add(5);
                    list.Add(6);
                    list.Add(7);

                    var model = this.IDKLManagerService.GetProjectContractInfo(id);
                    if (ids == null || ids.Count != 7)
                    {
                        model.TestStatus = 1;
                        if (ids != null)
                        {
                            foreach (var item in ids)
                            {
                                list.Remove(item);
                            }
                            model.TestStatusProblem = string.Join(",", list);
                            this.IDKLManagerService.UpdateProjectContract(model);
                        }
                        else
                        {
                            model.TestStatusProblem = string.Join(",", list);
                            this.IDKLManagerService.UpdateProjectContract(model);
                        }
                    }
                    else
                    {
                        if (model.MarketStatus == 2 && model.EstimateStatus == 2 && model.QualityStatus == 2 && model.GeneralAccountingDepartmentStatus == 2 && model.WorkerStatus == 2)
                        {
                            Sumbit(id, project, user, model);
                            return this.RefreshParent("提交成功,项目已创建,如需上传文件请通知质管部");
                        }
                        else
                        {
                            model.TestStatus = 2;
                            model.TestStatusProblem = " ";
                            this.IDKLManagerService.UpdateProjectContract(model);
                        }
                    }
                    var models = new TimeInstructions();
                    models.ProjectNumBer = model.ProjectNumber;
                    models.TimeNode = DateTime.Now;
                    models.SignTime = model.ContractDate.ToString();
                    int m = request.UserAccountType;
                    if (m == 5)
                    {
                        string s = m.ToString();
                        models.Instructions = "监测评价主管审核完毕";
                        models.ProjectName = model.ProjectName;
                        this.IDKLManagerService.InsertTimeInstructions(models);
                    }

                }
               
                if (request.UserAccountType == 17)
                {
                    list.Add(1);
                    list.Add(2);
                    var model = this.IDKLManagerService.GetProjectContractInfo(id);

                    if (ids == null || ids.Count != 2)
                    {
                        model.WorkerStatus = 1;
                        if (ids != null)
                        {
                            foreach (var item in ids)
                            {
                                list.Remove(item);
                            }

                            model.WorkerStatusProblem = string.Join(",", list);
                            this.IDKLManagerService.UpdateProjectContract(model);
                        }
                        else
                        {
                            model.WorkerStatusProblem = string.Join(",", list);
                            this.IDKLManagerService.UpdateProjectContract(model);
                        }

                    }
                    else
                    {
                        if (model.MarketStatus == 2 && model.EstimateStatus == 2 && model.QualityStatus == 2 && model.GeneralAccountingDepartmentStatus == 2 && model.TestStatus == 2)
                        {
                            Sumbit(id, project, user, model);
                            return this.RefreshParent("提交成功,项目已创建,如需上传文件请通知质管部");
                        }
                        else
                        {
                            model.WorkerStatus = 2;
                            model.WorkerStatusProblem = "";
                            this.IDKLManagerService.UpdateProjectContract(model);
                        }
                    }
                    var models = new TimeInstructions();
                    models.ProjectNumBer = model.ProjectNumber;
                    models.TimeNode = DateTime.Now;
                    models.SignTime = model.ContractDate.ToString();
                    int m = request.UserAccountType;
                    if (m == 17)
                    {
                        string s = m.ToString();
                        models.Instructions = "技术负责人审核完毕";
                        models.ProjectName = model.ProjectName;
                        this.IDKLManagerService.InsertTimeInstructions(models);
                    }
                }
                
                if (request.UserAccountType == 13)
                {
                    list.Add(1);
                    list.Add(2);
                    list.Add(3);
                    var model = this.IDKLManagerService.GetProjectContractInfo(id);
                    if (ids == null || ids.Count != 3)
                    {
                        model.EstimateStatus = 1;
                        if (ids != null)
                        {
                            foreach (var item in ids)
                            {
                                list.Remove(item);
                            }
                            model.EstimateStatusProblem = string.Join(",", list);
                            this.IDKLManagerService.UpdateProjectContract(model);
                        }
                        else
                        {
                            model.EstimateStatusProblem = string.Join(",", list);
                            this.IDKLManagerService.UpdateProjectContract(model);
                        }
                    }

                    else
                    {
                        if (model.MarketStatus == 2 && model.QualityStatus == 2 && model.GeneralAccountingDepartmentStatus == 2 && model.TestStatus == 2 && model.WorkerStatus == 2)
                        {
                            Sumbit(id, project, user, model);
                            return this.RefreshParent("提交成功,项目已创建,如需上传文件请通知质管部");
                        }
                        else
                        {
                            model.EstimateStatus = 2;
                            model.EstimateStatusProblem = "";
                            this.IDKLManagerService.UpdateProjectContract(model);
                        }
                    }
                    var models = new TimeInstructions();
                    models.ProjectNumBer = model.ProjectNumber;
                    models.TimeNode = DateTime.Now;
                    models.SignTime = model.ContractDate.ToString();
                    int m = request.UserAccountType;
                    if (m == 13)
                    {
                        string s = m.ToString();
                        models.Instructions = "职评部审核完毕";
                        models.ProjectName = model.ProjectName;
                        this.IDKLManagerService.InsertTimeInstructions(models);
                    }
                }
            }
            else
            {
                if (request.UserAccountType == 5)
                {
                    list.Add(1);
                    list.Add(2);
                    list.Add(3);
                    list.Add(4);
                    list.Add(5);
                    list.Add(6);
                    list.Add(7);
                    list.Add(8);
                    list.Add(9);

                    var model = this.IDKLManagerService.GetProjectContractInfo(id);
                    if (ids == null || ids.Count != 9)
                    {
                        model.TestStatus = 1;
                        model.WorkerStatus = 1;
                        if (ids != null)
                        {
                            foreach (var item in ids)
                            {
                                list.Remove(item);
                            }
                            model.TestStatusProblem = string.Join(",", list);
                            this.IDKLManagerService.UpdateProjectContract(model);
                        }
                        else
                        {
                            model.TestStatusProblem = string.Join(",", list);
                            this.IDKLManagerService.UpdateProjectContract(model);
                        }
                    }
                    else
                    {
                        if (model.MarketStatus == 2 && model.EstimateStatus == 2 && model.QualityStatus == 2 && model.GeneralAccountingDepartmentStatus == 2)
                        {
                            Sumbit(id, project, user, model);
                            return this.RefreshParent("提交成功,项目已创建,如需上传文件请通知质管部");
                        }
                        else
                        {
                            model.WorkerStatus = 2;
                            model.WorkerStatusProblem = "";
                            model.TestStatus = 2;
                            model.TestStatusProblem = " ";
                            this.IDKLManagerService.UpdateProjectContract(model);
                        }
                    }
                    var models = new TimeInstructions();
                    models.ProjectNumBer = model.ProjectNumber;
                    models.TimeNode = DateTime.Now;
                    models.SignTime = model.ContractDate.ToString();
                    int m = request.UserAccountType;
                    if (m == 5)
                    {
                        string s = m.ToString();
                        models.Instructions = "技术负责人，项目负责人审核完毕";
                        models.ProjectName = model.ProjectName;
                        this.IDKLManagerService.InsertTimeInstructions(models);
                    }

                }

                if (request.UserAccountType == 17)
                {
                    list.Add(1);
                    list.Add(2);
                    list.Add(3);
                    list.Add(4);
                    list.Add(5);
                    list.Add(6);
                    list.Add(7);
                    list.Add(8);
                    list.Add(9);

                    var model = this.IDKLManagerService.GetProjectContractInfo(id);

                    if (ids == null || ids.Count != 9)
                    {
                        model.TestStatus = 1;
                        model.WorkerStatus = 1;
                        if (ids != null)
                        {
                            foreach (var item in ids)
                            {
                                list.Remove(item);
                            }

                            model.WorkerStatusProblem = string.Join(",", list);
                            this.IDKLManagerService.UpdateProjectContract(model);
                        }
                        else
                        {
                            model.WorkerStatusProblem = string.Join(",", list);
                            this.IDKLManagerService.UpdateProjectContract(model);
                        }

                    }
                    else
                    {
                        model.TestStatus = 2;
                        model.TestStatusProblem = " ";
                        model.WorkerStatus = 2;
                        model.WorkerStatusProblem = "";
                        this.IDKLManagerService.UpdateProjectContract(model);
                    }
                    var models = new TimeInstructions();
                    models.ProjectNumBer = model.ProjectNumber;
                    models.TimeNode = DateTime.Now;
                    models.SignTime = model.ContractDate.ToString();
                    int m = request.UserAccountType;
                    if (m == 17)
                    {
                        string s = m.ToString();
                        models.Instructions = "技术负责人,项目负责人审核完毕";
                        models.ProjectName = model.ProjectName;
                        this.IDKLManagerService.InsertTimeInstructions(models);
                    }
                }
            }
            return this.RefreshParent();
        }

        private void Sumbit(int id, ProjectContract project, HYZK.Account.Contract.User user, ProjectContract model)
        {
            var user1 = this.AccountService.GetUser(this.LoginInfo.LoginName);
            var model1 = this.IDKLManagerService.GetProjectContractInfo(id);
            model1.ProjectStatus = -2;
            ProjectInfo project1 = new ProjectInfo();
            project1.ProjectCategory = model1.ProjectCategory;
            project1.ProjectNumber = model1.ProjectNumber;
            project1.ProjectName = model1.ProjectName;
            project1.CompaneName = model1.CompaneName;
            project1.CompanyAddress = model1.CompanyAddress;
            project1.CompanyContact = model1.CompanyContact;
            project1.ContactTel = model1.ContactTel;
            project1.ZipCode = model1.ZipCode;
            project1.ProjectClosingDate = model1.ProjectClosingDate;
            project1.CreateTime = DateTime.Now;
            project1.ProjectStatus = 2;
            project1.Area = model.Area;
            project1.MakeOutAnInvoiceTime = DateTime.Now;
            project1.Person = user.Name;



            var nn = this.IDKLManagerService.SelectContractInfo(model.ProjectName);
            project1.SignTime = nn.ContractDate;
            project1.ProjectName = nn.ProjectName;
            project1.ProjectNumber = nn.ProjectNumber;
            var models = new TimeInstructions();
            models.SignTime = project1.SignTime.ToString();
            models.ProjectNumBer = project1.ProjectNumber;
            models.ProjectName = project1.ProjectName;
            models.Instructions = user1.LoginName + "项目已创建成功到质管部";
            models.SignTime = project1.SignTime.ToLongDateString();
            models.TimeNode = DateTime.Now;
            this.IDKLManagerService.InsertTimeInstructions(models);
            this.IDKLManagerService.AddProjectInfo(project1);
            ProjectFile file = new ProjectFile();
            file.ProjectNumber = project.ProjectNumber;
            file.FilePath = "";
            file.CreateTime = project.CreateTime;


            this.IDKLManagerService.AddProjectFile(file);
            this.IDKLManagerService.UpdateProjectContract(model1);
        }
        //public ActionResult Edit(int id)
        //{

        //    var model = new ProjectContract();
        //    ViewData.Add("ProjectCategory", new SelectList(EnumHelper.GetItemValueList<EnumProjectCategory>(), "Key", "Value", model.ProjectCategory));

        //    model = this.IDKLManagerService.GetProjectContractInfo(id);
        //    return View("Refer", model);
        //}
        public ActionResult CreateWord(int id)
        {
            var money = 0;
            var model = new ProjectContract();
            model = this.IDKLManagerService.GetProjectContractInfo(id);
            money = Convert.ToInt32(model.Money);
            //if (model.MarketStatus != 2 || model.TestStatus != 2 || model.QualityStatus != 2 || model.WorkerStatus != 2||model.GeneralAccountingDepartmentStatus!=2||model.EstimateStatus!=2)
            //{
            //    return this.RefreshParent("本合同审核工作尚未完成，请稍后再试");
            //}
            if (model.ProjectCategory == 2)

                if (money >= 10000)
                {
                    ViewData.Add("ContractType", new SelectList(EnumHelper.GetItemValueList<EnumContractTypeCheck1>(), "Key", "Value"));
                }
                else
                {
                    ViewData.Add("ContractType", new SelectList(EnumHelper.GetItemValueList<EnumContractTypeCheck2>(), "Key", "Value"));
                }
            if (model.ProjectCategory == 1)
                ViewData.Add("ContractType", new SelectList(EnumHelper.GetItemValueList<EnumContractTypeEvaluate>(), "Key", "Value"));
            if (model.ProjectCategory == 0)
                ViewData.Add("ContractType", new SelectList(EnumHelper.GetItemValueList<EnumContractType>(), "Key", "Value"));
            if (model.ProjectCategory == 5)
            {
                return Back("这里没有合同");
            }

            return View(model);
        }
        public ActionResult WORD0(ProjectContract Model)
        {
            string strFileName = "";
            //   CreateContractTestingAndEvaluation cr = new CreateContractTestingAndEvaluation();                       
            CreateContractControlResultEvaluation cr = new CreateContractControlResultEvaluation(Model);
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
        public ActionResult WORD1(ProjectContract Model)
        {
            string strFileName = "";
            //   CreateContractTestingAndEvaluation cr = new CreateContractTestingAndEvaluation();                       
            CreateContractHazardAssessment cr = new CreateContractHazardAssessment(Model);
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
        public ActionResult WORD2(ProjectContract Model)
        {
            string strFileName = "";
            //   CreateContractTestingAndEvaluation cr = new CreateContractTestingAndEvaluation();                       
            CreateContractPre_Assessment cr = new CreateContractPre_Assessment(Model);
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
        public ActionResult WORD3(ProjectContract Model)
        {
            string strFileName = "";
            //   CreateContractTestingAndEvaluation cr = new CreateContractTestingAndEvaluation();                       
            CreateContractTestingAndEvaluation cr = new CreateContractTestingAndEvaluation(Model);
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
        public ActionResult WORD4(ProjectContract Model)
        {
            string strFileName = "";
            //   CreateContractTestingAndEvaluation cr = new CreateContractTestingAndEvaluation();                       
            Testingandevaluationreport cr = new Testingandevaluationreport(Model);
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
        public ActionResult CreateWord(int id, FormCollection collection)
        {
            var model = new ProjectContract();
            model = this.IDKLManagerService.GetProjectContractInfo(id);
            if (model.ProjectCategory == 2)
            {
                if (collection["ContractType"] == "0")
                    WORD3(model);
                if (collection["ContractType"] == "1")
                    WORD4(model);

            }
            if (model.ProjectCategory == 1)
            {
                if (collection["ContractType"] == "0")
                    WORD0(model);
                if (collection["ContractType"] == "1")
                    WORD1(model);
                if (collection["ContractType"] == "2")
                    WORD2(model);

            }
            if (model.ProjectCategory == 0) 
            {
                if (collection["ContractType"] == "0")
                    WORD0(model);
                if (collection["ContractType"] == "1")
                    WORD1(model);
                if (collection["ContractType"] == "2")
                    WORD2(model);
                if (collection["ContractType"] == "3")
                    WORD3(model);
            }

            return this.RefreshParent();
        }
        [HttpPost]
        //public ActionResult Edit()
        //{
        //    return this.RefreshParent();
        //}
        public ActionResult UploadFiles(int id)
        {
            UploadModel = this.IDKLManagerService.GetProjectContractInfo(id);
            ProjectDocFile model = new ProjectDocFile();
            model.ProjectNumber = UploadModel.ProjectNumber;
            return View(model);
        }
        [HttpPost]
        public ActionResult UploadFiles(FormCollection collection)      
        {
            if (collection["ProjectNumber"] == "" || collection["ProjectNumber"] == null)
                return this.RefreshParent("请填写项目编号！");
            //获取报告文档
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = files["docFile"];
            string fileName = "";
            if (file != null && file.ContentLength > 0)
            {
                fileName = GetFilePathByRawFile(file.FileName);
                file.SaveAs(fileName);
            }
            if (file == null || file.ContentLength == 0)
                return Back("未检测到上传文件！");
            var projectBasicDocFile = new ProjectDocFile();
            projectBasicDocFile.FilePath = fileName;
            projectBasicDocFile.CreateTime = DateTime.Now;
            projectBasicDocFile.ProjectNumber = collection["ProjectNumber"];
            UploadModel.ProjectNumber = collection["ProjectNumber"];
            this.IDKLManagerService.UpdateProjectContract(UploadModel);
            this.IDKLManagerService.AddProjectDocFile(projectBasicDocFile);

            return Back("上传成功");
        }
	
	}
}