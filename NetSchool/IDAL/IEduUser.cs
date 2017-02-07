using System;
using System.Collections.Generic;
using System.Data;

namespace NetSchool.IDAL
{
    public interface IEduUser
    {
        bool Add(NetSchool.Model.EduUser model);
        bool Update(NetSchool.Model.EduUser model);
        bool DeleteListByID(List<Guid> idList);
        DataTable GetALLRolesDistinct();
        DataTable Search(Guid? Id, bool IsSearchCompany, string Company, bool IsSearchNickName, string NickName, bool IsSearchIdcard, string Idcard, bool IsSearchRoles, string Roles, bool IsSearchGender, string Gender,  int? StartTime,int? EndTime,  DateTime? CreateTime, Common.Info.PageInfo pageInfo);
        T[] SearchList<T>(Guid? Id, bool IsSearchCompany, string Company, bool IsSearchNickName, string NickName, bool IsSearchIdcard, string Idcard, bool IsSearchRoles, string Roles, bool IsSearchGender, string Gender, int? StartTime, int? EndTime, DateTime? CreateTime, Common.Info.PageInfo pageInfo) where T : Model.EduUser, new();
        DataTable SearchCount(string Cloumn);
        DataTable SearchDateTime(Common.Enums.StatisticalTime style);
        DataTable SearchStamp(Common.Enums.StampStruct[] StampPara,bool isSearchCompany,string company,bool isSearchIdcard,string idcard,bool isSearchRole,string role,bool isSearchGender,string gender);
    }
}
