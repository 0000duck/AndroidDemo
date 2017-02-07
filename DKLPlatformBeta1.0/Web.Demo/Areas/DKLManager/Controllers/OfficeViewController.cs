using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Web;  
using System.Web.Mvc;  
using Microsoft.Office.Interop.Excel;  
using System.Diagnostics;  
using System.IO;  
using Microsoft.Office.Interop.Word;
using Web.Demo.Common;
namespace Web.Demo.Areas.DKLManager.Controllers
{
    public class OfficeViewController : AdminControllerBase
    {
        // GET: OfficeView
         #region Index页面  
        /// <summary>  
        /// Index页面  
        /// </summary>  
        /// <paramname="url">例：/uploads/......XXX.xls</param>  
        public ActionResult Index(string url)  
        {  
        //    string physicalPath = Server.MapPath(Server.UrlDecode(url));
            string physicalPath = url;
            string temp = url.Substring(0, url.IndexOf("\\App_Data"));
            temp = temp.Substring(temp.LastIndexOf("\\")+ "\\".Length, temp.Length - temp.LastIndexOf("\\")-"\\".Length);
            url = url.Substring(url.IndexOf(temp), url.Length - url.IndexOf(temp));
            string extension = Path.GetExtension(physicalPath);  
  
            string htmlUrl = "";  
            switch(extension.ToLower())  
            {  
               case ".xls":  
               case ".xlsx":
                   htmlUrl = PreviewExcel(physicalPath, url, temp);  
                   break;  
               case ".doc":  
               case ".docx":
                   htmlUrl = PreviewWord(physicalPath, url, temp);  
                   break;  
               case ".txt":
                   return Back("无法浏览该类型文件，请下载查看");
                //   htmlUrl = PreviewTxt(physicalPath, url);  
              //     break;  
               case ".pdf":
                   return Back("无法浏览该类型文件，请下载查看");
               //    htmlUrl = PreviewPdf(physicalPath, url);  
              //     break;  
               case ".jpg":
                   return Back("无法浏览该类型文件，请下载查看");
               case ".jpeg":
                   return Back("无法浏览该类型文件，请下载查看");
               case ".bmp":
                   return Back("无法浏览该类型文件，请下载查看");
               case ".gif":
                   return Back("无法浏览该类型文件，请下载查看");
               case ".png":
                   return Back("无法浏览该类型文件，请下载查看");
               //    htmlUrl = PreviewImg(physicalPath, url);  
               //    break;  
               default:  
                   htmlUrl = PreviewOther(physicalPath, url);  
                   break;  
            }
            htmlUrl = "ReadFiles" + htmlUrl.Substring(htmlUrl.LastIndexOf("\\"), htmlUrl.Length - htmlUrl.LastIndexOf("\\"));
            return Redirect(Url.Content(htmlUrl));  
        }  
        #endregion  
 
        #region 预览Excel  
        /// <summary>  
        /// 预览Excel  
        /// </summary>  
        public string PreviewExcel(string physicalPath, string url,string findIndex)  
        {  
           Microsoft.Office.Interop.Excel.Application application = null;  
           Microsoft.Office.Interop.Excel.Workbook workbook = null;  
            application= new Microsoft.Office.Interop.Excel.Application();  
            object missing = Type.Missing;  
            object trueObject = true;  
           application.Visible = false;  
           application.DisplayAlerts = false;  
            workbook =application.Workbooks.Open(physicalPath, missing, trueObject, missing, missing,missing,  
               missing, missing, missing, missing, missing, missing, missing, missing,missing);  
            //Save Excelto Html  
            object format = Microsoft.Office.Interop.Excel.XlFileFormat.xlHtml;
          
            string htmlName = Path.GetFileNameWithoutExtension(physicalPath) + ".html";  
            String outputFile = Path.GetDirectoryName(physicalPath) + "\\" + htmlName;
            string temp = outputFile.Substring(0, outputFile.IndexOf(findIndex));
            temp = temp + findIndex+"\\DKLManager\\ReadFiles" + outputFile.Substring(outputFile.LastIndexOf("\\"), outputFile.Length - outputFile.LastIndexOf("\\"));
            outputFile = temp;
            string strPath = Path.GetDirectoryName(outputFile);         //如果文件夹路径不存在就创建
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }
            workbook.SaveAs(outputFile, format, missing, missing, missing,  
                             missing, XlSaveAsAccessMode.xlNoChange, missing,  
                             missing, missing, missing, missing);  
           workbook.Close();  
           application.Quit();  
            return Path.GetDirectoryName(Server.UrlDecode(url)) + "\\" + htmlName;  
        }  
        #endregion  
 
        #region 预览Word  
        /// <summary>  
        /// 预览Word  
        /// </summary>  
        public string PreviewWord(string physicalPath, string url,string findIndex)  
        {  
           Microsoft.Office.Interop.Word._Application application = null;  
           Microsoft.Office.Interop.Word._Document doc = null;  
            application= new Microsoft.Office.Interop.Word.Application();  
            object missing = Type.Missing;  
            object trueObject = true;  
            application.Visible= false;  
           application.DisplayAlerts = WdAlertLevel.wdAlertsNone;  
            doc =application.Documents.Open(physicalPath, missing, trueObject, missing, missing,missing,  
               missing, missing, missing, missing, missing, missing, missing, missing,missing, missing);  
            //Save Excelto Html  
            object format = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatHTML;  
            string htmlName = Path.GetFileNameWithoutExtension(physicalPath) + ".html";  
            String outputFile = Path.GetDirectoryName(physicalPath) + "\\" + htmlName;
            string temp = outputFile.Substring(0, outputFile.IndexOf(findIndex));
            temp = temp + findIndex+ "\\DKLManager\\ReadFiles" + outputFile.Substring(outputFile.LastIndexOf("\\"), outputFile.Length - outputFile.LastIndexOf("\\"));
            outputFile = temp;
            string strPath = Path.GetDirectoryName(outputFile);         //如果文件夹路径不存在就创建
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }
           doc.SaveAs(outputFile, format, missing, missing, missing,  
                             missing, XlSaveAsAccessMode.xlNoChange, missing,  
                             missing, missing, missing, missing);  
            doc.Close();  
           application.Quit();  
            return Path.GetDirectoryName(Server.UrlDecode(url)) + "\\" + htmlName;  
        }  
        #endregion  
 
        #region 预览Txt  
        /// <summary>  
        /// 预览Txt  
        /// </summary>  
        public string PreviewTxt(string physicalPath, string url)  
        {  
            return Server.UrlDecode(url);  
        }  
        #endregion  
 
        #region 预览Pdf  
        /// <summary>  
        /// 预览Pdf  
        /// </summary>  
        public string PreviewPdf(string physicalPath, string url)  
        {  
            return Server.UrlDecode(url);  
        }  
        #endregion  
 
        #region 预览图片  
        /// <summary>  
        /// 预览图片  
        /// </summary>  
        public string PreviewImg(string physicalPath, string url)  
        {  
            return Server.UrlDecode(url);  
        }  
        #endregion  
 
        #region 预览其他文件  
        /// <summary>  
        /// 预览其他文件  
        /// </summary>  
        public string PreviewOther(string physicalPath, string url)  
        {  
            return Server.UrlDecode(url);  
        }  
        #endregion  
    }  
    }
