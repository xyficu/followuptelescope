namespace Follow_Up_Telescope
{
    partial class FormFUTMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.系统ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.选项OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.参数PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.labelWMovStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelWCurPos = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.timerUpdateStatus = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.labelWConnStatus = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.系统ToolStripMenuItem,
            this.选项OToolStripMenuItem,
            this.关于AboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(855, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 系统ToolStripMenuItem
            // 
            this.系统ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.退出EToolStripMenuItem});
            this.系统ToolStripMenuItem.Name = "系统ToolStripMenuItem";
            this.系统ToolStripMenuItem.Size = new System.Drawing.Size(59, 21);
            this.系统ToolStripMenuItem.Text = "系统(&S)";
            // 
            // 退出EToolStripMenuItem
            // 
            this.退出EToolStripMenuItem.Name = "退出EToolStripMenuItem";
            this.退出EToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.退出EToolStripMenuItem.Text = "退出(&E)";
            // 
            // 选项OToolStripMenuItem
            // 
            this.选项OToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.参数PToolStripMenuItem});
            this.选项OToolStripMenuItem.Name = "选项OToolStripMenuItem";
            this.选项OToolStripMenuItem.Size = new System.Drawing.Size(62, 21);
            this.选项OToolStripMenuItem.Text = "选项(&O)";
            // 
            // 参数PToolStripMenuItem
            // 
            this.参数PToolStripMenuItem.Name = "参数PToolStripMenuItem";
            this.参数PToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.参数PToolStripMenuItem.Text = "参数(&P)";
            // 
            // 关于AboutToolStripMenuItem
            // 
            this.关于AboutToolStripMenuItem.Name = "关于AboutToolStripMenuItem";
            this.关于AboutToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
            this.关于AboutToolStripMenuItem.Text = "关于(&A)";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(12, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 433);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "时间信息";
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(218, 28);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(254, 153);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "转台状态";
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(478, 28);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(254, 153);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "CCD状态";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.labelWConnStatus);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.labelWMovStatus);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.labelWCurPos);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Location = new System.Drawing.Point(218, 294);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(258, 167);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "滤光片转轮状态";
            // 
            // labelWMovStatus
            // 
            this.labelWMovStatus.AutoSize = true;
            this.labelWMovStatus.Location = new System.Drawing.Point(108, 135);
            this.labelWMovStatus.Name = "labelWMovStatus";
            this.labelWMovStatus.Size = new System.Drawing.Size(53, 12);
            this.labelWMovStatus.TabIndex = 3;
            this.labelWMovStatus.Text = "stopped.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "当前状态:";
            // 
            // labelWCurPos
            // 
            this.labelWCurPos.AutoSize = true;
            this.labelWCurPos.Location = new System.Drawing.Point(108, 100);
            this.labelWCurPos.Name = "labelWCurPos";
            this.labelWCurPos.Size = new System.Drawing.Size(47, 12);
            this.labelWCurPos.TabIndex = 1;
            this.labelWCurPos.Text = "unknown";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前位置:";
            // 
            // groupBox5
            // 
            this.groupBox5.Location = new System.Drawing.Point(482, 294);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(274, 167);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "调焦器状态";
            // 
            // timerUpdateStatus
            // 
            this.timerUpdateStatus.Tick += new System.EventHandler(this.timerUpdateStatus_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "连接状态:";
            // 
            // labelWConnStatus
            // 
            this.labelWConnStatus.AutoSize = true;
            this.labelWConnStatus.Location = new System.Drawing.Point(110, 48);
            this.labelWConnStatus.Name = "labelWConnStatus";
            this.labelWConnStatus.Size = new System.Drawing.Size(41, 12);
            this.labelWConnStatus.TabIndex = 5;
            this.labelWConnStatus.Text = "未连接";
            // 
            // FormFUTMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 514);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormFUTMain";
            this.Text = "Follow-up Telescope";
            this.Load += new System.EventHandler(this.FormFUTMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 系统ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出EToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于AboutToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label labelWMovStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelWCurPos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem 选项OToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 参数PToolStripMenuItem;
        private System.Windows.Forms.Timer timerUpdateStatus;
        private System.Windows.Forms.Label labelWConnStatus;
        private System.Windows.Forms.Label label2;
    }
}

