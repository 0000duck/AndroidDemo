/*
Copyright (c) 2017, qwinner

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using myconn.CmdUI;
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

        private void 设置地址命令ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetAddrFrm frm = new SetAddrFrm();
            frm.ShowDialog();
        }

        private void 设置波特率命令ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBuddeFrm frm = new SetBuddeFrm();
            frm.ShowDialog();
        }

        private void 设置坐标系命令ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetAxisCmdFrm frm = new SetAxisCmdFrm();
            frm.ShowDialog();
        }

        private void 设置输入有效电平ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetInLevelFrm frm = new SetInLevelFrm();
            frm.ShowDialog();
        }

        private void 设置输出有效电平ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetOutLevelFrm frm = new SetOutLevelFrm();
            frm.ShowDialog();
        }

        private void 设置坐标极限值ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 设置当前位置坐标为零ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetNowZeroFrm frm = new SetNowZeroFrm();
            frm.ShowDialog();
        }

        private void 读电机坐标值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetAxisValueFrm frm = new GetAxisValueFrm();
            frm.ShowDialog();
        }

        private void 读点击参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetMachineValueFrm frm = new GetMachineValueFrm();
            frm.ShowDialog();
        }

        private void 读极限值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetSettedLimiteFrm frm = new GetSettedLimiteFrm();
            frm.ShowDialog();
        }

        private void 单步运动命令ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveOneStepFrm frm = new MoveOneStepFrm();
            frm.ShowDialog();
        }

        private void 匀速定步长运动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveUniformStepFrm frm = new MoveUniformStepFrm();
            frm.ShowDialog();
        }

        private void 变速定步长运动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveVrateStillStepFrm frm = new MoveVrateStillStepFrm();
            frm.ShowDialog();
        }

        private void 匀速运动到规定通道置位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveXrateToAxis frm = new MoveXrateToAxis();
            frm.ShowDialog();
        }

        private void 变速运动到规定通道置位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveVrateToAxis frm = new MoveVrateToAxis();
            frm.ShowDialog();
        }

        private void 低速归零命令ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveLowRateToZero frm = new MoveLowRateToZero();
            frm.ShowDialog();
        }

        private void 高速归原点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveHiRateToZero frm = new MoveHiRateToZero();
            frm.ShowDialog();
        }

        private void 等待命令1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSleepOneFrm frm = new SetSleepOneFrm();
            frm.ShowDialog();
        }

        private void 等待命令2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSleepTwoFrm frm = new SetSleepTwoFrm();
            frm.ShowDialog();
        }

        private void 等待通道置位命令ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetWaitAxisResetFrm frm = new SetWaitAxisResetFrm();
            frm.ShowDialog();
        }

        private void 输出命令ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetOutputCmdFrm frm = new SetOutputCmdFrm();
            frm.ShowDialog();
        }

        private void 循环开始ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLoopStartFrm frm = new SetLoopStartFrm();
            frm.ShowDialog();
        }

        private void 循环结束ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLoopStopFrm frm = new SetLoopStopFrm();
            frm.ShowDialog();
        }

        private void 擦除扇区命令ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExFileEraseSectorFrm frm = new ExFileEraseSectorFrm();
            frm.ShowDialog();
        }

        private void 擦除块命令ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExFileEraseBlockFrm frm = new ExFileEraseBlockFrm();
            frm.ShowDialog();
        }

        private void 擦除存储器芯片命令ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExFileEraseAllFrm frm = new ExFileEraseAllFrm();
            frm.ShowDialog();
        }

        private void 传送文件到扇区ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExFileWriteSectorFrm frm = new ExFileWriteSectorFrm();
            frm.ShowDialog();
        }

        private void 传送文件到快区ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExFileWriteBlockFrm frm = new ExFileWriteBlockFrm();
            frm.ShowDialog();
        }

        private void 读页命令ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExFileReadPageFrm frm = new ExFileReadPageFrm();
            frm.ShowDialog();
        }

        private void 读扇区命令ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExFileReadSectorFrm frm = new ExFileReadSectorFrm();
            frm.ShowDialog();
        }

        private void 读块命令ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExFileReadBlockFrm frm = new ExFileReadBlockFrm();
            frm.ShowDialog();
        }

        private void 通用命令执行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExFileExcuteCommonFrm frm = new ExFileExcuteCommonFrm();
            frm.ShowDialog();
        }

        private void 轴轨迹运动执行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExFileExcute4AxisFrm frm = new ExFileExcute4AxisFrm();
            frm.ShowDialog();
        }

        private void 传送文件到页ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExFileWritePageFrm frm = new ExFileWritePageFrm();
            frm.ShowDialog();
        }
    }


}
