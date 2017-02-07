using System;
using System.Configuration;
using System.Reflection;

namespace NetSchool.DALFactory
{
    public class DataAccess
    {
        public static readonly string AssemblyPath = "NetSchool." + ConfigurationManager.AppSettings["DAL"];
        public DataAccess() { }
        private static object CreatObject(string AssemblyPath, string classNames)
        {
            object objType = NetSchool.Model.DataCache.GetCache(classNames);
            if (objType == null)
            {
                try
                {
                    objType = Assembly.Load(AssemblyPath).CreateInstance(classNames);
                    NetSchool.Model.DataCache.SetCache(classNames, objType);
                }
                catch
                {

                    return null;
                }
            }
            return objType;

        }
        public static IDAL.IEduCompany CreateEduCompany()
        {
            string ClassNamespace = AssemblyPath + ".EduCompany";
            object objType = CreatObject(AssemblyPath, ClassNamespace);
            return (NetSchool.IDAL.IEduCompany)objType;
        }
        public static IDAL.IEduExamInfo CreateEduExamInfo()
        {
            string ClassNamespace = AssemblyPath + ".EduExamInfo";
            object objType = CreatObject(AssemblyPath, ClassNamespace);
            return (NetSchool.IDAL.IEduExamInfo)objType;
        }
        public static IDAL.IEduLearning CreateEduLearning()
        {
            string ClassNamespace = AssemblyPath + ".EduLearning";
            object objType = CreatObject(AssemblyPath, ClassNamespace);
            return (NetSchool.IDAL.IEduLearning)objType;
        }
        public static IDAL.IEduPayInfo CreateEduPayInfo()
        {
            string ClassNamespace = AssemblyPath + ".EduPayInfo";
            object objType = CreatObject(AssemblyPath, ClassNamespace);
            return (NetSchool.IDAL.IEduPayInfo)objType;
        }
        public static IDAL.IEduUser CreateEduUser()
        {

            string ClassNamespace = AssemblyPath + ".EduUser";
            object objType = CreatObject(AssemblyPath, ClassNamespace);
            return (NetSchool.IDAL.IEduUser)objType;
        }
        public static IDAL.IPeople CreatePeople()
        {

            string ClassNamespace = AssemblyPath + ".People";
            object objType = CreatObject(AssemblyPath, ClassNamespace);
            return (NetSchool.IDAL.IPeople)objType;
        }
        public static IDAL.ILaw CreateLaw()
        {

            string ClassNamespace = AssemblyPath + ".Law";
            object objType = CreatObject(AssemblyPath, ClassNamespace);
            return (NetSchool.IDAL.ILaw)objType;
        }
        public static IDAL.INews CreateNews()
        {

            string ClassNamespace = AssemblyPath + ".News";
            object objType = CreatObject(AssemblyPath, ClassNamespace);
            return (NetSchool.IDAL.INews)objType;
        }
        public static IDAL.INotice CreateNotice()
        {

            string ClassNamespace = AssemblyPath + ".Notice";
            object objType = CreatObject(AssemblyPath, ClassNamespace);
            return (NetSchool.IDAL.INotice)objType;
        }
    }
}
    