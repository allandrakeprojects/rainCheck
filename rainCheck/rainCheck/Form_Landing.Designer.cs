namespace rainCheck
{
    partial class Form_Landing
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Landing));
            this.panel_authorization = new System.Windows.Forms.Panel();
            this.label_authorisation = new System.Windows.Forms.Label();
            this.label_timer = new System.Windows.Forms.Label();
            this.label_apichanges = new System.Windows.Forms.Label();
            this.label_macid = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_ip = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label_isp = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label_country = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label_region = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label_city = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_login = new System.Windows.Forms.Label();
            this.label_logo_login = new System.Windows.Forms.Label();
            this.panel_loader = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox_loader = new System.Windows.Forms.PictureBox();
            this.panel_retry = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.button_retry = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.panel_blank = new System.Windows.Forms.Panel();
            this.timer_apichanges = new System.Windows.Forms.Timer(this.components);
            this.panel_verified = new System.Windows.Forms.Panel();
            this.label24 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.timer_gotomain = new System.Windows.Forms.Timer(this.components);
            this.timer_authorisation = new System.Windows.Forms.Timer(this.components);
            this.panel_authorization.SuspendLayout();
            this.panel_loader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_loader)).BeginInit();
            this.panel_retry.SuspendLayout();
            this.panel_verified.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_authorization
            // 
            this.panel_authorization.Controls.Add(this.label_authorisation);
            this.panel_authorization.Controls.Add(this.label_timer);
            this.panel_authorization.Controls.Add(this.label_apichanges);
            this.panel_authorization.Controls.Add(this.label_macid);
            this.panel_authorization.Controls.Add(this.label4);
            this.panel_authorization.Controls.Add(this.label_ip);
            this.panel_authorization.Controls.Add(this.label6);
            this.panel_authorization.Controls.Add(this.label_isp);
            this.panel_authorization.Controls.Add(this.label11);
            this.panel_authorization.Controls.Add(this.label_country);
            this.panel_authorization.Controls.Add(this.label9);
            this.panel_authorization.Controls.Add(this.label_region);
            this.panel_authorization.Controls.Add(this.label7);
            this.panel_authorization.Controls.Add(this.label_city);
            this.panel_authorization.Controls.Add(this.label5);
            this.panel_authorization.Controls.Add(this.label2);
            this.panel_authorization.Controls.Add(this.label_login);
            this.panel_authorization.Controls.Add(this.label_logo_login);
            this.panel_authorization.Location = new System.Drawing.Point(12, 12);
            this.panel_authorization.Name = "panel_authorization";
            this.panel_authorization.Size = new System.Drawing.Size(680, 457);
            this.panel_authorization.TabIndex = 14;
            // 
            // label_authorisation
            // 
            this.label_authorisation.AutoSize = true;
            this.label_authorisation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_authorisation.ForeColor = System.Drawing.Color.White;
            this.label_authorisation.Location = new System.Drawing.Point(205, 444);
            this.label_authorisation.Name = "label_authorisation";
            this.label_authorisation.Size = new System.Drawing.Size(279, 13);
            this.label_authorisation.TabIndex = 29;
            this.label_authorisation.Text = "If no handshake within 60 seconds, program will auto exit.";
            // 
            // label_timer
            // 
            this.label_timer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_timer.ForeColor = System.Drawing.Color.White;
            this.label_timer.Location = new System.Drawing.Point(317, 414);
            this.label_timer.Name = "label_timer";
            this.label_timer.Size = new System.Drawing.Size(45, 28);
            this.label_timer.TabIndex = 28;
            this.label_timer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_apichanges
            // 
            this.label_apichanges.AutoSize = true;
            this.label_apichanges.Location = new System.Drawing.Point(401, 68);
            this.label_apichanges.Name = "label_apichanges";
            this.label_apichanges.Size = new System.Drawing.Size(65, 13);
            this.label_apichanges.TabIndex = 27;
            this.label_apichanges.Text = "api changes";
            this.label_apichanges.Visible = false;
            // 
            // label_macid
            // 
            this.label_macid.AutoSize = true;
            this.label_macid.Location = new System.Drawing.Point(74, 28);
            this.label_macid.Name = "label_macid";
            this.label_macid.Size = new System.Drawing.Size(15, 13);
            this.label_macid.TabIndex = 26;
            this.label_macid.Text = "ip";
            this.label_macid.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "mac id";
            this.label4.Visible = false;
            // 
            // label_ip
            // 
            this.label_ip.AutoSize = true;
            this.label_ip.Location = new System.Drawing.Point(74, 57);
            this.label_ip.Name = "label_ip";
            this.label_ip.Size = new System.Drawing.Size(15, 13);
            this.label_ip.TabIndex = 24;
            this.label_ip.Text = "ip";
            this.label_ip.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "ip";
            this.label6.Visible = false;
            // 
            // label_isp
            // 
            this.label_isp.AutoSize = true;
            this.label_isp.Location = new System.Drawing.Point(74, 173);
            this.label_isp.Name = "label_isp";
            this.label_isp.Size = new System.Drawing.Size(13, 13);
            this.label_isp.TabIndex = 22;
            this.label_isp.Text = "4";
            this.label_isp.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(27, 173);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(20, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "isp";
            this.label11.Visible = false;
            // 
            // label_country
            // 
            this.label_country.AutoSize = true;
            this.label_country.Location = new System.Drawing.Point(74, 144);
            this.label_country.Name = "label_country";
            this.label_country.Size = new System.Drawing.Size(13, 13);
            this.label_country.TabIndex = 20;
            this.label_country.Text = "3";
            this.label_country.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(27, 144);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "country";
            this.label9.Visible = false;
            // 
            // label_region
            // 
            this.label_region.AutoSize = true;
            this.label_region.Location = new System.Drawing.Point(74, 115);
            this.label_region.Name = "label_region";
            this.label_region.Size = new System.Drawing.Size(13, 13);
            this.label_region.TabIndex = 18;
            this.label_region.Text = "2";
            this.label_region.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 115);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "region";
            this.label7.Visible = false;
            // 
            // label_city
            // 
            this.label_city.AutoSize = true;
            this.label_city.Location = new System.Drawing.Point(74, 86);
            this.label_city.Name = "label_city";
            this.label_city.Size = new System.Drawing.Size(13, 13);
            this.label_city.TabIndex = 16;
            this.label_city.Text = "1";
            this.label_city.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "city";
            this.label5.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(205, 233);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(286, 25);
            this.label2.TabIndex = 14;
            this.label2.Text = "waiting for authorisation...";
            // 
            // label_login
            // 
            this.label_login.AutoSize = true;
            this.label_login.Font = new System.Drawing.Font("Microsoft Sans Serif", 24.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_login.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(143)))), ((int)(((byte)(202)))));
            this.label_login.Location = new System.Drawing.Point(279, 180);
            this.label_login.Name = "label_login";
            this.label_login.Size = new System.Drawing.Size(165, 38);
            this.label_login.TabIndex = 11;
            this.label_login.Text = "rainCheck";
            // 
            // label_logo_login
            // 
            this.label_logo_login.Image = global::rainCheck.Properties.Resources.icon_32x32;
            this.label_logo_login.Location = new System.Drawing.Point(231, 181);
            this.label_logo_login.Name = "label_logo_login";
            this.label_logo_login.Size = new System.Drawing.Size(56, 39);
            this.label_logo_login.TabIndex = 12;
            // 
            // panel_loader
            // 
            this.panel_loader.Controls.Add(this.button1);
            this.panel_loader.Controls.Add(this.label3);
            this.panel_loader.Controls.Add(this.pictureBox_loader);
            this.panel_loader.Location = new System.Drawing.Point(12, 12);
            this.panel_loader.Name = "panel_loader";
            this.panel_loader.Size = new System.Drawing.Size(680, 457);
            this.panel_loader.TabIndex = 15;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(538, 144);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(247, 274);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(187, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "Connecting to the server...";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox_loader
            // 
            this.pictureBox_loader.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_loader.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_loader.Image")));
            this.pictureBox_loader.Location = new System.Drawing.Point(268, 125);
            this.pictureBox_loader.Name = "pictureBox_loader";
            this.pictureBox_loader.Size = new System.Drawing.Size(135, 135);
            this.pictureBox_loader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_loader.TabIndex = 0;
            this.pictureBox_loader.TabStop = false;
            // 
            // panel_retry
            // 
            this.panel_retry.Controls.Add(this.label8);
            this.panel_retry.Controls.Add(this.button_retry);
            this.panel_retry.Controls.Add(this.label1);
            this.panel_retry.Location = new System.Drawing.Point(12, 12);
            this.panel_retry.Name = "panel_retry";
            this.panel_retry.Size = new System.Drawing.Size(680, 457);
            this.panel_retry.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(200, 236);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(283, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "(Automatically connects when connected back to Internet)";
            // 
            // button_retry
            // 
            this.button_retry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(139)))), ((int)(((byte)(202)))));
            this.button_retry.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_retry.FlatAppearance.BorderSize = 0;
            this.button_retry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_retry.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_retry.ForeColor = System.Drawing.Color.White;
            this.button_retry.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_retry.Location = new System.Drawing.Point(287, 301);
            this.button_retry.Name = "button_retry";
            this.button_retry.Size = new System.Drawing.Size(104, 29);
            this.button_retry.TabIndex = 11;
            this.button_retry.Text = "Retry (F5)";
            this.button_retry.UseVisualStyleBackColor = false;
            this.button_retry.Visible = false;
            this.button_retry.Click += new System.EventHandler(this.Button_retry_Click);
            this.button_retry.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Button_retry_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(216, 201);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "No Internet connection";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // panel_blank
            // 
            this.panel_blank.Location = new System.Drawing.Point(12, 12);
            this.panel_blank.Name = "panel_blank";
            this.panel_blank.Size = new System.Drawing.Size(680, 457);
            this.panel_blank.TabIndex = 17;
            // 
            // timer_apichanges
            // 
            this.timer_apichanges.Interval = 5000;
            this.timer_apichanges.Tick += new System.EventHandler(this.Timer_apichanges_Tick);
            // 
            // panel_verified
            // 
            this.panel_verified.Controls.Add(this.label24);
            this.panel_verified.Controls.Add(this.label26);
            this.panel_verified.Location = new System.Drawing.Point(12, 12);
            this.panel_verified.Name = "panel_verified";
            this.panel_verified.Size = new System.Drawing.Size(680, 457);
            this.panel_verified.TabIndex = 28;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this.label24.ForeColor = System.Drawing.Color.White;
            this.label24.Location = new System.Drawing.Point(238, 243);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(206, 25);
            this.label24.TabIndex = 14;
            this.label24.Text = "device authorised!";
            // 
            // label26
            // 
            this.label26.Image = global::rainCheck.Properties.Resources.verified;
            this.label26.Location = new System.Drawing.Point(281, 161);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(114, 78);
            this.label26.TabIndex = 12;
            // 
            // timer_gotomain
            // 
            this.timer_gotomain.Interval = 1000;
            this.timer_gotomain.Tick += new System.EventHandler(this.Timer_gotomain_Tick);
            // 
            // timer_authorisation
            // 
            this.timer_authorisation.Interval = 1000;
            this.timer_authorisation.Tick += new System.EventHandler(this.Timer_authorisation_Tick);
            // 
            // Form_Landing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(69)))), ((int)(((byte)(87)))));
            this.ClientSize = new System.Drawing.Size(704, 481);
            this.Controls.Add(this.panel_retry);
            this.Controls.Add(this.panel_loader);
            this.Controls.Add(this.panel_authorization);
            this.Controls.Add(this.panel_verified);
            this.Controls.Add(this.panel_blank);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(720, 520);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(720, 520);
            this.Name = "Form_Landing";
            this.Opacity = 0.98D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "rainCheck";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Landing_FormClosing);
            this.Load += new System.EventHandler(this.Form_Landing_Load);
            this.panel_authorization.ResumeLayout(false);
            this.panel_authorization.PerformLayout();
            this.panel_loader.ResumeLayout(false);
            this.panel_loader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_loader)).EndInit();
            this.panel_retry.ResumeLayout(false);
            this.panel_retry.PerformLayout();
            this.panel_verified.ResumeLayout(false);
            this.panel_verified.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel_authorization;
        private System.Windows.Forms.Label label_login;
        private System.Windows.Forms.Label label_logo_login;
        private System.Windows.Forms.Panel panel_loader;
        private System.Windows.Forms.PictureBox pictureBox_loader;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel_retry;
        private System.Windows.Forms.Button button_retry;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label label_city;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label_isp;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label_country;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label_region;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label_ip;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label_macid;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel_blank;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer_apichanges;
        private System.Windows.Forms.Label label_apichanges;
        private System.Windows.Forms.Panel panel_verified;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Timer timer_gotomain;
        private System.Windows.Forms.Label label_timer;
        private System.Windows.Forms.Timer timer_authorisation;
        private System.Windows.Forms.Label label_authorisation;
    }
}