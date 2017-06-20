using MatterHackers.MatterSlice;
using myconn.CmdUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myconn.UI
{
    public partial class guijitest : Basefm
    {
        private List<Byte> mdatas= new List<byte>();
        public guijitest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int num = int.Parse(textBox1.Text);
            byte value = 0;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    value = 0x40;//x轴正向 01000000
                    break;
                case 1:
                    value = 0xC0;//x轴反向 11000000
                    break;
                case 2:
                    value = 0x10;//y轴正向 00010000
                    break;
                case 3:
                    value = 0x30;//y轴反向 00110000
                    break;
                case 4:
                    value = 0x04;//z轴正向 00000100
                    break;
                case 5:
                    value = 0x0C;//z轴反向 00001100
                    break;
                default:
                    value = 0x00;
                    break;
            }
            for (int i = 0; i < num; i++)
            {
                mdatas.Add(value);
            }
            ProData();
        }

        private void ProData()
        {
            //wyq 输出数据到文件。
            int xor = 0;
            StringBuilder lineToWrite = new StringBuilder();
            int fileLength = mdatas.Count;
            int lenhi = (fileLength >> 16) & 0x00ff;
            int lenMi = (fileLength >> 8) & 0x00ff;
            int lenlo = fileLength & 0x00ff;
            lineToWrite.Append("{0:X2} ".FormatWith(lenhi));
            lineToWrite.Append("{0:X2} ".FormatWith(lenMi));
            lineToWrite.Append("{0:X2} ".FormatWith(lenlo));
            xor = xor ^ lenhi;
            xor = xor ^ lenMi;
            xor = xor ^ lenlo;
            

            foreach (var posValue in mdatas)
            {
                lineToWrite.Append("{0:X2} ".FormatWith(posValue));
                xor = xor ^ posValue;
            }
            lineToWrite.Append("{0:X2} ".FormatWith(xor));
            richTextBox1.Text = lineToWrite.ToString();
            mdatas.Insert(0,(byte)lenlo);
            mdatas.Insert(0, (byte)lenMi);
            mdatas.Insert(0, (byte)lenhi);
            mdatas.Add((byte)xor);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!SerialCom.instance.IsComOpened())
            {
                SerialCom.instance.OpenComPort();
            }
            SerialCom.instance.SetRecvFrm(this);
            SerialCom.instance.SendFile(mdatas.ToArray());
        }

        private void guijitest_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }
    }
}
