namespace rainCheck
{
    partial class Form_Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Login));
            this.label_raincheck = new System.Windows.Forms.Label();
            this.panel_credential = new System.Windows.Forms.Panel();
            this.label_password_logo = new System.Windows.Forms.Label();
            this.label_username_logo = new System.Windows.Forms.Label();
            this.label_credential_logo = new System.Windows.Forms.Label();
            this.button_login = new System.Windows.Forms.Button();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.label_password = new System.Windows.Forms.Label();
            this.textBox_username = new System.Windows.Forms.TextBox();
            this.label_line = new System.Windows.Forms.Label();
            this.label_credential = new System.Windows.Forms.Label();
            this.label_username = new System.Windows.Forms.Label();
            this.panel_bottom = new System.Windows.Forms.Panel();
            this.label_register = new System.Windows.Forms.Label();
            this.label_notyetregistered = new System.Windows.Forms.Label();
            this.label_logo = new System.Windows.Forms.Label();
            this.panel_credential.SuspendLayout();
            this.panel_bottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_raincheck
            // 
            this.label_raincheck.AutoSize = true;
            this.label_raincheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 24.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_raincheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(143)))), ((int)(((byte)(202)))));
            this.label_raincheck.Location = new System.Drawing.Point(291, 71);
            this.label_raincheck.Name = "label_raincheck";
            this.label_raincheck.Size = new System.Drawing.Size(165, 38);
            this.label_raincheck.TabIndex = 4;
            this.label_raincheck.Text = "rainCheck";
            // 
            // panel_credential
            // 
            this.panel_credential.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            this.panel_credential.Controls.Add(this.label_password_logo);
            this.panel_credential.Controls.Add(this.label_username_logo);
            this.panel_credential.Controls.Add(this.label_credential_logo);
            this.panel_credential.Controls.Add(this.button_login);
            this.panel_credential.Controls.Add(this.textBox_password);
            this.panel_credential.Controls.Add(this.label_password);
            this.panel_credential.Controls.Add(this.textBox_username);
            this.panel_credential.Controls.Add(this.label_line);
            this.panel_credential.Controls.Add(this.label_credential);
            this.panel_credential.Controls.Add(this.label_username);
            this.panel_credential.Location = new System.Drawing.Point(182, 127);
            this.panel_credential.Name = "panel_credential";
            this.panel_credential.Size = new System.Drawing.Size(339, 241);
            this.panel_credential.TabIndex = 5;
            // 
            // label_password_logo
            // 
            this.label_password_logo.BackColor = System.Drawing.Color.White;
            this.label_password_logo.Image = ((System.Drawing.Image)(resources.GetObject("label_password_logo.Image")));
            this.label_password_logo.Location = new System.Drawing.Point(278, 127);
            this.label_password_logo.Name = "label_password_logo";
            this.label_password_logo.Size = new System.Drawing.Size(22, 23);
            this.label_password_logo.TabIndex = 9;
            // 
            // label_username_logo
            // 
            this.label_username_logo.BackColor = System.Drawing.Color.White;
            this.label_username_logo.Image = global::rainCheck.Properties.Resources.user;
            this.label_username_logo.Location = new System.Drawing.Point(278, 84);
            this.label_username_logo.Name = "label_username_logo";
            this.label_username_logo.Size = new System.Drawing.Size(22, 23);
            this.label_username_logo.TabIndex = 8;
            // 
            // label_credential_logo
            // 
            this.label_credential_logo.Image = ((System.Drawing.Image)(resources.GetObject("label_credential_logo.Image")));
            this.label_credential_logo.Location = new System.Drawing.Point(26, 29);
            this.label_credential_logo.Name = "label_credential_logo";
            this.label_credential_logo.Size = new System.Drawing.Size(28, 23);
            this.label_credential_logo.TabIndex = 7;
            // 
            // button_login
            // 
            this.button_login.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(139)))), ((int)(((byte)(202)))));
            this.button_login.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_login.FlatAppearance.BorderSize = 0;
            this.button_login.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_login.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.button_login.ForeColor = System.Drawing.Color.White;
            this.button_login.Image = global::rainCheck.Properties.Resources.key;
            this.button_login.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_login.Location = new System.Drawing.Point(207, 181);
            this.button_login.Name = "button_login";
            this.button_login.Padding = new System.Windows.Forms.Padding(13, 0, 13, 0);
            this.button_login.Size = new System.Drawing.Size(99, 32);
            this.button_login.TabIndex = 6;
            this.button_login.Text = "Login";
            this.button_login.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_login.UseVisualStyleBackColor = false;
            // 
            // textBox_password
            // 
            this.textBox_password.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_password.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_password.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.textBox_password.Location = new System.Drawing.Point(36, 130);
            this.textBox_password.Multiline = true;
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.Size = new System.Drawing.Size(238, 20);
            this.textBox_password.TabIndex = 4;
            this.textBox_password.Text = "Password";
            this.textBox_password.TextChanged += new System.EventHandler(this.textBox_password_TextChanged);
            this.textBox_password.Enter += new System.EventHandler(this.textBox_password_Enter);
            // 
            // label_password
            // 
            this.label_password.BackColor = System.Drawing.Color.White;
            this.label_password.Location = new System.Drawing.Point(27, 123);
            this.label_password.Name = "label_password";
            this.label_password.Size = new System.Drawing.Size(279, 29);
            this.label_password.TabIndex = 5;
            this.label_password.Paint += new System.Windows.Forms.PaintEventHandler(this.label_password_Paint);
            // 
            // textBox_username
            // 
            this.textBox_username.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_username.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_username.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.textBox_username.Location = new System.Drawing.Point(36, 87);
            this.textBox_username.Multiline = true;
            this.textBox_username.Name = "textBox_username";
            this.textBox_username.Size = new System.Drawing.Size(238, 20);
            this.textBox_username.TabIndex = 2;
            this.textBox_username.Text = "Username";
            this.textBox_username.Enter += new System.EventHandler(this.textBox_username_Enter);
            // 
            // label_line
            // 
            this.label_line.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(227)))), ((int)(((byte)(239)))));
            this.label_line.Location = new System.Drawing.Point(29, 56);
            this.label_line.Name = "label_line";
            this.label_line.Size = new System.Drawing.Size(276, 1);
            this.label_line.TabIndex = 1;
            // 
            // label_credential
            // 
            this.label_credential.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_credential.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(143)))), ((int)(((byte)(202)))));
            this.label_credential.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label_credential.Location = new System.Drawing.Point(49, 30);
            this.label_credential.Name = "label_credential";
            this.label_credential.Size = new System.Drawing.Size(245, 26);
            this.label_credential.TabIndex = 0;
            this.label_credential.Text = "Credential";
            // 
            // label_username
            // 
            this.label_username.BackColor = System.Drawing.Color.White;
            this.label_username.Location = new System.Drawing.Point(27, 80);
            this.label_username.Name = "label_username";
            this.label_username.Size = new System.Drawing.Size(279, 29);
            this.label_username.TabIndex = 3;
            this.label_username.Paint += new System.Windows.Forms.PaintEventHandler(this.label_username_Paint);
            // 
            // panel_bottom
            // 
            this.panel_bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(144)))), ((int)(((byte)(193)))));
            this.panel_bottom.Controls.Add(this.label_register);
            this.panel_bottom.Controls.Add(this.label_notyetregistered);
            this.panel_bottom.Location = new System.Drawing.Point(182, 370);
            this.panel_bottom.Name = "panel_bottom";
            this.panel_bottom.Size = new System.Drawing.Size(339, 37);
            this.panel_bottom.TabIndex = 6;
            // 
            // label_register
            // 
            this.label_register.AutoSize = true;
            this.label_register.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_register.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_register.ForeColor = System.Drawing.Color.White;
            this.label_register.Location = new System.Drawing.Point(260, 11);
            this.label_register.Name = "label_register";
            this.label_register.Size = new System.Drawing.Size(46, 13);
            this.label_register.TabIndex = 1;
            this.label_register.Text = "Register";
            this.label_register.Click += new System.EventHandler(this.label_register_Click);
            // 
            // label_notyetregistered
            // 
            this.label_notyetregistered.AutoSize = true;
            this.label_notyetregistered.ForeColor = System.Drawing.Color.White;
            this.label_notyetregistered.Location = new System.Drawing.Point(204, 11);
            this.label_notyetregistered.Name = "label_notyetregistered";
            this.label_notyetregistered.Size = new System.Drawing.Size(59, 13);
            this.label_notyetregistered.TabIndex = 0;
            this.label_notyetregistered.Text = "New here?";
            this.label_notyetregistered.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_logo
            // 
            this.label_logo.Image = global::rainCheck.Properties.Resources.icon_32x32;
            this.label_logo.Location = new System.Drawing.Point(243, 72);
            this.label_logo.Name = "label_logo";
            this.label_logo.Size = new System.Drawing.Size(56, 39);
            this.label_logo.TabIndex = 3;
            // 
            // Form_Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(69)))), ((int)(((byte)(87)))));
            this.ClientSize = new System.Drawing.Size(704, 481);
            this.Controls.Add(this.panel_bottom);
            this.Controls.Add(this.panel_credential);
            this.Controls.Add(this.label_raincheck);
            this.Controls.Add(this.label_logo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(720, 520);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(720, 520);
            this.Name = "Form_Login";
            this.Opacity = 0.98D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "rainCheck - Login";
            this.panel_credential.ResumeLayout(false);
            this.panel_credential.PerformLayout();
            this.panel_bottom.ResumeLayout(false);
            this.panel_bottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_raincheck;
        private System.Windows.Forms.Label label_logo;
        private System.Windows.Forms.Panel panel_credential;
        private System.Windows.Forms.Label label_credential;
        private System.Windows.Forms.Label label_line;
        private System.Windows.Forms.TextBox textBox_username;
        private System.Windows.Forms.Label label_username;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Label label_password;
        private System.Windows.Forms.Panel panel_bottom;
        private System.Windows.Forms.Label label_notyetregistered;
        private System.Windows.Forms.Label label_register;
        private System.Windows.Forms.Label label_credential_logo;
        private System.Windows.Forms.Label label_password_logo;
        private System.Windows.Forms.Label label_username_logo;
        private System.Windows.Forms.Button button_login;
    }
}