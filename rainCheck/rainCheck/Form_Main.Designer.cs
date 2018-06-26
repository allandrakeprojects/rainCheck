﻿namespace rainCheck
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.label_separator = new System.Windows.Forms.Label();
            this.dataGridView_domain = new System.Windows.Forms.DataGridView();
            this.label_globe = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel_top = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label_rtc = new System.Windows.Forms.Label();
            this.panel_browser = new System.Windows.Forms.Panel();
            this.panel_loader = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel_uploaded = new System.Windows.Forms.Panel();
            this.button_okay = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_domain = new System.Windows.Forms.TextBox();
            this.button_go = new System.Windows.Forms.Button();
            this.label_domain = new System.Windows.Forms.Label();
            this.button_start = new System.Windows.Forms.Button();
            this.button_pause = new System.Windows.Forms.Button();
            this.timer_timeout = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label_brandhide = new System.Windows.Forms.Label();
            this.label_domainhide = new System.Windows.Forms.Label();
            this.pictureBox_loader = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.timer_domain = new System.Windows.Forms.Timer(this.components);
            this.label_timerstartpause = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_currentindex = new System.Windows.Forms.Label();
            this.timer_rtc = new System.Windows.Forms.Timer(this.components);
            this.timer_loader = new System.Windows.Forms.Timer(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.button_resume = new System.Windows.Forms.Button();
            this.label_status_1 = new System.Windows.Forms.Label();
            this.label_status = new System.Windows.Forms.Label();
            this.label_cyclein = new System.Windows.Forms.Label();
            this.label_cyclein_1 = new System.Windows.Forms.Label();
            this.timer_blink = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_domain)).BeginInit();
            this.panel_top.SuspendLayout();
            this.panel_loader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel_uploaded.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_loader)).BeginInit();
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
            // dataGridView_domain
            // 
            this.dataGridView_domain.AllowUserToAddRows = false;
            this.dataGridView_domain.AllowUserToDeleteRows = false;
            this.dataGridView_domain.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(254)))));
            this.dataGridView_domain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_domain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridView_domain.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_domain.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_domain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_domain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_domain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_domain.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView_domain.Enabled = false;
            this.dataGridView_domain.Location = new System.Drawing.Point(14, 90);
            this.dataGridView_domain.MultiSelect = false;
            this.dataGridView_domain.Name = "dataGridView_domain";
            this.dataGridView_domain.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(57)))), ((int)(((byte)(57)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_domain.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView_domain.RowHeadersVisible = false;
            this.dataGridView_domain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_domain.Size = new System.Drawing.Size(180, 618);
            this.dataGridView_domain.TabIndex = 18;
            this.dataGridView_domain.SelectionChanged += new System.EventHandler(this.DataGridView_devices_SelectionChanged);
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
            this.panel_top.Controls.Add(this.label11);
            this.panel_top.Controls.Add(this.label9);
            this.panel_top.Controls.Add(this.label8);
            this.panel_top.Controls.Add(this.label10);
            this.panel_top.Controls.Add(this.label_rtc);
            this.panel_top.Controls.Add(this.label1);
            this.panel_top.Controls.Add(this.label_globe);
            this.panel_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_top.Location = new System.Drawing.Point(0, 0);
            this.panel_top.Name = "panel_top";
            this.panel_top.Size = new System.Drawing.Size(1234, 44);
            this.panel_top.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(620, 17);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 13);
            this.label11.TabIndex = 45;
            this.label11.Text = "label11";
            this.label11.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(420, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 13);
            this.label9.TabIndex = 44;
            this.label9.Text = "label9";
            this.label9.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(383, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 13);
            this.label8.TabIndex = 43;
            this.label8.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(546, 17);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(0, 13);
            this.label10.TabIndex = 15;
            this.label10.Visible = false;
            // 
            // label_rtc
            // 
            this.label_rtc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_rtc.AutoSize = true;
            this.label_rtc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_rtc.ForeColor = System.Drawing.Color.White;
            this.label_rtc.Location = new System.Drawing.Point(1082, 12);
            this.label_rtc.Name = "label_rtc";
            this.label_rtc.Size = new System.Drawing.Size(46, 20);
            this.label_rtc.TabIndex = 14;
            this.label_rtc.Text = "RTC";
            // 
            // panel_browser
            // 
            this.panel_browser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_browser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_browser.Location = new System.Drawing.Point(216, 125);
            this.panel_browser.Name = "panel_browser";
            this.panel_browser.Size = new System.Drawing.Size(1006, 583);
            this.panel_browser.TabIndex = 19;
            // 
            // panel_loader
            // 
            this.panel_loader.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel_loader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(69)))), ((int)(((byte)(87)))));
            this.panel_loader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_loader.Controls.Add(this.label7);
            this.panel_loader.Controls.Add(this.pictureBox1);
            this.panel_loader.Location = new System.Drawing.Point(526, 714);
            this.panel_loader.Name = "panel_loader";
            this.panel_loader.Size = new System.Drawing.Size(294, 219);
            this.panel_loader.TabIndex = 16;
            this.panel_loader.Visible = false;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(56, 169);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(182, 16);
            this.label7.TabIndex = 1;
            this.label7.Text = "Uploading to the server...";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(76, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(135, 135);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel_uploaded
            // 
            this.panel_uploaded.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel_uploaded.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(69)))), ((int)(((byte)(87)))));
            this.panel_uploaded.Controls.Add(this.button_okay);
            this.panel_uploaded.Controls.Add(this.label5);
            this.panel_uploaded.Location = new System.Drawing.Point(200, 714);
            this.panel_uploaded.Name = "panel_uploaded";
            this.panel_uploaded.Size = new System.Drawing.Size(294, 219);
            this.panel_uploaded.TabIndex = 17;
            this.panel_uploaded.Visible = false;
            // 
            // button_okay
            // 
            this.button_okay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_okay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(139)))), ((int)(((byte)(202)))));
            this.button_okay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_okay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_okay.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_okay.ForeColor = System.Drawing.Color.White;
            this.button_okay.Location = new System.Drawing.Point(207, 175);
            this.button_okay.Name = "button_okay";
            this.button_okay.Size = new System.Drawing.Size(75, 31);
            this.button_okay.TabIndex = 22;
            this.button_okay.Text = "Okay";
            this.button_okay.UseVisualStyleBackColor = false;
            this.button_okay.Click += new System.EventHandler(this.Button_okay_Click);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(45, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(206, 25);
            this.label5.TabIndex = 1;
            this.label5.Text = "Upload Competed!";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_domain
            // 
            this.textBox_domain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_domain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_domain.Enabled = false;
            this.textBox_domain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_domain.Location = new System.Drawing.Point(764, 95);
            this.textBox_domain.Multiline = true;
            this.textBox_domain.Name = "textBox_domain";
            this.textBox_domain.Size = new System.Drawing.Size(370, 18);
            this.textBox_domain.TabIndex = 20;
            this.textBox_domain.WordWrap = false;
            this.textBox_domain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_domain_KeyDown);
            // 
            // button_go
            // 
            this.button_go.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_go.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(139)))), ((int)(((byte)(202)))));
            this.button_go.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_go.Enabled = false;
            this.button_go.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_go.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_go.ForeColor = System.Drawing.Color.White;
            this.button_go.Location = new System.Drawing.Point(1147, 88);
            this.button_go.Name = "button_go";
            this.button_go.Size = new System.Drawing.Size(75, 31);
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
            this.label_domain.Size = new System.Drawing.Size(386, 30);
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
            this.button_pause.Enabled = false;
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
            this.label3.Location = new System.Drawing.Point(821, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "label3";
            this.label3.Visible = false;
            // 
            // label_brandhide
            // 
            this.label_brandhide.AutoSize = true;
            this.label_brandhide.Location = new System.Drawing.Point(601, 53);
            this.label_brandhide.Name = "label_brandhide";
            this.label_brandhide.Size = new System.Drawing.Size(0, 13);
            this.label_brandhide.TabIndex = 28;
            this.label_brandhide.Visible = false;
            // 
            // label_domainhide
            // 
            this.label_domainhide.AutoSize = true;
            this.label_domainhide.Location = new System.Drawing.Point(662, 53);
            this.label_domainhide.Name = "label_domainhide";
            this.label_domainhide.Size = new System.Drawing.Size(0, 13);
            this.label_domainhide.TabIndex = 29;
            this.label_domainhide.Visible = false;
            // 
            // pictureBox_loader
            // 
            this.pictureBox_loader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_loader.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_loader.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_loader.Image")));
            this.pictureBox_loader.Location = new System.Drawing.Point(690, 63);
            this.pictureBox_loader.Name = "pictureBox_loader";
            this.pictureBox_loader.Size = new System.Drawing.Size(92, 80);
            this.pictureBox_loader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_loader.TabIndex = 31;
            this.pictureBox_loader.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(955, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "label2";
            this.label2.Visible = false;
            this.label2.TextChanged += new System.EventHandler(this.Label2_TextChanged);
            // 
            // timer_domain
            // 
            this.timer_domain.Tick += new System.EventHandler(this.Timer_domain_Tick);
            // 
            // label_timerstartpause
            // 
            this.label_timerstartpause.AutoSize = true;
            this.label_timerstartpause.Location = new System.Drawing.Point(759, 53);
            this.label_timerstartpause.Name = "label_timerstartpause";
            this.label_timerstartpause.Size = new System.Drawing.Size(35, 13);
            this.label_timerstartpause.TabIndex = 34;
            this.label_timerstartpause.Text = "label5";
            this.label_timerstartpause.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(889, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "label4";
            this.label4.Visible = false;
            // 
            // label_currentindex
            // 
            this.label_currentindex.AutoSize = true;
            this.label_currentindex.Location = new System.Drawing.Point(467, 52);
            this.label_currentindex.Name = "label_currentindex";
            this.label_currentindex.Size = new System.Drawing.Size(13, 13);
            this.label_currentindex.TabIndex = 37;
            this.label_currentindex.Text = "0";
            this.label_currentindex.Visible = false;
            // 
            // timer_rtc
            // 
            this.timer_rtc.Enabled = true;
            this.timer_rtc.Tick += new System.EventHandler(this.Timer_rtc_Tick);
            // 
            // timer_loader
            // 
            this.timer_loader.Interval = 1000;
            this.timer_loader.Tick += new System.EventHandler(this.Timer_loader_Tick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(361, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 38;
            this.label6.Text = "label6";
            this.label6.Visible = false;
            // 
            // button_resume
            // 
            this.button_resume.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(139)))), ((int)(((byte)(202)))));
            this.button_resume.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_resume.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_resume.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_resume.ForeColor = System.Drawing.Color.White;
            this.button_resume.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_resume.Location = new System.Drawing.Point(310, 88);
            this.button_resume.Name = "button_resume";
            this.button_resume.Size = new System.Drawing.Size(82, 30);
            this.button_resume.TabIndex = 39;
            this.button_resume.Text = "Resume";
            this.button_resume.UseVisualStyleBackColor = false;
            this.button_resume.Visible = false;
            this.button_resume.Click += new System.EventHandler(this.Button_resume_Click);
            // 
            // label_status_1
            // 
            this.label_status_1.AutoSize = true;
            this.label_status_1.BackColor = System.Drawing.Color.White;
            this.label_status_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_status_1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            this.label_status_1.Location = new System.Drawing.Point(407, 96);
            this.label_status_1.Name = "label_status_1";
            this.label_status_1.Size = new System.Drawing.Size(48, 16);
            this.label_status_1.TabIndex = 15;
            this.label_status_1.Text = "Status:";
            // 
            // label_status
            // 
            this.label_status.AutoSize = true;
            this.label_status.BackColor = System.Drawing.Color.White;
            this.label_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_status.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            this.label_status.Location = new System.Drawing.Point(451, 93);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(84, 20);
            this.label_status.TabIndex = 40;
            this.label_status.Text = "[Waiting]";
            // 
            // label_cyclein
            // 
            this.label_cyclein.AutoSize = true;
            this.label_cyclein.BackColor = System.Drawing.Color.White;
            this.label_cyclein.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_cyclein.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            this.label_cyclein.Location = new System.Drawing.Point(599, 93);
            this.label_cyclein.Name = "label_cyclein";
            this.label_cyclein.Size = new System.Drawing.Size(89, 20);
            this.label_cyclein.TabIndex = 42;
            this.label_cyclein.Text = "69 min(s)";
            // 
            // label_cyclein_1
            // 
            this.label_cyclein_1.AutoSize = true;
            this.label_cyclein_1.BackColor = System.Drawing.Color.White;
            this.label_cyclein_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_cyclein_1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            this.label_cyclein_1.Location = new System.Drawing.Point(546, 96);
            this.label_cyclein_1.Name = "label_cyclein_1";
            this.label_cyclein_1.Size = new System.Drawing.Size(58, 16);
            this.label_cyclein_1.TabIndex = 41;
            this.label_cyclein_1.Text = "Cycle In:";
            // 
            // timer_blink
            // 
            this.timer_blink.Interval = 600;
            this.timer_blink.Tick += new System.EventHandler(this.Timer_blink_Tick);
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1234, 729);
            this.Controls.Add(this.label_cyclein);
            this.Controls.Add(this.label_cyclein_1);
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.label_status_1);
            this.Controls.Add(this.panel_uploaded);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel_loader);
            this.Controls.Add(this.panel_browser);
            this.Controls.Add(this.label_currentindex);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label_timerstartpause);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_domainhide);
            this.Controls.Add(this.label_brandhide);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_pause);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.textBox_domain);
            this.Controls.Add(this.label_domain);
            this.Controls.Add(this.button_go);
            this.Controls.Add(this.dataGridView_domain);
            this.Controls.Add(this.label_separator);
            this.Controls.Add(this.panel_top);
            this.Controls.Add(this.pictureBox_loader);
            this.Controls.Add(this.button_resume);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1250, 768);
            this.Name = "Form_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "rainCheck";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Main_FormClosing);
            this.Load += new System.EventHandler(this.Form_Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_domain)).EndInit();
            this.panel_top.ResumeLayout(false);
            this.panel_top.PerformLayout();
            this.panel_loader.ResumeLayout(false);
            this.panel_loader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel_uploaded.ResumeLayout(false);
            this.panel_uploaded.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_loader)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label_separator;
        private System.Windows.Forms.DataGridView dataGridView_domain;
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
        private System.Windows.Forms.Label label_brandhide;
        private System.Windows.Forms.Label label_domainhide;
        private System.Windows.Forms.PictureBox pictureBox_loader;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer_domain;
        private System.Windows.Forms.Label label_timerstartpause;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_currentindex;
        private System.Windows.Forms.Label label_rtc;
        private System.Windows.Forms.Timer timer_rtc;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel_loader;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel_uploaded;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Timer timer_loader;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button_okay;
        private System.Windows.Forms.Button button_resume;
        private System.Windows.Forms.Label label_status_1;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.Label label_cyclein;
        private System.Windows.Forms.Label label_cyclein_1;
        private System.Windows.Forms.Timer timer_blink;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
    }
}