namespace ClientUpdate
{
    partial class FrmDownload
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDownload));
            this.timerSpeed = new System.Windows.Forms.Timer(this.components);
            this.pgbarDownload = new System.Windows.Forms.ProgressBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbFileSize = new System.Windows.Forms.Label();
            this.lbDlSize = new System.Windows.Forms.Label();
            this.lbTime = new System.Windows.Forms.Label();
            this.lbSpeed = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerSpeed
            // 
            this.timerSpeed.Interval = 1000;
            this.timerSpeed.Tick += new System.EventHandler(this.timerSpeed_Tick);
            // 
            // pgbarDownload
            // 
            this.pgbarDownload.Location = new System.Drawing.Point(12, 25);
            this.pgbarDownload.Name = "pgbarDownload";
            this.pgbarDownload.Size = new System.Drawing.Size(641, 23);
            this.pgbarDownload.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbFileSize);
            this.groupBox1.Controls.Add(this.lbDlSize);
            this.groupBox1.Controls.Add(this.lbTime);
            this.groupBox1.Controls.Add(this.lbSpeed);
            this.groupBox1.Location = new System.Drawing.Point(12, 65);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(641, 255);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "下载信息";
            // 
            // lbFileSize
            // 
            this.lbFileSize.AutoSize = true;
            this.lbFileSize.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbFileSize.Location = new System.Drawing.Point(395, 155);
            this.lbFileSize.Name = "lbFileSize";
            this.lbFileSize.Size = new System.Drawing.Size(187, 28);
            this.lbFileSize.TabIndex = 1;
            this.lbFileSize.Text = "文件大小：800MB";
            // 
            // lbDlSize
            // 
            this.lbDlSize.AutoSize = true;
            this.lbDlSize.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDlSize.Location = new System.Drawing.Point(395, 88);
            this.lbDlSize.Name = "lbDlSize";
            this.lbDlSize.Size = new System.Drawing.Size(142, 28);
            this.lbDlSize.TabIndex = 1;
            this.lbDlSize.Text = "已下载：0MB";
            // 
            // lbTime
            // 
            this.lbTime.AutoSize = true;
            this.lbTime.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTime.Location = new System.Drawing.Point(26, 155);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(240, 28);
            this.lbTime.TabIndex = 0;
            this.lbTime.Text = "剩余时间：0时25分32秒";
            // 
            // lbSpeed
            // 
            this.lbSpeed.AutoSize = true;
            this.lbSpeed.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbSpeed.Location = new System.Drawing.Point(26, 88);
            this.lbSpeed.Name = "lbSpeed";
            this.lbSpeed.Size = new System.Drawing.Size(219, 28);
            this.lbSpeed.TabIndex = 0;
            this.lbSpeed.Text = "下载速度：1.78 MB/S";
            // 
            // FrmDownload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 369);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pgbarDownload);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmDownload";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "下载更新程序-沟通平台";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmDownload_FormClosing);
            this.Load += new System.EventHandler(this.FrmDownload_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerSpeed;
        private System.Windows.Forms.ProgressBar pgbarDownload;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbDlSize;
        private System.Windows.Forms.Label lbTime;
        private System.Windows.Forms.Label lbSpeed;
        private System.Windows.Forms.Label lbFileSize;
    }
}