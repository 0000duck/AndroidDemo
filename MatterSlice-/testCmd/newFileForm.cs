using MatterHackers.MatterSlice;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myconn
{
    public partial class newFileForm : Form
    {
        public StringBuilder content = new StringBuilder();
        SynchronizationContext m_SyncContext = null;

        private bool fileok = false;
        public newFileForm()
        {
            InitializeComponent();
            //获取UI线程同步上下文
            m_SyncContext = SynchronizationContext.Current;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(content.ToString()))
            {
                if (DialogResult.Yes == MessageBox.Show("文件没有生成,确认关闭?", "确认关闭", MessageBoxButtons.YesNo))
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newFileForm_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread geneThread = new Thread(GenerateFile);
            geneThread.Start();            
        }

        private void GenerateFile()
        {
            ConfigSettings config = new ConfigSettings();
            fffProcessor processor = new fffProcessor(config);
            processor.SetTargetFile(@"I:\5mm.txt");
            processor.LoadStlFile(@"I:\5mm.stl");

            processor.DoProcessing();
            processor.finalize();
            processor.Cancel();

            using (System.IO.StreamReader sr = new System.IO.StreamReader(@"I:\5mm.txt"))
            {
                string str;
                while ((str = sr.ReadLine()) != null)
                {
                    //在线程中更新UI（通过UI线程同步上下文m_SyncContext）
                    m_SyncContext.Post(SetTextSafePost, str);
                }
            }

        }

        private void SetTextSafePost(object text)
        {
            richTextBox1.AppendText(text.ToString());
            content.Append(text.ToString());
        }
    }
}
