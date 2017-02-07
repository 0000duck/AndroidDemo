using System;
using System.Collections;

namespace NetSchool.Common.EnDeCryption
{
    public static class JCode
    {
        private static readonly Char encondChar='!';
        private static readonly Char spiltChar = '-';
        public static string Encode(string str)
        { 
            string result = string.Empty;
            try
            {
                long[] ascList = new long[str.Length];
                for (int i = 0; i < str.Length; i++)
                {
                    ascList[i] = Convert.ToInt64(str[i]) + Convert.ToInt64(encondChar);
                    result += ascList[i].ToString() + spiltChar;
                }
                result=result.Remove(result.Length - 1);
                return result;
            }
            catch
            {
                return "";
            }
        }
        public static String Decode(String str)
        {
            string result = string.Empty;
            try
            {
                string[]list =str.Split(spiltChar);
                for (int i = 0;  i < list.Length; i++)
                {
                    result += Convert.ToChar(Convert.ToInt64(list[i]) - Convert.ToInt64(encondChar));
                }
                    return result;
            }
            catch
            {
                return "";
            }
        }
    }
}
