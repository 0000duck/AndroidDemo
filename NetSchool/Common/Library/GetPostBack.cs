using System;

namespace NetSchool.Common.Library
{
    public class GetPostBack
    {
        public static string GetPostBackStr(string strName)
        {
            object strValue = System.Web.HttpContext.Current.Request[strName];

            return strValue == null ? string.Empty : strValue.ToString();
        }
        public static Guid GetPostBackGuid(string strName, Guid defValue)
        {
            string strValue = GetPostBackStr(strName);
            Guid gValue = Guid.Empty;
            if (!Guid.TryParse(strValue, out gValue))
            {
                gValue = defValue;
            }

            return gValue;
        }
        public static int GetPostBackInt(string strName, int nDefaultValue)
        {
            string strValue = GetPostBackStr(strName);
            int nValue = 0;
            if (!int.TryParse(strValue, out nValue))
            {
                nValue = nDefaultValue;
            }

            return nValue;
        }

    }

}
