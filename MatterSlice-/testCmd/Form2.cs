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

            InitDataGridView();
        }

        private List<DataGridViewColumn> GetColumn(string name,int width, int count)
        {
            List<DataGridViewColumn> cols = new List<DataGridViewColumn>();
            for (int i = 0; i < count; i++)
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Width = width;
                
                column.HeaderText = name + i.ToString();
                cols.Add(column);
            }
            return cols;
        }

        private void InitPageColumn()
        {
            //页 16*16*16*2
            var pagecols = GetColumn("page", 2, 16);
            //var pagecols = GetColumn("page", this.dataGridView1.Width / 8192, 16);
            foreach (var col in pagecols)
            {
                this.dataGridView1.Columns.Add(col);
            }
        }

        private void InitSectorColumn()
        {
            //扇区 16*16*2
            var sectorcols = GetColumn("sector", 32, 15);
            //var sectorcols = GetColumn("sector", this.dataGridView1.Width / 512, 15);
            foreach (var col in sectorcols)
            {
                this.dataGridView1.Columns.Add(col);
            }
        }

        private void InitBolckColumn()
        {
            //块 16*16*2
            var bolckcols = GetColumn("block", 20, 15);
            //var bolckcols = GetColumn("block", this.dataGridView1.Width / 32, 15);
            foreach (var col in bolckcols)
            {
                this.dataGridView2.Columns.Add(col);
            }
            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.Width = 320;// this.dataGridView1.Width / 2;
            column.HeaderText = "大文件";
            this.dataGridView2.Columns.Add(column);
        }

        private void InitDataGridView()
        {
            //this.dataGridView1.Width = 1000;
            InitPageColumn();
            InitSectorColumn();
            InitBolckColumn();

            this.dataGridView1.ColumnHeadersVisible = false;
            this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[0].Height = this.dataGridView1.Height;
            this.dataGridView1.ClearSelection();
           

            this.dataGridView2.ColumnHeadersVisible = false;
            this.dataGridView2.Rows.Add();
            this.dataGridView2.Rows[0].Height = this.dataGridView2.Height;
            this.dataGridView2.ClearSelection();

            PaintStoreColor(1, 2, 3, true);
            //ClearStoreColor(1, 2, 0, false);
        }

        private void PaintStoreColor(int pageNum,int SectorNum,int BlockNum,bool big)
        {
            if(pageNum != 0)
            {
                this.dataGridView1.Rows[0].Cells[pageNum].Style.BackColor = Color.Blue;
            }
            if (SectorNum != 0)
            {
                this.dataGridView1.Rows[0].Cells[15+ SectorNum].Style.BackColor = Color.Blue;
            }
            if (BlockNum != 0)
            {
                this.dataGridView2.Rows[0].Cells[BlockNum -1].Style.BackColor = Color.Blue;
            }
            if (big)
            {
                this.dataGridView2.Rows[0].Cells[15].Style.BackColor = Color.Blue;
            }
        }

        private void ClearStoreColor(int pageNum, int SectorNum, int BlockNum, bool big)
        {
            if (pageNum != 0)
            {
                this.dataGridView1.Rows[0].Cells[pageNum].Style.BackColor = Color.White;
            }
            if (SectorNum != 0)
            {
                this.dataGridView1.Rows[0].Cells[15 + SectorNum].Style.BackColor = Color.White;
            }
            if (BlockNum != 0)
            {
                this.dataGridView2.Rows[0].Cells[BlockNum - 1].Style.BackColor = Color.White;
            }
            if (big)
            {
                this.dataGridView2.Rows[0].Cells[15].Style.BackColor = Color.White;
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
