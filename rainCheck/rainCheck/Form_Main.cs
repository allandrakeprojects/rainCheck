using CefSharp;
using CefSharp.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class Form_Main : Form
    {
        MySqlConnection con = new MySqlConnection("server=mysql5018.site4now.net;user id=a3d1a6_check;password=admin12345;database=db_a3d1a6_check;persistsecurityinfo=True;SslMode=none");

        public ChromiumWebBrowser chromeBrowser { get; private set; }

        //MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;persistsecurityinfo=True;port=;database=raincheck;SslMode=none");

        public Form_Main()
        {
            InitializeComponent();

            //string city, string country, string isp
            //this.Text = "rainCheck: " + city + ", " + country + " - " + isp;

            // Design
            this.WindowState = FormWindowState.Maximized;

            //DataToGridView("SELECT CONCAT(b.brand_code, ' - ', REPEAT('*', length(d.domain_name)-5), RIGHT(d.domain_name, 5)) as 'Domain(s) List' FROM domains d inner join brands b ON d.brandname=b.brand_name order by FIELD(d.brandname, 'Tian Fa', 'Chang Le', 'Feng Yin', 'Yong Bao', 'Ju Yi Tang')");
            DataToGridView("select domain_name as 'Domain(s) List' from domains");
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            //dataGridView_domains.ClearSelection();

            InitializeChromium();

            foreach (DataGridViewColumn column in dataGridView_devices.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void InitializeChromium()
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
                chromeBrowser.LoadingStateChanged += ChromiumWebBrowser_LoadingStateChanged;
                chromeBrowser.AddressChanged += ChromiumWebBrowser_AddressChanged;
                //chromeBrowser.LoadError += BrowserLoadError;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }



        private void ChromiumWebBrowser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            this.Invoke(new MethodInvoker(() => 
            {
                textBox_domain.Text = e.Address;
            }));
        }

        //private static void BrowserLoadError(object sender, LoadErrorEventArgs e)
        //{
        //    MessageBox.Show("This is not called");
        //}
        
        public int i = 1;
        private void Timer_timeout_Tick(object sender, EventArgs e)
        {
            if (InvokeRequired) { Invoke(new Action(() => { Timer_timeout_Tick(sender, e); })); return; }
            label3.Text = i++.ToString();

            //if(i >= 2)
            //{
            //    // timeout message
            //    MessageBox.Show("timeout");

            //    // Stop function
            //    timer_timeout.Stop();
            //    chromeBrowser.Stop();

            //    // Date preview
            //    string end_load = DateTime.Now.ToString("hh:mm:ss");
            //    MessageBox.Show(end_load);

            //    // Set i to 1
            //    i = 1;

            //    return;
            //}
        }

        string datetime = DateTime.Now.ToString("yyyy-MM-dd");
        string start_load = "";
        string end_load = "";

        public void ChromiumWebBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {   
            if (e.IsLoading)
            {
                // Start time
                timer_timeout.Start();

                // Date preview
                start_load = DateTime.Now.ToString("HH:mm:ss");
            }
            else if (!e.IsLoading)
            {
                // Start time
                timer_timeout.Stop();
                
                // Date preview
                end_load = DateTime.Now.ToString("HH:mm:ss");

                // Set i to 1
                i = 1;

                DataToTextFile();
            }

            

            //INSERT INTO `domains` (`id`, `domain_name`, `brandname`, `status`, `website_type`, `channel`, `purpose`, `member`, `remark`, `created_date`, `created_by`, `updated_date`, `updated_by`) VALUES
            //(1, '1052a.com', 'Chang Le', 'A', 'Home page', 'Sales', 'purpose example', 'Acq,VIP1,asd1', 'qweqwe', '2018-06-18 01:02:44', 'ADMIN', '2018-06-14 11:59:19', 'ADMIN'),
            //(2, '2589b.com', 'Chang Le', 'A', 'Forum', '', '', 'Acq,VIP1,VIP2', 'qweqwe', '2018-06-18 01:36:49', 'ADMIN', '2018-06-18 16:36:37', 'ADMIN');
        }

        private void DataToTextFile()
        {
            MessageBox.Show("Date Today: " + datetime + "\n" +
                            "Start Time: " + start_load + "\n" +
                            "End Time: " + end_load);

            try
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Import.txt", true);

                //Write a line of text
                //sw.WriteLine("null, \t " + textBox_domain.Text + " '\t successful' '\t brand' '\t program start' '\t" + start_load + "' '\t" + end_load + "' '\t text search' '\t url hijacker' '\t hijacker' '\t printscreen' '\t isp' '\t city' '\t  + datetime + "', \t null");


                sw.WriteLine("null" + "\t"+textBox_domain.Text + "\tsuccessful" + "\tbrand" + "\tprogram start" + "\t"+start_load + "\t"+end_load + "\ttext search" + "\turl hijacker" + "\thijacker" + "\tprintscreen" + "\tisp" + "\tcity" + "\t"+datetime + "\tnull");

                //Close the file
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message);
            }
        }

        private async void GetLoadTimeAsync()
        {
            JavascriptResponse JavaScriptResponsePageLoadTime = await chromeBrowser.EvaluateScriptAsync(@"
                (function() {
                    var perfData = window.performance.timing;
                    var pageLoadTime = perfData.loadEventEnd - perfData.navigationStart;
                    return pageLoadTime;
                 })();
                ");
            Int32 JavaScriptResultPageLoadTime = (Int32)(JavaScriptResponsePageLoadTime.Success ? (JavaScriptResponsePageLoadTime.Result ?? "") : JavaScriptResponsePageLoadTime.Message);
            MessageBox.Show(JavaScriptResultPageLoadTime.ToString());
        }

        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            //DialogResult dr = MessageBox.Show("Are you sure you want to exit the program?", "rainCheck", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (dr == DialogResult.No)
            //{
            //    e.Cancel = true;
            //}
            //else
            //{
            //    Cef.Shutdown();
            //}
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

        private void Button_upload_Click(object sender, EventArgs e)
        {
            using (con)
            {
                try
                {
                    con.Open();
                    string sql = "LOAD DATA LOCAL INFILE '" + AppDomain.CurrentDomain.BaseDirectory + "\\Import.txt" + "' INTO TABLE reports FIELDS TERMINATED BY '\t' LINES TERMINATED BY '\n'";

                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    con.Close();

                    //panel_blank.Visible = true;
                    //panel_blank.BringToFront();
                    panel_top.Visible = false;
                    MessageBox.Show("There is a problem with the server! Please contact IT support. asdsadsad2 " + ex.Message, "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}