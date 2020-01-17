namespace Net_client
{
    partial class FriendList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FriendList));
            this.button_addFriend = new System.Windows.Forms.Button();
            this.listView_friendList = new System.Windows.Forms.ListView();
            this.columnHeader_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_ip = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_state = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_addFriend
            // 
            this.button_addFriend.Location = new System.Drawing.Point(183, 563);
            this.button_addFriend.Name = "button_addFriend";
            this.button_addFriend.Size = new System.Drawing.Size(109, 41);
            this.button_addFriend.TabIndex = 1;
            this.button_addFriend.Text = "添加好友";
            this.button_addFriend.UseVisualStyleBackColor = true;
            this.button_addFriend.Click += new System.EventHandler(this.button_addFriend_Click);
            // 
            // listView_friendList
            // 
            this.listView_friendList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_name,
            this.columnHeader_ip,
            this.columnHeader_state});
            this.listView_friendList.FullRowSelect = true;
            this.listView_friendList.Location = new System.Drawing.Point(12, 12);
            this.listView_friendList.Name = "listView_friendList";
            this.listView_friendList.Size = new System.Drawing.Size(435, 510);
            this.listView_friendList.TabIndex = 3;
            this.listView_friendList.UseCompatibleStateImageBehavior = false;
            this.listView_friendList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader_name
            // 
            this.columnHeader_name.Text = "好友名";
            // 
            // columnHeader_ip
            // 
            this.columnHeader_ip.Text = "好友IP";
            this.columnHeader_ip.Width = 192;
            // 
            // columnHeader_state
            // 
            this.columnHeader_state.Text = "状态";
            this.columnHeader_state.Width = 129;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(149, 525);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "（双击好友开始聊天！）";
            // 
            // FriendList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Net_client.Properties.Resources.frienList_back;
            this.ClientSize = new System.Drawing.Size(459, 616);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView_friendList);
            this.Controls.Add(this.button_addFriend);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FriendList";
            this.Text = "FriendList";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_addFriend;
        private System.Windows.Forms.ListView listView_friendList;
        private System.Windows.Forms.ColumnHeader columnHeader_name;
        private System.Windows.Forms.ColumnHeader columnHeader_ip;
        private System.Windows.Forms.ColumnHeader columnHeader_state;
        private System.Windows.Forms.Label label1;
    }
}