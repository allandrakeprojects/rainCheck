using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class Test : Form
    {
        private ChromiumWebBrowser chromeBrowser;

        public Test()
        {
            InitializeComponent();
            //InitializeChromium();
            //Gecko.Xpcom.Initialize("Firefox");
        }

        public class BrowserTab : TabPage
        {
            WebBrowser wb = new WebBrowser();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(textBox1.Text);
            webBrowser1.ScriptErrorsSuppressed = true;
        }
    }
}
