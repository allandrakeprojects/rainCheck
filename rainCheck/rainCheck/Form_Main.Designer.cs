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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.label_separator = new System.Windows.Forms.Label();
            this.dataGridView_devices = new System.Windows.Forms.DataGridView();
            this.label_globe = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel_top = new System.Windows.Forms.Panel();
            this.panel_browser = new System.Windows.Forms.Panel();
            this.textBox_domain = new System.Windows.Forms.TextBox();
            this.button_go = new System.Windows.Forms.Button();
            this.label_domain = new System.Windows.Forms.Label();
            this.button_start = new System.Windows.Forms.Button();
            this.button_pause = new System.Windows.Forms.Button();
            this.timer_timeout = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.button_upload = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_devices)).BeginInit();
            this.panel_top.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_separator
            // 
            this.label_separator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_separator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.label_separator.Location = new System.Drawing.Point(15, 70);
            this.label_separator.Name = "label_separator";
            this.label_separator.Size = new System.Drawing.Size(1207, 1);
            this.label_separator.TabIndex = 17;
            this.label_separator.Text = " ";
            // 
            // dataGridView_devices
            // 
            this.dataGridView_devices.AllowUserToAddRows = false;
            this.dataGridView_devices.AllowUserToDeleteRows = false;
            this.dataGridView_devices.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(254)))));
            this.dataGridView_devices.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView_devices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridView_devices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_devices.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_devices.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_devices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView_devices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_devices.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridView_devices.Location = new System.Drawing.Point(14, 90);
            this.dataGridView_devices.Name = "dataGridView_devices";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(57)))), ((int)(((byte)(57)))));
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_devices.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridView_devices.RowHeadersVisible = false;
            this.dataGridView_devices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_devices.Size = new System.Drawing.Size(180, 618);
            this.dataGridView_devices.TabIndex = 18;
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
            // panel_top
            // 
            this.panel_top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            this.panel_top.Controls.Add(this.label1);
            this.panel_top.Controls.Add(this.label_globe);
            this.panel_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_top.Location = new System.Drawing.Point(0, 0);
            this.panel_top.Name = "panel_top";
            this.panel_top.Size = new System.Drawing.Size(1234, 44);
            this.panel_top.TabIndex = 0;
            // 
            // panel_browser
            // 
            this.panel_browser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_browser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_browser.Location = new System.Drawing.Point(216, 125);
            this.panel_browser.Name = "panel_browser";
            this.panel_browser.Size = new System.Drawing.Size(1006, 513);
            this.panel_browser.TabIndex = 19;
            // 
            // textBox_domain
            // 
            this.textBox_domain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_domain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_domain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_domain.Location = new System.Drawing.Point(765, 95);
            this.textBox_domain.Multiline = true;
            this.textBox_domain.Name = "textBox_domain";
            this.textBox_domain.Size = new System.Drawing.Size(369, 18);
            this.textBox_domain.TabIndex = 20;
            this.textBox_domain.Text = "google.com";
            this.textBox_domain.WordWrap = false;
            this.textBox_domain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_domain_KeyDown);
            // 
            // button_go
            // 
            this.button_go.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_go.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(139)))), ((int)(((byte)(202)))));
            this.button_go.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_go.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_go.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_go.ForeColor = System.Drawing.Color.White;
            this.button_go.Location = new System.Drawing.Point(1147, 88);
            this.button_go.Name = "button_go";
            this.button_go.Size = new System.Drawing.Size(75, 30);
            this.button_go.TabIndex = 21;
            this.button_go.Text = "Go";
            this.button_go.UseVisualStyleBackColor = false;
            this.button_go.Click += new System.EventHandler(this.Button_go_Click);
            // 
            // label_domain
            // 
            this.label_domain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_domain.Location = new System.Drawing.Point(756, 89);
            this.label_domain.Name = "label_domain";
            this.label_domain.Size = new System.Drawing.Size(386, 29);
            this.label_domain.TabIndex = 22;
            this.label_domain.Paint += new System.Windows.Forms.PaintEventHandler(this.Label_domain_Paint);
            // 
            // button_start
            // 
            this.button_start.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(139)))), ((int)(((byte)(202)))));
            this.button_start.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_start.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_start.ForeColor = System.Drawing.Color.White;
            this.button_start.Image = ((System.Drawing.Image)(resources.GetObject("button_start.Image")));
            this.button_start.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_start.Location = new System.Drawing.Point(217, 88);
            this.button_start.Name = "button_start";
            this.button_start.Padding = new System.Windows.Forms.Padding(9, 0, 9, 0);
            this.button_start.Size = new System.Drawing.Size(82, 30);
            this.button_start.TabIndex = 23;
            this.button_start.Text = "Start";
            this.button_start.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_start.UseVisualStyleBackColor = false;
            this.button_start.Click += new System.EventHandler(this.Button_start_Click);
            // 
            // button_pause
            // 
            this.button_pause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(139)))), ((int)(((byte)(202)))));
            this.button_pause.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_pause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_pause.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_pause.ForeColor = System.Drawing.Color.White;
            this.button_pause.Image = global::rainCheck.Properties.Resources.pause;
            this.button_pause.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_pause.Location = new System.Drawing.Point(310, 88);
            this.button_pause.Name = "button_pause";
            this.button_pause.Padding = new System.Windows.Forms.Padding(3, 0, 2, 0);
            this.button_pause.Size = new System.Drawing.Size(82, 30);
            this.button_pause.TabIndex = 24;
            this.button_pause.Text = "Pause";
            this.button_pause.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_pause.UseVisualStyleBackColor = false;
            this.button_pause.Click += new System.EventHandler(this.Button_pause_Click);
            // 
            // timer_timeout
            // 
            this.timer_timeout.Interval = 1000;
            this.timer_timeout.Tick += new System.EventHandler(this.Timer_timeout_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(506, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "label3";
            // 
            // button_upload
            // 
            this.button_upload.Location = new System.Drawing.Point(561, 75);
            this.button_upload.Name = "button_upload";
            this.button_upload.Size = new System.Drawing.Size(75, 23);
            this.button_upload.TabIndex = 27;
            this.button_upload.Text = "test upload";
            this.button_upload.UseVisualStyleBackColor = true;
            this.button_upload.Click += new System.EventHandler(this.Button_upload_Click);
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1234, 729);
            this.Controls.Add(this.button_upload);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_pause);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.textBox_domain);
            this.Controls.Add(this.label_domain);
            this.Controls.Add(this.button_go);
            this.Controls.Add(this.dataGridView_devices);
            this.Controls.Add(this.panel_browser);
            this.Controls.Add(this.label_separator);
            this.Controls.Add(this.panel_top);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1250, 768);
            this.Name = "Form_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "rainCheck";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Main_FormClosing);
            this.Load += new System.EventHandler(this.Form_Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_devices)).EndInit();
            this.panel_top.ResumeLayout(false);
            this.panel_top.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label_separator;
        private System.Windows.Forms.DataGridView dataGridView_devices;
        private System.Windows.Forms.Label label_globe;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel_top;
        private System.Windows.Forms.Panel panel_browser;
        private System.Windows.Forms.TextBox textBox_domain;
        private System.Windows.Forms.Button button_go;
        private System.Windows.Forms.Label label_domain;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button button_pause;
        private System.Windows.Forms.Timer timer_timeout;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_upload;
    }
}