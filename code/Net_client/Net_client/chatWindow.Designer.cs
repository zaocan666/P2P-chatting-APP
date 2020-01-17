namespace Net_client
{
    partial class chatWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(chatWindow));
            this.textBox_message = new System.Windows.Forms.TextBox();
            this.listView_message = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_send = new System.Windows.Forms.Button();
            this.button_file = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // textBox_message
            // 
            this.textBox_message.Location = new System.Drawing.Point(38, 452);
            this.textBox_message.Multiline = true;
            this.textBox_message.Name = "textBox_message";
            this.textBox_message.Size = new System.Drawing.Size(517, 117);
            this.textBox_message.TabIndex = 0;
            this.textBox_message.TextChanged += new System.EventHandler(this.textBox_message_TextChanged);
            // 
            // listView_message
            // 
            this.listView_message.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView_message.Location = new System.Drawing.Point(38, 26);
            this.listView_message.Name = "listView_message";
            this.listView_message.Size = new System.Drawing.Size(602, 391);
            this.listView_message.TabIndex = 1;
            this.listView_message.UseCompatibleStateImageBehavior = false;
            this.listView_message.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "用户名";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "发送内容";
            this.columnHeader2.Width = 90;
            // 
            // button_send
            // 
            this.button_send.Location = new System.Drawing.Point(575, 461);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(85, 34);
            this.button_send.TabIndex = 2;
            this.button_send.Text = "发送";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // button_file
            // 
            this.button_file.Location = new System.Drawing.Point(575, 518);
            this.button_file.Name = "button_file";
            this.button_file.Size = new System.Drawing.Size(85, 34);
            this.button_file.TabIndex = 3;
            this.button_file.Text = "发送文件";
            this.button_file.UseVisualStyleBackColor = true;
            this.button_file.Click += new System.EventHandler(this.button_file_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // chatWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Net_client.Properties.Resources.chat_back;
            this.ClientSize = new System.Drawing.Size(734, 593);
            this.Controls.Add(this.button_file);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.listView_message);
            this.Controls.Add(this.textBox_message);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "chatWindow";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "chatWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_message;
        private System.Windows.Forms.ListView listView_message;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.Button button_file;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}