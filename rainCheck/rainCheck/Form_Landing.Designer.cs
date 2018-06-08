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
            this.label_login = new System.Windows.Forms.Label();
            this.label_logo_login = new System.Windows.Forms.Label();
            this.button_authorize = new System.Windows.Forms.Button();
            this.panel_loader = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox_loader = new System.Windows.Forms.PictureBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.panel_retry = new System.Windows.Forms.Panel();
            this.button_retry = new System.Windows.Forms.Button();
            this.label_noconnection = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel_authorization.SuspendLayout();
            this.panel_loader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_loader)).BeginInit();
            this.panel_retry.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_authorization
            // 
            this.panel_authorization.Controls.Add(this.label1);
            this.panel_authorization.Controls.Add(this.label_login);
            this.panel_authorization.Controls.Add(this.label_logo_login);
            this.panel_authorization.Controls.Add(this.button_authorize);
            this.panel_authorization.Location = new System.Drawing.Point(12, 12);
            this.panel_authorization.Name = "panel_authorization";
            this.panel_authorization.Size = new System.Drawing.Size(680, 457);
            this.panel_authorization.TabIndex = 14;
            // 
            // label_login
            // 
            this.label_login.AutoSize = true;
            this.label_login.Font = new System.Drawing.Font("Microsoft Sans Serif", 24.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_login.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(143)))), ((int)(((byte)(202)))));
            this.label_login.Location = new System.Drawing.Point(280, 166);
            this.label_login.Name = "label_login";
            this.label_login.Size = new System.Drawing.Size(165, 38);
            this.label_login.TabIndex = 11;
            this.label_login.Text = "rainCheck";
            // 
            // label_logo_login
            // 
            this.label_logo_login.Image = global::rainCheck.Properties.Resources.icon_32x32;
            this.label_logo_login.Location = new System.Drawing.Point(232, 167);
            this.label_logo_login.Name = "label_logo_login";
            this.label_logo_login.Size = new System.Drawing.Size(56, 39);
            this.label_logo_login.TabIndex = 12;
            // 
            // button_authorize
            // 
            this.button_authorize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(139)))), ((int)(((byte)(202)))));
            this.button_authorize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_authorize.FlatAppearance.BorderSize = 0;
            this.button_authorize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_authorize.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_authorize.ForeColor = System.Drawing.Color.White;
            this.button_authorize.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_authorize.Location = new System.Drawing.Point(235, 226);
            this.button_authorize.Name = "button_authorize";
            this.button_authorize.Size = new System.Drawing.Size(210, 40);
            this.button_authorize.TabIndex = 10;
            this.button_authorize.Text = "AUTHORIZE";
            this.button_authorize.UseVisualStyleBackColor = false;
            this.button_authorize.Click += new System.EventHandler(this.button_authorize_Click);
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
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // panel_retry
            // 
            this.panel_retry.Controls.Add(this.button_retry);
            this.panel_retry.Controls.Add(this.label_noconnection);
            this.panel_retry.Location = new System.Drawing.Point(12, 12);
            this.panel_retry.Name = "panel_retry";
            this.panel_retry.Size = new System.Drawing.Size(680, 457);
            this.panel_retry.TabIndex = 15;
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
            this.button_retry.Location = new System.Drawing.Point(283, 234);
            this.button_retry.Name = "button_retry";
            this.button_retry.Size = new System.Drawing.Size(104, 29);
            this.button_retry.TabIndex = 11;
            this.button_retry.Text = "Retry";
            this.button_retry.UseVisualStyleBackColor = false;
            this.button_retry.Click += new System.EventHandler(this.button_retry_Click);
            // 
            // label_noconnection
            // 
            this.label_noconnection.AutoSize = true;
            this.label_noconnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_noconnection.ForeColor = System.Drawing.Color.White;
            this.label_noconnection.Location = new System.Drawing.Point(213, 194);
            this.label_noconnection.Name = "label_noconnection";
            this.label_noconnection.Size = new System.Drawing.Size(250, 25);
            this.label_noconnection.TabIndex = 2;
            this.label_noconnection.Text = "No Internet connection";
            this.label_noconnection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(34, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 138);
            this.label1.TabIndex = 13;
            this.label1.Text = "label1";
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
        private System.Windows.Forms.Button button_authorize;
        private System.Windows.Forms.Panel panel_loader;
        private System.Windows.Forms.PictureBox pictureBox_loader;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Panel panel_retry;
        private System.Windows.Forms.Label label_noconnection;
        private System.Windows.Forms.Button button_retry;
        private System.Windows.Forms.Label label1;
    }
}