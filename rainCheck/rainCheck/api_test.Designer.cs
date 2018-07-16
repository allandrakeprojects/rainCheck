namespace rainCheck
{
    partial class api_test
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
            this.button_getmaindomains = new System.Windows.Forms.Button();
            this.dataGridView_api_test = new System.Windows.Forms.DataGridView();
            this.label_api_test = new System.Windows.Forms.Label();
            this.timer_api_test = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_api_test)).BeginInit();
            this.SuspendLayout();
            // 
            // button_getmaindomains
            // 
            this.button_getmaindomains.Location = new System.Drawing.Point(12, 258);
            this.button_getmaindomains.Name = "button_getmaindomains";
            this.button_getmaindomains.Size = new System.Drawing.Size(93, 23);
            this.button_getmaindomains.TabIndex = 71;
            this.button_getmaindomains.Text = "getmaindomains";
            this.button_getmaindomains.UseVisualStyleBackColor = true;
            this.button_getmaindomains.Click += new System.EventHandler(this.button_getmaindomains_Click);
            // 
            // dataGridView_api_test
            // 
            this.dataGridView_api_test.AllowUserToAddRows = false;
            this.dataGridView_api_test.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_api_test.Location = new System.Drawing.Point(180, 57);
            this.dataGridView_api_test.Name = "dataGridView_api_test";
            this.dataGridView_api_test.Size = new System.Drawing.Size(564, 437);
            this.dataGridView_api_test.TabIndex = 72;
            // 
            // label_api_test
            // 
            this.label_api_test.AutoSize = true;
            this.label_api_test.Location = new System.Drawing.Point(12, 306);
            this.label_api_test.Name = "label_api_test";
            this.label_api_test.Size = new System.Drawing.Size(41, 13);
            this.label_api_test.TabIndex = 73;
            this.label_api_test.Text = "api test";
            // 
            // timer_api_test
            // 
            this.timer_api_test.Enabled = true;
            this.timer_api_test.Tick += new System.EventHandler(this.timer_api_test_Tick);
            // 
            // api_test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 580);
            this.Controls.Add(this.label_api_test);
            this.Controls.Add(this.dataGridView_api_test);
            this.Controls.Add(this.button_getmaindomains);
            this.Name = "api_test";
            this.Text = "api_test";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_api_test)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_getmaindomains;
        private System.Windows.Forms.DataGridView dataGridView_api_test;
        private System.Windows.Forms.Label label_api_test;
        private System.Windows.Forms.Timer timer_api_test;
    }
}