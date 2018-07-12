using System;
using System.Globalization;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class Form_Loader : Form
    {
        public Form_Loader()
        {
            InitializeComponent();

            var culture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
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
                Form_Landing form_login = new Form_Landing();
                this.Hide();
                form_login.ShowDialog();
                this.Close();
            }
        }
    }
}
