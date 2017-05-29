using System;

namespace myconn.CmdUI
{
    public partial class SetBuddeFrm : Basefm
    {
        private byte[] addrc;
        
        public SetBuddeFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int commandAddr = Convert.ToInt16(comboBox1.Text);
            CmdInit ci = new CmdInit();
            switch(commandAddr)
            {
                case 2400:
                    {
                        Global.gBaudRate = 0x00;
                        break;
                    }
                case 4800:
                    {
                        Global.gBaudRate = 0x01;
                        break;
                    }
                case 9600:
                    {
                        Global.gBaudRate = 0x02;
                        break;
                    }
                case 19200:
                    {
                        Global.gBaudRate = 0x03;
                        break;
                    }
                case 38400:
                    {
                        Global.gBaudRate = 0x04;
                        break;
                    }
                default:
                    {
                        Global.gBaudRate = 0x05;
                        break;
                    }
            }
            addrc = ci.GetBaudRateCmd(Global.gBaudRate);
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
            if(recv == "0xEE 0x04 0x01 0xA1 0xA4")
            {
                richTextBox2.Text = "设置成功！/r/n";
            }

            richTextBox2.Text += recv;
        }
    }
}
