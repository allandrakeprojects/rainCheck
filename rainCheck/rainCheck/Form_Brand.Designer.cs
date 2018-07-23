namespace rainCheck
{
    partial class Form_Brand
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Brand));
            this.comboBox_brand = new System.Windows.Forms.ComboBox();
            this.label_help = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_start_urgent = new System.Windows.Forms.Button();
            this.label_brand_id = new System.Windows.Forms.Label();
            this.label_text_search = new System.Windows.Forms.Label();
            this.linkLabel_question = new System.Windows.Forms.LinkLabel();
            this.comboBox_websitetype = new System.Windows.Forms.ComboBox();
            this.label_websitetype = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox_brand
            // 
            this.comboBox_brand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_brand.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_brand.FormattingEnabled = true;
            this.comboBox_brand.Location = new System.Drawing.Point(90, 63);
            this.comboBox_brand.Name = "comboBox_brand";
            this.comboBox_brand.Size = new System.Drawing.Size(184, 26);
            this.comboBox_brand.TabIndex = 0;
            this.comboBox_brand.SelectedIndexChanged += new System.EventHandler(this.ComboBox_brand_SelectedIndexChanged);
            // 
            // label_help
            // 
            this.label_help.Cursor = System.Windows.Forms.Cursors.Default;
            this.label_help.Location = new System.Drawing.Point(26, 47);
            this.label_help.Name = "label_help";
            this.label_help.Size = new System.Drawing.Size(42, 43);
            this.label_help.TabIndex = 62;
            this.label_help.Text = " ";
            // 
            // label1
            // 
            this.label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.Location = new System.Drawing.Point(8, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 72);
            this.label1.TabIndex = 63;
            this.label1.Text = " ";
            // 
            // button_start_urgent
            // 
            this.button_start_urgent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(139)))), ((int)(((byte)(202)))));
            this.button_start_urgent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_start_urgent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_start_urgent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_start_urgent.ForeColor = System.Drawing.Color.White;
            this.button_start_urgent.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_start_urgent.Location = new System.Drawing.Point(470, 62);
            this.button_start_urgent.Name = "button_start_urgent";
            this.button_start_urgent.Padding = new System.Windows.Forms.Padding(13, 0, 12, 0);
            this.button_start_urgent.Size = new System.Drawing.Size(90, 28);
            this.button_start_urgent.TabIndex = 65;
            this.button_start_urgent.Text = "OK";
            this.button_start_urgent.UseVisualStyleBackColor = false;
            this.button_start_urgent.Click += new System.EventHandler(this.Button_start_urgent_Click);
            // 
            // label_brand_id
            // 
            this.label_brand_id.AutoSize = true;
            this.label_brand_id.Location = new System.Drawing.Point(183, 13);
            this.label_brand_id.Name = "label_brand_id";
            this.label_brand_id.Size = new System.Drawing.Size(49, 13);
            this.label_brand_id.TabIndex = 66;
            this.label_brand_id.Text = "Brand ID";
            this.label_brand_id.Visible = false;
            // 
            // label_text_search
            // 
            this.label_text_search.AutoSize = true;
            this.label_text_search.Location = new System.Drawing.Point(238, 12);
            this.label_text_search.Name = "label_text_search";
            this.label_text_search.Size = new System.Drawing.Size(65, 13);
            this.label_text_search.TabIndex = 67;
            this.label_text_search.Text = "Text Search";
            this.label_text_search.Visible = false;
            // 
            // linkLabel_question
            // 
            this.linkLabel_question.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            this.linkLabel_question.AutoSize = true;
            this.linkLabel_question.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            this.linkLabel_question.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.linkLabel_question.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(142)))), ((int)(((byte)(185)))));
            this.linkLabel_question.Location = new System.Drawing.Point(87, 37);
            this.linkLabel_question.Name = "linkLabel_question";
            this.linkLabel_question.Size = new System.Drawing.Size(409, 16);
            this.linkLabel_question.TabIndex = 69;
            this.linkLabel_question.TabStop = true;
            this.linkLabel_question.Text = "Select brand and website type for em.yb1223.com domain:";
            // 
            // comboBox_websitetype
            // 
            this.comboBox_websitetype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_websitetype.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_websitetype.FormattingEnabled = true;
            this.comboBox_websitetype.Location = new System.Drawing.Point(280, 63);
            this.comboBox_websitetype.Name = "comboBox_websitetype";
            this.comboBox_websitetype.Size = new System.Drawing.Size(184, 26);
            this.comboBox_websitetype.TabIndex = 70;
            this.comboBox_websitetype.SelectedIndexChanged += new System.EventHandler(this.comboBox_websitetype_SelectedIndexChanged);
            // 
            // label_websitetype
            // 
            this.label_websitetype.AutoSize = true;
            this.label_websitetype.Location = new System.Drawing.Point(364, 12);
            this.label_websitetype.Name = "label_websitetype";
            this.label_websitetype.Size = new System.Drawing.Size(35, 13);
            this.label_websitetype.TabIndex = 71;
            this.label_websitetype.Text = "label2";
            this.label_websitetype.Visible = false;
            // 
            // Form_Brand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(591, 126);
            this.ControlBox = false;
            this.Controls.Add(this.label_websitetype);
            this.Controls.Add(this.comboBox_websitetype);
            this.Controls.Add(this.linkLabel_question);
            this.Controls.Add(this.label_text_search);
            this.Controls.Add(this.label_brand_id);
            this.Controls.Add(this.button_start_urgent);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_help);
            this.Controls.Add(this.comboBox_brand);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(414, 165);
            this.Name = "Form_Brand";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "rainCheck";
            this.Load += new System.EventHandler(this.Form_Brand_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_brand;
        private System.Windows.Forms.Label label_help;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_start_urgent;
        private System.Windows.Forms.Label label_brand_id;
        private System.Windows.Forms.Label label_text_search;
        private System.Windows.Forms.LinkLabel linkLabel_question;
        private System.Windows.Forms.ComboBox comboBox_websitetype;
        private System.Windows.Forms.Label label_websitetype;
    }
}