/*
Copyright (c) 2017, qwinner

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace myconn
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private bool state = false;
        public delegate void InvokeDelegate();

        //定义委托
        private delegate void UpdateTextEventHandler(string text);
        //定义事件
        private event UpdateTextEventHandler textChanged;
        private byte[] receivedData;//= new byte[16];

        private int fileNum = 1;
        private List<string> sendList = new List<string>();

        private void InitListBox()
        {
            //listBox1.Items.Add("文件1");
            //listBox1.Items.Add("文件2");
            //listBox1.Items.Add("文件3");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);

            }

            timer1.Enabled = false;
            timer1.Interval = 1000;
            comboBox1.Text = "COM1";
            comboBox2.Text = "38400";
            comboBox3.Text = "None";
            comboBox4.Text = "8";
            comboBox5.Text = "1";
//          
            InitListBox();
          
        }
        /// <summary>
        /// 打开关闭串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

            if (state == false)
            {
                InitialSP();
                try
                {
                    serialPort1.Open();
                }
                catch
                {
                    MessageBox.Show("没有发现串口或串口已被占用！");

                    button1.Text = "打开串口";
                    state = false;
                }

                button1.Text = "关闭串口";
                state = true;
            }
            else
            {
                serialPort1.Close();
                button1.Text = "打开串口";
                state = false;
            }

        }
        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (state == true)
            {
                SendBytesData(serialPort1);
            }
            else
            {
                MessageBox.Show("串口没有打开！");
            }

        }
        /// <summary>
        /// 初始化串口
        /// </summary>
        private void InitialSP()
        {

            //comboBox1.Text = "COM1";
            //comboBox2.Text = "4800";
            //comboBox3.Text = "None";//校验位
            //comboBox4.Text = "8";//数据位
            //comboBox5.Text = "One";//停止位


            serialPort1 = new SerialPort(comboBox1.Text.ToString());
            serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
            serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), comboBox3.Text);
            serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), comboBox5.Text);
            serialPort1.DataBits = int.Parse(comboBox4.Text);
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
            textChanged += new UpdateTextEventHandler(ChangeText);
        }
        //发送二进制数据 
        private void SendBytesData(SerialPort serialPort)
        {
            //发送的内容应该是
            CmdGenerator cmdG = new CmdGenerator();
            string sendContent = cmdG.GetAssembledCmd(sendList);
            serialPort.Write(sendContent);
            //byte[] bytesSend = Encoding.ASCII.GetBytes(txtSend.Text);
            //serialPort.Write(bytesSend, 0, bytesSend.Length);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// 退出
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// 接收数据处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
           
            int int_Len = serialPort1.BytesToRead;

            receivedData = new byte[serialPort1.BytesToRead];
            serialPort1.Read(receivedData, 0, receivedData.Length);
            string text = Encoding.ASCII.GetString(receivedData);
            // serialPort1.DiscardInBuffer();
            this.Invoke(textChanged, new string[] { text });

        }
        //public void Display()
        //{
        //    this.txtReceive.Text += strReceive;
        //    strReceive += " ";
        //    label9.Text = rxlen.ToString();
        //}

        private void ChangeText(string text)
        {
            txtReceive.Text += text;//设置textBox2的Text
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sendList.Add(txtSend.Text);
            listBox1.Items.Add("文件" + fileNum.ToString());
            fileNum++;
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ContextMenuStrip strip = new ContextMenuStrip();
                strip.Items.Add("删除");
               // strip.Click += new System.EventHandler(DeleteListItem);
                strip.Show();
            }
        }

        //private void DeleteListItem(object sender, MouseEventArgs e)
        //{
        //    listBox1.Items.Remove(e.);
        //}


    }
}