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
    public class CreateJianCe
    {
         Costing model;
        Word.Application m_wordApp;
        Word.Document m_doc;
        Word.Tools.CommonUtils m_utils;
        string Footer = "联系地址：北京市北京经济开发区西环南路 18号 B座 201-1室" + "电话: 010-51570159   传真:010-51570168" + "E-mail:http//mail.deucalion.com.cn   邮编：100176";
        public CreateJianCe(Costing model)
        {
            this.model = model;
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
                WriteFirstPage(model);
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
            string name = "检测项目成本分析表" + DateTime.Now.ToFileTime();
            //      string documentFile = m_utils.File.Combine("c:\\", name, Word.Tools.DocumentFormat.Normal);
            string path = "d://DKLdownload";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string documentFile = m_utils.File.Combine(path, "检测项目成本分析表", Word.Tools.DocumentFormat.Normal);
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
        private void WriteFirstPage(Costing model)
        {
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 5;
            m_wordApp.Selection.Font.Size = 40;
            //        m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("评价项目成本分析表");

            //测试结束
            Word.Table basicInfo = m_doc.Tables.Add(m_wordApp.Selection.Range, 27, 11);
            basicInfo.Borders.Enable = 1;
            basicInfo.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Columns[1].Width = 90;

            basicInfo.Rows[1].Height = 20;
            basicInfo.Cell(1, 1).Merge(basicInfo.Cell(1, 11));
            basicInfo.Cell(1, 1).Select();
            SetCellHeaderText("基本信息");

            basicInfo.Rows[2].Height = 20;
            basicInfo.Cell(2, 1).Merge(basicInfo.Cell(2, 3));
            basicInfo.Cell(2, 1).Select();
            SetCellHeaderText("项目名称");
            basicInfo.Cell(2, 2).Merge(basicInfo.Cell(2, 6));
            basicInfo.Cell(2, 2).Select();
            SetCellHeaderText(model.ProjectName);
            basicInfo.Cell(2, 3).Select();
            SetCellHeaderText("项目简称");
            basicInfo.Cell(2, 4).Merge(basicInfo.Cell(2, 5));
            basicInfo.Cell(2, 4).Select();
            SetCellHeaderText(model.ProjectSynopsis);

            basicInfo.Rows[3].Height = 20;
            basicInfo.Cell(3, 1).Merge(basicInfo.Cell(3, 3));
            basicInfo.Cell(3, 1).Select();
            SetCellHeaderText("业务负责人");
            basicInfo.Cell(3, 2).Merge(basicInfo.Cell(3, 6));
            basicInfo.Cell(3, 2).Select();
            SetCellHeaderText("");
            basicInfo.Cell(3, 3).Select();
            SetCellHeaderText("项目编号");
            basicInfo.Cell(3, 4).Merge(basicInfo.Cell(3, 5));
            basicInfo.Cell(3, 4).Select();
            SetCellHeaderText("");

            basicInfo.Rows[4].Height = 20;
            basicInfo.Cell(4, 1).Merge(basicInfo.Cell(4, 3));
            basicInfo.Cell(4, 1).Select();
            SetCellHeaderText("项目类型");
            basicInfo.Cell(4, 2).Merge(basicInfo.Cell(4, 6));
            basicInfo.Cell(4, 2).Select();
            SetCellHeaderText(HYZK.FrameWork.Utility.EnumHelper.GetEnumTitle((EnumType)@Convert.ToInt32(model.Type)));
            basicInfo.Cell(4, 3).Select();
            SetCellHeaderText("客户区县");
            basicInfo.Cell(4, 4).Merge(basicInfo.Cell(4, 5));
            basicInfo.Cell(4, 4).Select();
            SetCellHeaderText(model.CustomerCounty);

            basicInfo.Rows[5].Height = 20;
            basicInfo.Cell(5, 1).Merge(basicInfo.Cell(5, 3));
            basicInfo.Cell(5, 1).Select();
            SetCellHeaderText("联系人");
            basicInfo.Cell(5, 2).Merge(basicInfo.Cell(5, 6));
            basicInfo.Cell(5, 2).Select();
            SetCellHeaderText(model.ContactsPserson);
            basicInfo.Cell(5, 3).Select();
            SetCellHeaderText("所属乡镇");
            basicInfo.Cell(5, 4).Merge(basicInfo.Cell(5, 5));
            basicInfo.Cell(5, 4).Select();
            SetCellHeaderText(model.Towns);

            basicInfo.Rows[6].Height = 20;
            basicInfo.Cell(6, 1).Merge(basicInfo.Cell(6, 3));
            basicInfo.Cell(6,1).Select();
            SetCellHeaderText("联系方式");
            basicInfo.Cell(6, 2).Merge(basicInfo.Cell(6, 6));
            basicInfo.Cell(6, 2).Select();
            SetCellHeaderText(model.Relation);
            basicInfo.Cell(6, 3).Select();
            SetCellHeaderText("备注");
            basicInfo.Cell(6, 4).Merge(basicInfo.Cell(6, 5));
            basicInfo.Cell(6, 4).Select();
            SetCellHeaderText(model.Remark);

            basicInfo.Rows[7].Height = 20;
            basicInfo.Cell(7, 1).Merge(basicInfo.Cell(7, 11));
            basicInfo.Cell(7, 1).Select();
            SetCellHeaderText("项目利润分析");

            basicInfo.Rows[8].Height = 20;
            basicInfo.Cell(8, 1).Select();
            SetCellHeaderText("");
            basicInfo.Cell(8, 2).Merge(basicInfo.Cell(8, 3));
            basicInfo.Cell(8, 2).Select();
            SetCellHeaderText("项目内容");
            basicInfo.Cell(8, 3).Merge(basicInfo.Cell(8, 8));
            basicInfo.Cell(8, 3).Select();
            SetCellHeaderText("明细");
            basicInfo.Cell(8, 4).Select();
            SetCellHeaderText("金额（元）");
            basicInfo.Cell(8, 5).Select();
            SetCellHeaderText("备注");

            basicInfo.Rows[9].Height = 20;
            basicInfo.Cell(9, 1).Select();
            SetCellHeaderText("1");
            basicInfo.Cell(9, 2).Merge(basicInfo.Cell(9, 3));
            basicInfo.Cell(9, 2).Select();
            SetCellHeaderText("销售额");
            basicInfo.Cell(9, 3).Merge(basicInfo.Cell(9, 8));
            basicInfo.Cell(9, 3).Select();
            SetCellHeaderText("");
            basicInfo.Cell(9, 4).Select();
            SetCellHeaderText(model.Sales.ToString());
            basicInfo.Cell(9, 5).Select();
            SetCellHeaderText("备注");

            basicInfo.Rows[10].Height = 20;
            basicInfo.Cell(10, 1).Select();
            SetCellHeaderText("");
            basicInfo.Cell(10, 2).Select();
            SetCellHeaderText("");
            basicInfo.Cell(10, 3).Select();
            SetCellHeaderText("");
            basicInfo.Cell(10, 4).Select();
            SetCellHeaderText("物理因素");
            basicInfo.Cell(10, 5).Select();
            SetCellHeaderText("");
            basicInfo.Cell(10, 6).Select();
            SetCellHeaderText(model.PhysicalFactors.ToString());
            basicInfo.Cell(10, 7).Select();
            SetCellHeaderText("");
            basicInfo.Cell(10, 8).Select();
            SetCellHeaderText(model.PhysicalFactors.ToString());
            basicInfo.Cell(10, 9).Select();
            SetCellHeaderText((model.PhysicalFactors * model.PhysicalFactorsPrice).ToString());
            basicInfo.Cell(10, 10).Select();
            SetCellHeaderText("");
            basicInfo.Cell(10, 11).Select();
            SetCellHeaderText("");

            basicInfo.Rows[11].Height = 20;
            basicInfo.Cell(11, 1).Select();
            SetCellHeaderText("");
            basicInfo.Cell(11, 2).Select();
            SetCellHeaderText("");
            basicInfo.Cell(11, 3).Select();
            SetCellHeaderText("");
            basicInfo.Cell(11, 4).Select();
            SetCellHeaderText("粉尘类");
            basicInfo.Cell(11, 5).Select();
            SetCellHeaderText("");
            basicInfo.Cell(11, 6).Select();
            SetCellHeaderText(model.Dust.ToString());
            basicInfo.Cell(11, 7).Select();
            SetCellHeaderText("");
            basicInfo.Cell(11, 8).Select();
            SetCellHeaderText(model.DustPrice.ToString());
            basicInfo.Cell(11, 9).Select();
            SetCellHeaderText((model.DustPrice * model.Dust).ToString());
            basicInfo.Cell(11, 10).Select();
            SetCellHeaderText("");
            basicInfo.Cell(11, 11).Select();
            SetCellHeaderText("");

            basicInfo.Rows[12].Height = 20;
            basicInfo.Cell(12, 1).Select();
            SetCellHeaderText("2");
            basicInfo.Cell(12, 2).Select();
            SetCellHeaderText("技术成本");
            basicInfo.Cell(12, 3).Select();
            SetCellHeaderText("样品");
            basicInfo.Cell(12, 4).Select();
            SetCellHeaderText("无机类");
            basicInfo.Cell(12, 5).Select();
            SetCellHeaderText("数量");
            basicInfo.Cell(12, 6).Select();
            SetCellHeaderText(model.Inorganic.ToString());
            basicInfo.Cell(12, 7).Select();
            SetCellHeaderText("样品单价");
            basicInfo.Cell(12, 8).Select();
            SetCellHeaderText(model.InorganicPrice.ToString());
            basicInfo.Cell(12, 9).Select();
            SetCellHeaderText((model.Inorganic * model.InorganicPrice).ToString());
            basicInfo.Cell(12, 10).Select();
            SetCellHeaderText((model.PhysicalFactors * model.PhysicalFactorsPrice) + (model.DustPrice * model.Dust) + (model.Inorganic * model.InorganicPrice) + (model.Organic * model.OrganicPrice) + (model.Free * model.FreePrice).ToString());
            basicInfo.Cell(12, 11).Select();
            SetCellHeaderText("");

            basicInfo.Rows[13].Height = 20;
            basicInfo.Cell(13, 1).Select();
            SetCellHeaderText("");
            basicInfo.Cell(13, 2).Select();
            SetCellHeaderText("");
            basicInfo.Cell(13, 3).Select();
            SetCellHeaderText("");
            basicInfo.Cell(13, 4).Select();
            SetCellHeaderText("有机类");
            basicInfo.Cell(13, 5).Select();
            SetCellHeaderText("");
            basicInfo.Cell(13, 6).Select();
            SetCellHeaderText(model.Organic.ToString());
            basicInfo.Cell(13, 7).Select();
            SetCellHeaderText("");
            basicInfo.Cell(13, 8).Select();
            SetCellHeaderText(model.OrganicPrice.ToString());
            basicInfo.Cell(13, 9).Select();
            SetCellHeaderText((model.OrganicPrice * model.Organic).ToString());
            basicInfo.Cell(13, 10).Select();
            SetCellHeaderText("");
            basicInfo.Cell(13, 11).Select();
            SetCellHeaderText("");

            basicInfo.Rows[14].Height = 20;
            basicInfo.Cell(14, 1).Select();
            SetCellHeaderText("");
            basicInfo.Cell(14, 2).Select();
            SetCellHeaderText("");
            basicInfo.Cell(14, 3).Select();
            SetCellHeaderText("");
            basicInfo.Cell(14, 4).Select();
            SetCellHeaderText("游离SiO2");
            basicInfo.Cell(14, 5).Select();
            SetCellHeaderText("");
            basicInfo.Cell(14, 6).Select();
            SetCellHeaderText(model.Free.ToString());
            basicInfo.Cell(14, 7).Select();
            SetCellHeaderText("");
            basicInfo.Cell(14, 8).Select();
            SetCellHeaderText(model.FreePrice.ToString());
            basicInfo.Cell(14, 9).Select();
            SetCellHeaderText((model.FreePrice * model.Free).ToString());
            basicInfo.Cell(14, 10).Select();
            SetCellHeaderText("");
            basicInfo.Cell(14, 11).Select();
            SetCellHeaderText("");

            basicInfo.Rows[15].Height = 20;
            basicInfo.Cell(15, 1).Select();
            SetCellHeaderText("");
            basicInfo.Cell(15, 2).Select();
            SetCellHeaderText("");
            basicInfo.Cell(15, 3).Select();
            SetCellHeaderText("报告");
            basicInfo.Cell(15, 4).Select();
            SetCellHeaderText("样品数量");
            basicInfo.Cell(15, 5).Select();
            SetCellHeaderText(model.NumBerSum.ToString());
            basicInfo.Cell(15, 6).Select();
            SetCellHeaderText("所需工时");
            basicInfo.Cell(15, 7).Select();
            SetCellHeaderText(model.WorkingHours.ToString());
            basicInfo.Cell(15, 8).Select();
            SetCellHeaderText("工时单价");
            basicInfo.Cell(15, 9).Select();
            SetCellHeaderText(model.WorkingHoursPrice.ToString());
            basicInfo.Cell(15, 10).Select();
            SetCellHeaderText((model.WorkingHours * model.WorkingHoursPrice).ToString());
            basicInfo.Cell(15, 11).Select();
            SetCellHeaderText("");

            basicInfo.Rows[16].Height = 20;
            basicInfo.Cell(16, 1).Select();
            SetCellHeaderText("3");
            basicInfo.Cell(16, 2).Merge(basicInfo.Cell(16, 3));
            basicInfo.Cell(16, 2).Select();
            SetCellHeaderText("推广费(1)");
            basicInfo.Cell(16, 3).Merge(basicInfo.Cell(16, 8));
            basicInfo.Cell(16, 3).Select();
            SetCellHeaderText("按实际发生");
            basicInfo.Cell(16, 4).Select();
            SetCellHeaderText(model.PromotionFee.ToString());
            basicInfo.Cell(16, 5).Select();
            SetCellHeaderText("");

            basicInfo.Rows[17].Height = 20;
            basicInfo.Cell(17, 1).Select();
            SetCellHeaderText("4");
            basicInfo.Cell(17, 2).Merge(basicInfo.Cell(17, 3));
            basicInfo.Cell(17, 2).Select();
            SetCellHeaderText("推广费(2)");
            basicInfo.Cell(17, 3).Merge(basicInfo.Cell(17, 8));
            basicInfo.Cell(17, 3).Select();
            SetCellHeaderText("按实际发生");
            basicInfo.Cell(17, 4).Select();
            SetCellHeaderText("");
            basicInfo.Cell(17, 5).Select();
            SetCellHeaderText("");

            basicInfo.Rows[18].Height = 20;
            basicInfo.Cell(18, 1).Select();
            SetCellHeaderText("5");
            basicInfo.Cell(18, 2).Merge(basicInfo.Cell(18, 3));
            basicInfo.Cell(18, 2).Select();
            SetCellHeaderText("协作费");
            basicInfo.Cell(18, 3).Merge(basicInfo.Cell(18, 8));
            basicInfo.Cell(18, 3).Select();
            SetCellHeaderText("（销售额-推广费）*15%");
            basicInfo.Cell(18, 4).Select();
            SetCellHeaderText(model.CollaborationFee.ToString());
            basicInfo.Cell(18, 5).Select();
            SetCellHeaderText("");

            basicInfo.Rows[19].Height = 20;
            basicInfo.Cell(19, 1).Select();
            SetCellHeaderText("6");
            basicInfo.Cell(19, 2).Merge(basicInfo.Cell(19, 3));
            basicInfo.Cell(19, 2).Select();
            SetCellHeaderText("提成");
            basicInfo.Cell(19, 3).Merge(basicInfo.Cell(19, 8));
            basicInfo.Cell(19, 3).Select();
            SetCellHeaderText("（销售额-推广费-协作费）*10%");
            basicInfo.Cell(19, 4).Select();
            SetCellHeaderText(model.Commission.ToString());
            basicInfo.Cell(19, 5).Select();
            SetCellHeaderText("");

            basicInfo.Rows[20].Height = 20;
            basicInfo.Cell(20, 1).Select();
            SetCellHeaderText("8");
            basicInfo.Cell(20, 2).Merge(basicInfo.Cell(20, 3));
            basicInfo.Cell(20, 2).Select();
            SetCellHeaderText("其他费用");
            basicInfo.Cell(20, 3).Merge(basicInfo.Cell(20, 8));
            basicInfo.Cell(20, 3).Select();
            SetCellHeaderText("实际发生费用");
            basicInfo.Cell(20, 4).Select();
            SetCellHeaderText(model.OtherFees.ToString());
            basicInfo.Cell(20, 5).Select();
            SetCellHeaderText("");

            basicInfo.Rows[21].Height = 20;
            basicInfo.Cell(21, 1).Select();
            SetCellHeaderText("9");
            basicInfo.Cell(21, 2).Merge(basicInfo.Cell(21, 3));
            basicInfo.Cell(21, 2).Select();
            SetCellHeaderText("税金");
            basicInfo.Cell(21, 3).Merge(basicInfo.Cell(21, 8));
            basicInfo.Cell(21, 3).Select();
            SetCellHeaderText("销售额/1.06*0.06");
            basicInfo.Cell(21, 4).Select();
            SetCellHeaderText(model.Tax.ToString());
            basicInfo.Cell(21, 5).Select();
            SetCellHeaderText("");

            basicInfo.Rows[22].Height = 20;
            basicInfo.Cell(22, 1).Select();
            SetCellHeaderText("10");
            basicInfo.Cell(22, 2).Merge(basicInfo.Cell(22, 3));
            basicInfo.Cell(22, 2).Select();
            SetCellHeaderText("毛利润");
            basicInfo.Cell(22, 3).Merge(basicInfo.Cell(22, 8));
            basicInfo.Cell(22, 3).Select();
            SetCellHeaderText("10=1-2-3-4-5-6-7-8-9");
            basicInfo.Cell(22, 4).Select();
            SetCellHeaderText(model.GrossProfit.ToString());
            basicInfo.Cell(22, 5).Select();
            SetCellHeaderText("");

            basicInfo.Rows[23].Height = 20;
            basicInfo.Cell(23, 1).Select();
            SetCellHeaderText("11");
            basicInfo.Cell(23, 2).Merge(basicInfo.Cell(23, 3));
            basicInfo.Cell(23, 2).Select();
            SetCellHeaderText("毛利润率");
            basicInfo.Cell(23, 3).Merge(basicInfo.Cell(23, 8));
            basicInfo.Cell(23, 3).Select();
            SetCellHeaderText("毛利润/销售额");
            basicInfo.Cell(23, 4).Select();
            SetCellHeaderText(model.GrossProfitMargin.ToString());
            basicInfo.Cell(23, 5).Select();
            SetCellHeaderText("");

            basicInfo.Rows[24].Height = 20;
            basicInfo.Cell(24, 1).Merge(basicInfo.Cell(24, 11));
            basicInfo.Cell(24, 1).Select();
            SetCellHeaderText("审批签字栏");

            basicInfo.Rows[25].Height = 20;
            basicInfo.Cell(25, 1).Merge(basicInfo.Cell(25, 2));
            basicInfo.Cell(25, 1).Select();
            SetCellHeaderText("");
            basicInfo.Cell(25, 2).Merge(basicInfo.Cell(25, 3));
            basicInfo.Cell(25, 2).Select();
            SetCellHeaderText("签约人");
            basicInfo.Cell(25, 3).Merge(basicInfo.Cell(25, 4));
            basicInfo.Cell(25, 3).Select();
            SetCellHeaderText("市场部");
            basicInfo.Cell(25, 4).Merge(basicInfo.Cell(25, 5));
            basicInfo.Cell(25, 4).Select();
            SetCellHeaderText("副总经理");
            basicInfo.Cell(25, 5).Merge(basicInfo.Cell(25, 6));
            basicInfo.Cell(25, 5).Select();
            SetCellHeaderText("总经理");
            basicInfo.Cell(25, 6).Select();
            SetCellHeaderText("其他");

            basicInfo.Rows[26].Height = 20;
            basicInfo.Cell(26, 1).Merge(basicInfo.Cell(26, 2));
            basicInfo.Cell(26, 1).Select();
            SetCellHeaderText("签字栏");
            basicInfo.Cell(26, 2).Merge(basicInfo.Cell(26, 3));
            basicInfo.Cell(26, 2).Select();
            SetCellHeaderText("");
            basicInfo.Cell(26, 3).Merge(basicInfo.Cell(26, 4));
            basicInfo.Cell(26, 3).Select();
            SetCellHeaderText("");
            basicInfo.Cell(26, 4).Merge(basicInfo.Cell(26, 5));
            basicInfo.Cell(26, 4).Select();
            SetCellHeaderText("");
            basicInfo.Cell(26, 5).Merge(basicInfo.Cell(26, 6));
            basicInfo.Cell(26, 5).Select();
            SetCellHeaderText("");
            basicInfo.Cell(26, 6).Select();
            SetCellHeaderText("");

            basicInfo.Rows[27].Height = 20;
            basicInfo.Cell(27, 1).Merge(basicInfo.Cell(27, 2));
            basicInfo.Cell(27, 1).Select();
            SetCellHeaderText("日期");
            basicInfo.Cell(27, 2).Merge(basicInfo.Cell(27, 3));
            basicInfo.Cell(27, 2).Select();
            SetCellHeaderText("");
            basicInfo.Cell(27, 3).Merge(basicInfo.Cell(27, 4));
            basicInfo.Cell(27, 3).Select();
            SetCellHeaderText("");
            basicInfo.Cell(27, 4).Merge(basicInfo.Cell(27, 5));
            basicInfo.Cell(27, 4).Select();
            SetCellHeaderText("");
            basicInfo.Cell(27, 5).Merge(basicInfo.Cell(27, 6));
            basicInfo.Cell(27, 5).Select();
            SetCellHeaderText("");
            basicInfo.Cell(27, 6).Select();
            SetCellHeaderText("");

            m_wordApp.Selection.TypeText("\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n");

            m_doc.Paragraphs.Add();
            Word.Range range = m_doc.Paragraphs.First.Range;
            //Word.InlineShape shap = range.InlineShapes.AddPicture(@"NETofficeTest\NETofficeTest\bin\Debug\fv.png");


            m_doc.Sections[1].Footers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.Text = Footer;
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
