using System;
using System.Text;

namespace NetSchool.Common.EnDeCryption
{
    public static class Base64
    {
        public static string Decode(string code, Encoding encode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                    return "";
                byte[] bytes = Convert.FromBase64String(code);
                return encode.GetString(bytes);
            }
            catch
            {
                return "";
            }
        }
        public static string Encode(string code)
        {
            try
            {
                byte[] bytes = Encoding.Default.GetBytes(code);
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return "";
            }
        }
    }
}
