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
            this.dataGridView_api_test = new System.Windows.Forms.DataGridView();
            this.label_api_test = new System.Windows.Forms.Label();
            this.timer_api_test = new System.Windows.Forms.Timer(this.components);
            this.button_post = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_api_test)).BeginInit();
            this.SuspendLayout();
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
            this.timer_api_test.Interval = 5000;
            this.timer_api_test.Tick += new System.EventHandler(this.timer_api_test_Tick);
            // 
            // button_post
            // 
            this.button_post.Location = new System.Drawing.Point(15, 364);
            this.button_post.Name = "button_post";
            this.button_post.Size = new System.Drawing.Size(75, 23);
            this.button_post.TabIndex = 74;
            this.button_post.Text = "post";
            this.button_post.UseVisualStyleBackColor = true;
            this.button_post.Click += new System.EventHandler(this.button_post_Click_1);
            // 
            // api_test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 580);
            this.Controls.Add(this.button_post);
            this.Controls.Add(this.label_api_test);
            this.Controls.Add(this.dataGridView_api_test);
            this.Name = "api_test";
            this.Text = "api_test";
            this.Load += new System.EventHandler(this.api_test_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_api_test)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView_api_test;
        private System.Windows.Forms.Label label_api_test;
        private System.Windows.Forms.Timer timer_api_test;
        private System.Windows.Forms.Button button_post;
    }
}