using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSchool.Common.Library
{
    public class ChangeTime
    {
        public static DateTime GetTime(string timeStamp)
        {
            return StampToDateTime(timeStamp);
        }
        public static int GetStamp(string time)
        {
            return DateTimeToStamp(DateTime.Parse(time));
        }
        // 时间戳转为C#格式时间
        private static DateTime StampToDateTime(string timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            dateTimeStart = dateTimeStart.Add(toNow);
            DateTime d = DateTime.Parse(dateTimeStart.ToLongDateString());
            return dateTimeStart;
        }

        // DateTime时间格式转换为Unix时间戳格式
        private static int DateTimeToStamp(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
    }
}
