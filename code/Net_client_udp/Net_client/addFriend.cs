using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Net_client
{
    public partial class addFriend : Form
    {
        public string friend_name;
        public addFriend()
        {
            InitializeComponent();
            friend_name = "";
        }

        private void button_yes_Click(object sender, EventArgs e)
        {
            friend_name = textBox_friendUN.Text;
            this.Close();
        }
    }
}
