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
   public class Testingandevaluationreport
    {
       ProjectContract model;
       string Header = "北京德康莱健康安全科技股份有限公司        DKL/QB-28-02           检测与评价委托协议书";
       string Footer = "联系地址：北京市北京经济开发区西环南路 18号 B座 201-1室" + "电话: 010-51570159   传真:010-51570168" + "E-mail:http//mail.deucalion.com.cn   邮编：100176";

       Word.Application m_wordApp;
       Word.Document m_doc;
       Word.Tools.CommonUtils m_utils;




       public Testingandevaluationreport(ProjectContract Model)
        {
            this.model = Model;
            this.Header =model.ProjectNumber + "                   DKL/QB-28-02                     "; 
        }



       private void SetPage()
       {
           m_doc.PageSetup.PaperSize = WdPaperSize.wdPaperA4;
           m_doc.PageSetup.TopMargin = (float)70.875;
           m_doc.PageSetup.FooterDistance = (float)70;
           m_doc.PageSetup.LeftMargin = (float)56.7;
           m_doc.PageSetup.RightMargin = (float)56.7;
       }
       private void SetParaText(int bold, int size, string text, WdParagraphAlignment wAlign)
       {
           m_wordApp.Selection.Font.Bold = bold;
           m_wordApp.Selection.Font.Color = WdColor.wdColorBlack;
           m_wordApp.Selection.Font.Size = size;
           m_wordApp.Selection.ParagraphFormat.Alignment = wAlign;
           m_wordApp.Selection.TypeText(text);
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
               appStatus = "0";
               WriteReportBasicInfo();
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
           string name = "DKLQB-11-04检测与评价委托协议书 (1)" + DateTime.Now.ToFileTime();
           //      string documentFile = m_utils.File.Combine("c:\\", name, Word.Tools.DocumentFormat.Normal);
           string path = "d://DKLdownload";
           if (!Directory.Exists(path))
           {
               Directory.CreateDirectory(path);
           }

           string documentFile = m_utils.File.Combine(path, "DKLQB-11-04检测与评价委托协议书 (1)", Word.Tools.DocumentFormat.Normal);
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
       private void WriteReportBasicInfo()
       {
           m_wordApp.Selection.Font.Size = 14;
           m_wordApp.Selection.Font.Bold = 1;
           m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
           m_wordApp.Selection.TypeText("检测与评价委托协议书\r");
           m_wordApp.Selection.Font.Bold = 0;
           m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
           m_wordApp.Selection.TypeText("协议编号："+model.ProjectNumber);
           Word.Table basicInfo = m_doc.Tables.Add(m_wordApp.Selection.Range, 14, 4);
           basicInfo.Borders.Enable = 1;
           basicInfo.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
           basicInfo.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
           basicInfo.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
           basicInfo.Columns[1].Width = 90;
         
          


           basicInfo.Rows[1].Height = 20;
           basicInfo.Cell(1, 1).Select();
           SetCellHeaderText("委托单位名称");
      //     basicInfo.Cell(1, 2).Merge(basicInfo.Cell(1, 3));
           basicInfo.Cell(1, 2).Select();
           SetCellHeaderText(model.CompaneName);
           basicInfo.Cell(1, 3).Select();
           SetCellHeaderText("委托时间");
           basicInfo.Cell(1, 4).Select();
           SetCellHeaderText(model.CreateTime.ToString("yyyy/MM/dd"));

           basicInfo.Rows[2].Height = 20;
           basicInfo.Cell(2, 1).Select();
           SetCellHeaderText("委托单位地址");
           basicInfo.Cell(2, 2).Select();
           SetCellHeaderText(model.CompanyAddress);
        //   basicInfo.Cell(2, 2).Merge(basicInfo.Cell(2, 3));
           basicInfo.Cell(2, 3).Select();
           SetCellHeaderText("电话/传真");
           basicInfo.Cell(2, 4).Select();
           SetCellHeaderText(model.ContactTel);
           basicInfo.Rows[3].Height = 20;
           basicInfo.Cell(3, 1).Select();
           SetCellHeaderText("联系人");
      //     basicInfo.Cell(3, 2).Merge(basicInfo.Cell(3, 3));
           basicInfo.Cell(3, 2).Select();
           SetCellHeaderText(model.ContactPersonA);
           basicInfo.Cell(3, 3).Select();
           SetCellHeaderText("联系电话");
           basicInfo.Cell(3, 4).Select();
           SetCellHeaderText(model.TelA);


           basicInfo.Rows[4].Height = 20;
           basicInfo.Cell(4, 1).Select();
           SetCellHeaderText("承检单位名称");
          
           basicInfo.Cell(4, 2).Merge(basicInfo.Cell(4, 4));
           basicInfo.Cell(4, 2).Select();
           SetCellHeaderText("北京德康莱健康安全科技股份有限公司");



           basicInfo.Rows[5].Height = 20;
           basicInfo.Cell(5, 1).Select();
           SetCellHeaderText("联系人");
     //      basicInfo.Cell(5, 2).Merge(basicInfo.Cell(5, 3));
           basicInfo.Cell(5, 2).Select();
           SetCellHeaderText("");
           basicInfo.Cell(5, 3).Select();
           SetCellHeaderText("联系电话");
           basicInfo.Cell(5, 4).Select();
           SetCellHeaderText("");



           basicInfo.Rows[6].Height = 20;
           basicInfo.Cell(6, 1).Select();
           SetCellHeaderText("检测类别");
         
           basicInfo.Cell(6, 2).Merge(basicInfo.Cell(6, 4));
           basicInfo.Cell(6, 2).Select();
           SetCellHeaderText("检测项目");



           basicInfo.Rows[7].Height = 20;
           basicInfo.Cell(7, 1).Select();
           SetCellHeaderText("完成时间要求");
           basicInfo.Cell(7, 2).Select();
           m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
           SetCellText(1, 12, "口 依据检测进度，无特殊要求（合同生效后__个工作日内) \r口 加急！（合同生效后在__个工作日内)\r口 ________________________", WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
           
           basicInfo.Cell(7, 2).Merge(basicInfo.Cell(7, 4));

         

           basicInfo.Cell(8, 1).Merge(basicInfo.Cell(9, 1));
           basicInfo.Cell(8, 1).Select();
           SetCellHeaderText("取报告\r方式");
           basicInfo.Cell(8, 2).Merge(basicInfo.Cell(9, 2));
           basicInfo.Cell(8, 2).Select();
           SetCellText(1, 12, "口 自取\r口 邮寄，报告交付日期以快递发送日邮戳为准", WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
         
        
         
           basicInfo.Cell(8, 3).Select();
           SetCellHeaderText("报告收取人");
           basicInfo.Cell(9, 3).Select();
           SetCellHeaderText("邮  编");
           basicInfo.Cell(9, 4).Select();
           SetCellHeaderText("");

           basicInfo.Cell(10, 1).Select();
           //basicInfo.Rows[10].Height = 20;
           //basicInfo.Cell(10, 1).Select();
           SetCellHeaderText("委托范围");
           basicInfo.Cell(10, 2).Select();
           SetCellText(1, 12, "口"+model.CompaneName+"工作场所职业病危害因素检测与评价\r口_______________", WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
         
          
           basicInfo.Cell(10, 2).Merge(basicInfo.Cell(10, 4));

      

           //basicInfo.Rows[11].Height = 20;
           basicInfo.Cell(11, 1).Select();
           SetCellHeaderText("付款方式");
           basicInfo.Cell(11, 2).Select();
           SetCellText(1, 11, "本协议总金额：" + model.Money + "（小写）         （大写）\r本协议签订后五个工作日内支付" + model.PayRatioFirst + "%，即：" + ((Convert.ToDouble(model.Money)) * (Convert.ToDouble(model.PayRatioFirst) / 100)).ToString() + "元。  报告交付前" + (100 - Convert.ToInt16(model.PayRatioFirst)) + "%，即：" + ((Convert.ToDouble(model.Money)) * ((Convert.ToDouble(100) - Convert.ToDouble(model.PayRatioFirst)) / 100)).ToString("0.00") + "元。 \r开户行:兴业银行北京经济技术开发区\r账号：321130100100206456\r", WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);


           basicInfo.Cell(11, 2).Merge(basicInfo.Cell(11, 4));
           basicInfo.Cell(12, 1).Select();
           SetCellHeaderText("协议说明");
           basicInfo.Cell(12, 2).Select();
           SetCellText(1, 12, "1.本协议书双方签字盖章之日起生效，本协议传真有效。\r2.承检单位在收到检测费全款后，按约定时间交付检测报告。\r3.本协议书一式四份，双方各持两份，具有同等法律效力。\r4.本协议未尽事宜，经双方友好协商解决；协商不成，以《中华人民共和国合同法》及相关法律法规为依据，双方均有保留向法院诉讼的权利。", WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);

          
           basicInfo.Cell(12, 2).Merge(basicInfo.Cell(12, 4));




           //basicInfo.Rows[13].Height = 20;
           basicInfo.Cell(13, 1).Select();
           SetCellHeaderText("双方承诺");
          
           basicInfo.Cell(13, 2).Select();
           m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
           SetCellText(1, 12, "委托单位承诺:\r1.按承检单位要求，提供工作场所工作环境、生产工艺、所用设备及防护设施、原辅材料及产品、工作方式及个体职业病防护等方面的相关资料，并保证所提供资料的真实性、完整性、客观性及与工作场所的一致性。\r2.在签订本协议书后  工作日内配合承检单位安排现场工作。\r3.委托单位未按照合同约定支付首付款的，承检单位有权拒绝赴现场工作，委托单位超过30日仍未支付首付款的，承检单位有权解除合同并要求委托方支付合同金额50%的违约金。承检单位完成检测与评价报告后，通知委托单位领取承检单位工作成果，委托单位应按照承检单位规定的时间接受承检单位的工作成果。委托单位无正当理由迟延接受超过30日的，视为委托单位已经接受承检单位的工作成果，委托单位仍应支付本合同剩余款项。\r承检单位承诺：\r1.按照国家法律、法规、标准和委托单位委托的范围开展检测与评价工作。\r2.若在收到检测费全款后未按本协议约定交付检测报告给委托单位的，承检单位每日按本协议总金额的0.5%支付违约金，直至本协议总金额的50%。", WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
           basicInfo.Cell(13, 2).Merge(basicInfo.Cell(13, 4));


           m_wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

           //basicInfo.Rows[14].Height = 20;
           basicInfo.Cell(14, 1).Select();
           basicInfo.Cell(14, 1).Merge(basicInfo.Cell(14, 2));
           SetCellText(1, 12, "□	委托单位法人代表（签名）：\r□	或委托代理人（签名）：\r\r\r委托单位：               （盖章）\r             年    月    日 ", WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);

           basicInfo.Cell(14, 2).Merge(basicInfo.Cell(14, 3));
           basicInfo.Cell(14, 2).Select();

           SetCellText(1, 12, "□	承检单位法人代表（签名）：           \r□	或委托代理人（签名）：      \r\r\r 承检单位：               (盖章)\r             年    月    日", WdParagraphAlignment.wdAlignParagraphLeft, WdCellVerticalAlignment.wdCellAlignVerticalCenter);
        
           m_wordApp.Selection.MoveDown();//光标下移，光标移出表格
           m_doc.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.Text = Header;

           m_doc.Sections[1].Footers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.Text = Footer;

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
    }

}
