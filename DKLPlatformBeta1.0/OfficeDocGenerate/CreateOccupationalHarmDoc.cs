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
using HYZK.FrameWork.Web;




namespace OfficeDocGenerate                                                         //职业病危害预评价
{
    public class CreateOccupationalHarmDoc:ControllerBase
    {
        string Header = "合同编号：                                 DKL/QB-11-05                      职业病危害预评价报告 ";
        string Footer = "北京德康莱健康安全科技股份有限公司                                                    ";
        Word.Application m_wordApp;
        Word.Document m_doc;
        Word.Tools.CommonUtils m_utils;

        public CreateOccupationalHarmDoc()
        {
            //this.model = Model;
            //// start word and turn off msg boxes
            //this.Header = model.ProjectNumber + "                   DKL/QB-28-02                     ";

        }
        public List<string> CreateReportWord()
        {
            string appStatus;
            string strRet = null;
            List<string> appList = new List<string>();  //函数执行状态+文件名
            //测试使用的是fristpage

            using (m_wordApp = new Word.Application())
            {
                #region  定义类
                WriteFirstPage FirstPage = new OfficeDocGenerate.WriteFirstPage("ProjectNumber", "CompaneName");
                #endregion
                m_wordApp.DisplayAlerts = WdAlertLevel.wdAlertsNone;
                m_utils = new Word.Tools.CommonUtils(m_wordApp);
                m_doc = m_wordApp.Documents.Add();

                //SetPage();
                WriteFirstPage("编号", "公司名", "二〇一五年六月一日");
                //      m_doc.Paragraphs.Last.Range.InsertBreak();
                WriteInstruction();
                //         m_doc.Paragraphs.Last.Range.InsertBreak();
              
                WriteContent();            
                WriteEmployerBasicInfo();
                WriteTotalLayout();
                WriteDeviceLayout();
                WriteBuildingHygiene();
                appStatus = "0";
                //WriteTestReport(ParmeterChemicalModels, physicalmodels, chemicalmodels, str);
                strRet = SaveFile();
                appList.Add(appStatus);
                appList.Add(strRet);
            }
            return appList;
        }
        private string SaveFile()
        {
            string fileName = "";
            string name = "职业病危害预评价报告" + DateTime.Now.ToFileTime();
            //      string documentFile = m_utils.File.Combine("c:\\", name, Word.Tools.DocumentFormat.Normal);
            string path = "d://DKLdownload";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string documentFile = m_utils.File.Combine(path, "职业病危害预评价报告", Word.Tools.DocumentFormat.Normal);
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
            
            Word.Table basicInfo = m_doc.Tables.Add(m_wordApp.Selection.Range, 4, 2);
            basicInfo.Rows.Alignment = WdRowAlignment.wdAlignRowRight;
            basicInfo.Borders.Enable = 1;
            basicInfo.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Columns[1].Width = 60;
            basicInfo.Columns[1].Width = 60;

            basicInfo.Rows[1].Height = 20;
            basicInfo.Cell(1, 1).Select();
            SetCellHeaderText("报告编号");
            basicInfo.Cell(1, 2).Select();
            SetCellHeaderText(reportNum);

            basicInfo.Rows[2].Height = 20;
            basicInfo.Cell(2, 1).Select();
            SetCellHeaderText("版次");
            basicInfo.Cell(2, 2).Select();
            SetCellHeaderText("第 1 版");

            basicInfo.Rows[3].Height = 20;
            basicInfo.Cell(3, 1).Select();
            SetCellHeaderText("总册数");
            basicInfo.Cell(3, 2).Select();
            SetCellHeaderText("共       本");

            basicInfo.Rows[4].Height = 20;
            basicInfo.Cell(4, 1).Select();
            SetCellHeaderText("本册为");
            basicInfo.Cell(4, 2).Select();
            SetCellHeaderText("第       本");

            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            m_wordApp.Selection.Font.Color = WdColor.wdColorBlack;
            m_wordApp.Selection.Font.Bold = 5;
            m_wordApp.Selection.Font.Size = 40;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("职业病危害预评价报告");

            m_wordApp.Selection.Font.Color = WdColor.wdColorBlack;
            m_wordApp.Selection.Font.Bold = 3;
            m_wordApp.Selection.Font.Size = 30;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("(正文册)");

            //测试结束
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 16;
            for (int i = 0; i < 8; ++i)
                m_wordApp.Selection.TypeText("\r\n");
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("\r\n北京德康莱安全卫生技术发展有限公司");
            m_wordApp.Selection.TypeText("\r\n二〇一五年十月");

            m_wordApp.Selection.TypeText("\r\n\r\n");

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
        //公司，日期
        private void WriteInstruction()
        {
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 3;
            m_wordApp.Selection.Font.Size = 30;
            m_wordApp.Selection.TypeText("\r\n\r\n\r\n声明\r\n\r\n");
           
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;   
            m_wordApp.Selection.TypeText("      北京德康莱健康安全科技股份有限公司遵守国家有关法律、法规，在北京A汽车有限公司B工厂二期建设项目职业病危害预评价过程坚持客观、真实、公正的原则，并对所出具的《北京A汽车有限公司B工厂二期建设项目职业病危害预评价报告》承担法律责任。");
            m_wordApp.Selection.TypeParagraph();
            
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("      评价机构名称：北京德康莱健康安全科技股份有限公司");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("\r\n");

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("      法人代表：王辉");   
            
            for (int i = 0; i < 3; ++i)
                m_wordApp.Selection.TypeText("\r\n");

            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("项目负责人：");
            m_wordApp.Selection.TypeText("\r\n");
            m_wordApp.Selection.TypeParagraph();
            
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("报告编写人：");
            m_wordApp.Selection.TypeText("\r\n");
            m_wordApp.Selection.TypeParagraph();
            
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("报告审核人：");
            m_wordApp.Selection.TypeText("\r\n");
            m_wordApp.Selection.TypeParagraph();
            
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("报告签发人：");
            m_wordApp.Selection.TypeText("\r\n");
            m_wordApp.Selection.TypeText("\r\n");
        }
        private void WriteContent()
        {
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 16;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("1 建设项目概况：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("1.1 建设项目名称：");
            for (int i = 0; i < 2; ++i)
                m_wordApp.Selection.TypeText("\r\n");

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("1.2 建设项目性质：");
            for (int i = 0; i < 2; ++i)
                m_wordApp.Selection.TypeText("\r\n");

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("1.3 建设单位：");
            for (int i = 0; i < 2; ++i)
                m_wordApp.Selection.TypeText("\r\n");

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("1.4 建设规模：");
            m_wordApp.Selection.TypeParagraph();
            
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("  项目投资规模：");
            m_wordApp.Selection.TypeParagraph();
            
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("  项目生产规模：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("1.5 主要生产内容：");
            m_wordApp.Selection.TypeParagraph();
            
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("  本项目涉及的工厂部门组成及生产内容见表1-1。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表1-1  工厂组成及生产内容");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo = m_doc.Tables.Add(m_wordApp.Selection.Range, 16, 4);
            basicInfo.Borders.Enable = 1;
            basicInfo.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Columns[1].Width = 60;
            basicInfo.Columns[2].Width = 90;
            basicInfo.Columns[4].Width = 150;

            basicInfo.Rows[1].Height = 20;
            basicInfo.Cell(1, 1).Select();
            SetCellHeaderText("序号");
            basicInfo.Cell(1, 2).Select();
            SetCellHeaderText("部门名称");
            basicInfo.Cell(1, 3).Select();
            SetCellHeaderText("生产内容");
            basicInfo.Cell(1, 4).Select();
            SetCellHeaderText("拟扩建项目设置情况");

            basicInfo.Rows[2].Height = 20;
            basicInfo.Cell(2, 1).Select();
            SetCellHeaderText("一");

            basicInfo.Rows[3].Height = 20;
            basicInfo.Cell(3, 1).Select();
            SetCellHeaderText("（一）");

            basicInfo.Rows[4].Height = 20;
            basicInfo.Cell(4, 1).Select();
            SetCellHeaderText("1");

            basicInfo.Rows[5].Height = 20;
            basicInfo.Cell(5, 1).Select();
            SetCellHeaderText("2");

            basicInfo.Rows[6].Height = 20;
            basicInfo.Cell(6, 1).Select();
            SetCellHeaderText("3");

            basicInfo.Rows[7].Height = 20;
            basicInfo.Cell(7, 1).Select();
            SetCellHeaderText("（二）");

            basicInfo.Rows[8].Height = 20;
            basicInfo.Cell(8, 1).Select();
            SetCellHeaderText("1");

            basicInfo.Rows[9].Height = 20;
            basicInfo.Cell(9, 1).Select();
            SetCellHeaderText("2");

            basicInfo.Rows[10].Height = 20;
            basicInfo.Cell(10, 1).Select();
            SetCellHeaderText("二");

            basicInfo.Rows[11].Height = 20;
            basicInfo.Cell(11,1).Select();
            SetCellHeaderText("1");

            basicInfo.Rows[12].Height = 20;
            basicInfo.Cell(12, 1).Select();
            SetCellHeaderText("2");

            basicInfo.Rows[13].Height = 20;
            basicInfo.Cell(13, 1).Select();
            SetCellHeaderText("3");

            basicInfo.Rows[14].Height = 20;
            basicInfo.Cell(14, 1).Select();
            SetCellHeaderText("4");

            basicInfo.Rows[15].Height = 20;
            basicInfo.Cell(15, 1).Select();
            SetCellHeaderText("5");

            basicInfo.Rows[16].Height = 20;
            basicInfo.Cell(16, 1).Select();
            SetCellHeaderText("6");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
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
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("1.6 拟扩建地点：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      本项目拟在北京经济技术开发区大兴区融兴北二街1号，亦庄南区北路以南，亦庄南区一街以北，亦庄南区六路以东，亦柏路以西，项目西南侧为北京李尔汽车系统公司，其他三侧为空地。");
            m_wordApp.Selection.TypeParagraph();
        }
        private void WriteEmployerBasicInfo(){
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2 职业病危害因素及其防护措施评价：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1 职业病危害因素评价：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.1 职业病危害因素识别与分析：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.1.1生产工艺过程中存在的有害因素：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      通过对拟扩建项目生产过程中所使用的原辅材料、产品、使用的生产设备和生产工艺过程进行系统分析，将拟扩建项目分为机械加工单元、装配单元和建设期施工单元3个评价单元进行职业病危害因素识别。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("其生产工艺过程中可能存在的职业病危害因素见表2-1。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("表2-1  拟建项目职业病危害因素的分布");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo = m_doc.Tables.Add(m_wordApp.Selection.Range,11,10);
            basicInfo.Borders.Enable = 1;
            basicInfo.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Columns[1].Width = 30;
            basicInfo.Columns[2].Width = 30;
            basicInfo.Columns[3].Width = 60;
            basicInfo.Columns[4].Width = 40;
            basicInfo.Columns[5].Width = 70;
            basicInfo.Columns[6].Width = 60;
            basicInfo.Columns[7].Width = 60;
            basicInfo.Columns[8].Width = 40;
            basicInfo.Columns[9].Width = 60;

            basicInfo.Rows[1].Height = 20;
            basicInfo.Cell(1, 1).Merge(basicInfo.Cell(1,2));
            basicInfo.Cell(1, 1).Select();
            SetCellHeaderText("评价单元");
            basicInfo.Cell(1, 2).Select();
            SetCellHeaderText("生产工艺过程");
            basicInfo.Cell(1, 3).Select();
            SetCellHeaderText("岗位");
            basicInfo.Cell(1, 4).Select();
            SetCellHeaderText("工作方式");
            basicInfo.Cell(1, 5).Select();
            SetCellHeaderText("职业病危害因素");
            basicInfo.Cell(1, 6).Select();
            SetCellHeaderText("总结出人数（人）");
            basicInfo.Cell(1, 7).Select();
            SetCellHeaderText("日接触时间（h）");
            basicInfo.Cell(1, 8).Select();
            SetCellHeaderText("班制");
            basicInfo.Cell(1, 9).Select();
            SetCellHeaderText("周工作天数（d）");
            basicInfo.Cell(10, 8).Select();
            SetCellHeaderText(" ");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
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
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.1.2 生产环境中存在的有害因素：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      拟扩建项目生产区的劳动者均在室内工作，夏季和冬季分别采取降温和采暖措施，受到自然环境因素中极端气象条件的影响较小。该工程总体布局和设备布局较为合理，厂区内各建筑采取自然通风或机械通风或两者相结合等通风方式，在采光不足的工作场所辅以人工照明，因此对工人健康影响较小。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.1.3劳动过程中存在的有害因素：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      拟扩建项目自动化程度较高，设备的运行维护管理采用控制监控和现场巡视相结合的方式，在设备正常运行状态下，劳动强度不大，体力劳动强度为   人生物紊乱、内分泌失调等。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      拟扩建项目部分作业人员工作是从事视屏监视与远程操作，由于长时间进行视屏作业，可能导致视力紧张。此外，如果控制台、显示装置及座椅的设计不符合人机工效学原理，还可能使工人发生下背痛、腕管综合症、颈肩腕综合症等工作相关疾病。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.1.4 维检修状态下产生的职业病危害因素：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      拟扩建项目计划一年进行一次检维修，主要是对工艺进行优化和设备进行维护。在检维修期间，如设备吹扫部完全，检维修人员可能会接触到装置的各种有害物质，如该项目存在的汽油、非甲烷总烃等；检维修期间设备焊接处要使用射线探伤机探伤，产生电离辐射（如X射线，r射线等）；检维修期间要进行电焊作业，电焊作业过程中可能产生一氧化碳、二氧化氮、臭氧、锰及其化合物、电焊烟尘、噪声和紫外辐射（电焊弧光）等有害因素。在装置检修停工、开工前的蒸汽吹扫会产生噪声。另外本项目进行大修时检修工人可能要进入空间狭小的设备或建筑物内进行检维修，因此存在着密闭空间作业，如果不采取防护措施，可能会因氧含量低而导致缺氧或窒息等不良健康影响。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      拟扩建项目检维修计划统一上报公司，由公司统一进行调配安排。建议公司对该项目进行检维修时委托有能力的单位进行。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.1.6 建设施工过程职业病危害分析：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      建设施工活动中劳动者接触的主要职业病危害因素包括：粉尘（矽尘、水泥粉尘、电焊烟尘、金属粉尘、其他粉尘、玻璃棉粉尘等），化学物质（锰及其化合物、二氧化氮、一氧化碳、臭氧、甲苯、二甲苯、汽油、石油沥青烟等），物理因素（噪声、高温、电焊弧光、振动等）。具体情况见表2-2。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("表2-2  建设施工活动中劳动者接触的主要有害因素");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo1 = m_doc.Tables.Add(m_wordApp.Selection.Range, 16, 4);
            basicInfo1.Borders.Enable = 1;
            basicInfo1.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo1.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo1.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo1.Columns[1].Width = 60;
            basicInfo1.Columns[2].Width = 100;
            basicInfo1.Columns[3].Width = 100;
            basicInfo1.Columns[4].Width = 150;

            basicInfo1.Rows[1].Height = 20;      
            basicInfo1.Cell(1, 1).Select();
            SetCellHeaderText("类型");
            basicInfo1.Cell(1, 2).Merge(basicInfo1.Cell(1, 3));
            basicInfo1.Cell(1, 2).Select();
            SetCellHeaderText("岗位/工种");
            basicInfo1.Cell(1, 3).Select();
            SetCellHeaderText("主要职业病危害因素");
            basicInfo1.Cell(16, 4).Select();

            //basicInfo1.Rows[2].Height = 20;
            //basicInfo1.Cell(2, 1).Select();
            //SetCellHeaderText("土木");
            //basicInfo1.Cell(2, 2).Select();
            //SetCellHeaderText("土石方施工人员");
            //basicInfo1.Cell(2, 3).Select();
            //SetCellHeaderText("挖掘机，铲运");
            //basicInfo1.Cell(2, 4).Select();
            //SetCellHeaderText("噪声、粉尘、高温、全身振动");

            //basicInfo1.Rows[3].Height = 20;
            //basicInfo1.Cell(3, 1).Select();
            //SetCellHeaderText("类型");
            //basicInfo1.Cell(3, 2).Merge(basicInfo1.Cell(3, 3));
            //basicInfo1.Cell(3, 2).Select();
            //SetCellHeaderText("岗位/工种");
            //basicInfo1.Cell(3, 3).Select();
            //SetCellHeaderText("主要职业病危害因素");

            //basicInfo1.Rows[4].Height = 20;
            //basicInfo1.Cell(4, 1).Merge(basicInfo1.Cell(13, 1));
            //basicInfo1.Cell(4, 1).Select();
            //SetCellHeaderText("工程");
            //basicInfo1.Cell(4, 3).Select();
            //SetCellHeaderText("机驾驶员");

            //basicInfo1.Rows[5].Height = 20;
            //basicInfo1.Cell(5, 2).Merge(basicInfo1.Cell(6, 2));
            //basicInfo1.Cell(5, 2).Select();
            //SetCellHeaderText("混凝土配制及制品加工人员");
            //basicInfo1.Cell(5, 3).Select();
            //SetCellHeaderText("混凝土工");
            //basicInfo1.Cell(5, 4).Select();
            //SetCellHeaderText("噪声、局部振动、高温");

            //basicInfo1.Rows[6].Height = 20;
            //basicInfo1.Cell(6, 3).Select();
            //SetCellHeaderText("混凝土搅拌机械操作工");
            //basicInfo1.Cell(6, 4).Select();
            //SetCellHeaderText("噪声、高温、粉尘、沥青烟");

            //basicInfo1.Rows[7].Height = 20;
            //basicInfo1.Cell(6, 2).Select();
            //SetCellHeaderText("钢筋加工人员");
            //basicInfo1.Cell(7, 3).Select();
            //SetCellHeaderText("钢筋工");
            //basicInfo1.Cell(7, 4).Select();
            //SetCellHeaderText("噪声、金属粉尘、高温、高处作业");

            //basicInfo1.Rows[8].Height = 20;
            //basicInfo1.Cell(8, 2).Select();
            //SetCellHeaderText("施工架子搭设人员");
            //basicInfo1.Cell(8, 3).Select();
            //SetCellHeaderText("架子工");
            //basicInfo1.Cell(8, 4).Select();
            //SetCellHeaderText("高温、高处作业");

            //basicInfo1.Rows[9].Height = 20;
            //basicInfo1.Cell(9, 2).Select();
            //SetCellHeaderText("工程防水人员");
            //basicInfo1.Cell(9, 3).Select();
            //SetCellHeaderText("防水工");
            //basicInfo1.Cell(9, 4).Select();
            //SetCellHeaderText("高温、沥青烟、煤焦油、甲苯、二甲苯、汽油等有机溶剂、保温材料");

            //basicInfo1.Rows[10].Height = 20;
            //basicInfo1.Cell(10, 2).Select();
            //SetCellHeaderText("工程防水人员");
            //basicInfo1.Cell(10, 3).Select();
            //SetCellHeaderText("防渗墙工");
            //basicInfo1.Cell(10, 4).Select();
            //SetCellHeaderText("噪声、高温、局部振动");


            //basicInfo1.Rows[11].Height = 20;
            //basicInfo1.Cell(10, 2).Merge(basicInfo1.Cell(12, 2));
            //basicInfo1.Cell(10, 2).Select();
            //SetCellHeaderText("其他");
            //basicInfo1.Cell(11, 3).Select();
            //SetCellHeaderText("外墙保温工");
            //basicInfo1.Cell(11, 4).Select();
            //SetCellHeaderText("高处作业、玻璃棉粉尘");

            //basicInfo1.Rows[12].Height = 20;
            //basicInfo1.Cell(12, 3).Select();
            //SetCellHeaderText("电焊工");
            //basicInfo1.Cell(12, 4).Select();
            //SetCellHeaderText("电焊烟尘、锰及其化合物、一氧化碳、二氧化氮、臭氧、电焊弧光、高温、高处作业");

            //basicInfo1.Rows[13].Height = 20;
            //basicInfo1.Cell(13, 3).Select();
            //SetCellHeaderText("起重机操作工");
            //basicInfo1.Cell(13, 4).Select();
            //SetCellHeaderText("噪声、高温");

            //basicInfo1.Rows[14].Height = 20;
            //basicInfo1.Cell(5, 1).Merge(basicInfo1.Cell(6, 1));
            //basicInfo1.Cell(5, 1).Select();
            //SetCellHeaderText("装修工程");
            //basicInfo1.Cell(11, 2).Merge(basicInfo1.Cell(12, 2));
            //basicInfo1.Cell(11, 2).Select();
            //SetCellHeaderText("装饰装修人员");
            //basicInfo1.Cell(14, 3).Select();
            //SetCellHeaderText("金属门窗工");
            //basicInfo1.Cell(14, 4).Select();
            //SetCellHeaderText("噪声、金属粉尘、高温、高处作业");

            //basicInfo1.Rows[15].Height = 20;
            //basicInfo1.Cell(15, 3).Select();
            //SetCellHeaderText("室内成套设施装饰");
            //basicInfo1.Cell(15, 4).Select();
            //SetCellHeaderText("噪声、高温");

            //basicInfo1.Rows[16].Height = 20;
            //basicInfo1.Cell(6, 1).Select();
            //SetCellHeaderText("设备安装及调试过程");
            //basicInfo1.Cell(12, 2).Select();
            //SetCellHeaderText("设备安装及调试人员");
            //basicInfo1.Cell(16, 3).Select();
            //SetCellHeaderText("设备安装及调试");
            //basicInfo1.Cell(16, 3).Select();
            //SetCellHeaderText("噪声、工频电场、苯、甲苯、二甲苯、电焊烟尘、紫外辐射、锰及其无机化合物、一氧化碳、氮氧化物等。");

            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
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
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.2 类比检测结果：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      本报告中职业病危害因素检测数据来源于我公司2014年9月至2015年10月对该B工厂一期控制效果评价的检测结果。检测时类比装置正常生产。检测数据见表2-3至表2-6。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("表2-3  主要岗位非甲烷总烃检测结果");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo2 = m_doc.Tables.Add(m_wordApp.Selection.Range, 13,11);
            basicInfo2.Borders.Enable = 1;
            basicInfo2.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo2.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo2.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo2.Columns[1].Width = 40;
            basicInfo2.Columns[2].Width = 40;
            basicInfo2.Columns[3].Width = 40;
            basicInfo2.Columns[4].Width = 60;
            basicInfo2.Columns[5].Width = 60;
            basicInfo2.Columns[6].Width = 40;
            basicInfo2.Columns[7].Width = 40;
            basicInfo2.Columns[8].Width = 40;
            basicInfo2.Columns[9].Width = 60;
            basicInfo2.Columns[10].Width = 40;
            basicInfo2.Columns[11].Width = 30;

            basicInfo2.Rows[1].Height = 20;
            basicInfo2.Cell(1, 1).Merge(basicInfo2.Cell(2, 1));
            basicInfo2.Cell(1, 1).Select();
            SetCellHeaderText("评价单元");
            basicInfo2.Cell(1, 2).Merge(basicInfo2.Cell(2, 2));
            basicInfo2.Cell(1, 2).Select();
            SetCellHeaderText("评价子单元");
            basicInfo2.Cell(1, 3).Merge(basicInfo2.Cell(2, 3));
            basicInfo2.Cell(1, 3).Select();
            SetCellHeaderText("岗位");
            basicInfo2.Cell(1, 4).Merge(basicInfo2.Cell(2, 4));
            basicInfo2.Cell(1, 4).Select();
            SetCellHeaderText("采样地点");
            basicInfo2.Cell(1, 5).Merge(basicInfo2.Cell(2, 5));
            basicInfo2.Cell(1, 5).Select();
            SetCellHeaderText("接触时间（h/d）");
            basicInfo2.Cell(1, 6).Merge(basicInfo2.Cell(1, 8));
            basicInfo2.Cell(1, 6).Select();
            SetCellHeaderText("检测结果（mg/m3)");
            basicInfo2.Cell(1, 7).Merge(basicInfo2.Cell(1, 8));
            basicInfo2.Cell(1, 7).Select();
            SetCellHeaderText("职业接触限值（mg/m3）");
            basicInfo2.Cell(1, 8).Merge(basicInfo2.Cell(2, 11));
            basicInfo2.Cell(1, 8).Select();
            SetCellHeaderText("结果判定");

            basicInfo2.Cell(2,6).Select();
            SetCellHeaderText("CSTEL");
            basicInfo2.Cell(2,7).Select();
            SetCellHeaderText("CTWA");
            basicInfo2.Cell(2,8).Select();
            SetCellHeaderText("超限倍数");
            basicInfo2.Cell(2, 9).Select();
            SetCellHeaderText("PC-TWA");
            basicInfo2.Cell(2, 10).Select();
            SetCellHeaderText("最大超限倍");

            //basicInfo2.Cell(2, 1).Merge(basicInfo2.Cell(10, 1));
            //basicInfo2.Cell(2, 2).Merge(basicInfo2.Cell(10, 2));
            //basicInfo2.Cell(2, 3).Merge(basicInfo2.Cell(4, 3));
            //basicInfo2.Cell(3, 9).Merge(basicInfo2.Cell(13, 9));
            //basicInfo2.Cell(3, 9).Select();
            //SetCellHeaderText("300");
            //basicInfo2.Cell(3, 10).Merge(basicInfo2.Cell(13, 10));
            //basicInfo2.Cell(3, 10).Select();
            //SetCellHeaderText("1.5");
            //basicInfo2.Cell(13, 10).Select();
            //SetCellHeaderText(" ");

            //basicInfo2.Rows[6].Height = 20;
            //basicInfo2.Cell(3, 3).Merge(basicInfo2.Cell(5, 3));

            //basicInfo2.Rows[9].Height = 20;
            //basicInfo2.Cell(4, 3).Merge(basicInfo2.Cell(6, 3));

            //basicInfo2.Rows[12].Height = 20;
            //basicInfo2.Cell(3, 1).Merge(basicInfo2.Cell(4, 1));
            //basicInfo2.Cell(3, 2).Merge(basicInfo2.Cell(4, 2));

            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
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
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Size = 10;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("注：由于目前国内没有非甲烷总烃的职业接触限值，故采用溶剂汽油的职业接触限值对非甲烷总烃检测结果进行评价。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("\r\n");
            m_wordApp.Selection.TypeText("\r\n");

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("表2-4  主要岗位异丙醇检测结果");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo3 = m_doc.Tables.Add(m_wordApp.Selection.Range, 3, 10);
            basicInfo3.Borders.Enable = 1;
            basicInfo3.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo3.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo3.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo3.Columns[1].Width = 40;
            basicInfo3.Columns[2].Width = 40;
            basicInfo3.Columns[3].Width = 40;
            basicInfo3.Columns[4].Width = 60;
            basicInfo3.Columns[5].Width = 40;
            basicInfo3.Columns[6].Width = 40;
            basicInfo3.Columns[7].Width = 40;
            basicInfo3.Columns[8].Width = 40;
            basicInfo3.Columns[9].Width = 40;
            basicInfo3.Columns[10].Width = 40;

            basicInfo3.Rows[1].Height = 30;
            basicInfo3.Cell(1, 1).Merge(basicInfo3.Cell(2, 1));
            basicInfo3.Cell(1, 1).Select();
            SetCellHeaderText("评价单元");
            basicInfo3.Cell(1, 2).Merge(basicInfo3.Cell(2, 2));
            basicInfo3.Cell(1, 2).Select();
            SetCellHeaderText("评价子单元");
            basicInfo3.Cell(1, 3).Merge(basicInfo3.Cell(2, 3));
            basicInfo3.Cell(1, 3).Select();
            SetCellHeaderText("岗位");
            basicInfo3.Cell(1, 4).Merge(basicInfo3.Cell(2, 4));
            basicInfo3.Cell(1, 4).Select();
            SetCellHeaderText("采样地点");
            basicInfo3.Cell(1, 5).Merge(basicInfo3.Cell(2, 5));
            basicInfo3.Cell(1, 5).Select();
            SetCellHeaderText("接触时间（h/d）");
            basicInfo3.Cell(1, 6).Merge(basicInfo3.Cell(1, 7));
            basicInfo3.Cell(1, 6).Select();
            SetCellHeaderText("检测结果（mg/m3)");
            basicInfo3.Cell(1, 7).Merge(basicInfo3.Cell(1, 8));
            basicInfo3.Cell(1, 7).Select();
            SetCellHeaderText("职业接触限值（mg/m3）");
            basicInfo3.Cell(1, 8).Merge(basicInfo3.Cell(2, 10));
            basicInfo3.Cell(1, 8).Select();
            SetCellHeaderText("结果判定");

            basicInfo3.Cell(2, 6).Select();
            SetCellHeaderText("CSTEL");
            basicInfo3.Cell(2, 7).Select();
            SetCellHeaderText("CTWA");
            basicInfo3.Cell(2, 8).Select();
            SetCellHeaderText("PC-STEL");
            basicInfo3.Cell(2, 9).Select();
            SetCellHeaderText("PC-TWA");

            basicInfo3.Cell(3, 8).Select();
            SetCellHeaderText("700");
            basicInfo3.Cell(3, 9).Select();
            SetCellHeaderText("350");
            basicInfo3.Cell(3, 10).Select();
            SetCellHeaderText(" ");

            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            m_wordApp.Selection.TypeText("\r\n");
            m_wordApp.Selection.TypeText("\r\n");

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("表2-5  主要岗位噪声强度测量结果");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo4 = m_doc.Tables.Add(m_wordApp.Selection.Range, 14, 10);
            basicInfo4.Borders.Enable = 1;
            basicInfo4.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo4.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo4.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo4.Columns[1].Width = 40;
            basicInfo4.Columns[2].Width = 60;
            basicInfo4.Columns[3].Width = 40;
            basicInfo4.Columns[4].Width = 40;
            basicInfo4.Columns[5].Width = 40;
            basicInfo4.Columns[6].Width = 40;
            basicInfo4.Columns[7].Width = 60;
            basicInfo4.Columns[8].Width = 50;
            basicInfo4.Columns[9].Width = 50;

            basicInfo4.Rows[1].Height = 20;
            basicInfo4.Cell(1, 1).Select();
            SetCellHeaderText("评价单元");
            basicInfo4.Cell(1, 2).Select();
            SetCellHeaderText("评价子单元");
            basicInfo4.Cell(1, 3).Select();
            SetCellHeaderText("岗位");
            basicInfo4.Cell(1, 4).Select();
            SetCellHeaderText("测量地点");
            basicInfo4.Cell(1, 5).Select();
            SetCellHeaderText("接触时间（h/d）");
            basicInfo4.Cell(1, 6).Select();
            SetCellHeaderText("噪声强度");
            basicInfo4.Cell(1, 7).Select();
            SetCellHeaderText("检测结果（mg/m3)");
            basicInfo4.Cell(1, 8).Select();
            SetCellHeaderText("LEX,8h");
            basicInfo4.Cell(1, 9).Select();
            SetCellHeaderText("职业接触限值（mg/m3）");
            basicInfo4.Cell(1, 10).Select();
            SetCellHeaderText("结果判定");

            basicInfo4.Cell(2, 1).Merge(basicInfo4.Cell(4, 1));
            basicInfo4.Cell(2, 2).Merge(basicInfo4.Cell(4, 2));
            basicInfo4.Cell(2, 9).Merge(basicInfo4.Cell(14, 9));
            basicInfo4.Cell(2, 9).Select();
            SetCellHeaderText("85");
            basicInfo4.Cell(14, 10).Select();
            SetCellHeaderText(" ");

            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            m_wordApp.Selection.TypeText("\r\n");
            m_wordApp.Selection.TypeText("\r\n");

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("表2-6  噪声工作场所噪声强度测定结果及频谱分析");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo5 = m_doc.Tables.Add(m_wordApp.Selection.Range, 11, 15);
            basicInfo5.Borders.Enable = 1;
            basicInfo5.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo5.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo5.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo5.Columns[1].Width = 50;
            basicInfo5.Columns[2].Width = 60;
            basicInfo5.Columns[3].Width = 80;
            basicInfo5.Columns[4].Width = 40;
            basicInfo5.Columns[5].Width = 30;
            basicInfo5.Columns[6].Width = 30;
            basicInfo5.Columns[7].Width = 30;
            basicInfo5.Columns[8].Width = 30;
            basicInfo5.Columns[9].Width = 30;
            basicInfo5.Columns[10].Width = 30;
            basicInfo5.Columns[11].Width = 30;
            basicInfo5.Columns[12].Width = 30;
            basicInfo5.Columns[13].Width = 30;
            basicInfo5.Columns[14].Width = 30;
            basicInfo5.Columns[15].Width = 30;

            basicInfo5.Rows[1].Height = 20;
            basicInfo5.Cell(1, 1).Merge(basicInfo5.Cell(2, 1));
            basicInfo5.Cell(1, 1).Select();
            SetCellHeaderText("评价单元");
            basicInfo5.Cell(1, 2).Merge(basicInfo5.Cell(2, 2));
            basicInfo5.Cell(1, 2).Select();
            SetCellHeaderText("评价子单元");
            basicInfo5.Cell(1, 3).Merge(basicInfo5.Cell(2, 3));
            basicInfo5.Cell(1, 3).Select();
            SetCellHeaderText("测定地点位");
            basicInfo5.Cell(1, 4).Merge(basicInfo5.Cell(2, 4));
            basicInfo5.Cell(1, 4).Select();
            SetCellHeaderText("噪声强度");
            basicInfo5.Cell(1, 5).Merge(basicInfo5.Cell(1,15));
            basicInfo5.Cell(1, 5).Select();
            SetCellHeaderText("频    谱 （Hz）");

            basicInfo5.Cell(2, 5).Select();
            SetCellHeaderText("16");
            basicInfo5.Cell(2, 6).Select();
            SetCellHeaderText("31.5");
            basicInfo5.Cell(2, 7).Select();
            SetCellHeaderText("63.0");
            basicInfo5.Cell(2, 8).Select();
            SetCellHeaderText("125");
            basicInfo5.Cell(2, 9).Select();
            SetCellHeaderText("250");
            basicInfo5.Cell(2, 10).Select();
            SetCellHeaderText("500");
            basicInfo5.Cell(2, 11).Select();
            SetCellHeaderText("1K");
            basicInfo5.Cell(2, 12).Select();
            SetCellHeaderText("2K");
            basicInfo5.Cell(2, 13).Select();
            SetCellHeaderText("4K");
            basicInfo5.Cell(2, 14).Select();
            SetCellHeaderText("8K");
            basicInfo5.Cell(2, 15).Select();
            SetCellHeaderText("16K");
            basicInfo5.Cell(11, 15).Select();
            SetCellHeaderText(" ");

            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.3职业病危害因素预期接触水平：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      通过对拟扩建项目工程分析，结合类比调查，根据评价单元，对该项目可能产生和存在的职业病危害因素进行如下分析：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.3.1化学物质预期接触水平：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      本项目产生化学有害物质的岗位如下：水管安装过程中会使用到异丙醇，工人工作过程中会接触到异丙醇职业病危害因素。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      通过类比检测，各  均符合《工作场所有害因素职业接触限值 第1部分：化学有害因素》的要求。类比企业车间为全封闭厂房，采用机械通风方式进行厂房通风换气，车间共设    。拟扩建项目各生产线设置在类比项目的厂房内，采取与类比项目相同的职业病防护设施。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      根据类比检测结果与职业病防护设施设置情况，预测本项目各SPC操作位接触的非甲烷总烃浓度、水管安装工异丙醇浓度符合职业接触限值的要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.3.2 物理因素预期接触水平：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      本项目主要的物理因素职业病危害因素以噪声为主，主要来源于生产过程中设备运转产生的机械性噪声。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      通过类比检测机 ，对接噪工人听力影响较大。其他岗位的噪声强度测量结果均符合《工作场所有害因素职业接触限值 第2部分：物理因素》的要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      类比项目根据生产工艺布置生产设备，在设备选型方面优先选择了噪声较小的设备，并设有减震垫或减震基础。   岗位工作过程中均会用到铁锤对零件进行敲打，或使用到气枪进行吹扫，这些操作过程中产生的机械性噪声较大，导致接触的噪声超标。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      拟扩建项目生产过程中所使用的生产设备和工艺均与类比工程相同，根据类比检测结果，岗位接触的噪声强度不符合职业接触限值的要求，其他各操作工接触的噪声符合职业接触限值的要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      根据类比检测结果，本项目各岗位主要职业病危害因素接触水平预测见表2-7。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("表2-7  各岗位职业病危害因素预期接触水平");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo6 = m_doc.Tables.Add(m_wordApp.Selection.Range, 14, 7);
            basicInfo6.Borders.Enable = 1;
            basicInfo6.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo6.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo6.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo6.Columns[1].Width = 50;
            basicInfo6.Columns[2].Width = 50;
            basicInfo6.Columns[3].Width = 90;
            basicInfo6.Columns[4].Width = 110;
            basicInfo6.Columns[5].Width = 50;
            basicInfo6.Columns[6].Width = 50;
            basicInfo6.Columns[7].Width = 70;

            basicInfo6.Rows[1].Height = 20;
            basicInfo6.Cell(1, 1).Merge(basicInfo6.Cell(2, 1));
            basicInfo6.Cell(1, 1).Select();
            SetCellHeaderText("评价单元");
            basicInfo6.Cell(1, 2).Merge(basicInfo6.Cell(2, 2));
            basicInfo6.Cell(1, 2).Select();
            SetCellHeaderText("工作岗位");
            basicInfo6.Cell(1, 3).Merge(basicInfo6.Cell(2, 3));
            basicInfo6.Cell(1, 3).Select();
            SetCellHeaderText("工作场所/生产设备");
            basicInfo6.Cell(1, 4).Merge(basicInfo6.Cell(2, 4));
            basicInfo6.Cell(1, 4).Select();
            SetCellHeaderText("职业病危害因素");
            basicInfo6.Cell(1, 5).Merge(basicInfo6.Cell(1, 7));
            basicInfo6.Cell(1, 5).Select();
            SetCellHeaderText("岗位预期接触水平（浓度或强度水平）");

            basicInfo6.Cell(2, 5).Select();
            SetCellHeaderText("粉尘（mg/m3）");
            basicInfo6.Cell(2, 6).Select();
            SetCellHeaderText("化学物质（mg/m3）");
            basicInfo6.Cell(2, 7).Select();
            SetCellHeaderText("物理因素（dB(A)）");
            basicInfo6.Cell(14, 7).Select();
            SetCellHeaderText(" ");

            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("从表2-7中可看出本项目评价单元中，预测本项目岗位接触的噪声强度预期接触水平不符合职业接触限值的要求，其他岗位接触的职业病危害因素预期接触水平符合国家职业接触限值的要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.4 职业病危害类别分析：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      本项目生产工艺过程中的产生或存在的主要职业病危害因素包括： ");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      依据《建设项目职业病危害风险分类管理目录》（2012年版）的规定，将“汽车制造业”的风险等级分类为“  ”，结合以上分析，经综合判断，将拟扩建项目分类为职业病危害 的建设项目。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.2职业病防护设施评价：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.2.1防毒措施评价：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      类比项目。类比现场检测结果显示：类比项目各岗位接触毒物浓度均未超过《工作场所有害因素职业接触限值第1部分：化学有害因素》的规定。拟扩建项目与类比项目采取相同的通风排毒设施，能满足职业病危害防护的需要。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.2.2物理因素防护措施评价：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      类比项目车间振动较大的设备。拟扩建项目优先选取了先进的设备，并拟采取与类比项目相同的隔声降噪设施，为操作人员配戴防护耳塞，能有效的降低噪声对人体健康的危害。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.3个体防护用品评价：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      拟扩建项目申请报告中未涉及，但拟扩建项目执行北京A汽车有限公司现有的管理制度，应能符合《中华人民共和国职业病防治法》、《个体防护装备选用规范》等要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.4应急救援措施评价：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      拟扩建项目申请报告中未涉及的设置要求，拟实行与类比项目相同的应急救援措施，应能符合《工业企业设计卫生标准》的要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.5职业卫生管理措施评价：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      拟扩建项目申请报告中未说明职业卫生防治专项经费的具体投入情况，在初步设计时应补充。");
            m_wordApp.Selection.TypeParagraph();
        }
        private void WriteTotalLayout()
        {
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("3 综合性评价");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("3.1 总体布局评价");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      拟扩建项目总平面布置功能分区明确，符合《工业企业设计卫生标准》的要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("3.2 生产工艺及设备布局评价");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      拟扩建项目拟采用先进的生产设备，同类设备中拟选用性能好、运行稳定、产生噪声强度较小的设备，设备自动化程度较高，减少了对作业环境的影响，能有效的控制生产过程中职业病危害因素的产生，减少劳动者接触职业病危害因素的机会，从源头上控制了职业病的发生。因此，本项目生产工艺设备较为先进。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("3.3 建筑卫生学评价");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("3.3.1 采光、照明");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      拟扩建项目在");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      拟建项目厂房一般照明灯具采用一体式配（深）照型工厂灯，光源为金属卤化物灯，工位局部照明灯具采用线槽式荧光灯，在生产车间、办公室生活间拟设置应急照明和疏散出口指示，但无具体照度设计内容。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("3.3.2 采暖、通风及空气调节");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      拟扩建项目 ，拟采取的采暖措施符合《工业企业设计卫生标准》要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      拟扩建项目申请报告中提及联合厂房设有，拟采取的通风及空气调节措施符合《工业企业设计卫生标准》要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("3.4辅助用室评价");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      拟扩建项目申请报告中未对辅助用室设计情况进行描述， 、设置位置和室内设置等均符合《工业企业设计卫生标准》对卫生辅助用室的要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("3.5职业卫生管理评价");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      拟扩建项目申请报告中未涉及 ，应能符合《中华人民共和国职业病防治法》、《工作场所职业卫生监督管理规定》等要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("3.6职业健康监护评价");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("        拟扩建项目申请报告中未提及职业健康监护内容，经职业卫生学调查，本项目的职业健康监护措施依托    公司现有制度，该公司人力资源部按照从事有毒有害作业人员名单、就业前离岗人员名单及在岗人员的变化情况，制定年度职业健康体检计划，委托具有相应资质的职业健康检查机构进行体检，职业健康监护的体检周期、体检项目、禁忌证以及职业病诊断、治疗等按国家有关标准执行，并且人力资源部建立从事有毒有害作业人员职业健康检查档案和职业病人档案。类比为部分接触职业病危害的人员进行了上岗前、在岗期间或离岗时的职业健康检查，但体检人数和体检项目不全。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("3.7职业卫生专项投资评价");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      拟扩建项目申请报告中未说明职业卫生防治专项经费的具体投入情况，在初步设计时应补充。");
            m_wordApp.Selection.TypeParagraph();
        }
        private void WriteDeviceLayout()
        {
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("4 职业病防护措施及建议");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("4.1 控制职业病危害的补充措施");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("4.1.1 职业病危害防护设施");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      拟扩建项目在初步设计中，应按《工业企业设计卫生标准》、《采暖通风与空气调节设计规范》的相关要求对生产过程中可能产生或存在的职业病危害防护设施进行合理设计。具体内容如下：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      根据《工业企业设计卫生标准》中的规定，拟扩建项目各车间、辅助用室采暖应符合表4-1的要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("表4-1  冬季采暖温度（干球温度）");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo = m_doc.Tables.Add(m_wordApp.Selection.Range, 5, 3);
            basicInfo.Borders.Enable = 1;
            basicInfo.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Columns[1].Width = 150;
            basicInfo.Columns[2].Width = 80;
            basicInfo.Columns[2].Width = 80;

            basicInfo.Rows[1].Height = 20;
            basicInfo.Cell(1, 1).Select();
            SetCellHeaderText("场所");
            basicInfo.Cell(1, 2).Select();
            SetCellHeaderText("温度（℃）");
            basicInfo.Cell(1, 3).Select();
            SetCellHeaderText("湿度（％）");

            basicInfo.Rows[2].Height = 20;
            basicInfo.Cell(2, 1).Select();
            SetCellHeaderText("各生产车间");
            basicInfo.Cell(2, 2).Select();
            SetCellHeaderText("≥18");
            basicInfo.Cell(2, 3).Select();
            SetCellHeaderText("70±10");

            basicInfo.Rows[3].Height = 20;
            basicInfo.Cell(3, 1).Select();
            SetCellHeaderText("办公室、休息室、就餐场所");
            basicInfo.Cell(3, 2).Select();
            SetCellHeaderText("≥18");
            basicInfo.Cell(3, 3).Select();
            SetCellHeaderText("——");

            basicInfo.Rows[4].Height = 20;
            basicInfo.Cell(4, 1).Select();
            SetCellHeaderText("浴室、更衣室、妇女卫生室");
            basicInfo.Cell(4, 2).Select();
            SetCellHeaderText("≥25");
            basicInfo.Cell(4, 3).Select();
            SetCellHeaderText("——");

            basicInfo.Rows[5].Height = 20;
            basicInfo.Cell(5, 1).Select();
            SetCellHeaderText("厕所、盥洗室");
            basicInfo.Cell(5, 2).Select();
            SetCellHeaderText("≥14");
            basicInfo.Cell(5, 3).Select();
            SetCellHeaderText("——");

            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("4.2个体防护");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      初步设计时应按劳动防护用品配备标准、劳动防护用品选用规则等国家有关标准的要求，结合工人接触职业病危害因素的实际情况，充分考虑个人职业病防护用品的设计。如拟扩建项目   ，企业应为以上岗位的作业工人配备性能良好的不同类型的防噪声耳塞或耳罩，在正常生产条件下如工人能够长期坚持佩戴密封性能良好的防噪声耳塞，可减少噪声对作业工人听力的损伤。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("4.3职业健康监护");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("4.4应急救援");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("4.5 职业卫生管理");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      初拟扩建项目应每年制订相应的计划，列出各项职业卫生经费的预算，从而为防治职业病提供资金保障。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("4.6 其他补充措施");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("4.7 建设施工过程职业卫生管理措施");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      施工单位应按照《职业病防治法》的要求，委托具有资质的职业卫生技术服务机构对建设期施工活动中产生或存在的职业病危害因素进行检测，对接触职业病危害因素的工人开展职业健康监护，设置职业病防护设施，提供个人防护用品，配备应急救援设备设施，建立职业卫生管理制度等，切实加强建设期职业病防护工作。拟建项目施工结束时施工方要向项目单位提交施工过程职业病防治管理的总结报告，包括施工现场职业病危害因素检测结果、职业健康监护结果、劳动者职业病危害告知情况、个人职业病防护情况、所采取的职业病危害防护设施防护效果情况等方面的总结报告。");
            m_wordApp.Selection.TypeParagraph();
        }
        private void WriteBuildingHygiene()
        {
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("5 评价结论");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      通过对该项目所提供资料的分析，结合类比现场调查和检测，得出该项目职业病危害预评价结论如下：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （1）拟扩建项目在可行性论证阶段委托具有资质的职业卫生技术服务机构进行职业病危害预评价，符合《中华人民共和国职业病防治法》等法律、法规的要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （2）拟扩建项目生产区、辅助生产区、非生产区之间功能分区明确，总平面布置符合《工业企业设计卫生标准》的要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （3）拟扩建项目生产工艺成熟，机械化、自动化程度较高，符合《职业病防治法》及产业政策的要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （4）拟扩建项目对照明灯具进行了设计，但无具体照度设计内容。。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （5）拟扩建项目生产工艺过程中可能产生或存在的主要职业病危害因素包括：  不符合职业接触限值的要求，其他岗位接触的职业病危害因素预期接触水平符合国家职业接触限值的要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （6）拟扩建项目有针对化学毒物和噪声等职业病危害因素防护设施的设计，符合《工业企业设计卫生标准》的要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （7）拟扩建项目执行北京   公司现有的管理制度，应能符合《中华人民共和国职业病防治法》、《个体防护装备选用规范》等要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （8）拟扩建项目依托   现有的辅助用室，辅助用室设计和数量方面符合《工业企业设计卫生标准》的要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （9）拟扩建项目沿用北京A汽车有限公司现有的职业卫生管理制度，应能符合《中华人民共和国职业病防治法》、《工作场所职业卫生监督管理规定》等要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      根据《国民经济行业分类》，拟建项目属于汽车制造业”，安监部门发布的");
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("《建设项目职业病危害风险分类管理目录》（2012年版）将“汽车制造业”的风险等级分类为“ ”，结合以上分析，经综合判断，将本项目分类为职业病危害 的建设项目。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      综上所述，根据拟扩建项目申请报告等资料，拟扩建项目基本执行了我国职业病危害预防控制的有关规定。拟扩建项目在今后工程的设计和工程建设中，若能将项目申请报告的职业病防护设施和本评价报告中提出的补充措施建议予以落实，良好的执行北京A汽车有限公司现有的职业卫生管理制度，预计项目建成后，拟扩建项目中存在的职业病危害能够得到有效预防和控制，");
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("拟扩建项目能够满足国家对职业病防治方面的法律、法规、标准及规范的要求。");
            m_wordApp.Selection.TypeParagraph();
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

