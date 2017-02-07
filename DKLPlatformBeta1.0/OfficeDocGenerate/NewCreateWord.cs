using System;
using System.Collections.Generic;
using System.Linq;
using Word = NetOffice.WordApi;
using NetOffice.WordApi.Enums;
using System.IO;
using DKLManager.Contract.Model;




namespace OfficeDocGenerate
{
    public class NewCreateWord
    {

        string Header = "2015JP140                    DKL/QB-28-02                第1页共10页";
        string Footer = "北京德康莱安全卫生技术发展有限公司  邮箱：service@deucalion.com.cn  邮编：100176\r\n联系电话：（010）51570159"
               + "地址：北京市亦庄经济技术开发区西环南路18号△：    网址：www.deucalion.com.cn \r\n传    真：（010）51570168";

        Word.Application m_wordApp;
        Word.Document m_doc;
        Word.Tools.CommonUtils m_utils;
       
        public NewCreateWord(ProjectInfo projectinfo)
        {
            // start word and turn off msg boxes
            this.Header = projectinfo.ProjectNumber + "                   DKL/QB-28-02                     "; ;
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
                WriteFirstPage FirstPage = new OfficeDocGenerate.WriteFirstPage(projectmodels.ProjectNumber, projectmodels.CompaneName);
                #endregion
                m_wordApp.DisplayAlerts = WdAlertLevel.wdAlertsNone;
                m_utils = new Word.Tools.CommonUtils(m_wordApp);
                m_doc = m_wordApp.Documents.Add();

                //SetPage();
                WriteFirstPage(FirstPage.ReportNumber, FirstPage.Client, projectmodels.CreateTime.Year.ToString() + "年" + projectmodels.CreateTime.Month.ToString() + "月" + projectmodels.CreateTime.Day.ToString() + "日");
                m_doc.Paragraphs.Last.Range.InsertBreak();
                WriteInstruction();
                m_doc.Paragraphs.Last.Range.InsertBreak();
                appStatus = WriteReportBasicInfo(projectmodels, str, strc, ProjectGistmodels, Parametermodels);
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
            string name = "报告模板样例" + DateTime.Now.ToFileTime();
            //      string documentFile = m_utils.File.Combine("c:\\", name, Word.Tools.DocumentFormat.Normal);
            string path = "d://DKLdownload";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string documentFile = m_utils.File.Combine(path, "报告模板样例", Word.Tools.DocumentFormat.Normal);
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

        private void WriteFirstPage(string reportNum, string company, string reportDate)
        {
            m_wordApp.Selection.Font.Color = WdColor.wdColorBlack;
            m_wordApp.Selection.TypeText("\r\n");
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

            m_wordApp.Selection.Font.Bold = 2;
            m_wordApp.Selection.Font.Size = 24;
            m_wordApp.Selection.TypeText("北京德康莱安全卫生技术发展有限公司");

            m_wordApp.Selection.Font.Bold = 5;
            m_wordApp.Selection.Font.Size = 48;
            m_wordApp.Selection.TypeText("\r\n\r\n检测与评价报告");

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
            //Word.InlineShape shap = range.InlineShapes.AddPicture(@"NETofficeTest\NETofficeTest\bin\Debug\fv.png");

            m_doc.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.Text = Header;
            //  m_doc.Sections[0].Headers[WdHeaderFooterIndex.wdHeaderFooterEvenPages].Range.Text = Header;

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

        private void WriteInstruction()
        {
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 22;
            m_wordApp.Selection.TypeText("\r\n\r\n\r\n\r\n说明\r\n\r\n");
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            //create a list
            m_wordApp.Selection.TypeText("1. 	本报告仅对本次委托项目检测与评价结果负责。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("2. 	检测与评价工作依据有关法律、法规、标准、规范、协议和技术文件进行。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("3. 	报告中有涂改、增删或复印无效。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("4. 	检测与评价报告包括：封面、说明、首页、正文，并盖有计量认证章，检测报告专用章和骑缝章。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("5. 	本报告的检测与评价结果及我公司名称，未经我公司书面同意不得用于局部复制、使用、引用，也不得用于广告、评优及商品宣传。我公司概不承担由此引起的一切法律纠纷及责任。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("6. 	本报告无CMA资质认证标识及检验专用章无效。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("7. 	对本检测与评价报告有异议，请于收到报告之日起十五日内向本公司提出，逾期不予受理。");

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;

            m_wordApp.Selection.TypeText("\r\n\r\n");
            m_wordApp.Selection.ParagraphFormat.LeftIndent = 27;

            m_wordApp.Selection.TypeText("检测与评价单位：北京德康莱安全卫生技术发展有限公司\r\n"
            + "通信地址：北京市北京经济技术开发区西环南路18号汇龙森B座二层\r\n"
            + "联系电话：（010）51570158、  51570159 \r\n"
            + "传真：（010）51570168\r\n" + "邮政编码：100176\r\n");

        }

        private String WriteReportBasicInfo(ProjectInfo projectmodels, List<string> str, List<string> strc, List<SampleProjectGist> ProjectGistmodels, List<Parameter> Parametermodels)
        {
            Word.Table basicInfo = m_doc.Tables.Add(m_wordApp.Selection.Range, 14 + str.Count - 4 + str.Count + strc.Count, 5);  //-
            basicInfo.Borders.Enable = 1;
            basicInfo.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Columns[1].Width = 90;

            basicInfo.Rows[1].Height = 20;
            basicInfo.Cell(1, 1).Select();
            SetCellHeaderText("单位名称");
            //     basicInfo.Cell(1, 2).Select();
            basicInfo.Cell(1, 2).Merge(basicInfo.Cell(1, 5));
            basicInfo.Cell(1, 2).Select();
            SetCellText(1, 12, projectmodels.CompaneName, WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);

            basicInfo.Rows[2].Height = 20;
            basicInfo.Cell(2, 1).Select();
            SetCellHeaderText("单位地址");
            basicInfo.Cell(2, 2).Merge(basicInfo.Cell(2, 5));
            basicInfo.Cell(2, 2).Select();
            SetCellText(1, 12, projectmodels.CompanyAddress, WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);

            basicInfo.Rows[3].Height = 20;
            basicInfo.Cell(3, 1).Select();
            SetCellHeaderText("项目名称");
            basicInfo.Cell(3, 2).Merge(basicInfo.Cell(3, 5));
            basicInfo.Cell(3, 2).Select();
            SetCellText(1, 12, projectmodels.ProjectName, WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);

            basicInfo.Rows[4].Height = 20;
            basicInfo.Cell(4, 1).Select();
            SetCellHeaderText("联系人");
            basicInfo.Cell(4, 2).Select();
            SetCellText(1, 12, projectmodels.CompanyContact, WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);

            basicInfo.Cell(4, 2).Merge(basicInfo.Cell(4, 3));
            basicInfo.Cell(4, 3).Select();
            SetCellHeaderText("联系电话");
            basicInfo.Cell(4, 4).Select();
            SetCellText(1, 12, projectmodels.ContactTel, WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);


            basicInfo.Rows[5].Height = 20;
            basicInfo.Cell(5, 1).Select();
            SetCellHeaderText("邮编");
            basicInfo.Cell(5, 2).Merge(basicInfo.Cell(5, 3));
            basicInfo.Cell(5, 3).Select();
            SetCellHeaderText("检测类别");

            basicInfo.Cell(5, 1).Merge(basicInfo.Cell(7, 1));
            basicInfo.Cell(5, 1).Select();
            SetCellHeaderText("样品名称\r\n数量\r\n状态");

            basicInfo.Cell(5, 2).Merge(basicInfo.Cell(7, 3));
            basicInfo.Cell(5, 3).Select();
            SetCellHeaderText("采样日期");
            basicInfo.Cell(6, 3).Select();
            // SetCellText(1, 12, projectmodels.ProjectClosingDate, WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            SetCellHeaderText("样品接收日期");
            basicInfo.Cell(7, 3).Select();
            SetCellHeaderText("检测日期");
            if (str.Count == 0)             //采样项目为空 返回错误状态 3
                return "3";

            basicInfo.Cell(8, 1).Merge(basicInfo.Cell(8 + str.Count, 1));
            basicInfo.Cell(8, 1).Select();
            SetCellHeaderText("采样");
            //        basicInfo.Cell(9, 3).Merge(basicInfo.Cell(8+str.Count, 3));
            basicInfo.Cell(8, 2).Select();
            SetCellHeaderText("采样项目");
            basicInfo.Cell(8, 3).Select();
            SetCellHeaderText("采样依据");
            basicInfo.Cell(8, 4).Select();
            SetCellHeaderText("仪器名称");
            basicInfo.Cell(8, 5).Select();
            SetCellHeaderText("仪器编号");
            int i = 9;
            //foreach (var item in str)
            //{
            //    basicInfo.Cell(i, 2).Select();
            //    SetCellText(1, 12, item, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            //    i++;
            //}
            foreach (var item in ProjectGistmodels)
            {
                if (item != null)
                {
                    basicInfo.Cell(i, 2).Select();
                    SetCellText(1, 12, item.SampleProject, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    basicInfo.Cell(i, 3).Select();
                    SetCellText(1, 12, item.SampleGist, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    basicInfo.Cell(i, 4).Select();
                    SetCellText(1, 12, item.ApparatusName, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    basicInfo.Cell(i, 5).Select();
                    SetCellText(1, 12, item.ApparatusNumber, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                }
                else
                    return "2";
                i++;
            }


            basicInfo.Cell(9 + str.Count, 1).Merge(basicInfo.Cell(9 + str.Count + strc.Count + str.Count, 1));
            basicInfo.Cell(9 + str.Count, 1).Select();
            SetCellHeaderText("检测");
            basicInfo.Cell(9 + str.Count, 2).Height = 20;
            basicInfo.Cell(9 + str.Count, 2).Select();
            SetCellHeaderText("检测项目");
            basicInfo.Cell(9 + str.Count, 3).Select();
            SetCellHeaderText("检测依据");
            basicInfo.Cell(9 + str.Count, 4).Select();
            SetCellHeaderText("仪器名称");
            basicInfo.Cell(9 + str.Count, 5).Select();
            SetCellHeaderText("仪器编号");
            int j = 10 + str.Count;

            foreach (var item in Parametermodels)
            {
                if (item != null)
                {
                    basicInfo.Cell(j, 2).Select();
                    SetCellText(1, 12, item.ParameterName, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);

                    basicInfo.Cell(j, 3).Select();
                    SetCellText(1, 12, item.DetectionPursuant, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);

                    basicInfo.Cell(j, 4).Select();
                    SetCellText(1, 12, item.ApparatusName, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    basicInfo.Cell(j, 5).Select();
                    SetCellText(1, 12, item.ApparatusNumber, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                }
                else
                    return "1";
                j++;
            }

            basicInfo.Cell(14 + str.Count - 4 + str.Count + strc.Count, 1).Height = 120;
            basicInfo.Cell(14 + str.Count - 4 + str.Count + strc.Count, 1).Select();
            SetCellHeaderText("评价\r\n依据");
            basicInfo.Cell(14 + str.Count - 4 + str.Count + strc.Count, 2).Select();
            SetCellHeaderText("《工作场所有害因素职业接触限值 第1部分：化学有害因素》GBZ 2.1-2007\r\n《工作场所有害因素职业接触限值 第2部分：物理因素》GBZ 2.2-2007\r\n《工作场所空气中有害物质监测的采样规范》GBZ 159-2004\r\n《作业场所空气采样仪器的技术规范》GB/T 17061-1997\r\n《个体防护装备选用规范》GB/T 11651-2008");
            basicInfo.Cell(14 + str.Count - 4 + str.Count + strc.Count, 2).Merge(basicInfo.Cell(14 + str.Count - 4 + str.Count + strc.Count, 5));
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
        private void WriteTestReport(List<Parameter> ParmeterChemicalModels, List<TestPhysicalReport> physicalmodels, List<TestChemicalReport> models, List<string> str)
        {

            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.InsertBreak();
            SetParaText(1, 22, "检测报告", WdParagraphAlignment.wdAlignParagraphCenter);
            m_wordApp.Selection.TypeParagraph();
            SetParaText(1, 14, "1、本次检测职业病危害因素的职业接触限值见表1-表2。\r\n", WdParagraphAlignment.wdAlignParagraphLeft);
            WriteTestTable1();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            WriteTestTable2(ParmeterChemicalModels);
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            SetParaText(1, 14, "2、检测结果\r\n", WdParagraphAlignment.wdAlignParagraphLeft);
            WriteTestTable3(physicalmodels);
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            WriteTestTable4(models, str, ParmeterChemicalModels);
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            WriteTestTable5(models, str);
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格    
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格    
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格    
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格    
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格    
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格    
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格    
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格    
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格   
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格       
            SetParaText(0, 9, "注：苯、二甲苯的最低检出浓度分别为（0.6、3.3）mg/m3（以采集1.5L空气样品计）\r\n", WdParagraphAlignment.wdAlignParagraphLeft);
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格         
            WriteTestTable12(models, str);
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            SetParaText(0, 12, "\r此页以下无正文\r\r\r", WdParagraphAlignment.wdAlignParagraphLeft);
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            WriteEvaluationReport();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格 

            WriteTestTable6();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格 
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格 
            SetParaText(1, 12, "检测当天，所有运行设备的配套防护设施均正常开启。\r\n", WdParagraphAlignment.wdAlignParagraphLeft);
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格 
            WriteTestTable7();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格 

            WriteTestTable8();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            WriteTestTable9(physicalmodels);
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            WriteTestTable10(models, str);
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            WriteTestTable11(models, str);
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            SetParaText(0, 11, "注：苯的最低检出浓度为0.6mg/m3（以采集1.5L空气样品计）", WdParagraphAlignment.wdAlignParagraphLeft);
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            WriteTestTable13(models, str);
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            SetParaText(1, 14, "\r评价结论：", WdParagraphAlignment.wdAlignParagraphLeft);
            for (int i = 0; i < 5; ++i)
                m_wordApp.Selection.TypeText("\r\n");

            WriteLastReport();
        }

        private void WriteTestTable1()
        {
            SetParaText(1, 12, "表1  工作场所噪声职业接触限值", WdParagraphAlignment.wdAlignParagraphCenter);
            Word.Table table1 = m_doc.Tables.Add(m_wordApp.Selection.Range, 4, 3);
            table1.Borders.Enable = 1;
            table1.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            table1.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            table1.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;

            table1.Rows.Height = 20;
            table1.Cell(1, 1).Select();
            SetCellText(1, 12, "接触时间", WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            table1.Cell(1, 2).Select();
            SetCellText(1, 12, "接触限值/[dB(A)]", WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            table1.Cell(1, 3).Select();
            SetCellText(1, 12, "备注", WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            table1.Cell(2, 1).Select();
            SetCellText(0, 12, "5d/w，=8h/d", WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            table1.Cell(2, 2).Select();
            SetCellText(0, 12, "85", WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            table1.Cell(2, 3).Select();
            SetCellText(0, 12, "非稳态噪声计算8h等效声级", WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            table1.Cell(3, 1).Select();
            SetCellText(0, 12, "5d/w，≠8h/d", WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            table1.Cell(3, 2).Select();
            SetCellText(0, 12, "85", WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            table1.Cell(3, 3).Select();
            SetCellText(0, 12, "计算8h等效声级", WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            table1.Cell(4, 1).Select();
            SetCellText(0, 12, "≠5d/w", WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            table1.Cell(4, 2).Select();
            SetCellText(0, 12, "85", WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            table1.Cell(4, 3).Select();
            SetCellText(0, 12, "计算40h等效声级", WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
        }

        public void WriteTestTable2(List<Parameter> ParmeterChemicalModels)
        {
            SetParaText(1, 12, "表2  工作场所空气中化学有害因素职业接触限值", WdParagraphAlignment.wdAlignParagraphCenter);
            Word.Table table2 = m_doc.Tables.Add(m_wordApp.Selection.Range, 6 - 4 + ParmeterChemicalModels.Count(), 6);
            table2.Borders.Enable = 1;
            table2.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            table2.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            table2.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            table2.Rows.Height = 20;

            table2.Cell(1, 1).Merge(table2.Cell(2, 1));
            table2.Cell(1, 1).Select();
            SetCellText(1, 12, "序号", WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);

            table2.Cell(1, 2).Merge(table2.Cell(2, 2));
            table2.Cell(1, 2).Select();
            SetCellText(1, 12, "职业病\r\n危害因素", WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);

            table2.Cell(1, 3).Merge(table2.Cell(1, 6));
            table2.Cell(1, 3).Select();
            SetCellText(1, 12, "职业接触限值", WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);

            table2.Cell(2, 3).Select();
            SetCellText(1, 12, "短时间接触容许浓度\r\n（PC-STEL, mg/m3）", WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            table2.Cell(2, 4).Select();
            SetCellText(1, 12, "时间加权平均容许浓度\r\n（PC-TWA，mg/m3）", WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            table2.Cell(2, 5).Select();
            SetCellText(1, 12, "最高容许浓度\r\n（MAC，mg/m3）", WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            table2.Cell(2, 6).Select();
            SetCellText(1, 12, "超限倍数", WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
            int j = 1;
            int i = 3;
            foreach (var item in ParmeterChemicalModels)
            {
                table2.Cell(i, 1).Select();
                SetCellText(1, 12, j.ToString(), WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                table2.Cell(i, 2).Select();
                SetCellText(1, 12, item.ParameterName, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                table2.Cell(i, 3).Select();
                SetCellText(1, 12, item.ShorttimeTouchAllowConcentration, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                table2.Cell(i, 4).Select();
                SetCellText(1, 12, item.TimeWeightingAverageAllowConcentration, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                table2.Cell(i, 5).Select();
                SetCellText(1, 12, item.HighestAllowConcentration, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                table2.Cell(i, 6).Select();
                SetCellText(1, 12, item.TransfiniteMultiple, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                i++;
                j++;
            }



        }
        private void WriteTestTable3(List<TestPhysicalReport> physicalModels)
        {
            SetParaText(1, 12, "表3  工作场所稳态噪声强度测量结果             ", WdParagraphAlignment.wdAlignParagraphCenter);
            SetParaText(1, 12, "单位：dB(A)", WdParagraphAlignment.wdAlignParagraphRight);
            Word.Table table3 = m_doc.Tables.Add(m_wordApp.Selection.Range, physicalModels.Count + 1, 7);
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
            SetCellHeaderText("样品编号");
            table3.Cell(1, 5).Select();
            SetCellHeaderText("接触时间h/d");
            table3.Cell(1, 6).Select();
            SetCellHeaderText("噪声强度");
            table3.Cell(1, 7).Select();
            if (physicalModels.Count == 0 || physicalModels[0].LexCategory == 0)
                SetCellHeaderText("Lex8H");
            else
                SetCellHeaderText("LexW");
            int i = 2;
            foreach (var item in physicalModels)
            {
                table3.Cell(i, 1).Select();
                SetCellText(1, 12, item.WordShop, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                table3.Cell(i, 2).Select();
                SetCellText(1, 12, item.Job, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                table3.Cell(i, 3).Select();
                SetCellText(1, 12, item.Location, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                table3.Cell(i, 4).Select();
                SetCellText(1, 12, item.SampleNumber, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                table3.Cell(i, 5).Select();
                SetCellText(1, 12, item.ContactTime, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                table3.Cell(i, 6).Select();
                SetCellText(1, 12, item.NoiseIntensity, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                table3.Cell(i, 7).Select();
                SetCellText(1, 12, item.Lex8hLexw, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                i++;
            }
        }

        private void WriteTestTable4(List<TestChemicalReport> models, List<string> str, List<Parameter> ParmeterChemicalModels)
        {
            SetParaText(1, 12, "\r\r\r\r\r\r\r\r", WdParagraphAlignment.wdAlignParagraphCenter);
            SetParaText(1, 12, "表4  工作场所空气中木粉尘浓度检测结果", WdParagraphAlignment.wdAlignParagraphCenter);
            SetParaText(1, 12, "\r单位：mg/m3", WdParagraphAlignment.wdAlignParagraphRight);
            int j = 0;
            //foreach (var item in models)
            //{
            //    if (item.SampleProject == str[0])
            //        j++;
            //}
            foreach (var item in models)
            {
                if (item.SampleProject == "木粉尘")
                    j++;
            }
            string twa = "3";
            foreach (var item in ParmeterChemicalModels)
            {
                if (item.ParameterName == "木粉尘")
                {
                    twa = item.TimeWeightingAverageAllowConcentration;
                    break;
                }
            }
            Word.Table table4 = m_doc.Tables.Add(m_wordApp.Selection.Range, j + 2, 8);
            table4.Borders.Enable = 1;
            table4.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            table4.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            table4.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;

            table4.Rows.Height = 20;
            table4.Columns[2].Width = 40;
            table4.Columns[1].Width = 40;
            //table4.Columns[5].Width = 100;
            table4.Columns[8].Width = 100;
            table4.Cell(1, 1).Merge(table4.Cell(2, 1));
            table4.Cell(1, 1).Select();
            SetCellHeaderText("车间");
            table4.Cell(1, 2).Merge(table4.Cell(2, 2));
            table4.Cell(1, 2).Select();
            SetCellHeaderText("岗位");
            table4.Cell(1, 3).Merge(table4.Cell(2, 3));
            table4.Cell(1, 3).Select();
            SetCellHeaderText("采样地点");
            table4.Cell(1, 4).Merge(table4.Cell(2, 4));
            table4.Cell(1, 4).Select();
            SetCellHeaderText("职业病\r\n危害因素");
            table4.Cell(1, 5).Merge(table4.Cell(2, 5));
            table4.Cell(1, 5).Select();
            SetCellHeaderText("样品编号");
            table4.Cell(1, 6).Merge(table4.Cell(1, 8));
            table4.Cell(1, 6).Select();
            SetCellHeaderText("检测结果");
            table4.Cell(2, 6).Select();
            SetCellHeaderText("短时间\r\n接触浓度");
            table4.Cell(2, 7).Select();
            SetCellHeaderText("CTWA");
            table4.Cell(2, 8).Select();
            SetCellHeaderText("短时间接触浓度/PC-TWA");


            int i = 3;
            foreach (var item in models)
            {
                //if (item.SampleProject == str[0])
                if (item.SampleProject == "木粉尘")
                {
                    table4.Cell(i, 1).Select();
                    SetCellText(1, 12, item.WorkShop, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table4.Cell(i, 2).Select();
                    SetCellText(1, 12, item.Job, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table4.Cell(i, 3).Select();
                    SetCellText(1, 12, item.Location, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table4.Cell(i, 4).Select();
                    SetCellText(1, 12, item.Factor, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table4.Cell(i, 5).Select();
                    SetCellText(1, 12, item.SampleNumber, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table4.Cell(i, 6).Select();
                    SetCellText(1, 12, item.CSTEL, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table4.Cell(i, 7).Select();
                    SetCellText(1, 12, item.CTWA, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table4.Cell(i, 8).Select();
                    if (item.CSTEL != null && item.CTWA != null)
                        SetCellText(1, 12, (float.Parse(item.CSTEL) / float.Parse(twa)).ToString("0.00"), WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);


                    i++;
                }
            }


        }

        private void WriteTestTable5(List<TestChemicalReport> models, List<string> str)
        {
            SetParaText(1, 12, "      ", WdParagraphAlignment.wdAlignParagraphCenter);

            SetParaText(1, 12, "\n", WdParagraphAlignment.wdAlignParagraphRight);
            SetParaText(1, 12, "表5  工作场所空气中苯、甲苯、二甲苯、乙酸乙酯、乙酸丁酯浓度检测结果\r\n", WdParagraphAlignment.wdAlignParagraphCenter);
            SetParaText(1, 12, "单位：mg/m3", WdParagraphAlignment.wdAlignParagraphRight);
            int j = 0;
            foreach (var item in models)
            {
                //if (item.SampleProject == str[1])
                if (item.SampleProject == "苯" || item.SampleProject == "甲苯" || item.SampleProject == "二甲苯" || item.SampleProject == "二甲苯" || item.SampleProject == "乙酸乙酯" || item.SampleProject == "乙酸丁酯")
                {
                    j++;
                }
            }
            Word.Table table5 = m_doc.Tables.Add(m_wordApp.Selection.Range, j + 2, 7);
            table5.Borders.Enable = 1;
            table5.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            table5.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            table5.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;

            table5.Rows.Height = 20;
            table5.Cell(1, 1).Merge(table5.Cell(2, 1));
            table5.Cell(1, 1).Select();
            SetCellHeaderText("车间");
            table5.Cell(1, 2).Merge(table5.Cell(2, 2));
            table5.Cell(1, 2).Select();
            SetCellHeaderText("岗位");
            table5.Cell(1, 3).Merge(table5.Cell(2, 3));
            table5.Cell(1, 3).Select();
            SetCellHeaderText("采样地点");
            table5.Cell(1, 4).Merge(table5.Cell(2, 4));
            table5.Cell(1, 4).Select();
            SetCellHeaderText("职业病危害因素");
            table5.Cell(1, 5).Merge(table5.Cell(2, 5));
            table5.Cell(1, 5).Select();
            SetCellHeaderText("样品编号");

            table5.Cell(1, 6).Merge(table5.Cell(1, 7));
            table5.Cell(1, 6).Select();
            SetCellHeaderText("检测结果");
            table5.Cell(2, 6).Select();
            SetCellHeaderText("CSTEL");
            table5.Cell(2, 7).Select();
            SetCellHeaderText("CTWA");
            int i = 3;
            foreach (var item in models)
            {
                //if (item.SampleProject == str[1])
                if (item.SampleProject == "苯" || item.SampleProject == "甲苯" || item.SampleProject == "二甲苯" || item.SampleProject == "二甲苯" || item.SampleProject == "乙酸乙酯" || item.SampleProject == "乙酸丁酯")
                {
                    table5.Cell(i, 1).Select();
                    SetCellText(1, 12, item.WorkShop, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table5.Cell(i, 2).Select();
                    SetCellText(1, 12, item.Job, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table5.Cell(i, 3).Select();
                    SetCellText(1, 12, item.Location, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table5.Cell(i, 4).Select();
                    SetCellText(1, 12, item.Factor, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table5.Cell(i, 5).Select();
                    SetCellText(1, 12, item.SampleNumber, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table5.Cell(i, 6).Select();
                    SetCellText(1, 12, item.CSTEL, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table5.Cell(i, 7).Select();
                    SetCellText(1, 12, item.CTWA, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    i++;
                }
            }

        }
        private void WriteTestTable12(List<TestChemicalReport> models, List<string> str)
        {
            SetParaText(1, 12, "\r\r   ", WdParagraphAlignment.wdAlignParagraphCenter);
            SetParaText(1, 12, "表6  工作场所空气中甲醛浓度检测结果", WdParagraphAlignment.wdAlignParagraphCenter);
            SetParaText(1, 12, "\r单位：mg/m3", WdParagraphAlignment.wdAlignParagraphRight);
            int j = 0;
            foreach (var item in models)
            {
                //if (item.SampleProject == str[2])
                if (item.SampleProject == "甲醛")
                    j++;
            }
            Word.Table table4 = m_doc.Tables.Add(m_wordApp.Selection.Range, j + 2, 7);
            table4.Borders.Enable = 1;
            table4.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            table4.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            table4.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;

            table4.Rows.Height = 20;
            table4.Columns[2].Width = 40;
            table4.Columns[1].Width = 40;
            //table4.Columns[5].Width = 100;
            table4.Columns[7].Width = 100;
            table4.Cell(1, 1).Merge(table4.Cell(2, 1));
            table4.Cell(1, 1).Select();
            SetCellHeaderText("车间");
            table4.Cell(1, 2).Merge(table4.Cell(2, 2));
            table4.Cell(1, 2).Select();
            SetCellHeaderText("岗位");
            table4.Cell(1, 3).Merge(table4.Cell(2, 3));
            table4.Cell(1, 3).Select();
            SetCellHeaderText("采样地点");
            table4.Cell(1, 4).Merge(table4.Cell(2, 4));
            table4.Cell(1, 4).Select();
            SetCellHeaderText("职业病\r\n危害因素");
            table4.Cell(1, 5).Merge(table4.Cell(2, 5));
            table4.Cell(1, 5).Select();
            SetCellHeaderText("样品编号");
            table4.Cell(1, 6).Merge(table4.Cell(1, 7));
            table4.Cell(1, 6).Select();
            SetCellHeaderText("检测结果");
            table4.Cell(2, 6).Select();
            SetCellHeaderText("短时间\r\n接触浓度");

            table4.Cell(2, 7).Select();
            SetCellHeaderText("CMAC");


            int i = 3;
            foreach (var item in models)
            {
                //if (item.SampleProject == str[2])
                if (item.SampleProject == "甲醛")
                {
                    table4.Cell(i, 1).Select();
                    SetCellText(1, 12, item.WorkShop, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table4.Cell(i, 2).Select();
                    SetCellText(1, 12, item.Job, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table4.Cell(i, 3).Select();
                    SetCellText(1, 12, item.Location, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table4.Cell(i, 4).Select();
                    SetCellText(1, 12, item.Factor, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table4.Cell(i, 5).Select();
                    SetCellText(1, 12, item.SampleNumber, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table4.Cell(i, 6).Select();
                    SetCellText(1, 12, item.CSTEL, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table4.Cell(i, 7).Select();
                    SetCellText(1, 12, item.CMAC, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);


                    i++;
                }
            }
        }
        private void WriteEvaluationReport()
        {
            SetParaText(1, 22, "\r评价报告\r\n", WdParagraphAlignment.wdAlignParagraphCenter);
            SetParaText(1, 14, "一、现场调查情况\r\n", WdParagraphAlignment.wdAlignParagraphLeft);
            SetParaText(1, 12, "1、工作场所概况：\r\n", WdParagraphAlignment.wdAlignParagraphLeft);
            for (int i = 0; i < 5; ++i)
                m_wordApp.Selection.TypeText("\r\n");
            SetParaText(1, 12, "2、评价范围：\r\n", WdParagraphAlignment.wdAlignParagraphLeft);
            for (int i = 0; i < 5; ++i)
                m_wordApp.Selection.TypeText("\r\n");
            SetParaText(1, 12, "3、\r\n", WdParagraphAlignment.wdAlignParagraphLeft);
            for (int i = 0; i < 5; ++i)
                m_wordApp.Selection.TypeText("\r\n");
            SetParaText(1, 12, "4、生产设备及职业病危害防护设施运行情况：见表1。\n", WdParagraphAlignment.wdAlignParagraphLeft);
        }
        private void WriteTestTable6()
        {
            SetParaText(1, 12, "表1生产设备及职业病危害防护设施运行情况", WdParagraphAlignment.wdAlignParagraphCenter);
            Word.Table table6 = m_doc.Tables.Add(m_wordApp.Selection.Range, 7, 4);
            table6.Borders.Enable = 1;
            table6.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            table6.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            table6.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;

            //table6.Rows.Height = 20;
            //table6.Columns[2].Width = 120;
            //table6.Columns[3].Width = 120;
            table6.Cell(1, 1).Select();
            SetCellHeaderText("车间");
            table6.Cell(1, 2).Select();
            SetCellHeaderText("设备");
            table6.Cell(1, 3).Select();
            SetCellHeaderText("运行数/总数");
            table6.Cell(1, 4).Select();
            SetCellHeaderText("职业病防护设施");
            //       table6.Cell(2, 1).Merge(table6.Cell(7, 1));
            table6.Cell(2, 1).Select();
            //     table6.Cell(2, 4).Merge(table6.Cell(7, 4));
            table6.Cell(2, 4).Select();
            SetCellHeaderText(" ");
        }
        private void WriteTestTable7()
        {
            //评价报告表2
            SetParaText(1, 12, "5、原辅料使用情况：见表2。\r\n\r\n\r\n\r\n", WdParagraphAlignment.wdAlignParagraphLeft);
            SetParaText(1, 12, "表2 工作场所原辅料调查情况", WdParagraphAlignment.wdAlignParagraphCenter);
            Word.Table table7 = m_doc.Tables.Add(m_wordApp.Selection.Range, 8, 4);
            table7.Borders.Enable = 1;
            table7.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            table7.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            table7.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            table7.Rows.Height = 20;

            table7.Cell(1, 1).Select();
            SetCellHeaderText("原辅料名称");
            table7.Cell(1, 2).Select();
            SetCellHeaderText("主要成分");
            table7.Cell(1, 3).Select();
            SetCellHeaderText("涉及岗位");
            table7.Cell(1, 4).Select();
            SetCellHeaderText("月用量");
            //  table7.Cell(2, 3).Merge(table7.Cell(4, 3));
            table7.Cell(2, 3).Select();
            //           table7.Cell(6, 2).Merge(table7.Cell(8, 2));
            table7.Cell(6, 2).Select();
            //          table7.Cell(6, 3).Merge(table7.Cell(8, 3));
            table7.Cell(6, 3).Select();
            //table7.Cell(2, 3).Merge(table7.Cell(4, 3));
            //table7.Cell(2, 3).Select();
            //           table7.Cell(2, 4).Merge(table7.Cell(4, 4));
            table7.Cell(8, 4).Select();
            SetCellHeaderText("");
        }
        private void WriteTestTable8()
        {
            //评价报告表3
            SetParaText(1, 12, "6、车间、岗位劳动定员、接触职业病危害因素、工作方式及作业者配备个人职业病防护用品情况：见表3。\r\n", WdParagraphAlignment.wdAlignParagraphLeft);
            SetParaText(1, 12, "表3  工作场所现场调查情况", WdParagraphAlignment.wdAlignParagraphCenter);
            Word.Table table8 = m_doc.Tables.Add(m_wordApp.Selection.Range, 7, 10);
            table8.Borders.Enable = 1;
            table8.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            table8.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            table8.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            table8.Rows.Height = 20;
            table8.Rows[1].Height = 30;
            table8.Rows[6].Height = 30;

            table8.Cell(1, 1).Select();
            SetCellHeaderText("车间");
            table8.Cell(1, 2).Select();
            SetCellHeaderText("岗位");
            table8.Cell(1, 3).Select();
            SetCellHeaderText("劳动定员");
            table8.Cell(1, 4).Select();
            SetCellHeaderText("工作班制h/d、d/w");
            table8.Cell(1, 5).Select();
            SetCellHeaderText("接触时间h/d");
            table8.Cell(1, 6).Select();
            SetCellHeaderText("作业方式");
            table8.Cell(1, 7).Select();
            SetCellHeaderText("接触职业病危害因素");
            table8.Cell(1, 8).Select();
            SetCellHeaderText("采样方式");
            table8.Cell(1, 9).Select();
            SetCellHeaderText("职业病防护设施");
            table8.Cell(1, 10).Select();
            SetCellHeaderText("个人职业病防护用品");
            /*
                        table8.Cell(2, 1).Merge(table8.Cell(4, 1));
                        table8.Cell(5, 1).Merge(table8.Cell(6, 1));
                        table8.Cell(2, 4).Merge(table8.Cell(6, 4));
                        table8.Cell(2, 6).Merge(table8.Cell(6, 6));
                        table8.Cell(2, 7).Merge(table8.Cell(3, 7));
                        table8.Cell(2, 8).Merge(table8.Cell(6, 8));
                        table8.Cell(2, 9).Merge(table8.Cell(4, 9));
                        table8.Cell(2, 10).Merge(table8.Cell(4, 10));
                        table8.Cell(5, 10).Merge(table8.Cell(6, 10));
                        table8.Cell(7, 1).Merge(table8.Cell(7, 2));
                        table8.Cell(7, 2).Merge(table8.Cell(7, 9));
             */
            table8.Cell(7, 2).Select();

            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格 
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格 
            m_wordApp.Selection.TypeText("\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n");

            SetParaText(1, 14, "二、检测条件和样品采集的质量控制", WdParagraphAlignment.wdAlignParagraphLeft);
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    现场使用的采样仪器、空气收集器及现场测量仪器符合国家相关标准的要求，仪器设备经过计量检定或校准，检测与评价人员持证上岗，报告经三级审核。\n    本次职业病危害因素检测按照表3中所存在的职业病危害因素进行相应的现场测量与样品采集。检测当天，封边、打磨岗位未作业，不具备检测条件。\n    采样和测量时，同步进行温度、湿度、大气压等场所内微小气候测定。");
        }
        private void WriteTestTable9(List<TestPhysicalReport> physicalModels)
        {
            //结果评价  表4
            SetParaText(1, 14, "\r\r\r三、结果评价\r", WdParagraphAlignment.wdAlignParagraphLeft);
            SetParaText(1, 12, "表4  工作场所稳态噪声强度测量结果             ", WdParagraphAlignment.wdAlignParagraphCenter);
            SetParaText(1, 12, "单位：dB(A)", WdParagraphAlignment.wdAlignParagraphRight);
            int j = 0;
            foreach (var item in physicalModels)
            {
                j++;
            }
            Word.Table table9 = m_doc.Tables.Add(m_wordApp.Selection.Range, j + 1, 7);
            table9.Borders.Enable = 1;
            table9.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            table9.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            table9.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            table9.Rows.Height = 20;
            table9.Rows[1].Height = 30;
            table9.Columns[2].Width = 40;
            table9.Columns[3].Width = 100;
            table9.Cell(1, 1).Select();
            SetCellHeaderText("车间");
            table9.Cell(1, 2).Select();
            SetCellHeaderText("岗位");
            table9.Cell(1, 3).Select();
            SetCellHeaderText("测量地点");
            table9.Cell(1, 4).Select();
            SetCellHeaderText("接触时间h/d");
            table9.Cell(1, 5).Select();
            SetCellHeaderText("噪声强度");
            table9.Cell(1, 6).Select();
            SetCellHeaderText("LEX,W");
            table9.Cell(1, 7).Select();
            SetCellHeaderText("结果判定");
            int i = 2;
            foreach (var item in physicalModels)
            {
                table9.Cell(i, 1).Select();
                SetCellText(1, 12, item.WordShop, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                table9.Cell(i, 2).Select();
                SetCellText(1, 12, item.Job, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                table9.Cell(i, 3).Select();
                SetCellText(1, 12, item.Location, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                table9.Cell(i, 4).Select();
                SetCellText(1, 12, item.ContactTime, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                table9.Cell(i, 5).Select();
                SetCellText(1, 12, item.NoiseIntensity, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                table9.Cell(i, 6).Select();
                SetCellText(1, 12, item.Lex8hLexw, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                table9.Cell(i, 7).Select();
                SetCellText(1, 12, item.ResultVerdict, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                i++;
            }


        }
        private void WriteTestTable10(List<TestChemicalReport> models, List<string> str)
        {
            SetParaText(1, 12, "\r\r表5  工作场所空气中总尘（木粉尘）浓度检测结果          ", WdParagraphAlignment.wdAlignParagraphCenter);
            SetParaText(1, 12, "单位：mg/m3", WdParagraphAlignment.wdAlignParagraphRight);
            int j = 0;
            foreach (var item in models)
            {
                //if (item.SampleProject == str[0])
                if (item.SampleProject == "木粉尘")
                    j++;
            }
            Word.Table table10 = m_doc.Tables.Add(m_wordApp.Selection.Range, j + 2, 7);
            table10.Borders.Enable = 1;
            table10.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            table10.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            table10.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            table10.Rows.Height = 20;
            table10.Cell(1, 1).Merge(table10.Cell(2, 1));
            table10.Cell(1, 1).Select();
            SetCellHeaderText("车间");
            table10.Cell(1, 2).Merge(table10.Cell(2, 2));
            table10.Cell(1, 2).Select();
            SetCellHeaderText("岗位");
            table10.Cell(1, 3).Merge(table10.Cell(2, 3));
            table10.Cell(1, 3).Select();
            SetCellHeaderText("采样地点");
            table10.Cell(1, 4).Merge(table10.Cell(1, 6));
            table10.Cell(1, 4).Select();
            SetCellHeaderText("检测结果");
            table10.Cell(2, 4).Select();
            SetCellHeaderText("短时间\r\n接触浓度");
            table10.Cell(2, 5).Select();
            SetCellHeaderText("CTWA");
            table10.Cell(2, 6).Select();
            SetCellHeaderText("短时间接触浓度/PC-TWA");
            table10.Cell(1, 5).Merge(table10.Cell(2, 7));
            table10.Cell(1, 5).Select();
            SetCellHeaderText("结果判定");
            int i = 3;
            foreach (var item in models)
            {
                //if (item.SampleProject == str[0])
                if (item.SampleProject == "木粉尘")
                {
                    table10.Cell(i, 1).Select();
                    SetCellText(1, 12, item.WorkShop, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table10.Cell(i, 2).Select();
                    SetCellText(1, 12, item.Job, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table10.Cell(i, 3).Select();
                    SetCellText(1, 12, item.Location, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table10.Cell(i, 4).Select();
                    SetCellText(1, 12, item.CSTEL, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table10.Cell(i, 5).Select();
                    SetCellText(1, 12, item.CTWA, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table10.Cell(i, 6).Select();
                    if (item.CSTEL != null && item.CTWA != null)
                        SetCellText(1, 12, (float.Parse(item.CSTEL) / float.Parse(item.CTWA)).ToString(), WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table10.Cell(i, 7).Select();
                    SetCellText(1, 12, item.ResultVerdict, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    i++;
                }
            }
        }
        private void WriteTestTable11(List<TestChemicalReport> models, List<string> str)
        {
            SetParaText(1, 12, "\r\r表6  工作场所空气中苯、甲苯、二甲苯、乙酸乙酯、乙酸丁酯浓度检测结果\r", WdParagraphAlignment.wdAlignParagraphCenter);
            SetParaText(1, 12, "单位：mg/m3", WdParagraphAlignment.wdAlignParagraphRight);
            int j = 0;
            foreach (var item in models)
            {
                //if (item.SampleProject == str[1])
                if (item.SampleProject == "苯" || item.SampleProject == "甲苯" || item.SampleProject == "二甲苯" || item.SampleProject == "二甲苯" || item.SampleProject == "乙酸乙酯" || item.SampleProject == "乙酸丁酯")
                    j++;
            }
            Word.Table table11 = m_doc.Tables.Add(m_wordApp.Selection.Range, j + 2, 7);
            table11.Borders.Enable = 1;
            table11.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            table11.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            table11.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            table11.Rows.Height = 20;

            table11.Cell(1, 1).Merge(table11.Cell(2, 1));
            table11.Cell(1, 1).Select();
            SetCellHeaderText("车间");
            table11.Cell(1, 2).Merge(table11.Cell(2, 2));
            table11.Cell(1, 2).Select();
            SetCellHeaderText("岗位");
            table11.Cell(1, 3).Merge(table11.Cell(2, 3));
            table11.Cell(1, 3).Select();
            SetCellHeaderText("采样地点");
            table11.Cell(1, 4).Merge(table11.Cell(2, 4));
            table11.Cell(1, 4).Select();
            SetCellHeaderText("职业病危害因素");
            table11.Cell(1, 5).Merge(table11.Cell(1, 6));
            table11.Cell(1, 5).Select();
            SetCellHeaderText("检测结果");
            table11.Cell(2, 5).Select();
            SetCellHeaderText("CSTEL");
            table11.Cell(2, 6).Select();
            SetCellHeaderText("CTWA");
            table11.Cell(1, 6).Merge(table11.Cell(2, 7));
            table11.Cell(1, 6).Select();
            SetCellHeaderText("结果判定");
            int i = 3;
            foreach (var item in models)
            {
                //if (item.SampleProject == str[1])
                if (item.SampleProject == "苯" || item.SampleProject == "甲苯" || item.SampleProject == "二甲苯" || item.SampleProject == "二甲苯" || item.SampleProject == "乙酸乙酯" || item.SampleProject == "乙酸丁酯")
                {
                    table11.Cell(i, 1).Select();
                    SetCellText(1, 12, item.WorkShop, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table11.Cell(i, 2).Select();
                    SetCellText(1, 12, item.Job, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table11.Cell(i, 3).Select();
                    SetCellText(1, 12, item.Location, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table11.Cell(i, 4).Select();
                    SetCellText(1, 12, item.Factor, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table11.Cell(i, 5).Select();
                    SetCellText(1, 12, item.CSTEL, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table11.Cell(i, 6).Select();
                    SetCellText(1, 12, item.CTWA, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table11.Cell(i, 7).Select();
                    SetCellText(1, 12, item.ResultVerdict, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    i++;
                }
            }

            //        m_wordApp.Selection.MoveDown();
            //          SetParaText(0, 11, "注：苯的最低检出浓度为0.6mg/m3（以采集1.5L空气样品计）", WdParagraphAlignment.wdAlignParagraphLeft);
        }
        private void WriteTestTable13(List<TestChemicalReport> models, List<string> str)
        {
            SetParaText(1, 12, "\r\r   ", WdParagraphAlignment.wdAlignParagraphCenter);
            SetParaText(1, 12, "\r表7  工作场所空气中甲醛浓度检测结果", WdParagraphAlignment.wdAlignParagraphCenter);
            SetParaText(1, 12, "\r单位：mg/m3", WdParagraphAlignment.wdAlignParagraphRight);
            int j = 0;
            foreach (var item in models)
            {
                //if (item.SampleProject == str[2])
                if (item.SampleProject == "甲醛")
                    j++;
            }
            Word.Table table4 = m_doc.Tables.Add(m_wordApp.Selection.Range, j + 2, 6);
            table4.Borders.Enable = 1;
            table4.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            table4.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            table4.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            table4.Rows.Height = 20;
            table4.Columns[2].Width = 70;
            table4.Columns[1].Width = 70;
            table4.Cell(1, 1).Merge(table4.Cell(2, 1));
            table4.Cell(1, 1).Select();
            SetCellHeaderText("车间");
            table4.Cell(1, 2).Merge(table4.Cell(2, 2));
            table4.Cell(1, 2).Select();
            SetCellHeaderText("岗位");
            table4.Cell(1, 3).Merge(table4.Cell(2, 3));
            table4.Cell(1, 3).Select();
            SetCellHeaderText("采样地点");
            table4.Cell(1, 4).Merge(table4.Cell(2, 4));
            table4.Cell(1, 4).Select();
            SetCellHeaderText("职业病\r\n危害因素");
            table4.Cell(1, 5).Select();
            SetCellHeaderText("检测结果");
            table4.Cell(2, 5).Select();
            SetCellHeaderText("CMAC");
            table4.Cell(1, 6).Merge(table4.Cell(2, 6));
            table4.Cell(1, 6).Select();
            SetCellHeaderText("结果判定");

            int i = 3;
            foreach (var item in models)
            {
                //if (item.SampleProject == str[2])
                if (item.SampleProject == "甲醛")
                {
                    table4.Cell(i, 1).Select();
                    SetCellText(1, 12, item.WorkShop, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table4.Cell(i, 2).Select();
                    SetCellText(1, 12, item.Job, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table4.Cell(i, 3).Select();
                    SetCellText(1, 12, item.Location, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table4.Cell(i, 4).Select();
                    SetCellText(1, 12, item.Factor, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table4.Cell(i, 5).Select();
                    SetCellText(1, 12, item.CMAC, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    table4.Cell(i, 6).Select();
                    SetCellText(1, 12, item.ResultVerdict, WdParagraphAlignment.wdAlignParagraphCenter, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
                    i++;
                }
            }
        }
        private void WriteLastReport()
        {
            SetParaText(1, 14, "四、建议", WdParagraphAlignment.wdAlignParagraphLeft);
            TypeText(0, 12, "   1、企业应向供应商索要油漆的MSDS，明确底漆、面漆岗位工人接触的职业病危害因,素，以便进行有针对性的职业病危害因素检测与评价。\r2、建议企业加强车间管理，督促劳动者正确佩戴个人职业病防护用品；对劳动者进行岗前职业卫生培训和岗中定期职业卫生培训，加强劳动者职业卫生知识的宣传，提高工人自我保护意识。");
            for (int i = 0; i < 5; ++i)
                m_wordApp.Selection.TypeText("\r\n");
            SetParaText(1, 14, "五、职业病危害因素对劳动者健康的影响及危害", WdParagraphAlignment.wdAlignParagraphLeft);
            m_wordApp.Selection.TypeParagraph();
            TypeText(0, 12, "    噪声：长期接触噪声可发生进行性的感音性听觉损伤。此外，生产性噪声对某些接触者的神经系统、心血管系统、内分泌系统及免疫系统、生殖系统和消化系统也会产生一定的损害。\r    木粉尘：生产性粉尘主要引起呼吸系统的损害，作业工人在生产环境中长期接触不同种类的粉尘可引起不同类型的尘肺病。矽尘能导致的是矽肺病。尘肺病是一种以肺组织弥漫性纤维化为主的全身性疾病，主要是以肺部纤维化改变为主，随着尘肺病病情的进一步发展还可以导致一些并发症的出现，如心脏等其他脏器的损害。\r    苯：为确定人类致癌物，可经呼吸道、皮肤进入人体。主要损害神经和造血系统。短期大量接触可引起头痛、头晕、恶心、呕吐、嗜睡、步态不稳，重者发生抽搐、昏迷。长期过量接触可引起白细胞减少、再生障碍性贫血、白血病。\r    甲苯：可经呼吸道、皮肤进入人体。短时间吸入高浓度甲苯可出现中枢神经系统功能障碍和皮肤粘膜刺激症状。长期接触中低浓度甲苯可出现不同程度的头晕、头痛、乏力、睡眠障碍和记忆力减退等症状。皮肤接触可致慢性皮炎、皮肤皲裂等。\r    二甲苯：可引起上呼吸道和眼刺激，中枢神经系统损害。\r    乙酸乙酯：对眼、鼻、咽喉有刺激作用。高浓度吸入可引起缓慢而渐进的麻醉作用，持续大量吸入可致呼吸麻痹，有致敏作用，因血管神经障碍而致牙龈路充血及粘膜炎症；可致湿疹样皮炎。长期接触有时可致角膜混浊，继发性贫血，白细胞增多等。\r    乙酸丁酯：对眼及上呼吸道均有强烈的刺激性作用，角膜上皮可有空泡形成，高浓度时可有麻痹作用，可引起皮肤干燥。\r");
            SetParaText(1, 12, "此页以下无正文", WdParagraphAlignment.wdAlignParagraphLeft);
            for (int i = 0; i < 10; ++i)
                m_wordApp.Selection.TypeText("\r\n");

            SetParaText(1, 12, "编制人：                             年    月    日\r", WdParagraphAlignment.wdAlignParagraphLeft);
            SetParaText(1, 12, "校核人：                             年    月    日\r", WdParagraphAlignment.wdAlignParagraphLeft);
            SetParaText(1, 12, "审核人：                             年    月    日\r", WdParagraphAlignment.wdAlignParagraphLeft);
            SetParaText(1, 12, "签发人：                             年    月    日\r", WdParagraphAlignment.wdAlignParagraphLeft);
            SetParaText(1, 12, "                                 北京德康莱安全卫生技术发展有限公司\r", WdParagraphAlignment.wdAlignParagraphLeft);
            SetParaText(1, 12, "                                               （印章）", WdParagraphAlignment.wdAlignParagraphLeft);
        }
    }
}

