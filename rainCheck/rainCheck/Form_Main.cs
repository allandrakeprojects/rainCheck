﻿using CefSharp;
using CefSharp.WinForms;
using ChoETL;
using Ionic.Zip;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class Form_Main : Form
    {
        //MySqlConnection con = new MySqlConnection("server=mysql5018.site4now.net;user id=a3d1a6_check;password=admin12345;database=db_a3d1a6_check;persistsecurityinfo=True;SslMode=none");
        //MySqlConnection con = new MySqlConnection("server=localhost;user id=ssimecgp_ssiit;password=p0w3r@SSI;Database=ssimecgp_raincheck;persistsecurityinfo=True;SslMode=none;port=3306");


        public ChromiumWebBrowser chromeBrowser { get; private set; }

        public static string SetValueForTextBrandID = "";
        public static string SetValueForTextSearch = "";
        public static string SetValueForWebsiteType = "";

        static bool networkIsAvailable = false;

        static List<string> inaccessble_lists = new List<string>();

        string city_get;
        string isp_get;
        int currentIndex;

        //TimeSpan TimeLeft = new TimeSpan();
        //DateTime VoteTime = Properties.Settings.Default.voteTime;

        //MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;persistsecurityinfo=True;port=;database=raincheck;SslMode=none");

        public Form_Main()
        {
            InitializeComponent();

            var culture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            //string city, string country, string isp
            //Text = "rainCheck: " + city + ", " + country + " - " + isp;

            //city_get = city;
            //isp_get = isp;

            // Design
            //this.WindowState = FormWindowState.Maximized;

            //DataToGridView("SELECT CONCAT(b.brand_code, ' - ', REPEAT('*', length(d.domain_name)-5), RIGHT(d.domain_name, 5)) as 'Domain(s) List', d.domain_name, b.id, b.text_search from domains_test d inner join brands b ON d.brand_name=b.id WHERE d.status='A' order by FIELD(b.id, 4, 1, 2, 5, 3), d.domain_name");
            APIGetDomains();
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            try
            {
                using (var client = new WebClient())
                {
                    string auth = "r@inCh3ckd234b70";
                    string type = "running";
                    string request = "http://raincheck.ssitex.com/api/api.php";
                    string mac_id = GetMACAddress();

                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "auth", auth },
                        { "type", type },
                        { "mac_id", mac_id }
                    };

                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1023", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }

            //dataGridView_domains.ClearSelection();

            InitializeChromium();

            foreach (DataGridViewColumn column in dataGridView_domain.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // Hide column
            try
            {
                dataGridView_domain.Columns["domain_name"].Visible = false;
                dataGridView_domain.Columns["id"].Visible = false;
                dataGridView_domain.Columns["text_search"].Visible = false;
                dataGridView_domain.Columns["website_type"].Visible = false;
            }
            catch (Exception ex)
            {
                button_start.Enabled = false;
                label_domainscount.Visible = false;

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("Domain(s) List", typeof(string)));

                dt.Rows.Add("No data available in table");

                dataGridView_domain.DataSource = dt;

                dataGridView_domain.ClearSelection();

                dataGridView_domain.CellBorderStyle = DataGridViewCellBorderStyle.None;
                dataGridView_domain.DefaultCellStyle.SelectionBackColor = dataGridView_domain.DefaultCellStyle.BackColor;
                dataGridView_domain.DefaultCellStyle.SelectionForeColor = dataGridView_domain.DefaultCellStyle.ForeColor;

                //var st = new StackTrace(ex, true);
                //var frame = st.GetFrame(0);
                //var line = frame.GetFileLineNumber();
                //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1001", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }

            // Detect history file
            string path_history = Path.GetTempPath() + @"\raincheck_history.txt";
            if (File.Exists(path_history))
            {
                using (StreamReader sr = File.OpenText(path_history))
                {
                    int count = 0;
                    string s = String.Empty;
                    while ((s = sr.ReadLine()) != null)
                    {
                        count++;

                        if (count <= 12)
                        {
                            if (s != "")
                            {
                                dataGridView_history.Rows.Add(s);
                            }

                            dataGridView_history.ClearSelection();
                        }
                    }
                }
            }
            else
            {
                detectnohistoryyet = true;
                dataGridView_history.Rows.Add("No history yet");
                dataGridView_history.ClearSelection();

                dataGridView_history.CellBorderStyle = DataGridViewCellBorderStyle.None;
                dataGridView_history.DefaultCellStyle.SelectionBackColor = dataGridView_domain.DefaultCellStyle.BackColor;
                dataGridView_history.DefaultCellStyle.SelectionForeColor = dataGridView_domain.DefaultCellStyle.ForeColor;
            }

            // Hide loader
            pictureBox_loader.Visible = false;

            // Clear selection of datagridview
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

            // Get inaccessible list
            try
            {
                using (var client = new WebClient())
                {
                    string auth = "r@inCh3ckd234b70";
                    string type = "category";
                    string request = "http://raincheck.ssitex.com/api/api.php";
                    string mac_id = GetMACAddress();

                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "auth", auth },
                        { "type", type }
                    };

                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));
                    //string jsonRes = JsonConvert.SerializeObject(pagesource);
                    inaccessble_lists.Add(pagesource);
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1002", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }

            // Get timeout option to server
            try
            {
                using (var client = new WebClient())
                {
                    string auth = "r@inCh3ckd234b70";
                    string type = "timeout";
                    string request = "http://raincheck.ssitex.com/api/api.php";
                    string mac_id = GetMACAddress();

                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "auth", auth },
                        { "type", type }
                    };

                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));

                    string result = pagesource.Replace("\"", "");
                    label13.Text = result;
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1003", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
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

            // Getting the total count domain
            domain_total = dataGridView_domain.RowCount;
            label_totalcountofdomain.Text = domain_total.ToString();
            label_domainscount.Text = "Total: " + domain_total.ToString();

            // Getting time for
            //label_timeget.Text = label_timefor.Text;

            // Getting mac id
            label_macid.Text = GetMACAddress().ToLower();
            
            try
            {
                using (var client = new WebClient())
                {
                    string auth = "r@inCh3ckd234b70";
                    string type = "running";
                    string mac_id = GetMACAddress();
                    string run_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
                    string request = "http://raincheck.ssitex.com/api/api.php";

                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "auth", auth },
                        { "type", type },
                        { "mac_id", mac_id },
                        { "run_time", run_time }
                    };

                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1036", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }

            // URGENT PANEL
            try
            {
                if (dataGridView_urgent.Rows.Count == 0)
                {
                    button_start_urgent.Enabled = false;

                    dataGridView_urgent.Rows.Add("No data available in table");
                    dataGridView_urgent.ClearSelection();
                    dataGridView_urgent.CellBorderStyle = DataGridViewCellBorderStyle.None;
                    dataGridView_urgent.DefaultCellStyle.SelectionBackColor = dataGridView_urgent.DefaultCellStyle.BackColor;
                    dataGridView_urgent.DefaultCellStyle.SelectionForeColor = dataGridView_urgent.DefaultCellStyle.ForeColor;

                    new ToolTip().SetToolTip(label_help, "Click Domain button to Import New Set of Domain(s)");

                    dataGridView_urgent.Columns["brand_id"].Visible = false;
                    dataGridView_urgent.Columns["text_search"].Visible = false;
                    dataGridView_urgent.Columns["website_type"].Visible = false;

                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1004", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }
        }
        
        // Get MAC Address
        public static string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
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
                    chromeBrowser.Stop();

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

                //settings.IgnoreCertificateErrors = true;
                //settings.SetOffScreenRenderingBestPerformanceArgs();
                //settings.PackLoadingDisabled = true;
                settings.CefCommandLineArgs.Add("disable-plugins-discovery", "1");
                settings.CefCommandLineArgs.Add("no-proxy-server", "1");

                Cef.Initialize(settings);

                //chromeBrowser = new ChromiumWebBrowser(CustomLinks[0].ToString());
                //JsDialogHandler jss = new JsDialogHandler();
                //chromeBrowser.JsDialogHandler = jss;

                //textBox_domain.Text = "google.com";

                chromeBrowser = new ChromiumWebBrowser(textBox_domain.Text);

                panel_browser.Controls.Add(chromeBrowser);

                chromeBrowser.Dock = DockStyle.Fill;
                
                chromeBrowser.LoadingStateChanged += ChromiumWebBrowser_LoadingStateChangedAsync;
                chromeBrowser.AddressChanged += ChromiumWebBrowser_AddressChanged;
                //chromeBrowser.TitleChanged += ChromiumWebBrowser_TitleChanged;
                //chromeBrowser.StatusMessage += OnBrowserStatusMessage;
                chromeBrowser.LoadError += ChromiumWebBrowser_BrowserLoadError;
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1005", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }
        }
        
        private void ChromiumWebBrowser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            Invoke(new MethodInvoker(() => 
            {
                //if (panel_main.Visible == true)
                //{
                //    textBox_domain.Text = e.Address;
                //}
                //else if (panel_urgent.Visible == true)
                //{
                //    textBox_domain_urgent.Text = e.Address;
                //}

                textBox_domain.Text = e.Address;
            }));
        }

        private void ChromiumWebBrowser_BrowserLoadError(object sender, LoadErrorEventArgs e)
        {
            //MessageBox.Show("browserloaderror " + e.ErrorText);

            Invoke(new Action(() =>
            {
                label_inaccessible_error_message.Text = e.ErrorText;
            }));
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
        DateTime start_load_inaccessible;
        DateTime end_load_inaccessible;
        int fully_loaded = 0;
        int start_detect = 0;
        int ms_detect = 0;
        int testonemoretime = 0;
        int erroraborted_testonemoretime = 0;

        public async void ChromiumWebBrowser_LoadingStateChangedAsync(object sender, LoadingStateChangedEventArgs e)
        {
            // ----TIMER MAIN ENABLED----
            if (timer_domain.Enabled)
            {
                // --Loading--
                if (e.IsLoading)
                {
                    // Detect when stop loads
                    detectnotloading = 0;
                    timer_detectnotloading.Stop();

                    Invoke(new Action(() =>
                    {
                        panel_browser.Controls.Add(chromeBrowser);
                        start_detect++;
                        label_start_detect.Text = start_detect.ToString();
                    }));

                    //MessageBox.Show("loading asdasdasdasdsa1 " + label_domainhide.Text);

                    // Date preview
                    start_load = DateTime.Now.ToString("HH:mm:ss.fff");
                    start_load_inaccessible = DateTime.Now;

                    Invoke(new Action(() =>
                    {
                        timer_timeout.Start();
                        pictureBox_loader.Visible = true;
                        label_ifloadornot.Text = "1";
                        ms_detect++;
                        label_loadeddetect.Text = ms_detect.ToString();

                        // else loaded
                        elseloaded_i = 0;
                        timer_elseloaded.Stop();
                    }));
                    
                    if (ms_detect == 1)
                    {
                        int webBrowser_i = 0;
                        while (webBrowser_i <= 2)
                        {
                            //string header = "Accept: application/xml\r\nAccept-Language: en-US\r\n";
                            webBrowser_new.Navigate(label_domainhide.Text);
                            //webBrowser_new.Refresh(WebBrowserRefreshOption.Completely);
                            webBrowser_i++;
                        }
                    }
                    else
                    {
                        int webBrowser_i = 0;
                        while (webBrowser_i <= 2)
                        {
                            //string header = "Accept: application/xml\r\nAccept-Language: en-US\r\n";
                            webBrowser_new.Navigate(label_domainhide.Text);
                            //webBrowser_new.Refresh(WebBrowserRefreshOption.Completely);
                            webBrowser_i++;
                        }
                    }
                }

                // --Loaded--
                if (!e.IsLoading)
                {
                    // Detect when stop loads
                    detectnotloading = 0;
                    timer_detectnotloading.Start();

                    // Date preview
                    end_load_inaccessible = DateTime.Now;
                    end_load = DateTime.Now.ToString("HH:mm:ss.fff");

                    await Task.Run(async () =>
                    {
                        await Task.Delay(2000);
                    });
                    
                    Invoke(new Action(() =>
                    {
                        fully_loaded++;
                        label_fully_loaded.Text = fully_loaded.ToString();

                        string webtitle = webBrowser_new.DocumentTitle;
                        label_webtitle.Text = webtitle;
                        textBox_webtitle.Text = webtitle;
                    }));
                    
                    if (label_fully_loaded.Text == "1")
                    {
                        // Inaccessible Status
                        string result = "";
                        string search_replace = label_webtitle.Text;

                        string upper_search = search_replace.ToUpper().ToString();

                        StringBuilder sb = new StringBuilder(upper_search);
                        sb.Replace("-", "");
                        sb.Replace(".", "");
                        string final_search = Regex.Replace(sb.ToString(), " {2,}", " ");

                        var final_inaccessble_lists = inaccessble_lists.Select(m => m.ToUpper());

                        string[] words = final_search.Split(' ');
                        foreach (string word in words)
                        {
                            var match = final_inaccessble_lists.FirstOrDefault(stringToCheck => stringToCheck.Contains(word));

                            if (match != null)
                            {
                                result = "match";
                                break;
                            }
                            else
                            {
                                result = "no match";
                            }
                        }

                        if (result == "match")
                        {
                            // hijacked
                            if (label_webtitle.Text == "" && label_inaccessible_error_message.Text == "")
                            {
                                if (label_webtype.Text == "Landing Page" || label_webtype.Text == "Landing page")
                                {
                                    var html = "";
                                    try
                                    {
                                        html = new WebClient().DownloadString(textBox_domain.Text);
                                    }
                                    catch (Exception)
                                    {
                                        // Empty
                                    }

                                    if (html.Contains("landing_image"))
                                    {
                                        await Task.Run(async () =>
                                        {
                                            await Task.Delay(500);
                                        });

                                        DataToTextFileSuccess();

                                        Invoke(new Action(() =>
                                        {
                                            // For timeout
                                            i = 1;
                                            timer_timeout.Stop();

                                            pictureBox_loader.Visible = false;

                                            label_timeout.Text = "";
                                            label_hijacked.Text = "";
                                            label_inaccessible.Text = "";
                                            label_inaccessible_error_message.Text = "";

                                            if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                            {
                                                fully_loaded = 0;
                                                start_detect = 0;
                                                label_ifloadornot.Text = "0";
                                            }

                                            panel_new.Visible = false;
                                        }));
                                    }
                                    else
                                    {
                                        // test one more time
                                        Invoke(new Action(() =>
                                        {
                                            testonemoretime++;
                                            label_testonemoretime.Text = testonemoretime.ToString();
                                        }));

                                        if (testonemoretime == 1)
                                        {
                                            Invoke(new Action(() =>
                                            {
                                                int getCurrentIndex = Convert.ToInt32(label_currentindex.Text);
                                                dataGridView_domain.ClearSelection();

                                                //timer_domain.Start();

                                                // For timeout
                                                i = 1;
                                                timer_timeout.Start();

                                                fully_loaded = 0;
                                                start_detect = 0;

                                                dataGridView_domain.Rows[getCurrentIndex].Selected = true;
                                            }));
                                        }
                                        else
                                        {
                                            if (ms_detect == 1)
                                            {
                                                if (label_webtitle.Text == "Can’t reach this page" || label_webtitle.Text == "This site isn’t secure" || label_webtitle.Text == "无法访问此页面" || label_webtitle.Text == "此站点不安全")
                                                {
                                                    Invoke(new Action(() =>
                                                    {
                                                        panel_new.Visible = true;
                                                        panel_new.BringToFront();
                                                    }));
                                                }
                                            }

                                            DataToTextFileHijacked();

                                            Invoke(new Action(() =>
                                            {
                                                // For timeout
                                                i = 1;
                                                timer_timeout.Stop();

                                                pictureBox_loader.Visible = false;

                                                label_timeout.Text = "";
                                                label_hijacked.Text = "";
                                                label_inaccessible.Text = "";
                                                label_inaccessible_error_message.Text = "";

                                                if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                                {
                                                    fully_loaded = 0;
                                                    start_detect = 0;
                                                    label_ifloadornot.Text = "0";
                                                }

                                                testonemoretime = 0;
                                                panel_new.Visible = false;
                                            }));
                                        }
                                    }
                                }
                                else
                                {
                                    // test one more time
                                    Invoke(new Action(() =>
                                    {
                                        testonemoretime++;
                                        label_testonemoretime.Text = testonemoretime.ToString();
                                    }));

                                    if (testonemoretime == 1)
                                    {
                                        Invoke(new Action(() =>
                                        {
                                            int getCurrentIndex = Convert.ToInt32(label_currentindex.Text);
                                            dataGridView_domain.ClearSelection();

                                            //timer_domain.Start();

                                            // For timeout
                                            i = 1;
                                            timer_timeout.Start();

                                            fully_loaded = 0;
                                            start_detect = 0;

                                            dataGridView_domain.Rows[getCurrentIndex].Selected = true;
                                        }));
                                    }
                                    else
                                    {
                                        if (ms_detect == 1)
                                        {
                                            if (label_webtitle.Text == "Can’t reach this page" || label_webtitle.Text == "This site isn’t secure" || label_webtitle.Text == "无法访问此页面" || label_webtitle.Text == "此站点不安全")
                                            {
                                                Invoke(new Action(() =>
                                                {
                                                    panel_new.Visible = true;
                                                    panel_new.BringToFront();
                                                }));
                                            }
                                        }

                                        DataToTextFileHijacked();

                                        Invoke(new Action(() =>
                                        {
                                            // For timeout
                                            i = 1;
                                            timer_timeout.Stop();

                                            pictureBox_loader.Visible = false;

                                            label_timeout.Text = "";
                                            label_hijacked.Text = "";
                                            label_inaccessible.Text = "";
                                            label_inaccessible_error_message.Text = "";

                                            if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                            {
                                                fully_loaded = 0;
                                                start_detect = 0;
                                                label_ifloadornot.Text = "0";
                                            }

                                            testonemoretime = 0;
                                            panel_new.Visible = false;
                                        }));
                                    }
                                }
                            }
                            // inaccessible
                            else
                            {
                                // error aborted test one more time
                                if (label_inaccessible_error_message.Text == "ERR_ABORTED" || label_inaccessible_error_message.Text == "ERR_NETWORK_CHANGED" || label_inaccessible_error_message.Text == "ERR_INTERNET_DISCONNECTED" || label_inaccessible_error_message.Text == "Navigation Canceled" || label_inaccessible_error_message.Text == "导航已取消")
                                {
                                    // test one more time
                                    Invoke(new Action(() =>
                                    {
                                        erroraborted_testonemoretime++;
                                        label_erroraborted.Text = erroraborted_testonemoretime.ToString();
                                    }));

                                    if (erroraborted_testonemoretime == 1)
                                    {
                                        Invoke(new Action(() =>
                                        {
                                            int getCurrentIndex = Convert.ToInt32(label_currentindex.Text);
                                            dataGridView_domain.ClearSelection();

                                            //timer_domain.Start();

                                            // For timeout
                                            i = 1;
                                            timer_timeout.Start();

                                            fully_loaded = 0;
                                            start_detect = 0;

                                            dataGridView_domain.Rows[getCurrentIndex].Selected = true;

                                            label_inaccessible_error_message.Text = "";
                                        }));
                                    }
                                    else
                                    {
                                        if (label_webtitle.Text == "Can’t reach this page" || label_webtitle.Text == "This site isn’t secure" || label_webtitle.Text == "无法访问此页面" || label_webtitle.Text == "此站点不安全")
                                        {
                                            Invoke(new Action(() =>
                                            {
                                                panel_new.Visible = true;
                                                panel_new.BringToFront();
                                            }));
                                        }

                                        if (ms_detect == 1)
                                        {
                                            if (label_webtitle.Text == "Can’t reach this page" || label_webtitle.Text == "This site isn’t secure" || label_webtitle.Text == "无法访问此页面" || label_webtitle.Text == "此站点不安全")
                                            {
                                                Invoke(new Action(() =>
                                                {
                                                    panel_new.Visible = true;
                                                    panel_new.BringToFront();
                                                }));
                                            }
                                        }

                                        Invoke(new Action(async () =>
                                        {
                                            label_inaccessible.Text = "inaccessible";

                                            TimeSpan span = end_load_inaccessible - start_load_inaccessible;
                                            int ms = (int)span.TotalMilliseconds;

                                            // for fast load
                                            if (ms < 500)
                                            {
                                                panel_new.Visible = true;
                                                panel_new.BringToFront();

                                                int webBrowser_i = 0;
                                                while (webBrowser_i <= 2)
                                                {
                                                    webBrowser_new.Navigate(label_domainhide.Text);
                                                    webBrowser_i++;
                                                }

                                                await Task.Run(async () =>
                                                {
                                                    await Task.Delay(500);
                                                });

                                                string datetime = label11.Text;
                                                string datetime_folder = label9.Text;
                                                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                                string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

                                                string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

                                                DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                                Rectangle bounds = Bounds;
                                                using (Bitmap bitmap = new Bitmap(bounds.Width - 267, bounds.Height - 202))
                                                {
                                                    using (Graphics g = Graphics.FromImage(bitmap))
                                                    {
                                                        g.CopyFromScreen(new Point(bounds.Left + 226, bounds.Top + 159), Point.Empty, bounds.Size);
                                                    }

                                                    Bitmap resized = new Bitmap(bitmap, new Size(bitmap.Width / 2, bitmap.Height / 2));
                                                    resized.Save(path + "_" + label_macid.Text + "_n_" + label_domainhide.Text + ".jpeg", ImageFormat.Jpeg);
                                                }

                                                DataToTextFileInaccessible();

                                                // For timeout
                                                i = 1;
                                                timer_timeout.Stop();

                                                pictureBox_loader.Visible = false;

                                                label_timeout.Text = "";
                                                label_hijacked.Text = "";
                                                label_inaccessible.Text = "";
                                                label_inaccessible_error_message.Text = "";
                                                erroraborted_testonemoretime = 0;

                                                if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                                {
                                                    fully_loaded = 0;
                                                    start_detect = 0;
                                                    label_ifloadornot.Text = "0";
                                                }

                                                Invoke(new Action(() =>
                                                {
                                                    panel_new.Visible = false;
                                                }));
                                            }
                                            else
                                            {
                                                await Task.Run(async () =>
                                                {
                                                    await Task.Delay(500);
                                                });

                                                string datetime = label11.Text;
                                                string datetime_folder = label9.Text;
                                                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                                string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

                                                string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

                                                DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                                Rectangle bounds = Bounds;
                                                using (Bitmap bitmap = new Bitmap(bounds.Width - 267, bounds.Height - 202))
                                                {
                                                    using (Graphics g = Graphics.FromImage(bitmap))
                                                    {
                                                        g.CopyFromScreen(new Point(bounds.Left + 226, bounds.Top + 159), Point.Empty, bounds.Size);
                                                    }

                                                    Bitmap resized = new Bitmap(bitmap, new Size(bitmap.Width / 2, bitmap.Height / 2));
                                                    resized.Save(path + "_" + label_macid.Text + "_n_" + label_domainhide.Text + ".jpeg", ImageFormat.Jpeg);
                                                }

                                                DataToTextFileInaccessible();

                                                // For timeout
                                                i = 1;
                                                timer_timeout.Stop();

                                                pictureBox_loader.Visible = false;

                                                label_timeout.Text = "";
                                                label_hijacked.Text = "";
                                                label_inaccessible.Text = "";
                                                label_inaccessible_error_message.Text = "";
                                                erroraborted_testonemoretime = 0;

                                                if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                                {
                                                    fully_loaded = 0;
                                                    start_detect = 0;
                                                    label_ifloadornot.Text = "0";
                                                }

                                                Invoke(new Action(() =>
                                                {
                                                    panel_new.Visible = false;
                                                }));
                                            }
                                        }));
                                    }
                                }
                                else
                                {
                                    if (label_webtitle.Text == "Can’t reach this page" || label_webtitle.Text == "This site isn’t secure" || label_webtitle.Text == "无法访问此页面" || label_webtitle.Text == "此站点不安全")
                                    {
                                        Invoke(new Action(() =>
                                        {
                                            panel_new.Visible = true;
                                            panel_new.BringToFront();
                                        }));
                                    }

                                    if (ms_detect == 1)
                                    {
                                        if (label_webtitle.Text == "Can’t reach this page" || label_webtitle.Text == "This site isn’t secure" || label_webtitle.Text == "无法访问此页面" || label_webtitle.Text == "此站点不安全")
                                        {
                                            Invoke(new Action(() =>
                                            {
                                                panel_new.Visible = true;
                                                panel_new.BringToFront();
                                            }));
                                        }
                                    }

                                    Invoke(new Action(async () =>
                                    {
                                        label_inaccessible.Text = "inaccessible";

                                        TimeSpan span = end_load_inaccessible - start_load_inaccessible;
                                        int ms = (int)span.TotalMilliseconds;

                                        // for fast load
                                        if (ms < 500)
                                        {
                                            panel_new.Visible = true;
                                            panel_new.BringToFront();

                                            int webBrowser_i = 0;
                                            while (webBrowser_i <= 2)
                                            {
                                                webBrowser_new.Navigate(label_domainhide.Text);
                                                webBrowser_i++;
                                            }

                                            await Task.Run(async () =>
                                            {
                                                await Task.Delay(500);
                                            });

                                            string datetime = label11.Text;
                                            string datetime_folder = label9.Text;
                                            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                            string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

                                            string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

                                            DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                            Rectangle bounds = Bounds;
                                            using (Bitmap bitmap = new Bitmap(bounds.Width - 267, bounds.Height - 202))
                                            {
                                                using (Graphics g = Graphics.FromImage(bitmap))
                                                {
                                                    g.CopyFromScreen(new Point(bounds.Left + 226, bounds.Top + 159), Point.Empty, bounds.Size);
                                                }

                                                Bitmap resized = new Bitmap(bitmap, new Size(bitmap.Width / 2, bitmap.Height / 2));
                                                resized.Save(path + "_" + label_macid.Text + "_n_" + label_domainhide.Text + ".jpeg", ImageFormat.Jpeg);
                                            }

                                            DataToTextFileInaccessible();

                                            // For timeout
                                            i = 1;
                                            timer_timeout.Stop();

                                            pictureBox_loader.Visible = false;

                                            label_timeout.Text = "";
                                            label_hijacked.Text = "";
                                            label_inaccessible.Text = "";
                                            label_inaccessible_error_message.Text = "";
                                            erroraborted_testonemoretime = 0;

                                            if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                            {
                                                fully_loaded = 0;
                                                start_detect = 0;
                                                label_ifloadornot.Text = "0";
                                            }

                                            Invoke(new Action(() =>
                                            {
                                                panel_new.Visible = false;
                                            }));
                                        }
                                        else
                                        {
                                            await Task.Run(async () =>
                                            {
                                                await Task.Delay(500);
                                            });

                                            string datetime = label11.Text;
                                            string datetime_folder = label9.Text;
                                            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                            string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

                                            string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

                                            DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                            Rectangle bounds = Bounds;
                                            using (Bitmap bitmap = new Bitmap(bounds.Width - 267, bounds.Height - 202))
                                            {
                                                using (Graphics g = Graphics.FromImage(bitmap))
                                                {
                                                    g.CopyFromScreen(new Point(bounds.Left + 226, bounds.Top + 159), Point.Empty, bounds.Size);
                                                }

                                                Bitmap resized = new Bitmap(bitmap, new Size(bitmap.Width / 2, bitmap.Height / 2));
                                                resized.Save(path + "_" + label_macid.Text + "_n_" + label_domainhide.Text + ".jpeg", ImageFormat.Jpeg);
                                            }

                                            DataToTextFileInaccessible();

                                            // For timeout
                                            i = 1;
                                            timer_timeout.Stop();

                                            pictureBox_loader.Visible = false;

                                            label_timeout.Text = "";
                                            label_hijacked.Text = "";
                                            label_inaccessible.Text = "";
                                            label_inaccessible_error_message.Text = "";
                                            erroraborted_testonemoretime = 0;

                                            if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                            {
                                                fully_loaded = 0;
                                                start_detect = 0;
                                                label_ifloadornot.Text = "0";
                                            }

                                            Invoke(new Action(() =>
                                            {
                                                panel_new.Visible = false;
                                            }));
                                        }
                                    }));
                                }
                            }
                        }
                        // Hijacked Status
                        else
                        {
                            // inaccessible
                            if (label_webtitle.Text == label_domainhide.Text)
                            {
                                await Task.Run(async () =>
                                {
                                    await Task.Delay(500);
                                });

                                string datetime = label11.Text;
                                string datetime_folder = label9.Text;
                                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

                                string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

                                DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                Rectangle bounds = Bounds;
                                using (Bitmap bitmap = new Bitmap(bounds.Width - 267, bounds.Height - 202))
                                {
                                    using (Graphics g = Graphics.FromImage(bitmap))
                                    {
                                        g.CopyFromScreen(new Point(bounds.Left + 226, bounds.Top + 159), Point.Empty, bounds.Size);
                                    }

                                    Bitmap resized = new Bitmap(bitmap, new Size(bitmap.Width / 2, bitmap.Height / 2));
                                    resized.Save(path + "_" + label_macid.Text + "_n_" + label_domainhide.Text + ".jpeg", ImageFormat.Jpeg);
                                }

                                DataToTextFileInaccessible();
                                
                                Invoke(new Action(() =>
                                {
                                    // For timeout
                                    i = 1;
                                    timer_timeout.Stop();

                                    pictureBox_loader.Visible = false;

                                    label_timeout.Text = "";
                                    label_hijacked.Text = "";
                                    label_inaccessible.Text = "";
                                    label_inaccessible_error_message.Text = "";

                                    if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                    {
                                        fully_loaded = 0;
                                        start_detect = 0;
                                        label_ifloadornot.Text = "0";
                                    }

                                    panel_new.Visible = false;
                                }));
                            }
                            // success
                            else if (label_webtitle.Text.Contains("Bing"))
                            {
                                await Task.Run(async () =>
                                {
                                    await Task.Delay(500);
                                });

                                DataToTextFileSuccess();

                                Invoke(new Action(() =>
                                {
                                    // For timeout
                                    i = 1;
                                    timer_timeout.Stop();

                                    pictureBox_loader.Visible = false;

                                    label_timeout.Text = "";
                                    label_hijacked.Text = "";
                                    label_inaccessible.Text = "";
                                    label_inaccessible_error_message.Text = "";

                                    if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                    {
                                        fully_loaded = 0;
                                        start_detect = 0;
                                        label_ifloadornot.Text = "0";
                                    }

                                    panel_new.Visible = false;
                                }));
                            }
                            // hijacked
                            else
                            {
                                await Task.Run(async () =>
                                {
                                    await Task.Delay(500);
                                });

                                string strValue = label_text_search.Text;
                                string[] strArray = strValue.Split(',');

                                foreach (string obj in strArray)
                                {
                                    bool contains = label_webtitle.Text.Contains(obj);

                                    if (contains == true)
                                    {
                                        Invoke(new Action(() =>
                                        {
                                            label_hijacked.Text = "";
                                        }));

                                        break;
                                    }
                                    else if (!contains)
                                    {
                                        //MessageBox.Show(label_text_search.Text + " asdasdasd " + label_domaintitle.Text + "\nnot safe " + label_domainhide.Text + "\n\n" + textBox_domain.Text);

                                        Invoke(new Action(() =>
                                        {
                                            label_hijacked.Text = "hijacked";
                                        }));
                                    }
                                }

                                // Send data to text file
                                if (label_hijacked.Text == "hijacked")
                                {
                                    DataToTextFileHijacked();
                                }
                                // Timeout Status
                                else if (label_timeout.Text == "timeout")
                                {
                                    DataToTextFileTimeout();
                                }
                                // Successful Status
                                else
                                {
                                    DataToTextFileSuccess();
                                }

                                Invoke(new Action(() =>
                                {
                                    // For timeout
                                    i = 1;
                                    timer_timeout.Stop();

                                    pictureBox_loader.Visible = false;

                                    label_timeout.Text = "";
                                    label_hijacked.Text = "";
                                    label_inaccessible.Text = "";
                                    label_inaccessible_error_message.Text = "";

                                    if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                    {
                                        fully_loaded = 0;
                                        start_detect = 0;
                                        label_ifloadornot.Text = "0";
                                    }

                                    panel_new.Visible = false;
                                }));
                            }
                        }
                    }
                    else
                    {
                        Invoke(new Action(() =>
                        {
                            timer_elseloaded.Start();
                        }));
                    }
                }
            }

            // ----BUTTON GO WAS CLICKED----
            else if (buttonGoWasClicked == true)
            {
                // --Loading--
                if (e.IsLoading)
                {
                    // Detect when stop loads
                    //detectnotloading = 0;
                    //timer_detectnotloading.Stop();

                    Invoke(new Action(() =>
                    {
                        panel_browser.Controls.Add(chromeBrowser);
                        start_detect++;
                        label_start_detect.Text = start_detect.ToString();
                    }));

                    //MessageBox.Show("loading asdasdasdasdsa1 " + label_domainhide.Text);

                    // Date preview
                    start_load = DateTime.Now.ToString("HH:mm:ss.fff");
                    start_load_inaccessible = DateTime.Now;

                    Invoke(new Action(() =>
                    {
                        timer_timeout.Start();
                        pictureBox_loader.Visible = true;
                        ms_detect++;
                        label_loadeddetect.Text = ms_detect.ToString();

                        // else loaded
                        elseloaded_i = 0;
                        timer_elseloaded.Stop();
                    }));

                    int webBrowser_i = 0;
                    while (webBrowser_i <= 2)
                    {
                        webBrowser_new.Navigate(label_domainhide.Text);
                        webBrowser_i++;
                    }
                }

                // --Loaded--
                if (!e.IsLoading)
                {
                    //MessageBox.Show(label_domainhide.Text);
                    // Detect when stop loads
                    //detectnotloading = 0;
                    //timer_detectnotloading.Start();

                    // Date preview
                    end_load_inaccessible = DateTime.Now;
                    end_load = DateTime.Now.ToString("HH:mm:ss.fff");

                    await Task.Run(async () =>
                    {
                        await Task.Delay(2000);
                    });

                    Invoke(new Action(() =>
                    {
                        fully_loaded++;
                        label_fully_loaded.Text = fully_loaded.ToString();

                        string webtitle = webBrowser_new.DocumentTitle;
                        label_webtitle.Text = webtitle;
                        textBox_webtitle.Text = webtitle;
                    }));

                    if (label_fully_loaded.Text == "1")
                    {
                        // Inaccessible Status
                        string result = "";
                        string search_replace = label_webtitle.Text;

                        string upper_search = search_replace.ToUpper().ToString();

                        StringBuilder sb = new StringBuilder(upper_search);
                        sb.Replace("-", "");
                        sb.Replace(".", "");
                        string final_search = Regex.Replace(sb.ToString(), " {2,}", " ");

                        var final_inaccessble_lists = inaccessble_lists.Select(m => m.ToUpper());

                        string[] words = final_search.Split(' ');
                        foreach (string word in words)
                        {
                            var match = final_inaccessble_lists.FirstOrDefault(stringToCheck => stringToCheck.Contains(word));

                            if (match != null)
                            {
                                result = "match";
                                break;
                            }
                            else
                            {
                                result = "no match";
                            }
                        }

                        if (result == "match")
                        {
                            // hijacked
                            if (label_webtitle.Text == "" && label_inaccessible_error_message.Text == "")
                            {
                                if (label_webtype_urgent.Text == "Landing Page" || label_webtype_urgent.Text == "Landing page")
                                {
                                    var html = "";
                                    try
                                    {
                                        html = new WebClient().DownloadString(textBox_domain.Text);
                                    }
                                    catch (Exception)
                                    {
                                        // Empty
                                    }

                                    if (html.Contains("landing_image"))
                                    {
                                        await Task.Run(async () =>
                                        {
                                            await Task.Delay(500);
                                        });

                                        DataToTextFileSuccess();

                                        Invoke(new Action(() =>
                                        {
                                            // For timeout
                                            i = 1;
                                            timer_timeout.Stop();

                                            pictureBox_loader.Visible = false;

                                            label_timeout.Text = "";
                                            label_hijacked.Text = "";
                                            label_inaccessible.Text = "";
                                            label_inaccessible_error_message.Text = "";

                                            TopMost = false;
                                            buttonGoWasClicked = false;

                                            if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                            {
                                                fully_loaded = 0;
                                                start_detect = 0;
                                            }

                                            panel_new.Visible = false;
                                        }));
                                    }
                                    else
                                    {
                                        // test one more time
                                        Invoke(new Action(() =>
                                        {
                                            testonemoretime++;
                                            label_testonemoretime.Text = testonemoretime.ToString();
                                        }));

                                        if (testonemoretime == 1)
                                        {
                                            Invoke(new Action(() =>
                                            {
                                                int getCurrentIndex = Convert.ToInt32(label_currentindex.Text);
                                                dataGridView_domain.ClearSelection();

                                                //timer_domain.Start();

                                                // For timeout
                                                i = 1;
                                                timer_timeout.Start();

                                                fully_loaded = 0;
                                                start_detect = 0;

                                                dataGridView_domain.Rows[getCurrentIndex].Selected = true;
                                            }));
                                        }
                                        else
                                        {
                                            if (ms_detect == 1)
                                            {
                                                if (label_webtitle.Text == "Can’t reach this page" || label_webtitle.Text == "This site isn’t secure" || label_webtitle.Text == "无法访问此页面" || label_webtitle.Text == "此站点不安全")
                                                {
                                                    Invoke(new Action(() =>
                                                    {
                                                        panel_new.Visible = true;
                                                        panel_new.BringToFront();
                                                    }));
                                                }
                                            }

                                            DataToTextFileHijacked();

                                            Invoke(new Action(() =>
                                            {
                                                // For timeout
                                                i = 1;
                                                timer_timeout.Stop();

                                                pictureBox_loader.Visible = false;

                                                label_timeout.Text = "";
                                                label_hijacked.Text = "";
                                                label_inaccessible.Text = "";
                                                label_inaccessible_error_message.Text = "";

                                                TopMost = false;
                                                buttonGoWasClicked = false;

                                                if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                                {
                                                    fully_loaded = 0;
                                                    start_detect = 0;
                                                }

                                                testonemoretime = 0;
                                                panel_new.Visible = false;
                                            }));
                                        }
                                    }
                                }
                                else
                                {
                                    // test one more time
                                    Invoke(new Action(() =>
                                    {
                                        testonemoretime++;
                                        label_testonemoretime.Text = testonemoretime.ToString();
                                    }));

                                    if (testonemoretime == 1)
                                    {
                                        Invoke(new Action(() =>
                                        {
                                            // For timeout
                                            i = 1;
                                            timer_timeout.Start();

                                            fully_loaded = 0;
                                            start_detect = 0;

                                            button_go.PerformClick();
                                        }));
                                    }
                                    else
                                    {
                                        if (ms_detect == 1)
                                        {
                                            if (label_webtitle.Text == "Can’t reach this page" || label_webtitle.Text == "This site isn’t secure" || label_webtitle.Text == "无法访问此页面" || label_webtitle.Text == "此站点不安全")
                                            {
                                                Invoke(new Action(() =>
                                                {
                                                    panel_new.Visible = true;
                                                    panel_new.BringToFront();
                                                }));
                                            }
                                        }

                                        DataToTextFileHijacked();

                                        Invoke(new Action(() =>
                                        {
                                            // For timeout
                                            i = 1;
                                            timer_timeout.Stop();

                                            pictureBox_loader.Visible = false;

                                            label_timeout.Text = "";
                                            label_hijacked.Text = "";
                                            label_inaccessible.Text = "";
                                            label_inaccessible_error_message.Text = "";

                                            TopMost = false;
                                            buttonGoWasClicked = false;

                                            if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                            {
                                                fully_loaded = 0;
                                                start_detect = 0;
                                            }

                                            testonemoretime = 0;
                                            panel_new.Visible = false;
                                        }));
                                    }
                                }
                            }
                            // inaccessible
                            else
                            {
                                // error aborted test one more time
                                if (label_inaccessible_error_message.Text == "ERR_ABORTED" || label_inaccessible_error_message.Text == "ERR_NETWORK_CHANGED" || label_inaccessible_error_message.Text == "ERR_INTERNET_DISCONNECTED")
                                {
                                    // test one more time
                                    Invoke(new Action(() =>
                                    {
                                        erroraborted_testonemoretime++;
                                        label_erroraborted.Text = erroraborted_testonemoretime.ToString();
                                    }));

                                    if (erroraborted_testonemoretime == 1)
                                    {
                                        Invoke(new Action(() =>
                                        {
                                            // For timeout
                                            i = 1;
                                            timer_timeout.Start();

                                            fully_loaded = 0;
                                            start_detect = 0;

                                            button_go.PerformClick();

                                            label_inaccessible_error_message.Text = "";
                                        }));
                                    }
                                    else
                                    {
                                        if (label_webtitle.Text == "Can’t reach this page" || label_webtitle.Text == "This site isn’t secure" || label_webtitle.Text == "无法访问此页面" || label_webtitle.Text == "此站点不安全")
                                        {
                                            Invoke(new Action(() =>
                                            {
                                                panel_new.Visible = true;
                                                panel_new.BringToFront();
                                            }));
                                        }

                                        if (ms_detect == 1)
                                        {
                                            if (label_webtitle.Text == "Can’t reach this page" || label_webtitle.Text == "This site isn’t secure" || label_webtitle.Text == "无法访问此页面" || label_webtitle.Text == "此站点不安全")
                                            {
                                                Invoke(new Action(() =>
                                                {
                                                    panel_new.Visible = true;
                                                    panel_new.BringToFront();
                                                }));
                                            }
                                        }

                                        Invoke(new Action(async () =>
                                        {
                                            label_inaccessible.Text = "inaccessible";

                                            TimeSpan span = end_load_inaccessible - start_load_inaccessible;
                                            int ms = (int)span.TotalMilliseconds;

                                            // for fast load
                                            if (ms < 500)
                                            {
                                                panel_new.Visible = true;
                                                panel_new.BringToFront();

                                                int webBrowser_i = 0;
                                                while (webBrowser_i <= 2)
                                                {
                                                    webBrowser_new.Navigate(label_domainhide.Text);
                                                    webBrowser_i++;
                                                }

                                                await Task.Run(async () =>
                                                {
                                                    await Task.Delay(500);
                                                });

                                                string datetime = label11.Text;
                                                string datetime_folder = label9.Text;
                                                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                                string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

                                                string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

                                                DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                                Rectangle bounds = Bounds;
                                                using (Bitmap bitmap = new Bitmap(bounds.Width - 267, bounds.Height - 202))
                                                {
                                                    using (Graphics g = Graphics.FromImage(bitmap))
                                                    {
                                                        g.CopyFromScreen(new Point(bounds.Left + 226, bounds.Top + 159), Point.Empty, bounds.Size);
                                                    }

                                                    Bitmap resized = new Bitmap(bitmap, new Size(bitmap.Width / 2, bitmap.Height / 2));
                                                    resized.Save(path + "_" + label_macid.Text + "_n_" + label_domainhide.Text + ".jpeg", ImageFormat.Jpeg);
                                                }

                                                DataToTextFileInaccessible();

                                                // For timeout
                                                i = 1;
                                                timer_timeout.Stop();

                                                pictureBox_loader.Visible = false;

                                                label_timeout.Text = "";
                                                label_hijacked.Text = "";
                                                label_inaccessible.Text = "";
                                                label_inaccessible_error_message.Text = "";
                                                erroraborted_testonemoretime = 0;

                                                TopMost = false;
                                                buttonGoWasClicked = false;

                                                if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                                {
                                                    fully_loaded = 0;
                                                    start_detect = 0;
                                                }

                                                Invoke(new Action(() =>
                                                {
                                                    panel_new.Visible = false;
                                                }));
                                            }
                                            else
                                            {
                                                await Task.Run(async () =>
                                                {
                                                    await Task.Delay(500);
                                                });

                                                string datetime = label11.Text;
                                                string datetime_folder = label9.Text;
                                                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                                string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

                                                string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

                                                DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                                Rectangle bounds = Bounds;
                                                using (Bitmap bitmap = new Bitmap(bounds.Width - 267, bounds.Height - 202))
                                                {
                                                    using (Graphics g = Graphics.FromImage(bitmap))
                                                    {
                                                        g.CopyFromScreen(new Point(bounds.Left + 226, bounds.Top + 159), Point.Empty, bounds.Size);
                                                    }

                                                    Bitmap resized = new Bitmap(bitmap, new Size(bitmap.Width / 2, bitmap.Height / 2));
                                                    resized.Save(path + "_" + label_macid.Text + "_n_" + label_domainhide.Text + ".jpeg", ImageFormat.Jpeg);
                                                }

                                                DataToTextFileInaccessible();

                                                // For timeout
                                                i = 1;
                                                timer_timeout.Stop();

                                                pictureBox_loader.Visible = false;

                                                label_timeout.Text = "";
                                                label_hijacked.Text = "";
                                                label_inaccessible.Text = "";
                                                label_inaccessible_error_message.Text = "";
                                                erroraborted_testonemoretime = 0;
                                                
                                                TopMost = false;
                                                buttonGoWasClicked = false;

                                                if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                                {
                                                    fully_loaded = 0;
                                                    start_detect = 0;
                                                }

                                                Invoke(new Action(() =>
                                                {
                                                    panel_new.Visible = false;
                                                }));
                                            }
                                        }));
                                    }
                                }
                                else
                                {
                                    if (label_webtitle.Text == "Can’t reach this page" || label_webtitle.Text == "This site isn’t secure" || label_webtitle.Text == "无法访问此页面" || label_webtitle.Text == "此站点不安全")
                                    {
                                        Invoke(new Action(() =>
                                        {
                                            panel_new.Visible = true;
                                            panel_new.BringToFront();
                                        }));
                                    }

                                    if (ms_detect == 1)
                                    {
                                        if (label_webtitle.Text == "Can’t reach this page" || label_webtitle.Text == "This site isn’t secure" || label_webtitle.Text == "无法访问此页面" || label_webtitle.Text == "此站点不安全")
                                        {
                                            Invoke(new Action(() =>
                                            {
                                                panel_new.Visible = true;
                                                panel_new.BringToFront();
                                            }));
                                        }
                                    }

                                    Invoke(new Action(async () =>
                                    {
                                        label_inaccessible.Text = "inaccessible";

                                        TimeSpan span = end_load_inaccessible - start_load_inaccessible;
                                        int ms = (int)span.TotalMilliseconds;

                                        // for fast load
                                        if (ms < 500)
                                        {
                                            panel_new.Visible = true;
                                            panel_new.BringToFront();

                                            int webBrowser_i = 0;
                                            while (webBrowser_i <= 2)
                                            {
                                                webBrowser_new.Navigate(label_domainhide.Text);
                                                webBrowser_i++;
                                            }

                                            await Task.Run(async () =>
                                            {
                                                await Task.Delay(500);
                                            });

                                            string datetime = label11.Text;
                                            string datetime_folder = label9.Text;
                                            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                            string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

                                            string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

                                            DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                            Rectangle bounds = Bounds;
                                            using (Bitmap bitmap = new Bitmap(bounds.Width - 267, bounds.Height - 202))
                                            {
                                                using (Graphics g = Graphics.FromImage(bitmap))
                                                {
                                                    g.CopyFromScreen(new Point(bounds.Left + 226, bounds.Top + 159), Point.Empty, bounds.Size);
                                                }

                                                Bitmap resized = new Bitmap(bitmap, new Size(bitmap.Width / 2, bitmap.Height / 2));
                                                resized.Save(path + "_" + label_macid.Text + "_n_" + label_domainhide.Text + ".jpeg", ImageFormat.Jpeg);
                                            }

                                            DataToTextFileInaccessible();

                                            // For timeout
                                            i = 1;
                                            timer_timeout.Stop();

                                            pictureBox_loader.Visible = false;

                                            label_timeout.Text = "";
                                            label_hijacked.Text = "";
                                            label_inaccessible.Text = "";
                                            label_inaccessible_error_message.Text = "";
                                            erroraborted_testonemoretime = 0;
                                            
                                            TopMost = false;
                                            buttonGoWasClicked = false;

                                            if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                            {
                                                fully_loaded = 0;
                                                start_detect = 0;
                                            }

                                            Invoke(new Action(() =>
                                            {
                                                panel_new.Visible = false;
                                            }));
                                        }
                                        else
                                        {
                                            await Task.Run(async () =>
                                            {
                                                await Task.Delay(500);
                                            });

                                            string datetime = label11.Text;
                                            string datetime_folder = label9.Text;
                                            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                            string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

                                            string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

                                            DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                            Rectangle bounds = Bounds;
                                            using (Bitmap bitmap = new Bitmap(bounds.Width - 267, bounds.Height - 202))
                                            {
                                                using (Graphics g = Graphics.FromImage(bitmap))
                                                {
                                                    g.CopyFromScreen(new Point(bounds.Left + 226, bounds.Top + 159), Point.Empty, bounds.Size);
                                                }

                                                Bitmap resized = new Bitmap(bitmap, new Size(bitmap.Width / 2, bitmap.Height / 2));
                                                resized.Save(path + "_" + label_macid.Text + "_n_" + label_domainhide.Text + ".jpeg", ImageFormat.Jpeg);
                                            }

                                            DataToTextFileInaccessible();

                                            // For timeout
                                            i = 1;
                                            timer_timeout.Stop();

                                            pictureBox_loader.Visible = false;

                                            label_timeout.Text = "";
                                            label_hijacked.Text = "";
                                            label_inaccessible.Text = "";
                                            label_inaccessible_error_message.Text = "";
                                            erroraborted_testonemoretime = 0;
                                            
                                            TopMost = false;
                                            buttonGoWasClicked = false;

                                            if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                            {
                                                fully_loaded = 0;
                                                start_detect = 0;
                                            }

                                            Invoke(new Action(() =>
                                            {
                                                panel_new.Visible = false;
                                            }));
                                        }
                                    }));
                                }
                            }
                        }
                        // Hijacked Status
                        else
                        {
                            // inaccessible
                            if (label_webtitle.Text == label_domainhide.Text)
                            {
                                await Task.Run(async () =>
                                {
                                    await Task.Delay(500);
                                });

                                string datetime = label11.Text;
                                string datetime_folder = label9.Text;
                                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

                                string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

                                DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                Rectangle bounds = Bounds;
                                using (Bitmap bitmap = new Bitmap(bounds.Width - 267, bounds.Height - 202))
                                {
                                    using (Graphics g = Graphics.FromImage(bitmap))
                                    {
                                        g.CopyFromScreen(new Point(bounds.Left + 226, bounds.Top + 159), Point.Empty, bounds.Size);
                                    }

                                    Bitmap resized = new Bitmap(bitmap, new Size(bitmap.Width / 2, bitmap.Height / 2));
                                    resized.Save(path + "_" + label_macid.Text + "_n_" + label_domainhide.Text + ".jpeg", ImageFormat.Jpeg);

                                }

                                DataToTextFileInaccessible();

                                Invoke(new Action(() =>
                                {
                                    // For timeout
                                    i = 1;
                                    timer_timeout.Stop();

                                    pictureBox_loader.Visible = false;

                                    label_timeout.Text = "";
                                    label_hijacked.Text = "";
                                    label_inaccessible.Text = "";
                                    label_inaccessible_error_message.Text = "";

                                    TopMost = false;
                                    buttonGoWasClicked = false;

                                    if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                    {
                                        fully_loaded = 0;
                                        start_detect = 0;
                                    }

                                    panel_new.Visible = false;
                                }));
                            }
                            // success
                            else if (label_webtitle.Text.Contains("Bing"))
                            {
                                await Task.Run(async () =>
                                {
                                    await Task.Delay(500);
                                });

                                DataToTextFileSuccess();

                                Invoke(new Action(() =>
                                {
                                    // For timeout
                                    i = 1;
                                    timer_timeout.Stop();

                                    pictureBox_loader.Visible = false;

                                    label_timeout.Text = "";
                                    label_hijacked.Text = "";
                                    label_inaccessible.Text = "";
                                    label_inaccessible_error_message.Text = "";
                                    
                                    TopMost = false;
                                    buttonGoWasClicked = false;

                                    if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                    {
                                        fully_loaded = 0;
                                        start_detect = 0;
                                    }

                                    panel_new.Visible = false;
                                }));
                            }
                            // hijacked
                            else
                            {
                                await Task.Run(async () =>
                                {
                                    await Task.Delay(500);
                                });

                                string strValue = label_text_search.Text;
                                string[] strArray = strValue.Split(',');

                                foreach (string obj in strArray)
                                {
                                    bool contains = label_webtitle.Text.Contains(obj);

                                    if (contains == true)
                                    {
                                        Invoke(new Action(() =>
                                        {
                                            label_hijacked.Text = "";
                                        }));

                                        break;
                                    }
                                    else if (!contains)
                                    {
                                        //MessageBox.Show(label_text_search.Text + " asdasdasd " + label_domaintitle.Text + "\nnot safe " + label_domainhide.Text + "\n\n" + textBox_domain.Text);

                                        Invoke(new Action(() =>
                                        {
                                            label_hijacked.Text = "hijacked";
                                        }));
                                    }
                                }

                                // Send data to text file
                                if (label_hijacked.Text == "hijacked")
                                {
                                    DataToTextFileHijacked();
                                }
                                // Timeout Status
                                else if (label_timeout.Text == "timeout")
                                {
                                    DataToTextFileTimeout();
                                }
                                // Successful Status
                                else
                                {
                                    DataToTextFileSuccess();
                                }

                                Invoke(new Action(() =>
                                {
                                    // For timeout
                                    i = 1;
                                    timer_timeout.Stop();

                                    pictureBox_loader.Visible = false;

                                    label_timeout.Text = "";
                                    label_hijacked.Text = "";
                                    label_inaccessible.Text = "";
                                    label_inaccessible_error_message.Text = "";

                                    TopMost = false;
                                    buttonGoWasClicked = false;

                                    if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                    {
                                        fully_loaded = 0;
                                        start_detect = 0;
                                    }

                                    panel_new.Visible = false;
                                }));
                            }
                        }
                    }
                    else
                    {
                        Invoke(new Action(() =>
                        {
                            TopMost = false;
                            buttonGoWasClicked = false;

                            fully_loaded = 0;
                            start_detect = 0;
                            //timer_elseloaded.Start();
                        }));
                    }                    
                }
            }

            
            

















































            // ----TIMER URGENT ENABLED----
            if (timer_domain_urgent.Enabled)
            {
                // --Loading--
                if (e.IsLoading)
                {
                    // Date preview
                    //start_load = DateTime.Now.ToString("HH:mm:ss.fff");

                    //Invoke(new Action(() =>
                    //{
                    //    timer_timeout_urgent.Start();
                    //    pictureBox_loader_urgent.Visible = true;
                    //    label_ifloadornot_urgent.Text = "0";
                    //}));

                    // Detect when stop loads
                    detectnotloading = 0;
                    timer_detectnotloading.Stop();

                    Invoke(new Action(() =>
                    {
                        panel_browser_urgent.Controls.Add(chromeBrowser);
                        start_detect++;
                        label_start_detect.Text = start_detect.ToString();
                    }));

                    //MessageBox.Show("loading asdasdasdasdsa1 " + label_domainhide.Text);

                    // Date preview
                    start_load = DateTime.Now.ToString("HH:mm:ss.fff");
                    start_load_inaccessible = DateTime.Now;

                    Invoke(new Action(() =>
                    {
                        timer_timeout_urgent.Start();
                        pictureBox_loader_urgent.Visible = true;
                        label_ifloadornot_urgent.Text = "1";
                        ms_detect++;
                        label_loadeddetect.Text = ms_detect.ToString();

                        // else loaded
                        elseloaded_i = 0;
                        timer_elseloaded.Stop();
                    }));

                    if (ms_detect == 1)
                    {
                        int webBrowser_i = 0;
                        while (webBrowser_i <= 2)
                        {
                            //string header = "Accept: application/xml\r\nAccept-Language: en-US\r\n";
                            webBrowser_new.Navigate(label_domainhide_urgent.Text);
                            //webBrowser_new.Refresh(WebBrowserRefreshOption.Completely);
                            webBrowser_i++;
                        }
                    }
                    else
                    {
                        int webBrowser_i = 0;
                        while (webBrowser_i <= 2)
                        {
                            //string header = "Accept: application/xml\r\nAccept-Language: en-US\r\n";
                            webBrowser_new.Navigate(label_domainhide_urgent.Text);
                            //webBrowser_new.Refresh(WebBrowserRefreshOption.Completely);
                            webBrowser_i++;
                        }
                    }
                }
                // --Loaded--
                if (!e.IsLoading)
                {
                    // Detect when stop loads
                    detectnotloading = 0;
                    timer_detectnotloading.Start();

                    // Date preview
                    end_load_inaccessible = DateTime.Now;
                    end_load = DateTime.Now.ToString("HH:mm:ss.fff");

                    await Task.Run(async () =>
                    {
                        await Task.Delay(2000);
                    });

                    Invoke(new Action(() =>
                    {
                        fully_loaded++;
                        label_fully_loaded.Text = fully_loaded.ToString();

                        string webtitle = webBrowser_new.DocumentTitle;
                        label_webtitle_urgent.Text = webtitle;
                        textBox_webtitle.Text = webtitle;
                    }));

                    if (label_fully_loaded.Text == "1")
                    {
                        // Inaccessible Status
                        string result = "";
                        string search_replace = label_webtitle_urgent.Text;

                        string upper_search = search_replace.ToUpper().ToString();

                        StringBuilder sb = new StringBuilder(upper_search);
                        sb.Replace("-", "");
                        sb.Replace(".", "");
                        string final_search = Regex.Replace(sb.ToString(), " {2,}", " ");

                        var final_inaccessble_lists = inaccessble_lists.Select(m => m.ToUpper());

                        string[] words = final_search.Split(' ');
                        foreach (string word in words)
                        {
                            var match = final_inaccessble_lists.FirstOrDefault(stringToCheck => stringToCheck.Contains(word));

                            if (match != null)
                            {
                                result = "match";
                                break;
                            }
                            else
                            {
                                result = "no match";
                            }
                        }

                        if (result == "match")
                        {
                            // hijacked
                            if (label_webtitle_urgent.Text == "" && label_inaccessible_error_message.Text == "")
                            {
                                if (label_webtype_urgent.Text == "Landing Page" || label_webtype_urgent.Text == "Landing page")
                                {
                                    var html = "";
                                    try
                                    {
                                        html = new WebClient().DownloadString(textBox_domain.Text);
                                    }
                                    catch (Exception)
                                    {
                                        // Empty
                                    }

                                    if (html.Contains("landing_image"))
                                    {
                                        await Task.Run(async () =>
                                        {
                                            await Task.Delay(500);
                                        });

                                        DataToTextFileSuccess_Urgent();

                                        Invoke(new Action(() =>
                                        {
                                            // For timeout
                                            i_urgent = 1;
                                            timer_timeout_urgent.Stop();

                                            pictureBox_loader_urgent.Visible = false;

                                            label_timeout.Text = "";
                                            label_hijacked.Text = "";
                                            label_inaccessible.Text = "";
                                            label_inaccessible_error_message.Text = "";

                                            if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                            {
                                                fully_loaded = 0;
                                                start_detect = 0;
                                                label_ifloadornot_urgent.Text = "0";
                                            }

                                            panel_new.Visible = false;
                                        }));
                                    }
                                    else
                                    {
                                        // test one more time
                                        Invoke(new Action(() =>
                                        {
                                            testonemoretime++;
                                            label_testonemoretime.Text = testonemoretime.ToString();
                                        }));

                                        if (testonemoretime == 1)
                                        {
                                            Invoke(new Action(() =>
                                            {
                                                int getCurrentIndex = Convert.ToInt32(label_currentindex.Text);
                                                dataGridView_domain.ClearSelection();

                                                //timer_domain.Start();

                                                // For timeout
                                                i_urgent = 1;
                                                timer_timeout_urgent.Start();

                                                fully_loaded = 0;
                                                start_detect = 0;

                                                dataGridView_domain.Rows[getCurrentIndex].Selected = true;
                                            }));
                                        }
                                        else
                                        {
                                            if (ms_detect == 1)
                                            {
                                                if (label_webtitle_urgent.Text == "Can’t reach this page" || label_webtitle_urgent.Text == "This site isn’t secure" || label_webtitle_urgent.Text == "无法访问此页面" || label_webtitle_urgent.Text == "此站点不安全")
                                                {
                                                    Invoke(new Action(() =>
                                                    {
                                                        panel_new.Visible = true;
                                                        panel_new.BringToFront();
                                                    }));
                                                }
                                            }

                                            DataToTextFileHijacked_Urgent();

                                            Invoke(new Action(() =>
                                            {
                                                // For timeout
                                                i_urgent = 1;
                                                timer_timeout_urgent.Stop();

                                                pictureBox_loader_urgent.Visible = false;

                                                label_timeout.Text = "";
                                                label_hijacked.Text = "";
                                                label_inaccessible.Text = "";
                                                label_inaccessible_error_message.Text = "";

                                                if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                                {
                                                    fully_loaded = 0;
                                                    start_detect = 0;
                                                    label_ifloadornot_urgent.Text = "0";
                                                }

                                                testonemoretime = 0;
                                                panel_new.Visible = false;
                                            }));
                                        }
                                    }
                                }
                                else
                                {
                                    // test one more time
                                    Invoke(new Action(() =>
                                    {
                                        testonemoretime++;
                                        label_testonemoretime.Text = testonemoretime.ToString();
                                    }));

                                    if (testonemoretime == 1)
                                    {
                                        Invoke(new Action(() =>
                                        {
                                            int getCurrentIndex = Convert.ToInt32(label_currentindex.Text);
                                            dataGridView_domain.ClearSelection();

                                            //timer_domain.Start();

                                            // For timeout
                                            i_urgent = 1;
                                            timer_timeout_urgent.Start();

                                            fully_loaded = 0;
                                            start_detect = 0;

                                            dataGridView_domain.Rows[getCurrentIndex].Selected = true;
                                        }));
                                    }
                                    else
                                    {
                                        if (ms_detect == 1)
                                        {
                                            if (label_webtitle_urgent.Text == "Can’t reach this page" || label_webtitle_urgent.Text == "This site isn’t secure" || label_webtitle_urgent.Text == "无法访问此页面" || label_webtitle_urgent.Text == "此站点不安全")
                                            {
                                                Invoke(new Action(() =>
                                                {
                                                    panel_new.Visible = true;
                                                    panel_new.BringToFront();
                                                }));
                                            }
                                        }

                                        DataToTextFileHijacked_Urgent();

                                        Invoke(new Action(() =>
                                        {
                                            // For timeout
                                            i_urgent = 1;
                                            timer_timeout_urgent.Stop();

                                            pictureBox_loader_urgent.Visible = false;

                                            label_timeout.Text = "";
                                            label_hijacked.Text = "";
                                            label_inaccessible.Text = "";
                                            label_inaccessible_error_message.Text = "";

                                            if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                            {
                                                fully_loaded = 0;
                                                start_detect = 0;
                                                label_ifloadornot_urgent.Text = "0";
                                            }

                                            testonemoretime = 0;
                                            panel_new.Visible = false;
                                        }));
                                    }
                                }
                            }
                            // inaccessible
                            else
                            {
                                // error aborted test one more time
                                if (label_inaccessible_error_message.Text == "ERR_ABORTED" || label_inaccessible_error_message.Text == "ERR_NETWORK_CHANGED" || label_inaccessible_error_message.Text == "ERR_INTERNET_DISCONNECTED" || label_inaccessible_error_message.Text == "Navigation Canceled" || label_inaccessible_error_message.Text == "导航已取消")
                                {
                                    // test one more time
                                    Invoke(new Action(() =>
                                    {
                                        erroraborted_testonemoretime++;
                                        label_erroraborted.Text = erroraborted_testonemoretime.ToString();
                                    }));

                                    if (erroraborted_testonemoretime == 1)
                                    {
                                        Invoke(new Action(() =>
                                        {
                                            int getCurrentIndex = Convert.ToInt32(label_currentindex.Text);
                                            dataGridView_domain.ClearSelection();

                                            //timer_domain.Start();

                                            // For timeout
                                            i_urgent = 1;
                                            timer_timeout_urgent.Start();

                                            fully_loaded = 0;
                                            start_detect = 0;

                                            dataGridView_domain.Rows[getCurrentIndex].Selected = true;

                                            label_inaccessible_error_message.Text = "";
                                        }));
                                    }
                                    else
                                    {
                                        if (label_webtitle_urgent.Text == "Can’t reach this page" || label_webtitle_urgent.Text == "This site isn’t secure" || label_webtitle_urgent.Text == "无法访问此页面" || label_webtitle_urgent.Text == "此站点不安全")
                                        {
                                            Invoke(new Action(() =>
                                            {
                                                panel_new.Visible = true;
                                                panel_new.BringToFront();
                                            }));
                                        }

                                        if (ms_detect == 1)
                                        {
                                            if (label_webtitle_urgent.Text == "Can’t reach this page" || label_webtitle_urgent.Text == "This site isn’t secure" || label_webtitle_urgent.Text == "无法访问此页面" || label_webtitle_urgent.Text == "此站点不安全")
                                            {
                                                Invoke(new Action(() =>
                                                {
                                                    panel_new.Visible = true;
                                                    panel_new.BringToFront();
                                                }));
                                            }
                                        }

                                        Invoke(new Action(async () =>
                                        {
                                            label_inaccessible.Text = "inaccessible";

                                            TimeSpan span = end_load_inaccessible - start_load_inaccessible;
                                            int ms = (int)span.TotalMilliseconds;

                                            // for fast load
                                            if (ms < 500)
                                            {
                                                panel_new.Visible = true;
                                                panel_new.BringToFront();

                                                int webBrowser_i = 0;
                                                while (webBrowser_i <= 2)
                                                {
                                                    webBrowser_new.Navigate(label_domainhide_urgent.Text);
                                                    webBrowser_i++;
                                                }

                                                await Task.Run(async () =>
                                                {
                                                    await Task.Delay(500);
                                                });

                                                string datetime = label11.Text;
                                                string datetime_folder = label9.Text;
                                                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                                string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder + "_urgent_" + i_timeout + "\\" + datetime_folder;

                                                string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder + "_urgent_" + i_timeout;

                                                DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                                Rectangle bounds = Bounds;
                                                using (Bitmap bitmap = new Bitmap(bounds.Width - 267, bounds.Height - 202))
                                                {
                                                    using (Graphics g = Graphics.FromImage(bitmap))
                                                    {
                                                        g.CopyFromScreen(new Point(bounds.Left + 226, bounds.Top + 159), Point.Empty, bounds.Size);
                                                    }

                                                    Bitmap resized = new Bitmap(bitmap, new Size(bitmap.Width / 2, bitmap.Height / 2));
                                                    resized.Save(path + "_" + label_macid.Text + "_u_" + label_domainhide_urgent.Text + ".jpeg", ImageFormat.Jpeg);
                                                }

                                                DataToTextFileInaccessible_Urgent();

                                                // For timeout
                                                i_urgent = 1;
                                                timer_timeout_urgent.Stop();

                                                pictureBox_loader_urgent.Visible = false;

                                                label_timeout.Text = "";
                                                label_hijacked.Text = "";
                                                label_inaccessible.Text = "";
                                                label_inaccessible_error_message.Text = "";
                                                erroraborted_testonemoretime = 0;

                                                if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                                {
                                                    fully_loaded = 0;
                                                    start_detect = 0;
                                                    label_ifloadornot_urgent.Text = "0";
                                                }

                                                Invoke(new Action(() =>
                                                {
                                                    panel_new.Visible = false;
                                                }));
                                            }
                                            else
                                            {
                                                await Task.Run(async () =>
                                                {
                                                    await Task.Delay(500);
                                                });

                                                string datetime = label11.Text;
                                                string datetime_folder = label9.Text;
                                                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                                string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder + "_urgent_" + i_timeout + "\\" + datetime_folder;

                                                string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder + "_urgent_" + i_timeout;

                                                DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                                Rectangle bounds = Bounds;
                                                using (Bitmap bitmap = new Bitmap(bounds.Width - 267, bounds.Height - 202))
                                                {
                                                    using (Graphics g = Graphics.FromImage(bitmap))
                                                    {
                                                        g.CopyFromScreen(new Point(bounds.Left + 226, bounds.Top + 159), Point.Empty, bounds.Size);
                                                    }

                                                    Bitmap resized = new Bitmap(bitmap, new Size(bitmap.Width / 2, bitmap.Height / 2));
                                                    resized.Save(path + "_" + label_macid.Text + "_u_" + label_domainhide_urgent.Text + ".jpeg", ImageFormat.Jpeg);
                                                }

                                                DataToTextFileInaccessible_Urgent();

                                                // For timeout
                                                i_urgent = 1;
                                                timer_timeout_urgent.Stop();

                                                pictureBox_loader_urgent.Visible = false;

                                                label_timeout.Text = "";
                                                label_hijacked.Text = "";
                                                label_inaccessible.Text = "";
                                                label_inaccessible_error_message.Text = "";
                                                erroraborted_testonemoretime = 0;

                                                if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                                {
                                                    fully_loaded = 0;
                                                    start_detect = 0;
                                                    label_ifloadornot_urgent.Text = "0";
                                                }

                                                Invoke(new Action(() =>
                                                {
                                                    panel_new.Visible = false;
                                                }));
                                            }
                                        }));
                                    }
                                }
                                else
                                {
                                    if (label_webtitle_urgent.Text == "Can’t reach this page" || label_webtitle_urgent.Text == "This site isn’t secure" || label_webtitle_urgent.Text == "无法访问此页面" || label_webtitle_urgent.Text == "此站点不安全")
                                    {
                                        Invoke(new Action(() =>
                                        {
                                            panel_new.Visible = true;
                                            panel_new.BringToFront();
                                        }));
                                    }

                                    if (ms_detect == 1)
                                    {
                                        if (label_webtitle_urgent.Text == "Can’t reach this page" || label_webtitle_urgent.Text == "This site isn’t secure" || label_webtitle_urgent.Text == "无法访问此页面" || label_webtitle_urgent.Text == "此站点不安全")
                                        {
                                            Invoke(new Action(() =>
                                            {
                                                panel_new.Visible = true;
                                                panel_new.BringToFront();
                                            }));
                                        }
                                    }

                                    Invoke(new Action(async () =>
                                    {
                                        label_inaccessible.Text = "inaccessible";

                                        TimeSpan span = end_load_inaccessible - start_load_inaccessible;
                                        int ms = (int)span.TotalMilliseconds;

                                        // for fast load
                                        if (ms < 500)
                                        {
                                            panel_new.Visible = true;
                                            panel_new.BringToFront();

                                            int webBrowser_i = 0;
                                            while (webBrowser_i <= 2)
                                            {
                                                webBrowser_new.Navigate(label_domainhide_urgent.Text);
                                                webBrowser_i++;
                                            }

                                            await Task.Run(async () =>
                                            {
                                                await Task.Delay(500);
                                            });

                                            string datetime = label11.Text;
                                            string datetime_folder = label9.Text;
                                            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                            string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder + "_urgent_" + i_timeout + "\\" + datetime_folder;

                                            string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder + "_urgent_" + i_timeout;

                                            DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                            Rectangle bounds = Bounds;
                                            using (Bitmap bitmap = new Bitmap(bounds.Width - 267, bounds.Height - 202))
                                            {
                                                using (Graphics g = Graphics.FromImage(bitmap))
                                                {
                                                    g.CopyFromScreen(new Point(bounds.Left + 226, bounds.Top + 159), Point.Empty, bounds.Size);
                                                }

                                                Bitmap resized = new Bitmap(bitmap, new Size(bitmap.Width / 2, bitmap.Height / 2));
                                                resized.Save(path + "_" + label_macid.Text + "_u_" + label_domainhide_urgent.Text + ".jpeg", ImageFormat.Jpeg);
                                            }

                                            DataToTextFileInaccessible_Urgent();

                                            // For timeout
                                            i_urgent = 1;
                                            timer_timeout_urgent.Stop();

                                            pictureBox_loader_urgent.Visible = false;

                                            label_timeout.Text = "";
                                            label_hijacked.Text = "";
                                            label_inaccessible.Text = "";
                                            label_inaccessible_error_message.Text = "";
                                            erroraborted_testonemoretime = 0;

                                            if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                            {
                                                fully_loaded = 0;
                                                start_detect = 0;
                                                label_ifloadornot_urgent.Text = "0";
                                            }

                                            Invoke(new Action(() =>
                                            {
                                                panel_new.Visible = false;
                                            }));
                                        }
                                        else
                                        {
                                            await Task.Run(async () =>
                                            {
                                                await Task.Delay(500);
                                            });

                                            string datetime = label11.Text;
                                            string datetime_folder = label9.Text;
                                            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                            string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder + "_urgent_" + i_timeout + "\\" + datetime_folder;

                                            string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder + "_urgent_" + i_timeout;

                                            DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                            Rectangle bounds = Bounds;
                                            using (Bitmap bitmap = new Bitmap(bounds.Width - 267, bounds.Height - 202))
                                            {
                                                using (Graphics g = Graphics.FromImage(bitmap))
                                                {
                                                    g.CopyFromScreen(new Point(bounds.Left + 226, bounds.Top + 159), Point.Empty, bounds.Size);
                                                }

                                                Bitmap resized = new Bitmap(bitmap, new Size(bitmap.Width / 2, bitmap.Height / 2));
                                                resized.Save(path + "_" + label_macid.Text + "_u_" + label_domainhide_urgent.Text + ".jpeg", ImageFormat.Jpeg);
                                            }

                                            DataToTextFileInaccessible_Urgent();

                                            // For timeout
                                            i_urgent = 1;
                                            timer_timeout_urgent.Stop();

                                            pictureBox_loader_urgent.Visible = false;

                                            label_timeout.Text = "";
                                            label_hijacked.Text = "";
                                            label_inaccessible.Text = "";
                                            label_inaccessible_error_message.Text = "";
                                            erroraborted_testonemoretime = 0;

                                            if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                            {
                                                fully_loaded = 0;
                                                start_detect = 0;
                                                label_ifloadornot_urgent.Text = "0";
                                            }

                                            Invoke(new Action(() =>
                                            {
                                                panel_new.Visible = false;
                                            }));
                                        }
                                    }));
                                }
                            }
                        }
                        // Hijacked Status
                        else
                        {
                            // inaccessible
                            if (label_webtitle_urgent.Text == label_domainhide_urgent.Text)
                            {
                                await Task.Run(async () =>
                                {
                                    await Task.Delay(500);
                                });

                                string datetime = label11.Text;
                                string datetime_folder = label9.Text;
                                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder + "_urgent_" + i_timeout + "\\" + datetime_folder;

                                string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder + "_urgent_" + i_timeout;

                                DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                Rectangle bounds = Bounds;
                                using (Bitmap bitmap = new Bitmap(bounds.Width - 267, bounds.Height - 202))
                                {
                                    using (Graphics g = Graphics.FromImage(bitmap))
                                    {
                                        g.CopyFromScreen(new Point(bounds.Left + 226, bounds.Top + 159), Point.Empty, bounds.Size);
                                    }

                                    Bitmap resized = new Bitmap(bitmap, new Size(bitmap.Width / 2, bitmap.Height / 2));
                                    resized.Save(path + "_" + label_macid.Text + "_u_" + label_domainhide_urgent.Text + ".jpeg", ImageFormat.Jpeg);
                                }

                                DataToTextFileInaccessible_Urgent();

                                Invoke(new Action(() =>
                                {
                                    // For timeout
                                    i_urgent = 1;
                                    timer_timeout_urgent.Stop();

                                    pictureBox_loader_urgent.Visible = false;

                                    label_timeout.Text = "";
                                    label_hijacked.Text = "";
                                    label_inaccessible.Text = "";
                                    label_inaccessible_error_message.Text = "";

                                    if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                    {
                                        fully_loaded = 0;
                                        start_detect = 0;
                                        label_ifloadornot_urgent.Text = "0";
                                    }

                                    panel_new.Visible = false;
                                }));
                            }
                            // success
                            else if (label_webtitle_urgent.Text.Contains("Bing"))
                            {
                                await Task.Run(async () =>
                                {
                                    await Task.Delay(500);
                                });

                                DataToTextFileSuccess_Urgent();

                                Invoke(new Action(() =>
                                {
                                    // For timeout
                                    i_urgent = 1;
                                    timer_timeout_urgent.Stop();

                                    pictureBox_loader_urgent.Visible = false;

                                    label_timeout.Text = "";
                                    label_hijacked.Text = "";
                                    label_inaccessible.Text = "";
                                    label_inaccessible_error_message.Text = "";

                                    if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                    {
                                        fully_loaded = 0;
                                        start_detect = 0;
                                        label_ifloadornot_urgent.Text = "0";
                                    }

                                    panel_new.Visible = false;
                                }));
                            }
                            // hijacked
                            else
                            {
                                await Task.Run(async () =>
                                {
                                    await Task.Delay(500);
                                });

                                string strValue = label_text_search_urgent.Text;
                                string[] strArray = strValue.Split(',');

                                foreach (string obj in strArray)
                                {
                                    bool contains = label_webtitle_urgent.Text.Contains(obj);

                                    if (contains == true)
                                    {
                                        Invoke(new Action(() =>
                                        {
                                            label_hijacked.Text = "";
                                        }));

                                        break;
                                    }
                                    else if (!contains)
                                    {
                                        //MessageBox.Show(label_text_search.Text + " asdasdasd " + label_domaintitle.Text + "\nnot safe " + label_domainhide.Text + "\n\n" + textBox_domain.Text);

                                        Invoke(new Action(() =>
                                        {
                                            label_hijacked.Text = "hijacked";
                                        }));
                                    }
                                }

                                // Send data to text file
                                if (label_hijacked.Text == "hijacked")
                                {
                                    DataToTextFileHijacked_Urgent();
                                }
                                // Timeout Status
                                else if (label_timeout.Text == "timeout")
                                {
                                    DataToTextFileTimeout_Urgent();
                                }
                                // Successful Status
                                else
                                {
                                    DataToTextFileSuccess_Urgent();
                                }

                                Invoke(new Action(() =>
                                {
                                    // For timeout
                                    i_urgent = 1;
                                    timer_timeout_urgent.Stop();

                                    pictureBox_loader_urgent.Visible = false;

                                    label_timeout.Text = "";
                                    label_hijacked.Text = "";
                                    label_inaccessible.Text = "";
                                    label_inaccessible_error_message.Text = "";

                                    if (Convert.ToInt32(label_start_detect.Text) <= 1)
                                    {
                                        fully_loaded = 0;
                                        start_detect = 0;
                                        label_ifloadornot_urgent.Text = "0";
                                    }

                                    panel_new.Visible = false;
                                }));
                            }
                        }
                    }
                    else
                    {
                        Invoke(new Action(() =>
                        {
                            timer_elseloaded.Start();
                        }));
                    }
                }
            }
        }
        
        private void CloseIt()
        {
            System.Threading.Thread.Sleep(500);
            Microsoft.VisualBasic.Interaction.AppActivate(
                 System.Diagnostics.Process.GetCurrentProcess().Id);
            SendKeys.SendWait(" ");
        }

        //public bool OnJSDialog(IWebBrowser browserControl, IBrowser browser, string originUrl, CefJsDialogType dialogType, string messageText, string defaultPromptText, IJsDialogCallback callback, ref bool suppressMessage)
        //{
        //    callback.Continue(true);
        //    return true;
        //}

        //public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IWindowInfo windowInfo, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        //{
        //    //System.Diagnostics.Process.Start(targetUrl);
        //    //newBrowser = null;
        //    //return false;

        //    chromeBrowser.Load(targetUrl);
        //    return false;
        //}

        // Main
        private void DataToTextFileSuccess()
        {
            //MessageBox.Show("Start Time: " + start_load + "\n" +
            //                "End Time: " + end_load);

            try
            {
                string datetime = label11.Text;
                string datetime_folder = label9.Text;
                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                string path = path_desktop + "\\rainCheck\\" + datetime_folder;

                if (Directory.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type";
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        //swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + "," + textBox_domain.Text + "," + "," + "," + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        //swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + "," + textBox_domain.Text + "," + "," + "," + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");
                        swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");

                        swww.Close();
                    }
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type";
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        //swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + "," + textBox_domain.Text + "," + "," + "," + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);

                        swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");

                        swww.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1006", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }
        }

        private void DataToTextFileTimeout()
        {
            //MessageBox.Show("Date Today: " + datetime + "\n" +
            //                "Start Time: " + start_load + "\n" +
            //                "End Time: " + end_load);

            try
            {
                string datetime = label11.Text;
                string datetime_folder = label9.Text;
                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                string path = path_desktop + "\\rainCheck\\" + datetime_folder;

                if (Directory.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type";
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        //swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + "," + textBox_domain.Text + "," + "," + "," + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);

                        swww.WriteLine("," + label_domainhide.Text + ",T" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");

                        swww.Close();
                    }
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type";
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        //swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + "," + textBox_domain.Text + "," + "," + "," + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);

                        swww.WriteLine("," + label_domainhide.Text + ",T" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");

                        swww.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1007", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }
        }

        private void DataToTextFileHijacked()
        {
            //MessageBox.Show("Date Today: " + datetime + "\n" +
            //                "Start Time: " + start_load + "\n" +
            //                "End Time: " + end_load);

            try
            {
                string datetime = label11.Text;
                string datetime_folder = label9.Text;
                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                string path = path_desktop + "\\rainCheck\\" + datetime_folder;

                if (Directory.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type";
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        //swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + "," + textBox_domain.Text + "," + "," + "," + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);

                        if (label_webtitle.Text == "")
                        {
                            Invoke(new Action(() =>
                            {
                                label_webtitle.Text = textBox_domain.Text;
                            }));
                        }

                        if (string.IsNullOrEmpty(isp_get))
                        {
                            isp_get = "-";
                        }

                        if (string.IsNullOrEmpty(city_get))
                        {
                            city_get = "-";
                        }

                        swww.WriteLine("," + label_domainhide.Text + ",H" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + "," + textBox_domain.Text + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");

                        swww.Close();
                    }
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type";
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        //swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + "," + textBox_domain.Text + "," + "," + "," + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);

                        if (label_webtitle.Text == "")
                        {
                            Invoke(new Action(() =>
                            {
                                label_webtitle.Text = textBox_domain.Text;
                            }));
                        }

                        if (string.IsNullOrEmpty(isp_get))
                        {
                            isp_get = "-";
                        }

                        if (string.IsNullOrEmpty(city_get))
                        {
                            city_get = "-";
                        }

                        swww.WriteLine("," + label_domainhide.Text + ",H" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + "," + textBox_domain.Text + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");

                        swww.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1008", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }
        }

        private void DataToTextFileInaccessible()
        {
            //MessageBox.Show("Date Today: " + datetime + "\n" +
            //                "Start Time: " + start_load + "\n" +
            //                "End Time: " + end_load);

            try
            {
                string datetime = label11.Text;
                string datetime_folder = label9.Text;
                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                string path = path_desktop + "\\rainCheck\\" + datetime_folder;

                if (Directory.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type";
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        //swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + "," + textBox_domain.Text + "," + "," + "," + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);

                        string error_message = "";

                        if (label_webtitle.Text != "")
                        {
                            error_message = label_webtitle.Text;

                            if (label_webtitle.Text == label_domainhide.Text)
                            {
                                //error_message = label_inaccessible_error_message.Text;
                                error_message = "Domain name expired";
                            }
                        }
                        else
                        {
                            error_message = label_inaccessible_error_message.Text;
                        }

                        if (label_webtitle.Text == "")
                        {
                            label_webtitle.Text = label_domainhide.Text;
                        }

                        if (string.IsNullOrEmpty(isp_get))
                        {
                            isp_get = "-";
                        }

                        if (string.IsNullOrEmpty(city_get))
                        {
                            city_get = "-";
                        }

                        swww.WriteLine(","+label_domainhide.Text + ",I" + ","+label_brandhide.Text + ","+start_load + ","+end_load + ","+label_webtitle.Text  + ",-" + ",-" + ","+error_message + ","+datetime_folder + "_" + label_macid.Text + "_n_" + label_domainhide.Text + ","+isp_get + ","+city_get + ","+datetime + "," + ",N");

                        swww.Close();
                    }
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type";
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        //swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + "," + textBox_domain.Text + "," + "," + "," + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);

                        string error_message = "";

                        if (label_webtitle.Text != "")
                        {
                            error_message = label_webtitle.Text;

                            if (label_webtitle.Text == label_domainhide.Text)
                            {
                                //error_message = label_inaccessible_error_message.Text;
                                error_message = "Domain name expired";
                            }
                        }
                        else
                        {
                            error_message = label_inaccessible_error_message.Text;
                        }

                        if (label_webtitle.Text == "")
                        {
                            label_webtitle.Text = label_domainhide.Text;
                        }

                        if (string.IsNullOrEmpty(isp_get))
                        {
                            isp_get = "-";
                        }

                        if (string.IsNullOrEmpty(city_get))
                        {
                            city_get = "-";
                        }

                        swww.WriteLine("," + label_domainhide.Text + ",I" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + ",-" + ",-" + "," + error_message + "," + datetime_folder + "_" + label_macid.Text + "_n_" + label_domainhide.Text + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");

                        swww.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1009", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }
        }

        // Urgent
        int i_timeout = 1;
        private void DataToTextFileSuccess_Urgent()
        {
            //MessageBox.Show("Date Today: " + datetime + "\n" +
            //                "Start Time: " + start_load + "\n" +
            //                "End Time: " + end_load);

            try
            {
                string datetime = label11.Text;
                string datetime_folder = label9.Text;
                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder + "_urgent_" + i_timeout;

                if (Directory.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type";
                    if (File.ReadLines(path + "\\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide_urgent.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + @"\result.txt", true, System.Text.Encoding.UTF8);
                        swww.WriteLine("," + label_domainhide_urgent.Text + ",S" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + label_webtitle_urgent.Text + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + datetime + "," + ",U");

                        swww.Close();
                    }
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type";
                    if (File.ReadLines(path + "\\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide_urgent.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + @"\result.txt", true, System.Text.Encoding.UTF8);
                        swww.WriteLine("," + label_domainhide_urgent.Text + ",S" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + label_webtitle_urgent.Text + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + datetime + "," + ",U");

                        swww.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1010", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }
        }

        private void DataToTextFileTimeout_Urgent()
        {
            //MessageBox.Show("Date Today: " + datetime + "\n" +
            //                "Start Time: " + start_load + "\n" +
            //                "End Time: " + end_load);

            try
            {
                string datetime = label11.Text;
                string datetime_folder = label9.Text;
                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder + "_urgent_" + i_timeout;

                if (Directory.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type";
                    if (File.ReadLines(path + "\\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide_urgent.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        
                        swww.WriteLine("," + label_domainhide_urgent.Text + ",T" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + label_webtitle_urgent.Text + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + datetime + "," + ",U");

                        swww.Close();
                    }
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type";
                    if (File.ReadLines(path + "\\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide_urgent.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);

                        swww.WriteLine("," + label_domainhide_urgent.Text + ",T" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + label_webtitle_urgent.Text + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + datetime + "," + ",U");

                        swww.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1011", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }
        }
        
        private void DataToTextFileHijacked_Urgent()
        {
            //MessageBox.Show("Date Today: " + datetime + "\n" +
            //                "Start Time: " + start_load + "\n" +
            //                "End Time: " + end_load);

            try
            {
                string datetime = label11.Text;
                string datetime_folder = label9.Text;
                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder + "_urgent_" + i_timeout;

                if (Directory.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();
                    
                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type";
                    if (File.ReadLines(path + "\\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide_urgent.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);

                        if (label_webtitle_urgent.Text == "")
                        {
                            Invoke(new Action(() =>
                            {
                                label_webtitle_urgent.Text = textBox_domain_urgent.Text;
                            }));
                        }

                        if (string.IsNullOrEmpty(isp_get))
                        {
                            isp_get = "-";
                        }

                        if (string.IsNullOrEmpty(city_get))
                        {
                            city_get = "-";
                        }

                        swww.WriteLine("," + label_domainhide_urgent.Text + ",H" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + label_webtitle_urgent.Text + "," + textBox_domain_urgent.Text + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + datetime + "," + ",U");

                        swww.Close();
                    }
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type";
                    if (File.ReadLines(path + "\\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide_urgent.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);

                        if (label_webtitle_urgent.Text == "")
                        {
                            Invoke(new Action(() =>
                            {
                                label_webtitle_urgent.Text = textBox_domain_urgent.Text;
                            }));
                        }

                        if (string.IsNullOrEmpty(isp_get))
                        {
                            isp_get = "-";
                        }

                        if (string.IsNullOrEmpty(city_get))
                        {
                            city_get = "-";
                        }

                        swww.WriteLine("," + label_domainhide_urgent.Text + ",H" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + label_webtitle_urgent.Text + "," + textBox_domain_urgent.Text + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + datetime + "," + ",U");

                        swww.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1012", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }
        }

        private void DataToTextFileInaccessible_Urgent()
        {
            //MessageBox.Show("Date Today: " + datetime + "\n" +
            //                "Start Time: " + start_load + "\n" +
            //                "End Time: " + end_load);

            try
            {
                string datetime = label11.Text;
                string datetime_folder = label9.Text;
                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder + "_urgent_" + i_timeout;

                if (Directory.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type";
                    if (File.ReadLines(path + "\\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        //swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + "," + textBox_domain.Text + "," + "," + "," + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide_urgent.Text;
                    if (File.ReadLines(path + "\\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);

                        string error_message = "";

                        if (label_webtitle_urgent.Text != "")
                        {
                            error_message = label_webtitle_urgent.Text;

                            if (label_webtitle_urgent.Text == label_domainhide_urgent.Text)
                            {
                                //error_message = label_inaccessible_error_message.Text;
                                error_message = "Domain name expired";
                            }
                        }
                        else
                        {
                            error_message = label_inaccessible_error_message.Text;
                        }

                        if (label_webtitle_urgent.Text == "")
                        {
                            label_webtitle_urgent.Text = label_domainhide_urgent.Text;
                        }

                        if (string.IsNullOrEmpty(isp_get))
                        {
                            isp_get = "-";
                        }

                        if (string.IsNullOrEmpty(city_get))
                        {
                            city_get = "-";
                        }

                        swww.WriteLine("," + label_domainhide_urgent.Text + ",I" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + label_webtitle_urgent.Text + ",-" + ",-" + "," + error_message + "," + datetime_folder + "_" +label_macid.Text + "_u_" + label_domainhide_urgent.Text + "," + isp_get + "," + city_get + "," + datetime + "," + ",U");

                        swww.Close();
                    }
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type";
                    if (File.ReadLines(path + "\\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        //swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + "," + textBox_domain.Text + "," + "," + "," + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide_urgent.Text;
                    if (File.ReadLines(path + "\\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);

                        string error_message = "";

                        if (label_webtitle_urgent.Text != "")
                        {
                            error_message = label_webtitle_urgent.Text;

                            if (label_webtitle_urgent.Text == label_domainhide_urgent.Text)
                            {
                                //error_message = label_inaccessible_error_message.Text;
                                error_message = "Domain name expired";
                            }
                        }
                        else
                        {
                            error_message = label_inaccessible_error_message.Text;
                        }

                        if (label_webtitle_urgent.Text == "")
                        {
                            label_webtitle_urgent.Text = label_domainhide_urgent.Text;
                        }

                        if (string.IsNullOrEmpty(isp_get))
                        {
                            isp_get = "-";
                        }

                        if (string.IsNullOrEmpty(city_get))
                        {
                            city_get = "-";
                        }

                        swww.WriteLine("," + label_domainhide_urgent.Text + ",I" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + label_webtitle_urgent.Text + ",-" + ",-" + "," + error_message + "," + datetime_folder + "_" + label_macid.Text + "_u_" + label_domainhide_urgent.Text + "," + isp_get + "," + city_get + "," + datetime + "," + ",U");

                        swww.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1009", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }
        }

        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (can_close)
            {
                dr = MessageBox.Show("Are you sure you want to exit the program?", "rainCheck", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    Cef.Shutdown();
                }
            }
        }

        private void Button_go_Click(object sender, EventArgs e)
        {
            // Set browser panel dock style
            chromeBrowser.Dock = DockStyle.Fill;

            i = 1;

            label_domainhide.Text = textBox_domain.Text;
            string domain_urgent = label_domainhide.Text;

            // API Brand
            try
            {
                using (var client = new WebClient())
                {
                    string auth = "r@inCh3ckd234b70";
                    string type = "brand";
                    string request = "http://raincheck.ssitex.com/api/api.php";
                    string domain = domain_urgent;

                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "auth", auth },
                        { "type", type },
                        { "domain", domain }
                    };

                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));

                    if (pagesource != "")
                    {
                        JArray jsonObject = JArray.Parse(pagesource);
                        string brand_name = jsonObject[0]["brand_name"].Value<string>();
                        string text_search = jsonObject[0]["text_search"].Value<string>();
                        string website_type = jsonObject[0]["website_type"].Value<string>();

                        label_brandhide.Text = brand_name;
                        label_text_search.Text = text_search;
                        label_webtype.Text = website_type;

                        //MessageBox.Show(brand_name + " " + text_search);
                    }
                    else
                    {
                        label_brand_id.Text = "";

                        Form_Brand form_brand = new Form_Brand(domain_urgent);
                        form_brand.ShowDialog();

                        label_brandhide.Text = SetValueForTextBrandID;
                        label_text_search.Text = SetValueForTextSearch;
                        label_webtype.Text = SetValueForWebsiteType;
                    }

                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1013", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }

            buttonGoWasClicked = true;
            buttonDetect = true;

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
                label_webtitle.Text = "";
                domain_total = dataGridView_domain.RowCount;
                
                label_domainscount.Text = "Total: " + (index + 2) + " of " + domain_total.ToString();

                if (timerfornext == true)
                {
                    dataGridView_domain.ClearSelection();
                    index = domain_total;
                    chromeBrowser.Stop();
                }
                else
                {
                    index = dataGridView_domain.SelectedRows[0].Index + 1;
                    label_currentindex.Text = index.ToString();
                }

                if (index == domain_total)
                {
                    label_status.Text = "[Loading]";

                    index = 0;
                    label_domainscount.Text = "Total: " + domain_total.ToString();

                    // else loaded
                    elseloaded_i = 0;
                    timer_elseloaded.Stop();

                    // Set browser panel dock style
                    chromeBrowser.Dock = DockStyle.None;
                    textBox_domain.Text = "";

                    dataGridView_domain.ClearSelection();

                    // Enable visible buttons
                    button_start.Visible = true;
                    button_pause.Visible = false;
                    button_start.Enabled = false;
                    button_startover.Enabled = false;
                    pictureBox_loader.Visible = false;

                    timer_domain.Stop();

                    TopMost = false;
                    MinimizeBox = true;

                    // Detect when stop loads
                    detectnotloading = 0;
                    timer_detectnotloading.Stop();

                    //ms_detect = 0;
                    fully_loaded = 0;
                    start_detect = 0;

                    // set time for next to false
                    timerfornext = false;
                                       
                    string datetime_folder = label9.Text;
                    string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                    string path = path_desktop + "\\rainCheck\\" + datetime_folder;
                    
                    // Insert
                    string read = File.ReadAllText(path + "\\result.txt");

                    StringBuilder sb = new StringBuilder();
                    using (var p = ChoCSVReader.LoadText(read).WithFirstLineHeader())
                    {
                        using (var w = new ChoJSONWriter(sb))
                        {
                            w.Write(p);
                        }
                    }

                    int upload = 1;
                    while (upload <= 5)
                    {
                        try
                        {
                            using (var client = new WebClient())
                            {
                                string auth = "r@inCh3ckd234b70";
                                string type = "reports_normal";
                                string request = "http://raincheck.ssitex.com/api/api.php";
                                string reports = sb.ToString();

                                NameValueCollection postData = new NameValueCollection()
                            {
                                { "auth", auth },
                                { "type", type },
                                { "reports", reports },
                            };

                                pagesource_history = Encoding.UTF8.GetString(client.UploadValues(request, postData));

                                if (pagesource_history == "SUCCESS")
                                {
                                    break;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            upload++;

                            label_uploadstatus.Text = "Upload Error!";
                        }
                    }

                    panel_loader.Visible = true;
                    panel_loader.BringToFront();
                    timer_loader.Start();

                    label_currentindex.Text = "0";
                    
                    label9.Text = "";
                    label11.Text = "";

                    string outputpath = "";
                    using (ZipFile zip = new ZipFile())
                    {
                        try
                        {
                            outputpath = path_desktop + "\\rainCheck\\" + datetime_folder + ".zip";
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
                            MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1014", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            //Close();
                        }
                    }
                    
                    try
                    {
                        FtpWebRequest req = (FtpWebRequest)WebRequest.Create("ftp://raincheck.ssitex.com/public/zip/" + datetime_folder);
                        req.UseBinary = true;
                        req.Method = WebRequestMethods.Ftp.UploadFile;
                        req.Credentials = new NetworkCredential("ftpuser@hades.ssitex.com", "p0w3r@SSI");
                        byte[] fileData = File.ReadAllBytes(outputpath);

                        req.ContentLength = fileData.Length;
                        Stream reqStream = req.GetRequestStream();
                        reqStream.Write(fileData, 0, fileData.Length);
                        reqStream.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1030", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // Timer Main
                    domain_i = 0;

                    string date = DateTime.Now.ToString("MMM dd");
                    // Balloon Notification
                    var notification = new NotifyIcon()
                    {
                        Visible = true,
                        Icon = SystemIcons.Information,
                        BalloonTipIcon = ToolTipIcon.Info,
                        BalloonTipTitle = "Information",
                        BalloonTipText = date + " " + label_timeget.Text + " done.",
                    };

                    notification.ShowBalloonTip(1000);
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

                //if (label9.Text == "")
                //{
                //    label9.Text = label9.Text;
                //}

                //if (label11.Text == "")
                //{
                //    label11.Text = label11.Text;
                //}

                ms_detect = 0;
                fully_loaded = 0;
                start_detect = 0;
                domain_i = 0;
                label_currentindex.Text = "0";

                string datetime = label11.Text;
                string datetime_folder = label9.Text;
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

                dataGridView_domain.ClearSelection();
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

            chromeBrowser.Stop();

            // Set browser panel dock style
            chromeBrowser.Dock = DockStyle.None;

            timer_blink.Start();
            label_status.Text = "[Paused]";
            timer_domain.Stop();
            timer_timeout.Stop();
            pictureBox_loader.Visible = false;

            timer_detectnotloading.Stop();
            detectnotloading = 0;

            button_pause.Visible = false;
            button_start.Visible = true;
            button_start.Enabled = true;

            textBox_domain.Enabled = true;
            button_go.Enabled = true;

            label_inaccessible_error_message.Text = "";
            textBox_domain.Text = "";
            ActiveControl = textBox_domain;

            button_urgent.Visible = true;
            
            fully_loaded = 0;
            start_detect = 0;
            //label_domaintitle.Text = "";
        }

        private void Button_resume_Click(object sender, EventArgs e)
        {
            if (!buttonDetect)
            {
                if (label_currentindex.Text == "0")
                {
                    string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string path = path_desktop + "\\rainCheck\\" + label9.Text;

                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }
                }
            }

            panel_browser.Controls.Add(chromeBrowser);

            TopMost = true;
            MinimizeBox = false;

            pictureBox_loader.Visible = true;

            // Set browser panel dock style
            chromeBrowser.Dock = DockStyle.Fill;

            //if (textchange_date == 1)
            //{
            //    if (label9.Text == "")
            //    {
            //        label9.Text = label9.Text;
            //    }

            //    if (label11.Text == "")
            //    {
            //        label11.Text = label11.Text;
            //    }
            //}

            label_domainscount.Text = "Total: " + (index + 1) + " of " + domain_total.ToString();

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

            // textchanged timefor
            textchanged_timefor = true;
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
                    string domain;
                    string brand;
                    string text_search;
                    string webtype;
                    //currentIndex = dataGridView_domain.CurrentRow.Index;

                    try
                    {
                        domain = row.Cells[1].Value.ToString();
                        brand = row.Cells[2].Value.ToString();
                        text_search = row.Cells[3].Value.ToString();
                        webtype = row.Cells[4].Value.ToString();

                        // Load Browser
                        chromeBrowser.Load(domain);
                        textBox_domain.Text = domain;

                        Invoke(new Action(() =>
                        {
                            label_domainhide.Text = domain;
                            label_brandhide.Text = brand;
                            label_text_search.Text = text_search;
                            label_webtype.Text = webtype;
                            //label4.Text = currentIndex.ToString();
                        }));
                    }
                    catch (Exception)
                    {
                        //var st = new StackTrace(ex, true);
                        //var frame = st.GetFrame(0);
                        //var line = frame.GetFileLineNumber();
                        //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1016", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //Close();
                    }

                    
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
            string time = DateTime.Now.ToString("HH:mm");
            label_rtc.Text = date + " " + time;
            
            string datetime_folder = DateTime.Now.ToString("yyyy-MM-dd_");
            string replace = label_timefor.Text.Replace(":", "");
            label9.Text = datetime_folder + replace;
            
            string datetime = DateTime.Now.ToString("yyyy-MM-dd ");
            label11.Text = datetime + label_timefor.Text + ":00";
        }

        int timer_loader_uploaded = 0;
        int timer_loader_okay = 10;
        private bool buttonGoWasClicked;
        private bool buttonDetect;

        private void Timer_loader_Tick(object sender, EventArgs e)
        {
            timer_loader_uploaded++;
            timer_loader_okay -= 1;
            label6.Text = timer_loader_uploaded.ToString();

            button_okay.Text = "Okay ("+timer_loader_okay+")";

            if (timer_loader_uploaded == 1)
            {
                if (panel_main.Visible == true)
                {
                    label_status.Text = "[Uploading]";
                    
                    string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string path_other = path_desktop + "\\rainCheck\\result.txt";

                    if (File.Exists(path_other))
                    {
                        File.Delete(path_other);
                    }
                }
                else if (panel_urgent.Visible == true)
                {
                    label_status_urgent.Text = "[Uploading]";
                }
            }

            if (timer_loader_uploaded == 5)
            {
                if (panel_main.Visible == true)
                {
                    label_status.Text = "[Waiting]";
                }
                else if (panel_urgent.Visible == true)
                {
                    label_status_urgent.Text = "[Waiting]";
                }

                if (pagesource_history == "SUCCESS")
                {
                    label_uploadstatus.Text = "Upload Successful!";
                }
                else if (pagesource_history == "ERROR")
                {
                    label_uploadstatus.Text = "Upload Error!";
                }

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

                //button_start.Enabled = true;

                if (panel_main.Visible == true)
                {
                    if (start_detect_button == true)
                    {
                        button_start.Enabled = true;
                        button_start.PerformClick();
                        button_start.Enabled = false;

                        start_detect_button = false;
                    }

                    if (pagesource_history == "SUCCESS")
                    {
                        if (detectnohistoryyet)
                        {
                            dataGridView_history.Rows.Clear();
                            detectnohistoryyet = false;
                        }

                        // Last load
                        string path_last_load = Path.GetTempPath() + @"\raincheck_lastload.txt";
                        if (!File.Exists(path_last_load))
                        {
                            StreamWriter sw = new StreamWriter(path_last_load);
                            sw.Write(label_lastload.Text);
                            sw.Close();
                        }

                        string date_history = DateTime.Now.ToString("dd MMM ");
                        
                        if (dataGridView_history.RowCount == 12)
                        {
                            dataGridView_history.Rows.RemoveAt(12 - 1);
                        }

                        dataGridView_history.Rows.Insert(0, date_history + label_timeget.Text + " OK");

                        dataGridView_history.ClearSelection();

                        // Insert in temp file
                        try
                        {
                            dataGridView_history.CellBorderStyle = DataGridViewCellBorderStyle.Single;

                            string hex = "#438eb9";
                            Color color = ColorTranslator.FromHtml(hex);
                            dataGridView_history.DefaultCellStyle.SelectionBackColor = color;
                            dataGridView_history.DefaultCellStyle.SelectionForeColor = Color.White;

                            string path_history = Path.GetTempPath() + @"\raincheck_history.txt";
                            StreamWriter sw_create = new StreamWriter(path_history, true, Encoding.UTF8);
                            sw_create.Close();

                            string oldText = File.ReadAllText(path_history);
                            using (var sw = new StreamWriter(path_history, false, Encoding.UTF8))
                            {
                                sw.WriteLine(date_history + label_timeget.Text + " OK");
                                sw.WriteLine(oldText);
                                sw.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1016", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (pagesource_history == "ERROR")
                    {
                        if (detectnohistoryyet)
                        {
                            dataGridView_history.Rows.Clear();
                            detectnohistoryyet = false;
                        }

                        // Last load
                        string path_last_load = Path.GetTempPath() + @"\raincheck_lastload.txt";
                        if (!File.Exists(path_last_load))
                        {
                            StreamWriter sw = new StreamWriter(path_last_load);
                            sw.Write(label_lastload.Text);
                            sw.Close();
                        }

                        string date_history = DateTime.Now.ToString("dd MMM ");

                        if (dataGridView_history.RowCount == 12)
                        {
                            dataGridView_history.Rows.RemoveAt(12 - 1);
                        }

                        dataGridView_history.Rows.Insert(0, date_history + label_timeget.Text + " ERR");

                        dataGridView_history.ClearSelection();

                        // Insert in temp file
                        try
                        {
                            dataGridView_history.CellBorderStyle = DataGridViewCellBorderStyle.Single;

                            string hex = "#438eb9";
                            Color color = ColorTranslator.FromHtml(hex);
                            dataGridView_history.DefaultCellStyle.SelectionBackColor = color;
                            dataGridView_history.DefaultCellStyle.SelectionForeColor = Color.White;

                            string path_history = Path.GetTempPath() + @"\raincheck_history.txt";
                            StreamWriter sw_create = new StreamWriter(path_history, true, Encoding.UTF8);
                            sw_create.Close();

                            string oldText = File.ReadAllText(path_history);
                            using (var sw = new StreamWriter(path_history, false, Encoding.UTF8))
                            {
                                sw.WriteLine(date_history + label_timeget.Text + " ERR");
                                sw.WriteLine(oldText);
                                sw.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1016", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    label_timeget.Text = label_timefor.Text;

                    // Restart
                    string path_last_load_restart = Path.GetTempPath() + @"\raincheck_lastload.txt";
                    if (File.Exists(path_last_load_restart))
                    {
                        string last_load = File.ReadAllText(path_last_load_restart);

                        if (label_timefor.Text == last_load)
                        {
                            timefor = 0;
                            textchanged_timefor = false;

                            //string path_history = Path.GetTempPath() + @"\raincheck_history.txt";
                            string path_last_load = Path.GetTempPath() + @"\raincheck_lastload.txt";

                            //File.Delete(path_history);
                            File.Delete(path_last_load);

                            dr = MessageBox.Show("24 hours session done. Ready for the next session!", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (dr == DialogResult.OK)
                            {
                                can_close = false;
                                Application.Restart();
                            }
                        }
                    }
                }
                else if (panel_urgent.Visible == true)
                {
                    
                }
            }
        }

        private void Button_okay_Click(object sender, EventArgs e)
        {
            timer_loader_okay = 10;
            timer_loader_uploaded = 0;
            timer_loader.Stop();

            panel_uploaded.Visible = false;

            //button_start.Enabled = true;

            if (panel_main.Visible == true)
            {
                if (start_detect_button == true)
                {
                    button_start.Enabled = true;
                    button_start.PerformClick();
                    button_start.Enabled = false;

                    start_detect_button = false;
                }

                if (pagesource_history == "SUCCESS")
                {
                    if (detectnohistoryyet)
                    {
                        dataGridView_history.Rows.Clear();
                        detectnohistoryyet = false;
                    }

                    // Last load
                    string path_last_load = Path.GetTempPath() + @"\raincheck_lastload.txt";
                    if (!File.Exists(path_last_load))
                    {
                        StreamWriter sw = new StreamWriter(path_last_load);
                        sw.Write(label_lastload.Text);
                        sw.Close();
                    }

                    string date_history = DateTime.Now.ToString("dd MMM ");

                    if (dataGridView_history.RowCount == 12)
                    {
                        dataGridView_history.Rows.RemoveAt(12 - 1);
                    }

                    dataGridView_history.Rows.Insert(0, date_history + label_timeget.Text + " OK");

                    dataGridView_history.ClearSelection();

                    // Insert in temp file
                    try
                    {
                        dataGridView_history.CellBorderStyle = DataGridViewCellBorderStyle.Single;

                        string hex = "#438eb9";
                        Color color = ColorTranslator.FromHtml(hex);
                        dataGridView_history.DefaultCellStyle.SelectionBackColor = color;
                        dataGridView_history.DefaultCellStyle.SelectionForeColor = Color.White;

                        string path_history = Path.GetTempPath() + @"\raincheck_history.txt";
                        StreamWriter sw_create = new StreamWriter(path_history, true, Encoding.UTF8);
                        sw_create.Close();

                        string oldText = File.ReadAllText(path_history);
                        using (var sw = new StreamWriter(path_history, false, Encoding.UTF8))
                        {
                            sw.WriteLine(date_history + label_timeget.Text + " OK");
                            sw.WriteLine(oldText);
                            sw.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1016", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (pagesource_history == "ERROR")
                {
                    if (detectnohistoryyet)
                    {
                        dataGridView_history.Rows.Clear();
                        detectnohistoryyet = false;
                    }

                    // Last load
                    string path_last_load = Path.GetTempPath() + @"\raincheck_lastload.txt";
                    if (!File.Exists(path_last_load))
                    {
                        StreamWriter sw = new StreamWriter(path_last_load);
                        sw.Write(label_lastload.Text);
                        sw.Close();
                    }

                    string date_history = DateTime.Now.ToString("dd MMM ");

                    if (dataGridView_history.RowCount == 12)
                    {
                        dataGridView_history.Rows.RemoveAt(12 - 1);
                    }

                    dataGridView_history.Rows.Insert(0, date_history + label_timeget.Text + " ERR");

                    dataGridView_history.ClearSelection();

                    // Insert in temp file
                    try
                    {
                        dataGridView_history.CellBorderStyle = DataGridViewCellBorderStyle.Single;

                        string hex = "#438eb9";
                        Color color = ColorTranslator.FromHtml(hex);
                        dataGridView_history.DefaultCellStyle.SelectionBackColor = color;
                        dataGridView_history.DefaultCellStyle.SelectionForeColor = Color.White;

                        string path_history = Path.GetTempPath() + @"\raincheck_history.txt";
                        StreamWriter sw_create = new StreamWriter(path_history, true, Encoding.UTF8);
                        sw_create.Close();

                        string oldText = File.ReadAllText(path_history);
                        using (var sw = new StreamWriter(path_history, false, Encoding.UTF8))
                        {
                            sw.WriteLine(date_history + label_timeget.Text + " ERR");
                            sw.WriteLine(oldText);
                            sw.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1016", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                label_timeget.Text = label_timefor.Text;

                // Restart
                string path_last_load_restart = Path.GetTempPath() + @"\raincheck_lastload.txt";
                if (File.Exists(path_last_load_restart))
                {
                    string last_load = File.ReadAllText(path_last_load_restart);

                    if (label_timefor.Text == last_load)
                    {
                        timefor = 0;
                        textchanged_timefor = false;

                        //string path_history = Path.GetTempPath() + @"\raincheck_history.txt";
                        string path_last_load = Path.GetTempPath() + @"\raincheck_lastload.txt";

                        //File.Delete(path_history);
                        File.Delete(path_last_load);

                        dr = MessageBox.Show("24 hours session done. Ready for the next session!", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (dr == DialogResult.OK)
                        {
                            can_close = false;
                            Application.Restart();
                        }
                    }
                }
            }
            else if (panel_urgent.Visible == true)
            {

            }
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

            //Rectangle bounds = Bounds;
            //using (Bitmap bitmap = new Bitmap(bounds.Width-267, bounds.Height-202))
            //{
            //    using (Graphics g = Graphics.FromImage(bitmap))
            //    {
            //        g.CopyFromScreen(new Point(bounds.Left+226, bounds.Top+159), Point.Empty, bounds.Size);
            //    }

            //    Bitmap resized = new Bitmap(bitmap, new Size(bitmap.Width / 2, bitmap.Height / 2));
            //    string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //    resized.Save(path_desktop + "\\" + label_macid.Text + "_" + label_domainhide.Text + ".jpeg", ImageFormat.Jpeg);
            //}

            string path_last_load = Path.GetTempPath();
            textBox_webtitle.Text = path_last_load;

            int domain_total = dataGridView_urgent.RowCount;
            MessageBox.Show(domain_total.ToString());

            //int upload = 1;
            //while (upload <= 5)
            //{
            //    string datetime_folder = label9.Text;
            //    string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //    string path = path_desktop + "\\rainCheck\\" + datetime_folder;

            //    // Insert
            //    string read = File.ReadAllText(path + "\\result.txt");

            //    StringBuilder sb = new StringBuilder();
            //    using (var p = ChoCSVReader.LoadText(read).WithFirstLineHeader())
            //    {
            //        using (var w = new ChoJSONWriter(sb))
            //        {
            //            w.Write(p);
            //        }
            //    }

            //    try
            //    {
            //        using (var client = new WebClient())
            //        {
            //            string auth = "r@inCh3ckd234b70";
            //            string type = "reports_normal";
            //            string request = "http://raincheck.ssitex.com/api/api.php";
            //            string reports = sb.ToString();

            //            NameValueCollection postData = new NameValueCollection()
            //                {
            //                    { "auth", auth },
            //                    { "type", type },
            //                    { "reports", reports },
            //                };

            //            pagesource_history = Encoding.UTF8.GetString(client.UploadValues(request, postData));

            //            if (pagesource_history == "SUCCESS")
            //            {
            //                break;
            //            }
            //        }
            //    }
            //    catch (Exception)
            //    {
            //        upload++;

            //        label_uploadstatus.Text = "Upload Error!";
            //    }
            //}

            //if (panel_main.Visible == true)
            //{
            //    MessageBox.Show("main visible");
            //}
            //else if (panel_urgent.Visible == true)
            //{
            //    MessageBox.Show("urgent visible");
            //}

            //MessageBox.Show("ok!");
        }

        private void Button_urgent_Click(object sender, EventArgs e)
        {
            //Form_Urgent form_urgent = new Form_Urgent();
            //form_urgent.ShowDialog();
            
            panel_urgent.Visible = true;

            button_urgent.Visible = false;

            panel_main.Visible = false;

            label_domainscount.Visible = false;

            label_back.Visible = true;
            label_domain_urgent.Visible = true;
            
            textBox_domain_urgent.Text = "";
        }

        private void Label_back_Click(object sender, EventArgs e)
        {
            if (label_status_urgent.Text == "[Waiting]")
            {
                panel_urgent.Visible = false;

                button_urgent.Visible = true;
                panel_main.Visible = true;

                label_domainscount.Visible = true;

                label_back.Visible = false;
                label_domain_urgent.Visible = false;
                label_domainscount_urgent.Visible = false;

                textBox_domain.Text = "";
            }
            else
            {
                MessageBox.Show("Please wait until the process is finish. Thank you!", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Button_domain_urgent_Click(object sender, EventArgs e)
        {
            if (label_status_urgent.Text == "[Waiting]")
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
                        dataGridView_urgent.ClearSelection();

                        dataGridView_urgent.CellBorderStyle = DataGridViewCellBorderStyle.Single;

                        string hex = "#438eb9";
                        Color color = ColorTranslator.FromHtml(hex);
                        dataGridView_urgent.DefaultCellStyle.SelectionBackColor = color;
                        dataGridView_urgent.DefaultCellStyle.SelectionForeColor = Color.White;

                        while (dataGridView_urgent.Rows.Count > 0)
                        {
                            dataGridView_urgent.Rows.RemoveAt(0);
                        }

                        StreamReader streamReader = new StreamReader(ofd.FileName);
                        string domain_urgent = "";
                        for (domain_urgent = streamReader.ReadLine(); domain_urgent != null; domain_urgent = streamReader.ReadLine())
                        {   
                            using (var client = new WebClient())
                            {
                                string auth = "r@inCh3ckd234b70";
                                string type = "brand";
                                string request = "http://raincheck.ssitex.com/api/api.php";
                                string domain = domain_urgent;

                                NameValueCollection postData = new NameValueCollection()
                                {
                                    { "auth", auth },
                                    { "type", type },
                                    { "domain", domain }
                                };

                                string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));

                                if (pagesource != "")
                                {
                                    JArray jsonObject = JArray.Parse(pagesource);

                                    string brand_name = jsonObject[0]["brand_name"].Value<string>();
                                    string text_search = jsonObject[0]["text_search"].Value<string>();
                                    string website_type = jsonObject[0]["website_type"].Value<string>();

                                    label_brand_id.Text = brand_name;
                                    label_text_search_urgent.Text = text_search;
                                    label_webtype.Text = website_type;

                                    //MessageBox.Show(brand_name + " " + text_search);
                                }
                                else
                                {
                                    label_brand_id.Text = "";

                                    Form_Brand form_brand = new Form_Brand(domain_urgent);
                                    form_brand.ShowDialog();

                                    label_brand_id.Text = SetValueForTextBrandID;
                                    label_text_search_urgent.Text = SetValueForTextSearch;
                                    label_webtype.Text = SetValueForWebsiteType;
                                }
                            }                            

                            dataGridView_urgent.ClearSelection();
                            dataGridView_urgent.Rows.Add(domain_urgent, label_brand_id.Text, label_text_search_urgent.Text, label_webtype.Text);
                        }

                        button_start_urgent.Enabled = true;
                                                      
                        label_brand_id.Text = "";

                        label_domainscount_urgent.Visible = true;
                        label_domainscount_urgent.Text = "Total: " + dataGridView_urgent.RowCount.ToString();

                        streamReader.Close();
                    }
                    catch (Exception ex)
                    {
                        var st = new StackTrace(ex, true);
                        var frame = st.GetFrame(0);
                        var line = frame.GetFileLineNumber();
                        MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1018", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //Close();
                    }

                    if (dataGridView_urgent.Rows.Count > 0)
                    {
                        label_clear.Visible = true;
                        label_help.Visible = false;
                    }

                    //dataGridView_urgent.DataSource = dt;
                }
            }
            else
            {
                MessageBox.Show("Please wait until the process is finish. Thank you!", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Label_clear_Click(object sender, EventArgs e)
        {
            if (label_status_urgent.Text == "[Waiting]")
            {
                while (dataGridView_urgent.Rows.Count > 0)
                {
                    dataGridView_urgent.Rows.RemoveAt(0);
                }

                button_start_urgent.Enabled = false;
                label_clear.Visible = false;
                label_help.Visible = true;
                label_domainscount_urgent.Visible = false;

                dataGridView_urgent.Rows.Add("No data available in table");
                dataGridView_urgent.ClearSelection();
                dataGridView_urgent.CellBorderStyle = DataGridViewCellBorderStyle.None;
                dataGridView_urgent.DefaultCellStyle.SelectionBackColor = dataGridView_urgent.DefaultCellStyle.BackColor;
                dataGridView_urgent.DefaultCellStyle.SelectionForeColor = dataGridView_urgent.DefaultCellStyle.ForeColor;
            }
            else
            {
                MessageBox.Show("Please wait until the process is finish. Thank you!", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, ref string newUrl)
        {

        }


        private void Button_start_urgent_Click(object sender, EventArgs e)
        {
            TopMost = true;
            MinimizeBox = false;
            
            pictureBox_loader_urgent.Visible = true;

            // Set browser panel dock style
            panel_browser_urgent.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;

            //if (label9.Text == "")
            //{
            //    label9.Text = label9.Text;
            //}

            //if (label11.Text == "")
            //{
            //    label11.Text = label11.Text;
            //    MessageBox.Show(label11.Text);
            //}

            label_domainscount_urgent.Text = "Total: " + (index_urgent + 1) + " of " + dataGridView_urgent.RowCount.ToString();

            timer_blink_urgent.Stop();
            label_status_urgent.Visible = true;
            label_status_urgent.Text = "[Running]";
            timer_domain_urgent.Start();

            int getCurrentIndex = Convert.ToInt32(label_currentindex_urgent.Text);
            dataGridView_urgent.ClearSelection();
            dataGridView_urgent.Rows[getCurrentIndex].Selected = true;

            // For timeout
            i_urgent = 1;
            timer_timeout_urgent.Start();

            button_pause_urgent.Visible = true;
            button_start_urgent.Visible = false;

            label_inaccessible_error_message.Text = "";

            button_startover_urgent.Enabled = true;

            buttonDetect = true;
        }
        
        private void Button_pause_urgent_Click(object sender, EventArgs e)
        {
            TopMost = false;
            MinimizeBox = true;

            // Set browser panel dock style
            chromeBrowser.Dock = DockStyle.None;

            timer_blink_urgent.Start();
            label_status_urgent.Text = "[Paused]";
            timer_domain_urgent.Stop();
            timer_timeout_urgent.Stop();
            pictureBox_loader_urgent.Visible = false;

            button_pause_urgent.Visible = false;
            button_start_urgent.Visible = true;
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
                    string text_search;
                    string webtype;

                    if (String.IsNullOrEmpty(dataGridView_urgent.Rows[0].Cells[1].Value as String))
                    {
                        return;
                    }
                    else
                    {
                        brand = row.Cells[1].Value.ToString();
                    }

                    if (String.IsNullOrEmpty(dataGridView_urgent.Rows[0].Cells[2].Value as String))
                    {
                        return;
                    }
                    else
                    {
                        text_search = row.Cells[2].Value.ToString();
                    }

                    if (String.IsNullOrEmpty(dataGridView_urgent.Rows[0].Cells[3].Value as String))
                    {
                        return;
                    }
                    else
                    {
                        webtype = row.Cells[3].Value.ToString();
                    }


                    //currentIndex = dataGridView_domain.CurrentRow.Index;

                    try
                    {
                        // Load Browser
                        chromeBrowser.Load(domain);
                    }
                    catch (Exception ex)
                    {
                        var st = new StackTrace(ex, true);
                        var frame = st.GetFrame(0);
                        var line = frame.GetFileLineNumber();
                        MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1019", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //Close();
                    }

                    Invoke(new Action(() =>
                    {
                        label_domainhide_urgent.Text = domain;
                        label_brandhide_urgent.Text = brand;
                        label_text_search_urgent.Text = text_search;
                        label_webtype_urgent.Text = webtype;
                        //label4.Text = currentIndex.ToString();
                    }));
                }
            }
        }

        int domain_urgent = 0;
        private void Timer_domain_urgent_Tick(object sender, EventArgs e)
        {
            label_timerstartpause_urgent.Text = domain_urgent++.ToString();
        }

        private void Label_ifloadornot_urgent_TextChanged(object sender, EventArgs e)
        {
            if (label_ifloadornot_urgent.Text == "0")
            {
                label_webtitle_urgent.Text = "";
                int domain_total = dataGridView_urgent.RowCount;
                
                label_domainscount_urgent.Text = "Total: " + (index_urgent + 2) + " of " + domain_total.ToString();

                index_urgent = dataGridView_urgent.SelectedRows[0].Index + 1;
                label_currentindex_urgent.Text = index_urgent.ToString();
                
                if (index_urgent == domain_total)
                {
                    // Set browser panel dock style
                    chromeBrowser.Dock = DockStyle.None;
                    textBox_domain.Text = "";

                    dataGridView_urgent.ClearSelection();
                    
                    index_urgent = 0;
                    label_domainscount_urgent.Text = "Total: " + domain_total.ToString();

                    // else loaded
                    elseloaded_i = 0;
                    timer_elseloaded.Stop();

                    // Detect when stop loads
                    detectnotloading = 0;
                    timer_detectnotloading.Stop();

                    //ms_detect = 0;
                    fully_loaded = 0;
                    start_detect = 0;

                    string datetime_folder = label9.Text;
                    string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                    string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder + "_urgent_" + i_timeout;


                    // Insert
                    string read = File.ReadAllText(path + "\\result.txt");

                    //MessageBox.Show(read);

                    StringBuilder sb = new StringBuilder();
                    using (var p = ChoCSVReader.LoadText(read).WithFirstLineHeader())
                    {
                        using (var w = new ChoJSONWriter(sb))
                        {
                            w.Write(p);
                        }
                    }

                    //MessageBox.Show(sb.ToString());
                    int upload = 1;
                    while (upload <= 5)
                    {
                        try
                        {
                            using (var client = new WebClient())
                            {
                                string auth = "r@inCh3ckd234b70";
                                string type = "reports_normal";
                                string request = "http://raincheck.ssitex.com/api/api.php";
                                string reports = sb.ToString();

                                NameValueCollection postData = new NameValueCollection()
                                {
                                    { "auth", auth },
                                    { "type", type },
                                    { "reports", reports },
                                };

                                string pagesource_history = Encoding.UTF8.GetString(client.UploadValues(request, postData));

                                if (pagesource_history == "SUCCESS")
                                {
                                    break;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            upload++;

                            label_uploadstatus.Text = "Upload Error!";
                        }
                    }

                    // Upload zip file urgent
                    //try
                    //{
                    //    FtpWebRequest req = (FtpWebRequest)WebRequest.Create("ftp://raincheck.ssitex.com/public/zip/" + datetime_folder);
                    //    req.UseBinary = true;
                    //    req.Method = WebRequestMethods.Ftp.UploadFile;
                    //    req.Credentials = new NetworkCredential("ftpuser@hades.ssitex.com", "p0w3r@SSI");
                    //    byte[] fileData = File.ReadAllBytes(outputpath);

                    //    req.ContentLength = fileData.Length;
                    //    Stream reqStream = req.GetRequestStream();
                    //    reqStream.Write(fileData, 0, fileData.Length);
                    //    reqStream.Close();
                    //}
                    //catch (Exception ex)
                    //{
                    //    MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1030", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}

                    // Enable visible buttons
                    button_start_urgent.Visible = true;
                    button_pause_urgent.Visible = false;
                    button_startover_urgent.Enabled = false;
                    pictureBox_loader_urgent.Visible = false;
                                                            
                    label_currentindex_urgent.Text = "0";

                    label_status_urgent.Text = "[Loading]";
                    timer_domain_urgent.Stop();

                    TopMost = false;
                    MinimizeBox = true;

                    // Timer Urgent
                    domain_urgent = 0;
                    i_timeout++;

                    panel_loader.Visible = true;
                    panel_loader.BringToFront();
                    timer_loader.Start();
                }
                else
                {
                    dataGridView_urgent.FirstDisplayedScrollingRowIndex = index_urgent;
                    dataGridView_urgent.Rows[index_urgent].Selected = true;
                }
            }
        }

        private void Timer_blink_urgent_Tick(object sender, EventArgs e)
        {
            label_status_urgent.Visible = !label_status_urgent.Visible;
        }

        int i_urgent = 0;
        private void Timer_timeout_urgent_Tick(object sender, EventArgs e)
        {
            if (InvokeRequired) { Invoke(new Action(() => { Timer_timeout_urgent_Tick(sender, e); })); return; }
            label_timeoutcount_urgent.Text = i_urgent++.ToString();

            if (label_timeoutcount_urgent.Text == label13.Text)
            {
                chromeBrowser.Stop();
                label_timeout_urgent.Text = "timeout";
            }
        }

        private void Timer_new_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show("voila!");

            string datetime = label11.Text;
            string datetime_folder = label9.Text;
            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

            if (File.Exists(path + "_" + label_domainhide.Text + ".jpeg"))
            {
                timer_new.Stop();

                Invoke(new Action(() =>
                {
                    //MessageBox.Show("ok " + path + "_" + label_domainhide.Text + ".jpeg");
                    label_ifloadornot.Text = "0";
                }));
            }
        }

        int elseloaded_i = 0;
        private void timer_elseloaded_Tick(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                elseloaded_i++;
                label18.Text = elseloaded_i.ToString();
                if (elseloaded_i++ == 1)
                {
                    fully_loaded = 0;
                    start_detect = 0;

                    if (panel_main.Visible == true)
                    {
                        label_ifloadornot.Text = "0";
                    }
                    else if (panel_urgent.Visible == true)
                    {
                        label_ifloadornot_urgent.Text = "0";
                    }
                }
            }));
        }

        private void button_getmaindomains_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(start_get.ToString());

            //string myDate = "21/07/2018 00:00:00";
            //DateTime dt1 = DateTime.ParseExact(myDate, "dd/MM/yyyy HH:mm:ss",
            //                                           CultureInfo.InvariantCulture);

            //string start_myDate = "21/07/2018 23:00:00";
            //DateTime start = DateTime.ParseExact(start_myDate, "dd/MM/yyyy HH:mm:ss",
            //                                           CultureInfo.InvariantCulture);

            //TimeSpan difference = dt1 - start;
            //int hrs = difference.Hours;
            //int mins = difference.Minutes;
            //int secs = difference.Seconds;

            //TimeSpan spinTime = new TimeSpan(hrs, mins, secs);

            //TimeSpan delta = DateTime.Now - start;
            //TimeSpan timeRemaining = spinTime - delta;


            //string mins_view;
            //if (timeRemaining.Minutes == 0 || timeRemaining.Minutes == 1)
            //{
            //    mins_view = " min ";
            //}
            //else
            //{
            //    mins_view = " mins ";
            //}

            //string secs_view;
            //if (timeRemaining.Seconds == 0 || timeRemaining.Seconds == 1)
            //{
            //    secs_view = " sec";
            //}
            //else
            //{
            //    secs_view = " secs";
            //}

            //if (timeRemaining.Hours != 0 && timeRemaining.Minutes != 0)
            //{
            //    label_cycle_in.Text = timeRemaining.Hours + " hr " + timeRemaining.Minutes + mins_view;
            //}
            //else if (timeRemaining.Hours == 0 && timeRemaining.Minutes == 0)
            //{
            //    label_cycle_in.Text = timeRemaining.Seconds + secs_view;
            //}
            //else if (timeRemaining.Hours == 0)
            //{
            //    label_cycle_in.Text = timeRemaining.Minutes + mins_view + timeRemaining.Seconds + secs_view;
            //}

            Random rnd = new Random();
            int month = rnd.Next(1, 1000);
            label_timefor.Text = month.ToString();

            //timerfornext = true;
            //string time = textBox1.Text;
            //label_timer_timefor.Text = time;

            //string result = time.Replace(":", ".");

            //if (Convert.ToDouble(result) >= 0 && Convert.ToDouble(result) <= 1.59)
            //{
            //    MessageBox.Show("00:00");
            //}
            //else if (Convert.ToDouble(result) >= 2 && Convert.ToDouble(result) <= 3.59)
            //{
            //    MessageBox.Show("02:00");
            //}
            //else if (Convert.ToDouble(result) >= 4 && Convert.ToDouble(result) <= 5.59)
            //{
            //    MessageBox.Show("04:00");
            //}
            //else if (Convert.ToDouble(result) >= 6 && Convert.ToDouble(result) <= 7.59)
            //{
            //    MessageBox.Show("06:00");
            //}
            //else if (Convert.ToDouble(result) >= 8 && Convert.ToDouble(result) <= 9.59)
            //{
            //    MessageBox.Show("08:00");
            //}
            //else if (Convert.ToDouble(result) >= 10 && Convert.ToDouble(result) <= 11.59)
            //{
            //    MessageBox.Show("10:00");
            //}
            //else if (Convert.ToDouble(result) >= 12 && Convert.ToDouble(result) <= 13.59)
            //{
            //    MessageBox.Show("12:00");
            //}
            //else if(Convert.ToDouble(result) >= 14 && Convert.ToDouble(result) <= 15.59)
            //{
            //    MessageBox.Show("14:00");
            //}
            //else if (Convert.ToDouble(result) >= 16 && Convert.ToDouble(result) <= 17.59)
            //{
            //    MessageBox.Show("16:00");
            //}
            //else if (Convert.ToDouble(result) >= 18 && Convert.ToDouble(result) <= 19.59)
            //{
            //    MessageBox.Show("18:00");
            //}
            //else if (Convert.ToDouble(result) >= 20 && Convert.ToDouble(result) <= 21.59)
            //{
            //    MessageBox.Show("20:00");
            //}
            //else if (Convert.ToDouble(result) >= 22 && Convert.ToDouble(result) <= 23.59)
            //{
            //    MessageBox.Show("22:00");
            //}



            // Balloon Notification
            //var notification = new NotifyIcon()
            //{
            //    Visible = true,
            //    Icon = SystemIcons.Information,
            //    BalloonTipIcon = ToolTipIcon.Info,
            //    BalloonTipTitle = "Information",
            //    BalloonTipText = "Jul 18 12:00 is already done.",
            //};

            //notification.ShowBalloonTip(1000);

            //string result = "";
            //string search_replace = "天發娱乐城";

            //string upper_search = search_replace.ToUpper().ToString();

            //StringBuilder sb = new StringBuilder(upper_search);
            //sb.Replace("-", "");
            //sb.Replace(".", "");
            //string final_search = Regex.Replace(sb.ToString(), " {2,}", " ");

            //var final_inaccessble_lists = inaccessble_lists.Select(m => m.ToUpper());            

            //string[] words = final_search.Split(' ');
            //foreach (string word in words)
            //{
            //    var match = final_inaccessble_lists.FirstOrDefault(stringToCheck => stringToCheck.Contains(word));

            //    if (match != null)
            //    {
            //        result = "match";
            //        break;
            //    }
            //    else
            //    {
            //        result = "no match";
            //    }
            //}

            //if (result == "match")
            //{
            //    MessageBox.Show("booom match");
            //}
            //else
            //{
            //    MessageBox.Show("booom no match");
            //}
        }

        private void APIGetDomains()
        {
            //try
            //{
            //    using (var client = new WebClient())
            //    {
            //        string auth = "r@inCh3ckd234b70";
            //        string type = "domain_main";
            //        string request = "http://raincheck.ssitex.com/api/api.php";

            //        NameValueCollection postData = new NameValueCollection()
            //        {
            //            { "auth", auth },
            //            { "type", type }
            //        };

            //        string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));

            //        var arr = JsonConvert.DeserializeObject<JArray>(pagesource);

            //        dataGridView_domain.DataSource = arr;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    var st = new StackTrace(ex, true);
            //    var frame = st.GetFrame(0);
            //    var line = frame.GetFileLineNumber();
            //    MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1022", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //    //Close();
            //}

            try
            {
                using (var client = new WebClient())
                {
                    string auth = "r@inCh3ckd234b70";
                    string type = "domain_main_test";
                    string request = "http://raincheck.ssitex.com/api/api.php";

                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "auth", auth },
                        { "type", type }
                    };

                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));
                    var arr = JsonConvert.DeserializeObject<JArray>(pagesource);

                    dataGridView_domain.DataSource = arr;
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1022", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Close();
            }
        }

        int detectnotloading = 0;
        private void Timer_detectnotloading_Tick(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                detectnotloading++;
                label_detectnotloading.Text = detectnotloading.ToString();

                if (detectnotloading > 20)
                {
                    if (panel_main.Visible == true)
                    {
                        //MessageBox.Show("loading now");

                        int getCurrentIndex = Convert.ToInt32(label_currentindex.Text);
                        dataGridView_domain.ClearSelection();

                        // For timeout
                        i = 1;
                        timer_timeout.Start();

                        dataGridView_domain.Rows[getCurrentIndex].Selected = true;

                        detectnotloading = 0;
                        timer_detectnotloading.Stop();
                    }
                    else if (panel_urgent.Visible == true)
                    {
                        //MessageBox.Show("loading now");

                        int getCurrentIndex = Convert.ToInt32(label_currentindex_urgent.Text);
                        dataGridView_urgent.ClearSelection();

                        // For timeout
                        i = 1;
                        timer_timeout_urgent.Start();

                        dataGridView_urgent.Rows[getCurrentIndex].Selected = true;

                        detectnotloading = 0;
                        timer_detectnotloading.Stop();
                    }
                }
            }));
        }
        
        private void timer_timefor_Tick(object sender, EventArgs e)
        {            
            string time = DateTime.Now.ToString("HH:mm");
            label_timer_timefor.Text = time;

            string result = time.Replace(":", ".");

            if (Convert.ToDouble(result) >= 0 && Convert.ToDouble(result) <= 1.59)
            {
                label_timefor.Text = "00:00";
                label_timefor_urgent.Text = "00:00";
                label_cyclein_get.Text = DateTime.Now.ToString("dd/MM/yyyy 02:00:00");
                label_lastload.Text = "22:00";
            }
            else if (Convert.ToDouble(result) >= 2 && Convert.ToDouble(result) <= 3.59)
            {
                label_timefor.Text = "02:00";
                label_timefor_urgent.Text = "02:00";
                label_cyclein_get.Text = DateTime.Now.ToString("dd/MM/yyyy 04:00:00");
                label_lastload.Text = "00:00";
            }
            else if (Convert.ToDouble(result) >= 4 && Convert.ToDouble(result) <= 5.59)
            {
                label_timefor.Text = "04:00";
                label_timefor_urgent.Text = "04:00";
                label_cyclein_get.Text = DateTime.Now.ToString("dd/MM/yyyy 06:00:00");
                label_lastload.Text = "02:00";
            }
            else if (Convert.ToDouble(result) >= 6 && Convert.ToDouble(result) <= 7.59)
            {
                label_timefor.Text = "06:00";
                label_timefor_urgent.Text = "06:00";
                label_cyclein_get.Text = DateTime.Now.ToString("dd/MM/yyyy 08:00:00");
                label_lastload.Text = "04:00";
            }
            else if (Convert.ToDouble(result) >= 8 && Convert.ToDouble(result) <= 9.59)
            {
                label_timefor.Text = "08:00";
                label_timefor_urgent.Text = "08:00";
                label_cyclein_get.Text = DateTime.Now.ToString("dd/MM/yyyy 10:00:00");
                label_lastload.Text = "06:00";
            }
            else if (Convert.ToDouble(result) >= 10 && Convert.ToDouble(result) <= 11.59)
            {
                label_timefor.Text = "10:00";
                label_timefor_urgent.Text = "10:00";
                label_cyclein_get.Text = DateTime.Now.ToString("dd/MM/yyyy 12:00:00");
                label_lastload.Text = "08:00";
            }
            else if (Convert.ToDouble(result) >= 12 && Convert.ToDouble(result) <= 13.59)
            {
                label_timefor.Text = "12:00";
                label_timefor_urgent.Text = "12:00";
                label_cyclein_get.Text = DateTime.Now.ToString("dd/MM/yyyy 14:00:00");
                label_lastload.Text = "10:00";
            }
            else if (Convert.ToDouble(result) >= 14 && Convert.ToDouble(result) <= 15.59)
            {
                label_timefor.Text = "14:00";
                label_timefor_urgent.Text = "14:00";
                label_cyclein_get.Text = DateTime.Now.ToString("dd/MM/yyyy 16:00:00");
                label_lastload.Text = "12:00";
            }
            else if (Convert.ToDouble(result) >= 16 && Convert.ToDouble(result) <= 17.59)
            {
                label_timefor.Text = "16:00";
                label_timefor_urgent.Text = "16:00";
                label_cyclein_get.Text = DateTime.Now.ToString("dd/MM/yyyy 18:00:00");
                label_lastload.Text = "14:00";
            }
            else if (Convert.ToDouble(result) >= 18 && Convert.ToDouble(result) <= 19.59)
            {
                label_timefor.Text = "18:00";
                label_timefor_urgent.Text = "18:00";
                label_cyclein_get.Text = DateTime.Now.ToString("dd/MM/yyyy 20:00:00");
                label_lastload.Text = "16:00";
            }
            else if (Convert.ToDouble(result) >= 20 && Convert.ToDouble(result) <= 21.59)
            {
                label_timefor.Text = "20:00";
                label_timefor_urgent.Text = "20:00";
                label_cyclein_get.Text = DateTime.Now.ToString("dd/MM/yyyy 22:00:00");
                label_lastload.Text = "18:00";
            }
            else if (Convert.ToDouble(result) >= 22 && Convert.ToDouble(result) <= 23.59)
            {
                label_timefor.Text = "22:00";
                label_timefor_urgent.Text = "22:00";
                DateTime date_parse = DateTime.Now.AddDays(1);
                string date = date_parse.ToString("dd/MM/yyyy");

                label_cyclein_get.Text = date + " 00:00:00";
                label_lastload.Text = "20:00";
            }
        }

        string start_get = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        private void Timer_cyclein_Tick(object sender, EventArgs e)
        {
            string cyclein_parse = label_cyclein_get.Text;
            DateTime cyclein = DateTime.ParseExact(cyclein_parse, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            string start_parse = start_get;
            DateTime start = DateTime.ParseExact(start_parse, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            TimeSpan difference = cyclein - start;
            int hrs = difference.Hours;
            int mins = difference.Minutes;
            int secs = difference.Seconds;

            TimeSpan spinTime = new TimeSpan(hrs, mins, secs);

            TimeSpan delta = DateTime.Now - start;
            TimeSpan timeRemaining = spinTime - delta;
            
            string mins_view;
            if (timeRemaining.Minutes == 0 || timeRemaining.Minutes == 1)
            {
                mins_view = " min ";
            }
            else
            {
                mins_view = " mins ";
            }

            string secs_view;
            if (timeRemaining.Seconds == 0 || timeRemaining.Seconds == 1)
            {
                secs_view = " sec";
            }
            else
            {
                secs_view = " secs";
            }

            if (timeRemaining.Hours != 0 && timeRemaining.Minutes != 0)
            {
                label_cycle_in.Text = timeRemaining.Hours + " hr " + timeRemaining.Minutes + mins_view;
                label_cyclein_urgent.Text = timeRemaining.Hours + " hr " + timeRemaining.Minutes + mins_view;
            }
            else if (timeRemaining.Hours == 0 && timeRemaining.Minutes == 0)
            {
                label_cycle_in.Text = timeRemaining.Seconds + secs_view;
                label_cyclein_urgent.Text = timeRemaining.Seconds + secs_view;
            }
            else if (timeRemaining.Hours == 0)
            {
                label_cycle_in.Text = timeRemaining.Minutes + mins_view + timeRemaining.Seconds + secs_view;
                label_cyclein_urgent.Text = timeRemaining.Minutes + mins_view + timeRemaining.Seconds + secs_view;
            }
        }

        int timefor = 0;
        int detect_start = 0;
        private bool textchanged_timefor = false;
        private int domain_total;
        private int index;
        private bool timerfornext = false;
        private bool start_detect_button = false;
        private string pagesource_history;
        private bool detectnohistoryyet = false;
        private DialogResult dr;
        private bool can_close = true;
        private bool auto_start = true;
        private int index_urgent;

        private void label_timefor_TextChanged(object sender, EventArgs e)
        {
            // Auto start the checking if label time for is not exists in history
            //string path = Path.GetTempPath() + @"\raincheck_history.txt";
            //if (File.Exists(path))
            //{
            //    detect_start++;

            //    if (detect_start == 1)
            //    {
            //        string read = File.ReadAllText(path);

            //        if (read.Contains(label_timefor.Text))
            //        {
            //            button_start.Enabled = false;
            //        }
            //        else
            //        {
            //            label_timeget.Text = label_timefor.Text;
            //            button_start.Enabled = true;
            //            button_start.PerformClick();
            //            button_start.Enabled = false;
            //            auto_start = false;
            //        }
            //    }
            //}       
                        
            if (label_status.Text != "[Running]")
            {
                label_timeget.Text = label_timefor.Text;
            }

            if (textchanged_timefor == true)
            {
                timefor++;
            }

            label_textchangedtimefor.Text = timefor.ToString();

            if (timefor > 0)
            {
                pictureBox_loader.Visible = false;

                if (label_status.Text != "[Waiting]")
                {
                    if (auto_start)
                    {
                        timerfornext = true;
                        label_ifloadornot.Text = "0";

                        timer_blink.Stop();
                        label_status.Visible = true;
                        label_status.Text = "[Running]";

                        start_detect_button = true;

                        chromeBrowser.Stop();
                    }

                    auto_start = true;
                }
                else
                {
                    button_start.Enabled = true;
                    button_start.PerformClick();
                    button_start.Enabled = false;
                }
            }
        }

        private void label_inaccessible_error_message_TextChanged(object sender, EventArgs e)
        {
            if (label_inaccessible_error_message.Text == "ERR_INTERNET_DISCONNECTED")
            {
                timer_domain.Stop();
                chromeBrowser.Stop();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string path = path_desktop + "\\2018-07-23_1600_e0d55e8ec0ae_u_dasdsadsa.com.zip";
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create("ftp://raincheck.ssitex.com/public/zip/" + "zip_test");
                req.UseBinary = true;
                req.Method = WebRequestMethods.Ftp.UploadFile;
                req.Credentials = new NetworkCredential("ftpuser@hades.ssitex.com", "p0w3r@SSI");
                byte[] fileData = File.ReadAllBytes(path);

                req.ContentLength = fileData.Length;
                Stream reqStream = req.GetRequestStream();
                reqStream.Write(fileData, 0, fileData.Length);
                reqStream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timer_deviceon_Tick(object sender, EventArgs e)
        {
            try
            {
                using (var client = new WebClient())
                {
                    string auth = "r@inCh3ckd234b70";
                    string type = "running";
                    string mac_id = GetMACAddress();
                    string run_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
                    string request = "http://raincheck.ssitex.com/api/api.php";

                    MessageBox.Show(run_time);
                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "auth", auth },
                        { "type", type },
                        { "mac_id", mac_id },
                        { "run_time", run_time }
                    };

                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1036", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }
        }

        private void button_startover_urgent_Click(object sender, EventArgs e)
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
                i_urgent = 1;

                //if (label9.Text == "")
                //{
                //    label9.Text = label9.Text;
                //}

                //if (label11.Text == "")
                //{
                //    label11.Text = label11.Text;
                //}

                ms_detect = 0;
                fully_loaded = 0;
                start_detect = 0;
                domain_i = 0;
                label_currentindex_urgent.Text = "0";

                string datetime = label11.Text;
                string datetime_folder = label9.Text;
                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder + "_urgent_" + i_timeout;

                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }

                timer_blink.Stop();
                label_status_urgent.Visible = true;
                button_pause_urgent.Visible = true;
                button_start_urgent.Visible = false;
                label_status_urgent.Text = "[Running]";
                timer_domain_urgent.Start();

                dataGridView_urgent.ClearSelection();
                dataGridView_urgent.Rows[0].Selected = true;

                textBox_domain_urgent.Enabled = false;

                button_pause_urgent.Enabled = true;
            }
        }
    }
}