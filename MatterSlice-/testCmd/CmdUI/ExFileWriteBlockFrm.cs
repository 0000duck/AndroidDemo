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
    public partial class ExFileWriteBlockFrm : Basefm
    {
        private byte[] addrc;
        
        public ExFileWriteBlockFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte hiAddr = Convert.ToByte(textBox1.Text, 16);
            byte loAddr = Convert.ToByte(textBox2.Text, 16);
            CmdFileManage ci = new CmdFileManage();
            addrc = ci.GetWriteToBlockCmd(hiAddr, loAddr);
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
            if(recv == "0xEE 0x04 0x01 0xD4 0xD1")
            {
                richTextBox2.Text = "写1部分成功！/r/n";
            }
            else if(recv == "0xEE 0x05 0x01 0xD4 0xED 0x3D")
            {
                richTextBox2.Text = "写2部分成功！/r/n";
            }

            richTextBox2.Text += recv;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //轨迹文件
        }
    }
}
