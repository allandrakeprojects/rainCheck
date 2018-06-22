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

        string city_get;
        string isp_get;

        //MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;persistsecurityinfo=True;port=;database=raincheck;SslMode=none");

        public Form_Main(string city, string country, string isp)
        {
            InitializeComponent();

            //string city, string country, string isp
            this.Text = "rainCheck: " + city + ", " + country + " - " + isp;

            city_get = city;
            isp_get = isp;

            // Design
            this.WindowState = FormWindowState.Maximized;

            DataToGridView("SELECT CONCAT(b.brand_code, ' - ', REPEAT('*', length(d.domain_name)-5), RIGHT(d.domain_name, 5)) as 'Domain(s) List', d.domain_name, d.brand_name FROM domains d inner join brands b ON d.brand_name=b.brand_name order by FIELD(d.brand_name, 'Tian Fa', 'Chang Le', 'Feng Yin', 'Yong Bao', 'Ju Yi Tang')");
            //DataToGridView("select domain_name as 'Domain(s) List', brand_name from domains");
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            //dataGridView_domains.ClearSelection();

            InitializeChromium();

            foreach (DataGridViewColumn column in dataGridView_devices.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // Hide Column brand_name
            dataGridView_devices.Columns["domain_name"].Visible = false;
            dataGridView_devices.Columns["brand_name"].Visible = false;

            // Hide loader
            pictureBox_loader.Visible = false;

            // Clear Selection of datagridview
            dataGridView_devices.ClearSelection();

            label_domainhide.Text = "";
            label_brandhide.Text = "";
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

                //textBox_domain.Text = "google.com";

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
                this.Invoke(new Action(() =>
                {
                    pictureBox_loader.Visible = true;
                }));


                // Start time
                timer_timeout.Start();

                // Date preview
                start_load = DateTime.Now.ToString("HH:mm:ss");
            }
            else if (!e.IsLoading)
            {
                this.Invoke(new Action(() =>
                {
                    pictureBox_loader.Visible = false ;
                }));

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
            //MessageBox.Show("Date Today: " + datetime + "\n" +
            //                "Start Time: " + start_load + "\n" +
            //                "End Time: " + end_load);

            try
            {
                StreamWriter sw = new StreamWriter(Path.GetTempPath() + "\\import.txt", true, System.Text.Encoding.UTF8);
                sw.Close();
                
                string contain_text = label_domainhide.Text;
                if (File.ReadLines(Path.GetTempPath() + @"\import.txt").Any(line => line.Contains(contain_text)))
                {
                    // Leave for blank
                }
                else
                {
                    StreamWriter swww = new StreamWriter(Path.GetTempPath() + "\\import.txt", true, System.Text.Encoding.UTF8);

                    swww.WriteLine("null" + ","+label_domainhide.Text + ",successful" + ","+label_brandhide.Text + ",tprogram start" + "," + start_load + "," + end_load + ",text search" + ",url hijacker" + ",hijacker" + ",printscreen" + ","+isp_get + ","+city_get + "," + datetime + ",null");

                    swww.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message);
            }
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

        private void Button_go_Click(object sender, EventArgs e)
        {
            
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

            // Set selected in datagridview
            dataGridView_devices.Rows[0].Selected = true;
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
            string line;

            using (con)
            {
                try
                {
                    con.Open();
                    StreamReader sr = new StreamReader(Path.GetTempPath() + @"\import.txt", System.Text.Encoding.UTF8);
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] fields = line.Split(',');

                        MySqlCommand cmd = new MySqlCommand("INSERT INTO `reports`(`id`, `domain_name`, `status`, `brand`, `program_start`, `start_load`, `end_load`, `text_search`, `url_hijacker`, `hijacker`, `printscreen`, `isp`, `city`, `date_created`, `action_by`) VALUES " +
                            "(@id, @domain_name, @status, @brand, @program_start, @start_load, @end_load, @text_search, @url_hijacker, @hijacker, @printscreen, @isp, @city, @date_created, @action_by)", con);
                        cmd.Parameters.AddWithValue("@id", fields[0].ToString());
                        cmd.Parameters.AddWithValue("@domain_name", fields[1].ToString());
                        cmd.Parameters.AddWithValue("@status", fields[2].ToString());
                        cmd.Parameters.AddWithValue("@brand", fields[3].ToString());
                        cmd.Parameters.AddWithValue("@program_start", fields[4].ToString());
                        cmd.Parameters.AddWithValue("@start_load", fields[5].ToString());
                        cmd.Parameters.AddWithValue("@end_load", fields[6].ToString());
                        cmd.Parameters.AddWithValue("@text_search", fields[7].ToString());
                        cmd.Parameters.AddWithValue("@url_hijacker", fields[8].ToString());
                        cmd.Parameters.AddWithValue("@hijacker", fields[9].ToString());
                        cmd.Parameters.AddWithValue("@printscreen", fields[10].ToString());
                        cmd.Parameters.AddWithValue("@isp", fields[11].ToString());
                        cmd.Parameters.AddWithValue("@city", fields[12].ToString());
                        cmd.Parameters.AddWithValue("@date_created", fields[13].ToString());
                        cmd.Parameters.AddWithValue("@action_by", fields[14].ToString());
                        cmd.ExecuteNonQuery();
                    }

                    sr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There is a problem with the server! Please contact IT support." + ex.Message, "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        
        private void DataGridView_devices_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dataGridView_devices.CurrentCell == null || dataGridView_devices.CurrentCell.Value == null || e.RowIndex == -1)
            //{
            //    return;
            //}
            //else
            //{
            //    DataGridViewRow row = this.dataGridView_devices.Rows[e.RowIndex];
            //    string domain = dataGridView_devices.Rows[e.RowIndex].Cells[1].Value.ToString();
            //    string brand = dataGridView_devices.Rows[e.RowIndex].Cells[2].Value.ToString();

            //    // Load Browser
            //    chromeBrowser.Load(domain);

            //    label_domainhide.Text = domain;
            //    label_brandhide.Text = brand;
            //}
        }

        // SELECTED CHANGED
        private void DataGridView_devices_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView_devices.CurrentCell == null || dataGridView_devices.CurrentCell.Value == null)
            {
                return;
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView_devices.SelectedRows)
                {
                    string domain = row.Cells[1].Value.ToString();
                    string brand = row.Cells[2].Value.ToString();

                    try
                    {
                        // Load Browser
                        chromeBrowser.Load(domain);
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }

                    label_domainhide.Text = domain;
                    label_brandhide.Text = brand;
                }
            }

            //if (sender is DataGridView dgv && dgv.SelectedRows.Count > 0)
            //{
            //    DataGridViewRow row = dgv.SelectedRows[0];
            //    if (row != null)
            //    {
            //        string domain = row.Cells[1].Value.ToString();
            //        string brand = row.Cells[2].Value.ToString();

            //        label_domainhide.Text = domain;
            //        label_brandhide.Text = brand;

            //        // Load Browser
            //        chromeBrowser.Load(domain);
            //    }
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
                    MessageBox.Show("There is a problem with the server! Please contact IT support." + e.Message, "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
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