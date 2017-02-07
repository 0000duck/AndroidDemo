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
    public class CreateEvaluationDoc
    {
        string Header = "合同编号：                                 DKL/QB-11-05                     职业病危害控制效果评价报告书 ";
        string Footer = "北京德康莱健康安全科技股份有限公司                                                    ";
        Word.Application m_wordApp;
        Word.Document m_doc;
        Word.Tools.CommonUtils m_utils;

        public CreateEvaluationDoc()
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
                //         m_doc.Paragraphs.Last.Range.InsertBreak();
                WriteContent();
                WriteEmployerBasicInfo();
                WriteTotalLayout();
                WriteDeviceLayout();
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
            string name = "职业病危害控制效果评价报告书" + DateTime.Now.ToFileTime();
            //      string documentFile = m_utils.File.Combine("c:\\", name, Word.Tools.DocumentFormat.Normal);
            string path = "d://DKLdownload";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string documentFile = m_utils.File.Combine(path, "职业病危害控制效果评价报告书", Word.Tools.DocumentFormat.Normal);
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
            Word.Table basicInfo = m_doc.Tables.Add(m_wordApp.Selection.Range, 4, 2);
            basicInfo.Rows.Alignment = WdRowAlignment.wdAlignRowRight;
            basicInfo.Borders.Enable = 1;
            basicInfo.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Columns[1].Width = 60;
            basicInfo.Columns[1].Width = 60;

            basicInfo.Rows[1].Height = 20;          ///填入数据  ，报告编号 放第一个格子里面  值放第二个格子里面
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

            for (int i = 0; i < 4; ++i)
                m_wordApp.Selection.TypeText("\r\n");
            m_wordApp.Selection.Font.Color = WdColor.wdColorBlack;    ///颜色
            m_wordApp.Selection.Font.Bold = 5;     ///字体
            m_wordApp.Selection.Font.Size = 40;         ///字大小
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("职业病危害控制效果评价报告书");
            m_wordApp.Selection.TypeParagraph();


            m_wordApp.Selection.Font.Color = WdColor.wdColorBlack;
            m_wordApp.Selection.Font.Bold = 3;
            m_wordApp.Selection.Font.Size = 20;
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
            m_wordApp.Selection.TypeText("1.3 项目组成及工程内容：");
            for (int i = 0; i < 2; ++i)
                m_wordApp.Selection.TypeText("\r\n");

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("1.4 总投资：");
            m_wordApp.Selection.TypeParagraph();
            for (int i = 0; i < 2; ++i)
                m_wordApp.Selection.TypeText("\r\n");

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("1.5 建设地点：");
            m_wordApp.Selection.TypeParagraph();
            for (int i = 0; i < 2; ++i)
                m_wordApp.Selection.TypeText("\r\n");

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("1.6 “三同时”执行情况：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      本项目在可行性研究阶段，北京  公司委托北京德康莱安全卫生技术发展有限公司（现已改名为北京德康莱健康安全科技股份有限公司）对项目进行了职业病危害预评价，《北京A汽车有限公司新建B工厂项目职业病危害预评价报告书》经过专家评审后准予该项目通过审核，并已经取得了“北京A汽车有限公司新建B工厂项目职业病危害预评价报告书的批复”（京安监许可审字[2014]1号）。根据职业卫生现场调查，2014年初企业编制了职业安全卫生专篇，但企业未能提供相关资料。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      针对本工程存在的职业病危害因素所采取的防护设施与主体工程同时设计、同时施工、同时投入生产和使用。建成后投入试运行，依据《中华人民共和国职业病防治法》有关规定，该项目正进行职业病危害控制效果评价。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("1.7 项目试运行情况：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      本项目于自试运行开始至今，运行稳定。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      该项目制定有职业病防治规章制度、操作规程；编制了职业病危害事故应急救援预案；投入试运行后，在工作场所设置了职业病危害警示标识；为接触职业病危害的作业人员配备了个体防护用品。目前该工程的职业病防护设施运行正常，工程投运后没有发生急性职业性中毒事故。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("1.8 施工期及设备安装调试期情况调查：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      该项目施工内容主要包括土建施工和设备安装两大部分。土建施工包括场地平整、道路、供水、供电、供气、通讯施工工程，为主体工程施工完成基础准备和施工保证；主体工程土建施工主要完成生产厂房、构筑物、辅助生产和生活建筑物的建设。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      该项目已于 年 月完结，施工单位和监理单位未提供施工过程职业病危害防治总结报告。该阶段职业病危害因素及防护设施、防护用品、职业卫生管理制度等企业未留存相关资料，无从调查。");
            m_wordApp.Selection.TypeParagraph();
        }
        //公司，日期
        private void WriteEmployerBasicInfo()
        {
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2 职业病危害评价：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1 职业病危害因素评价：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.1 生产工艺过程中的职业病危害因素及分布：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.1.1 正常生产过程中产生的职业病危害因素：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      该项目重点评价的职业病危害因素为： 。该项目重点评价的职业病危害因素的分布情况见表2-1。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("表2-1  该项目重点评价的职业病危害因素的分布情况");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo = m_doc.Tables.Add(m_wordApp.Selection.Range, 10, 5);
            basicInfo.Borders.Enable = 1;
            basicInfo.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Columns[1].Width = 40;
            basicInfo.Columns[2].Width = 60;
            basicInfo.Columns[3].Width = 120;
            basicInfo.Columns[4].Width = 160;

            basicInfo.Rows[1].Height = 20;
            basicInfo.Cell(1, 1).Merge(basicInfo.Cell(1, 2));
            basicInfo.Cell(1, 1).Select();
            SetCellHeaderText("评价单元");
            basicInfo.Cell(1, 2).Select();
            SetCellHeaderText("生产工艺过程");
            basicInfo.Cell(1, 3).Select();
            SetCellHeaderText("岗位");
            basicInfo.Cell(1, 4).Select();
            SetCellHeaderText("可能产生的职业病危害因素");

            basicInfo.Cell(10, 5).Select();
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
            m_wordApp.Selection.TypeText("2.1.1.2 维检修状态下产生的职业病危害因素：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      该项目计划一年 ");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      该项目检维修统一上报公司，由公司统一进行调配安排。建议公司对该项目进行检维修时委托有能力的单位进行。 ");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.1.3 事故应急状态下产生的职业病危害因素：");
            m_wordApp.Selection.TypeParagraph();
            for (int i = 0; i < 2; ++i)
                m_wordApp.Selection.TypeText("\r\n");

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.1.4 施工期和设备安装调试期职业病危害因素识别：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("   （1）建设施工过程中的职业病危害因素及其存在环节");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      建筑工程施工过程中产生的主要职业病危害因素：挖方工程、土方工程、填方过程中挖土机、推土机、凿岩机、钻孔机等设备产生的粉尘、噪声和振动；混凝土运输、储存和浇筑过程中混凝土运输罐车和浇筑泵车等设备产生的水泥粉尘和噪声；钢筋、铝合金切割产生金属粉尘、噪声和振动；焊缝探伤过程中产生的电离辐射、X射线、γ射线；保温过程、绝缘过程作业人员可接触到岩棉粉尘；油漆、防腐作业产生苯、甲苯、二甲苯、汽油等；涂料作业产生甲醛、苯、二甲苯等，建筑物防水工程作业产生石油沥青、甲苯、二甲苯等；建筑施工过程中存在电焊作业，可产生锰及化合物、电焊烟尘、氮氧化合物、一氧化碳、臭氧、紫外辐射等职业病危害因素。另外，排水管、排水沟地下管道、烟道等，以及其他通风不足的场所作业活动存在封闭空间作业；砌筑、门窗安装等作业存在高处作业。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("   （2）设备安装过程中存在的职业病危害因素及其分布");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      该项目安装工程主要包括：储罐吊装、厂内运输与安装、管道安装、厂内拖运及吊装等。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      安装工程施工过程中产生的主要职业病危害因素：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      各种吊装机械如起重机、龙门吊、升降机、塔吊、汽车吊运等运行过程中产生的生产性噪声和振动；钢筋、铝合金切割产生金属性粉尘、噪声和振动；安装工程施工过程中存在电焊作业，可产生锰及化合物、电焊烟尘、氮氧化合物、一氧化碳、臭氧、紫外辐射等职业病危害因素。");
            m_wordApp.Selection.TypeParagraph();


            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      作业人员在夏季露天作业过程中存在高温作业，冬天露天作业过程中存在低温。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.2 生产环境中的有害因素：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      本项目生产区的劳动者均在室内工作，夏季和冬季分别采取降温和采暖措施，受到自然环境因素中极端气象条件的影响较小。该工程总体布局和设备布局较为合理，厂区内各建筑采取自然通风或机械通风或两者相结合等通风方式，在采光不足的工作场所辅以人工照明，因此对工人健康影响较小。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.3 劳动过程中的职业病危害因素：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      该项目自动化程度较高，设备的运行维护管理采用控制监控和现场巡视相结合的方式，在设备正常运行状态下，劳动强度不大，体力劳动强度为");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      此外，如果控制台、显示装置及座椅的设计不符合人机工效学原理，还可能使工人发生下背痛、腕管综合症、颈肩腕综合症等工作相关疾病。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.5 职业病危害因素分析与评价：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      该工程生产工艺过程中产生或存在的化学有害因素有化学物质 。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      根据检测结果，该企业各岗位接触的纤维素粉尘、矽尘和化学物质的水平均符合《工作场所有害因素职业接触限值 第1部分：化学有害因素》的要求； 接触水平均符合《工作场所有害因素职业接触限值 第2部分：物理因素》的要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      根据噪声检测结果，对作业场所的超标的噪声进行职业病危害因素分级，结果见表2-2。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("表2-2  本项目超标岗位噪声作业危害程度分级");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo1 = m_doc.Tables.Add(m_wordApp.Selection.Range, 8, 7);
            basicInfo1.Borders.Enable = 1;
            basicInfo1.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo1.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo1.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo1.Columns[1].Width = 60;
            basicInfo1.Columns[2].Width = 60;
            basicInfo1.Columns[3].Width = 80;
            basicInfo1.Columns[4].Width = 100;
            basicInfo1.Columns[5].Width = 40;
            basicInfo1.Columns[6].Width = 40;
            basicInfo1.Columns[7].Width = 70;

            basicInfo1.Rows[1].Height = 20;
            basicInfo1.Cell(1, 1).Select();
            SetCellHeaderText("评价单元");
            basicInfo1.Cell(1, 2).Select();
            SetCellHeaderText("评价子单元");
            basicInfo1.Cell(1, 3).Select();
            SetCellHeaderText("岗位");
            basicInfo1.Cell(1, 4).Select();
            SetCellHeaderText("测量地点");
            basicInfo1.Cell(1, 5).Select();
            SetCellHeaderText("LEX,8h");
            basicInfo1.Cell(1, 6).Select();
            SetCellHeaderText("分级");
            basicInfo1.Cell(1, 7).Select();
            SetCellHeaderText("危害程度");
            basicInfo1.Cell(8, 7).Select();
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
            m_wordApp.Selection.TypeText("2.2职业病防护设施评价：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      职业病防护设施设置情况见表2-3。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("表2-3  职业病防护设施设置情况表");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo2 = m_doc.Tables.Add(m_wordApp.Selection.Range, 8, 4);
            basicInfo2.Borders.Enable = 1;
            basicInfo2.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo2.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo2.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo2.Columns[1].Width = 60;
            basicInfo2.Columns[2].Width = 150;
            basicInfo2.Columns[3].Width = 100;
            basicInfo2.Columns[4].Width = 100;

            basicInfo2.Rows[1].Height = 20;
            basicInfo2.Cell(1, 1).Select();
            SetCellHeaderText("防护种类");
            basicInfo2.Cell(1, 2).Select();
            SetCellHeaderText("防护措施");
            basicInfo2.Cell(1, 3).Select();
            SetCellHeaderText("设置地点");
            basicInfo2.Cell(1, 4).Select();
            SetCellHeaderText("符合性评价");
            basicInfo2.Cell(8, 4).Select();
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
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      本项目按照目前的生产工艺，安装使用了防尘、降噪措施，并落实了《北京A汽车有限公司新建B工厂项目职业病危害预评价报告书》中关于防护设施的建议（具体见2.10相关内容），经采取职业病防护设施或佩戴相应的防护用品后，本次职业病危害因素检测结果均符合GBZ 2.1、GBZ 2.2的职业接触限值要求，说明目前采取的防护措施是可行有效的。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.3 个人使用的职业病防护用品防护用品评价：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      经职业卫生现场调查，本项目 配备了相应的个人防护用品，相应的个人职业病危害因素防护用品配备情况见表2-4。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("表2-4  职业病防护设施设置情况表");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo3 = m_doc.Tables.Add(m_wordApp.Selection.Range, 8, 6);
            basicInfo3.Borders.Enable = 1;
            basicInfo3.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo3.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo3.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo3.Columns[1].Width = 40;
            basicInfo3.Columns[2].Width = 60;
            basicInfo3.Columns[3].Width = 80;
            basicInfo3.Columns[4].Width = 100;
            basicInfo3.Columns[5].Width = 100;
            basicInfo3.Columns[6].Width = 80;

            basicInfo3.Rows[1].Height = 20;
            basicInfo3.Cell(1, 1).Select();
            SetCellHeaderText("评价单元");
            basicInfo3.Cell(1, 2).Select();
            SetCellHeaderText("生产工艺过程");
            basicInfo3.Cell(1, 3).Select();
            SetCellHeaderText("产生的职业病危害因素");
            basicInfo3.Cell(1, 4).Select();
            SetCellHeaderText("配备的个人防护用品名称");
            basicInfo3.Cell(1, 5).Select();
            SetCellHeaderText("配备数量");
            basicInfo3.Cell(8, 6).Select();
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

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      由职业病危害因素现场检测结果可得，，根据《工业企业职工听力保护规范》第二十七条，该耳塞现场使用实际声衰减值为 ，以勒防护耳罩的噪声衰减值如表2-5所示。该企业接触噪声强度超标的劳动者在全工作日佩戴其防噪声耳塞后实际接触的噪声强度见表2-6。");
            m_wordApp.Selection.TypeParagraph();


            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("表2-5  以勒耳罩防噪声性能");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo4 = m_doc.Tables.Add(m_wordApp.Selection.Range, 3, 8);
            basicInfo4.Borders.Enable = 1;
            basicInfo4.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo4.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo4.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo4.Columns[1].Width = 100;
            basicInfo4.Columns[2].Width = 40;
            basicInfo4.Columns[3].Width = 60;
            basicInfo4.Columns[4].Width = 60;
            basicInfo4.Columns[5].Width = 60;
            basicInfo4.Columns[6].Width = 60;

            basicInfo4.Rows[1].Height = 20;
            basicInfo4.Cell(1, 1).Select();
            SetCellHeaderText("1/3倍频带中心频率HZ");

            basicInfo4.Rows[2].Height = 20;
            basicInfo4.Cell(2, 1).Select();
            SetCellHeaderText("实测声衰减量dB(A)");

            basicInfo4.Rows[3].Height = 20;
            basicInfo4.Cell(3, 1).Select();
            SetCellHeaderText("GB5893.2-86标准声衰减量dB(A)");

            basicInfo4.Cell(3, 8).Select();
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

            m_wordApp.Selection.TypeText("\r\n");

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("表2-6  佩戴耳塞后噪声强度接触情况              单位：dB(A)");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo5 = m_doc.Tables.Add(m_wordApp.Selection.Range, 10, 8);
            basicInfo5.Borders.Enable = 1;
            basicInfo5.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo5.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo5.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo5.Columns[1].Width = 60;
            basicInfo5.Columns[2].Width = 80;
            basicInfo5.Columns[3].Width = 80;
            basicInfo5.Columns[4].Width = 40;
            basicInfo5.Columns[5].Width = 40;
            basicInfo5.Columns[6].Width = 60;
            basicInfo5.Columns[7].Width = 60;
            basicInfo5.Columns[8].Width = 40;

            basicInfo5.Rows[1].Height = 20;
            basicInfo5.Cell(1, 1).Select();
            SetCellHeaderText("评价单元");
            basicInfo5.Cell(1, 2).Select();
            SetCellHeaderText("评价子单元");
            basicInfo5.Cell(1, 3).Select();
            SetCellHeaderText("测定地点");
            basicInfo5.Cell(1, 4).Select();
            SetCellHeaderText("噪声强度");
            basicInfo5.Cell(1, 5).Select();
            SetCellHeaderText("LEX,W");
            basicInfo5.Cell(1, 6).Select();
            SetCellHeaderText("实际接触噪声强度");
            basicInfo5.Cell(1, 7).Select();
            SetCellHeaderText("职业接触限值");
            basicInfo5.Cell(1, 8).Select();
            SetCellHeaderText("结果判定");

            basicInfo5.Rows[2].Height = 20;
            basicInfo5.Cell(2, 7).Merge(basicInfo5.Cell(10, 7));
            basicInfo5.Cell(2, 7).Select();
            SetCellHeaderText("85");

            basicInfo5.Cell(10, 8).Select();
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
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      该企业员工在全工作日佩戴该防噪声耳塞后噪声防护有效。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      依据《中华人民共和国职业病防治法》、《劳动防护用品监督管理规定》、《工作场所职业卫生监督管理规定》的相关要求，对建设项目个人防护用品情况列表检查，结果见表2-7。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("表2-7  个人职业病防护用品检查表");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo6 = m_doc.Tables.Add(m_wordApp.Selection.Range, 5, 5);
            basicInfo6.Borders.Enable = 1;
            basicInfo6.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo6.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo6.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo6.Columns[1].Width = 40;
            basicInfo6.Columns[2].Width = 120;
            basicInfo6.Columns[3].Width = 80;
            basicInfo6.Columns[4].Width = 100;
            basicInfo6.Columns[5].Width = 80;

            basicInfo6.Rows[1].Height = 20;
            basicInfo6.Cell(1, 1).Select();
            SetCellHeaderText("序号");
            basicInfo6.Cell(1, 2).Select();
            SetCellHeaderText("检查内容");
            basicInfo6.Cell(1, 3).Select();
            SetCellHeaderText("检查依据");
            basicInfo6.Cell(1, 4).Select();
            SetCellHeaderText("检查情况");
            basicInfo6.Cell(1, 5).Select();
            SetCellHeaderText("结论");

            basicInfo6.Rows[2].Height = 20;
            basicInfo6.Cell(2, 1).Select();
            SetCellHeaderText("1");
            basicInfo6.Cell(2, 2).Select();
            SetCellHeaderText("用人单位为劳动者提供个人使用的职业病防护用品。");
            basicInfo6.Cell(2, 3).Select();
            SetCellHeaderText("《中华人民共和国职业病防治法》第二十三条");

            basicInfo6.Rows[3].Height = 20;
            basicInfo6.Cell(3, 1).Select();
            SetCellHeaderText("2");
            basicInfo6.Cell(3, 2).Select();
            SetCellHeaderText("对个人使用的职业病防护用品用人单位应当进行经常性的维护、检修，定期检测其性能和效果，确保其处于正常状态。");
            basicInfo6.Cell(3, 3).Select();
            SetCellHeaderText("《中华人民共和国职业病防治法》第二十六条");

            basicInfo6.Rows[4].Height = 20;
            basicInfo6.Cell(4, 1).Select();
            SetCellHeaderText("3");
            basicInfo6.Cell(4, 2).Select();
            SetCellHeaderText("从业人员在作业过程中，应按照安全生产规章制度和劳动防护用品使用规则，正确佩戴和使用劳动防护用品；未按规定佩戴和使用劳动防护用品的，不得上岗作业。");
            basicInfo6.Cell(4, 3).Select();
            SetCellHeaderText("《劳动防护用品监督管理规定》第十九条");

            basicInfo6.Rows[5].Height = 20;
            basicInfo6.Cell(5, 1).Select();
            SetCellHeaderText("4");
            basicInfo6.Cell(5, 2).Select();
            SetCellHeaderText("用人单位应当为劳动者提供符合国家职业卫生标准的职业病防护用品，并督促、指导劳动者按照使用规则正确佩戴、使用，不得发放钱物替代发放职业病防护用品。");
            basicInfo6.Cell(5, 3).Select();
            SetCellHeaderText("《工作场所职业卫生监督管理规定》第十六条");

            basicInfo6.Cell(5, 5).Select();
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
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("     根据现场调查，企业根据劳动者接触职业病危害因素情况，为劳动者发放了防尘口罩、防毒口罩、防毒面具、耳塞、防护手套、防护服等个人防护用品。共检查4项，均符合法律法规的要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.4 应急救援措施评价：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      应急救援设施检查内容见表2-8。。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("表2-8  应急救援设施检查表");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo7 = m_doc.Tables.Add(m_wordApp.Selection.Range, 6, 5);
            basicInfo7.Borders.Enable = 1;
            basicInfo7.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo7.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo7.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo7.Columns[1].Width = 40;
            basicInfo7.Columns[2].Width = 120;
            basicInfo7.Columns[3].Width = 60;
            basicInfo7.Columns[4].Width = 120;
            basicInfo7.Columns[5].Width = 80;

            basicInfo7.Rows[1].Height = 20;
            basicInfo7.Cell(1, 1).Select();
            SetCellHeaderText("序号");
            basicInfo7.Cell(1, 2).Select();
            SetCellHeaderText("检查内容");
            basicInfo7.Cell(1, 3).Select();
            SetCellHeaderText("选用标准");
            basicInfo7.Cell(1, 4).Select();
            SetCellHeaderText("检查结果");
            basicInfo7.Cell(1, 5).Select();
            SetCellHeaderText("评价");

            basicInfo7.Cell(2, 1).Select();
            SetCellHeaderText("1");
            basicInfo7.Cell(2, 2).Select();
            SetCellHeaderText("存在或可能产生职业病危害的生产车间、设备应按照GBZ 158设置职业病危害警示标识。");
            basicInfo7.Cell(2, 3).Select();
            SetCellHeaderText("GBZ1-2010 5.2.1.6");

            basicInfo7.Cell(3, 1).Select();
            SetCellHeaderText("2");
            basicInfo7.Cell(3, 2).Select();
            SetCellHeaderText("可能发生急性职业病危害的有毒、有害的生产车间的布置应设置与相应事故防范和应急救援相配套的设施及设备，并留有应急通道。");
            basicInfo7.Cell(3, 3).Select();
            SetCellHeaderText("GBZ1-2010 5.2.1.7");

            basicInfo7.Cell(4, 1).Select();
            SetCellHeaderText("3");
            basicInfo7.Cell(4, 2).Select();
            SetCellHeaderText("贮存酸、碱及高危液体物质贮罐区周围应设置泄险沟（堰）。");
            basicInfo7.Cell(4, 3).Select();
            SetCellHeaderText("GBZ1-2010 6.1.3");

            basicInfo7.Cell(5, 1).Select();
            SetCellHeaderText("4");
            basicInfo7.Cell(5, 2).Select();
            SetCellHeaderText("生产或使用有毒物质的、有可能发生急性职业病危害的工业企业的劳动定员设计应包括应急救援组织机构（站）编制和人员定员。");
            basicInfo7.Cell(5, 3).Select();
            SetCellHeaderText("GBZ1-2010 8.1");

            basicInfo7.Cell(6, 1).Select();
            SetCellHeaderText("5");
            basicInfo7.Cell(6, 2).Select();
            SetCellHeaderText("有可能发生化学性灼伤及经皮肤粘膜吸收引起急性中毒的工作地点或车间，应根据可能产生或存在的职业性有害因素及其危害特点，在工作地点就近设置现场应急处理设施。急救设施应包括：不断水 的冲淋、洗眼设施；气体防护柜；个人防护用品；急救包或急救箱以及急救药品；转运病人的担架和装置；急救处理的设施以及应急救援通讯设备等。");
            basicInfo7.Cell(6, 3).Select();
            SetCellHeaderText("GBZ1-2010 8.3");

            basicInfo7.Cell(6, 5).Select();
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
            m_wordApp.Selection.TypeText("2.5 总体布局和设备布局评价：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("     该企业的总体布局结合建设地点条件进行布置，厂房周围设有道路和防护绿化带。厂房内功能分区明确，布局合理，产生职业病危害因素的岗位分区域进行设置，采取了相应的 措施防止或减轻其产生的有毒物质对作业人员的影响。企业的总体布局符合《工业企业设计卫生标准》GBZ 1-2010中的相关要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("     本项目各生产车间的生产设备根据工艺流程的特点以及防护要求进行布置，优先选用了噪声较小的设备并采取了相应的排风、隔声减振措施，从而减轻其产生的化学和物理因素对作业工人的危害，综上所述，本项目设备布局符合《工业企业设计卫生标准》GBZ1-2010的有关要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.6 建筑卫生学评价：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("     本项目建筑物内部地面采用了防腐、有利于地面清扫的措施。建筑采用南北布置，并合理开窗，保证良好的自然采光和自然通风。生产车间、库房及办公室采用暖气供暖，生产车间照明采用人工照明为主的方式。建设项目车间卫生特征按照3级标准进行管理，设置了厕所、休息室、浴室、食堂等辅助用室，其辅助用室情况符合《工业企业设计卫生标准》的要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.7 辅助用室评价：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("     本项目生产车间和能源中心卫生特征分级为 级，切削液供给/处理区卫生特征分级为 级。车间内辅助用室及卫生设施情况见表2-9。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("表2-9  车间辅助用室及卫生设施情况");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo8 = m_doc.Tables.Add(m_wordApp.Selection.Range, 7, 4);
            basicInfo8.Borders.Enable = 1;
            basicInfo8.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo8.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo8.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo8.Columns[1].Width = 60;
            basicInfo8.Columns[2].Width = 120;
            basicInfo8.Columns[3].Width = 80;
            basicInfo8.Columns[4].Width = 100;

            basicInfo8.Rows[1].Height = 20;
            basicInfo8.Cell(1, 1).Select();
            SetCellHeaderText("位置");
            basicInfo8.Cell(1, 2).Select();
            SetCellHeaderText("辅助卫生用室");
            basicInfo8.Cell(1, 3).Select();
            SetCellHeaderText("卫生设施");
            basicInfo8.Cell(1, 4).Select();
            SetCellHeaderText("设置数量");
            basicInfo8.Cell(7, 4).Select();

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

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;         
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("     该企业厕所、盥洗室、浴室、更衣柜、食堂等卫生辅助用室的数量、设置位置和室内设置等均符合《工业企业设计卫生标准》对卫生辅助用室的要求，能够满足本工程正式投产运行后的需要。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.8 职业卫生管理情况评价:");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    本项目设置有职业健康领导小组，安全部门是职业病预防的主管部门，B工厂配备有  名专职职业卫生管理人员，且已参加了北京市 安全生产监督管理局组织的职业卫生管理员培训，经考试合格取得了北京市职业卫生管理员培训合格证，并参加了每年的继续教育。符合《中华人民共和国职业病防治法》第二十一条第一款和《工作场所职业卫生监督管理规定》第八条对职业病防治管理措施相应内容的规定。本项目所属的北京A汽车有限公司制定的职业卫生管理制度较为全面，符合《中华人民共和国职业病防治法》和《工作场所职业卫生监督管理规定》中对职业卫生管理制度的相关规定。本单位建立有职业卫生档案和职业健康监护档案，执行了职业病危害因素日常检测与评价制度、职业病危害申报和告知情况执行良好，符合《中华人民共和国职业病防治法》和《工作场所职业卫生监督管理规定》中相关规定。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.9 职业健康监护管理情况评价:");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    企业制定");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    对从事接触职业病危害的作业的员工，组织进行岗前体检、定期体检（在岗时）和离岗时的职业健康体检，检查结果通知员工本人；发现有与所从事的职业相关的健康损害的员工，书面通知人力资源管理部门，安排调离原工作岗位，并妥善安置；负责建立健全职业危害作业人员健康监护档案和职业病档案。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    根据企业提供的资料和职业卫生现场调查，企业每年定期组织接触职业病危害因素岗位的劳动者进行上岗前、在岗期间和离岗时的职业健康检查，");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    结合《中华人民共和国职业病防治法》、《职业健康检查管理办法》和《职业健康监护技术规范》等法律法规的要求组织接触职业病危害的人员进行职业健康体检，提高体检率，杜绝漏检项目。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.10 职业病危害防治经费:");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    本项目在职业病防治方面投入了一定的资金用于安装职业病防护设施、购买个人使用的职业病防护用品及为接触职业病危害因素的劳动者进行相应的职业健康检查。每个月统计下个月需要购买的防护用品数量等，具体的经费概算由每个部门单独统计后上报给总负责人。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.11 预评价建议的落实情况:");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    预评价报告建议的落实情况检查见表2-10。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("表2-10  预评价报告建议的落实情况检查");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo9 = m_doc.Tables.Add(m_wordApp.Selection.Range, 4, 3);
            basicInfo9.Borders.Enable = 1;
            basicInfo9.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo9.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo9.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo9.Columns[1].Width = 120;
            basicInfo9.Columns[2].Width = 100;
            basicInfo9.Columns[3].Width = 80;

            basicInfo9.Rows[1].Height = 20;
            basicInfo9.Cell(1, 1).Select();
            SetCellHeaderText("预评价建议");
            basicInfo9.Cell(1, 2).Select();
            SetCellHeaderText("落实情况");
            basicInfo9.Cell(1, 3).Select();
            SetCellHeaderText("检查结果");

            basicInfo9.Cell(4, 3).Select();
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
        }
        private void WriteTotalLayout()
        {
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("3 控制职业病危害的补充措施及建议");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      结合职业卫生调查分析，为更好的预防和控制职业病危害，对该项目提出以下补充措施及建议：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("3.1 职业病防护设施");
            m_wordApp.Selection.TypeParagraph();
            for (int i = 0; i < 2; ++i)
                m_wordApp.Selection.TypeText("\r\n");

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("3.2 个体防护用品");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （1） 提高个人防护意识，并督促其在进行操作时，能争取使用合格的个人防护用品，且保证防护用品在有效的使用期限内，使个人防护用品切实起到降低职业危害的作用。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("     （2） 建议企业签订职业卫生管理协议，并监督其职业卫生管理及个人防护用品的佩戴情况，严禁不佩戴个人防护用品进行相关操作。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("3.3 职业卫生管理");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （1）按照《中华人民共和国职业病防治法》等有关法律、法规的要求，进一步完善本企业职业病防治管理工作，细化职业卫生管理制度，定期对职工进行职业卫生知识的相关培训和告知，确保各项职业卫生管理制度的落实。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （2）加强噪声作业和噪声超标人员的个人防护，尽量减少工人接触噪声的工作时间。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （3）建议企业对生产车间内的警示标识进行经常性的检查，对字迹模糊、有破损的警示标识及时更换。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("     （4）加强对劳动者配戴个人防护用品的监督和培训，提高作业人员自我保护意识，督促其按要求正确使用各种个人防护用品，并且加强个人防护用品的定期更换及维护，保证防护用品的有效性。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （5）企业根据工人实际接触职业病危害因素情况，结合《中华人民共和国职业病防治法》、《职业健康检查管理办法》和《职业健康监护技术规范》等法律法规的要求组织接触职业病危害的人员进行职业健康体检。并加强对外包单位作业人员健康检查的监督。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （6）企业在生产规模扩大后，应注意加强对职业病危害因素的防护，定期委托职业卫生技术服务机构开展相关的检测评价工作。");
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
            m_wordApp.Selection.TypeText("4 评价结论");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      本次职业病危害控制效果评价按照国家安监局《建设项目职业病危害控制效果评价编制要求》的要求，根据建设单位提供的职业卫生管理等资料，结合项目所包含的建设内容以及现场调查结果，依据国家有关职业卫生法律、法规、标准和规范等，以现场调查、职业病危害因素检测相结合的方法，对照《工业企业设计卫生标准》、《工作场所有害因素职业接触限值 第1部分：化学有害因素》、《工作场所有害因素职业接触限值 第2部分：物理因素》等标准的有关要求对该项目进行综合分析作出评价。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （1）建设项目总平面布局、设备布局、建筑卫生学、辅助用室等方面符合《中华人民共和国职业病防治法》、《工业企业设计卫生标准》等有关法律、法规的要求。职业卫生管理、职业健康监护需要进一步完善。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("     （2）该工程生产工艺过程中产生或存在的化学有害因素为化学物质 ");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      依据《建设项目职业病危害风险分类管理目录》（2012年版）规定，将“汽车制造业”的风险等级分类为“ ”，结合以上分析，经综合判断，将本项目分类为职业病危害 的建设项目。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （3） 等职业病危害因素的控制效果符合《中华人民共和国职业病防治法》、《工业企业设计卫生标准》GBZ 1-2010等相关规定的要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （4）该工程较全面地考虑了生产过程中产生的各种职业病危害因素，并采取了除尘、通风排毒、隔声降噪等防护措施，有效地控制了工作场所中职业病危害因素的浓度和强度。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （5）该工程为接触职业病危害因素的作业工人配备了个人防护用品，并定期培训，发放管理均进行了登记，企业应督促作业人员正确使用和佩戴，保证个体防护用品的有效性。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （6）职业卫生管理：用人单位设有职业卫生管理机构，配备有名专职管理人员，制定了较为完整的职业卫生管理制度，并且开展了部分职业病防治工作，今后应如每年定期委托有资质的职业卫生技术服务机构进行了职业病危害因素日常检测，委托有资质的职业健康体检机构进行了员工的职业健康体检。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （7）职业健康检查：用人单位建立有《 》，并每年定期组织接触职业病危害因素的劳动者进行了上岗前、在岗期间和离岗时的职业健康检查，但体检人数和体检项目不全，存在着职业病危害因素漏检员工和漏检项目问题。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("      （8）今后建设单位如有新建、改建、扩建、技术改造、技术引进等建设项目，应按《建筑行业职业病危害预防控制规范》（GBZ/T 211-2008）要求对施工过程中职业病危害进行预防和控制。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("      通过本报告书的综合分析，本项目在实施过程中，能够落实有关职业病危害防护措施，在正常生产且职业病危害防护设施运转正常的条件下，经现场检测，工人在配备有效的个人防护用品后实际接触职业病危害因素的浓度或强度符合国家有关职业卫生标准、规范的要求。职业病危害防护措施可行、有效。该项目已具备职业卫生竣工验收条件。");
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
