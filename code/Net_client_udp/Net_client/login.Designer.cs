namespace Net_client
{
    partial class login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(login));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_userName = new System.Windows.Forms.TextBox();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.textBox_serverIP = new System.Windows.Forms.TextBox();
            this.textBox_serverPort = new System.Windows.Forms.TextBox();
            this.button_login = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(162, 149);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(162, 185);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "密码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(162, 260);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "服务器IP：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(162, 292);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "服务器端口：";
            // 
            // textBox_userName
            // 
            this.textBox_userName.Location = new System.Drawing.Point(278, 139);
            this.textBox_userName.Name = "textBox_userName";
            this.textBox_userName.Size = new System.Drawing.Size(174, 25);
            this.textBox_userName.TabIndex = 4;
            this.textBox_userName.Text = "2017011589";
            // 
            // textBox_password
            // 
            this.textBox_password.Location = new System.Drawing.Point(278, 175);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.Size = new System.Drawing.Size(174, 25);
            this.textBox_password.TabIndex = 5;
            this.textBox_password.Text = "net2019";
            this.textBox_password.UseSystemPasswordChar = true;
            // 
            // textBox_serverIP
            // 
            this.textBox_serverIP.Location = new System.Drawing.Point(278, 250);
            this.textBox_serverIP.Name = "textBox_serverIP";
            this.textBox_serverIP.Size = new System.Drawing.Size(174, 25);
            this.textBox_serverIP.TabIndex = 6;
            this.textBox_serverIP.Text = "166.111.140.57";
            // 
            // textBox_serverPort
            // 
            this.textBox_serverPort.Location = new System.Drawing.Point(278, 282);
            this.textBox_serverPort.Name = "textBox_serverPort";
            this.textBox_serverPort.Size = new System.Drawing.Size(174, 25);
            this.textBox_serverPort.TabIndex = 7;
            this.textBox_serverPort.Text = "8000";
            // 
            // button_login
            // 
            this.button_login.Location = new System.Drawing.Point(265, 361);
            this.button_login.Name = "button_login";
            this.button_login.Size = new System.Drawing.Size(75, 23);
            this.button_login.TabIndex = 8;
            this.button_login.Text = "登录";
            this.button_login.UseVisualStyleBackColor = true;
            this.button_login.Click += new System.EventHandler(this.button_login_Click);
            // 
            // login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(608, 450);
            this.Controls.Add(this.button_login);
            this.Controls.Add(this.textBox_serverPort);
            this.Controls.Add(this.textBox_serverIP);
            this.Controls.Add(this.textBox_password);
            this.Controls.Add(this.textBox_userName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "login";
            this.Text = "login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_userName;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.TextBox textBox_serverIP;
        private System.Windows.Forms.TextBox textBox_serverPort;
        private System.Windows.Forms.Button button_login;
    }
}