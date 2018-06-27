using CefSharp;
using CefSharp.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class Form_Main : Form
    {
        MySqlConnection con = new MySqlConnection("server=mysql5018.site4now.net;user id=a3d1a6_check;password=admin12345;database=db_a3d1a6_check;persistsecurityinfo=True;SslMode=none");

        public ChromiumWebBrowser chromeBrowser { get; private set; }
        static bool networkIsAvailable = false;

        string city_get;
        string isp_get;
        int currentIndex;

        //MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;persistsecurityinfo=True;port=;database=raincheck;SslMode=none");

        public Form_Main()
        {
            InitializeComponent();

            //string city, string country, string isp
            //this.Text = "rainCheck: " + city + ", " + country + " - " + isp;

            //city_get = city;
            //isp_get = isp;

            // Design
            this.WindowState = FormWindowState.Maximized;

            DataToGridView("SELECT CONCAT(b.brand_code, ' - ', REPEAT('*', length(d.domain_name)-5), RIGHT(d.domain_name, 5)) as 'Domain(s) List', d.domain_name, b.id FROM domains d inner join brands b ON d.brand_name=b.id WHERE d.status='A' order by FIELD(d.brand_name, 'Tian Fa', 'Chang Le', 'Feng Yin', 'Yong Bao', 'Ju Yi Tang')");
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            //dataGridView_domains.ClearSelection();

            InitializeChromium();

            foreach (DataGridViewColumn column in dataGridView_domain.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // Hide Column brand_name
            dataGridView_domain.Columns["domain_name"].Visible = false;
            dataGridView_domain.Columns["id"].Visible = false;

            // Hide loader
            pictureBox_loader.Visible = false;

            // Clear Selection of datagridview
            dataGridView_domain.ClearSelection();

            label_domainhide.Text = "";
            label_brandhide.Text = "";

            panel_loader.Left = (ClientSize.Width - panel_loader.Width) / 2;
            panel_loader.Top = (ClientSize.Height - panel_loader.Height) / 2;

            panel_uploaded.Left = (ClientSize.Width - panel_uploaded.Width) / 2;
            panel_uploaded.Top = (ClientSize.Height - panel_uploaded.Height) / 2;

            // Check if result.txt have not yet uploaded
            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string path = path_desktop + "\\rainCheck\\";

            if (Directory.Exists(path))
            {
                var file = Directory.GetFiles(path, "pending.txt", SearchOption.AllDirectories)
                    .FirstOrDefault();
                if (file != null)
                {
                    //button_start.Enabled = false;
                    //MessageBox.Show("found");

                    
                }
                else
                {
                    //MessageBox.Show("not found");
                }
            }

            // Get timeout option to server
            using (con)
            {
                try
                {
                    con.Open();
                    MySqlCommand command = new MySqlCommand("SELECT timeout FROM `timeout`", con);
                    command.CommandType = CommandType.Text;
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            label13.Text = reader["timeout"].ToString();
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();

                    MessageBox.Show("There is a problem with the server! Please contact IT support." + ex.Message, "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                finally
                {
                    con.Close();
                }
            }

            // Enabling scrolls
            //dataGridView_domain.Controls[0].Enabled = true; // Index zero is the horizontal scrollbar
            //dataGridView_domain.Controls[1].Enabled = true; // Index one is the vertical scrollbar

            // Checking internet connection
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface nic in nics)
            {
                if (
                    (nic.NetworkInterfaceType != NetworkInterfaceType.Loopback && nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel) &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    networkIsAvailable = true;
                }
            }

            //if (networkIsAvailable)
            //{
            //    panel_retry.Visible = false;
            //}
            //else
            //{
            //    panel_retry.Visible = true;
            //}

            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);

            Console.ReadLine();
        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            networkIsAvailable = e.IsAvailable;

            if (networkIsAvailable)
            {
                Invoke(new Action(() =>
                {
                    panel_retry.Visible = false;

                    timer_blink.Stop();
                    label_status.Visible = true;
                    label_status.Text = "[Running]";
                    timer_domain.Start();

                    // For timeout
                    i = 1;
                    timer_timeout.Start();

                    pictureBox_loader.Visible = true;

                    int getCurrentIndex = Convert.ToInt32(label_currentindex.Text);
                    dataGridView_domain.ClearSelection();
                    dataGridView_domain.Rows[getCurrentIndex].Selected = true;

                    button_pause.Visible = true;
                    button_resume.Visible = false;

                    textBox_domain.Enabled = false;
                    button_go.Enabled = false;
                }));
            }
            else
            {
                Invoke(new Action(() =>
                {
                    panel_retry.Visible = true;

                    timer_blink.Start();
                    label_status.Text = "[Paused]";
                    timer_domain.Stop();
                    timer_timeout.Stop();
                    pictureBox_loader.Visible = false;
                    button_pause.Visible = false;
                    button_resume.Visible = true;

                    textBox_domain.Enabled = true;
                    button_go.Enabled = true;
                }));
            }
        }



        private void InitializeChromium()
        {
            try
            {
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
            Invoke(new MethodInvoker(() => 
            {
                textBox_domain.Text = e.Address;
            }));
        }

        private static void BrowserLoadError(object sender, LoadErrorEventArgs e)
        {
            MessageBox.Show("browserloaderror");
        }

        public int i = 1;
        private void Timer_timeout_Tick(object sender, EventArgs e)
        {
            if (InvokeRequired) { Invoke(new Action(() => { Timer_timeout_Tick(sender, e); })); return; }
            label3.Text = i++.ToString();
            
            if (label3.Text == label13.Text)
            {
                //MessageBox.Show("timeout");
                chromeBrowser.Stop();
                label12.Text = "timeout";
            }

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

        string start_load = "";
        string end_load = "";

        public void ChromiumWebBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (timer_domain.Enabled)
            {
                if (e.IsLoading)
                {
                    // Date preview
                    start_load = DateTime.Now.ToString("HH:mm:ss.fff");
                    
                    Invoke(new Action(() =>
                    {
                        timer_timeout.Start();
                        pictureBox_loader.Visible = true;
                        label2.Text = "1";
                    }));
                }

                if (!e.IsLoading)
                {
                    // Date preview
                    end_load = DateTime.Now.ToString("HH:mm:ss.fff");

                    // Send data to text file
                    if (label12.Text == "timeout")
                    {
                        DataToTextFileTimeout();
                    }
                    else
                    {
                        DataToTextFileSuccess();
                    }
                    
                    Invoke(new Action(() =>
                    {
                        timer_timeout.Stop();
                        i = 1;
                        pictureBox_loader.Visible = false;
                        label2.Text = "0";
                        label12.Text = "";
                    }));
                }
            }
            else if (buttonGoWasClicked == true)
            {
                if (e.IsLoading)
                {
                    // Date preview
                    start_load = DateTime.Now.ToString("HH:mm:ss.fff");

                    Invoke(new Action(() =>
                    {
                        pictureBox_loader.Visible = true;
                        button_go.Enabled = false;
                    }));
                }

                if (!e.IsLoading)
                {
                    // Date preview
                    end_load = DateTime.Now.ToString("HH:mm:ss.fff");

                    // Send data to text file
                    //DataToTextFile();

                    Invoke(new Action(() =>
                    {
                        pictureBox_loader.Visible = false;
                        button_go.Enabled = true;
                    }));
                }
            }
        }

        private void DataToTextFileSuccess()
        {
            //MessageBox.Show("Date Today: " + datetime + "\n" +
            //                "Start Time: " + start_load + "\n" +
            //                "End Time: " + end_load);

            try
            {
                string datetime = label10.Text;
                string datetime_folder = label8.Text;
                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                string path = path_desktop + "\\rainCheck\\" + datetime_folder;

                if (Directory.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path + "\\pending.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\pending.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\pending.txt", true, System.Text.Encoding.UTF8);

                        swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + ",text search" + ",url hijacker" + ",hijacker" + ",printscreen" + "," + isp_get + "," + city_get + "," + datetime + ",");

                        swww.Close();
                    }
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\pending.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\pending.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\pending.txt", true, System.Text.Encoding.UTF8);

                        swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + ",text search" + ",url hijacker" + ",hijacker" + ",printscreen" + "," + isp_get + "," + city_get + "," + datetime + ",");

                        swww.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message);
            }
        }

        private void DataToTextFileTimeout()
        {
            //MessageBox.Show("Date Today: " + datetime + "\n" +
            //                "Start Time: " + start_load + "\n" +
            //                "End Time: " + end_load);

            try
            {
                string datetime = label10.Text;
                string datetime_folder = label8.Text;
                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                string path = path_desktop + "\\rainCheck\\" + datetime_folder;

                if (Directory.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path + "\\pending.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\pending.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\pending.txt", true, System.Text.Encoding.UTF8);

                        swww.WriteLine("," + label_domainhide.Text + ",T" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + "," + "," + "," + "," + isp_get + "," + city_get + "," + datetime + ",");

                        swww.Close();
                    }
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\pending.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\pending.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\pending.txt", true, System.Text.Encoding.UTF8);

                        swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + ",text search" + ",url hijacker" + ",hijacker" + ",printscreen" + "," + isp_get + "," + city_get + "," + datetime + ",");

                        swww.Close();
                    }
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
            buttonGoWasClicked = true;
            label_domainhide.Text = textBox_domain.Text;
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
        
        private void Label2_TextChanged(object sender, EventArgs e)
        {
            if (label2.Text == "0")
            {                
                int domain_total = dataGridView_domain.RowCount;
                int index = dataGridView_domain.SelectedRows[0].Index + 1;
                label_currentindex.Text = index.ToString();

                if (index == domain_total)
                {
                    label_status.Text = "[Uploading]";
                    timer_domain.Stop();
                    dataGridView_domain.ClearSelection();

                    label8.Text = "";
                    label10.Text = "";

                    //string line;

                    //using (con)
                    //{
                    //    try
                    //    {
                    //        con.Open();
                    //        string datetime_folder = label8.Text;
                    //        string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                    //        string path = path_desktop + "\\rainCheck\\" + datetime_folder;
                    //        StreamReader sr = new StreamReader(path + @"\pending.txt", System.Text.Encoding.UTF8);
                    //        while ((line = sr.ReadLine()) != null)
                    //        {
                    //            string[] fields = line.Split(',');

                    //            MySqlCommand cmd = new MySqlCommand("INSERT INTO `reports`(`id`, `domain_name`, `status`, `brand`, `start_load`, `end_load`, `text_search`, `url_hijacker`, `hijacker`, `printscreen`, `isp`, `city`, `datetime_created`, `action_by`) VALUES " +
                    //                "(@id, @domain_name, @status, @brand, @start_load, @end_load, @text_search, @url_hijacker, @hijacker, @printscreen, @isp, @city, @datetime_created, @action_by)", con);
                    //            cmd.Parameters.AddWithValue("@id", fields[0].ToString());
                    //            cmd.Parameters.AddWithValue("@domain_name", fields[1].ToString());
                    //            cmd.Parameters.AddWithValue("@status", fields[2].ToString());
                    //            cmd.Parameters.AddWithValue("@brand", fields[3].ToString());
                    //            cmd.Parameters.AddWithValue("@start_load", fields[4].ToString());
                    //            cmd.Parameters.AddWithValue("@end_load", fields[5].ToString());
                    //            cmd.Parameters.AddWithValue("@text_search", fields[6].ToString());
                    //            cmd.Parameters.AddWithValue("@url_hijacker", fields[7].ToString());
                    //            cmd.Parameters.AddWithValue("@hijacker", fields[8].ToString());
                    //            cmd.Parameters.AddWithValue("@printscreen", fields[9].ToString());
                    //            cmd.Parameters.AddWithValue("@isp", fields[10].ToString());
                    //            cmd.Parameters.AddWithValue("@city", fields[11].ToString());
                    //            cmd.Parameters.AddWithValue("@datetime_created", fields[12].ToString());
                    //            cmd.Parameters.AddWithValue("@action_by", fields[13].ToString());
                    //            cmd.ExecuteNonQuery();
                    //        }

                    //        sr.Close();

                    //        panel_loader.Visible = true;
                    //        timer_loader.Start();

                    //        // rename pending.txt to result.txt
                    //        string old_path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\pending.txt";
                    //        string new_path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\result.txt";

                    //        File.Move(old_path, new_path);

                    //        label8.Text = "";
                    //        label10.Text = "";

                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        MessageBox.Show("There is a problem with the server! Please contact IT support." + ex.Message, "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    }
                    //}
                }
                else
                {
                    dataGridView_domain.FirstDisplayedScrollingRowIndex = index;
                    dataGridView_domain.Rows[index].Selected = true;
                }
            }
        }

        private void Button_start_Click(object sender, EventArgs e)
        {
            // For timeout
            i = 1;

            if (label8.Text == "")
            {
                label8.Text = label9.Text;
            }

            if (label10.Text == "")
            {
                label10.Text = label11.Text;
            }

            string datetime = label10.Text;
            string datetime_folder = label8.Text;
            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string path = path_desktop + "\\rainCheck\\" + datetime_folder;

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            timer_blink.Stop();
            label_status.Visible = true;
            button_pause.Visible = true;
            button_resume.Visible = false;
            label_status.Text = "[Running]";
            timer_domain.Start();
            //int getCurrentIndex = Convert.ToInt32(label_currentindex.Text);
            //dataGridView_domain.ClearSelection();
            dataGridView_domain.Rows[0].Selected = true;

            textBox_domain.Enabled = false;
            button_go.Enabled = false;

            button_pause.Enabled = true;
        }

        private void Button_pause_Click(object sender, EventArgs e)
        {
            timer_blink.Start();
            label_status.Text = "[Paused]";
            timer_domain.Stop();
            timer_timeout.Stop();
            pictureBox_loader.Visible = false;
            button_pause.Visible = false;
            button_resume.Visible = true;

            textBox_domain.Enabled = true;
            button_go.Enabled = true;
        }

        private void Button_resume_Click(object sender, EventArgs e)
        {
            timer_blink.Stop();
            label_status.Visible = true;
            label_status.Text = "[Running]";
            timer_domain.Start();

            // For timeout
            i = 1;
            timer_timeout.Start();

            int getCurrentIndex = Convert.ToInt32(label_currentindex.Text);
            dataGridView_domain.ClearSelection();
            dataGridView_domain.Rows[getCurrentIndex].Selected = true;

            button_pause.Visible = true;
            button_resume.Visible = false;

            textBox_domain.Enabled = false;
            button_go.Enabled = false;
        }

        // SELECTED CHANGED
        private void DataGridView_devices_SelectionChanged(object sender, EventArgs e)
        {
            //if (dataGridView_domain.SelectedCells.Count > 0)
            //{
            //    dataGridView_domain.ClearSelection();
            //}

            if (dataGridView_domain.CurrentCell == null || dataGridView_domain.CurrentCell.Value == null)
            {
                return;
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView_domain.SelectedRows)
                {
                    string domain = row.Cells[1].Value.ToString();
                    string brand = row.Cells[2].Value.ToString();
                    //currentIndex = dataGridView_domain.CurrentRow.Index;

                    try
                    {
                        // Load Browser
                        chromeBrowser.Load(domain);
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }

                    Invoke(new Action(() =>
                    {
                        label_domainhide.Text = domain;
                        label_brandhide.Text = brand;
                        //label4.Text = currentIndex.ToString();
                    }));
                }
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

                        dataGridView_domain.DataSource = source;

                        con.Close();

                        dataGridView_domain.ClearSelection();

                        dataGridView_domain.CellBorderStyle = DataGridViewCellBorderStyle.None;
                        dataGridView_domain.DefaultCellStyle.SelectionBackColor = dataGridView_domain.DefaultCellStyle.BackColor;
                        dataGridView_domain.DefaultCellStyle.SelectionForeColor = dataGridView_domain.DefaultCellStyle.ForeColor;
                    }
                    else
                    {
                        BindingSource source = new BindingSource();
                        source.DataSource = table;

                        dataGridView_domain.DataSource = source;

                        con.Close();

                        dataGridView_domain.ClearSelection();

                        dataGridView_domain.CellBorderStyle = DataGridViewCellBorderStyle.Single;

                        string hex = "#438eb9";
                        Color color = ColorTranslator.FromHtml(hex);
                        dataGridView_domain.DefaultCellStyle.SelectionBackColor = color;
                        dataGridView_domain.DefaultCellStyle.SelectionForeColor = Color.White;
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

        int domain_i = 0;
        private void Timer_domain_Tick(object sender, EventArgs e)
        {
            label_timerstartpause.Text = domain_i++.ToString();
        }

        private void Timer_rtc_Tick(object sender, EventArgs e)
        {
            string date = DateTime.Now.ToString("MMM dd");
            string time = DateTime.Now.ToString("hh:mm");
            label_rtc.Text = date + " " + time;


            string datetime_folder = DateTime.Now.ToString("yyyy-MM-dd_HHmm");
            label9.Text = datetime_folder;
            
            string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            label11.Text = datetime;
        }

        int timer_loader_uploaded = 0;
        int timer_loader_okay = 10;
        private bool buttonGoWasClicked;

        private void Timer_loader_Tick(object sender, EventArgs e)
        {
            timer_loader_uploaded++;
            timer_loader_okay -= 1;
            label6.Text = timer_loader_uploaded.ToString();
            button_okay.Text = "Okay ("+timer_loader_okay+")";

            if (timer_loader_uploaded == 5)
            {
                panel_loader.Visible = false;
                panel_uploaded.Visible = true;
            }

            if (timer_loader_uploaded == 10)
            {
                timer_loader_okay = 10;
                timer_loader_uploaded = 0;
                timer_loader.Stop();
                panel_uploaded.Visible = false;
                label_status.Text = "[Waiting]";
            }
        }

        private void Button_okay_Click(object sender, EventArgs e)
        {
            timer_loader_okay = 10;
            timer_loader_uploaded = 0;
            timer_loader.Stop();
            panel_uploaded.Visible = false;
        }

        private void Timer_blink_Tick(object sender, EventArgs e)
        {
            label_status.Visible = !label_status.Visible;
        }
    }
}