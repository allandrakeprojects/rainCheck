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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.panel_top = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label_globe = new System.Windows.Forms.Label();
            this.label_domains_list = new System.Windows.Forms.Label();
            this.label_separator = new System.Windows.Forms.Label();
            this.panel_resultbox = new System.Windows.Forms.Panel();
            this.textBox_search = new System.Windows.Forms.TextBox();
            this.dataGridView_devices = new System.Windows.Forms.DataGridView();
            this.label_search = new System.Windows.Forms.Label();
            this.panel_innerresultbox = new System.Windows.Forms.Panel();
            this.label_resultbox = new System.Windows.Forms.Label();
            this.label_bordersearch = new System.Windows.Forms.Label();
            this.panel_filterbox = new System.Windows.Forms.Panel();
            this.button_reset = new System.Windows.Forms.Button();
            this.comboBox_status = new System.Windows.Forms.ComboBox();
            this.label_status = new System.Windows.Forms.Label();
            this.comboBox_channel = new System.Windows.Forms.ComboBox();
            this.label_channel = new System.Windows.Forms.Label();
            this.comboBox_websitetype = new System.Windows.Forms.ComboBox();
            this.label_websitetype = new System.Windows.Forms.Label();
            this.comboBox_member = new System.Windows.Forms.ComboBox();
            this.label_member = new System.Windows.Forms.Label();
            this.comboBox_brands = new System.Windows.Forms.ComboBox();
            this.label_brands = new System.Windows.Forms.Label();
            this.panel_innerfilterbox = new System.Windows.Forms.Panel();
            this.label_filterbox = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel_top.SuspendLayout();
            this.panel_resultbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_devices)).BeginInit();
            this.panel_innerresultbox.SuspendLayout();
            this.panel_filterbox.SuspendLayout();
            this.panel_innerfilterbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_top
            // 
            this.panel_top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            this.panel_top.Controls.Add(this.label1);
            this.panel_top.Controls.Add(this.label_globe);
            this.panel_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_top.Location = new System.Drawing.Point(0, 0);
            this.panel_top.Name = "panel_top";
            this.panel_top.Size = new System.Drawing.Size(1014, 44);
            this.panel_top.TabIndex = 0;
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
            // label_domains_list
            // 
            this.label_domains_list.AutoSize = true;
            this.label_domains_list.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_domains_list.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(121)))), ((int)(((byte)(181)))));
            this.label_domains_list.Location = new System.Drawing.Point(10, 57);
            this.label_domains_list.Name = "label_domains_list";
            this.label_domains_list.Size = new System.Drawing.Size(167, 29);
            this.label_domains_list.TabIndex = 15;
            this.label_domains_list.Text = "Domain(s) List";
            // 
            // label_separator
            // 
            this.label_separator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_separator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.label_separator.Location = new System.Drawing.Point(15, 103);
            this.label_separator.Name = "label_separator";
            this.label_separator.Size = new System.Drawing.Size(987, 1);
            this.label_separator.TabIndex = 17;
            this.label_separator.Text = " ";
            // 
            // panel_resultbox
            // 
            this.panel_resultbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_resultbox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_resultbox.Controls.Add(this.textBox_search);
            this.panel_resultbox.Controls.Add(this.dataGridView_devices);
            this.panel_resultbox.Controls.Add(this.label_search);
            this.panel_resultbox.Controls.Add(this.panel_innerresultbox);
            this.panel_resultbox.Controls.Add(this.label_bordersearch);
            this.panel_resultbox.Location = new System.Drawing.Point(12, 245);
            this.panel_resultbox.Name = "panel_resultbox";
            this.panel_resultbox.Size = new System.Drawing.Size(846, 472);
            this.panel_resultbox.TabIndex = 19;
            // 
            // textBox_search
            // 
            this.textBox_search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_search.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_search.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_search.Location = new System.Drawing.Point(651, 59);
            this.textBox_search.Multiline = true;
            this.textBox_search.Name = "textBox_search";
            this.textBox_search.Size = new System.Drawing.Size(150, 20);
            this.textBox_search.TabIndex = 26;
            this.textBox_search.TextChanged += new System.EventHandler(this.TextBox_search_TextChanged);
            // 
            // dataGridView_devices
            // 
            this.dataGridView_devices.AllowUserToAddRows = false;
            this.dataGridView_devices.AllowUserToDeleteRows = false;
            this.dataGridView_devices.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(254)))));
            this.dataGridView_devices.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_devices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_devices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_devices.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_devices.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_devices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_devices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_devices.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView_devices.Location = new System.Drawing.Point(2, 89);
            this.dataGridView_devices.Name = "dataGridView_devices";
            this.dataGridView_devices.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(57)))), ((int)(((byte)(57)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_devices.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView_devices.RowHeadersVisible = false;
            this.dataGridView_devices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_devices.Size = new System.Drawing.Size(838, 378);
            this.dataGridView_devices.TabIndex = 15;
            // 
            // label_search
            // 
            this.label_search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_search.AutoSize = true;
            this.label_search.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_search.Location = new System.Drawing.Point(514, 64);
            this.label_search.Name = "label_search";
            this.label_search.Size = new System.Drawing.Size(129, 15);
            this.label_search.TabIndex = 25;
            this.label_search.Text = "Search domain name:";
            // 
            // panel_innerresultbox
            // 
            this.panel_innerresultbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_innerresultbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.panel_innerresultbox.Controls.Add(this.label_resultbox);
            this.panel_innerresultbox.Location = new System.Drawing.Point(1, 1);
            this.panel_innerresultbox.Name = "panel_innerresultbox";
            this.panel_innerresultbox.Size = new System.Drawing.Size(840, 45);
            this.panel_innerresultbox.TabIndex = 0;
            // 
            // label_resultbox
            // 
            this.label_resultbox.AutoSize = true;
            this.label_resultbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_resultbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(121)))), ((int)(((byte)(181)))));
            this.label_resultbox.Location = new System.Drawing.Point(21, 12);
            this.label_resultbox.Name = "label_resultbox";
            this.label_resultbox.Size = new System.Drawing.Size(86, 20);
            this.label_resultbox.TabIndex = 19;
            this.label_resultbox.Text = "Result Box";
            // 
            // label_bordersearch
            // 
            this.label_bordersearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_bordersearch.Location = new System.Drawing.Point(645, 54);
            this.label_bordersearch.Name = "label_bordersearch";
            this.label_bordersearch.Size = new System.Drawing.Size(162, 26);
            this.label_bordersearch.TabIndex = 27;
            this.label_bordersearch.Paint += new System.Windows.Forms.PaintEventHandler(this.Label_bordersearch_Paint);
            // 
            // panel_filterbox
            // 
            this.panel_filterbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_filterbox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_filterbox.Controls.Add(this.button_reset);
            this.panel_filterbox.Controls.Add(this.comboBox_status);
            this.panel_filterbox.Controls.Add(this.label_status);
            this.panel_filterbox.Controls.Add(this.comboBox_channel);
            this.panel_filterbox.Controls.Add(this.label_channel);
            this.panel_filterbox.Controls.Add(this.comboBox_websitetype);
            this.panel_filterbox.Controls.Add(this.label_websitetype);
            this.panel_filterbox.Controls.Add(this.comboBox_member);
            this.panel_filterbox.Controls.Add(this.label_member);
            this.panel_filterbox.Controls.Add(this.comboBox_brands);
            this.panel_filterbox.Controls.Add(this.label_brands);
            this.panel_filterbox.Controls.Add(this.panel_innerfilterbox);
            this.panel_filterbox.Location = new System.Drawing.Point(12, 116);
            this.panel_filterbox.Name = "panel_filterbox";
            this.panel_filterbox.Size = new System.Drawing.Size(847, 116);
            this.panel_filterbox.TabIndex = 20;
            // 
            // button_reset
            // 
            this.button_reset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(139)))), ((int)(((byte)(202)))));
            this.button_reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_reset.ForeColor = System.Drawing.Color.White;
            this.button_reset.Location = new System.Drawing.Point(746, 72);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(75, 26);
            this.button_reset.TabIndex = 38;
            this.button_reset.Text = "Reset";
            this.button_reset.UseVisualStyleBackColor = false;
            this.button_reset.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // comboBox_status
            // 
            this.comboBox_status.BackColor = System.Drawing.Color.White;
            this.comboBox_status.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_status.FormattingEnabled = true;
            this.comboBox_status.Items.AddRange(new object[] {
            "All",
            "Active",
            "Inactive"});
            this.comboBox_status.Location = new System.Drawing.Point(606, 76);
            this.comboBox_status.Name = "comboBox_status";
            this.comboBox_status.Size = new System.Drawing.Size(121, 21);
            this.comboBox_status.TabIndex = 37;
            this.comboBox_status.SelectedIndexChanged += new System.EventHandler(this.ComboBox_status_SelectedIndexChanged);
            // 
            // label_status
            // 
            this.label_status.AutoSize = true;
            this.label_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_status.Location = new System.Drawing.Point(603, 57);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(41, 15);
            this.label_status.TabIndex = 36;
            this.label_status.Text = "Status";
            // 
            // comboBox_channel
            // 
            this.comboBox_channel.BackColor = System.Drawing.Color.White;
            this.comboBox_channel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_channel.FormattingEnabled = true;
            this.comboBox_channel.Items.AddRange(new object[] {
            "All"});
            this.comboBox_channel.Location = new System.Drawing.Point(467, 76);
            this.comboBox_channel.Name = "comboBox_channel";
            this.comboBox_channel.Size = new System.Drawing.Size(121, 21);
            this.comboBox_channel.TabIndex = 35;
            this.comboBox_channel.SelectedIndexChanged += new System.EventHandler(this.ComboBox_channel_SelectedIndexChanged);
            // 
            // label_channel
            // 
            this.label_channel.AutoSize = true;
            this.label_channel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_channel.Location = new System.Drawing.Point(464, 57);
            this.label_channel.Name = "label_channel";
            this.label_channel.Size = new System.Drawing.Size(53, 15);
            this.label_channel.TabIndex = 34;
            this.label_channel.Text = "Channel";
            // 
            // comboBox_websitetype
            // 
            this.comboBox_websitetype.BackColor = System.Drawing.Color.White;
            this.comboBox_websitetype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_websitetype.FormattingEnabled = true;
            this.comboBox_websitetype.Items.AddRange(new object[] {
            "All"});
            this.comboBox_websitetype.Location = new System.Drawing.Point(326, 76);
            this.comboBox_websitetype.Name = "comboBox_websitetype";
            this.comboBox_websitetype.Size = new System.Drawing.Size(121, 21);
            this.comboBox_websitetype.TabIndex = 33;
            this.comboBox_websitetype.SelectedIndexChanged += new System.EventHandler(this.ComboBox_websitetype_SelectedIndexChanged);
            // 
            // label_websitetype
            // 
            this.label_websitetype.AutoSize = true;
            this.label_websitetype.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_websitetype.Location = new System.Drawing.Point(323, 57);
            this.label_websitetype.Name = "label_websitetype";
            this.label_websitetype.Size = new System.Drawing.Size(80, 15);
            this.label_websitetype.TabIndex = 32;
            this.label_websitetype.Text = "Website Type";
            // 
            // comboBox_member
            // 
            this.comboBox_member.BackColor = System.Drawing.Color.White;
            this.comboBox_member.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_member.FormattingEnabled = true;
            this.comboBox_member.Items.AddRange(new object[] {
            "All"});
            this.comboBox_member.Location = new System.Drawing.Point(187, 76);
            this.comboBox_member.Name = "comboBox_member";
            this.comboBox_member.Size = new System.Drawing.Size(121, 21);
            this.comboBox_member.TabIndex = 31;
            this.comboBox_member.SelectedIndexChanged += new System.EventHandler(this.ComboBox_member_SelectedIndexChanged);
            // 
            // label_member
            // 
            this.label_member.AutoSize = true;
            this.label_member.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_member.Location = new System.Drawing.Point(184, 57);
            this.label_member.Name = "label_member";
            this.label_member.Size = new System.Drawing.Size(54, 15);
            this.label_member.TabIndex = 30;
            this.label_member.Text = "Member";
            // 
            // comboBox_brands
            // 
            this.comboBox_brands.BackColor = System.Drawing.Color.White;
            this.comboBox_brands.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_brands.FormattingEnabled = true;
            this.comboBox_brands.Items.AddRange(new object[] {
            "All"});
            this.comboBox_brands.Location = new System.Drawing.Point(25, 76);
            this.comboBox_brands.Name = "comboBox_brands";
            this.comboBox_brands.Size = new System.Drawing.Size(121, 21);
            this.comboBox_brands.TabIndex = 29;
            this.comboBox_brands.SelectedIndexChanged += new System.EventHandler(this.ComboBox_brands_SelectedIndexChanged);
            // 
            // label_brands
            // 
            this.label_brands.AutoSize = true;
            this.label_brands.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_brands.Location = new System.Drawing.Point(22, 57);
            this.label_brands.Name = "label_brands";
            this.label_brands.Size = new System.Drawing.Size(46, 15);
            this.label_brands.TabIndex = 28;
            this.label_brands.Text = "Brands";
            // 
            // panel_innerfilterbox
            // 
            this.panel_innerfilterbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_innerfilterbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.panel_innerfilterbox.Controls.Add(this.label_filterbox);
            this.panel_innerfilterbox.Location = new System.Drawing.Point(1, 1);
            this.panel_innerfilterbox.Name = "panel_innerfilterbox";
            this.panel_innerfilterbox.Size = new System.Drawing.Size(843, 45);
            this.panel_innerfilterbox.TabIndex = 0;
            // 
            // label_filterbox
            // 
            this.label_filterbox.AutoSize = true;
            this.label_filterbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_filterbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(121)))), ((int)(((byte)(181)))));
            this.label_filterbox.Location = new System.Drawing.Point(21, 12);
            this.label_filterbox.Name = "label_filterbox";
            this.label_filterbox.Size = new System.Drawing.Size(75, 20);
            this.label_filterbox.TabIndex = 19;
            this.label_filterbox.Text = "Filter Box";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(270, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "label2";
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1014, 729);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel_filterbox);
            this.Controls.Add(this.panel_resultbox);
            this.Controls.Add(this.label_separator);
            this.Controls.Add(this.label_domains_list);
            this.Controls.Add(this.panel_top);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.Name = "Form_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "rainCheck";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Main_FormClosing);
            this.Load += new System.EventHandler(this.Form_Main_Load);
            this.panel_top.ResumeLayout(false);
            this.panel_top.PerformLayout();
            this.panel_resultbox.ResumeLayout(false);
            this.panel_resultbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_devices)).EndInit();
            this.panel_innerresultbox.ResumeLayout(false);
            this.panel_innerresultbox.PerformLayout();
            this.panel_filterbox.ResumeLayout(false);
            this.panel_filterbox.PerformLayout();
            this.panel_innerfilterbox.ResumeLayout(false);
            this.panel_innerfilterbox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel_top;
        private System.Windows.Forms.Label label_globe;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_domains_list;
        private System.Windows.Forms.Label label_separator;
        private System.Windows.Forms.Panel panel_resultbox;
        private System.Windows.Forms.Panel panel_innerresultbox;
        private System.Windows.Forms.Label label_resultbox;
        private System.Windows.Forms.DataGridView dataGridView_devices;
        private System.Windows.Forms.Panel panel_filterbox;
        private System.Windows.Forms.Panel panel_innerfilterbox;
        private System.Windows.Forms.Label label_filterbox;
        private System.Windows.Forms.TextBox textBox_search;
        private System.Windows.Forms.Label label_search;
        private System.Windows.Forms.Label label_bordersearch;
        private System.Windows.Forms.Label label_brands;
        private System.Windows.Forms.ComboBox comboBox_brands;
        private System.Windows.Forms.ComboBox comboBox_member;
        private System.Windows.Forms.Label label_member;
        private System.Windows.Forms.ComboBox comboBox_websitetype;
        private System.Windows.Forms.Label label_websitetype;
        private System.Windows.Forms.ComboBox comboBox_status;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.ComboBox comboBox_channel;
        private System.Windows.Forms.Label label_channel;
        private System.Windows.Forms.Button button_reset;
        private System.Windows.Forms.Label label2;
    }
}