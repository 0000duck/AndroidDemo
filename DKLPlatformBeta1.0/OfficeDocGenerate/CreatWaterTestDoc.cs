using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetOffice.WordApi.Enums;
using Word = NetOffice.WordApi;
using DKLManager.Contract.Model;
using System.IO;

namespace OfficeDocGenerate
{
    public class CreatWaterTestDoc
    {
        string Header = "2016SJ018                                 DKL/QB-33-03                    第1页共5页 ";
        string Footer = "编制人：          校检人：          审核人：               授权签字人： \r\n 年   月   日       年  月  日        年  月  日             年  月  日  ";
        Word.Application m_wordApp;
        Word.Document m_doc;
        Word.Tools.CommonUtils m_utils;

        public CreatWaterTestDoc(ProjectInfo projectinfo)
        {
            // start word and turn off msg boxes
            this.Header = projectinfo.ProjectNumber + "                   DKL/QB-33-03                     "; ;
        }

        public List<string> CreateReportWord(List<string> strc, ProjectInfo projectmodels, List<TestChemicalReport> chemicalmodels)
        {
            string appStatus;
            string strRet = null;
            List<string> appList = new List<string>();  //函数执行状态+文件名
            //测试使用的是fristpage

            using (m_wordApp = new Word.Application())
            {
                #region  定义类
                WriteFirstPage FirstPage = new OfficeDocGenerate.WriteFirstPage("2016SJ018", "北京京门世纪物业管理有限公司");
                #endregion
                m_wordApp.DisplayAlerts = WdAlertLevel.wdAlertsNone;
                m_utils = new Word.Tools.CommonUtils(m_wordApp);
                m_doc = m_wordApp.Documents.Add();

                //SetPage();
                WriteFirstPage(FirstPage.ReportNumber, FirstPage.Client, projectmodels.CreateTime.Year.ToString() + "年" + projectmodels.CreateTime.Month.ToString() + "月" + projectmodels.CreateTime.Day.ToString() + "日");
                m_doc.Paragraphs.Last.Range.InsertBreak();
                //         m_doc.Paragraphs.Last.Range.InsertBreak();
                WriteContent();
                m_doc.Paragraphs.Last.Range.InsertBreak();
                appStatus = WriteEmployerBasicInfo(projectmodels);
                WriteTestReport(chemicalmodels, strc);
                strRet = SaveFile();
                appList.Add(appStatus);
                appList.Add(strRet);
            }
            return appList;
        }
        private string SaveFile()
        {
            string fileName = "";
            string name = "检测报告" + DateTime.Now.ToFileTime();
            //      string documentFile = m_utils.File.Combine("c:\\", name, Word.Tools.DocumentFormat.Normal);
            string path = "d://DKLdownload";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string documentFile = m_utils.File.Combine(path, "水质检测报告1.0", Word.Tools.DocumentFormat.Normal);
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
        private void WriteFirstPage(string reportNum, string company, string reportDate)              ///编号，公司名，日期
        {
            m_wordApp.Selection.Font.Color = WdColor.wdColorBlack;    ///颜色
            m_wordApp.Selection.Font.Bold = 2;     ///字体
            m_wordApp.Selection.Font.Size = 24;         ///字大小
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("\r\n北京德康莱健康安全科技股份有限公司");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Color = WdColor.wdColorBlack;
            m_wordApp.Selection.Font.Bold = 5;
            m_wordApp.Selection.Font.Size = 48;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("\r\n\r\n水质检测报告");

            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 16;
            for (int i = 0; i < 6; ++i)
                m_wordApp.Selection.TypeText("\r\n");

            m_wordApp.Selection.TypeText("\r\n报告编号：	" + reportNum);
            m_wordApp.Selection.TypeText("\r\n委托单位：	" + company);
            m_wordApp.Selection.TypeText("\r\n受检单位：	" + company);
            m_wordApp.Selection.TypeText("\r\n\r\n" + reportDate);
            m_wordApp.Selection.TypeText("\r\n");

            m_doc.Paragraphs.Add();
            Word.Range range = m_doc.Paragraphs.First.Range;
            m_doc.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.Text = Header;
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
        private void WriteContent()
        {
            m_wordApp.Selection.Font.Bold = 3;
            m_wordApp.Selection.Font.Size = 18;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("\r\n\r\n说明\r\n\r\n");
            ////List
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("1. 	检测报告包括：封面、说明、首页、正文，封面需盖有本单位CMA章、检验专用章和骑缝章;");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("2. 	报告中有涂改、增删或复印无效；");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("3. 	本报告无CMA资质认证标识及检验检测专用章无效；");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("4. 	若是送检样品，本报告仅对送检样品负责；");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("5. 	对报告有异议者，请于收到报告之日起十五日内向本公司提出，逾期不予受理；");
            m_wordApp.Selection.TypeParagraph();

            for (int i = 0; i < 3; ++i)
                m_wordApp.Selection.TypeText("\r\n");
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("\r\n\r\n");
            m_wordApp.Selection.ParagraphFormat.LeftIndent = 27;            ///向左缩进
            m_wordApp.Selection.TypeText("本单位名称：北京德康莱健康安全科技股份有限公司\r\n"
            + "通信地址：北京市北京经济技术开发区西环南路18号B座201-1室\r\n"
            + "联系电话：（010）51570158-5000  51570159-8001\r\n"
            + "传 真：（010）51570168\r\n"
            + "邮政编码：100176\r\n\r\n\r\n");
        }
        private String WriteEmployerBasicInfo(ProjectInfo projectmodels)
        {
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 2;
            m_wordApp.Selection.TypeText("水  质   检   测   报   告 ");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo = m_doc.Tables.Add(m_wordApp.Selection.Range, 14, 4);
            basicInfo.Borders.Enable = 1;
            basicInfo.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Columns[1].Width = 100;

            basicInfo.Rows[1].Height = 10;                    ///单位名称
            basicInfo.Cell(1, 1).Select();
            SetCellHeaderText("委托单位：");
            basicInfo.Cell(1, 2).Merge(basicInfo.Cell(1, 4));
            basicInfo.Cell(1, 2).Select();
            SetCellText(0, 12, projectmodels.CompaneName, WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);

            basicInfo.Rows[2].Height = 10;                    ///单位名称
            basicInfo.Cell(2, 1).Select();
            SetCellHeaderText("受检单位：");
            basicInfo.Cell(2, 2).Merge(basicInfo.Cell(2, 4));
            basicInfo.Cell(2, 2).Select();
            SetCellText(0, 12, projectmodels.CompaneName, WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);

            basicInfo.Rows[3].Height = 10;                 ///项目名称
            basicInfo.Cell(3, 1).Select();
            SetCellHeaderText("受检单位地址：");
            basicInfo.Cell(3, 2).Merge(basicInfo.Cell(3, 4));
            basicInfo.Cell(3, 2).Select();
            SetCellText(0, 12, projectmodels.CompanyAddress, WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);

            basicInfo.Rows[4].Height = 10;                 ///项目名称
            basicInfo.Cell(4, 1).Select();
            SetCellHeaderText("检测类别：");
            basicInfo.Cell(4, 2).Select();
            SetCellHeaderText("   ");
            basicInfo.Cell(4, 3).Select();
            SetCellHeaderText("受理日期：");
            basicInfo.Cell(4, 4).Select();

            basicInfo.Rows[5].Height = 10;                 ///项目名称
            basicInfo.Cell(5, 1).Select();
            SetCellHeaderText("样品来源：");
            basicInfo.Cell(5, 2).Select();
            SetCellHeaderText("采样");
            basicInfo.Cell(5, 3).Select();
            SetCellHeaderText("采样日期：");
            basicInfo.Cell(5, 4).Select();

            basicInfo.Rows[6].Height = 10;                 ///项目名称
            basicInfo.Cell(6, 1).Select();
            SetCellHeaderText("样品数量：");
            basicInfo.Cell(6, 2).Select();
            SetCellHeaderText("   ");
            basicInfo.Cell(6, 3).Select();
            SetCellHeaderText("检测日期：");
            basicInfo.Cell(6, 4).Select();

            basicInfo.Cell(7, 1).Merge(basicInfo.Cell(8, 1));
            basicInfo.Cell(7, 1).Select();
            SetCellHeaderText("主要仪器设备：");
            basicInfo.Cell(7, 2).Merge(basicInfo.Cell(8, 4));
            basicInfo.Cell(7, 2).Select();
            SetCellHeaderText("        ");

            basicInfo.Cell(8, 1).Merge(basicInfo.Cell(14, 1));
            basicInfo.Cell(8, 1).Select();
            SetCellHeaderText("检测依据：");
            basicInfo.Cell(8, 2).Merge(basicInfo.Cell(14, 4));
            basicInfo.Cell(8, 2).Select();
            SetCellHeaderText("        ");

            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeText("\r\n");
            return "0";
           
        }

        private void SetCellText(int bold, int size, string text, WdParagraphAlignment wAlign, WdCellVerticalAlignment vAlign)
        {
            m_wordApp.Selection.Cells.VerticalAlignment = vAlign;
            SetParaText(bold, size, text, wAlign);
        }

        private void SetCellHeaderText(string text)
        {
            SetCellText(1, 12, text, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
        }
        private void SetParaText(int bold, int size, string text, WdParagraphAlignment wAlign)
        {
            m_wordApp.Selection.Font.Bold = bold;
            m_wordApp.Selection.Font.Color = WdColor.wdColorBlack;
            m_wordApp.Selection.Font.Size = size;
            m_wordApp.Selection.ParagraphFormat.Alignment = wAlign;
            m_wordApp.Selection.TypeText(text);
        }
        private void WriteTestReport( List<TestChemicalReport> chemicalmodels, List<string> strc)
        {
            int i = strc.Count;
            for (int j = 0; j < i; j++)
            {
                m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
                m_wordApp.Selection.InsertBreak();
                m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                m_wordApp.Selection.Font.Size = 14;
                m_wordApp.Selection.Font.Bold = 3;
                m_wordApp.Selection.TypeText("检测结果\r\n");

                Word.Table basicInfo = m_doc.Tables.Add(m_wordApp.Selection.Range, 25, 5);
                basicInfo.Borders.Enable = 1;
                basicInfo.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
                basicInfo.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
                basicInfo.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
                basicInfo.Columns[1].Width = 90;

                basicInfo.Rows[1].Height = 10;                    
                basicInfo.Cell(1, 1).Select();
                SetCellHeaderText("样品编号");
                basicInfo.Cell(1, 2).Select();
                SetCellHeaderText("样品名称");
                basicInfo.Cell(1, 3).Select();
                SetCellHeaderText("样品状态");
                basicInfo.Cell(1, 4).Select();
                SetCellHeaderText("采样地点");
                basicInfo.Cell(1, 5).Select();
                SetCellHeaderText("样品包装");

                var chemicalmodel = chemicalmodels.FirstOrDefault();       ///得到数列中的一个 元素   
                basicInfo.Rows[2].Height = 10;                    
                basicInfo.Cell(2, 1).Select();
                SetCellText(0, 12, chemicalmodel.SampleNumber, WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                basicInfo.Cell(2, 2).Select();
                SetCellHeaderText("   ");
                basicInfo.Cell(2, 3).Select();
                SetCellHeaderText("   ");
                basicInfo.Cell(2, 4).Select();
                SetCellText(0, 12, chemicalmodel.Location, WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                basicInfo.Cell(2, 5).Select();
                SetCellHeaderText("   ");
                chemicalmodels.Remove(chemicalmodel);

                basicInfo.Rows[3].Height = 10;
                basicInfo.Cell(3, 1).Merge(basicInfo.Cell(3, 2));
                basicInfo.Cell(3, 1).Select();
                SetCellHeaderText("检测项目");
                basicInfo.Cell(3, 2).Select();
                SetCellHeaderText("检测结果");
                basicInfo.Cell(3, 3).Merge(basicInfo.Cell(3, 4));
                basicInfo.Cell(3, 3).Select();
                SetCellHeaderText("限值");

                basicInfo.Rows[4].Height = 10;
                basicInfo.Cell(4, 1).Merge(basicInfo.Cell(4, 2));
                basicInfo.Cell(4, 1).Select();
                SetCellHeaderText("色度（铂钴色度单位）");
                basicInfo.Cell(4, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(4, 3).Merge(basicInfo.Cell(4, 4));
                basicInfo.Cell(4, 3).Select();
                SetCellHeaderText("15");

                basicInfo.Rows[5].Height = 10;
                basicInfo.Cell(5, 1).Merge(basicInfo.Cell(5, 2));
                basicInfo.Cell(5, 1).Select();
                SetCellHeaderText("浑浊度（NTU）");
                basicInfo.Cell(5, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(5, 3).Merge(basicInfo.Cell(5, 4));
                basicInfo.Cell(5, 3).Select();
                SetCellHeaderText("1\r\n 水源与净水技术条件限制时为3");

                basicInfo.Rows[6].Height = 10;
                basicInfo.Cell(6, 1).Merge(basicInfo.Cell(6, 2));
                basicInfo.Cell(6, 1).Select();
                SetCellHeaderText("臭和味");
                basicInfo.Cell(6, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(6, 3).Merge(basicInfo.Cell(6, 4));
                basicInfo.Cell(6, 3).Select();
                SetCellHeaderText("无异臭、异味");

                basicInfo.Rows[7].Height = 10;
                basicInfo.Cell(7, 1).Merge(basicInfo.Cell(7, 2));
                basicInfo.Cell(7, 1).Select();
                SetCellHeaderText("肉眼可见物");
                basicInfo.Cell(7, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(7, 3).Merge(basicInfo.Cell(7, 4));
                basicInfo.Cell(7, 3).Select();
                SetCellHeaderText("无");

                basicInfo.Rows[8].Height = 10;
                basicInfo.Cell(8, 1).Merge(basicInfo.Cell(8, 2));
                basicInfo.Cell(8, 1).Select();
                SetCellHeaderText("pH");
                basicInfo.Cell(8, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(8, 3).Merge(basicInfo.Cell(8, 4));
                basicInfo.Cell(8, 3).Select();
                SetCellHeaderText("不小于6.5且不大于8.5");

                basicInfo.Rows[9].Height = 10;
                basicInfo.Cell(9, 1).Merge(basicInfo.Cell(9, 2));
                basicInfo.Cell(9, 1).Select();
                SetCellHeaderText("总硬度（以CaCO3计）/ (mg/L)");
                basicInfo.Cell(9, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(9, 3).Merge(basicInfo.Cell(9, 4));
                basicInfo.Cell(9, 3).Select();
                SetCellHeaderText("450");

                basicInfo.Rows[10].Height = 10;
                basicInfo.Cell(10, 1).Merge(basicInfo.Cell(10, 2));
                basicInfo.Cell(10, 1).Select();
                SetCellHeaderText("亚硝酸盐(mg/L)");
                basicInfo.Cell(10, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(10, 3).Merge(basicInfo.Cell(10, 4));
                basicInfo.Cell(10, 3).Select();
                SetCellHeaderText("1");

                basicInfo.Rows[11].Height = 10;
                basicInfo.Cell(11, 1).Merge(basicInfo.Cell(11, 2));
                basicInfo.Cell(11, 1).Select();
                SetCellHeaderText("耗氧量（CODMn法，以O2计）/ (mg/L)");
                basicInfo.Cell(11, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(11, 3).Merge(basicInfo.Cell(11, 4));
                basicInfo.Cell(11, 3).Select();
                SetCellHeaderText("3\r\n 水源限制，原水耗氧量＞6 mg/L时为5");

                basicInfo.Rows[12].Height = 10;
                basicInfo.Cell(12, 1).Merge(basicInfo.Cell(12, 2));
                basicInfo.Cell(12, 1).Select();
                SetCellHeaderText("铬（六价）(mg/L)");
                basicInfo.Cell(12, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(12, 3).Merge(basicInfo.Cell(12, 4));
                basicInfo.Cell(12, 3).Select();
                SetCellHeaderText("0.05");

                basicInfo.Rows[13].Height = 10;
                basicInfo.Cell(13, 1).Merge(basicInfo.Cell(13, 2));
                basicInfo.Cell(13, 1).Select();
                SetCellHeaderText("氨氮（以N计）/ (mg/L)");
                basicInfo.Cell(13, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(13, 3).Merge(basicInfo.Cell(13, 4));
                basicInfo.Cell(13, 3).Select();
                SetCellHeaderText("0.5");

                basicInfo.Rows[14].Height = 10;
                basicInfo.Cell(14, 1).Merge(basicInfo.Cell(14, 2));
                basicInfo.Cell(14, 1).Select();
                SetCellHeaderText("总大肠菌群（MPN/100mL）");
                basicInfo.Cell(14, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(14, 3).Merge(basicInfo.Cell(14, 4));
                basicInfo.Cell(14, 3).Select();
                SetCellHeaderText("不得检出");

                basicInfo.Rows[15].Height = 10;
                basicInfo.Cell(15, 1).Merge(basicInfo.Cell(15, 2));
                basicInfo.Cell(15, 1).Select();
                SetCellHeaderText("耐热大肠菌群（MPN/100mL）");
                basicInfo.Cell(15, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(15, 3).Merge(basicInfo.Cell(15, 4));
                basicInfo.Cell(15, 3).Select();
                SetCellHeaderText("不得检出");

                basicInfo.Rows[16].Height = 10;
                basicInfo.Cell(16, 1).Merge(basicInfo.Cell(16, 2));
                basicInfo.Cell(16, 1).Select();
                SetCellHeaderText("大肠埃希氏菌（MPN/100mL）");
                basicInfo.Cell(16, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(16, 3).Merge(basicInfo.Cell(16, 4));
                basicInfo.Cell(16, 3).Select();
                SetCellHeaderText("不得检出");

                basicInfo.Rows[17].Height = 10;
                basicInfo.Cell(17, 1).Merge(basicInfo.Cell(17, 2));
                basicInfo.Cell(17, 1).Select();
                SetCellHeaderText("菌落总数(cfu/mL)");
                basicInfo.Cell(17, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(17, 3).Merge(basicInfo.Cell(17, 4));
                basicInfo.Cell(17, 3).Select();
                SetCellHeaderText("无异臭、异味");

                basicInfo.Rows[18].Height = 10;
                basicInfo.Cell(18, 1).Merge(basicInfo.Cell(18, 2));
                basicInfo.Cell(18, 1).Select();
                SetCellHeaderText("铁(mg/L)");
                basicInfo.Cell(18, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(18, 3).Merge(basicInfo.Cell(18, 4));
                basicInfo.Cell(18, 3).Select();
                SetCellHeaderText("0.3");

                basicInfo.Rows[19].Height = 10;
                basicInfo.Cell(19, 1).Merge(basicInfo.Cell(19, 2));
                basicInfo.Cell(19, 1).Select();
                SetCellHeaderText("铜(mg/L)");
                basicInfo.Cell(19, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(19, 3).Merge(basicInfo.Cell(19, 4));
                basicInfo.Cell(19, 3).Select();
                SetCellHeaderText("1.0");

                basicInfo.Rows[20].Height = 10;
                basicInfo.Cell(20, 1).Merge(basicInfo.Cell(20, 2));
                basicInfo.Cell(20, 1).Select();
                SetCellHeaderText("锰(mg/L)");
                basicInfo.Cell(20, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(20, 3).Merge(basicInfo.Cell(20, 4));
                basicInfo.Cell(20, 3).Select();
                SetCellHeaderText("0.1");

                basicInfo.Rows[21].Height = 10;
                basicInfo.Cell(21, 1).Merge(basicInfo.Cell(21, 2));
                basicInfo.Cell(21, 1).Select();
                SetCellHeaderText("铅(mg/L)");
                basicInfo.Cell(21, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(21, 3).Merge(basicInfo.Cell(21, 4));
                basicInfo.Cell(21, 3).Select();
                SetCellHeaderText("0.01");

                basicInfo.Rows[22].Height = 10;
                basicInfo.Cell(22, 1).Merge(basicInfo.Cell(22, 2));
                basicInfo.Cell(22, 1).Select();
                SetCellHeaderText("氯化物(mg/L)");
                basicInfo.Cell(22, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(22, 3).Merge(basicInfo.Cell(22, 4));
                basicInfo.Cell(22, 3).Select();
                SetCellHeaderText("250");

                basicInfo.Rows[23].Height = 10;
                basicInfo.Cell(23, 1).Merge(basicInfo.Cell(23, 2));
                basicInfo.Cell(23, 1).Select();
                SetCellHeaderText("硫酸盐(mg/L)");
                basicInfo.Cell(23, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(23, 3).Merge(basicInfo.Cell(23, 4));
                basicInfo.Cell(23, 3).Select();
                SetCellHeaderText("250");

                basicInfo.Rows[24].Height = 10;
                basicInfo.Cell(24, 1).Merge(basicInfo.Cell(24, 2));
                basicInfo.Cell(24, 1).Select();
                SetCellHeaderText("游离余氯(mg/L)");
                basicInfo.Cell(24, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(24, 3).Merge(basicInfo.Cell(24, 4));
                basicInfo.Cell(24, 3).Select();
                SetCellHeaderText("≥0.05\r\n （管网末梢水中余量）");

                basicInfo.Rows[25].Height = 10;
                basicInfo.Cell(25, 1).Merge(basicInfo.Cell(25, 2));
                basicInfo.Cell(25, 1).Select();
                SetCellHeaderText("硝酸盐(（以N计）/mg/L)");
                basicInfo.Cell(25, 2).Select();
                SetCellHeaderText("    ");
                basicInfo.Cell(25, 3).Merge(basicInfo.Cell(25, 4));
                basicInfo.Cell(25, 3).Select();
                SetCellHeaderText("10\r\n 地下水源限制时为20");
            }
        }
    }
}
