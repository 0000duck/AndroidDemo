using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace myconn
{
    public partial class Form2 : Form
    {

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.listView1.View = View.Details;
           
            this.listView1.Columns.Add("ID", 60, HorizontalAlignment.Left); 
            this.listView1.Columns.Add("名称", 120, HorizontalAlignment.Left); 
            this.listView1.Columns.Add("文件地址", 120, HorizontalAlignment.Left); 
            this.listView1.Columns.Add("文件大小", 120, HorizontalAlignment.Left);

            InitGroupPanel();
        }

        private void InitGroupPanel()
        {
            int currentX = this.groupBox2.Location.X;
            int currentY = this.groupBox2.Location.Y;
            for (int i = 0; i < 19; i++)
            {
                Button btn = new Button();
                btn.Tag = i;
                btn.BackColor = Color.White;
                btn.Height = this.groupBox2.Height - 15;
                btn.Width = this.groupBox2.Width / 20;
                btn.Text = "文件" + i.ToString();
                btn.Location = new Point(currentX, currentY);
                currentX = btn.Location.X + btn.Width;
                currentY = btn.Location.Y;
                this.groupBox2.Controls.Add(btn);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            newFileForm form = new newFileForm();
            form.ShowDialog();
            
            if (form.content != null)
            {
                this.listView1.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度
                //ListViewItem lvi = new ListViewItem();
                //lvi.ImageIndex = fileNum;
                //lvi.Text = fileNum.ToString();
                //lvi.SubItems.Add("第" + fileNum + "行");
                //lvi.SubItems.Add("第" + fileNum + "行");
                //lvi.SubItems.Add(fileNum + "地址");
                //lvi.SubItems.Add("200kb");
                //this.listView1.Items.Add(lvi);

                this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。

            }
        }

        //private void btnClick(object sender, MouseEventArgs e)
        //{
        //    if(e.Button == MouseButtons.Right)
        //    {

        //    }
        //}



        private void button3_Click(object sender, EventArgs e)
        {
            if (this.listView1.CheckedItems.Count > 0)
            {
                foreach (ListViewItem item in this.listView1.CheckedItems)
                {
                    this.listView1.Items.Remove(item);
                }
            }
            else
            {
                MessageBox.Show("请选择删除项");
            }
        }
    }


}
