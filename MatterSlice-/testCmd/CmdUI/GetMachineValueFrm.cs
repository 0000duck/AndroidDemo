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
    public partial class GetMachineValueFrm : Basefm
    {
        private byte[] addrc;
        
        public GetMachineValueFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //具体参数文档和串口助手不一致，？、
            byte commandAddr = (byte)((comboBox1.Text == "0控制板") ? 0x00 : 0x01); ;
            CmdInit ci = new CmdInit();
            addrc = ci.GetMachineValueCmd(commandAddr);
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
            if (!string.IsNullOrEmpty(recv))
            {
                richTextBox2.Text = "读取成功！/r/n";
            }

            richTextBox2.Text += recv;
        }

        private void GetMachineValueFrm_Load(object sender, EventArgs e)
        {

        }
    }
}
