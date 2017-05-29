using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myconn.CmdUI
{
    public partial class ExFileExcuteCommonFrm : Basefm
    {
        private byte[] addrc;
        
        public ExFileExcuteCommonFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte hiAddr = Convert.ToByte(textBox1.Text, 16);
            byte loAddr = Convert.ToByte(textBox2.Text, 16);
            CmdFileManage ci = new CmdFileManage();
            addrc = ci.GetExcuteCommonCmd(hiAddr, loAddr);
            richTextBox1.Text = DataChange.byteToHexStr(addrc);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(!SerialCom.instance.IsComOpened())
            {
                SerialCom.instance.OpenComPort();
            }
            SerialCom.instance.SetRecvFrm(this);
            SerialCom.instance.SendFile(addrc);
        }

        public override void OnRecvData(byte[] datas)
        {
            string recv = DataChange.byteToHexOXStr(datas).Replace("0x00","").Trim();
            if(recv == "0xEE 0x04 0x01 0xD8 0xDD")
            {
                richTextBox2.Text = "读取成功！/r/n";
            }
            

            richTextBox2.Text += recv;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //轨迹文件
        }
    }
}
