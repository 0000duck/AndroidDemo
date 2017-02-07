using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NetSchool.Common.WebBase;
using Microsoft.JScript;
namespace NetSchool.Ajax
{
    public class PeopleManage : AjaxBase
    {

        protected override string SwitchCmd(HttpRequest Request)
        {
            switch (NetSchool.Common.Library.GetPostBack.GetPostBackStr("cmd"))
            {
                case "getList":
                    return GetList();
                case "getInfo":
                    return GetInfo();
                case "addPeople":
                    return AddPeople();
                case "editPeople":
                    return EditPeople();
                case "deletePeople":
                    return DeletePeople();
                case "changePassword":
                    return ChangePassword();
                default:
                    return base.SwitchCmd(Request);
            }
        }

        private static string ChangePassword()
        {
            string strReturn;
            var oldPassword = NetSchool.Common.Library.GetPostBack.GetPostBackStr("oldPassword");
            var newPassword = NetSchool.Common.Library.GetPostBack.GetPostBackStr("newPassword");
            var userName =NetSchool.Common.Info.CurUserInfo.Username;
            if (NetSchool.BLL.People.GetModel(userName).Password == Common.EnDeCryption.Base64.Encode(Common.EnDeCryption.JCode.Encode(oldPassword)))
            {
                NetSchool.Model.People peopleInfo = NetSchool.BLL.People.GetModel(userName);
                peopleInfo.Password = Common.EnDeCryption.Base64.Encode(Common.EnDeCryption.JCode.Encode(newPassword));
                if (NetSchool.BLL.People.Update(peopleInfo))
                {
                    strReturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                }
                else
                {
                    strReturn = JsonConvert.SerializeObject(new { state = "error", msg = "修改失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                }
            }
            else
            {
                strReturn = JsonConvert.SerializeObject(new { state = "error", msg = "密码错误" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            
            return strReturn;
        }
        private static string GetList()
        {
            string strReturn;
            string strSearch = NetSchool.Common.Library.GetPostBack.GetPostBackStr("strSearch");
            int pageindex = NetSchool.Common.Library.GetPostBack.GetPostBackInt("pageindex", 1);
            int pagesize = NetSchool.Common.Library.GetPostBack.GetPostBackInt("pagesize", 20);
            NetSchool.Common.Info.PageInfo pageInfo = new Common.Info.PageInfo();
            pageInfo.PageSize = pagesize;
            pageInfo.CurrentPageIndex = pageindex - 1;
            pageInfo.IsPage = true;
            pageInfo.SortField1 = "RealName";
            pageInfo.SortType1 = NetSchool.Common.Enums.SortType.DESC;

            bool isSearch = false;
            if (strSearch != "")
            {
                isSearch = true;
            }
            var peopleList = NetSchool.BLL.People.SearchList<PeopleWith>(IsSearchRealName: isSearch, RealName: strSearch, pageInfo: pageInfo);
            PackingPeopleList(peopleList);
            strReturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", count = pageInfo.RecordCount, list = peopleList }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strReturn;
        }

        private static string GetInfo()
        {
            string strReturn;
            string userName = NetSchool.Common.Library.GetPostBack.GetPostBackStr("userName");

            if ("" != userName)
            {
                NetSchool.Model.People peopleInfo = NetSchool.BLL.People.GetModel(UserName: userName);
                strReturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK", peopleInfo = peopleInfo }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else
            {
                strReturn = JsonConvert.SerializeObject(new { state = "error", msg = "获取用户名失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            return strReturn;
        }

        private static string AddPeople()
        {
            string strReturn;

            string userName = NetSchool.Common.Library.GetPostBack.GetPostBackStr("userName");
            string realName = NetSchool.Common.Library.GetPostBack.GetPostBackStr("realName");
            if ("" == userName)
            {
                strReturn = JsonConvert.SerializeObject(new { state = "error", msg = "获取用户名失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else
            {
                if ("" == realName)
                {
                    strReturn = JsonConvert.SerializeObject(new { state = "error", msg = "没有填写姓名" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                }
                else
                {
                    if (NetSchool.BLL.People.Exists(userName))
                    {
                        strReturn = JsonConvert.SerializeObject(new { state = "error", msg = "该用户名已存在，不可重复使用" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    }
                    else
                    {
                        NetSchool.Model.People peopleInfo = new Model.People();
                        peopleInfo.UserName = userName;
                        peopleInfo.RealName = realName;
                        peopleInfo.TelePhone = NetSchool.Common.Library.GetPostBack.GetPostBackStr("telephone");
                        peopleInfo.Email = NetSchool.Common.Library.GetPostBack.GetPostBackStr("Email");
                        peopleInfo.Permission = NetSchool.Common.Library.GetPostBack.GetPostBackStr("permission");
                        peopleInfo.Password = Common.EnDeCryption.Base64.Encode(Common.EnDeCryption.JCode.Encode("123456"));//Common.EnDeCryption.
                        if (NetSchool.BLL.People.Add(peopleInfo))
                        {
                            strReturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        }
                        else
                        {
                            strReturn = JsonConvert.SerializeObject(new { state = "error", msg = "添加失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        }
                    }
                }
            }
            return strReturn;
        }

        private static string EditPeople()
        {
            string strReturn;
            string userName = NetSchool.Common.Library.GetPostBack.GetPostBackStr("userName");
            string realName = NetSchool.Common.Library.GetPostBack.GetPostBackStr("realName");
            if ("" == userName)
            {
                strReturn = JsonConvert.SerializeObject(new { state = "error", msg = "获取用户名失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else
            {
                if ("" == realName)
                {
                    strReturn = JsonConvert.SerializeObject(new { state = "error", msg = "没有填写姓名" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                }
                else
                {
                    NetSchool.Model.People peopleInfo = NetSchool.BLL.People.GetModel(userName);
                    peopleInfo.RealName = realName;
                    peopleInfo.TelePhone = NetSchool.Common.Library.GetPostBack.GetPostBackStr("telephone");
                    peopleInfo.Email = NetSchool.Common.Library.GetPostBack.GetPostBackStr("Email");
                    peopleInfo.Permission = NetSchool.Common.Library.GetPostBack.GetPostBackStr("permission");
                    if (NetSchool.BLL.People.Update(peopleInfo))
                    {
                        strReturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    }
                    else
                    {
                        strReturn = JsonConvert.SerializeObject(new { state = "error", msg = "添加失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    }
                }
            }

            return strReturn;
        }

        private static string DeletePeople()
        {
            string strreturn;
            string username = NetSchool.Common.Library.GetPostBack.GetPostBackStr("id");
            List<string> usernameList = new List<string>();
            foreach (string item in username.Split(','))
            {
                usernameList.Add(item);
            }
            if (NetSchool.BLL.People.DeleteListByUserName(usernameList))
            {
                strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            else
            {
                strreturn = JsonConvert.SerializeObject(new { state = "error", msg = "删除失败" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            strreturn = JsonConvert.SerializeObject(new { state = "success", msg = "OK" }, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            return strreturn;
        }


        public class PeopleWith : NetSchool.Model.People
        {
            public string PermissionName;
        }

        private static void PackingPeopleList(IEnumerable<PeopleWith> list)
        {
            string read = Common.Enums.Permission.Read.ToString();
            string Write = Common.Enums.Permission.Write.ToString();
            foreach (var item in list)
            {
                switch (item.Permission)
                {
                    case "0x0001":
                        item.PermissionName = "只读";
                        break;
                    case "0x0002":
                        item.PermissionName = "读写";
                        break;
                }
            }

        }
    }
}