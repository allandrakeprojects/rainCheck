namespace rainCheck
{
    partial class Form_Loader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Loader));
            this.label_raincheck = new System.Windows.Forms.Label();
            this.label_logo = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rectangleShape_loader_1 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rectangleShape_loader = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.label_timer = new System.Windows.Forms.Label();
            this.label_loader = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_raincheck
            // 
            this.label_raincheck.AutoSize = true;
            this.label_raincheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 24.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_raincheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(143)))), ((int)(((byte)(202)))));
            this.label_raincheck.Location = new System.Drawing.Point(194, 80);
            this.label_raincheck.Name = "label_raincheck";
            this.label_raincheck.Size = new System.Drawing.Size(165, 38);
            this.label_raincheck.TabIndex = 2;
            this.label_raincheck.Text = "rainCheck";
            // 
            // label_logo
            // 
            this.label_logo.Image = ((System.Drawing.Image)(resources.GetObject("label_logo.Image")));
            this.label_logo.Location = new System.Drawing.Point(146, 81);
            this.label_logo.Name = "label_logo";
            this.label_logo.Size = new System.Drawing.Size(56, 39);
            this.label_logo.TabIndex = 1;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectangleShape_loader_1,
            this.rectangleShape_loader});
            this.shapeContainer1.Size = new System.Drawing.Size(507, 239);
            this.shapeContainer1.TabIndex = 3;
            this.shapeContainer1.TabStop = false;
            // 
            // rectangleShape_loader_1
            // 
            this.rectangleShape_loader_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(173)))), ((int)(((byte)(236)))));
            this.rectangleShape_loader_1.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Opaque;
            this.rectangleShape_loader_1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(173)))), ((int)(((byte)(236)))));
            this.rectangleShape_loader_1.Location = new System.Drawing.Point(0, 190);
            this.rectangleShape_loader_1.Name = "rectangleShape_loader_1";
            this.rectangleShape_loader_1.Size = new System.Drawing.Size(10, 23);
            // 
            // rectangleShape_loader
            // 
            this.rectangleShape_loader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(143)))), ((int)(((byte)(202)))));
            this.rectangleShape_loader.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Opaque;
            this.rectangleShape_loader.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(143)))), ((int)(((byte)(202)))));
            this.rectangleShape_loader.Location = new System.Drawing.Point(0, 190);
            this.rectangleShape_loader.Name = "rectangleShape_loader";
            this.rectangleShape_loader.Size = new System.Drawing.Size(515, 23);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // label_timer
            // 
            this.label_timer.AutoSize = true;
            this.label_timer.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_timer.ForeColor = System.Drawing.Color.White;
            this.label_timer.Location = new System.Drawing.Point(259, 220);
            this.label_timer.Name = "label_timer";
            this.label_timer.Size = new System.Drawing.Size(13, 12);
            this.label_timer.TabIndex = 4;
            this.label_timer.Text = "%";
            // 
            // label_loader
            // 
            this.label_loader.AutoSize = true;
            this.label_loader.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_loader.ForeColor = System.Drawing.Color.White;
            this.label_loader.Location = new System.Drawing.Point(222, 220);
            this.label_loader.Name = "label_loader";
            this.label_loader.Size = new System.Drawing.Size(37, 12);
            this.label_loader.TabIndex = 5;
            this.label_loader.Text = "Loading";
            // 
            // Form_Loader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(69)))), ((int)(((byte)(87)))));
            this.ClientSize = new System.Drawing.Size(507, 239);
            this.Controls.Add(this.label_loader);
            this.Controls.Add(this.label_timer);
            this.Controls.Add(this.label_raincheck);
            this.Controls.Add(this.label_logo);
            this.Controls.Add(this.shapeContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(507, 239);
            this.MinimumSize = new System.Drawing.Size(507, 239);
            this.Name = "Form_Loader";
            this.Opacity = 0.86D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label_raincheck;
        private System.Windows.Forms.Label label_logo;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape_loader;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape_loader_1;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label label_timer;
        private System.Windows.Forms.Label label_loader;
    }
}

