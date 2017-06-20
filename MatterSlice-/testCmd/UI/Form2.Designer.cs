namespace myconn
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.初始化命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置地址命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置波特率命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置坐标系命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置输入有效电平ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置输出有效电平ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置坐标极限值ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置当前位置坐标为零ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.读电机坐标值ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.读点击参数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.读极限值ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.运动命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.单步运动命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.匀速定步长运动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.变速定步长运动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.匀速运动到规定通道置位ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.变速运动到规定通道置位ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.低速归零命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.高速归原点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.延时命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.等待命令1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.等待命令2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.等待通道置位命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.输入输出命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.输出命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.循环命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.循环开始ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.循环结束ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.文件管理命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.擦除扇区命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.擦除块命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.擦除存储器芯片命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.传送文件到扇区ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.传送文件到快区ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.读页命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.读扇区命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.读块命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.通用命令执行ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.轴轨迹运动执行ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.传送文件到页ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.LightGray;
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Location = new System.Drawing.Point(7, 48);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(946, 165);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "页和扇区（）";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Enabled = false;
            this.dataGridView1.Location = new System.Drawing.Point(3, 21);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(937, 141);
            this.dataGridView1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(13, 446);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(169, 46);
            this.button1.TabIndex = 3;
            this.button1.Text = "单独方向运动";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.LightGray;
            this.groupBox3.Controls.Add(this.dataGridView2);
            this.groupBox3.Location = new System.Drawing.Point(10, 219);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(943, 165);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "块和大文件（）";
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToResizeColumns = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Enabled = false;
            this.dataGridView2.Location = new System.Drawing.Point(3, 21);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dataGridView2.RowTemplate.Height = 27;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView2.Size = new System.Drawing.Size(934, 141);
            this.dataGridView2.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.初始化命令ToolStripMenuItem,
            this.运动命令ToolStripMenuItem,
            this.延时命令ToolStripMenuItem,
            this.输入输出命令ToolStripMenuItem,
            this.循环命令ToolStripMenuItem,
            this.文件管理命令ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(965, 35);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 初始化命令ToolStripMenuItem
            // 
            this.初始化命令ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置地址命令ToolStripMenuItem,
            this.设置波特率命令ToolStripMenuItem,
            this.设置坐标系命令ToolStripMenuItem,
            this.设置输入有效电平ToolStripMenuItem,
            this.设置输出有效电平ToolStripMenuItem,
            this.设置坐标极限值ToolStripMenuItem,
            this.设置当前位置坐标为零ToolStripMenuItem,
            this.读电机坐标值ToolStripMenuItem,
            this.读点击参数ToolStripMenuItem,
            this.读极限值ToolStripMenuItem});
            this.初始化命令ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.初始化命令ToolStripMenuItem.Name = "初始化命令ToolStripMenuItem";
            this.初始化命令ToolStripMenuItem.Size = new System.Drawing.Size(124, 31);
            this.初始化命令ToolStripMenuItem.Text = "初始化命令";
            // 
            // 设置地址命令ToolStripMenuItem
            // 
            this.设置地址命令ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.设置地址命令ToolStripMenuItem.Name = "设置地址命令ToolStripMenuItem";
            this.设置地址命令ToolStripMenuItem.Size = new System.Drawing.Size(290, 32);
            this.设置地址命令ToolStripMenuItem.Text = "设置地址命令";
            this.设置地址命令ToolStripMenuItem.Click += new System.EventHandler(this.设置地址命令ToolStripMenuItem_Click);
            // 
            // 设置波特率命令ToolStripMenuItem
            // 
            this.设置波特率命令ToolStripMenuItem.Name = "设置波特率命令ToolStripMenuItem";
            this.设置波特率命令ToolStripMenuItem.Size = new System.Drawing.Size(290, 32);
            this.设置波特率命令ToolStripMenuItem.Text = "设置波特率命令";
            this.设置波特率命令ToolStripMenuItem.Click += new System.EventHandler(this.设置波特率命令ToolStripMenuItem_Click);
            // 
            // 设置坐标系命令ToolStripMenuItem
            // 
            this.设置坐标系命令ToolStripMenuItem.Name = "设置坐标系命令ToolStripMenuItem";
            this.设置坐标系命令ToolStripMenuItem.Size = new System.Drawing.Size(290, 32);
            this.设置坐标系命令ToolStripMenuItem.Text = "设置坐标系命令";
            this.设置坐标系命令ToolStripMenuItem.Click += new System.EventHandler(this.设置坐标系命令ToolStripMenuItem_Click);
            // 
            // 设置输入有效电平ToolStripMenuItem
            // 
            this.设置输入有效电平ToolStripMenuItem.Name = "设置输入有效电平ToolStripMenuItem";
            this.设置输入有效电平ToolStripMenuItem.Size = new System.Drawing.Size(290, 32);
            this.设置输入有效电平ToolStripMenuItem.Text = "设置输入有效电平";
            this.设置输入有效电平ToolStripMenuItem.Click += new System.EventHandler(this.设置输入有效电平ToolStripMenuItem_Click);
            // 
            // 设置输出有效电平ToolStripMenuItem
            // 
            this.设置输出有效电平ToolStripMenuItem.Name = "设置输出有效电平ToolStripMenuItem";
            this.设置输出有效电平ToolStripMenuItem.Size = new System.Drawing.Size(290, 32);
            this.设置输出有效电平ToolStripMenuItem.Text = "设置输出有效电平";
            this.设置输出有效电平ToolStripMenuItem.Click += new System.EventHandler(this.设置输出有效电平ToolStripMenuItem_Click);
            // 
            // 设置坐标极限值ToolStripMenuItem
            // 
            this.设置坐标极限值ToolStripMenuItem.Name = "设置坐标极限值ToolStripMenuItem";
            this.设置坐标极限值ToolStripMenuItem.Size = new System.Drawing.Size(290, 32);
            this.设置坐标极限值ToolStripMenuItem.Text = "设置坐标极限值";
            this.设置坐标极限值ToolStripMenuItem.Click += new System.EventHandler(this.设置坐标极限值ToolStripMenuItem_Click);
            // 
            // 设置当前位置坐标为零ToolStripMenuItem
            // 
            this.设置当前位置坐标为零ToolStripMenuItem.Name = "设置当前位置坐标为零ToolStripMenuItem";
            this.设置当前位置坐标为零ToolStripMenuItem.Size = new System.Drawing.Size(290, 32);
            this.设置当前位置坐标为零ToolStripMenuItem.Text = "设置当前位置坐标为零";
            this.设置当前位置坐标为零ToolStripMenuItem.Click += new System.EventHandler(this.设置当前位置坐标为零ToolStripMenuItem_Click);
            // 
            // 读电机坐标值ToolStripMenuItem
            // 
            this.读电机坐标值ToolStripMenuItem.Name = "读电机坐标值ToolStripMenuItem";
            this.读电机坐标值ToolStripMenuItem.Size = new System.Drawing.Size(290, 32);
            this.读电机坐标值ToolStripMenuItem.Text = "读电机坐标值";
            this.读电机坐标值ToolStripMenuItem.Click += new System.EventHandler(this.读电机坐标值ToolStripMenuItem_Click);
            // 
            // 读点击参数ToolStripMenuItem
            // 
            this.读点击参数ToolStripMenuItem.Name = "读点击参数ToolStripMenuItem";
            this.读点击参数ToolStripMenuItem.Size = new System.Drawing.Size(290, 32);
            this.读点击参数ToolStripMenuItem.Text = "读电机参数";
            this.读点击参数ToolStripMenuItem.Click += new System.EventHandler(this.读点击参数ToolStripMenuItem_Click);
            // 
            // 读极限值ToolStripMenuItem
            // 
            this.读极限值ToolStripMenuItem.Name = "读极限值ToolStripMenuItem";
            this.读极限值ToolStripMenuItem.Size = new System.Drawing.Size(290, 32);
            this.读极限值ToolStripMenuItem.Text = "读极限值";
            this.读极限值ToolStripMenuItem.Click += new System.EventHandler(this.读极限值ToolStripMenuItem_Click);
            // 
            // 运动命令ToolStripMenuItem
            // 
            this.运动命令ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.单步运动命令ToolStripMenuItem,
            this.匀速定步长运动ToolStripMenuItem,
            this.变速定步长运动ToolStripMenuItem,
            this.匀速运动到规定通道置位ToolStripMenuItem,
            this.变速运动到规定通道置位ToolStripMenuItem,
            this.低速归零命令ToolStripMenuItem,
            this.高速归原点ToolStripMenuItem});
            this.运动命令ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.运动命令ToolStripMenuItem.Name = "运动命令ToolStripMenuItem";
            this.运动命令ToolStripMenuItem.Size = new System.Drawing.Size(104, 31);
            this.运动命令ToolStripMenuItem.Text = "运动命令";
            // 
            // 单步运动命令ToolStripMenuItem
            // 
            this.单步运动命令ToolStripMenuItem.Name = "单步运动命令ToolStripMenuItem";
            this.单步运动命令ToolStripMenuItem.Size = new System.Drawing.Size(310, 32);
            this.单步运动命令ToolStripMenuItem.Text = "单步运动命令";
            this.单步运动命令ToolStripMenuItem.Click += new System.EventHandler(this.单步运动命令ToolStripMenuItem_Click);
            // 
            // 匀速定步长运动ToolStripMenuItem
            // 
            this.匀速定步长运动ToolStripMenuItem.Name = "匀速定步长运动ToolStripMenuItem";
            this.匀速定步长运动ToolStripMenuItem.Size = new System.Drawing.Size(310, 32);
            this.匀速定步长运动ToolStripMenuItem.Text = "匀速定步长运动";
            this.匀速定步长运动ToolStripMenuItem.Click += new System.EventHandler(this.匀速定步长运动ToolStripMenuItem_Click);
            // 
            // 变速定步长运动ToolStripMenuItem
            // 
            this.变速定步长运动ToolStripMenuItem.Name = "变速定步长运动ToolStripMenuItem";
            this.变速定步长运动ToolStripMenuItem.Size = new System.Drawing.Size(310, 32);
            this.变速定步长运动ToolStripMenuItem.Text = "变速定步长运动";
            this.变速定步长运动ToolStripMenuItem.Click += new System.EventHandler(this.变速定步长运动ToolStripMenuItem_Click);
            // 
            // 匀速运动到规定通道置位ToolStripMenuItem
            // 
            this.匀速运动到规定通道置位ToolStripMenuItem.Name = "匀速运动到规定通道置位ToolStripMenuItem";
            this.匀速运动到规定通道置位ToolStripMenuItem.Size = new System.Drawing.Size(310, 32);
            this.匀速运动到规定通道置位ToolStripMenuItem.Text = "匀速运动到规定通道置位";
            this.匀速运动到规定通道置位ToolStripMenuItem.Click += new System.EventHandler(this.匀速运动到规定通道置位ToolStripMenuItem_Click);
            // 
            // 变速运动到规定通道置位ToolStripMenuItem
            // 
            this.变速运动到规定通道置位ToolStripMenuItem.Name = "变速运动到规定通道置位ToolStripMenuItem";
            this.变速运动到规定通道置位ToolStripMenuItem.Size = new System.Drawing.Size(310, 32);
            this.变速运动到规定通道置位ToolStripMenuItem.Text = "变速运动到规定通道置位";
            this.变速运动到规定通道置位ToolStripMenuItem.Click += new System.EventHandler(this.变速运动到规定通道置位ToolStripMenuItem_Click);
            // 
            // 低速归零命令ToolStripMenuItem
            // 
            this.低速归零命令ToolStripMenuItem.Name = "低速归零命令ToolStripMenuItem";
            this.低速归零命令ToolStripMenuItem.Size = new System.Drawing.Size(310, 32);
            this.低速归零命令ToolStripMenuItem.Text = "低速归零命令";
            this.低速归零命令ToolStripMenuItem.Click += new System.EventHandler(this.低速归零命令ToolStripMenuItem_Click);
            // 
            // 高速归原点ToolStripMenuItem
            // 
            this.高速归原点ToolStripMenuItem.Name = "高速归原点ToolStripMenuItem";
            this.高速归原点ToolStripMenuItem.Size = new System.Drawing.Size(310, 32);
            this.高速归原点ToolStripMenuItem.Text = "高速归原点";
            this.高速归原点ToolStripMenuItem.Click += new System.EventHandler(this.高速归原点ToolStripMenuItem_Click);
            // 
            // 延时命令ToolStripMenuItem
            // 
            this.延时命令ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.等待命令1ToolStripMenuItem,
            this.等待命令2ToolStripMenuItem,
            this.等待通道置位命令ToolStripMenuItem});
            this.延时命令ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.延时命令ToolStripMenuItem.Name = "延时命令ToolStripMenuItem";
            this.延时命令ToolStripMenuItem.Size = new System.Drawing.Size(104, 31);
            this.延时命令ToolStripMenuItem.Text = "延时命令";
            // 
            // 等待命令1ToolStripMenuItem
            // 
            this.等待命令1ToolStripMenuItem.Name = "等待命令1ToolStripMenuItem";
            this.等待命令1ToolStripMenuItem.Size = new System.Drawing.Size(250, 32);
            this.等待命令1ToolStripMenuItem.Text = "等待命令1";
            this.等待命令1ToolStripMenuItem.Click += new System.EventHandler(this.等待命令1ToolStripMenuItem_Click);
            // 
            // 等待命令2ToolStripMenuItem
            // 
            this.等待命令2ToolStripMenuItem.Name = "等待命令2ToolStripMenuItem";
            this.等待命令2ToolStripMenuItem.Size = new System.Drawing.Size(250, 32);
            this.等待命令2ToolStripMenuItem.Text = "等待命令2";
            this.等待命令2ToolStripMenuItem.Click += new System.EventHandler(this.等待命令2ToolStripMenuItem_Click);
            // 
            // 等待通道置位命令ToolStripMenuItem
            // 
            this.等待通道置位命令ToolStripMenuItem.Name = "等待通道置位命令ToolStripMenuItem";
            this.等待通道置位命令ToolStripMenuItem.Size = new System.Drawing.Size(250, 32);
            this.等待通道置位命令ToolStripMenuItem.Text = "等待通道置位命令";
            this.等待通道置位命令ToolStripMenuItem.Click += new System.EventHandler(this.等待通道置位命令ToolStripMenuItem_Click);
            // 
            // 输入输出命令ToolStripMenuItem
            // 
            this.输入输出命令ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.输出命令ToolStripMenuItem});
            this.输入输出命令ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.输入输出命令ToolStripMenuItem.Name = "输入输出命令ToolStripMenuItem";
            this.输入输出命令ToolStripMenuItem.Size = new System.Drawing.Size(144, 31);
            this.输入输出命令ToolStripMenuItem.Text = "输入输出命令";
            // 
            // 输出命令ToolStripMenuItem
            // 
            this.输出命令ToolStripMenuItem.Name = "输出命令ToolStripMenuItem";
            this.输出命令ToolStripMenuItem.Size = new System.Drawing.Size(170, 32);
            this.输出命令ToolStripMenuItem.Text = "输出命令";
            this.输出命令ToolStripMenuItem.Click += new System.EventHandler(this.输出命令ToolStripMenuItem_Click);
            // 
            // 循环命令ToolStripMenuItem
            // 
            this.循环命令ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.循环开始ToolStripMenuItem,
            this.循环结束ToolStripMenuItem});
            this.循环命令ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.循环命令ToolStripMenuItem.Name = "循环命令ToolStripMenuItem";
            this.循环命令ToolStripMenuItem.Size = new System.Drawing.Size(104, 31);
            this.循环命令ToolStripMenuItem.Text = "循环命令";
            // 
            // 循环开始ToolStripMenuItem
            // 
            this.循环开始ToolStripMenuItem.Name = "循环开始ToolStripMenuItem";
            this.循环开始ToolStripMenuItem.Size = new System.Drawing.Size(170, 32);
            this.循环开始ToolStripMenuItem.Text = "循环开始";
            this.循环开始ToolStripMenuItem.Click += new System.EventHandler(this.循环开始ToolStripMenuItem_Click);
            // 
            // 循环结束ToolStripMenuItem
            // 
            this.循环结束ToolStripMenuItem.Name = "循环结束ToolStripMenuItem";
            this.循环结束ToolStripMenuItem.Size = new System.Drawing.Size(170, 32);
            this.循环结束ToolStripMenuItem.Text = "循环结束";
            this.循环结束ToolStripMenuItem.Click += new System.EventHandler(this.循环结束ToolStripMenuItem_Click);
            // 
            // 文件管理命令ToolStripMenuItem
            // 
            this.文件管理命令ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.擦除扇区命令ToolStripMenuItem,
            this.擦除块命令ToolStripMenuItem,
            this.擦除存储器芯片命令ToolStripMenuItem,
            this.传送文件到扇区ToolStripMenuItem,
            this.传送文件到快区ToolStripMenuItem,
            this.读页命令ToolStripMenuItem,
            this.读扇区命令ToolStripMenuItem,
            this.读块命令ToolStripMenuItem,
            this.通用命令执行ToolStripMenuItem,
            this.轴轨迹运动执行ToolStripMenuItem,
            this.传送文件到页ToolStripMenuItem});
            this.文件管理命令ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.文件管理命令ToolStripMenuItem.Name = "文件管理命令ToolStripMenuItem";
            this.文件管理命令ToolStripMenuItem.Size = new System.Drawing.Size(144, 31);
            this.文件管理命令ToolStripMenuItem.Text = "文件管理命令";
            // 
            // 擦除扇区命令ToolStripMenuItem
            // 
            this.擦除扇区命令ToolStripMenuItem.Name = "擦除扇区命令ToolStripMenuItem";
            this.擦除扇区命令ToolStripMenuItem.Size = new System.Drawing.Size(270, 32);
            this.擦除扇区命令ToolStripMenuItem.Text = "擦除扇区命令";
            this.擦除扇区命令ToolStripMenuItem.Click += new System.EventHandler(this.擦除扇区命令ToolStripMenuItem_Click);
            // 
            // 擦除块命令ToolStripMenuItem
            // 
            this.擦除块命令ToolStripMenuItem.Name = "擦除块命令ToolStripMenuItem";
            this.擦除块命令ToolStripMenuItem.Size = new System.Drawing.Size(270, 32);
            this.擦除块命令ToolStripMenuItem.Text = "擦除块命令";
            this.擦除块命令ToolStripMenuItem.Click += new System.EventHandler(this.擦除块命令ToolStripMenuItem_Click);
            // 
            // 擦除存储器芯片命令ToolStripMenuItem
            // 
            this.擦除存储器芯片命令ToolStripMenuItem.Name = "擦除存储器芯片命令ToolStripMenuItem";
            this.擦除存储器芯片命令ToolStripMenuItem.Size = new System.Drawing.Size(270, 32);
            this.擦除存储器芯片命令ToolStripMenuItem.Text = "擦除存储器芯片命令";
            this.擦除存储器芯片命令ToolStripMenuItem.Click += new System.EventHandler(this.擦除存储器芯片命令ToolStripMenuItem_Click);
            // 
            // 传送文件到扇区ToolStripMenuItem
            // 
            this.传送文件到扇区ToolStripMenuItem.Name = "传送文件到扇区ToolStripMenuItem";
            this.传送文件到扇区ToolStripMenuItem.Size = new System.Drawing.Size(270, 32);
            this.传送文件到扇区ToolStripMenuItem.Text = "传送文件到扇区";
            this.传送文件到扇区ToolStripMenuItem.Click += new System.EventHandler(this.传送文件到扇区ToolStripMenuItem_Click);
            // 
            // 传送文件到快区ToolStripMenuItem
            // 
            this.传送文件到快区ToolStripMenuItem.Name = "传送文件到快区ToolStripMenuItem";
            this.传送文件到快区ToolStripMenuItem.Size = new System.Drawing.Size(270, 32);
            this.传送文件到快区ToolStripMenuItem.Text = "传送文件到块/区";
            this.传送文件到快区ToolStripMenuItem.Click += new System.EventHandler(this.传送文件到快区ToolStripMenuItem_Click);
            // 
            // 读页命令ToolStripMenuItem
            // 
            this.读页命令ToolStripMenuItem.Name = "读页命令ToolStripMenuItem";
            this.读页命令ToolStripMenuItem.Size = new System.Drawing.Size(270, 32);
            this.读页命令ToolStripMenuItem.Text = "读页命令";
            this.读页命令ToolStripMenuItem.Click += new System.EventHandler(this.读页命令ToolStripMenuItem_Click);
            // 
            // 读扇区命令ToolStripMenuItem
            // 
            this.读扇区命令ToolStripMenuItem.Name = "读扇区命令ToolStripMenuItem";
            this.读扇区命令ToolStripMenuItem.Size = new System.Drawing.Size(270, 32);
            this.读扇区命令ToolStripMenuItem.Text = "读扇区命令";
            this.读扇区命令ToolStripMenuItem.Click += new System.EventHandler(this.读扇区命令ToolStripMenuItem_Click);
            // 
            // 读块命令ToolStripMenuItem
            // 
            this.读块命令ToolStripMenuItem.Name = "读块命令ToolStripMenuItem";
            this.读块命令ToolStripMenuItem.Size = new System.Drawing.Size(270, 32);
            this.读块命令ToolStripMenuItem.Text = "读块命令";
            this.读块命令ToolStripMenuItem.Click += new System.EventHandler(this.读块命令ToolStripMenuItem_Click);
            // 
            // 通用命令执行ToolStripMenuItem
            // 
            this.通用命令执行ToolStripMenuItem.Name = "通用命令执行ToolStripMenuItem";
            this.通用命令执行ToolStripMenuItem.Size = new System.Drawing.Size(270, 32);
            this.通用命令执行ToolStripMenuItem.Text = "通用命令执行";
            this.通用命令执行ToolStripMenuItem.Click += new System.EventHandler(this.通用命令执行ToolStripMenuItem_Click);
            // 
            // 轴轨迹运动执行ToolStripMenuItem
            // 
            this.轴轨迹运动执行ToolStripMenuItem.Name = "轴轨迹运动执行ToolStripMenuItem";
            this.轴轨迹运动执行ToolStripMenuItem.Size = new System.Drawing.Size(270, 32);
            this.轴轨迹运动执行ToolStripMenuItem.Text = "4轴轨迹运动执行";
            this.轴轨迹运动执行ToolStripMenuItem.Click += new System.EventHandler(this.轴轨迹运动执行ToolStripMenuItem_Click);
            // 
            // 传送文件到页ToolStripMenuItem
            // 
            this.传送文件到页ToolStripMenuItem.Name = "传送文件到页ToolStripMenuItem";
            this.传送文件到页ToolStripMenuItem.Size = new System.Drawing.Size(270, 32);
            this.传送文件到页ToolStripMenuItem.Text = "传送文件到页";
            this.传送文件到页ToolStripMenuItem.Click += new System.EventHandler(this.传送文件到页ToolStripMenuItem_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(965, 665);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "控制程序";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 初始化命令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置地址命令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置波特率命令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置坐标系命令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置输入有效电平ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置输出有效电平ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置坐标极限值ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置当前位置坐标为零ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 读电机坐标值ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 读点击参数ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 读极限值ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 运动命令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 单步运动命令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 匀速定步长运动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 变速定步长运动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 匀速运动到规定通道置位ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 变速运动到规定通道置位ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 低速归零命令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 高速归原点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 延时命令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 等待命令1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 等待命令2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 等待通道置位命令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 输入输出命令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 输出命令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 循环命令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 循环开始ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 循环结束ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 文件管理命令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 擦除扇区命令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 擦除块命令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 擦除存储器芯片命令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 传送文件到扇区ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 传送文件到快区ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 读页命令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 读扇区命令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 读块命令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 通用命令执行ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 轴轨迹运动执行ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 传送文件到页ToolStripMenuItem;
    }
}