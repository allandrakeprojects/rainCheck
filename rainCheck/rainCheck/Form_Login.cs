using System;
using System.Drawing;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class Form_Login : Form
    {
        public Form_Login()
        {
            InitializeComponent();

            // -------------- Design
            label_credential.Select();
        }

        // -------------- Design
        private void label_username_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, label_username.DisplayRectangle, Color.LightGray, ButtonBorderStyle.Solid);
        }

        private void label_password_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, label_password.DisplayRectangle, Color.LightGray, ButtonBorderStyle.Solid);
        }

        private void textBox_username_Enter(object sender, EventArgs e)
        {
            if (textBox_password.Text == "")
            {
                textBox_password.Text = "Password";
                string hex = "#BFCDDB";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_password.ForeColor = color;
                textBox_password.PasswordChar = Char.MinValue;
            }

            if (textBox_username.Text == "Username")
            {
                textBox_username.Text = "";
                string hex = "#858585";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_username.ForeColor = color;
            }
        }

        private void textBox_password_Enter(object sender, EventArgs e)
        {
            if (textBox_username.Text == "")
            {
                textBox_username.Text = "Username";
                string hex = "#BFCDDB";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_username.ForeColor = color;
            }

            if (textBox_password.Text == "Password")
            {
                textBox_password.Text = "";
                string hex = "#858585";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_password.ForeColor = color;
                textBox_password.PasswordChar = Char.MinValue;
            }
        }

        private void textBox_password_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox_password.Text))
            {
                textBox_password.Text = "";
                textBox_password.PasswordChar = Char.MinValue;
            }

            if (textBox_password.PasswordChar.Equals(Char.MinValue) && textBox_password.Text.Equals("Password").Equals(false))
            {
                textBox_password.PasswordChar = '•';
            }
        }
    }
}
