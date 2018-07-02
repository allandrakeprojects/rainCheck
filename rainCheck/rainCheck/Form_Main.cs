﻿using CefSharp;
using CefSharp.WinForms;
using Ionic.Zip;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class Form_Main : Form
    {
        MySqlConnection con = new MySqlConnection("server=mysql5018.site4now.net;user id=a3d1a6_check;password=admin12345;database=db_a3d1a6_check;persistsecurityinfo=True;SslMode=none");

        public ChromiumWebBrowser chromeBrowser { get; private set; }
        public ChromiumWebBrowser chromeBrowser_urgent { get; private set; }

        public static string SetValueForTextBrandID = "";

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
            //this.WindowState = FormWindowState.Maximized;

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
            

            // URGENT PANEL
            if (dataGridView_urgent.Rows.Count == 0)
            {
                button_start_urgent.Enabled = false;

                dataGridView_urgent.Rows.Add("No data available in table");
                dataGridView_urgent.ClearSelection();
                dataGridView_urgent.CellBorderStyle = DataGridViewCellBorderStyle.None;
                dataGridView_urgent.DefaultCellStyle.SelectionBackColor = dataGridView_urgent.DefaultCellStyle.BackColor;
                dataGridView_urgent.DefaultCellStyle.SelectionForeColor = dataGridView_urgent.DefaultCellStyle.ForeColor;
            }

            new ToolTip().SetToolTip(label_help, "Click Domain button to Import New Set of Domain(s)");

            dataGridView_urgent.Columns["brand_id"].Visible = false;
        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            networkIsAvailable = e.IsAvailable;

            if (networkIsAvailable)
            {
                Invoke(new Action(() =>
                {
                    int getCurrentIndex = Convert.ToInt32(label_currentindex.Text);
                    dataGridView_domain.ClearSelection();

                    if (getCurrentIndex > 0)
                    {
                        panel_retry.Visible = false;
                        panel_retry.BringToFront();

                        TopMost = true;
                        MinimizeBox = false;

                        timer_blink.Stop();
                        label_status.Visible = true;
                        label_status.Text = "[Running]";
                        timer_domain.Start();

                        // For timeout
                        i = 1;
                        timer_timeout.Start();

                        pictureBox_loader.Visible = true;

                        dataGridView_domain.Rows[getCurrentIndex].Selected = true;

                        button_pause.Visible = true;
                        button_start.Visible = false;

                        textBox_domain.Enabled = false;
                        button_go.Enabled = false;
                    }
                    else
                    {
                        panel_retry.Visible = false;

                        TopMost = true;
                        MinimizeBox = false;
                    }

                }));
            }
            else
            {
                Invoke(new Action(() =>
                {
                    panel_retry.Visible = true;
                    panel_retry.BringToFront();

                    TopMost = false;
                    MinimizeBox = true;
                    
                    timer_domain.Stop();
                    timer_timeout.Stop();
                    pictureBox_loader.Visible = false;
                    button_pause.Visible = false;
                    button_start.Visible = true;
                }));
            }
        }
        
        private void InitializeChromium()
        {
            try
            {
                CefSettings settings = new CefSettings();

                Cef.Initialize(settings);

                //chromeBrowser = new ChromiumWebBrowser(CustomLinks[0].ToString());
                //JsDialogHandler jss = new JsDialogHandler();
                //chromeBrowser.JsDialogHandler = jss;

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
                if (panel_main.Visible == true)
                {
                    textBox_domain.Text = e.Address;
                }
                else if (panel_urgent.Visible == true)
                {
                    textBox_domain_urgent.Text = e.Address;
                }
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
                chromeBrowser.Stop();
                label_timeout.Text = "timeout";
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
                        label_ifloadornot.Text = "1";
                    }));
                }

                if (!e.IsLoading)
                {
                    // Date preview
                    end_load = DateTime.Now.ToString("HH:mm:ss.fff");
                    // Send data to text file
                    if (label_timeout.Text == "timeout")
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
                        label_ifloadornot.Text = "0";
                        label_timeout.Text = "";
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
                        TopMost = true;
                        MinimizeBox = false;
                        timer_timeout.Start();
                        pictureBox_loader.Visible = true;
                        button_go.Enabled = false;
                        button_start.Enabled = false;
                    }));
                }

                if (!e.IsLoading)
                {
                    // Date preview
                    end_load = DateTime.Now.ToString("HH:mm:ss.fff");

                    // Send data to text file
                    if (label_timeout.Text == "timeout")
                    {
                        DataToTextFileTimeout();
                    }
                    else
                    {
                        DataToTextFileSuccess();
                    }

                    Invoke(new Action(() =>
                    {
                        TopMost = false;
                        MinimizeBox = true;
                        timer_timeout.Stop();
                        i = 1;
                        pictureBox_loader.Visible = false;
                        label_timeout.Text = "";
                        button_go.Enabled = true;
                        button_start.Enabled = true;
                        buttonGoWasClicked = false;
                    }));
                }
            }

            //if (panel_urgent.Visible == true)
            //{
            //    if (e.IsLoading)
            //    {
            //        // Date preview
            //        start_load = DateTime.Now.ToString("HH:mm:ss.fff");
            //        MessageBox.Show(start_load);
            //        Invoke(new Action(() =>
            //        {
            //            //timer_timeout.Start();
            //            pictureBox_loader_urgent.Visible = true;
            //            //label_ifloadornot.Text = "1";
            //        }));
            //    }

            //    if (!e.IsLoading)
            //    {
            //        // Date preview
            //        end_load = DateTime.Now.ToString("HH:mm:ss.fff");
            //        MessageBox.Show(end_load);
            //        // Send data to text file
            //        if (label_timeout.Text == "timeout")
            //        {
            //            DataToTextFileTimeout();
            //        }
            //        else
            //        {
            //            DataToTextFileSuccess();
            //        }

            //        Invoke(new Action(() =>
            //        {
            //            //timer_timeout.Stop();
            //            //i = 1;
            //            pictureBox_loader_urgent.Visible = false;
            //            //label_ifloadornot.Text = "0";
            //            //label_timeout.Text = "";
            //        }));
            //    }
            //}
        }

        public bool OnJSDialog(IWebBrowser browserControl, IBrowser browser, string originUrl, CefJsDialogType dialogType, string messageText, string defaultPromptText, IJsDialogCallback callback, ref bool suppressMessage)
        {
            callback.Continue(true);
            return true;
        }
        //public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IWindowInfo windowInfo, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        //{
        //    //System.Diagnostics.Process.Start(targetUrl);
        //    //newBrowser = null;
        //    //return false;

        //    chromeBrowser.Load(targetUrl);
        //    return false;
        //}

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
                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);

                        swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + ",text search" + ",url hijacker" + ",hijacker" + ",printscreen" + "," + isp_get + "," + city_get + "," + datetime + ",");

                        swww.Close();
                    }
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);

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
                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);

                        swww.WriteLine("," + label_domainhide.Text + ",T" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + "," + "," + "," + "," + isp_get + "," + city_get + "," + datetime + ",");

                        swww.Close();
                    }
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);

                        swww.WriteLine("," + label_domainhide.Text + ",T" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + "," + "," + "," + "," + isp_get + "," + city_get + "," + datetime + ",");

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
            // Set browser panel dock style
            chromeBrowser.Dock = DockStyle.Fill;

            i = 1;
            buttonGoWasClicked = true;
            label_domainhide.Text = textBox_domain.Text;
            label_brandhide.Text = "";
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
            if (label_ifloadornot.Text == "0")
            {                
                int domain_total = dataGridView_domain.RowCount;
                int index = dataGridView_domain.SelectedRows[0].Index + 1;
                label_currentindex.Text = index.ToString();

                if (index == domain_total)
                {
                    // Set browser panel dock style
                    chromeBrowser.Dock = DockStyle.None;
                    textBox_domain.Text = "";

                    dataGridView_domain.ClearSelection();

                    // Enable visible buttons
                    button_start.Visible = true;
                    button_pause.Visible = false;
                    //button_start.Enabled = false;
                    button_startover.Enabled = false;
                    
                    label_status.Text = "[Loading]";
                    timer_domain.Stop();

                    //////////////////////////
                    //string datetime_folder = label8.Text;
                    //string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                    //string path = path_desktop + "\\rainCheck\\" + datetime_folder;
                    //string path_other = path_desktop + "\\rainCheck\\result.txt";

                    //panel_loader.Visible = true;
                    //panel_loader.BringToFront();
                    //timer_loader.Start();

                    //label_currentindex.Text = "0";

                    //label8.Text = "";
                    //label10.Text = "";

                    //using (ZipFile zip = new ZipFile())
                    //{
                    //    try
                    //    {
                    //        string outputpath = path_desktop + "\\rainCheck\\" + datetime_folder + ".zip";
                    //        zip.Password = "youdidntknowthispasswordhaha";
                    //        zip.Password = "a";
                    //        zip.AddDirectory(path);
                    //        zip.Save(outputpath);

                    //        if (Directory.Exists(path))
                    //        {
                    //            Directory.Delete(path, true);
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        MessageBox.Show(ex.Message);
                    //    }
                    //}
                    ////////////////////////

                    TopMost = false;
                    MinimizeBox = true;

                    string line;

                    using (con)
                    {
                        try
                        {
                            con.Open();
                            string datetime_folder = label8.Text;
                            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                            string path = path_desktop + "\\rainCheck\\" + datetime_folder;

                            StreamReader sr = new StreamReader(path + @"\result.txt", System.Text.Encoding.UTF8);
                            while ((line = sr.ReadLine()) != null)
                            {
                                string[] fields = line.Split(',');

                                MySqlCommand cmd = new MySqlCommand("INSERT INTO `reports`(`id`, `domain_name`, `status`, `brand`, `start_load`, `end_load`, `text_search`, `url_hijacker`, `hijacker`, `printscreen`, `isp`, `city`, `datetime_created`, `action_by`) VALUES " +
                                    "(@id, @domain_name, @status, @brand, @start_load, @end_load, @text_search, @url_hijacker, @hijacker, @printscreen, @isp, @city, @datetime_created, @action_by)", con);
                                cmd.Parameters.AddWithValue("@id", fields[0].ToString());
                                cmd.Parameters.AddWithValue("@domain_name", fields[1].ToString());
                                cmd.Parameters.AddWithValue("@status", fields[2].ToString());
                                cmd.Parameters.AddWithValue("@brand", fields[3].ToString());
                                cmd.Parameters.AddWithValue("@start_load", fields[4].ToString());
                                cmd.Parameters.AddWithValue("@end_load", fields[5].ToString());
                                cmd.Parameters.AddWithValue("@text_search", fields[6].ToString());
                                cmd.Parameters.AddWithValue("@url_hijacker", fields[7].ToString());
                                cmd.Parameters.AddWithValue("@hijacker", fields[8].ToString());
                                cmd.Parameters.AddWithValue("@printscreen", fields[9].ToString());
                                cmd.Parameters.AddWithValue("@isp", fields[10].ToString());
                                cmd.Parameters.AddWithValue("@city", fields[11].ToString());
                                cmd.Parameters.AddWithValue("@datetime_created", fields[12].ToString());
                                cmd.Parameters.AddWithValue("@action_by", fields[13].ToString());
                                cmd.ExecuteNonQuery();
                            }

                            sr.Close();

                            panel_loader.Visible = true;
                            timer_loader.Start();

                            label_currentindex.Text = "0";

                            label8.Text = "";
                            label10.Text = "";

                            using (ZipFile zip = new ZipFile())
                            {
                                try
                                {
                                    string outputpath = path_desktop + "\\rainCheck\\" + datetime_folder + ".zip";
                                    //zip.Password = "youdidntknowthispasswordhaha";
                                    zip.Password = "a";
                                    zip.AddDirectory(path);
                                    zip.Save(outputpath);

                                    if (Directory.Exists(path))
                                    {
                                        Directory.Delete(path, true);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("There is a problem with the server! Please contact IT support." + ex.Message, "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
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
            DialogResult dr = MessageBox.Show("Are you sure you want to Start Over domain checking?", "rainCheck", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {

            }
            else
            {
                // Set browser panel dock style
                chromeBrowser.Dock = DockStyle.Fill;

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
                button_start.Visible = false;
                label_status.Text = "[Running]";
                timer_domain.Start();
                //int getCurrentIndex = Convert.ToInt32(label_currentindex.Text);
                //dataGridView_domain.ClearSelection();
                dataGridView_domain.Rows[0].Selected = true;

                textBox_domain.Enabled = false;
                button_go.Enabled = false;

                button_pause.Enabled = true;
            }
        }

        private void Button_pause_Click(object sender, EventArgs e)
        {
            TopMost = false;
            MinimizeBox = true;

            // Set browser panel dock style
            chromeBrowser.Dock = DockStyle.None;

            timer_blink.Start();
            label_status.Text = "[Paused]";
            timer_domain.Stop();
            timer_timeout.Stop();
            pictureBox_loader.Visible = false;

            button_pause.Visible = false;
            button_start.Visible = true;

            textBox_domain.Enabled = true;
            button_go.Enabled = true;

            textBox_domain.Text = "";
            ActiveControl = textBox_domain;

            button_urgent.Visible = true;
        }

        private void Button_resume_Click(object sender, EventArgs e)
        {
            panel_browser.Controls.Add(chromeBrowser);

            TopMost = true;
            MinimizeBox = false;

            pictureBox_loader.Visible = true;

            // Set browser panel dock style
            chromeBrowser.Dock = DockStyle.Fill;

            if (label8.Text == "")
            {
                label8.Text = label9.Text;
            }

            if (label10.Text == "")
            {
                label10.Text = label11.Text;
            }

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
            button_start.Visible = false;

            textBox_domain.Enabled = false;
            button_go.Enabled = false;

            button_startover.Enabled = true;
            
            button_urgent.Visible = false;
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
                        button_start.Enabled = false;

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

            if (timer_loader_uploaded == 1)
            {
                label_status.Text = "[Uploading]";

                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string path_other = path_desktop + "\\rainCheck\\result.txt";
                
                if (File.Exists(path_other))
                {
                    File.Delete(path_other);
                }
            }

            if (timer_loader_uploaded == 5)
            {
                label_status.Text = "[Waiting]";
                panel_loader.Visible = false;
                panel_uploaded.Visible = true;

                panel_uploaded.BringToFront();
            }

            if (timer_loader_uploaded == 10)
            {
                timer_loader_okay = 10;
                timer_loader_uploaded = 0;
                timer_loader.Stop();
                panel_uploaded.Visible = false;
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

        private void button1_Click(object sender, EventArgs e)
        {
            //Bitmap bitmap = new Bitmap(Width, Height);
            //DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
            //bitmap.Save("C:\\Users\\adulay\\Desktop\\testdomain.png", ImageFormat.Png);
            
            Rectangle bounds = Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width-266, bounds.Height-201))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left+225, bounds.Top+158), Point.Empty, bounds.Size);
                }
                Bitmap resized = new Bitmap(bitmap, new Size(bitmap.Width / 2, bitmap.Height / 2));
                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                resized.Save(path_desktop + "\\testdomain.jpeg", ImageFormat.Jpeg);
            }

            //MessageBox.Show("ok!");
        }

        private void Button_urgent_Click(object sender, EventArgs e)
        {
            //Form_Urgent form_urgent = new Form_Urgent();
            //form_urgent.ShowDialog();
            
            panel_urgent.Visible = true;

            button_urgent.Visible = false;

            panel_main.Visible = false;

            label_back.Visible = true;
            label_domain_urgent.Visible = true;

            textBox_domain_urgent.Text = "";
        }

        private void Label_back_Click(object sender, EventArgs e)
        {
            panel_urgent.Visible = false ;

            button_urgent.Visible = true;
            panel_main.Visible = true;

            label_back.Visible = false;
            label_domain_urgent.Visible = false;
        }

        private void Button_domain_urgent_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.FileName = "";
            ofd.Title = "title";
            ofd.Filter = "Text Files (*.txt)|*.txt";
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                DataTable dt = new DataTable();
                
                try
                {
                    while (dataGridView_urgent.Rows.Count > 0)
                    {
                        dataGridView_urgent.Rows.RemoveAt(0);
                    }

                    StreamReader streamReader = new StreamReader(ofd.FileName);
                    string text = "";
                    for (text = streamReader.ReadLine(); text != null; text = streamReader.ReadLine())
                    {
                        using (con)
                        {
                            try
                            {
                                con.Open();
                                MySqlCommand command = new MySqlCommand("SELECT brand_name FROM `domains` WHERE domain_name = '"+ text +"'", con);
                                command.CommandType = CommandType.Text;
                                MySqlDataReader reader = command.ExecuteReader();

                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        label_brand_id.Text = reader["brand_name"].ToString();

                                    }
                                }
                                else
                                {
                                    label_brand_id.Text = "";

                                    Form_Brand form_brand = new Form_Brand(text);
                                    form_brand.ShowDialog();

                                    label_brand_id.Text = SetValueForTextBrandID;
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
                        
                        dataGridView_urgent.Rows.Add(text, label_brand_id.Text);
                    }

                    button_start_urgent.Enabled = true;

                    dataGridView_urgent.ClearSelection();

                    dataGridView_urgent.CellBorderStyle = DataGridViewCellBorderStyle.Single;

                    string hex = "#438eb9";
                    Color color = ColorTranslator.FromHtml(hex);
                    dataGridView_urgent.DefaultCellStyle.SelectionBackColor = color;
                    dataGridView_urgent.DefaultCellStyle.SelectionForeColor = Color.White;

                    label_brand_id.Text = "";

                    streamReader.Close();
                }
                catch (Exception err)
                {
                    MessageBox.Show("Error" + err.Message);
                }

                if(dataGridView_urgent.Rows.Count > 0)
                {
                    label_clear.Visible = true;
                    label_help.Visible = false;
                }

                //dataGridView_urgent.DataSource = dt;
            }
        }

        private void Label_clear_Click(object sender, EventArgs e)
        {
            while (dataGridView_urgent.Rows.Count > 0)
            {
                dataGridView_urgent.Rows.RemoveAt(0);
            }

            button_start_urgent.Enabled = false;
            label_clear.Visible = false;
            label_help.Visible = true;
            
            dataGridView_urgent.Rows.Add("No data available in table");
            dataGridView_urgent.ClearSelection();
            dataGridView_urgent.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView_urgent.DefaultCellStyle.SelectionBackColor = dataGridView_urgent.DefaultCellStyle.BackColor;
            dataGridView_urgent.DefaultCellStyle.SelectionForeColor = dataGridView_urgent.DefaultCellStyle.ForeColor;
        }

        private void Button_start_urgent_Click(object sender, EventArgs e)
        {
            //TopMost = true;
            //MinimizeBox = false;

            //// Set browser panel dock style
            //chromeBrowser.Dock = DockStyle.Fill;

            //if (label8.Text == "")
            //{
            //    label8.Text = label9.Text;
            //}

            //if (label10.Text == "")
            //{
            //    label10.Text = label11.Text;
            //}

            //timer_blink.Stop();
            //label_status.Visible = true;
            //label_status.Text = "[Running]";
            //timer_domain.Start();
        }

        private void DataGridView_urgent_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView_urgent.CurrentCell == null || dataGridView_urgent.CurrentCell.Value == null)
            {
                return;
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView_urgent.SelectedRows)
                {
                    string domain = row.Cells[0].Value.ToString();
                    string brand;

                    if (String.IsNullOrEmpty(dataGridView_urgent.Rows[0].Cells[1].Value as String))
                    {
                        return;
                    }
                    else
                    {
                        brand = row.Cells[1].Value.ToString();
                    }

                    //currentIndex = dataGridView_domain.CurrentRow.Index;

                    try
                    {
                        // Load Browser
                        panel_browser_urgent.Controls.Add(chromeBrowser);
                        chromeBrowser.Dock = DockStyle.Fill;

                        chromeBrowser.Load(domain);
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }

                    
                }
            }
        }
    }
}