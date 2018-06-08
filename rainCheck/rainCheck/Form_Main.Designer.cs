namespace rainCheck
{
    partial class Form_Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.panel_top = new System.Windows.Forms.Panel();
            this.comboBox_top = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label_globe = new System.Windows.Forms.Label();
            this.label_user_logged_in = new System.Windows.Forms.Label();
            this.label_howdy = new System.Windows.Forms.Label();
            this.panel_top.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_top
            // 
            this.panel_top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            this.panel_top.Controls.Add(this.label_howdy);
            this.panel_top.Controls.Add(this.label_user_logged_in);
            this.panel_top.Controls.Add(this.comboBox_top);
            this.panel_top.Controls.Add(this.label1);
            this.panel_top.Controls.Add(this.label_globe);
            this.panel_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_top.Location = new System.Drawing.Point(0, 0);
            this.panel_top.Name = "panel_top";
            this.panel_top.Size = new System.Drawing.Size(1008, 44);
            this.panel_top.TabIndex = 0;
            // 
            // comboBox_top
            // 
            this.comboBox_top.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.comboBox_top.DropDownHeight = 50;
            this.comboBox_top.DropDownWidth = 118;
            this.comboBox_top.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox_top.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_top.FormattingEnabled = true;
            this.comboBox_top.IntegralHeight = false;
            this.comboBox_top.Items.AddRange(new object[] {
            "Change Password",
            "Logout"});
            this.comboBox_top.Location = new System.Drawing.Point(869, 11);
            this.comboBox_top.Margin = new System.Windows.Forms.Padding(10);
            this.comboBox_top.MaximumSize = new System.Drawing.Size(118, 0);
            this.comboBox_top.Name = "comboBox_top";
            this.comboBox_top.Size = new System.Drawing.Size(18, 24);
            this.comboBox_top.TabIndex = 14;
            this.comboBox_top.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(56, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "rainCheck";
            // 
            // label_globe
            // 
            this.label_globe.BackColor = System.Drawing.Color.Transparent;
            this.label_globe.Cursor = System.Windows.Forms.Cursors.Default;
            this.label_globe.Image = global::rainCheck.Properties.Resources.globe;
            this.label_globe.Location = new System.Drawing.Point(35, 10);
            this.label_globe.Name = "label_globe";
            this.label_globe.Size = new System.Drawing.Size(22, 23);
            this.label_globe.TabIndex = 12;
            // 
            // label_user_logged_in
            // 
            this.label_user_logged_in.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label_user_logged_in.AutoSize = true;
            this.label_user_logged_in.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_user_logged_in.ForeColor = System.Drawing.Color.White;
            this.label_user_logged_in.Location = new System.Drawing.Point(891, 18);
            this.label_user_logged_in.Name = "label_user_logged_in";
            this.label_user_logged_in.Size = new System.Drawing.Size(86, 20);
            this.label_user_logged_in.TabIndex = 15;
            this.label_user_logged_in.Text = "John Doe";
            // 
            // label_howdy
            // 
            this.label_howdy.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label_howdy.AutoSize = true;
            this.label_howdy.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_howdy.ForeColor = System.Drawing.Color.White;
            this.label_howdy.Location = new System.Drawing.Point(894, 6);
            this.label_howdy.Name = "label_howdy";
            this.label_howdy.Size = new System.Drawing.Size(38, 12);
            this.label_howdy.TabIndex = 16;
            this.label_howdy.Text = "Howdy,";
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.panel_top);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.Name = "Form_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "rainCheck";
            this.panel_top.ResumeLayout(false);
            this.panel_top.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_top;
        private System.Windows.Forms.Label label_globe;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_top;
        private System.Windows.Forms.Label label_user_logged_in;
        private System.Windows.Forms.Label label_howdy;
    }
}