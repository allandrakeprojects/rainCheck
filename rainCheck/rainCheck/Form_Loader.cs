using System;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class Form_Loader : Form
    {
        public Form_Loader()
        {
            InitializeComponent();
        }

        int i = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            rectangleShape_loader_1.Width += 25;
            i+=5;
            label_timer.Text = i + "%";

            if (rectangleShape_loader_1.Width >= 535)
            {
                timer.Stop();
                Form_Login form_login = new Form_Login();
                this.Hide();
                form_login.ShowDialog();
                this.Close();
            }
        }
    }
}
