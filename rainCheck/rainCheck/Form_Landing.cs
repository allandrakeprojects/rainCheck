using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class Form_Landing : Form
    {

        public Form_Landing()
        {
            InitializeComponent();

            // -------------- Design
            label_login.Select();
            //panel_registration.BringToFront();
            panel_login.BringToFront();
            textBox_name_registration.Enabled = false;
            textBox_username_registration.Enabled = false;
            textBox_password_registration.Enabled = false;
            button_submit.Enabled = false;
        }

        // -------------- Design

        //
        // Login
        //
        private void label_username_login_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, label_username_login.DisplayRectangle, Color.LightGray, ButtonBorderStyle.Solid);
        }

        private void label_password_login_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, label_password_login.DisplayRectangle, Color.LightGray, ButtonBorderStyle.Solid);
        }

        private void textBox_username_login_Enter(object sender, EventArgs e)
        {
            if (textBox_password_login.Text == "")
            {
                textBox_password_login.Text = "Password";
                string hex = "#BFCDDB";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_password_login.ForeColor = color;
                textBox_password_login.PasswordChar = Char.MinValue;
            }

            if (textBox_username_login.Text == "Username")
            {
                textBox_username_login.Text = "";
                string hex = "#858585";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_username_login.ForeColor = color;
            }
        }

        private void textBox_password_login_Enter(object sender, EventArgs e)
        {
            if (textBox_username_login.Text == "")
            {
                textBox_username_login.Text = "Username";
                string hex = "#BFCDDB";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_username_login.ForeColor = color;
            }

            if (textBox_password_login.Text == "Password")
            {
                textBox_password_login.Text = "";
                string hex = "#858585";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_password_login.ForeColor = color;
                textBox_password_login.PasswordChar = Char.MinValue;
            }
        }

        private void button_login_Enter(object sender, EventArgs e)
        {
            if (textBox_username_login.Text == "")
            {
                textBox_username_login.Text = "Username";
                string hex = "#BFCDDB";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_username_login.ForeColor = color;
            }

            if (textBox_password_login.Text == "")
            {
                textBox_password_login.Text = "Password";
                string hex = "#BFCDDB";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_password_login.ForeColor = color;
                textBox_password_login.PasswordChar = Char.MinValue;
            }
        }

        private void textBox_password_login_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox_password_login.Text))
            {
                textBox_password_login.Text = "";
                textBox_password_login.PasswordChar = Char.MinValue;
            }

            if (textBox_password_login.PasswordChar.Equals(Char.MinValue) && textBox_password_login.Text.Equals("Password").Equals(false))
            {
                textBox_password_login.PasswordChar = '•';
            }
        }

        private void label_register_Click(object sender, EventArgs e)
        {
            textBox_username_login.Enabled = false;
            textBox_password_login.Enabled = false;
            button_login.Enabled = false;

            textBox_name_registration.Enabled = true;
            textBox_username_registration.Enabled = true;
            textBox_password_registration.Enabled = true;
            button_submit.Enabled = true;

            if (textBox_password_registration.Text == "Password")
            {
                textBox_password_registration.PasswordChar = Char.MinValue;
            }

            label_register.Focus();

            panel_registration.BringToFront();
        }

        private void textBox_username_login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.SuppressKeyPress = true;
        }

        private void textBox_password_login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.SuppressKeyPress = true;
        }

        private void label_register_MouseEnter(object sender, EventArgs e)
        {
            label_register.Font = new Font(label_register.Font.Name, label_register.Font.SizeInPoints, FontStyle.Underline);
        }

        private void label_register_MouseLeave(object sender, EventArgs e)
        {
            label_register.Font = new Font(label_register.Font.Name, label_register.Font.SizeInPoints, FontStyle.Regular);
        }
        
        //
        // Registration
        //
        private void label_back_MouseLeave(object sender, EventArgs e)
        {
            label_back.Font = new Font(label_back.Font.Name, label_back.Font.SizeInPoints, FontStyle.Regular);
        }

        private void label_name_registration_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, label_name_registration.DisplayRectangle, Color.LightGray, ButtonBorderStyle.Solid);
        }

        private void label_username_registration_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, label_username_registration.DisplayRectangle, Color.LightGray, ButtonBorderStyle.Solid);
        }

        private void label_password_registration_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, label_password_registration.DisplayRectangle, Color.LightGray, ButtonBorderStyle.Solid);
        }

        private void label_back_Click(object sender, EventArgs e)
        {
            textBox_username_login.Enabled = true;
            textBox_password_login.Enabled = true;
            button_login.Enabled = true;

            textBox_name_registration.Enabled = false;
            textBox_username_registration.Enabled = false;
            textBox_password_registration.Enabled = false;
            button_submit.Enabled = false;

            label_login.Focus();

            if (textBox_username_login.Text == "")
            {
                textBox_username_login.Text = "Username";
                string hex = "#BFCDDB";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_username_login.ForeColor = color;
            }

            if (textBox_password_login.Text == "")
            {
                textBox_password_login.Text = "Password";
                string hex = "#BFCDDB";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_password_login.ForeColor = color;
            }

            panel_login.BringToFront();
        }

        private void label_back_MouseEnter(object sender, EventArgs e)
        {
            label_back.Font = new Font(label_back.Font.Name, label_back.Font.SizeInPoints, FontStyle.Underline);
        }

        private void textBox_name_registration_Enter(object sender, EventArgs e)
        {
            if (textBox_username_registration.Text == "")
            {
                textBox_username_registration.Text = "Username";
                string hex = "#BFCDDB";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_username_registration.ForeColor = color;
                textBox_username_registration.PasswordChar = Char.MinValue;
            }

            if (textBox_password_registration.Text == "")
            {
                textBox_password_registration.Text = "Password";
                string hex = "#BFCDDB";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_password_registration.ForeColor = color;
                textBox_password_registration.PasswordChar = Char.MinValue;
            }

            if (textBox_name_registration.Text == "Name")
            {
                textBox_name_registration.Text = "";
                string hex = "#858585";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_name_registration.ForeColor = color;
            }
        }

        private void textBox_username_registration_Enter(object sender, EventArgs e)
        {
            if (textBox_name_registration.Text == "")
            {
                textBox_name_registration.Text = "Name";
                string hex = "#BFCDDB";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_name_registration.ForeColor = color;
                textBox_name_registration.PasswordChar = Char.MinValue;
            }

            if (textBox_password_registration.Text == "")
            {
                textBox_password_registration.Text = "Password";
                string hex = "#BFCDDB";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_password_registration.ForeColor = color;
                textBox_password_registration.PasswordChar = Char.MinValue;
            }

            if (textBox_username_registration.Text == "Username")
            {
                textBox_username_registration.Text = "";
                string hex = "#858585";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_username_registration.ForeColor = color;
            }
        }

        private void textBox_password_registration_Enter(object sender, EventArgs e)
        {
            if (textBox_name_registration.Text == "")
            {
                textBox_name_registration.Text = "Name";
                string hex = "#BFCDDB";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_name_registration.ForeColor = color;
                textBox_name_registration.PasswordChar = Char.MinValue;
            }

            if (textBox_username_registration.Text == "")
            {
                textBox_username_registration.Text = "Username";
                string hex = "#BFCDDB";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_username_registration.ForeColor = color;
            }

            if (textBox_password_registration.Text == "Password")
            {
                textBox_password_registration.Text = "";
                string hex = "#858585";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_password_registration.ForeColor = color;
                textBox_password_registration.PasswordChar = Char.MinValue;
            }
        }

        private void textBox_password_registration_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox_password_registration.Text))
            {
                textBox_password_registration.Text = "";
                textBox_password_registration.PasswordChar = Char.MinValue;
            }

            if (textBox_password_registration.PasswordChar.Equals(Char.MinValue) && textBox_password_registration.Text.Equals("Password").Equals(false))
            {
                textBox_password_registration.PasswordChar = '•';
            }
        }

        private void button_submit_Enter(object sender, EventArgs e)
        {
            if (textBox_name_registration.Text == "")
            {
                textBox_name_registration.Text = "Name";
                string hex = "#BFCDDB";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_name_registration.ForeColor = color;
                textBox_name_registration.PasswordChar = Char.MinValue;
            }

            if (textBox_password_registration.Text == "")
            {
                textBox_password_registration.Text = "Password";
                string hex = "#BFCDDB";
                Color color = ColorTranslator.FromHtml(hex);
                textBox_password_registration.ForeColor = color;
            }
        }

        private void textBox_name_registration_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.SuppressKeyPress = true;
        }

        private void textBox_username_registration_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.SuppressKeyPress = true;
        }

        private void textBox_password_registration_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.SuppressKeyPress = true;
        }
                
        private void label_password_view_MouseEnter(object sender, EventArgs e)
        {
            textBox_password_registration.UseSystemPasswordChar = true;
        }

        private void label_password_view_MouseLeave(object sender, EventArgs e)
        {
            textBox_password_registration.UseSystemPasswordChar = false;
        }

        private void Form_Landing_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox_name_registration.Text) && textBox_name_registration.Text != "Name" || 
                !String.IsNullOrEmpty(textBox_username_registration.Text) && textBox_username_registration.Text != "Username" ||
                !String.IsNullOrEmpty(textBox_password_registration.Text) && textBox_password_registration.Text != "Password")
            {
                DialogResult dr = MessageBox.Show("Leave? Changes you made may not be saved.",
                                        "rainCheck says...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    e.Cancel = true;
                    panel_registration.BringToFront();

                    textBox_name_registration.Enabled = true;
                    textBox_username_registration.Enabled = true;
                    textBox_password_registration.Enabled = true;
                    button_submit.Enabled = true;

                    if (textBox_password_registration.Text == "Password")
                    {
                        textBox_password_registration.PasswordChar = Char.MinValue;
                    }

                    label_register.Focus();

                    panel_registration.BringToFront();
                }
            }
        }

        private void button_login_Click(object sender, EventArgs e)
        {
            MessageBox.Show("asd");
        }
    }
}
