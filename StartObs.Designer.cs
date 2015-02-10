namespace Follow_Up_Telescope
{
    partial class StartObs
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeaderNumb = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderRA = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDEC = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderColor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderExpTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAmount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonStartObs = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonAddTarget = new System.Windows.Forms.Button();
            this.textBoxAmount = new System.Windows.Forms.TextBox();
            this.textBoxExpTime = new System.Windows.Forms.TextBox();
            this.textBoxColor = new System.Windows.Forms.TextBox();
            this.textBoxDEC = new System.Windows.Forms.TextBox();
            this.textBoxRA = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.columnHeaderFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonDelTarget = new System.Windows.Forms.Button();
            this.buttonInit = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderNumb,
            this.columnHeaderRA,
            this.columnHeaderDEC,
            this.columnHeaderColor,
            this.columnHeaderExpTime,
            this.columnHeaderAmount,
            this.columnHeaderFileName});
            this.listView1.Location = new System.Drawing.Point(23, 22);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(436, 242);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderNumb
            // 
            this.columnHeaderNumb.Text = "序号";
            // 
            // columnHeaderRA
            // 
            this.columnHeaderRA.Text = "RA";
            // 
            // columnHeaderDEC
            // 
            this.columnHeaderDEC.Text = "DEC";
            // 
            // columnHeaderColor
            // 
            this.columnHeaderColor.Text = "滤光片";
            // 
            // columnHeaderExpTime
            // 
            this.columnHeaderExpTime.Text = "曝光时间";
            // 
            // columnHeaderAmount
            // 
            this.columnHeaderAmount.Text = "拍摄张数";
            // 
            // buttonStartObs
            // 
            this.buttonStartObs.Location = new System.Drawing.Point(696, 241);
            this.buttonStartObs.Name = "buttonStartObs";
            this.buttonStartObs.Size = new System.Drawing.Size(75, 23);
            this.buttonStartObs.TabIndex = 2;
            this.buttonStartObs.Text = "开始观测";
            this.buttonStartObs.UseVisualStyleBackColor = true;
            this.buttonStartObs.Click += new System.EventHandler(this.buttonStartObs_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonAddTarget);
            this.groupBox1.Controls.Add(this.buttonDelTarget);
            this.groupBox1.Controls.Add(this.textBoxFileName);
            this.groupBox1.Controls.Add(this.textBoxAmount);
            this.groupBox1.Controls.Add(this.textBoxExpTime);
            this.groupBox1.Controls.Add(this.textBoxColor);
            this.groupBox1.Controls.Add(this.textBoxDEC);
            this.groupBox1.Controls.Add(this.textBoxRA);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(465, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 213);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "添加目标";
            // 
            // buttonAddTarget
            // 
            this.buttonAddTarget.Location = new System.Drawing.Point(231, 53);
            this.buttonAddTarget.Name = "buttonAddTarget";
            this.buttonAddTarget.Size = new System.Drawing.Size(75, 23);
            this.buttonAddTarget.TabIndex = 10;
            this.buttonAddTarget.Text = "添加";
            this.buttonAddTarget.UseVisualStyleBackColor = true;
            this.buttonAddTarget.Click += new System.EventHandler(this.buttonAddTarget_Click);
            // 
            // textBoxAmount
            // 
            this.textBoxAmount.Location = new System.Drawing.Point(112, 142);
            this.textBoxAmount.Name = "textBoxAmount";
            this.textBoxAmount.Size = new System.Drawing.Size(100, 21);
            this.textBoxAmount.TabIndex = 9;
            // 
            // textBoxExpTime
            // 
            this.textBoxExpTime.Location = new System.Drawing.Point(113, 113);
            this.textBoxExpTime.Name = "textBoxExpTime";
            this.textBoxExpTime.Size = new System.Drawing.Size(100, 21);
            this.textBoxExpTime.TabIndex = 8;
            // 
            // textBoxColor
            // 
            this.textBoxColor.Location = new System.Drawing.Point(112, 84);
            this.textBoxColor.Name = "textBoxColor";
            this.textBoxColor.Size = new System.Drawing.Size(100, 21);
            this.textBoxColor.TabIndex = 7;
            // 
            // textBoxDEC
            // 
            this.textBoxDEC.Location = new System.Drawing.Point(113, 55);
            this.textBoxDEC.Name = "textBoxDEC";
            this.textBoxDEC.Size = new System.Drawing.Size(100, 21);
            this.textBoxDEC.TabIndex = 6;
            // 
            // textBoxRA
            // 
            this.textBoxRA.Location = new System.Drawing.Point(113, 26);
            this.textBoxRA.Name = "textBoxRA";
            this.textBoxRA.Size = new System.Drawing.Size(100, 21);
            this.textBoxRA.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 146);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "拍摄张数：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "曝光时间：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "滤光片：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(65, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "赤纬：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "赤经：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(53, 176);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "文件名：";
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Location = new System.Drawing.Point(112, 172);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Size = new System.Drawing.Size(100, 21);
            this.textBoxFileName.TabIndex = 9;
            // 
            // columnHeaderFileName
            // 
            this.columnHeaderFileName.Text = "文件名";
            // 
            // buttonDelTarget
            // 
            this.buttonDelTarget.Location = new System.Drawing.Point(231, 106);
            this.buttonDelTarget.Name = "buttonDelTarget";
            this.buttonDelTarget.Size = new System.Drawing.Size(75, 23);
            this.buttonDelTarget.TabIndex = 11;
            this.buttonDelTarget.Text = "删除";
            this.buttonDelTarget.UseVisualStyleBackColor = true;
            this.buttonDelTarget.Click += new System.EventHandler(this.buttonDelTarget_Click);
            // 
            // buttonInit
            // 
            this.buttonInit.Location = new System.Drawing.Point(496, 241);
            this.buttonInit.Name = "buttonInit";
            this.buttonInit.Size = new System.Drawing.Size(75, 23);
            this.buttonInit.TabIndex = 12;
            this.buttonInit.Text = "初始化";
            this.buttonInit.UseVisualStyleBackColor = true;
            this.buttonInit.Click += new System.EventHandler(this.buttonInit_Click);
            // 
            // StartObs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 428);
            this.Controls.Add(this.buttonInit);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonStartObs);
            this.Controls.Add(this.listView1);
            this.Name = "StartObs";
            this.Text = "StartObs";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StartObs_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button buttonStartObs;
        private System.Windows.Forms.ColumnHeader columnHeaderNumb;
        private System.Windows.Forms.ColumnHeader columnHeaderRA;
        private System.Windows.Forms.ColumnHeader columnHeaderDEC;
        private System.Windows.Forms.ColumnHeader columnHeaderExpTime;
        private System.Windows.Forms.ColumnHeader columnHeaderColor;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxAmount;
        private System.Windows.Forms.TextBox textBoxExpTime;
        private System.Windows.Forms.TextBox textBoxColor;
        private System.Windows.Forms.TextBox textBoxDEC;
        private System.Windows.Forms.TextBox textBoxRA;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonAddTarget;
        private System.Windows.Forms.ColumnHeader columnHeaderAmount;
        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ColumnHeader columnHeaderFileName;
        private System.Windows.Forms.Button buttonDelTarget;
        private System.Windows.Forms.Button buttonInit;


    }
}