using System;
using System.Collections.Generic;
using Word = NetOffice.WordApi;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetOffice.WordApi.Enums;
using System.IO;
using DKLManager.Contract.Model;

namespace OfficeDocGenerate
{
   public class CreatTestDoc
    {
        string Header = "2016JF281                                 DKL/QB-30-03                    第1页共6页 ";
        string Footer = "编制人：          校检人：          审核人：               授权签字人： \r\n 年   月   日       年  月  日        年  月  日             年  月  日  ";
        Word.Application m_wordApp;
        Word.Document m_doc;
        Word.Tools.CommonUtils m_utils;

         public CreatTestDoc(ProjectInfo projectinfo)
        {   
            // start word and turn off msg boxes
            this.Header = projectinfo.ProjectNumber+"                   DKL/QB-28-02                     "; ;
        }
        public List<string> CreateReportWord(List<string> str, List<string> strc, List<SampleProjectGist> ProjectGistmodels, List<Parameter> Parametermodels, List<Parameter> ParmeterChemicalModels, ProjectInfo projectmodels, List<TestPhysicalReport> physicalmodels, List<TestChemicalReport> chemicalmodels)
        {
            string appStatus;
            string strRet = null;
            List<string> appList = new List<string>();  //函数执行状态+文件名
            //测试使用的是fristpage

            using (m_wordApp = new Word.Application())
            {
                #region  定义类
                WriteFirstPage FirstPage = new OfficeDocGenerate.WriteFirstPage("2016JF281", "北京宝贵石艺科技有限公司");
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
                appStatus = WriteEmployerBasicInfo(projectmodels, chemicalmodels, str, strc, ProjectGistmodels, Parametermodels);
                WriteTestReport(ParmeterChemicalModels, physicalmodels, chemicalmodels, str);
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

            string documentFile = m_utils.File.Combine(path, "检测模板样例1.0", Word.Tools.DocumentFormat.Normal);
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
            m_wordApp.Selection.TypeText("\r\n\r\n检测报告");

            //测试结束            
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 16;
            for (int i = 0; i < 7; ++i)
                m_wordApp.Selection.TypeText("\r\n");

            m_wordApp.Selection.TypeText("\r\n报告编号：	" + reportNum);
            m_wordApp.Selection.TypeText("\r\n委托单位：	" + company);
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
            m_wordApp.Selection.TypeText("\r\n\r\n\r\n(说明)\r\n\r\n\r\n");
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
        private String WriteEmployerBasicInfo(ProjectInfo projectmodels,List<TestChemicalReport> chemicalmodels,List<string> str, List<string> strc, List<SampleProjectGist> ProjectGistmodels, List<Parameter> Parametermodels)
        {            
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 2;
            m_wordApp.Selection.TypeText("基本信息：");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo = m_doc.Tables.Add(m_wordApp.Selection.Range, 14 + str.Count - 4 + str.Count + strc.Count, 6);  //-
            basicInfo.Borders.Enable = 1;
            basicInfo.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Columns[1].Width = 90;

            basicInfo.Rows[1].Height = 10;                    ///单位名称
            basicInfo.Cell(1, 1).Select();
            SetCellHeaderText("单位名称");
            basicInfo.Cell(1, 2).Merge(basicInfo.Cell(1, 6));
            basicInfo.Cell(1, 2).Select();
            SetCellText(1, 12, projectmodels.CompaneName, WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            ////SetParaText(bold, size, text, wAlign);   粗细  大小  文本  

            basicInfo.Rows[2].Height = 10;                  ///单位地址
            basicInfo.Cell(2, 1).Select(); 
            SetCellHeaderText("单位地址");
            basicInfo.Cell(2, 2).Merge(basicInfo.Cell(2, 6));
            basicInfo.Cell(2, 2).Select();
            SetCellText(0, 12, projectmodels.CompanyAddress, WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);


            basicInfo.Rows[3].Height = 20;                 ///项目名称
            basicInfo.Cell(3, 1).Select();
            SetCellHeaderText("项目名称");
            basicInfo.Cell(3, 2).Merge(basicInfo.Cell(3, 6));
            basicInfo.Cell(3, 2).Select();
            SetCellText(0, 12, projectmodels.ProjectName, WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);

            basicInfo.Rows[4].Height = 10;                   ///联系人
            basicInfo.Cell(4, 1).Select();
            SetCellHeaderText("联系人");
            basicInfo.Cell(4, 2).Select();
            SetCellText(0, 12, projectmodels.CompanyContact, WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            basicInfo.Cell(4, 3).Select();
            SetCellHeaderText("联系电话");
            basicInfo.Cell(4, 4).Select();
            SetCellText(1, 12, projectmodels.ContactTel, WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            basicInfo.Cell(4, 5).Select();
            SetCellHeaderText("邮 编");
            basicInfo.Cell(4, 6).Select();
            SetCellText(0, 12, projectmodels.ZipCode, WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);

            basicInfo.Rows[5].Height = 30;                    ///检测参数    传入采样项目名称
            basicInfo.Cell(5, 1).Select();
            SetCellHeaderText("检测参数");
            basicInfo.Cell(5, 2).Merge(basicInfo.Cell(5, 4));
            basicInfo.Cell(5, 2).Select();
            //SetCellText(1, 12, chemicalmodels., WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            basicInfo.Cell(5, 3).Select();
            SetCellHeaderText("检测类别");
            basicInfo.Cell(5, 4).Select();
            SetCellHeaderText("   ");                  ////需要新添一条字段，字段内容为检测类别

            basicInfo.Cell(6, 1).Merge(basicInfo.Cell(8, 1));              ///需要加一大堆采样设备
            basicInfo.Cell(6, 1).Select();
            SetCellHeaderText("样品名称\r\n数量\r\n状态");
            basicInfo.Cell(6, 2).Merge(basicInfo.Cell(8, 4));
            basicInfo.Cell(6, 2).Select();
            SetCellHeaderText(" ");           ///？？？？？
            basicInfo.Cell(6, 3).Select();
            SetCellHeaderText("采样日期");
            basicInfo.Cell(7, 3).Select();
            SetCellHeaderText("接收日期");
            basicInfo.Cell(8, 3).Select();
            SetCellHeaderText("检测日期");
            if (str.Count == 0)             //采样项目为空 返回错误状态 3
                return "3";                ///采样项目为空  错误

            basicInfo.Cell(9, 1).Merge(basicInfo.Cell(9 + str.Count, 1));           ////str表示的是采样个数
            basicInfo.Cell(9, 1).Select();
            SetCellHeaderText("采样");                ///采样
            basicInfo.Cell(9, 2).Select();
            SetCellHeaderText("采样项目");
            basicInfo.Cell(9, 3).Merge(basicInfo.Cell(9, 4));
            basicInfo.Cell(9, 3).Select();
            SetCellHeaderText("采样依据");
            basicInfo.Cell(9, 4).Select();
            SetCellHeaderText("仪器名称");
            basicInfo.Cell(9, 5).Select();
            SetCellHeaderText("仪器编号");
            int i = 10;     

            foreach (var item in ProjectGistmodels)        ///从第十排开始显示采样项目
            {
                if (item != null)
                {
                    basicInfo.Cell(i, 2).Select();
                    SetCellText(0, 12, item.SampleProject, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    basicInfo.Cell(i, 3).Select();
                    SetCellText(0, 12, item.SampleGist, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    basicInfo.Cell(i, 4).Select();
                    SetCellText(0, 12, item.ApparatusName, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    basicInfo.Cell(i, 5).Select();
                    SetCellText(0, 12, item.ApparatusNumber, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    
                }
                else
                    return "2";           ///采样项目为空，返回错误2
                i++;
            }

            basicInfo.Cell(10 + str.Count, 1).Merge(basicInfo.Cell(10 + str.Count + strc.Count + str.Count, 1));  ///9+str+1  到 9+str+1+strc
            basicInfo.Cell(10 + str.Count, 1).Select();
            SetCellHeaderText("检测");
            basicInfo.Cell(10 + str.Count, 2).Height = 10;
            basicInfo.Cell(10 + str.Count, 2).Select();
            SetCellHeaderText("检测项目");
            basicInfo.Cell(10 + str.Count, 3).Select();
            SetCellHeaderText("检测依据");
            basicInfo.Cell(10 + str.Count, 4).Select();
            SetCellHeaderText("仪器名称");
            basicInfo.Cell(10 + str.Count, 5).Select();
            SetCellHeaderText("仪器编号");
            int j = 11 + str.Count;          ///原本存在的11行

            foreach (var item in Parametermodels)
            {
                if (item != null)
                {
                    basicInfo.Cell(j, 2).Select();
                    SetCellText(0, 12, item.ParameterName, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);

                    basicInfo.Cell(j, 3).Select();
                    SetCellText(0, 12, item.DetectionPursuant, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);

                    basicInfo.Cell(j, 4).Select();
                    SetCellText(0, 12, item.ApparatusName, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    basicInfo.Cell(j, 5).Select();
                    SetCellText(0, 12, item.ApparatusNumber, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                }
                else
                    return "1";          ///检测项目为空，则返回错误1
                j++;
            }
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeText("\r\n");
            return "0";
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
       private void WriteTestReport(List<Parameter> ParmeterChemicalModels, List<TestPhysicalReport> physicalmodels, List<TestChemicalReport> chemicalmodels, List<string> str)
       {
           m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
           m_wordApp.Selection.InsertBreak();
           m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
           m_wordApp.Selection.Font.Size = 14;
           m_wordApp.Selection.Font.Bold = 3;
           m_wordApp.Selection.TypeText("检测结果：\r\n");
           //WriteTestTable1(physicalmodels);
           //m_wordApp.Selection.MoveDown();//光标下移，光标移出表格 
           //m_wordApp.Selection.TypeText("\r\n\r\n\r\n\r\n");

           //WriteTestTable2(chemicalmodels);
           //m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
           //m_wordApp.Selection.TypeText("\r\n\r\n\r\n\r\n");

           //WriteTestTable3(physicalmodels);
           //m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
           //m_wordApp.Selection.TypeText("\r\n\r\n\r\n\r\n");

           //WriteTestTable4(chemicalmodels);
           //m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
           //m_wordApp.Selection.TypeText("\r\n\r\n\r\n\r\n");

           //WriteTestTable5(chemicalmodels);
           //m_wordApp.Selection.MoveDown();//光标下移，光标移出表格 
           //m_wordApp.Selection.TypeText("\r\n\r\n\r\n\r\n");

           //WriteTestTable6(chemicalmodels);
           //m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
           //m_wordApp.Selection.TypeText("\r\n\r\n\r\n\r\n");

           for (int i = 0; i < 9; i++)
               m_wordApp.Selection.TypeText("\r\n");
           m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
           m_wordApp.Selection.Font.Bold = 2;
           m_wordApp.Selection.Font.Size = 14;
           m_wordApp.Selection.TypeText("北京德康莱健康安全科技股份有限公司\r\n");
           m_wordApp.Selection.TypeText("（盖章）      \r\n");

       }
       private void WriteTestTable1(List<TestPhysicalReport> physicalModels)
       {
           SetParaText(1, 12, "表1  工作场所稳态噪声强度测量结果             ", WdParagraphAlignment.wdAlignParagraphCenter);
           SetParaText(1, 12, "单位：dB(A)", WdParagraphAlignment.wdAlignParagraphRight);
           Word.Table table1 = m_doc.Tables.Add(m_wordApp.Selection.Range, physicalModels.Count + 1, 7);   ///physicalmodel有一条加一条
           table1.Borders.Enable = 1;
           table1.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
           table1.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
           table1.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
           table1.Rows.Height = 20;
           table1.Rows[1].Height = 30;
           table1.Columns[2].Width = 40;
           table1.Columns[3].Width = 100;
           table1.Cell(1, 1).Select();
           SetCellHeaderText("车间");
           table1.Cell(1, 2).Select();
           SetCellHeaderText("岗位");
           table1.Cell(1, 3).Select();
           SetCellHeaderText("测量地点");
           table1.Cell(1, 4).Select();
           SetCellHeaderText("样品编号");
           table1.Cell(1, 5).Select();
           SetCellHeaderText("接触时间h/d");
           table1.Cell(1, 6).Select();
           SetCellHeaderText("噪声强度");
           table1.Cell(1, 7).Select();
           if (physicalModels.Count == 0 || physicalModels[0].LexCategory == 0)        ////是否存在，或者LEX类别是否为0；有一条加一条
               SetCellHeaderText("Lex8H");
           else
               SetCellHeaderText("LexW");
           int i = 2;
           foreach (var item in physicalModels)
           {
               m_wordApp.Selection.Font.Bold = 0;
               m_wordApp.Selection.Font.Size = 14;
               table1.Cell(i, 1).Select();
               SetCellText(0, 12, item.WordShop, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table1.Cell(i, 2).Select();
               SetCellText(0, 12, item.Job, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table1.Cell(i, 3).Select();
               SetCellText(0, 12, item.Location, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table1.Cell(i, 4).Select();
               SetCellText(0, 12, item.SampleNumber, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table1.Cell(i, 5).Select();
               SetCellText(0, 12, item.ContactTime, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table1.Cell(i, 6).Select();
               SetCellText(0, 12, item.NoiseIntensity, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table1.Cell(i, 7).Select();
               SetCellText(0, 12, item.Lex8hLexw, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               i++;
           }
       }
       private void WriteTestTable2(List<TestChemicalReport> chemicalModels)
       {
           SetParaText(1, 12, "表2  工作场所空气中二氧化氮、臭氧、一氧化碳浓度检测结果     ", WdParagraphAlignment.wdAlignParagraphCenter);
           SetParaText(1, 12, "单位：mg/m3", WdParagraphAlignment.wdAlignParagraphRight);
           Word.Table table2 = m_doc.Tables.Add(m_wordApp.Selection.Range, chemicalModels.Count + 1, 7);   ///physicalmodel有一条加一条
           table2.Borders.Enable = 1;
           table2.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
           table2.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
           table2.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
           table2.Rows.Height = 20;
           table2.Rows[1].Height = 30;
           table2.Columns[2].Width = 40;
           table2.Columns[3].Width = 100;
           table2.Cell(1, 1).Select();
           SetCellHeaderText("车间");
           table2.Cell(1, 2).Select();
           SetCellHeaderText("岗位");
           table2.Cell(1, 3).Select();
           SetCellHeaderText("采样地点");
           table2.Cell(1, 4).Select();
           SetCellHeaderText("职业病危害因素");
           table2.Cell(1, 5).Select();
           SetCellHeaderText("样品编号");
           table2.Cell(1, 6).Select();
           SetCellHeaderText("检测结果");         
           int i = 2;
           foreach (var item in chemicalModels)
           {
               m_wordApp.Selection.Font.Bold = 0;
               m_wordApp.Selection.Font.Size = 14;
               table2.Cell(i, 1).Select();
               SetCellText(0, 12, item.WorkShop, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table2.Cell(i, 2).Select();
               SetCellText(0, 12, item.Job, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table2.Cell(i, 3).Select();
               SetCellText(0, 12, item.Location, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table2.Cell(i, 4).Select();
               SetCellText(0, 12, item.Factor, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table2.Cell(i, 5).Select();
               SetCellText(0, 12, item.SampleNumber, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table2.Cell(i, 6).Select();
               SetCellText(0, 12, item.CMAC, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);              
               i++;
           }
           m_wordApp.Selection.MoveDown();
           SetParaText(0, 11, "注：臭氧最低检出浓度为0.07mg/m3（以采集30L空气样品计）", WdParagraphAlignment.wdAlignParagraphLeft);
       }
       private void WriteTestTable3(List<TestPhysicalReport> physicalModels)
       {
           SetParaText(1, 12, "表3 工作场所紫外辐射（电焊弧光）测量结果             ", WdParagraphAlignment.wdAlignParagraphCenter);
           SetParaText(1, 12, "单位：µW/cm2", WdParagraphAlignment.wdAlignParagraphRight);
           Word.Table table3 = m_doc.Tables.Add(m_wordApp.Selection.Range, physicalModels.Count + 1, 7);   ///physicalmodel有一条加一条
           table3.Borders.Enable = 1;
           table3.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
           table3.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
           table3.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
           table3.Rows.Height = 20;
           table3.Rows[1].Height = 30;
           table3.Columns[2].Width = 40;
           table3.Columns[3].Width = 100;
           table3.Cell(1, 1).Select();
           SetCellHeaderText("车间");
           table3.Cell(1, 2).Select();
           SetCellHeaderText("岗位");
           table3.Cell(1, 3).Select();
           SetCellHeaderText("测量地点");
           table3.Cell(1, 4).Select();
           SetCellHeaderText("测量位置");
           table3.Cell(1, 5).Select();
           SetCellHeaderText("样品编号");
           table3.Cell(1, 6).Select();
           SetCellHeaderText("测量结果");          
           int i = 2;
           foreach (var item in physicalModels)
           {
               table3.Cell(i, 1).Select();
               SetCellText(0, 12, item.WordShop, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table3.Cell(i, 2).Select();
               SetCellText(0, 12, item.Job, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table3.Cell(i, 3).Select();
               SetCellText(0, 12, item.Location, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table3.Cell(i, 4).Select();
               SetCellText(0, 12, item.TestContent, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table3.Cell(i, 5).Select();
               SetCellText(0, 12, item.SampleNumber, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table3.Cell(i, 6).Select();
               SetCellText(0, 12, item.ResultVerdict, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               i++;
           }
           m_wordApp.Selection.MoveDown();
           SetParaText(0, 11, "注：紫外辐照计最小分辨力为0.1µW/cm2）", WdParagraphAlignment.wdAlignParagraphLeft);
       }
       private void WriteTestTable4(List<TestChemicalReport> chemicalModels)
       {
           SetParaText(1, 12, "表4 工作场所空气中聚苯板粉尘、电焊烟尘、金属粉尘、水泥粉尘（总尘、呼尘）锰及其化合物浓度检测结果                ", WdParagraphAlignment.wdAlignParagraphCenter);
           SetParaText(1, 12, "单位：mg/m3", WdParagraphAlignment.wdAlignParagraphRight);
           Word.Table table4 = m_doc.Tables.Add(m_wordApp.Selection.Range, chemicalModels.Count + 1, 7);   ///physicalmodel有一条加一条
           table4.Borders.Enable = 1;
           table4.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
           table4.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
           table4.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
           table4.Rows.Height = 20;
           table4.Rows[1].Height = 30;
           table4.Columns[2].Width = 40;
           table4.Columns[3].Width = 100;
           table4.Cell(1, 1).Select();
           SetCellHeaderText("车间");
           table4.Cell(1, 2).Select();
           SetCellHeaderText("岗位");
           table4.Cell(1, 3).Select();
           SetCellHeaderText("采样地点");
           table4.Cell(1, 4).Select();
           SetCellHeaderText("职业病危害因素");
           table4.Cell(1, 5).Select();
           SetCellHeaderText("样品编号");
           table4.Cell(1, 6).Select();
           SetCellHeaderText("检测结果");          
           int i = 2;
           foreach (var item in chemicalModels)
           {
               table4.Cell(i, 1).Select();
               SetCellText(0, 12, item.WorkShop, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table4.Cell(i, 2).Select();
               SetCellText(0, 12, item.Job, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table4.Cell(i, 3).Select();
               SetCellText(0, 12, item.Location, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table4.Cell(i, 4).Select();
               SetCellText(0, 12, item.Factor, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table4.Cell(i, 5).Select();
               SetCellText(0, 12, item.SampleNumber, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table4.Cell(i, 6).Select();
               SetCellText(0, 12, item.CMAC, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               i++;
           }
           m_wordApp.Selection.MoveDown();
           SetParaText(0, 11, "注：*为个体采样；锰及其化合物最低检出浓度为0.006mg/m3（以采集75L空气样品计）", WdParagraphAlignment.wdAlignParagraphLeft);
       }
       private void WriteTestTable5(List<TestChemicalReport> chemicalModels)
       {
           SetParaText(1, 12, "表5 工作场所空气中聚苯板粉尘、电焊烟尘、金属粉尘、水泥粉尘（总尘、呼尘）锰及其化合物浓度检测结果                ", WdParagraphAlignment.wdAlignParagraphCenter);
           SetParaText(1, 12, "单位：mg/m3", WdParagraphAlignment.wdAlignParagraphRight);
           Word.Table table5 = m_doc.Tables.Add(m_wordApp.Selection.Range, chemicalModels.Count + 1, 7);   ///physicalmodel有一条加一条
           table5.Borders.Enable = 1;
           table5.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
           table5.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
           table5.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
           table5.Rows.Height = 20;
           table5.Rows[1].Height = 30;
           table5.Columns[2].Width = 40;
           table5.Columns[3].Width = 100;
           table5.Cell(1, 1).Select();
           SetCellHeaderText("车间");
           table5.Cell(1, 2).Select();
           SetCellHeaderText("岗位");
           table5.Cell(1, 3).Select();
           SetCellHeaderText("采样地点");
           table5.Cell(1, 4).Select();
           SetCellHeaderText("职业病危害因素");
           table5.Cell(1, 5).Select();
           SetCellHeaderText("样品编号");
           table5.Cell(1, 6).Select();
           SetCellHeaderText("检测结果");
           int i = 2;
           foreach (var item in chemicalModels)
           {
               table5.Cell(i, 1).Select();
               SetCellText(0, 12, item.WorkShop, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table5.Cell(i, 2).Select();
               SetCellText(0, 12, item.Job, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table5.Cell(i, 3).Select();
               SetCellText(0, 12, item.Location, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table5.Cell(i, 4).Select();
               SetCellText(0, 12, item.Factor, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table5.Cell(i, 5).Select();
               SetCellText(0, 12, item.SampleNumber, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table5.Cell(i, 6).Select();
               SetCellText(0, 12, item.CMAC, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               i++;
           }
           m_wordApp.Selection.MoveDown();
           SetParaText(0, 11, "注：苯乙烯最低检出浓度为1.7mg/m3（以采集1.5L空气样品计）", WdParagraphAlignment.wdAlignParagraphLeft);
       }
       private void WriteTestTable6(List<TestChemicalReport> chemicalModels)
       {
           SetParaText(1, 12, "表6 工作场所空气中聚苯板粉尘、电焊烟尘、金属粉尘、水泥粉尘（总尘、呼尘）锰及其化合物浓度检测结果                ", WdParagraphAlignment.wdAlignParagraphCenter);
           SetParaText(1, 12, "单位：mg/m3", WdParagraphAlignment.wdAlignParagraphRight);
           Word.Table table6 = m_doc.Tables.Add(m_wordApp.Selection.Range, chemicalModels.Count + 1, 6);   ///physicalmodel有一条加一条
           table6.Borders.Enable = 1;
           table6.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
           table6.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
           table6.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
           table6.Rows.Height = 20;
           table6.Rows[1].Height = 30;
           table6.Columns[2].Width = 40;
           table6.Columns[3].Width = 100;
           table6.Cell(1, 1).Select();
           SetCellHeaderText("车间");
           table6.Cell(1, 2).Select();
           SetCellHeaderText("岗位");
           table6.Cell(1, 3).Select();
           SetCellHeaderText("采样地点");
           table6.Cell(1, 4).Select();
           SetCellHeaderText("样品编号");
           table6.Cell(1, 5).Select();
           SetCellHeaderText("检测结果");        
           int i = 2;
           foreach (var item in chemicalModels)
           {
               table6.Cell(i, 1).Select();
               SetCellText(0, 12, item.WorkShop, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table6.Cell(i, 2).Select();
               SetCellText(0, 12, item.Job, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table6.Cell(i, 3).Select();
               SetCellText(0, 12, item.Location, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table6.Cell(i, 4).Select();
               SetCellText(0, 12, item.SampleNumber, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               table6.Cell(i, 5).Select();
               SetCellText(0, 12, item.CMAC, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
               i++;
           }
           m_wordApp.Selection.MoveDown();           
       }

    }
}
