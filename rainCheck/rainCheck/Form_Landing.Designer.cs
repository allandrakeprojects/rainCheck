﻿namespace rainCheck
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
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox_loader = new System.Windows.Forms.PictureBox();
            this.panel_retry = new System.Windows.Forms.Panel();
            this.button_retry = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.label_ip = new System.Windows.Forms.Label();
            this.panel_authorization.SuspendLayout();
            this.panel_loader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_loader)).BeginInit();
            this.panel_retry.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_authorization
            // 
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
            // label_isp
            // 
            this.label_isp.AutoSize = true;
            this.label_isp.Location = new System.Drawing.Point(74, 173);
            this.label_isp.Name = "label_isp";
            this.label_isp.Size = new System.Drawing.Size(13, 13);
            this.label_isp.TabIndex = 22;
            this.label_isp.Text = "4";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(27, 173);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(20, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "isp";
            // 
            // label_country
            // 
            this.label_country.AutoSize = true;
            this.label_country.Location = new System.Drawing.Point(74, 143);
            this.label_country.Name = "label_country";
            this.label_country.Size = new System.Drawing.Size(13, 13);
            this.label_country.TabIndex = 20;
            this.label_country.Text = "3";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(27, 143);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "country";
            // 
            // label_region
            // 
            this.label_region.AutoSize = true;
            this.label_region.Location = new System.Drawing.Point(74, 113);
            this.label_region.Name = "label_region";
            this.label_region.Size = new System.Drawing.Size(13, 13);
            this.label_region.TabIndex = 18;
            this.label_region.Text = "2";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 113);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "region";
            // 
            // label_city
            // 
            this.label_city.AutoSize = true;
            this.label_city.Location = new System.Drawing.Point(74, 83);
            this.label_city.Name = "label_city";
            this.label_city.Size = new System.Drawing.Size(13, 13);
            this.label_city.TabIndex = 16;
            this.label_city.Text = "1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "city";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(209, 233);
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
            this.label_login.Location = new System.Drawing.Point(280, 180);
            this.label_login.Name = "label_login";
            this.label_login.Size = new System.Drawing.Size(165, 38);
            this.label_login.TabIndex = 11;
            this.label_login.Text = "rainCheck";
            // 
            // label_logo_login
            // 
            this.label_logo_login.Image = global::rainCheck.Properties.Resources.icon_32x32;
            this.label_logo_login.Location = new System.Drawing.Point(232, 181);
            this.label_logo_login.Name = "label_logo_login";
            this.label_logo_login.Size = new System.Drawing.Size(56, 39);
            this.label_logo_login.TabIndex = 12;
            // 
            // panel_loader
            // 
            this.panel_loader.Controls.Add(this.label3);
            this.panel_loader.Controls.Add(this.pictureBox_loader);
            this.panel_loader.Location = new System.Drawing.Point(12, 12);
            this.panel_loader.Name = "panel_loader";
            this.panel_loader.Size = new System.Drawing.Size(680, 457);
            this.panel_loader.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(247, 281);
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
            this.pictureBox_loader.Location = new System.Drawing.Point(235, 100);
            this.pictureBox_loader.Name = "pictureBox_loader";
            this.pictureBox_loader.Size = new System.Drawing.Size(201, 175);
            this.pictureBox_loader.TabIndex = 0;
            this.pictureBox_loader.TabStop = false;
            // 
            // panel_retry
            // 
            this.panel_retry.Controls.Add(this.button_retry);
            this.panel_retry.Controls.Add(this.label1);
            this.panel_retry.Location = new System.Drawing.Point(12, 12);
            this.panel_retry.Name = "panel_retry";
            this.panel_retry.Size = new System.Drawing.Size(680, 457);
            this.panel_retry.TabIndex = 16;
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
            this.button_retry.Location = new System.Drawing.Point(287, 234);
            this.button_retry.Name = "button_retry";
            this.button_retry.Size = new System.Drawing.Size(104, 29);
            this.button_retry.TabIndex = 11;
            this.button_retry.Text = "Retry (F5)";
            this.button_retry.UseVisualStyleBackColor = false;
            this.button_retry.Click += new System.EventHandler(this.button_retry_Click);
            this.button_retry.KeyDown += new System.Windows.Forms.KeyEventHandler(this.button_retry_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(216, 194);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "No Internet connection";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "ip";
            // 
            // label_ip
            // 
            this.label_ip.AutoSize = true;
            this.label_ip.Location = new System.Drawing.Point(74, 53);
            this.label_ip.Name = "label_ip";
            this.label_ip.Size = new System.Drawing.Size(15, 13);
            this.label_ip.TabIndex = 24;
            this.label_ip.Text = "ip";
            // 
            // Form_Landing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(69)))), ((int)(((byte)(87)))));
            this.ClientSize = new System.Drawing.Size(704, 481);
            this.Controls.Add(this.panel_authorization);
            this.Controls.Add(this.panel_retry);
            this.Controls.Add(this.panel_loader);
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
            this.panel_authorization.ResumeLayout(false);
            this.panel_authorization.PerformLayout();
            this.panel_loader.ResumeLayout(false);
            this.panel_loader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_loader)).EndInit();
            this.panel_retry.ResumeLayout(false);
            this.panel_retry.PerformLayout();
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
    }
}