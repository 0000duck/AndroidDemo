using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = NetOffice.WordApi;
using NetOffice.WordApi.Enums;
using System.IO;
using HYZK.FrameWork.DAL;
using DKLManager.Contract.Model;
using DKLManager.Contract;
using HYZK.Core.Config;
using HYZK.Core.Cache;
using DKLManager.Dal;
using HYZK.FrameWork.Common;
using HYZK.Account.Contract;
using DKLManager.Bll;

namespace OfficeDocGenerate
{
    public class AttorneyBook
    {
        //Costing model;
        Word.Application m_wordApp;
        Word.Document m_doc;
        Word.Tools.CommonUtils m_utils;
        //string Footer = "联系地址：北京市北京经济开发区西环南路 18号 B座 201-1室" + "电话: 010-51570159   传真:010-51570168" + "E-mail:http//mail.deucalion.com.cn   邮编：100176";
        public AttorneyBook()
        {
           
        }
        public List<string> CreateReportWord()
        {

            string appStatus;
            string strRet = null;
            List<string> appList = new List<string>();  //函数执行状态+文件名
            //测试使用的是fristpage

            using (m_wordApp = new Word.Application())
            {
                m_wordApp.DisplayAlerts = WdAlertLevel.wdAlertsNone;
                m_utils = new Word.Tools.CommonUtils(m_wordApp);
                m_doc = m_wordApp.Documents.Add();
                WriteFirstPage();
                appStatus = "0";
                strRet = SaveFile();
                appList.Add(appStatus);
                appList.Add(strRet);
            }
            return appList;
        }
        private string SaveFile()
        {
            string fileName = "";
            string name = "委托书" + DateTime.Now.ToFileTime();
            //      string documentFile = m_utils.File.Combine("c:\\", name, Word.Tools.DocumentFormat.Normal);
            string path = "d://DKLdownload";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string documentFile = m_utils.File.Combine(path, "委托书", Word.Tools.DocumentFormat.Normal);
            if (FileSatus.FileIsOpen(documentFile) == 1)
            {
                //close the doc file
            }
            else
            {

                // fileName = documentFile;//.Replace(".docx", ".doc");
                fileName = documentFile.Replace(".docx", ".doc");     //如果出现文件损坏就用上边那个
                m_doc.SaveAs(fileName);
                m_wordApp.Quit();

                //if(!string.IsNullOrEmpty(fileName))
                //    System.Diagnostics.Process.Start(fileName);                
            }
            return fileName;
        }
        private void SetPage()
        {
            m_doc.PageSetup.PaperSize = WdPaperSize.wdPaperA4;
            m_doc.PageSetup.TopMargin = (float)70.875;
            m_doc.PageSetup.FooterDistance = (float)70;
            m_doc.PageSetup.LeftMargin = (float)56.7;
            m_doc.PageSetup.RightMargin = (float)56.7;
        }
        private void WriteFirstPage()
        {
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 5;
            m_wordApp.Selection.Font.Size = 25;
            //        m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("委 托 书");

            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.Font.Size = 16;
            for (int i = 0; i < 2; ++i)
                m_wordApp.Selection.TypeText("\r\n");
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("\r\n北京德康莱健康安全科技股份有限公司：");
            m_wordApp.Selection.TypeText("\r\n根据《中华人民共和国职业病防治法》及国家相关规定的要求，现委托贵公司对我单位______________________工作场所___________工作。有关工作内容、费用、时限等，以双方签定的合同为准。");

            for (int i = 0; i < 9; ++i)
                m_wordApp.Selection.TypeText("\r\n");
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.Font.Size = 16;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("\r\n                            年   月   日");


            m_doc.Paragraphs.Add();
            Word.Range range = m_doc.Paragraphs.First.Range;
            //Word.InlineShape shap = range.InlineShapes.AddPicture(@"NETofficeTest\NETofficeTest\bin\Debug\fv.png");


            //m_doc.Sections[1].Footers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.Text = Footer;
            m_wordApp.Selection.TypeText("\r\n");
            Word.PageNumbers pns = m_wordApp.Selection.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterEvenPages].PageNumbers;
            pns.NumberStyle = WdPageNumberStyle.wdPageNumberStyleNumberInDash;
            pns.HeadingLevelForChapter = 0;
            pns.IncludeChapterNumber = false;
            pns.ChapterPageSeparator = WdSeparatorType.wdSeparatorHyphen;
            pns.RestartNumberingAtSection = false;
            pns.StartingNumber = 0;
            object pagenmbetal = WdPageNumberAlignment.wdAlignPageNumberRight;
            object first = true;
            m_wordApp.Selection.Sections[1].Footers[WdHeaderFooterIndex.wdHeaderFooterEvenPages].PageNumbers.Add(pagenmbetal, first);
            m_wordApp.Selection.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterEvenPages].PageNumbers.Add(pagenmbetal, first);

        }
        private void SetCellHeaderText(string text)
        {
            SetCellText(1, 12, text, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
        }
        private void SetCellText(int bold, int size, string text, WdParagraphAlignment wAlign, WdCellVerticalAlignment vAlign)
        {
            m_wordApp.Selection.Cells.VerticalAlignment = vAlign;
            SetParaText(bold, size, text, wAlign);
        }
        private void SetParaText(int bold, int size, string text, WdParagraphAlignment wAlign)
        {
            m_wordApp.Selection.Font.Bold = bold;
            m_wordApp.Selection.Font.Color = WdColor.wdColorBlack;
            m_wordApp.Selection.Font.Size = size;
            m_wordApp.Selection.ParagraphFormat.Alignment = wAlign;
            m_wordApp.Selection.TypeText(text);
        }
        private void TypeText(int bold, int size, string text)
        {
            m_wordApp.Selection.Font.Bold = bold;
            m_wordApp.Selection.Font.Color = WdColor.wdColorBlack;
            m_wordApp.Selection.Font.Size = size;
            m_wordApp.Selection.TypeText(text);
        }
    }
}
