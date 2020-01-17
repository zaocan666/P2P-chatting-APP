namespace Net_client
{
    partial class addFriend
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(addFriend));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_friendUN = new System.Windows.Forms.TextBox();
            this.button_yes = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "好友学号：";
            // 
            // textBox_friendUN
            // 
            this.textBox_friendUN.Location = new System.Drawing.Point(159, 41);
            this.textBox_friendUN.Name = "textBox_friendUN";
            this.textBox_friendUN.Size = new System.Drawing.Size(202, 25);
            this.textBox_friendUN.TabIndex = 1;
            // 
            // button_yes
            // 
            this.button_yes.Location = new System.Drawing.Point(174, 93);
            this.button_yes.Name = "button_yes";
            this.button_yes.Size = new System.Drawing.Size(98, 38);
            this.button_yes.TabIndex = 2;
            this.button_yes.Text = "确定";
            this.button_yes.UseVisualStyleBackColor = true;
            this.button_yes.Click += new System.EventHandler(this.button_yes_Click);
            // 
            // addFriend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 154);
            this.Controls.Add(this.button_yes);
            this.Controls.Add(this.textBox_friendUN);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "addFriend";
            this.Text = "添加好友";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_friendUN;
        private System.Windows.Forms.Button button_yes;
    }
}