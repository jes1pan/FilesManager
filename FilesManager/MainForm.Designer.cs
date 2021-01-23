namespace FilesManager
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lblPoint = new System.Windows.Forms.Label();
            this.lblSync = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.cbFtpSave = new System.Windows.Forms.CheckBox();
            this.cbFtpSel = new System.Windows.Forms.CheckBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnQuery = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.txtSaveFolder = new System.Windows.Forms.TextBox();
            this.btnSaveFolder = new System.Windows.Forms.Button();
            this.txtSelectFolder = new System.Windows.Forms.TextBox();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblLoading = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblPoint);
            this.splitContainer1.Panel1.Controls.Add(this.lblSync);
            this.splitContainer1.Panel1.Controls.Add(this.lblCount);
            this.splitContainer1.Panel1.Controls.Add(this.cbFtpSave);
            this.splitContainer1.Panel1.Controls.Add(this.cbFtpSel);
            this.splitContainer1.Panel1.Controls.Add(this.btnClear);
            this.splitContainer1.Panel1.Controls.Add(this.btnSave);
            this.splitContainer1.Panel1.Controls.Add(this.btnQuery);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.dtpEnd);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.dtpStart);
            this.splitContainer1.Panel1.Controls.Add(this.txtSaveFolder);
            this.splitContainer1.Panel1.Controls.Add(this.btnSaveFolder);
            this.splitContainer1.Panel1.Controls.Add(this.txtSelectFolder);
            this.splitContainer1.Panel1.Controls.Add(this.btnSelectFolder);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblLoading);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(1045, 565);
            this.splitContainer1.SplitterDistance = 124;
            this.splitContainer1.TabIndex = 4;
            // 
            // lblPoint
            // 
            this.lblPoint.AutoSize = true;
            this.lblPoint.Location = new System.Drawing.Point(486, 92);
            this.lblPoint.Name = "lblPoint";
            this.lblPoint.Size = new System.Drawing.Size(19, 19);
            this.lblPoint.TabIndex = 19;
            this.lblPoint.Text = " ";
            this.lblPoint.Visible = false;
            // 
            // lblSync
            // 
            this.lblSync.AutoSize = true;
            this.lblSync.Location = new System.Drawing.Point(413, 92);
            this.lblSync.Name = "lblSync";
            this.lblSync.Size = new System.Drawing.Size(85, 19);
            this.lblSync.TabIndex = 18;
            this.lblSync.Text = "已同步：";
            this.lblSync.Visible = false;
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(198, 92);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(66, 19);
            this.lblCount.TabIndex = 17;
            this.lblCount.Text = "合计：";
            this.lblCount.Visible = false;
            // 
            // cbFtpSave
            // 
            this.cbFtpSave.AutoSize = true;
            this.cbFtpSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbFtpSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbFtpSave.Location = new System.Drawing.Point(984, 15);
            this.cbFtpSave.Name = "cbFtpSave";
            this.cbFtpSave.Size = new System.Drawing.Size(58, 23);
            this.cbFtpSave.TabIndex = 16;
            this.cbFtpSave.Text = "FTP";
            this.cbFtpSave.UseVisualStyleBackColor = true;
            // 
            // cbFtpSel
            // 
            this.cbFtpSel.AutoSize = true;
            this.cbFtpSel.BackColor = System.Drawing.Color.Transparent;
            this.cbFtpSel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbFtpSel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbFtpSel.Location = new System.Drawing.Point(464, 14);
            this.cbFtpSel.Name = "cbFtpSel";
            this.cbFtpSel.Size = new System.Drawing.Size(58, 23);
            this.cbFtpSel.TabIndex = 15;
            this.cbFtpSel.Text = "FTP";
            this.cbFtpSel.UseVisualStyleBackColor = false;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(669, 54);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(98, 28);
            this.btnClear.TabIndex = 14;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(893, 54);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(98, 28);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(781, 54);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(98, 28);
            this.btnQuery.TabIndex = 12;
            this.btnQuery.Text = "搜索";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(379, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 19);
            this.label2.TabIndex = 11;
            this.label2.Text = "—";
            // 
            // dtpEnd
            // 
            this.dtpEnd.CustomFormat = " ";
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(413, 52);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(238, 28);
            this.dtpEnd.TabIndex = 10;
            this.dtpEnd.Tag = "";
            this.dtpEnd.Value = new System.DateTime(2021, 1, 1, 0, 0, 0, 0);
            this.dtpEnd.ValueChanged += new System.EventHandler(this.dtp_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 19);
            this.label1.TabIndex = 9;
            this.label1.Text = "修改日期";
            // 
            // dtpStart
            // 
            this.dtpStart.CustomFormat = " ";
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dtpStart.Location = new System.Drawing.Point(135, 52);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(238, 28);
            this.dtpStart.TabIndex = 8;
            this.dtpStart.Value = new System.DateTime(2021, 1, 1, 0, 0, 0, 0);
            this.dtpStart.ValueChanged += new System.EventHandler(this.dtp_ValueChanged);
            // 
            // txtSaveFolder
            // 
            this.txtSaveFolder.Location = new System.Drawing.Point(669, 13);
            this.txtSaveFolder.Name = "txtSaveFolder";
            this.txtSaveFolder.Size = new System.Drawing.Size(314, 28);
            this.txtSaveFolder.TabIndex = 7;
            // 
            // btnSaveFolder
            // 
            this.btnSaveFolder.Location = new System.Drawing.Point(527, 12);
            this.btnSaveFolder.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSaveFolder.Name = "btnSaveFolder";
            this.btnSaveFolder.Size = new System.Drawing.Size(136, 28);
            this.btnSaveFolder.TabIndex = 6;
            this.btnSaveFolder.Text = "选择保存目录";
            this.btnSaveFolder.UseVisualStyleBackColor = true;
            this.btnSaveFolder.Click += new System.EventHandler(this.btnSaveFolder_Click);
            // 
            // txtSelectFolder
            // 
            this.txtSelectFolder.Location = new System.Drawing.Point(144, 13);
            this.txtSelectFolder.Name = "txtSelectFolder";
            this.txtSelectFolder.Size = new System.Drawing.Size(314, 28);
            this.txtSelectFolder.TabIndex = 5;
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(3, 12);
            this.btnSelectFolder.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(136, 28);
            this.btnSelectFolder.TabIndex = 4;
            this.btnSelectFolder.Text = "选择读取目录";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1045, 437);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            // 
            // lblLoading
            // 
            this.lblLoading.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.lblLoading.Image = ((System.Drawing.Image)(resources.GetObject("lblLoading.Image")));
            this.lblLoading.Location = new System.Drawing.Point(445, 103);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(162, 159);
            this.lblLoading.TabIndex = 1;
            this.lblLoading.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 565);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("宋体", 11F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.Text = "文件管理";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtSaveFolder;
        private System.Windows.Forms.Button btnSaveFolder;
        private System.Windows.Forms.TextBox txtSelectFolder;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.CheckBox cbFtpSave;
        private System.Windows.Forms.CheckBox cbFtpSel;
        private System.Windows.Forms.Label lblSync;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label lblPoint;
        private System.Windows.Forms.Label lblLoading;
    }
}

