using System;
using System.Drawing;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class Form_Urgent : Form
    {
        public Form_Urgent()
        {
            InitializeComponent();
        }

        private void Label_domain_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, label_domain.DisplayRectangle, Color.Gray, ButtonBorderStyle.Solid);
        }
    }
}
