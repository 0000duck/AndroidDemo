using System;
using System.Data;
using System.Collections.Generic;

namespace NetSchool.IDAL
{
    public interface IPeople
    {
        bool Exists(string UserName);
        bool Add(NetSchool.Model.People model);
        bool Update(NetSchool.Model.People model);
        bool DeleteListByUserName(List<string> userNameList);
        DataTable Search(bool IsSearchUserName, string UserName, bool IsSearchRealName, string RealName, bool IsSearchTelephone, string Telephone, bool IsSearchEmail, string Email, bool IsSearchPermission, string Permission, bool IsSearchPassword, string Password, Common.Info.PageInfo pageInfo);
        T[] SearchList<T>(bool IsSearchUserName, string UserName, bool IsSearchRealName, string RealName, bool IsSearchTelephone, string Telephone, bool IsSearchEmail, string Email, bool IsSearchPermission, string Permission, bool IsSearchPassword, string Password, Common.Info.PageInfo pageInfo) where T : Model.People, new();
    }
}
