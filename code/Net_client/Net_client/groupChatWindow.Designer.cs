namespace Net_client
{
    partial class groupChatWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(groupChatWindow));
            this.listView_message = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_send = new System.Windows.Forms.Button();
            this.textBox_message = new System.Windows.Forms.TextBox();
            this.listView_members = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_sendFile = new System.Windows.Forms.Button();
            this.listView_file = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listView_message
            // 
            this.listView_message.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView_message.Location = new System.Drawing.Point(12, 12);
            this.listView_message.Name = "listView_message";
            this.listView_message.Size = new System.Drawing.Size(568, 442);
            this.listView_message.TabIndex = 2;
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
            this.columnHeader2.Width = 95;
            // 
            // button_send
            // 
            this.button_send.Location = new System.Drawing.Point(602, 495);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(85, 34);
            this.button_send.TabIndex = 4;
            this.button_send.Text = "发送";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // textBox_message
            // 
            this.textBox_message.Location = new System.Drawing.Point(12, 479);
            this.textBox_message.Multiline = true;
            this.textBox_message.Name = "textBox_message";
            this.textBox_message.Size = new System.Drawing.Size(568, 117);
            this.textBox_message.TabIndex = 3;
            // 
            // listView_members
            // 
            this.listView_members.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader3});
            this.listView_members.Location = new System.Drawing.Point(602, 12);
            this.listView_members.Name = "listView_members";
            this.listView_members.Size = new System.Drawing.Size(231, 236);
            this.listView_members.TabIndex = 5;
            this.listView_members.UseCompatibleStateImageBehavior = false;
            this.listView_members.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "成员权限";
            this.columnHeader4.Width = 74;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "成员名";
            this.columnHeader3.Width = 134;
            // 
            // button_sendFile
            // 
            this.button_sendFile.Location = new System.Drawing.Point(602, 546);
            this.button_sendFile.Name = "button_sendFile";
            this.button_sendFile.Size = new System.Drawing.Size(85, 34);
            this.button_sendFile.TabIndex = 6;
            this.button_sendFile.Text = "上传文件";
            this.button_sendFile.UseVisualStyleBackColor = true;
            this.button_sendFile.Click += new System.EventHandler(this.button_sendFile_Click);
            // 
            // listView_file
            // 
            this.listView_file.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.listView_file.FullRowSelect = true;
            this.listView_file.Location = new System.Drawing.Point(602, 270);
            this.listView_file.Name = "listView_file";
            this.listView_file.Size = new System.Drawing.Size(231, 184);
            this.listView_file.TabIndex = 7;
            this.listView_file.UseCompatibleStateImageBehavior = false;
            this.listView_file.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "成员名";
            this.columnHeader5.Width = 69;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "文件大小";
            this.columnHeader6.Width = 73;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "文件名";
            this.columnHeader7.Width = 74;
            // 
            // groupChatWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Net_client.Properties.Resources.chat_back;
            this.ClientSize = new System.Drawing.Size(884, 613);
            this.Controls.Add(this.listView_file);
            this.Controls.Add(this.button_sendFile);
            this.Controls.Add(this.listView_members);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.textBox_message);
            this.Controls.Add(this.listView_message);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "groupChatWindow";
            this.Text = "groupChatWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView_message;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.TextBox textBox_message;
        private System.Windows.Forms.ListView listView_members;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button button_sendFile;
        private System.Windows.Forms.ListView listView_file;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
    }
}