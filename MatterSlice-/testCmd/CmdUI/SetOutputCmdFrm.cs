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
    public partial class SetOutputCmdFrm : Basefm
    {
        private byte[] addrc;
        
        public SetOutputCmdFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte commandAddr = (byte)((comboBox1.Text == "X轴") ? 0x00 : 0x01);
            byte value = (byte)((comboBox2.Text == "低电平") ? 0x00 : 0x01);
            CmdInOut ci = new CmdInOut();
            addrc = ci.GetIOCmd(commandAddr, value);
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
            if(recv == "0xEE 0x04 0x01 0xC3 0xC7")
            {
                richTextBox2.Text = "设置成功！/r/n";
            }

            richTextBox2.Text += recv;
        }
    }
}
