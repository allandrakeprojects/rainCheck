using CefSharp;
using CefSharp.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class Form_Main : Form
    {
        MySqlConnection con = new MySqlConnection("server=mysql5018.site4now.net;user id=a3d1a6_check;password=admin12345;database=db_a3d1a6_check;persistsecurityinfo=True;SslMode=none");

        public ChromiumWebBrowser chromeBrowser { get; private set; }

        //MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;persistsecurityinfo=True;port=;database=raincheck;SslMode=none");

        public Form_Main(string city, string country, string isp)
        {
            InitializeComponent();

            //string city, string country, string isp
            this.Text = "rainCheck: " + city + ", " + country + " - " + isp;

            // Design
            this.WindowState = FormWindowState.Maximized;

            DataToGridView("SELECT `domain_name` as 'Domain(s) List' FROM `domains`");
        }



        private void Form_Main_Load(object sender, EventArgs e)
        {
            //dataGridView_domains.ClearSelection();

            InitializeChromiumAsync();

            foreach (DataGridViewColumn column in dataGridView_devices.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public void InitializeChromiumAsync()
        {
            try
            {
                //using (ChromiumWebBrowser browser = new ChromiumWebBrowser("http://crawlbin.com/"))
                //{
                //    Stopwatch sw = new Stopwatch();
                //    sw.Start();
                //    await LoadPageAsync(browser);
                //    sw.Stop();
                //    string timeToLoad = sw.Elapsed.TotalSeconds.ToString();
                //}


                CefSettings settings = new CefSettings();
                
                Cef.Initialize(settings);

                textBox_domain.Text = "google.com";

                chromeBrowser = new ChromiumWebBrowser(textBox_domain.Text);

                panel_browser.Controls.Add(chromeBrowser);
                chromeBrowser.Dock = DockStyle.Fill;
                                               
                chromeBrowser.AddressChanged += ChromeBrowser_AddressChanged;

                //JavascriptResponse JavaScriptResponsePageLoadTime = await chromeBrowser.EvaluateScriptAsync(@"
                //(function() {
                //    var perfData = window.performance.timing;
                //    var pageLoadTime = perfData.loadEventEnd - perfData.navigationStart;
                //    return pageLoadTime;
                // })();
                //");
                //Int32 JavaScriptResultPageLoadTime = (Int32)(JavaScriptResponsePageLoadTime.Success ? (JavaScriptResponsePageLoadTime.Result ?? "") : JavaScriptResponsePageLoadTime.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void ChromeBrowser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            this.Invoke(new MethodInvoker(() => 
            {
                textBox_domain.Text = e.Address;
            }));
        }

        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to exit the program?", "rainCheck", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                Cef.Shutdown();
            }

        }

        // Show domains
        private void DataToGridView(string query)
        {
            using (con)
            {
                try
                {
                    con.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    adapter.SelectCommand = new MySqlCommand(query, con);

                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    if (table.Rows.Count == 0)
                    {
                        BindingSource source = new BindingSource();
                        source.DataSource = table;

                        table.Rows.Add("No data available in table");

                        dataGridView_devices.DataSource = source;

                        con.Close();

                        dataGridView_devices.ClearSelection();

                        dataGridView_devices.CellBorderStyle = DataGridViewCellBorderStyle.None;
                        dataGridView_devices.DefaultCellStyle.SelectionBackColor = dataGridView_devices.DefaultCellStyle.BackColor;
                        dataGridView_devices.DefaultCellStyle.SelectionForeColor = dataGridView_devices.DefaultCellStyle.ForeColor;
                    }
                    else
                    {
                        BindingSource source = new BindingSource();
                        source.DataSource = table;

                        dataGridView_devices.DataSource = source;

                        con.Close();

                        dataGridView_devices.ClearSelection();

                        dataGridView_devices.CellBorderStyle = DataGridViewCellBorderStyle.Single;

                        string hex = "#438eb9";
                        Color color = ColorTranslator.FromHtml(hex);
                        dataGridView_devices.DefaultCellStyle.SelectionBackColor = color;
                        dataGridView_devices.DefaultCellStyle.SelectionForeColor = Color.White;
                    }

                }
                catch (Exception e)
                {
                    con.Close();

                    //panel_blank.Visible = true;
                    //panel_blank.BringToFront();
                    panel_top.Visible = false;
                    MessageBox.Show("There is a problem with the server! Please contact IT support. main " + e.Message, "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void Button_go_Click(object sender, EventArgs e)
        {
            chromeBrowser.Load(textBox_domain.Text);
        }

        private void Label_domain_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, label_domain.DisplayRectangle, Color.Gray, ButtonBorderStyle.Solid);
        }

        private void TextBox_domain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void Button_start_Click(object sender, EventArgs e)
        {
            //DialogResult dr = MessageBox.Show("Are you sure you want to start?", "rainCheck", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (dr == DialogResult.Yes)
            //{
            //    MessageBox.Show("FIRE UP!");
            //}
        }

        private void Button_pause_Click(object sender, EventArgs e)
        {
            //DialogResult dr = MessageBox.Show("Are you sure you want to pause?", "rainCheck", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (dr == DialogResult.Yes)
            //{
            //    MessageBox.Show("PAUSE!");
            //}
        }
    }
}