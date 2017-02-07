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
    public class CreatePresentEvaluationDoc
    {
        public static   List<Parameter> ParmeterChemicalModel = new List<Parameter>();
        public static List<OccupationaldiseaseHarm> OccupationaldiseaseHarm = new List<OccupationaldiseaseHarm>();
        public static ProjectInfo projectinfo = new ProjectInfo();
        string Header = "合同编号：            DKL/QB-11-05                      职业病危害控制效果评价评价 ";
        string Footer = "北京德康莱健康安全科技股份有限公司";  
        Word.Application m_wordApp;
        Word.Document m_doc;
        Word.Tools.CommonUtils m_utils;

        public CreatePresentEvaluationDoc(ProjectInfo projectmodels, List<Parameter> ParmeterChemicalModels, List<OccupationaldiseaseHarm> OccupationaldiseaseHarms)
        {
            projectinfo = projectmodels;
            ParmeterChemicalModel = ParmeterChemicalModels;
            OccupationaldiseaseHarm = OccupationaldiseaseHarms;
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

                #endregion
                m_wordApp.DisplayAlerts = WdAlertLevel.wdAlertsNone;
                m_utils = new Word.Tools.CommonUtils(m_wordApp);
                m_doc = m_wordApp.Documents.Add();

                //SetPage();
                WriteFirstPage("编号", "公司名", "二〇一五年六月一日");
                //      m_doc.Paragraphs.Last.Range.InsertBreak();
                //WriteInstruction();
                WriteInstruction("", "");
                WriteContent("", "", null, null, null, null, "");
                WriteEmployerBasicInfo();
                WriteTotalLayout();
                WriteDeviceLayout();
                WriteBuildingHygiene();
                WriteOccupationalHazard();
                WriteEmergency();
                WriteOccupationalHealth();
                WriteHealthDevice();
                WriteAuxiliaryRoom();
                WriteHealthManagement();
                WriteConclusion();
                WriteSuggestion();
                WriteLast();

                //         m_doc.Paragraphs.Last.Range.InsertBreak();
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
            string name = "合同模板样例" + DateTime.Now.ToFileTime();
            //      string documentFile = m_utils.File.Combine("c:\\", name, Word.Tools.DocumentFormat.Normal);
            string path = "d://DKLdownload";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string documentFile = m_utils.File.Combine(path, "合同模板样例", Word.Tools.DocumentFormat.Normal);
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
            m_doc.PageSetup.LeftMargin = (float)35.7;
            m_doc.PageSetup.RightMargin = (float)35.7;
        }
        private void WriteFirstPage(string reportNum, string company, string reportDate)
        {
            
            Word.Table basicInfo = m_doc.Tables.Add(m_wordApp.Selection.Range, 3, 2);
            basicInfo.Rows.Alignment = WdRowAlignment.wdAlignRowRight;
            basicInfo.Borders.Enable = 1;
            basicInfo.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Columns[1].Width = 90;

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

            basicInfo.Rows[3].Height = 20;
            basicInfo.Cell(3, 1).Select();
            SetCellHeaderText("本册为");
            basicInfo.Cell(3, 2).Select();
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
            m_wordApp.Selection.TypeText("\r\n\r\n" + company);
            m_wordApp.Selection.TypeText("工作场所职业病危害现状评价报告书");

            //测试结束
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 16;
            for (int i = 0; i < 7; ++i)
                m_wordApp.Selection.TypeText("\r\n");
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("\r\n北京德康莱安全卫生技术发展有限公司");
            m_wordApp.Selection.TypeText("\r\n二〇一五年六月");

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
        private void WriteInstruction(string company, string date)
        {
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.Font.Size = 16;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("前    言");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("    根据《中华人民共和国职业病防治法》及《工作场所职业卫生监督管理规定》规定，对存在职业病危害的单位，应委托具有相应资质的职业卫生技术服务机构定期进行职业病危害现状评价。");
            m_wordApp.Selection.TypeText(projectinfo.CreateTime.ToString("yyyy/MM/dd") + "北京德康莱安全卫生技术发展有限公司接受" +projectinfo.CompaneName + "的委托，对其工作场所进行职业病危害现状评价。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    按照《建设项目职业病危害评价规范》的要求，北京德康莱安全卫生技术发展有限公司组织评价人员对该用人单位工作场所的总体布局、生产工艺、设备布局、原辅材料与产品、建筑卫生学、职业病危害因素种类和分布、职业病防护设施及职业卫生管理等进行了现场调查，并在初步分析的基础上，编制了现状评价方案，对现场职业病危害因素进行了检测并编制现状评价报告书。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.TypeText("    在职业病危害现状评价过程中得到了" + company + "有关部门及同志的大力支持和协助，特此感谢！");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            m_wordApp.Selection.TypeText("北京德康莱安全卫生技术发展有限公司");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText(projectinfo.ProjectClosingDate.ToString("yyyy/MM/dd"));
            m_wordApp.Selection.TypeParagraph();
        }
        
        //总论部分
        //参数包括：公司，日期，法律法规，标准规范，基础依据，其他依据，评价范围。
        private void WriteContent(string company, string date,List<string> laws, List<string> normal, List<string> basicAccord, List<string> otherAccord, string evalueScope)
        {
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 16;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("1总论");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("任务来源");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText(company);
            m_wordApp.Selection.TypeText(projectinfo.CompaneName+"为切实保障劳动者的职业健康，依据《中华人民共和国职业病防治法》(中华人民共和国主席令第52号)、《建设项目职业卫生“三同时”监督管理暂行办法》（国家安全生产监督管理总局令第51号）、《工作场所职业卫生监督管理规定》（国家安全生产监督管理总局令第47号）等法律法规的相关规定及北京市安全监管部门的要求，于");
            m_wordApp.Selection.TypeText(projectinfo.CreateTime.Year.ToString()+"年"+projectinfo.CreateTime.Month.ToString() + "月委托北京德康莱安全卫生技术发展有限公司对其工作场所进行职业病危害现状评价（委托书见附件1）。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("1.2评价目的");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("⑴贯彻落实国家有关职业卫生的法律、法规、规章和标准。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("⑵明确用人单位生产经营活动过程中的职业病危害因素种类及其危害程度，以及职业病防护设施和职业卫生管理措施的效果等。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("⑶为用人单位职业病防治的日常管理提供科学依据。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("⑷为政府监管部门对用人单位职业卫生实施监督管理提供科学依据。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("1.3评价依据");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("1.3.1法律、法规、规章");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.Font.Size = 14;
            if(laws != null)
            {
                foreach(var item in laws)
                {
                    m_wordApp.Selection.TypeText(item);
                    m_wordApp.Selection.TypeParagraph();
                }
            }

            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("1.3.2规范、标准");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.Font.Size = 14;

            if (normal != null)
            {
                foreach (var item in normal)
                {
                    m_wordApp.Selection.TypeText(item);
                    m_wordApp.Selection.TypeParagraph();
                }
            }
            //以下依据，随项目不同而变化
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("1.3.3基础依据");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.Font.Size = 14;
            if (basicAccord != null)
            {
                foreach (var item in basicAccord)
                {
                    m_wordApp.Selection.TypeText(item);
                    m_wordApp.Selection.TypeParagraph();
                }
            }
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("1.3.4其他依据");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.Font.Size = 14;
            if (otherAccord != null)
            {
                foreach (var item in otherAccord)
                {
                    m_wordApp.Selection.TypeText(item);
                    m_wordApp.Selection.TypeParagraph();
                }
            }
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("1.4评价范围");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("    本次评价主要针对" + evalueScope);
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    本现状评价工作只针对现有的设备、工艺及原辅材料进行评价，如设备、工艺及原辅材料等发生更改，需重新进行评价。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("1.5评价内容");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("    依据《建设项目职业病危害评价规范》的要求，本次评价内容主要包括该用人单位的总体布局及设备布局、建筑卫生学、职业病危害因素分布及对劳动者健康的影响、职业病危害防护设施及效果、辅助用室基本卫生学要求、个人使用的职业病防护用品、职业健康监护、应急救援措施、职业卫生管理措施及落实情况等。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("1.6评价单元");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("    根据该用人单位的主要工程建设内容以及设备布置相对独立性的特点，将该用人单位划分以下评价单元进行职业病危害因素的识别。评价单元的划分情况见表1-1。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表1-1  评价单元划分一览表");
            m_wordApp.Selection.TypeParagraph();

            Word.Table basicInfo = m_doc.Tables.Add(m_wordApp.Selection.Range, 4, 3);
            basicInfo.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            basicInfo.Borders.Enable = 1;
            basicInfo.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Columns[1].Width = 30;
            basicInfo.Columns[2].Width = 120;
            basicInfo.Rows[1].Height = 20;
            basicInfo.Cell(1, 1).Select();
            SetCellHeaderText("序号");
            basicInfo.Cell(1, 2).Select();
            SetCellHeaderText("评价单元");
            basicInfo.Cell(1, 3).Select();
            SetCellHeaderText("包括的工作场所");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("1.7评价方法");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("    根据用人单位职业病危害的特点，采用职业卫生调查、职业卫生检测、职业健康检查、检查表分析、职业病危害作业分级等方法，对用人单位正常生产期间存在职业病危害暴露的劳动者的职业病危害因素接触水平、职业病防护设施效果以及职业卫生管理措施进行综合分析、定性和定量评价。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("1.7.1职业卫生调查法");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("    在分析用人单位提供的有关资料的基础上，展开职业卫生调查。内容主要包括：用人单位概况、设计生产能力及实际运行情况、总体布局、生产工艺、生产设备及布局、生产过程中的物料及产品、建筑卫生学、职业病危害因素及接触水平、职业病防护设施及运行情况、个人使用的职业病防护用品、辅助用室、应急救援设施、职业卫生管理制度及其执行情况、历年职业病危害因素检测（监测）及评价、职业健康检查资料等。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("1.7.2职业卫生检测");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("    依据《工作场所空气中有害物质监测的采样规范》（GBZ159-2004）、《工作场所有害因素职业接触限值 第1部分：化学有害因素》（GBZ2.2-2007）、《工作场所有害因素职业接触限值 第2部分：物理因素》（GBZ2.2-2007）等检测标准和方法，对该用人单位工作场所的化学有害因素（含粉尘）、物理因素等职业病危害因素进行检测。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("1.7.1职业卫生调查法");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("    在分析用人单位提供的有关资料的基础上，展开职业卫生调查。内容主要包括：用人单位概况、设计生产能力及实际运行情况、总体布局、生产工艺、生产设备及布局、生产过程中的物料及产品、建筑卫生学、职业病危害因素及接触水平、职业病防护设施及运行情况、个人使用的职业病防护用品、辅助用室、应急救援设施、职业卫生管理制度及其执行情况、历年职业病危害因素检测（监测）及评价、职业健康检查资料等。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("1.7.3职业健康检查法");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("    按照《用人单位职业健康监护监督管理办法》（国家安全生产监督管理总局令第49号）、《职业健康监护技术规范》（GBZ188-2014）等相关规定、标准对该用人单位职业健康监护资料进行综合分析。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("1.7.4检查表分析法");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("    依据《中华人民共和国职业病防治法》、《工业企业设计卫生标准》（GBZ1-2010）等的法律、法规和技术规范，按检查项目、相应检查依据、检查实际情况、检测结果等，编制成表，对用人单位的选址、总体布局、工艺、设备布局、职业病危害因素、职业病危害防护设施、个人职业病防护用品、辅助用室、应急救援设施、建筑卫生学、职业卫生管理进行职业卫生调查与评价。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("1.7.5综合分析法");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("    依据《中华人民共和国职业病防治法》、《工作场所职业卫生监督管理规定》（国家安全生产监督管理总局令第47号）、《职业病危害项目申报办法》（国家安全生产监督管理总局令第48号）和《工业企业设计卫生标准》（GBZ1-2010）等的相关法律、法规对用人单位选址、总体布局、工艺、设备布局、职业病危害因素、职业病防护设施、辅助用室、职业卫生管理等方面现状情况及其合法合规性进行综合分析与评价。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("1.8评价程序");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("    北京德康莱安全卫生技术发展有限公司接受委托后，立即组织有资质的职业卫生评价及检测人员，对该用人单位进行了初步调查，并对用人单位提供的相关资料进行了综合分析与研究。依据《建设项目职业病危害评价规范》编制了《职业病危害现状评价方案》，并按质量管理程序对该方案进行了审查。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("_____年___月___日至___月___日");//现场检测时间
            m_wordApp.Selection.TypeText("    评价组按审定的方案对该用人单位作业场所进行了职业卫生现场检测与测量、作业人员工作日写实、工作场所职业病危害因素与职业病危害防护设施调查、职业卫生检测，在此基础上编制了该评价报告书。工作程序见图1-1。");
            m_wordApp.Selection.TypeParagraph();

            //图片
            Word.Paragraph para = m_doc.Paragraphs.Add();
            Word.Range range = para.Range;
            //Word.InlineShape shap = range.InlineShapes.AddPicture(@"NETofficeTest\NETofficeTest\bin\Debug\fv.png");

            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("1.9质量控制");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.TypeText("    依据公司质量手册、程序文件、作业指导书等质量控制文件（第三版第一次修改），我公司对本项目评价工作实施全过程质量控制。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("人员要求：评价技术人员、检测技术人员、评价报告书审核人员均通过国家或北京市职业卫生技术服务专业技术人员资质培训，持证上岗；");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("评价过程：职业病危害现状评价全过程严格按照公司质量手册、程序文件和作业指导书等有关质量管理规定执行；");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("评价依据：国家、地方、行业现行有效的有关职业卫生法律、法规、技术规范及标准，企业相关的支撑性文件及材料；");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("检测检验：检测和实验室检验使用的仪器均经过国家计量检定合格，仪器使用前均校准，职业病危害因素的采样、检测检验依据相应的技术规范及标准等受控技术文件完成；");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("职业健康监护：报告中采用的职业健康检查资料均由企业提供；");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("评价报告书的审核：严格按照我单位作业指导书中的审核程序进行审核。该评价工作依据的质量控制程序见图1-2。");
            m_wordApp.Selection.TypeParagraph();

            //图片
            Word.Paragraph para2 = m_doc.Paragraphs.Add();
            Word.Range range2 = para.Range;
            //Word.InlineShape shap = range2.InlineShapes.AddPicture(@"NETofficeTest\NETofficeTest\bin\Debug\fv.png");
        }

        //用人单位概况
        private void WriteEmployerBasicInfo()
        {
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 16;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2用人单位概况");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1基本情况");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.1地理位置");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0; 
            m_wordApp.Selection.TypeText("    地理位置见图2-1，周边环境见图2-2。");
            m_wordApp.Selection.TypeParagraph();
            //图片



            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.2自然环境");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    北京经济技术开发区属暖温带半湿润大陆季风气候，四季分明。春季干旱多风，夏季高温多雨，秋季天高气爽，冬季寒冷干燥。春秋季短，冬夏季漫长。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    北京经济技术开发区年均降水量600mm，年平均相对湿度60.2%。全年无霜期约180-200d，最大冻土层厚度约700mm。夏季最小风频为SSE；全年最小风频为WNW。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    多年平均气压                   1010.6hPa");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    多年平均气温                   11.5℃");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    最冷月（1月）平均气温          -4.6℃");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    最热月（8月）平均气温          26.4℃");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    历年极端最高气温                40℃");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    历年极端最低气温               -27.4℃");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    最高月平均相对湿度              78％");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    最低月平均相对湿度              45％");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    年平均降水量                    580mm");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    夏季最小风频                    SSE");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    全年最小风频                    WNW");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    该地区全年及夏季风频图见图2-3。");
            m_wordApp.Selection.TypeParagraph();
            //图

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("2.1.3生产规模");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.TypeText("2.1.4原、辅材料及产品");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    主要化学品成份及用量见表2-1。");
            m_wordApp.Selection.TypeParagraph();

                        
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表2-1  主要化学品成分及用量一览表");

            Word.Table basicInfo = m_doc.Tables.Add(m_wordApp.Selection.Range, 5, 5);
            basicInfo.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            basicInfo.Borders.Enable = 1;
            basicInfo.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            basicInfo.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            basicInfo.Columns[1].Width = 50;
            basicInfo.Columns[2].Width = 120;
            basicInfo.Rows[1].Height = 20;
            basicInfo.Cell(1, 1).Select();
            SetCellHeaderText("辅料名称");
            basicInfo.Cell(1, 2).Select();
            SetCellHeaderText("性状");
            basicInfo.Cell(1, 3).Select();
            SetCellHeaderText("成分");
            basicInfo.Cell(1, 5).Select();
            SetCellHeaderText("年用量");
            basicInfo.Cell(1, 1).Merge(basicInfo.Cell(2, 1));
            basicInfo.Cell(1, 2).Merge(basicInfo.Cell(2, 2));
            basicInfo.Cell(1, 5).Merge(basicInfo.Cell(2, 5));
            basicInfo.Cell(1, 3).Merge(basicInfo.Cell(1, 4));
            basicInfo.Cell(2, 3).Select();
            SetCellHeaderText("名称");
            basicInfo.Cell(2, 4).Select();
            SetCellHeaderText("含量 %");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("2.1.5劳动定员及工作制度");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    该用人单位维修作业人员  人，其中机修工  人、钣金  人、喷漆（打磨）工  人、洗车工  人、主管  人，  名调漆工为外雇人员，实行长白班、单休工作制度，每天工作  h，每周工作  天，年工作日约为  天。接触职业病危害因素岗位及定员见表2-2。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表2-2  接触职业病危害因素岗位及定员一览表");

            Word.Table info = m_doc.Tables.Add(m_wordApp.Selection.Range, 5, 5);
            info.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info.Borders.Enable = 1;
            info.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info.Columns[1].Width = 30;
            info.Columns[2].Width = 120;
            info.Rows[1].Height = 20;
            info.Cell(1, 1).Select();
            SetCellHeaderText("序号");
            info.Cell(1, 2).Select();
            SetCellHeaderText("工作场所");
            info.Cell(1, 3).Select();
            SetCellHeaderText("工种（岗位）");
            info.Cell(1, 4).Select();
            SetCellHeaderText("年用量");
            info.Cell(1, 5).Select();
            SetCellHeaderText("作业方式");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("2.1.6公用工程");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("⑴供电");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    该用人单位用电设备属___类负荷。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("⑵给排水");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    给水：该用人单位生产、生活及绿化用水均由北京市经济技术开发区市政自来水管网供给，用水总量约为_________m3/a。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("排水：");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("⑶供热：");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("2.2职业卫生“三同时”执行情况");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    该用人单位  ,根据《建设项目职业卫生“三同时”监督管理暂行办法》规定，2011年1月1日以前投产运行的，   。目前委托北京德康莱安全卫生技术发展有限公司（资质证书编号：（京）安职技字（2013）第B-0004号）进行工作场所职业病危害因素现状评价工作。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("2.3生产运行情况");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeParagraph();
        }

        //总体布局调查与评价
        private void WriteTotalLayout()
        {
            m_wordApp.Selection.Font.Size = 16;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("3总体布局调查与评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("3.1总体布局调查");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("3.1.1总平面布置");
            m_wordApp.Selection.TypeParagraph();

            //图
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("3.1.2竖向布置");
            m_wordApp.Selection.TypeParagraph();


            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("3.2总体布局评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    依据《工业企业设计卫生标准》（GBZ1-2010）的相关规定，对该用人单位总体布局情况应用检查表法进行检查与评价，详见表3-1。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表3-1  总体布局检查表");
            Word.Table info = m_doc.Tables.Add(m_wordApp.Selection.Range, 7, 5);
            info.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info.Borders.Enable = 1;
            info.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info.Columns[1].Width = 30;
            info.Columns[2].Width = 120;
            info.Rows[1].Height = 20;
            info.Cell(1, 1).Select();
            SetCellHeaderText("序号");
            info.Cell(1, 2).Select();
            SetCellHeaderText("检查内容");
            info.Cell(1, 3).Select();
            SetCellHeaderText("检查依据");
            info.Cell(1, 4).Select();
            SetCellHeaderText("检查结果");
            info.Cell(1, 5).Select();
            SetCellHeaderText("评价");

            info.Rows[2].Height = 80;
            info.Cell(2, 1).Select();
            SetCellHeaderText("1");
            info.Cell(2, 2).Select();
            SetCellHeaderText("工业企业厂区总平面布置应明确功能分区，可分为生产区、生活办公区、辅助生产区。");
            info.Cell(2, 3).Select();
            SetCellHeaderText("工业企业设计卫生标准》（GBZ1-2010）5.2.1.1");
            info.Rows[3].Height = 180;
            info.Cell(3, 1).Select();
            SetCellHeaderText("2");
            info.Cell(3, 2).Select();
            SetCellHeaderText("工业企业厂区总平面功能分区的分区原则应遵循：分期建设项目宜一次整体规划，使各单体建筑均在其功能区内有序合理，避免分期建设时破坏原功能分区；行政办公用房应设置在非生产区；生产车间及与生产有关的辅助用室应布置在生产区内。");
            info.Cell(3, 3).Select();
            SetCellHeaderText("工业企业设计卫生标准》（GBZ1-2010）5.2.1.3");
            info.Rows[4].Height = 180;
            info.Cell(4, 1).Select();
            SetCellHeaderText("3");
            info.Cell(4, 2).Select();
            SetCellHeaderText("工业企业的总平面布置，在满足主体工程的前提下，已将可能产生严重职业性有害因素的设施远离产生一般职业性有害因素的其他设施，应将车间按有无危害、危害的类型及其危害浓度（强度）分开。");
            info.Cell(4, 3).Select();
            SetCellHeaderText("工业企业设计卫生标准》（GBZ1-2010）5.2.1.5");
            info.Rows[5].Height = 80;
            info.Cell(5, 1).Select();
            SetCellHeaderText("4");
            info.Cell(5, 2).Select();
            SetCellHeaderText("存在或可能产生职业病危害的生产车间、设备按照GBZ158设置职业病危害警示标识。");
            info.Cell(5, 3).Select();
            SetCellHeaderText("工业企业设计卫生标准》（GBZ1-2010）5.2.1.6");
            info.Rows[6].Height = 180;
            info.Cell(6, 1).Select();
            SetCellHeaderText("5");
            info.Cell(6, 2).Select();
            SetCellHeaderText("放散大量热量或有害气体的厂房宜采用单层建筑。当厂房是多层建筑物时，放散热和有害气体的生产过程宜布置在建筑物的高层。如必须布置在下层时，应采取有效措施防止污染上层工作环境。");
            info.Cell(6, 3).Select();
            SetCellHeaderText("工业企业设计卫生标准》（GBZ1-2010）5.2.2.1");
            info.Rows[7].Height = 180;
            info.Cell(7, 1).Select();
            SetCellHeaderText("6");
            info.Cell(7, 2).Select();
            SetCellHeaderText("噪声与振动较大的生产设备宜安装在单层厂房内。当设计需要将这些生产设备安置在多层厂房内时，宜将其安装在底层，并采取有效的隔声和减振措施。");
            info.Cell(7, 3).Select();
            SetCellHeaderText("工业企业设计卫生标准》（GBZ1-2010）5.2.2.2");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("    对该用人单位的总体布局情况共检查上述6项，均符合上述相关标准、规定的要求。");
            m_wordApp.Selection.TypeParagraph();
        }

        //生成工艺和设备布局调查与评价
        private void WriteDeviceLayout()
        {
            m_wordApp.Selection.Font.Size = 16;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("4生产工艺和设备布局调查与评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("4.1生产工艺调查与评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("4.1.1生产工艺调查");
            m_wordApp.Selection.TypeParagraph();


            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1; 
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("图4-1  车辆维修工艺流程图");
            m_wordApp.Selection.TypeParagraph();
            //图

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("4.1.2生产工艺评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    依据《工业企业设计卫生标准》（GBZl-2010）的相关法规、标准、规范对各工作场所的生产工艺进行检查表分析评价，具体内容详见表4-1。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表4-1  生产工艺评价检查表");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info1 = m_doc.Tables.Add(m_wordApp.Selection.Range, 5, 5);
            info1.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info1.Borders.Enable = 1;
            info1.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info1.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info1.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info1.Columns[1].Width = 20;
            info1.Columns[2].Width = 120;
            info1.Rows[1].Height = 20;
            info1.Cell(1, 1).Select();
            SetCellHeaderText("序号");
            info1.Cell(1, 2).Select();
            SetCellHeaderText("检查内容");
            info1.Cell(1, 3).Select();
            SetCellHeaderText("检查依据");
            info1.Cell(1, 4).Select();
            SetCellHeaderText("检查记录");
            info1.Cell(1, 5).Select();
            SetCellHeaderText("评价");
            info1.Rows[2].Height = 80;
            info1.Cell(2, 1).Select();
            SetCellHeaderText("1");
            info1.Cell(2, 2).Select();
            SetCellHeaderText("原材料选择应遵循无毒物质替代有毒物质，低毒物质替代高毒物质的原则");
            info1.Cell(2, 3).Select();
            SetCellHeaderText("工业企业设计卫生标准》（GBZ1-2010）6.1.1.1");
            info1.Rows[3].Height = 180;
            info1.Cell(3, 1).Select();
            SetCellHeaderText("2");
            info1.Cell(3, 2).Select();
            SetCellHeaderText("对产生粉尘、毒物的生产过程和设备（含露天作业的工业设施），应优先采用机械化和自动化，避免直接工人操作。");
            info1.Cell(3, 3).Select();
            SetCellHeaderText("工业企业设计卫生标准》（GBZ1-2010）6.1.1.2");
            info1.Rows[4].Height = 180;
            info1.Cell(4, 1).Select();
            SetCellHeaderText("3");
            info1.Cell(4, 2).Select();
            SetCellHeaderText("对于逸散粉尘的生产过程，应对产尘设备采取密闭措施；设置适宜的局部排风除尘设施对尘源进行控制；生产工艺和粉尘性质可采取湿式作业的，应采取湿法抑尘。当湿式作业仍不能满足卫生要求时，应采用其他通风、除尘方式。");
            info1.Cell(4, 3).Select();
            SetCellHeaderText("工业企业设计卫生标准》（GBZ1-2010）6.1.1.3");
            info1.Rows[5].Height = 80;
            info1.Cell(5, 1).Select();
            SetCellHeaderText("4");
            info1.Cell(5, 2).Select();
            SetCellHeaderText("在满足工艺流程要求的前提下，宜将高噪声设备相对集中，并采取相应的隔声、吸声、消声、减振等控制措施。");
            info1.Cell(5, 3).Select();
            SetCellHeaderText("工业企业设计卫生标准》（GBZ1-2010）6.1.1.4");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("    对该用人单位的生产工艺共检查4项内容，均符合上述相关标准、规定的要求。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("4.2生产设备及布局调查与评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("4.2.1生产设备及布局调查");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    该用人单位主要设备见表4-2。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表4-2  主要设备一览表");
            Word.Table info2 = m_doc.Tables.Add(m_wordApp.Selection.Range, 7, 5);
            info2.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info2.Borders.Enable = 1;
            info2.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info2.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info2.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info2.Columns[1].Width = 20;
            info2.Columns[2].Width = 120;
            info2.Rows[1].Height = 20;
            info2.Cell(1, 1).Select();
            SetCellHeaderText("序号");
            info2.Cell(1, 2).Select();
            SetCellHeaderText("作业场所");
            info2.Cell(1, 3).Select();
            SetCellHeaderText("设备名称");
            info2.Cell(1, 4).Select();
            SetCellHeaderText("数量");
            info2.Cell(1, 5).Select();
            SetCellHeaderText("单位");
            info2.Cell(7, 5).Select();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("    设备按工艺路线和工艺要求进行布置，各设备之间有间距，不会产生相互影响；");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("4.2.2生产设备及布局评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    依据《工业企业设计卫生标准》（GBZl-2010）等相关法规、标准、规范对设备布局进行检查表分析评价，具体内容见表4-2。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表4-2  设备布局检查表");
            Word.Table info3 = m_doc.Tables.Add(m_wordApp.Selection.Range, 5, 5);
            info3.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info3.Borders.Enable = 1;
            info3.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info3.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info3.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info3.Columns[1].Width = 20;
            info3.Columns[2].Width = 120;
            info3.Rows[1].Height = 20;
            info3.Cell(1, 1).Select();
            SetCellHeaderText("序号");
            info3.Cell(1, 2).Select();
            SetCellHeaderText("检查内容");
            info3.Cell(1, 3).Select();
            SetCellHeaderText("检查依据");
            info3.Cell(1, 4).Select();
            SetCellHeaderText("检查结果");
            info3.Cell(1, 5).Select();
            SetCellHeaderText("评价");

            info3.Rows[2].Height = 80;
            info3.Cell(2, 1).Select();
            SetCellHeaderText("1");
            info3.Cell(2, 2).Select();
            SetCellHeaderText("放散大量热量或有害气体的厂房宜采用单层建筑。当厂房是多层建筑物时，放散热和有害气体的生产过程宜布置在建筑物的高层。如必须布置在下层时，应采取有效的措施防止污染上层工作环境。");
            info3.Cell(2, 3).Select();
            SetCellHeaderText("工业企业设计卫生标准》（GBZ1-2010）5.2.2.1");
            info3.Rows[3].Height = 180;
            info3.Cell(3, 1).Select();
            SetCellHeaderText("2");
            info3.Cell(3, 2).Select();
            SetCellHeaderText("噪声与振动较大的生产设备宜安装在单层厂房内，当设计需要将这些生产设备安置在多层厂房内时，宜将其安装在底层，并采取有效的隔声和减振措施。");
            info3.Cell(3, 3).Select();
            SetCellHeaderText("工业企业设计卫生标准》（GBZ1-2010）5.2.2.2");
            info3.Rows[4].Height = 180;
            info3.Cell(4, 1).Select();
            SetCellHeaderText("3");
            info3.Cell(4, 2).Select();
            SetCellHeaderText("产生噪声、振动的厂房设计和设备布局应采取降噪和减振措施。");
            info3.Cell(4, 3).Select();
            SetCellHeaderText("工业企业设计卫生标准》（GBZ1-2010）5.3.4");
            info3.Rows[5].Height = 80;
            info3.Cell(5, 1).Select();
            SetCellHeaderText("4");
            info3.Cell(5, 2).Select();
            SetCellHeaderText("设备布置便于操作和维护；尽量避免生产装置之间危害因素的相互影响，减小对人员的综合作用。");
            info3.Cell(5, 3).Select();
            SetCellHeaderText("工业企业设计卫生标准》（GBZ1-2010）5.7.2");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("    对该用人单位设备及布局共检查4项内容，均符合上述相关标准、规定的要求。");
            m_wordApp.Selection.TypeParagraph();

        }

        //建筑卫生学调查与评价
        private void WriteBuildingHygiene()
        {
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("5建筑卫生学调查与评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("5.1建筑卫生学调查");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    北京三江华晨汽车销售服务有限公司建筑物均为砖混结构，主要建筑物情况见表5-1。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("5-1  主要建筑物一览表");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info0 = m_doc.Tables.Add(m_wordApp.Selection.Range, 5, 5);
            info0.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info0.Borders.Enable = 1;
            info0.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info0.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info0.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info0.Columns[1].Width = 30;
            info0.Columns[2].Width = 120;

            info0.Rows[1].Height = 20;
            info0.Cell(1, 1).Select();
            SetCellHeaderText("序号");
            info0.Cell(1, 2).Select();
            SetCellHeaderText("建构筑物名称");
            info0.Cell(1, 3).Select();
            SetCellHeaderText("结构形式");
            info0.Cell(1, 4).Select();
            SetCellHeaderText("朝向");
            info0.Cell(1, 5).Select();
            SetCellHeaderText("层数");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            

            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("5.1.1通风、空气调节");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("   ");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("5.1.2采光照明");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("   ");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("5.1.1通风、空气调节");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("   ");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("5.1.3微小气候");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("    该用人单位微小气候检测结果见表5-2。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表5-2  微小气候检测结果一览表");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info11 = m_doc.Tables.Add(m_wordApp.Selection.Range, 5, 5);
            info11.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info11.Borders.Enable = 1;
            info11.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info11.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info11.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info11.Columns[1].Width = 30;
            info11.Columns[2].Width = 120;
            info11.Rows[1].Height = 20;
            info11.Cell(1, 1).Select();
            SetCellHeaderText("序号");
            info11.Cell(1, 2).Select();
            SetCellHeaderText("测定地点");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("5.2建筑卫生学评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    依据《工业企业设计卫生标准》（GBZ1-2010）、《采暖通风与空气调节设计规范》（GB50019-2003）、《建筑照明设计标准》（GB50034-2013）的要求，对该用人单位的建筑卫生学应用检查表法进行检查与评价，见表5-3。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表5-3 建筑卫生学评价检查表");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info1 = m_doc.Tables.Add(m_wordApp.Selection.Range, 7, 5);
            info1.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info1.Borders.Enable = 1;
            info1.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info1.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info1.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info1.Columns[1].Width = 30;
            info1.Columns[2].Width = 120;

            info1.Rows[1].Height = 20;
            info1.Cell(1, 1).Select();
            SetCellHeaderText("序号");
            info1.Cell(1, 2).Select();
            SetCellHeaderText("检查内容");
            info1.Cell(1, 3).Select();
            SetCellHeaderText("检查依据");
            info1.Cell(1, 4).Select();
            SetCellHeaderText("检查结果");
            info1.Cell(1, 5).Select();
            SetCellHeaderText("评价");
            info1.Rows[2].Height = 20;
            info1.Cell(2, 1).Select();
            SetCellHeaderText("通风、空气调节");
            info1.Cell(2, 1).Merge(info1.Cell(2, 5));
            info1.Rows[3].Height = 140;
            info1.Cell(3, 1).Select();
            SetCellHeaderText("1");
            info1.Cell(3, 2).Select();
            SetCellHeaderText("设计局部排风或全面排风时，宜采用自然通风。当自然通风不能满足卫生、环保或生产工艺要求时，应采用机械通风或自然与机械的联合通风。");
            info1.Cell(3, 3).Select();
            SetCellHeaderText("《采暖通风与空气调节设计规范》（GB50019-2003）5.1.9");
            info1.Rows[4].Height = 20;
            info1.Cell(4, 1).Select();
            SetCellHeaderText("采光照明");
            info1.Cell(4, 1).Merge(info1.Cell(4, 5));
            info1.Rows[5].Height = 180;
            info1.Cell(5, 1).Select();
            SetCellHeaderText("1");
            info1.Cell(5, 2).Select();
            SetCellHeaderText("厂房建筑方位应能使室内有良好的自然通风和自然采光。");
            info1.Cell(5, 3).Select();
            SetCellHeaderText("《工业企业设计卫生标准》（GBZl-2010）5.3.1");
            info1.Rows[6].Height = 180;
            info1.Cell(6, 1).Select();
            SetCellHeaderText("2");
            info1.Cell(6, 2).Select();
            SetCellHeaderText("在含有可燃易爆气体及粉尘的工作场所，应采用防爆灯具和防爆开关。");
            info1.Cell(6, 3).Select();
            SetCellHeaderText("《工业企业设计卫生标准》（GBZl-2010）6.5.4.7");
            info1.Rows[7].Height = 80;
            info1.Cell(7, 1).Select();
            SetCellHeaderText("3");
            info1.Cell(7, 2).Select();
            SetCellHeaderText("选用的照明光源应符合国家现行相关标准的有关规定。");
            info1.Cell(7, 3).Select();
            SetCellHeaderText("《建筑照明设计标准》（GB50034-2004）3.2.1");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("    对用人单位建筑卫生学情况进行调查，共检查4项内容，均符合上述标准、规范的相关要求。");
            m_wordApp.Selection.TypeParagraph();
        }

        //6职业病危害因素
        private void WriteOccupationalHazard()
        {
            m_wordApp.Selection.Font.Size = 16;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("6职业病危害因素");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("6.1职业病危害因素辨识");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("6.1.1生产（运行）工艺过程中的职业病危害因素辨识");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    本次评价按照工作场所区域分割的相对独立性划分原则、维修工艺流程和设备平面布置的特点，对该用人单位各评价单元的职业病危害因素进行辨识。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("6.1.2生产（运行）环境及劳动过程中的危害因素辨识");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("⑴生产环境中的危害因素");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    生产环境中的职业病危害因素主要是指工作场所布局不合理，各操作岗位通风换气、采光照明效果不好等。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("⑵劳动过程中的有害因素");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    劳动过程中的职业病危害因素主要是指劳动制度不合理，长时间不良体位，心理紧张等给工人造成的危害。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("6.1.3职业病危害因素接触水平分析");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("   该用人单位工作场所的主要职业病危害因素暴露及接触水平见表6-1。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表6-1  主要职业病危害因素暴露及接触水平分析表");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info0 = m_doc.Tables.Add(m_wordApp.Selection.Range, 5, 8);
            info0.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info0.Borders.Enable = 1;
            info0.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info0.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info0.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info0.Columns[1].Width = 30;
            info0.Columns[2].Width = 120;
            info0.Rows[1].Height = 20;
            info0.Cell(1, 1).Select();
            SetCellHeaderText("评价单元");
            info0.Cell(1, 2).Select();
            SetCellHeaderText("工作场所");
            info0.Cell(1, 3).Select();
            SetCellHeaderText("工种");
            info0.Cell(1, 4).Select();
            SetCellHeaderText("接触职业病危害因素名称");
            info0.Cell(1, 7).Select();
            SetCellHeaderText("接触方式");
            info0.Cell(1, 8).Select();
            SetCellHeaderText("产生职业病危害因素的工序");
            info0.Cell(2, 4).Select();
            SetCellHeaderText("粉尘");
            info0.Cell(2, 5).Select();
            SetCellHeaderText("物理因素");
            info0.Cell(2, 6).Select();
            SetCellHeaderText("化学因素");
            info0.Cell(2, 1).Merge(info0.Cell(1, 1));
            info0.Cell(2, 2).Merge(info0.Cell(1, 2));
            info0.Cell(2, 3).Merge(info0.Cell(1, 3));
            info0.Cell(2, 7).Merge(info0.Cell(1, 7));
            info0.Cell(2, 8).Merge(info0.Cell(1, 8));
            info0.Cell(1, 4).Merge(info0.Cell(1, 6));
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("    注：配电室内配电柜高压低于10KV，未对工频电场进行检测。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("6.2职业病危害因素对人体健康的影响");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    该用人单位生产过程中产生的主要职业病危害因素对人体健康的影响详见表6-2。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表6-2  主要职业病危害因素对人体健康的影响分析一览表");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info3 = m_doc.Tables.Add(m_wordApp.Selection.Range, ParmeterChemicalModel.Count +1, 5);//职业病危害因素，从用户界面选择的数据里面来。循环写到表格里面。
            info3.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info3.Borders.Enable = 1;
            info3.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info3.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info3.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info3.Columns[1].Width = 90;
            info3.Columns[2].Width = 120;
            info3.Rows[1].Height = 20;
            info3.Cell(1, 1).Select();
            SetCellHeaderText("职业病危害因素");
            info3.Cell(1, 2).Select();
            SetCellHeaderText("可能产生的职业病");
            info3.Cell(1, 3).Select();
            SetCellHeaderText("对人体主要危害");
            info3.Cell(1, 4).Select();
            SetCellHeaderText("职业接触限值");
            info3.Cell(1, 5).Select();
            SetCellHeaderText("职业禁忌证");
            //循环数据源，写到表格里面
            m_wordApp.Selection.TypeParagraph();
            int i = 2;
            foreach (var item in ParmeterChemicalModel)
            {
                
                string temp="";
                info3.Cell(i, 1).Select();
                SetCellHeaderText(item.ParameterName);                           
                foreach (var item1 in OccupationaldiseaseHarm)
                {
                    
                    if (item1.Endanger == item.ParameterName)
                    {
                        temp = item1.Disease;
                        break;
                    }
                }
                info3.Cell(i, 2).Select();
                SetCellHeaderText(temp);
                temp = "";
                foreach (var item1 in OccupationaldiseaseHarm)
                {
                    if (item1.Endanger == item.ParameterName)
                    {
                        temp = item1.Describe;
                        break;
                    }
                }
                info3.Cell(i, 3).Select();
                SetCellHeaderText(temp);
                info3.Cell(i, 4).Select();
                SetCellHeaderText("PC-TWA:" + item.TimeWeightingAverageAllowConcentration + "mg/m3");
                temp = "";
                foreach (var item1 in OccupationaldiseaseHarm)
                {
                    if (item1.Endanger == item.ParameterName)
                    {
                        temp = item1.Contraindication;
                        break;
                    }
                }
                info3.Cell(i, 5).Select();
                SetCellHeaderText(temp);
                i++;

            }
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("6.3职业病危害因素检测结果与评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("6.3.1职业病危害因素");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("   ");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("6.3.2检测方法及所用仪器");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    对职业病危害因素进行检测分析，相关检测仪器及检测方法严格根据有关标准进行，具体见表6-3。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表6-3  检测仪器和检测方法一览表");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info4 = m_doc.Tables.Add(m_wordApp.Selection.Range, 5, 5);//检测仪器，从用户界面使用设备数据里面来。循环写到表格里面。
            info4.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info4.Borders.Enable = 1;
            info4.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info4.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info4.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info4.Columns[1].Width = 30;
            info4.Columns[2].Width = 120;
            info4.Rows[1].Height = 20;
            info4.Cell(1, 1).Select();
            SetCellHeaderText("序号");
            info4.Cell(1, 2).Select();
            SetCellHeaderText("职业病危害因素");
            info4.Cell(1, 3).Select();
            SetCellHeaderText("检测方法");
            info4.Cell(1, 4).Select();
            SetCellHeaderText("检测仪器");
            info4.Cell(1, 5).Select();
            SetCellHeaderText("检测依据");
            //循环数据源，写到表格里面
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格


            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("6.3.3化学有害因素检测结果与评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("⑴粉尘检测结果及评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    该用人单位粉尘检测结果及评价具体见表6-4。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表6-4  粉尘（总尘）检测结果及评价");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info5 = m_doc.Tables.Add(m_wordApp.Selection.Range, 3, 9);
            info5.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info5.Borders.Enable = 1;
            info5.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info5.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info5.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info5.Columns[1].Width = 30;
            info5.Columns[2].Width = 120;
            info5.Rows[1].Height = 20;
            info5.Cell(1, 1).Select();
            SetCellHeaderText("评价单元");
            info5.Cell(1, 2).Select();
            SetCellHeaderText("工种");
            info5.Cell(1, 3).Select();
            SetCellHeaderText("检测地点");
            info5.Cell(1, 4).Select();
            SetCellHeaderText("粉尘性质");
            info5.Cell(1, 5).Select();
            SetCellHeaderText("接触时间（h/d）");
            info5.Cell(1, 2).Select();
            SetCellHeaderText("检测结果（mg/m3）");
            info5.Cell(1, 3).Select();
            SetCellHeaderText("CTWA（mg/m3）");
            info5.Cell(1, 4).Select();
            SetCellHeaderText("PC-TWA（mg/m3）");
            info5.Cell(1, 5).Select();
            SetCellHeaderText("评价");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("    由上表可知，该用人单位          作业过程中接触粉尘浓度符合国家职业卫生接触限值的要求。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("⑵化学有毒物质检测结果及评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    该用人单位化学有毒物质检测结果及评价见表6-5~6-8。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表6-5  化学有毒物质检测结果及评价");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info6 = m_doc.Tables.Add(m_wordApp.Selection.Range, 5, 10);
            info6.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info6.Borders.Enable = 1;
            info6.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info6.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info6.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info6.Columns[1].Width = 30;
            info6.Columns[2].Width = 120;
            info6.Rows[1].Height = 20;
            info6.Cell(1, 1).Select();
            SetCellHeaderText("化学物质");
            info6.Cell(1, 2).Select();
            SetCellHeaderText("工种");
            info6.Cell(1, 3).Select();
            SetCellHeaderText("检测地点");
            info6.Cell(1, 4).Select();
            SetCellHeaderText("接触时间（h/d）");
            info6.Cell(1, 5).Select();
            SetCellHeaderText("检测结果（mg/m3）");
            info6.Cell(1, 8).Select();
            SetCellHeaderText("接触限值（mg/m3");
            info6.Cell(1, 10).Select();
            SetCellHeaderText("评价");
            info6.Cell(2, 10).Merge(info6.Cell(1, 10));
            info6.Cell(1, 8).Merge(info6.Cell(1, 9));
            info6.Cell(1, 5).Merge(info6.Cell(1, 7));
            info6.Cell(2, 5).Select();
            SetCellHeaderText("检测值");
            info6.Cell(2, 6).Select();
            SetCellHeaderText("CTWA");
            info6.Cell(2, 7).Select();
            SetCellHeaderText("CSTEL");
            info6.Cell(2, 8).Select();
            SetCellHeaderText("PC-TWA");
            info6.Cell(2, 9).Select();
            SetCellHeaderText("PC-STEL");
            info6.Cell(2, 1).Merge(info6.Cell(1, 1));
            info6.Cell(2, 2).Merge(info6.Cell(1, 2));
            info6.Cell(2, 3).Merge(info6.Cell(1, 3));
            info6.Cell(2, 4).Merge(info6.Cell(1, 4));
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeParagraph();


            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表6-6  溶剂汽油、二氧化锰检测结果及评价");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info7 = m_doc.Tables.Add(m_wordApp.Selection.Range, 5, 10);
            info7.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info7.Borders.Enable = 1;
            info7.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info7.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info7.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info7.Columns[1].Width = 30;
            info7.Columns[2].Width = 120;
            info7.Rows[1].Height = 20;
            info7.Cell(1, 1).Select();
            SetCellHeaderText("化学物质");
            info7.Cell(1, 2).Select();
            SetCellHeaderText("工种");
            info7.Cell(1, 3).Select();
            SetCellHeaderText("检测地点");
            info7.Cell(1, 4).Select();
            SetCellHeaderText("接触时间（h/d）");
            info7.Cell(1, 5).Select();
            SetCellHeaderText("检测结果（mg/m3）");
            info7.Cell(1, 8).Select();
            SetCellHeaderText("接触限值（mg/m3");
            info7.Cell(1, 10).Select();
            SetCellHeaderText("评价");
            info7.Cell(2, 5).Select();
            SetCellHeaderText("检测值");
            info7.Cell(2, 6).Select();
            SetCellHeaderText("CTWA");
            info7.Cell(2, 7).Select();
            SetCellHeaderText("CSTEL");
            info7.Cell(2, 8).Select();
            SetCellHeaderText("PC-TWA");
            info7.Cell(2, 9).Select();
            SetCellHeaderText("PC-STEL");
            info7.Cell(2, 1).Merge(info7.Cell(1, 1));
            info7.Cell(2, 2).Merge(info7.Cell(1, 2));
            info7.Cell(2, 3).Merge(info7.Cell(1, 3));
            info7.Cell(2, 4).Merge(info7.Cell(1, 4));
            info7.Cell(2, 10).Merge(info7.Cell(1, 10));
            info7.Cell(1, 8).Merge(info7.Cell(1, 9));
            info7.Cell(1, 5).Merge(info7.Cell(1, 7));
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表6-7  臭氧检测结果及评价");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info8 = m_doc.Tables.Add(m_wordApp.Selection.Range, 5, 7);
            info8.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info8.Borders.Enable = 1;
            info8.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info8.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info8.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info8.Columns[1].Width = 30;
            info8.Columns[2].Width = 120;
            info8.Rows[1].Height = 20;
            info8.Cell(1, 1).Select();
            SetCellHeaderText("评价单元");
            info8.Cell(1, 2).Select();
            SetCellHeaderText("工种");
            info7.Cell(1, 3).Select();
            SetCellHeaderText("检测地点");
            info8.Cell(1, 4).Select();
            SetCellHeaderText("接触时间（h/d）");
            info8.Cell(1, 5).Select();
            SetCellHeaderText("检测值（mg/m3）");
            info8.Cell(1, 6).Select();
            SetCellHeaderText("MAC（mg/m3）");
            info8.Cell(1, 7).Select();
            SetCellHeaderText("评价");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("    由上表可知，该用人单位各工作场化学有毒物质浓度均符合国家职业卫生接触限值的要求。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("6.3.4物理因素检测结果与评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("⑴噪声检测结果及评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    该用人单位噪声检测结果及评价具体见表6-8。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表6-8  噪声检测结果及评价");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info9 = m_doc.Tables.Add(m_wordApp.Selection.Range, 5, 8);
            info9.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info9.Borders.Enable = 1;
            info9.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info9.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info9.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info9.Columns[1].Width = 30;
            info9.Columns[2].Width = 120;
            info9.Rows[1].Height = 20;
            info9.Cell(1, 1).Select();
            SetCellHeaderText("评价单元");
            info9.Cell(1, 2).Select();
            SetCellHeaderText("工种");
            info9.Cell(1, 3).Select();
            SetCellHeaderText("检测地点");
            info9.Cell(1, 4).Select();
            SetCellHeaderText("接触时间（h/d）");
            info9.Cell(1, 5).Select();
            SetCellHeaderText("检测值（mg/m3）");
            info9.Cell(1, 6).Select();
            SetCellHeaderText("8h等效声级dB（A）");
            info9.Cell(1, 6).Select();
            SetCellHeaderText("接触限值dB（A）");
            info9.Cell(1, 7).Select();
            SetCellHeaderText("评价");
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
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();


            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("    由上表检测数据可知，该用人单位各工作场所维修人员接触噪声8h等效声级均符合国家职业卫生接触限值的要求。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("⑵紫外辐射检测结果及评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    该用人单位紫外辐射检测结果及评价见表6-9。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表6-9  紫外辐射检测结果及评价");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info10 = m_doc.Tables.Add(m_wordApp.Selection.Range, 5, 8);
            info10.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info10.Borders.Enable = 1;
            info10.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info10.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info10.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info10.Columns[1].Width = 30;
            info10.Columns[2].Width = 120;
            info10.Rows[1].Height = 20;
            info10.Cell(1, 1).Select();
            SetCellHeaderText("工种");
            info10.Cell(1, 2).Select();
            SetCellHeaderText("检测地点");
            info10.Cell(1, 3).Select();
            SetCellHeaderText("测量部位");
            info10.Cell(1, 4).Select();
            SetCellHeaderText("紫外光谱分类");
            info10.Cell(1, 5).Select();
            SetCellHeaderText("辐射度μW/cm2");
            info10.Cell(1, 6).Select();
            SetCellHeaderText("职业接触限值μW/cm2");
            info10.Cell(1, 7).Select();
            SetCellHeaderText("评价");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("    由上表检测数据可知，该用人单位钣金工电焊过程接触紫外辐射的强度符合国家职业卫生接触限值的要求。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("6.3.5职业病危害因素超标原因分析");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("   ");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("6.3.6职业病危害评价结论");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            //物理因素和化学因素的列表
            m_wordApp.Selection.TypeText("    该用人单位钣金工接触      的8h加权平均浓度均符合《工作场所有害因素职业接触限值 第1部分：化学有害因素》（GBZ2.1-2007）的相关要求；");
            m_wordApp.Selection.TypeText("    所测      物理因素噪声8h等效声级均符合《工作场所有害因素职业接触限值 第2部分：物理因素》（GBZ2.2-2007）的规定。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("6.4职业病危害因素关键控制点");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    该用人单位关键控制点见表6-12。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表6-12  关键控制点一览表");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info11 = m_doc.Tables.Add(m_wordApp.Selection.Range, 5, 2);
            info11.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info11.Borders.Enable = 1;
            info11.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info11.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info11.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info11.Columns[1].Width = 220;
            info11.Columns[2].Width = 220;
            info11.Rows[1].Height = 20;
            info11.Cell(1, 1).Select();
            SetCellHeaderText("职业危害因素");
            info11.Cell(1, 2).Select();
            SetCellHeaderText("关键控制点");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();
        }

        //7职业病防护设施和应急救援设施调查与评价
        private void WriteEmergency()
        {
            m_wordApp.Selection.Font.Size = 16;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("7职业病防护设施和应急救援设施调查与评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("7.1职业病危害防护设施和应急救援设施的设置情况");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("7.1.1防尘、防毒");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("   ");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("7.1.2噪声与振动控制");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("   ");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("7.1.3防寒、防暑");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("   ");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("7.1.4防工频电场");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("   ");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("7.1.5应急救援设施");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("   ");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("7.1.6防护设施防护能力调查和检测");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    各车间内设备按维修流程");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    综合职业病危害因素浓度和强度的检测结果可知，该项目设置的职业病防护设施合理有效，符合《工业企业设计卫生标准》（GBZ1-2010）等相关标准、规范的要求。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("7.2职业病防护设施的维护情况");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    用人单位制定了");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("7.3职业病防护设施和应急救援设施评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    通过对该用人单位各工作场所职业危害防护技术措施的调查，依据《工业企业设计卫生标准》（GBZ1-2010）制定检查表对其职业危害防护设施进行评价，见表7-1。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表7-1  职业病防护设施和应急救援设施评价检查表");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info0 = m_doc.Tables.Add(m_wordApp.Selection.Range, 12, 5);
            info0.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info0.Borders.Enable = 1;
            info0.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info0.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info0.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info0.Columns[1].Width = 30;
            info0.Columns[2].Width = 120;
            info0.Rows[1].Height = 20;
            info0.Cell(1, 1).Select();
            SetCellHeaderText("序号");
            info0.Cell(1, 2).Select();
            SetCellHeaderText("检查内容");
            info0.Cell(1, 3).Select();
            SetCellHeaderText("检查依据");
            info0.Cell(1, 4).Select();
            SetCellHeaderText("检查结果");
            info0.Cell(1, 5).Select();
            SetCellHeaderText("评价");

            info0.Rows[2].Height = 100;
            info0.Cell(2, 1).Select();
            SetCellHeaderText("1");
            info0.Cell(2, 2).Select();
            SetCellHeaderText("对产生粉尘、毒物的生产过程和设备(含露天作业的工艺设备)，应优先采用机械化和自动化，避免直接人工操作。");
            info0.Cell(2, 3).Select();
            SetCellHeaderText("《工业企业设计卫生标准》（GBZ1-2010）6.1.1.2");


            info0.Rows[3].Height = 120;
            info0.Cell(3, 1).Select();
            SetCellHeaderText("2");
            info0.Cell(3, 2).Select();
            SetCellHeaderText("在生产种可能突然逸出大量有害物质或易造成急性中毒或易燃易爆的化学物质的室内作业场所，应设置事故通风装置及与事故排风系统相连锁的装置。");
            info0.Cell(3, 3).Select();
            SetCellHeaderText("《工业企业设计卫生标准》（GBZ1-2010）6.1.5.2");

            info0.Rows[4].Height = 120;
            info0.Cell(4, 1).Select();
            SetCellHeaderText("3");
            info0.Cell(4, 2).Select();
            SetCellHeaderText("应结合生产工艺和毒物特性，再有可能发生急性职业中毒的工作场所，根据自动报警装置技术发展水平设计自动报警。");
            info0.Cell(4, 3).Select();
            SetCellHeaderText("《工业企业设计卫生标准》（GBZ1-2010）6.1.6");

            info0.Rows[5].Height = 120;
            info0.Cell(5, 1).Select();
            SetCellHeaderText("4");
            info0.Cell(5, 2).Select();
            SetCellHeaderText("可能存在或产生有毒物质的工作场所应根据有毒物质的理化特性和危害特点配备现场急救用品，设置冲洗喷淋设备、应急撤离通道、必要的泄险区以及风向标。");
            info0.Cell(5, 3).Select();
            SetCellHeaderText("《工业企业设计卫生标准》（GBZ1-2010）6.1.7");


            info0.Rows[6].Height = 120;
            info0.Cell(6, 1).Select();
            SetCellHeaderText("5");
            info0.Cell(6, 2).Select();
            SetCellHeaderText("产生噪声的车间与非噪声作业车间、高噪声车间与低噪声车间应分开布置。");
            info0.Cell(6, 3).Select();
            SetCellHeaderText("《工业企业设计卫生标准》（GBZ1-2010）6.3.1.2");

            info0.Rows[7].Height = 120;
            info0.Cell(7, 1).Select();
            SetCellHeaderText("6");
            info0.Cell(7, 2).Select();
            SetCellHeaderText("在满足工艺流程要求的前提下，宜将高噪声设备相对集中，并采取相应的隔声、消声、吸声减振等控制措施。");
            info0.Cell(7, 3).Select();
            SetCellHeaderText("《工业企业设计卫生标准》（GBZ1-2010）6.3.1.4");

            info0.Rows[8].Height = 120;
            info0.Cell(8, 1).Select();
            SetCellHeaderText("7");
            info0.Cell(8, 2).Select();
            SetCellHeaderText("对于在生产过程中有可能产生非电力辐射的设备，应制定非电离辐射防护规划，采取有效的屏蔽、接地、吸收等工程集输措施及自动化或半自动化远距离操作，如预期不能屏蔽的应设计反射性隔离或吸收性隔离措施，使劳动者非电离辐射作业的接触水平符合GBZ2.2的要求。");
            info0.Cell(8, 3).Select();
            SetCellHeaderText("《工业企业设计卫生标准》（GBZ1-2010）6.4.4");

            info0.Rows[9].Height = 120;
            info0.Cell(9, 1).Select();
            SetCellHeaderText("8");
            info0.Cell(9, 2).Select();
            SetCellHeaderText("建立、健全职业病危害事故应急救援预案。");
            info0.Cell(9, 3).Select();
            SetCellHeaderText("《中华人民共和国职业病防治法》第二十一条（六）");

            info0.Rows[10].Height = 120;
            info0.Cell(10, 1).Select();
            SetCellHeaderText("9");
            info0.Cell(10, 2).Select();
            SetCellHeaderText("存在或使用有毒气体，并可能导致劳动者发生急性职业中毒的工作场所，应设有毒气体检测报警点。");
            info0.Cell(10, 3).Select();
            SetCellHeaderText("《工作场所有毒气体检测报警装置设置规范》GBZ/T2234.1.1");

            info0.Rows[11].Height = 120;
            info0.Cell(11, 1).Select();
            SetCellHeaderText("10");
            info0.Cell(11, 2).Select();
            SetCellHeaderText("急救箱应当设置在便于劳动者取用的地点，配备内容可根据实际需要参照《工业企业设计卫生标准》（GBZ1-2010）附录A表A.4确定，并由专人负责定期检查和更新。");
            info0.Cell(11, 3).Select();
            SetCellHeaderText("《工业企业设计卫生标准》（GBZ1-2010）8.3.3");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("    对该用人单位工作场所防护技术措施共检查10项内容，均符合上述标准、规范的相关要求。");
            m_wordApp.Selection.TypeParagraph();
        }

        //8职业健康监护
        private void WriteOccupationalHealth()
        {
            m_wordApp.Selection.Font.Size = 16;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("8职业健康监护");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("8.1职业健康监护情况");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("   建立了劳动者职业健康监护档案，档案中包括了劳动者的基本信息、职业史、职业病危害接触史、职业健康检查结果和职业病诊疗等相关资料。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("检查单位：");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("资质证号：");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("检查时间：");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("检查类别：");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("检查种类：");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("体检人数：");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("检查项目：");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("职业健康体检结果：");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("1、本次体检从事苯系化合物作业人员岗前  人，未发现目标疾病，可继续从事苯系工作；本次体检从事苯系化合物作业人员在岗  人，未发现目标疾病，可继续从事原苯系工作。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("2、本次体检噪声作业人员在岗  人，未发现目标疾病，可继续原噪声工作。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("3、本次体检岗前粉尘作业人员  人，未发现目标疾病，可从事粉尘工作；本次体检从事粉尘作业人员在岗  人，发现目标疾病1人（杨金柱）肺功能轻度阻塞型通气功能障碍，建议两周内来我院门诊复查肺功能，其余3人未发现目标疾病，可继续原粉尘工作。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("8.2职业健康监护评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    依据《用人单位职业健康监护监督管理办法》（国家安全监督管理总局第49号令）、《职业健康监护技术规范》（GBZ188-2014）等法律、法规和标准、规范的要求，对该用人单位职业健康监护情况进行评价，职业健康监护评价检查表见表8-1。");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表8-1  职业健康监护评价检查表");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info0 = m_doc.Tables.Add(m_wordApp.Selection.Range, 12, 5);
            info0.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info0.Borders.Enable = 1;
            info0.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info0.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info0.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info0.Columns[1].Width = 30;
            info0.Columns[2].Width = 120;
            info0.Rows[1].Height = 20;
            info0.Cell(1, 1).Select();
            SetCellHeaderText("序号");
            info0.Cell(1, 2).Select();
            SetCellHeaderText("检查内容");
            info0.Cell(1, 3).Select();
            SetCellHeaderText("检查依据");
            info0.Cell(1, 4).Select();
            SetCellHeaderText("检查结果");
            info0.Cell(1, 5).Select();
            SetCellHeaderText("评价");

            info0.Rows[2].Height = 100;
            info0.Cell(2, 1).Select();
            SetCellHeaderText("1");
            info0.Cell(2, 2).Select();
            SetCellHeaderText("用人单位应当依照本办法以及《职业健康监护技术规范》（GBZ188-2014）等国家职业卫生标准的要求，制定、落实本单位职业健康检查年度计划，并保证所需要的专项经费。");
            info0.Cell(2, 3).Select();
            SetCellHeaderText("《用人单位职业健康监护监督管理办法》（国家安全监督管理总局第49号令）第七条");


            info0.Rows[3].Height = 120;
            info0.Cell(3, 1).Select();
            SetCellHeaderText("2");
            info0.Cell(3, 2).Select();
            SetCellHeaderText("用人单位应当组织劳动者进行职业健康检查，并承担职业健康检查费用。");
            info0.Cell(3, 3).Select();
            SetCellHeaderText("《用人单位职业健康监护监督管理办法》（国家安全监督管理总局第49号令）第八条");

            info0.Rows[4].Height = 120;
            info0.Cell(4, 1).Select();
            SetCellHeaderText("3");
            info0.Cell(4, 2).Select();
            SetCellHeaderText("用人单位应当选择由省级以上人民政府卫生行政部门批准的医疗卫生机构承担职业健康检查工作，并确保参加职业健康检查的劳动者身份的真实性。");
            info0.Cell(4, 3).Select();
            SetCellHeaderText("《用人单位职业健康监护监督管理办法》（国家安全监督管理总局第49号令）第九条");

            info0.Rows[5].Height = 120;
            info0.Cell(5, 1).Select();
            SetCellHeaderText("4");
            info0.Cell(5, 2).Select();
            SetCellHeaderText("用人单位应当对下列劳动者进行上岗前的职业健康检查：(一)拟从事接触职业病危害作业的新录用劳动者，包括转岗到该作业岗位的劳动者；(二)拟从事有特殊健康要求作业的劳动者。");
            info0.Cell(5, 3).Select();
            SetCellHeaderText("《用人单位职业健康监护监督管理办法》（国家安全监督管理总局第49号令）第十一条");


            info0.Rows[6].Height = 120;
            info0.Cell(6, 1).Select();
            SetCellHeaderText("5");
            info0.Cell(6, 2).Select();
            SetCellHeaderText("用人单位应当及时将职业健康检查结果及职业健康检查机构的建议以书面形式如实告知劳动者。");
            info0.Cell(6, 3).Select();
            SetCellHeaderText("《用人单位职业健康监护监督管理办法》（国家安全监督管理总局第49号令）第十六条");

            info0.Rows[7].Height = 120;
            info0.Cell(7, 1).Select();
            SetCellHeaderText("6");
            info0.Cell(7, 2).Select();
            SetCellHeaderText("对需要复查的劳动者，按照职业健康检查机构要求的时间安排复查和医学观察。");
            info0.Cell(7, 3).Select();
            SetCellHeaderText("《用人单位职业健康监护监督管理办法》（国家安全监督管理总局第49号令）第十七条（三）");

            info0.Rows[8].Height = 120;
            info0.Cell(8, 1).Select();
            SetCellHeaderText("7");
            info0.Cell(8, 2).Select();
            SetCellHeaderText("用人单位应当为劳动者个人建立职业健康监护档案，并按照有关规定妥善保存。");
            info0.Cell(8, 3).Select();
            SetCellHeaderText("《用人单位职业健康监护监督管理办法》（国家安全监督管理总局第49号令）第十九条");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("    对该用人单位职业健康监护情况进行调查与评价，共检查7项内容，除杨金柱未进行肺功能复查外其余均符合上述标准、规范的相关要求。");
            m_wordApp.Selection.TypeParagraph();
        }

        //9个人防护用品调查与评价
        private void WriteHealthDevice()
        {
            m_wordApp.Selection.Font.Size = 16;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("9个人防护用品调查与评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("9.1个人防护用品调查");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    该用人单位为维修人员发放");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    职业病防护用品配备情况见表9-1。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表9-1  个人使用职业病防护用品配备情况一览表");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info0 = m_doc.Tables.Add(m_wordApp.Selection.Range, 3, 6);
            info0.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info0.Borders.Enable = 1;
            info0.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info0.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info0.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info0.Columns[1].Width = 30;
            info0.Columns[2].Width = 120;
            info0.Rows[1].Height = 20;
            info0.Cell(1, 1).Select();
            SetCellHeaderText("评价单元");
            info0.Cell(1, 2).Select();
            SetCellHeaderText("作业地点");
            info0.Cell(1, 3).Select();
            SetCellHeaderText("岗位/工种");
            info0.Cell(1, 4).Select();
            SetCellHeaderText("接触职业病危害因素");
            info0.Cell(1, 5).Select();
            SetCellHeaderText("防护用品名称");
            info0.Cell(1, 6).Select();
            SetCellHeaderText("发放周期");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("    该用人单位制定了《职业病防护用品管理制度》，规定了防护用品的发放规定及标准、使用规范、更换报废等内容，维修人员进行车辆维修作业必须严格佩戴个人防护用品。用人单位人事行政部根据劳动者接触职业病危害因素的特点进行配备个人防护用品，并留有记录。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("9.2个人防护用品评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    依据《中华人民共和国职业病防治法》、《工作场所职业卫生监督管理规定》（国家安全生产监督管理总局令第47号）、《个体防护装备选用规范》（GB/T11651-2008）、《生产过程安全卫生要求总则》（GB/T12801-2008）等的相关标准规定，制成检查表进行个人使用的职业病防护用品评价，见表9-2。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表9-2 个人职业病防护用品评价检查表");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info1 = m_doc.Tables.Add(m_wordApp.Selection.Range, 7, 5);
            info1.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info1.Borders.Enable = 1;
            info1.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info1.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info1.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info1.Columns[1].Width = 30;
            info1.Columns[2].Width = 120;
            info1.Rows[1].Height = 20;
            info1.Cell(1, 1).Select();
            SetCellHeaderText("序号");
            info1.Cell(1, 2).Select();
            SetCellHeaderText("检查内容");
            info1.Cell(1, 3).Select();
            SetCellHeaderText("检查依据");
            info1.Cell(1, 4).Select();
            SetCellHeaderText("检查结果");
            info1.Cell(1, 5).Select();
            SetCellHeaderText("评价");
            info1.Rows[2].Height = 120;
            info1.Cell(2, 1).Select();
            SetCellHeaderText("1");
            info1.Cell(2, 2).Select();
            SetCellHeaderText("用人单位应当为劳动者提供符合国家职业卫生标准的职业病防护用品，并督促、指导劳动者按照使用规则正确佩戴、使用，不得发放钱物替代发放职业病防护用品。");
            info1.Cell(2, 3).Select();
            SetCellHeaderText("《工作场所职业卫生监督管理规定》（国家安全生产监督管理总局第47号）第十六条");

            info1.Rows[3].Height = 20;
            info1.Cell(3, 1).Select();
            SetCellHeaderText("2");
            info1.Cell(3, 2).Select();
            SetCellHeaderText("在强光作业场所可以使用防强光、紫外线、红外线护目镜或面罩，焊接面罩，焊接手套，焊接防护鞋，焊接防护服，白帆布类隔热服。");
            info1.Cell(3, 3).Select();
            SetCellHeaderText("《个体防护装备选用规范》（GB/T11651-2008）");

            info1.Rows[4].Height = 120;
            info1.Cell(4, 1).Select();
            SetCellHeaderText("3");
            info1.Cell(4, 2).Select();
            SetCellHeaderText("在粉尘作业场所应佩戴防尘口罩等个人防护用品");
            info1.Cell(4, 3).Select();
            SetCellHeaderText("《个体防护装备选用规范》（GB/T 11651-2008）");

            info1.Rows[5].Height = 120;
            info1.Cell(5, 1).Select();
            SetCellHeaderText("4");
            info1.Cell(5, 2).Select();
            SetCellHeaderText("在噪声作业场所应佩戴耳塞等防护用品，建议使用耳罩");
            info1.Cell(5, 3).Select();
            SetCellHeaderText("《个体防护装备选用规范》（GB/T 11651-2008）");

            info1.Rows[6].Height = 120;
            info1.Cell(6, 1).Select();
            SetCellHeaderText("5");
            info1.Cell(6, 2).Select();
            SetCellHeaderText("企业应当建立健全劳动防护用品的采购、保管、发放、使用、报废等管理制度。");
            info1.Cell(6, 3).Select();
            SetCellHeaderText("《生产过程安全卫生要求总则》（GB/T12801-2008）6.2.5");
            info1.Rows[7].Height = 120;
            info1.Cell(7, 1).Select();
            SetCellHeaderText("6");
            info1.Cell(7, 2).Select();
            SetCellHeaderText("用人单位必须采用有效的职业病防护设施，并为劳动者提供个人使用的职业病防护用品。用人单位为劳动者个人提供的职业病防护用品必须符合防治职业病的要求；不符合要求的，不得使用。");
            info1.Cell(7, 3).Select();
            SetCellHeaderText("《中华人民共和国职业病防治法》第二十三条");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("   对该用人单位的个人职业病防护用品进行检查，本次共检查6项内容，均符合上述标准、规范的相关要求。");
            m_wordApp.Selection.TypeParagraph();

        }
        //10辅助用室调查与评价
        private void WriteAuxiliaryRoom()
        {
            m_wordApp.Selection.Font.Size = 16;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("10辅助用室调查与评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("10.1辅助用室调查");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    该用人单位设有休息室、厨房、食堂、厕所、盥洗室、更衣室等辅助用室，休息室内设有饮水机，卫生间内设有洗手池。辅助用室的设置见表10-1。");
            m_wordApp.Selection.TypeParagraph();


            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表10-1  辅助用室设置一览表");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info0 = m_doc.Tables.Add(m_wordApp.Selection.Range, 3, 5);
            info0.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info0.Borders.Enable = 1;
            info0.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info0.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info0.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info0.Columns[1].Width = 30;
            info0.Columns[2].Width = 120;
            info0.Rows[1].Height = 20;
            info0.Cell(1, 1).Select();
            SetCellHeaderText("评价单元");
            info0.Cell(1, 2).Select();
            SetCellHeaderText("设置地点");
            info0.Cell(1, 3).Select();
            SetCellHeaderText("辅助用室");
            info0.Cell(1, 4).Select();
            SetCellHeaderText("卫生设施");
            info0.Cell(1, 5).Select();
            SetCellHeaderText("数量（个）");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("    该项目对工作场所按照3级标准进行管理，设置的辅助用室合理，符合《工业企业设计卫生标准》（GBZ 1-2010）等法律、法规的相关要求。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("10.2辅助用室评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    依据《工业企业设计卫生标准》（GBZ1-2010）的相关规定，制成检查表对辅助用室进行评价，见表10-2。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表10-2  辅助用室评价检查表");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info1 = m_doc.Tables.Add(m_wordApp.Selection.Range, 7, 5);
            info1.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info1.Borders.Enable = 1;
            info1.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info1.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info1.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info1.Columns[1].Width = 30;
            info1.Columns[2].Width = 120;
            info1.Rows[1].Height = 20;
            info1.Cell(1, 1).Select();
            SetCellHeaderText("序号");
            info1.Cell(1, 2).Select();
            SetCellHeaderText("检查内容");
            info1.Cell(1, 3).Select();
            SetCellHeaderText("检查依据");
            info1.Cell(1, 4).Select();
            SetCellHeaderText("检查结果");
            info1.Cell(1, 5).Select();
            SetCellHeaderText("评价");
            info1.Rows[2].Height = 120;
            info1.Cell(2, 1).Select();
            SetCellHeaderText("1");
            info1.Cell(2, 2).Select();
            SetCellHeaderText("应依据工业企业生产特点、实际需要和使用方便的原则设置辅助用室，包括车间卫生用室（浴室、更/存衣室、盥洗室以及在特殊作业、工种或岗位设置的洗衣室）、生活室（休息室、就餐场所、厕所）、妇女卫生室，并应符合相应卫生标准。");
            info1.Cell(2, 3).Select();
            SetCellHeaderText("《工业企业设计卫生标准》（GBZ1-2010）7.1.1");

            info1.Rows[3].Height = 20;
            info1.Cell(3, 1).Select();
            SetCellHeaderText("2");
            info1.Cell(3, 2).Select();
            SetCellHeaderText("辅助用室应避开有害物质、病原体、高温等职业性有害因素的影响，建筑物内部构造应易于清扫，卫生设施便于使用。");
            info1.Cell(3, 3).Select();
            SetCellHeaderText("《工业企业设计卫生标准》（GBZ1-2010）7.1.2");

            info1.Rows[4].Height = 120;
            info1.Cell(4, 1).Select();
            SetCellHeaderText("3");
            info1.Cell(4, 2).Select();
            SetCellHeaderText("车间卫生特征3级的更/存衣室，便服室、工作服室可按照同柜分层存放的原则设计。更衣室与休息室可合并设置。");
            info1.Cell(4, 3).Select();
            SetCellHeaderText("《工业企业设计卫生标准》（GBZ1-2010）7.2.3.3");

            info1.Rows[5].Height = 120;
            info1.Cell(5, 1).Select();
            SetCellHeaderText("4");
            info1.Cell(5, 2).Select();
            SetCellHeaderText("应根据生产特点和实际需要设置休息室或休息区。休息室内应设置清洁饮水设施。");
            info1.Cell(5, 3).Select();
            SetCellHeaderText("《工业企业设计卫生标准》（GBZ1-2010）第7.3.2条");

            info1.Rows[6].Height = 120;
            info1.Cell(6, 1).Select();
            SetCellHeaderText("5");
            info1.Cell(6, 2).Select();
            SetCellHeaderText("厕所不宜距工作地点过远，并应有排臭、防蝇措施。车间内的厕所，一般应为水冲式，同时应设洗手池、洗污池。寒冷地区宜设在室内。");
            info1.Cell(6, 3).Select();
            SetCellHeaderText("《工业企业设计卫生标准》（GBZ1-2010）第7.3.4条");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("    对该用人单位的辅助用室设置情况进行检查，本次共检查5项内容，均符合上述标准、规范的相关要求。");
            m_wordApp.Selection.TypeParagraph();
        }

        //11职业卫生管理情况调查与评价
        private void WriteHealthManagement()
        {
            m_wordApp.Selection.Font.Size = 16;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("11职业卫生管理情况调查与评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("11.1职业卫生管理组织机构及人员");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    为加强职业卫生管理工作，该用人单位根据《工作场所职业卫生监督管理规定》（安全生产监督管理总局第47号令）的要求设置了职业卫生管理机构（人事行政部和售后部），配备2名兼职职业卫生管理人员负责职业卫生管理工作。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("11.2职业病防治规划、实施方案及执行情况");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    该用人单位制定了《职业病防治计划与实施方案》，其中包括职业病防护设施配备与维护、工作场所职业病危害因素检测与评价、个人使用的职业病防护用品配置与更换、警示标识设置、职业卫生培训、职业健康体检及职业病人诊疗等内容。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    该用人单位制定的职业病防治计划和实施方案内容全面，严格按照计划和方案的规定落实职业卫生管理工作。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("11.3职业卫生管理制度和操作规程及执行情况");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("11.3.1职业病危害防治责任制度");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    其中制定了主要负责人、职业卫生负责人、售后部及其负责人、职业卫生管理部门、兼职职业卫生管理人员及员工的职责；制定了建设项目职业卫生“三同时”管理、职业病危害警示及告知、作业场所监测、职业健康教育培训、职业健康检查、个人防护用品发放及管理等的管理程序。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    该用人单位严格按照制定的职业健康管理程序逐步落实职业卫生管理工作，制度较全面，落实情况良好。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("11.3.2职业病危害警示与告知制度");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    该用人单位制定了《职业病危害警示与告知制度》，与从事接触职业危害岗位的新进员工签订劳动合同时，将工作过程中可能产生的职业危害及其后果、职业病防护措施和待遇等进行告知，并在合同中写明；未与在岗员工签订职业病危害劳动告知合同或岗位（工作内容）发生变动的员工，按国家职业病危害防治法律、法规的相关规定与员工进行补签；在醒目位置设置公告栏，公布有关职业病危害防治的规章制度、操作规程、职业病危害事故应急救援措施以及作业场所职业病危害因素检测和评价的结果等内容；人事行政部每年对员工进行职业病危害预防控制培训、考核，定期委托有资质单位对作业场所的职业病危害因素进行检测、评价，并将结果上报安全生产监督管理部门。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    该用人单位在工作场所醒目的位置设置了警示标示及中文警示说明和告知栏，设置情况见表11-1。");
            m_wordApp.Selection.TypeParagraph();


            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表11-1  警示标识设置情况一览表");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info0 = m_doc.Tables.Add(m_wordApp.Selection.Range, 3, 3);
            info0.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info0.Borders.Enable = 1;
            info0.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info0.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info0.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info0.Columns[1].Width = 120;
            info0.Columns[2].Width = 120;
            info0.Rows[1].Height = 20;
            info0.Cell(1, 1).Select();
            SetCellHeaderText("评价单元");
            info0.Cell(1, 2).Select();
            SetCellHeaderText("设置地点");
            info0.Cell(1, 3).Select();
            SetCellHeaderText("警示标识内容");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("11.3.3职业病危害项目申报制度");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    该用人单位制定了《职业病危害项目申报制度》，规定申报工作主要由人事行政部负责、售后部配合，目前用人单位已经将职业病危害情况向所属地安全生产监督管理部门进行了申报。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("11.3.4职业病防治宣传教育培训制度");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    该用人单位制定了《职业病防治宣传教育培训制度》，新职工入厂后首先要进行上岗前的职业卫生培训，经考试合格后方可上岗作业。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    培训内容主要有职业卫生法律、法规与标准、职业卫生基本知识、职业卫生管理制度和操作规程、正确使用、维护职业病防护设备和个人使用的职业病防护用品、发生事故时的应急救援措施等，留有记录，并将培训记录存入职业卫生档案。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("11.3.5职业病防护设施维护检修制度");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    该用人单位制定了《职业病防护设施维护检修制度》，制度中规定存在职业病危害因素的作业场所使用的职业病防护设施由专人负责维护和保养；定期组织职业病防护设施正确使用和维护保养的教育培训；制定实施职业病防护设施检修计划和方案，定期检查并做好相关记录；人事行政部对职业病防护设施的运行情况进行一次检查，售后部每周对防护设施的运行情况进行检查，当班员工每天对设施运行情况进行检查；防护设施在检修时，做好现场监护和有关人员的协调和指挥工作，检修结束后，维护检修人员确认合格并签字交接。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    该用人单位对防护设施进行定期检查、维修、保养，保证防护设施正常运转，每年对防护设施的效果进行综合性检测，评定防护设施对职业病危害因素控制的效果，无擅自拆除或停用防护设施的情况。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("11.3.6个人职业病防护用品管理制度");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    该用人单位制定了《职业病防护用品管理制度》，制度中主要规定了劳动防护用品的发放、采购的要求，采购的劳动防护用品需符合国家标准的要求；人事行政部和售后部对员工正确地使用劳动防护用品进行教育和培训并负责职业病防护用品的采购标准。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    经过现场调查，现场劳动者均按照要求严格佩戴个体防护用品。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("11.3.7职业病危害监测及评价管理制度");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    该用人单位制定了《职业病危害检测及评价管理制度》，制度中规定由人事行政部负责制度的实施与监督，每年委托有资质的服务机构对作业场所的职业病危害因素进行检测评价，检测与评价结果及时向员工公布并上报区安全监管部门备案，存入职业卫生档案，目前该用人单位委托我公司对其作业场所进行职业病危害现状评价。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("11.3.8建设项目职业卫生“三同时”管理制度");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    该用人单位制定了《建设项目职业卫生“三同时”管理制度》，未开展建设项目职业病危害预评价和控制效果评价工作，根据《建设项目职业卫生“三同时”监督管理暂行办法》规定，2011年1月1日以前投产运行的，应当责令建设单位进行职业病危害现状评价，并参照控制效果评价程序申请备案，本次评价为该用人单位首次进行职业病危害现状评价，职业病防护设施所需费用纳入了公司年度预算。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("11.3.9职业卫生档案和劳动者职业健康监护及其档案管理制度");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("⑴职业卫生档案");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    该公司制定了《职业健康监护及其档案管理制度》，根据《国家安全监管总局办公厅关于印发职业卫生档案管理规范的通知》（安监总厅安健〔2013〕171号）的要求建立了职业卫生档案，将职业卫生管理制度以及相关的程序文件、作业文件、职业健康监护和劳动者职业健康监护档案等统一存放于人事行政部；个人防护用品的发放、职业健康体检、职业卫生培训、应急救援设施的配置、职业病危害因素检测安排等工作由人事行政部负责、各维修车间协助。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("⑵职业健康监护档案");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    该公司对维修车间作业人员进行了职业健康检查并建立了劳动者职业健康监护档案，档案中包括了劳动者的职业史、职业病危害接触史、职业健康检查结果和职业病诊疗等资料。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("11.3.10职业病危害事故与报告制度");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    该用人单位制定了《职业病危害事故处置与报告制度》，制度中规定了职业病危害事故分级、管理分工、事故处置与报告的程序等内容。人事行政部负责处理职业病危害事故、制定职业病危害事故的处置方案、事故疏散、记录、上报，售后部负责应急处理和救援。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("11.3.11职业病危害应急救援与管理制度");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    该用人单位制定了《职业病危害应急救援与管理制度》，规定人事行政部和售后部为职业病危害应急救援指挥机构，总经理为总指挥、售后部负责人为副总指挥；规定了职业病危害事故各机构的职责、人员分工，制定应急设备管理档案。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeText("    制定了《职业病危害事故应急救援预案》，规定了事故发生后的疏通线路、紧急集合点、技术方案、救援设施的维护和启动、医疗救护方案等内容；职业病危害事故应急救援目标为钣金车间、烤漆房、漆库；定期演练职业病危害事故应急救援预案并进行记录。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("11.3.12岗位职业卫生操作规程");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    为保障员工的职业健康和安全、防治职业病危害，依据《中华人民共和国职业病防治法》、《工作场所职业卫生监督管理规定》等有关法律、法规的规定，该公司制定了维修车间岗位职业健康操作规程。规定了操作工在操作时必须严格遵守劳动纪律，服从管理，正确佩带和使用劳动防护用品；发生职业卫生事故采取的急救措施等。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("11.4职业病防治经费");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    该公司每年均划拨专项资金用于职业病防治经费，费用大多用在职业病防护设施上，主要包括防护设施维护、个人劳动防护用品、职业病危害因素检测与评价、职业卫生培训、职业健康体检、警示标识及中文警示说明等方面。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("11.5职业卫生管理评价");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    依据《中华人民共和国职业病防治法》、《工作场所职业卫生监督管理规定》（国家安全生产监督管理总局令第47号）、《职业病危害项目申报办法》（国家安全生产监督管理总局令第48号）和《用人单位职业病防治指南》（GBZ/T225-2010）等法律、法规，对用人单位职业卫生管理措施的有效性、合理性制成检查表，见表11-2。");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表11-2  职业卫生管理评价检查表");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info1 = m_doc.Tables.Add(m_wordApp.Selection.Range, 28, 5);
            info1.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info1.Borders.Enable = 1;
            info1.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info1.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info1.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info1.Columns[1].Width = 30;
            info1.Columns[2].Width = 120;
            info1.Rows[1].Height = 20;
            info1.Cell(1, 1).Select();
            SetCellHeaderText("序号");
            info1.Cell(1, 2).Select();
            SetCellHeaderText("检查内容");
            info1.Cell(1, 3).Select();
            SetCellHeaderText("检查依据");
            info1.Cell(1, 4).Select();
            SetCellHeaderText("检查结果");
            info1.Cell(1, 5).Select();
            SetCellHeaderText("评价");

            info1.Rows[2].Height = 20;
            info1.Cell(2, 1).Select();
            SetCellHeaderText("一、职业卫生管理组织机构和人员");
            info1.Cell(2, 1).Merge(info1.Cell(2, 5));

            info1.Rows[3].Height = 20;
            info1.Cell(3, 1).Select();
            SetCellHeaderText("1");
            info1.Cell(3, 2).Select();
            SetCellHeaderText("职业病危害严重的用人单位，应当设置或者指定职业卫生管理机构或者组织，配备专职职业卫生管理人员。其他存在职业病危害的用人单位，劳动者超过100人的，应当设置或者指定职业卫生管理机构或者组织，配备专职职业卫生管理人员；劳动者在100人以下的，应当配备专职或者兼职的职业卫生管理人员，负责本单位的职业病防治工作。");
            info1.Cell(3, 3).Select();
            SetCellHeaderText("《工作场所职业卫生监督管理规定》（国家安全生产监督管理总局令第47号）第八条");

            info1.Rows[4].Height = 20;
            info1.Cell(4, 1).Select();
            SetCellHeaderText("二、职业病防治规划、实施方案");
            info1.Cell(4, 1).Merge(info1.Cell(4, 5));
            info1.Rows[5].Height = 80;
            info1.Cell(5, 1).Select();
            SetCellHeaderText("1");
            info1.Cell(5, 2).Select();
            SetCellHeaderText("制定职业病防治计划和实施方案。");
            info1.Cell(5, 3).Select();
            SetCellHeaderText("《中华人民共和国职业病防治法》第二十一条（二）");

            info1.Rows[6].Height = 20;
            info1.Cell(6, 1).Select();
            SetCellHeaderText("三、职业卫生管理制度与操作规程");
            info1.Cell(6, 1).Merge(info1.Cell(6, 5));
            info1.Rows[7].Height = 220;
            info1.Cell(7, 1).Select();
            SetCellHeaderText("1");
            info1.Cell(7, 2).Select();
            SetCellHeaderText("存在职业病危害的用人单位应当制定职业病危害防治计划和实施方案，建立、健全下列职业卫生管理制度和操作规程：（一）职业病危害防治责任制度；（二）职业病危害警示与告知制度；（三）职业病危害项目申报制度；（四）职业病防治宣传教育培训制度；（五）职业病防护设施维护检修制度；（六）职业病防护用品管理制度；（七）职业病危害监测及评价管理制度；（八）建设项目职业卫生“三同时”管理制度；（九）劳动者职业健康监护及其档案管理制度；（十）职业病危害事故处置与报告制度；（十一）职业病危害应急救援与管理制度；（十二）岗位职业卫生操作规程；（十三）法律、法规、规章规定的其他职业病防治制度。");
            info1.Cell(7, 3).Select();
            SetCellHeaderText("《工作场所职业卫生监督管理规定》（国家安全生产监督管理总局令第47号）第十一条");

            info1.Rows[8].Height = 20;
            info1.Cell(8, 1).Select();
            SetCellHeaderText("四、职业病危害监测及评价制度");
            info1.Cell(8, 1).Merge(info1.Cell(8, 5));
            info1.Rows[9].Height = 220;
            info1.Cell(9, 1).Select();
            SetCellHeaderText("1");
            info1.Cell(9, 2).Select();
            SetCellHeaderText("建立、健全工作场所职业病危害因素监测及评价制度。");
            info1.Cell(9, 3).Select();
            SetCellHeaderText("《中华人民共和国职业病防治法》第二十一条（五）");

            info1.Rows[10].Height = 20;
            info1.Cell(10, 1).Select();
            SetCellHeaderText("五、职业病危害的告知情况");
            info1.Cell(10, 1).Merge(info1.Cell(10, 5));
            info1.Rows[11].Height = 220;
            info1.Cell(11, 1).Select();
            SetCellHeaderText("1");
            info1.Cell(11, 2).Select();
            SetCellHeaderText("产生职业病危害的用人单位，应当在醒目位置设置公告栏，公布有关职业病防治的规章制度、操作规程、职业病危害事故应急救援措施和工作场所职业病危害因素检测结果。");
            info1.Cell(11, 3).Select();
            SetCellHeaderText("《中华人民共和国职业病防治法》第二十五条第一款");
            info1.Cell(11, 1).Select();
            SetCellHeaderText("2");
            info1.Cell(11, 2).Select();
            SetCellHeaderText("用人单位与劳动者订立劳动合同（含聘用合同，下同）时，应当将工作过程中可能产生的职业病危害及其后果、职业病防护措施和待遇等如实告知劳动者，并在劳动合同中写明，不得隐瞒或者欺骗。");
            info1.Cell(11, 3).Select();
            SetCellHeaderText("《工作场所职业卫生监督管理规定》（国家安全生产监督管理总局令第47号）第二十九条");

            info1.Rows[12].Height = 20;
            info1.Cell(12, 1).Select();
            SetCellHeaderText("六、职业卫生培训情况");
            info1.Cell(12, 1).Merge(info1.Cell(12, 5));
            info1.Rows[13].Height = 220;
            info1.Cell(13, 1).Select();
            SetCellHeaderText("1");
            info1.Cell(13, 2).Select();
            SetCellHeaderText("生产经营单位的主要负责人和职业健康管理人员应当具备与本单位所从事的生产经营活动相适应的职业健康知识和管理能力，并接受职业卫生培训。");
            info1.Cell(13, 3).Select();
            SetCellHeaderText("《工作场所职业卫生监督管理规定》（国家安全生产监督管理总局令第47号）第九条第一项");

            info1.Rows[14].Height = 20;
            info1.Cell(14, 1).Select();
            SetCellHeaderText("七、职业健康监护制度");
            info1.Cell(14, 1).Merge(info1.Cell(14, 5));
            info1.Rows[15].Height = 220;
            info1.Cell(15, 1).Select();
            SetCellHeaderText("1");
            info1.Cell(15, 2).Select();
            SetCellHeaderText("用人单位应当组织劳动者进行职业健康检查，并承担职业健康检查费用。");
            info1.Cell(15, 3).Select();
            SetCellHeaderText("《用人单位职业健康监护监督管理办法》（国家安全生产监督管理总局令第49号）第八条");

            info1.Rows[16].Height = 20;
            info1.Cell(16, 1).Select();
            SetCellHeaderText("八、职业病危害事故应急救援预案、设施及演练情况");
            info1.Cell(16, 1).Merge(info1.Cell(16, 5));
            info1.Rows[17].Height = 220;
            info1.Cell(17, 1).Select();
            SetCellHeaderText("1");
            info1.Cell(17, 2).Select();
            SetCellHeaderText("建立、健全职业病危害事故应急救援预案。");
            info1.Cell(17, 3).Select();
            SetCellHeaderText("《中华人民共和国职业病防治法》第二十一条（六）");
            info1.Rows[18].Height = 220;
            info1.Cell(18, 1).Select();
            SetCellHeaderText("2");
            info1.Cell(18, 2).Select();
            SetCellHeaderText("用人单位应当对职业病防护设备、应急救援设施进行经常性的维护、检修和保养，定期检测其性能和效果，确保其处于正常状态，不得擅自拆除或者停止使用。");
            info1.Cell(18, 3).Select();
            SetCellHeaderText("《工作场所职业卫生监督管理规定》（国家安全生产监督管理总局令第47号）第十八条");

            info1.Rows[19].Height = 20;
            info1.Cell(19, 1).Select();
            SetCellHeaderText("九、警示标识及中文警示说明");
            info1.Cell(19, 1).Merge(info1.Cell(19, 5));
            info1.Rows[20].Height = 220;
            info1.Cell(20, 1).Select();
            SetCellHeaderText("1");
            info1.Cell(20, 2).Select();
            SetCellHeaderText("在产生粉尘的作业场所设置“注意防尘”警告标识和“戴防尘口罩”指令标识；在产生噪声的作业场所设置“噪声有害”警告标识和“戴护耳器”指令标识，在高温作业场所，设置“注意高温”警告标识。");
            info1.Cell(20, 3).Select();
            SetCellHeaderText("《工作场所职业病危害警示标识》（GBZ158-2003）第8条");

            info1.Rows[21].Height = 20;
            info1.Cell(21, 1).Select();
            SetCellHeaderText("十、职业病危害申报");
            info1.Cell(21, 1).Merge(info1.Cell(21, 5));
            info1.Rows[22].Height = 220;
            info1.Cell(22, 1).Select();
            SetCellHeaderText("1");
            info1.Cell(22, 2).Select();
            SetCellHeaderText("用人单位（煤矿除外）工作场所存在职业病目录所列职业病的危害因素的，应当及时、如实向所在地安全生产监督管理部门申报危害项目，并接受安全生产监督管理部门的监督管理。");
            info1.Cell(22, 3).Select();
            SetCellHeaderText("《职业病危害项目申报办法》（国家安全生产监督管理总局令第48号）第二条");

            info1.Rows[23].Height = 20;
            info1.Cell(23, 1).Select();
            SetCellHeaderText("十一、职业卫生档案管理");
            info1.Cell(23, 1).Merge(info1.Cell(23, 5));
            info1.Rows[24].Height = 220;
            info1.Cell(24, 1).Select();
            SetCellHeaderText("1");
            info1.Cell(24, 2).Select();
            SetCellHeaderText("建立、健全职业卫生档案和劳动者健康监护档案。");
            info1.Cell(24, 3).Select();
            SetCellHeaderText("《中华人民共和国职业病防治法》第二十一条第四项");

            info1.Rows[25].Height = 20;
            info1.Cell(25, 1).Select();
            SetCellHeaderText("十二、职业病危害防治经费");
            info1.Cell(25, 1).Merge(info1.Cell(25, 5));
            info1.Rows[26].Height = 220;
            info1.Cell(26, 1).Select();
            SetCellHeaderText("1");
            info1.Cell(26, 2).Select();
            SetCellHeaderText("用人单位按照职业病防治要求，用于预防和治理职业病危害、工作场所卫生检测、健康监护和职业卫生培训等费用，按照国家有关规定，在生产成本中据实列支。");
            info1.Cell(26, 3).Select();
            SetCellHeaderText("《中华人民共和国职业病防治法》第四十二条");

            info1.Rows[27].Height = 20;
            info1.Cell(27, 1).Select();
            SetCellHeaderText("十三、其他");
            info1.Cell(27, 1).Merge(info1.Cell(27, 5));
            info1.Rows[28].Height = 220;
            info1.Cell(28, 1).Select();
            SetCellHeaderText("1");
            info1.Cell(28, 2).Select();
            SetCellHeaderText("用人单位必须依法参加工伤社会保险。");
            info1.Cell(28, 3).Select();
            SetCellHeaderText("《中华人民共和国职业病防治法》第七条");
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    对该用人单位职业卫生管理情况进行检查，本次共检查15项内容，均符合上述标准、规范的相关要求。");
            m_wordApp.Selection.TypeParagraph();
        }

        //12结论
        private void WriteConclusion()
        {
            m_wordApp.Selection.Font.Size = 16;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("12结论");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.TypeText("12.1分项结论");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.Font.Size = 10;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("表1　用人单位职业病危害现状评价分项结论");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 10;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            Word.Table info1 = m_doc.Tables.Add(m_wordApp.Selection.Range, 16, 3);
            info1.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info1.Borders.Enable = 1;
            info1.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info1.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info1.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info1.Columns[1].Width = 230;
            info1.Columns[2].Width = 30;
            info1.Rows[1].Height = 20;
            info1.Cell(1, 1).Select();
            SetCellHeaderText("项目");
            info1.Cell(1, 2).Select();
            SetCellHeaderText("判断");
            info1.Cell(1, 3).Select();
            SetCellHeaderText("存在问题简要说明");
            info1.Rows[2].Height = 20;
            info1.Cell(2, 1).Select();
            SetCellHeaderText("1.总体布局");
            info1.Rows[3].Height = 20;
            info1.Cell(3, 1).Select();
            SetCellHeaderText("2.设备布局");
            info1.Rows[4].Height = 20;
            info1.Cell(4, 1).Select();
            SetCellHeaderText("3.建筑卫生学");
            info1.Rows[5].Height = 20;
            info1.Cell(5, 1).Select();
            SetCellHeaderText("4.职业病危害因素");
            info1.Rows[6].Height = 20;
            info1.Cell(6, 1).Select();
            SetCellHeaderText("5.职业病防护设施");
            info1.Rows[7].Height = 20;
            info1.Cell(7, 1).Select();
            SetCellHeaderText("6.应急救援设施");
            info1.Rows[8].Height = 20;
            info1.Cell(8, 1).Select();
            SetCellHeaderText("7.职业健康监护");
            info1.Rows[9].Height = 20;
            info1.Cell(9, 1).Select();
            SetCellHeaderText("8.个人防护用品");
            info1.Rows[10].Height = 20;
            info1.Cell(10, 1).Select();
            SetCellHeaderText("9.辅助用室");
            info1.Rows[11].Height = 20;
            info1.Cell(11, 1).Select();
            SetCellHeaderText("10.职业卫生管理组织机构");
            info1.Rows[12].Height = 20;
            info1.Cell(12, 1).Select();
            SetCellHeaderText("11.职业卫生管理制度");
            info1.Rows[13].Height = 20;
            info1.Cell(13, 1).Select();
            SetCellHeaderText("12.职业病危害告知");
            info1.Rows[14].Height = 20;
            info1.Cell(14, 1).Select();
            SetCellHeaderText("13.职业卫生培训");
            info1.Rows[15].Height = 20;
            info1.Cell(15, 1).Select();
            SetCellHeaderText("14.职业病危害项目申报");
            info1.Rows[16].Height = 20;
            info1.Cell(16, 1).Select();
            SetCellHeaderText("15.既往职业卫生评价建议落实情况");


            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("12.2职业病危害风险分类");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    依据《国民经济行业分类》（GB/T 4754-2011）将该项目划分为汽车修理与维护行业，依据《国家安全监管总局关于公布建设项目职业病危害风险分类管理目录（2012年版）的通知》（安监总安健[2012]73号）的要求及现场检测数据、防护设施、个人防护用品设置情况、应急救援、职业卫生管理等，综合考虑最终将该项目定义为职业病危害较重的项目。");
            m_wordApp.Selection.TypeParagraph();
        }


        //13建议
        private void WriteSuggestion()
        {
            m_wordApp.Selection.Font.Size = 16;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("13建议");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 0;
            m_wordApp.Selection.TypeText("    通过对该用人单位存在的主要职业病危害因素的危害程度和采取的职业病防护措施的综合分析，针对不足之处提出以下职业病危害控制措施的建议");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            //从用户选择的数据列表之中加载填充。
            m_wordApp.Selection.TypeText("13.1职业病防护设施");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.TypeText("13.2个人使用的职业病防护用品");
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.TypeText("13.3职业卫生管理");
            m_wordApp.Selection.TypeParagraph();
        }

        private void WriteLast()
        {
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("附表");
            m_wordApp.Selection.TypeParagraph();
            m_wordApp.Selection.Font.Size = 12;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            m_wordApp.Selection.TypeText("XX单位职业病危害现状汇总表");
            m_wordApp.Selection.TypeParagraph();
            Word.Table info1 = m_doc.Tables.Add(m_wordApp.Selection.Range, 3, 16);
            info1.Rows.Alignment = WdRowAlignment.wdAlignRowLeft;
            info1.Borders.Enable = 1;
            info1.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            info1.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            info1.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
            info1.Columns[1].Width = 20;
            info1.Rows[1].Height = 20;
            info1.Cell(1, 1).Select();
            SetCellHeaderText("评价单元");
            info1.Cell(1, 2).Select();
            SetCellHeaderText("岗位/工种");
            info1.Cell(1, 3).Select();
            SetCellHeaderText("工作地点");
            info1.Cell(1, 4).Select();
            SetCellHeaderText("工作方式");
            info1.Cell(1, 5).Select();
            SetCellHeaderText("接触职业病危害因素种类");
            info1.Cell(1, 6).Select();
            SetCellHeaderText("检测结果");
            info1.Cell(1, 7).Select();
            SetCellHeaderText("接触职业病危害人数");

            info1.Cell(1, 10).Select();
            SetCellHeaderText("日接触时间");
            info1.Cell(1, 11).Select();
            SetCellHeaderText("是否进行职业健康检查");
            info1.Cell(1, 13).Select();
            SetCellHeaderText("职业病防护设施");
            info1.Cell(1, 15).Select();
            SetCellHeaderText("个人防护用品");

            info1.Cell(2, 7).Select();
            SetCellHeaderText("总数");
            info1.Cell(2, 8).Select();
            SetCellHeaderText("男");
            info1.Cell(2, 9).Select();
            SetCellHeaderText("女");
            info1.Cell(2, 11).Select();
            SetCellHeaderText("是（人数）");
            info1.Cell(2, 12).Select();
            SetCellHeaderText("否");
            info1.Cell(2, 13).Select();
            SetCellHeaderText("有（名称）");
            info1.Cell(2, 14).Select();
            SetCellHeaderText("无");
            info1.Cell(2, 15).Select();
            SetCellHeaderText("有（名称）");
            info1.Cell(2, 16).Select();
            SetCellHeaderText("无");

            info1.Cell(1,15).Merge(info1.Cell(1,16));
            info1.Cell(1, 13).Merge(info1.Cell(1, 14));
            info1.Cell(1, 11).Merge(info1.Cell(1, 12));
            info1.Cell(2, 10).Merge(info1.Cell(1, 10));
            info1.Cell(1, 7).Merge(info1.Cell(1, 9));

            info1.Cell(2, 1).Merge(info1.Cell(1, 1));
            info1.Cell(2, 2).Merge(info1.Cell(1, 2));
            info1.Cell(2, 3).Merge(info1.Cell(1, 3));
            info1.Cell(2, 4).Merge(info1.Cell(1, 4));
            info1.Cell(2, 5).Merge(info1.Cell(1, 5));
            info1.Cell(2, 6).Merge(info1.Cell(1, 6));
            
            m_wordApp.Selection.TypeParagraph();

            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
            m_wordApp.Selection.MoveDown();//光标下移，光标移出表格

            m_wordApp.Selection.Font.Size = 14;
            m_wordApp.Selection.Font.Bold = 1;
            m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            m_wordApp.Selection.TypeText("附件1：委托书");


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
