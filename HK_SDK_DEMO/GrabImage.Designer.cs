
namespace HK_SDK_DEMO
{
    partial class GrabImage
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.cbCamList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStartGrabbing = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbSoftware = new System.Windows.Forms.RadioButton();
            this.rbHardware = new System.Windows.Forms.RadioButton();
            this.btnStopGrabbing = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnOpenDevice = new System.Windows.Forms.Button();
            this.btnCloseDevice = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbxExposureTime = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxGain = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxFrameRate = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbTriggerOn = new System.Windows.Forms.RadioButton();
            this.rbTriggerOff = new System.Windows.Forms.RadioButton();
            this.btnCommandSoftware = new System.Windows.Forms.Button();
            this.btnSaveBMP = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lbVersion = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbCamList
            // 
            this.cbCamList.FormattingEnabled = true;
            this.cbCamList.Location = new System.Drawing.Point(71, 36);
            this.cbCamList.Name = "cbCamList";
            this.cbCamList.Size = new System.Drawing.Size(151, 20);
            this.cbCamList.TabIndex = 0;
            this.cbCamList.SelectedIndexChanged += new System.EventHandler(this.cbCamList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "设备列表";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnStartGrabbing
            // 
            this.btnStartGrabbing.Location = new System.Drawing.Point(14, 498);
            this.btnStartGrabbing.Name = "btnStartGrabbing";
            this.btnStartGrabbing.Size = new System.Drawing.Size(99, 28);
            this.btnStartGrabbing.TabIndex = 2;
            this.btnStartGrabbing.Text = "开始采集";
            this.btnStartGrabbing.UseVisualStyleBackColor = true;
            this.btnStartGrabbing.Click += new System.EventHandler(this.btnGrabSingleImage_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCommandSoftware);
            this.groupBox1.Controls.Add(this.rbHardware);
            this.groupBox1.Controls.Add(this.rbSoftware);
            this.groupBox1.Location = new System.Drawing.Point(14, 270);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(208, 84);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "触发源";
            // 
            // rbSoftware
            // 
            this.rbSoftware.AutoSize = true;
            this.rbSoftware.Location = new System.Drawing.Point(6, 20);
            this.rbSoftware.Name = "rbSoftware";
            this.rbSoftware.Size = new System.Drawing.Size(59, 16);
            this.rbSoftware.TabIndex = 0;
            this.rbSoftware.TabStop = true;
            this.rbSoftware.Text = "软触发";
            this.rbSoftware.UseVisualStyleBackColor = true;
            this.rbSoftware.CheckedChanged += new System.EventHandler(this.rbSoftware_CheckedChanged);
            // 
            // rbHardware
            // 
            this.rbHardware.AutoSize = true;
            this.rbHardware.Location = new System.Drawing.Point(107, 20);
            this.rbHardware.Name = "rbHardware";
            this.rbHardware.Size = new System.Drawing.Size(59, 16);
            this.rbHardware.TabIndex = 1;
            this.rbHardware.TabStop = true;
            this.rbHardware.Text = "硬触发";
            this.rbHardware.UseVisualStyleBackColor = true;
            this.rbHardware.CheckedChanged += new System.EventHandler(this.rbHardware_CheckedChanged);
            // 
            // btnStopGrabbing
            // 
            this.btnStopGrabbing.Location = new System.Drawing.Point(119, 498);
            this.btnStopGrabbing.Name = "btnStopGrabbing";
            this.btnStopGrabbing.Size = new System.Drawing.Size(99, 28);
            this.btnStopGrabbing.TabIndex = 5;
            this.btnStopGrabbing.Text = "停止采图";
            this.btnStopGrabbing.UseVisualStyleBackColor = true;
            this.btnStopGrabbing.Click += new System.EventHandler(this.btnStopGrabbing_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Location = new System.Drawing.Point(247, 36);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(769, 545);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "图像窗口";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Desktop;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 17);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(763, 525);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(14, 69);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(208, 28);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.Text = "刷新设备列表";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnOpenDevice
            // 
            this.btnOpenDevice.Location = new System.Drawing.Point(14, 117);
            this.btnOpenDevice.Name = "btnOpenDevice";
            this.btnOpenDevice.Size = new System.Drawing.Size(99, 23);
            this.btnOpenDevice.TabIndex = 8;
            this.btnOpenDevice.Text = "打开设备";
            this.btnOpenDevice.UseVisualStyleBackColor = true;
            this.btnOpenDevice.Click += new System.EventHandler(this.btnOpenDevice_Click);
            // 
            // btnCloseDevice
            // 
            this.btnCloseDevice.Location = new System.Drawing.Point(123, 117);
            this.btnCloseDevice.Name = "btnCloseDevice";
            this.btnCloseDevice.Size = new System.Drawing.Size(99, 23);
            this.btnCloseDevice.TabIndex = 9;
            this.btnCloseDevice.Text = "关闭设备";
            this.btnCloseDevice.UseVisualStyleBackColor = true;
            this.btnCloseDevice.Click += new System.EventHandler(this.btnCloseDevice_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.tbxFrameRate);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.tbxGain);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.tbxExposureTime);
            this.groupBox3.Location = new System.Drawing.Point(20, 360);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 132);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "参数";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // tbxExposureTime
            // 
            this.tbxExposureTime.Location = new System.Drawing.Point(77, 20);
            this.tbxExposureTime.Name = "tbxExposureTime";
            this.tbxExposureTime.Size = new System.Drawing.Size(100, 21);
            this.tbxExposureTime.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "曝光时间";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "增益";
            // 
            // tbxGain
            // 
            this.tbxGain.Location = new System.Drawing.Point(77, 56);
            this.tbxGain.Name = "tbxGain";
            this.tbxGain.Size = new System.Drawing.Size(100, 21);
            this.tbxGain.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "帧率";
            // 
            // tbxFrameRate
            // 
            this.tbxFrameRate.Location = new System.Drawing.Point(77, 95);
            this.tbxFrameRate.Name = "tbxFrameRate";
            this.tbxFrameRate.Size = new System.Drawing.Size(100, 21);
            this.tbxFrameRate.TabIndex = 4;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rbTriggerOff);
            this.groupBox4.Controls.Add(this.rbTriggerOn);
            this.groupBox4.Location = new System.Drawing.Point(14, 166);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 73);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "触发模式";
            this.groupBox4.Enter += new System.EventHandler(this.groupBox4_Enter);
            // 
            // rbTriggerOn
            // 
            this.rbTriggerOn.AutoSize = true;
            this.rbTriggerOn.Location = new System.Drawing.Point(7, 12);
            this.rbTriggerOn.Name = "rbTriggerOn";
            this.rbTriggerOn.Size = new System.Drawing.Size(35, 16);
            this.rbTriggerOn.TabIndex = 0;
            this.rbTriggerOn.TabStop = true;
            this.rbTriggerOn.Text = "开";
            this.rbTriggerOn.UseVisualStyleBackColor = true;
            this.rbTriggerOn.CheckedChanged += new System.EventHandler(this.rbTriggerOn_CheckedChanged);
            // 
            // rbTriggerOff
            // 
            this.rbTriggerOff.AutoSize = true;
            this.rbTriggerOff.Location = new System.Drawing.Point(7, 34);
            this.rbTriggerOff.Name = "rbTriggerOff";
            this.rbTriggerOff.Size = new System.Drawing.Size(35, 16);
            this.rbTriggerOff.TabIndex = 1;
            this.rbTriggerOff.TabStop = true;
            this.rbTriggerOff.Text = "关";
            this.rbTriggerOff.UseVisualStyleBackColor = true;
            this.rbTriggerOff.CheckedChanged += new System.EventHandler(this.rbTriggerOff_CheckedChanged);
            // 
            // btnCommandSoftware
            // 
            this.btnCommandSoftware.Location = new System.Drawing.Point(7, 55);
            this.btnCommandSoftware.Name = "btnCommandSoftware";
            this.btnCommandSoftware.Size = new System.Drawing.Size(75, 23);
            this.btnCommandSoftware.TabIndex = 2;
            this.btnCommandSoftware.Text = "软触发一次";
            this.btnCommandSoftware.UseVisualStyleBackColor = true;
            this.btnCommandSoftware.Click += new System.EventHandler(this.btnCommandSoftware_Click);
            // 
            // btnSaveBMP
            // 
            this.btnSaveBMP.Location = new System.Drawing.Point(14, 532);
            this.btnSaveBMP.Name = "btnSaveBMP";
            this.btnSaveBMP.Size = new System.Drawing.Size(99, 28);
            this.btnSaveBMP.TabIndex = 12;
            this.btnSaveBMP.Text = "保存BMP";
            this.btnSaveBMP.UseVisualStyleBackColor = true;
            this.btnSaveBMP.Click += new System.EventHandler(this.btnSaveBMP_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "SDK版本";
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.Location = new System.Drawing.Point(64, 13);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(47, 12);
            this.lbVersion.TabIndex = 14;
            this.lbVersion.Text = "1.1.1.1";
            // 
            // GrabImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 585);
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnSaveBMP);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnCloseDevice);
            this.Controls.Add(this.btnOpenDevice);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnStopGrabbing);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnStartGrabbing);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbCamList);
            this.Name = "GrabImage";
            this.Text = "采集图像";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GrabImage_FormClosing);
            this.Load += new System.EventHandler(this.GrabImage_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbCamList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStartGrabbing;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbSoftware;
        private System.Windows.Forms.RadioButton rbHardware;
        private System.Windows.Forms.Button btnStopGrabbing;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnOpenDevice;
        private System.Windows.Forms.Button btnCloseDevice;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxExposureTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxFrameRate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxGain;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rbTriggerOff;
        private System.Windows.Forms.RadioButton rbTriggerOn;
        private System.Windows.Forms.Button btnCommandSoftware;
        private System.Windows.Forms.Button btnSaveBMP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbVersion;
    }
}

