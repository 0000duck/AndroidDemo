using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = NetOffice.WordApi;
using NetOffice.WordApi.Enums;
using System.IO;
namespace OfficeDocGenerate
{
   public class WriteFirstPage
   {
       public  string ReportNumber; // 报告编号
       public  string Client; // 委托单位
      public WriteFirstPage(string reportnumber,string client)
        {
             this.ReportNumber = reportnumber;
             this.Client = client;
        }
       
    }

 

}
