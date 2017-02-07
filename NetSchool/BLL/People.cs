using System;
using System.Data;
using System.Collections.Generic;

namespace NetSchool.BLL
{
    public class People
    {
        private static readonly IDAL.IPeople dal = DALFactory.DataAccess.CreatePeople();
        public static bool Exists(string UserName)
        {
            return dal.Exists(UserName);
        }
        public static bool Add(Model.People model)
        {
            return dal.Add(model);
        }
        public static bool Update(Model.People model)
        {
            return dal.Update(model);
        }
        public static bool DeleteListByUserName(List<string> usernameList)
        {
            return dal.DeleteListByUserName(usernameList);
        }
        public static Model.People GetModel(string UserName)
        {
            if (string.IsNullOrWhiteSpace(UserName))
                return null;
            DataTable dt = Search(IsSearchUserName:true, UserName: UserName);
            if (dt.Rows.Count > 0)
                return DataRowToModel(dt.Rows[0]);
            else
                return null;
        }
        public static DataTable Search(bool IsSearchUserName = false, string UserName = null, bool IsSearchRealName = false, string RealName = null, bool IsSearchTelephone = false, string Telephone = null, bool IsSearchEmail = false, string Email = null, bool IsSearchPermission = false, string Permission = null, bool IsSearchPassword = false, string Password = null, Common.Info.PageInfo pageInfo = null)
        {
            return dal.Search(IsSearchUserName, UserName, IsSearchRealName, RealName, IsSearchTelephone, Telephone, IsSearchEmail, Email, IsSearchPermission, Permission, IsSearchPassword, Password, pageInfo);
        }
        public static T[] SearchList<T>(bool IsSearchUserName = false, string UserName = null, bool IsSearchRealName = false, string RealName = null, bool IsSearchTelephone = false, string Telephone = null, bool IsSearchEmail = false, string Email = null, bool IsSearchPermission = false, string Permission = null, bool IsSearchPassword = false, string Password = null,  Common.Info.PageInfo pageInfo = null) where T : Model.People, new()
        {
            return dal.SearchList<T>(IsSearchUserName, UserName, IsSearchRealName, RealName, IsSearchTelephone, Telephone, IsSearchEmail, Email, IsSearchPermission, Permission, IsSearchPassword, Password, pageInfo);
        }
        public static Model.People DataRowToModel(DataRow Rows)
        {
            Model.People model = new Model.People();
            if (Rows["UserName"] != DBNull.Value)
                model.UserName = (string)Rows["UserName"];
            if (Rows["RealName"] != DBNull.Value)
                model.RealName = (string)Rows["RealName"];
            if (Rows["TelePhone"] != DBNull.Value)
                model.TelePhone = (string)Rows["TelePhone"];
            if (Rows["Email"] != DBNull.Value)
                model.Email = (string)Rows["Email"];
            if (Rows["Permission"] != DBNull.Value)
                model.Permission = (string)Rows["Permission"];
            if (Rows["Password"] != DBNull.Value)
                model.Password = (string)Rows["Password"];
            return model;
        }
    }
}
