namespace rainCheck
{
    partial class Form_Urgent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Urgent));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel_top = new System.Windows.Forms.Panel();
            this.panel_loader = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel_uploaded = new System.Windows.Forms.Panel();
            this.button_okay = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.panel_browser = new System.Windows.Forms.Panel();
            this.button_startover = new System.Windows.Forms.Button();
            this.textBox_domain = new System.Windows.Forms.TextBox();
            this.label_domain = new System.Windows.Forms.Label();
            this.pictureBox_loader = new System.Windows.Forms.PictureBox();
            this.button_start = new System.Windows.Forms.Button();
            this.panel_retry = new System.Windows.Forms.Panel();
            this.button_retry = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.dataGridView_history = new System.Windows.Forms.DataGridView();
            this.History = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label_status = new System.Windows.Forms.Label();
            this.label_cyclein = new System.Windows.Forms.Label();
            this.label_timefor_1 = new System.Windows.Forms.Label();
            this.label_status_1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel_loader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel_uploaded.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_loader)).BeginInit();
            this.panel_retry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_history)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_top
            // 
            this.panel_top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            this.panel_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_top.Location = new System.Drawing.Point(0, 0);
            this.panel_top.Name = "panel_top";
            this.panel_top.Size = new System.Drawing.Size(1238, 19);
            this.panel_top.TabIndex = 20;
            // 
            // panel_loader
            // 
            this.panel_loader.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel_loader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(69)))), ((int)(((byte)(87)))));
            this.panel_loader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_loader.Controls.Add(this.label7);
            this.panel_loader.Controls.Add(this.pictureBox1);
            this.panel_loader.Location = new System.Drawing.Point(510, 653);
            this.panel_loader.Name = "panel_loader";
            this.panel_loader.Size = new System.Drawing.Size(294, 219);
            this.panel_loader.TabIndex = 46;
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
            this.pictureBox1.Location = new System.Drawing.Point(70, 20);
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
            this.panel_uploaded.Location = new System.Drawing.Point(210, 653);
            this.panel_uploaded.Name = "panel_uploaded";
            this.panel_uploaded.Size = new System.Drawing.Size(294, 219);
            this.panel_uploaded.TabIndex = 47;
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
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(36, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(212, 25);
            this.label5.TabIndex = 1;
            this.label5.Text = "Upload Completed!";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel_browser
            // 
            this.panel_browser.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel_browser.BackColor = System.Drawing.Color.White;
            this.panel_browser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_browser.Location = new System.Drawing.Point(207, 73);
            this.panel_browser.Name = "panel_browser";
            this.panel_browser.Size = new System.Drawing.Size(1024, 583);
            this.panel_browser.TabIndex = 49;
            // 
            // button_startover
            // 
            this.button_startover.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_startover.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(139)))), ((int)(((byte)(202)))));
            this.button_startover.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_startover.Enabled = false;
            this.button_startover.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_startover.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_startover.ForeColor = System.Drawing.Color.White;
            this.button_startover.Image = global::rainCheck.Properties.Resources.start_over;
            this.button_startover.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_startover.Location = new System.Drawing.Point(43, 600);
            this.button_startover.Name = "button_startover";
            this.button_startover.Padding = new System.Windows.Forms.Padding(9, 0, 9, 0);
            this.button_startover.Size = new System.Drawing.Size(117, 30);
            this.button_startover.TabIndex = 52;
            this.button_startover.Text = "Start Over";
            this.button_startover.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_startover.UseVisualStyleBackColor = false;
            // 
            // textBox_domain
            // 
            this.textBox_domain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_domain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_domain.Enabled = false;
            this.textBox_domain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_domain.Location = new System.Drawing.Point(933, 40);
            this.textBox_domain.Multiline = true;
            this.textBox_domain.Name = "textBox_domain";
            this.textBox_domain.Size = new System.Drawing.Size(285, 18);
            this.textBox_domain.TabIndex = 50;
            this.textBox_domain.WordWrap = false;
            // 
            // label_domain
            // 
            this.label_domain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_domain.Location = new System.Drawing.Point(925, 34);
            this.label_domain.Name = "label_domain";
            this.label_domain.Size = new System.Drawing.Size(303, 30);
            this.label_domain.TabIndex = 51;
            this.label_domain.Paint += new System.Windows.Forms.PaintEventHandler(this.Label_domain_Paint);
            // 
            // pictureBox_loader
            // 
            this.pictureBox_loader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_loader.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_loader.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_loader.Image")));
            this.pictureBox_loader.Location = new System.Drawing.Point(861, 8);
            this.pictureBox_loader.Name = "pictureBox_loader";
            this.pictureBox_loader.Size = new System.Drawing.Size(92, 80);
            this.pictureBox_loader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_loader.TabIndex = 53;
            this.pictureBox_loader.TabStop = false;
            // 
            // button_start
            // 
            this.button_start.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(139)))), ((int)(((byte)(202)))));
            this.button_start.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_start.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_start.ForeColor = System.Drawing.Color.White;
            this.button_start.Image = global::rainCheck.Properties.Resources.start;
            this.button_start.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_start.Location = new System.Drawing.Point(207, 34);
            this.button_start.Name = "button_start";
            this.button_start.Padding = new System.Windows.Forms.Padding(13, 0, 12, 0);
            this.button_start.Size = new System.Drawing.Size(90, 30);
            this.button_start.TabIndex = 54;
            this.button_start.TabStop = false;
            this.button_start.Text = "Start";
            this.button_start.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_start.UseVisualStyleBackColor = false;
            // 
            // panel_retry
            // 
            this.panel_retry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(69)))), ((int)(((byte)(87)))));
            this.panel_retry.Controls.Add(this.button_retry);
            this.panel_retry.Controls.Add(this.label14);
            this.panel_retry.Location = new System.Drawing.Point(171, 665);
            this.panel_retry.Name = "panel_retry";
            this.panel_retry.Size = new System.Drawing.Size(1067, 12);
            this.panel_retry.TabIndex = 55;
            this.panel_retry.Visible = false;
            // 
            // button_retry
            // 
            this.button_retry.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button_retry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(139)))), ((int)(((byte)(202)))));
            this.button_retry.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_retry.FlatAppearance.BorderSize = 0;
            this.button_retry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_retry.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_retry.ForeColor = System.Drawing.Color.White;
            this.button_retry.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_retry.Location = new System.Drawing.Point(481, 18);
            this.button_retry.Name = "button_retry";
            this.button_retry.Size = new System.Drawing.Size(104, 29);
            this.button_retry.TabIndex = 12;
            this.button_retry.Text = "Retry (F5)";
            this.button_retry.UseVisualStyleBackColor = false;
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(406, -18);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(250, 25);
            this.label14.TabIndex = 2;
            this.label14.Text = "No Internet connection";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridView_history
            // 
            this.dataGridView_history.AllowUserToAddRows = false;
            this.dataGridView_history.AllowUserToDeleteRows = false;
            this.dataGridView_history.AllowUserToResizeRows = false;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(254)))));
            this.dataGridView_history.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle17;
            this.dataGridView_history.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridView_history.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_history.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_history.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            dataGridViewCellStyle18.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_history.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle18;
            this.dataGridView_history.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_history.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.History});
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_history.DefaultCellStyle = dataGridViewCellStyle19;
            this.dataGridView_history.Enabled = false;
            this.dataGridView_history.Location = new System.Drawing.Point(5, 35);
            this.dataGridView_history.MultiSelect = false;
            this.dataGridView_history.Name = "dataGridView_history";
            this.dataGridView_history.ReadOnly = true;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(57)))), ((int)(((byte)(57)))));
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_history.RowHeadersDefaultCellStyle = dataGridViewCellStyle20;
            this.dataGridView_history.RowHeadersVisible = false;
            this.dataGridView_history.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_history.Size = new System.Drawing.Size(196, 533);
            this.dataGridView_history.TabIndex = 57;
            // 
            // History
            // 
            this.History.HeaderText = "Domain(s) List";
            this.History.Name = "History";
            this.History.ReadOnly = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.White;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            this.label15.Location = new System.Drawing.Point(747, 39);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(89, 20);
            this.label15.TabIndex = 53;
            this.label15.Text = "69 min(s)";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.White;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            this.label16.Location = new System.Drawing.Point(694, 42);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(58, 16);
            this.label16.TabIndex = 52;
            this.label16.Text = "Cycle In:";
            // 
            // label_status
            // 
            this.label_status.AutoSize = true;
            this.label_status.BackColor = System.Drawing.Color.White;
            this.label_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_status.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            this.label_status.Location = new System.Drawing.Point(462, 39);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(84, 20);
            this.label_status.TabIndex = 51;
            this.label_status.Text = "[Waiting]";
            // 
            // label_cyclein
            // 
            this.label_cyclein.AutoSize = true;
            this.label_cyclein.BackColor = System.Drawing.Color.White;
            this.label_cyclein.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_cyclein.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            this.label_cyclein.Location = new System.Drawing.Point(624, 39);
            this.label_cyclein.Name = "label_cyclein";
            this.label_cyclein.Size = new System.Drawing.Size(55, 20);
            this.label_cyclein.TabIndex = 50;
            this.label_cyclein.Text = "12:00";
            // 
            // label_timefor_1
            // 
            this.label_timefor_1.AutoSize = true;
            this.label_timefor_1.BackColor = System.Drawing.Color.White;
            this.label_timefor_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_timefor_1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            this.label_timefor_1.Location = new System.Drawing.Point(564, 42);
            this.label_timefor_1.Name = "label_timefor_1";
            this.label_timefor_1.Size = new System.Drawing.Size(65, 16);
            this.label_timefor_1.TabIndex = 49;
            this.label_timefor_1.Text = "Time For:";
            // 
            // label_status_1
            // 
            this.label_status_1.AutoSize = true;
            this.label_status_1.BackColor = System.Drawing.Color.White;
            this.label_status_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_status_1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            this.label_status_1.Location = new System.Drawing.Point(418, 42);
            this.label_status_1.Name = "label_status_1";
            this.label_status_1.Size = new System.Drawing.Size(48, 16);
            this.label_status_1.TabIndex = 48;
            this.label_status_1.Text = "Status:";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(139)))), ((int)(((byte)(202)))));
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Image = global::rainCheck.Properties.Resources.globe;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(304, 34);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.button1.Size = new System.Drawing.Size(90, 30);
            this.button1.TabIndex = 58;
            this.button1.TabStop = false;
            this.button1.Text = "Domain";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // Form_Urgent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1238, 663);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.panel_top);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.dataGridView_history);
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.panel_loader);
            this.Controls.Add(this.label_cyclein);
            this.Controls.Add(this.panel_uploaded);
            this.Controls.Add(this.label_timefor_1);
            this.Controls.Add(this.panel_browser);
            this.Controls.Add(this.label_status_1);
            this.Controls.Add(this.button_startover);
            this.Controls.Add(this.textBox_domain);
            this.Controls.Add(this.label_domain);
            this.Controls.Add(this.pictureBox_loader);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.panel_retry);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form_Urgent";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "URGENT DOMAIN";
            this.panel_loader.ResumeLayout(false);
            this.panel_loader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel_uploaded.ResumeLayout(false);
            this.panel_uploaded.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_loader)).EndInit();
            this.panel_retry.ResumeLayout(false);
            this.panel_retry.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_history)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel_top;
        private System.Windows.Forms.Panel panel_loader;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel_uploaded;
        private System.Windows.Forms.Button button_okay;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel_browser;
        private System.Windows.Forms.Button button_startover;
        private System.Windows.Forms.TextBox textBox_domain;
        private System.Windows.Forms.Label label_domain;
        private System.Windows.Forms.PictureBox pictureBox_loader;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Panel panel_retry;
        private System.Windows.Forms.Button button_retry;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DataGridView dataGridView_history;
        private System.Windows.Forms.DataGridViewTextBoxColumn History;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.Label label_cyclein;
        private System.Windows.Forms.Label label_timefor_1;
        private System.Windows.Forms.Label label_status_1;
        private System.Windows.Forms.Button button1;
    }
}