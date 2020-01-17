namespace Net_server
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.listBox_online = new System.Windows.Forms.ListBox();
            this.textBox_localIP = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "本地IP：";
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(27, 70);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(462, 341);
            this.txtMsg.TabIndex = 2;
            // 
            // listBox_online
            // 
            this.listBox_online.FormattingEnabled = true;
            this.listBox_online.ItemHeight = 15;
            this.listBox_online.Location = new System.Drawing.Point(524, 70);
            this.listBox_online.Name = "listBox_online";
            this.listBox_online.Size = new System.Drawing.Size(250, 334);
            this.listBox_online.TabIndex = 3;
            // 
            // textBox_localIP
            // 
            this.textBox_localIP.Location = new System.Drawing.Point(120, 28);
            this.textBox_localIP.Name = "textBox_localIP";
            this.textBox_localIP.Size = new System.Drawing.Size(170, 25);
            this.textBox_localIP.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBox_localIP);
            this.Controls.Add(this.listBox_online);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.ListBox listBox_online;
        private System.Windows.Forms.TextBox textBox_localIP;
    }
}

