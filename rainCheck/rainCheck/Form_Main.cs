using CefSharp;
using CefSharp.WinForms;
using ChoETL;
using Ionic.Zip;
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
        public ChromiumWebBrowser chromeBrowser { get; private set; }

        public static string SetResult = "";
        public static string BrandIDs = "";
        public static string LastCurrentIndex = "";
        public static string SetValueForTextBrandID = "";
        public static string SetValueForTextSearch = "";
        public static string SetValueForWebsiteType = "";

        static bool networkIsAvailable = false;

        static List<string> inaccessble_lists = new List<string>();

        string city_get;
        string isp_get;

        public Form_Main(string city, string country, string isp)
        {
            InitializeComponent();

            var culture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            //string city, string country, string isp
            Text = "rainCheck: " + city + ", " + country + " - " + isp;

            city_get = city;
            isp_get = isp;

            // Design
            //this.WindowState = FormWindowState.Maximized;

            APIGetDomains();
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            InitializeChromium();

            string path_urgent_domain_detect = Path.GetTempPath() + @"\raincheck_urgent.txt";
            if (File.Exists(path_urgent_domain_detect))
            {
                if (label_status.Text == "[Running]")
                {
                    button_pause.PerformClick();
                }

                try
                {
                    using (var client_1 = new WebClient())
                    {
                        string auth_1 = "r@inCh3ckd234b70";
                        string type_1 = "domain_urgent";
                        string request_1 = "http://raincheck.ssitex.com/api/api.php";

                        NameValueCollection postData_1 = new NameValueCollection()
                        {
                            { "auth", auth_1 },
                            { "type", type_1 }
                        };

                        string pagesource_1 = Encoding.UTF8.GetString(client_1.UploadValues(request_1, postData_1));
                        var arr = JsonConvert.DeserializeObject<JArray>(pagesource_1);
                        dataGridView_urgent.DataSource = arr;
                    }
                }
                catch (Exception)
                {
                    StreamWriter sw_create = new StreamWriter(path_urgent_domain_detect, true, Encoding.UTF8);
                    sw_create.Close();

                    string path_autoyes = Path.GetTempPath() + @"\raincheck_autoyes.txt";
                    StreamWriter sw_autoyes = new StreamWriter(path_autoyes, true, Encoding.UTF8);
                    sw_autoyes.Close();

                    can_close = false;
                    Close();
                    Application.Restart();
                }

                if (can_close)
                {
                    // Table UI
                    dataGridView_urgent.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                    string hex = "#438eb9";
                    Color color = ColorTranslator.FromHtml(hex);
                    dataGridView_urgent.DefaultCellStyle.SelectionBackColor = color;
                    dataGridView_urgent.DefaultCellStyle.SelectionForeColor = Color.White;
                    dataGridView_urgent.Columns["domain_name"].Visible = false;
                    dataGridView_urgent.Columns["id"].Visible = false;
                    dataGridView_urgent.Columns["text_search"].Visible = false;
                    dataGridView_urgent.Columns["website_type"].Visible = false;
                    label_domainscount_urgent.Visible = true;
                    label_domainscount_urgent.Text = "Total: " + dataGridView_urgent.RowCount.ToString();

                    // Go to urgent panel
                    panel_urgent.Visible = true;
                    panel_main.Visible = false;
                    label_domainscount.Visible = false;
                    label_domain_urgent.Visible = true;
                    textBox_domain_urgent.Text = "";

                    File.Delete(path_urgent_domain_detect);

                    dataGridView_urgent.ClearSelection();

                    // Start urgent
                    button_start_urgent.Enabled = false;
                    timer_start_urgent.Start();
                    timer_urgent_detect.Stop();
                }
            }

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

                string path_autoyes = Path.GetTempPath() + @"\raincheck_autoyes.txt";
                StreamWriter sw_autoyes = new StreamWriter(path_autoyes, true, Encoding.UTF8);
                sw_autoyes.Close();
                
                can_close = false;
                Close();
                Application.Restart();
            }

            try
            {
                using (var client = new WebClient())
                {
                    string auth = "r@inCh3ckd234b70";
                    string type = "text_search_brand";
                    string request = "http://raincheck.ssitex.com/api/api.php";

                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "auth", auth },
                        { "type", type },
                    };

                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));
                    label_textsearch_brand.Text = pagesource;
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1036", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                string path_autoyes = Path.GetTempPath() + @"\raincheck_autoyes.txt";
                StreamWriter sw_autoyes = new StreamWriter(path_autoyes, true, Encoding.UTF8);
                sw_autoyes.Close();

                can_close = false;
                Close();
                Application.Restart();
            }

            try
            {
                using (var client = new WebClient())
                {
                    string auth = "r@inCh3ckd234b70";
                    string type = "urgent_device";
                    string mac_id = GetMACAddress();
                    string request = "http://raincheck.ssitex.com/api/api.php";

                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "auth", auth },
                        { "type", type },
                        { "mac_id", mac_id }
                    };

                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));

                    JArray jsonObject = JArray.Parse(pagesource);
                    string u_id = jsonObject[0]["u_id"].Value<string>();
                    string u_type = jsonObject[0]["set_type"].Value<string>();
                    
                    label_urgent_detect.Text = u_id;
                    label_utype.Text = u_type;
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1036", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                string path_autoyes = Path.GetTempPath() + @"\raincheck_autoyes.txt";
                StreamWriter sw_autoyes = new StreamWriter(path_autoyes, true, Encoding.UTF8);
                sw_autoyes.Close();

                can_close = false;
                Close();
                Application.Restart();
            }
            
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
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1001", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }

            // Delete after 7 days
            DateTime date = DateTime.Now.AddDays(-7);
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\rainCheck\";
            if (Directory.Exists(directory))
            {
                // Normal
                string[] files_zip = Directory.GetFiles(directory, date.ToString("yyyy-MM-dd") + "_????.zip");
                string[] files_folder = Directory.GetDirectories(directory, date.ToString("yyyy-MM-dd") + "_????");

                string pattern_zip = date.ToString("yyyy-MM-dd") + "_[0-9]{4}\\.zip";
                string pattern_folder = date.ToString("yyyy-MM-dd") + "_[0-9]{4}";

                foreach (string file in files_zip)
                {
                    if (Regex.IsMatch(file, pattern_zip))
                    {
                        File.Delete(file);
                    }
                }

                foreach (string file in files_folder)
                {
                    if (Regex.IsMatch(file, pattern_folder))
                    {
                        Directory.Delete(file, true);
                    }
                }

                // Urgent
                string[] files_urgent_zip_1 = Directory.GetFiles(directory, date.ToString("yyyy-MM-dd") + "_????_urgent_?.zip");
                string[] files_urgent_zip_2= Directory.GetFiles(directory, date.ToString("yyyy-MM-dd") + "_????_urgent_??.zip");
                string[] files_urgent_folder_1 = Directory.GetDirectories(directory, date.ToString("yyyy-MM-dd") + "_????_urgent_?");
                string[] files_urgent_folder_2 = Directory.GetDirectories(directory, date.ToString("yyyy-MM-dd") + "_????_urgent_??");

                string pattern_urgent_zip_1 = date.ToString("yyyy-MM-dd") + "_[0-9]{4}_urgent_[0-9]{1}\\.zip";
                string pattern_urgent_zip_2 = date.ToString("yyyy-MM-dd") + "_[0-9]{4}_urgent_[0-9]{2}\\.zip";
                string pattern_urgent_folder_1 = date.ToString("yyyy-MM-dd") + "_[0-9]{4}_urgent_[0-9]{1}";
                string pattern_urgent_folder_2 = date.ToString("yyyy-MM-dd") + "_[0-9]{4}_urgent_[0-9]{2}";

                // zip
                foreach (string file in files_urgent_zip_1)
                {
                    if (Regex.IsMatch(file, pattern_urgent_zip_1))
                    {
                        File.Delete(file);
                    }
                }

                foreach (string file in files_urgent_zip_2)
                {
                    if (Regex.IsMatch(file, pattern_urgent_zip_2))
                    {
                        File.Delete(file);
                    }
                }

                // folder
                foreach (string file in files_urgent_folder_1)
                {
                    if (Regex.IsMatch(file, pattern_urgent_folder_1))
                    {
                        Directory.Delete(file, true);
                    }
                }

                foreach (string file in files_urgent_folder_2)
                {
                    if (Regex.IsMatch(file, pattern_urgent_folder_2))
                    {
                        Directory.Delete(file, true);
                    }
                }
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

                Close();
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

                Close();
            }
            
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

            if (networkIsAvailable)
            {
                panel_retry.Visible = false;
            }
            else
            {
                panel_retry.Visible = true;
            }

            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);

            Console.ReadLine();

            // Getting the total count domain
            domain_total = dataGridView_domain.RowCount;
            label_totalcountofdomain.Text = domain_total.ToString();
            label_domainscount.Text = "Total: " + domain_total.ToString();
            
            // Getting mac id
            label_macid.Text = GetMACAddress().ToLower();
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
                if (panel_main.Visible == true)
                {
                    string color = "#438eb9";
                    Color color_change = ColorTranslator.FromHtml(color);
                    panel_top.BackColor = color_change;

                    Invoke(new Action(() =>
                    {
                        int getCurrentIndex = Convert.ToInt32(label_currentindex.Text);
                        dataGridView_domain.ClearSelection();

                        if (getCurrentIndex > 0)
                        {
                            panel_retry.Visible = false;
                            panel_retry.BringToFront();
                            
                            if (label_status.Text == "[Running]")
                            {
                                timer_blink.Stop();
                                label_status.Visible = true;
                                label_status.Text = "[Running]";
                                timer_domain.Start();
                                
                                // For timeout
                                i = 1;
                                timer_timeout.Start();

                                pictureBox_loader.Visible = true;
                                
                                button_pause.Visible = true;
                                button_start.Visible = false;

                                textBox_domain.Enabled = false;
                                button_go.Enabled = false;

                                dataGridView_domain.Rows[getCurrentIndex].Selected = true;
                            }
                            else
                            {
                                button_pause.Visible = false;
                                button_start.Visible = true;

                                dataGridView_domain.Rows[getCurrentIndex].Selected = true;
                                textBox_domain.Text = "";
                            }
                        }
                        else
                        {
                            panel_retry.Visible = false;
                        }
                    }));
                }
                else if (panel_urgent.Visible == true)
                {
                    string color = "#394557";
                    Color color_change = ColorTranslator.FromHtml(color);
                    panel_top.BackColor = color_change;

                    Invoke(new Action(() =>
                    {
                        int getCurrentIndex = Convert.ToInt32(label_currentindex_urgent.Text);
                        dataGridView_urgent.ClearSelection();

                        if (getCurrentIndex > 0)
                        {
                            panel_retry.Visible = false;
                            panel_retry.BringToFront();

                            if (label_status_urgent.Text == "[Running]")
                            {
                                timer_blink_urgent.Stop();
                                label_status_urgent.Visible = true;
                                label_status_urgent.Text = "[Running]";
                                timer_domain_urgent.Start();

                                // For timeout
                                i = 1;
                                timer_timeout.Start();

                                pictureBox_loader_urgent.Visible = true;

                                button_pause_urgent.Visible = true;
                                button_start_urgent.Visible = false;

                                dataGridView_urgent.Rows[getCurrentIndex].Selected = true;
                            }
                            else
                            {
                                button_pause_urgent.Visible = false;
                                button_pause_urgent.Visible = true;

                                dataGridView_urgent.Rows[getCurrentIndex].Selected = true;
                            }
                        }
                        else
                        {
                            panel_retry.Visible = false;
                        }
                    }));
                }
            }
            else
            {
                string color = "#394557";
                Color color_change = ColorTranslator.FromHtml(color);
                panel_top.BackColor = color_change;

                if (panel_main.Visible == true)
                {
                    Invoke(new Action(() =>
                    {
                        chromeBrowser.Stop();

                        panel_retry.Visible = true;
                        panel_retry.BringToFront();

                        timer_domain.Stop();
                        timer_timeout.Stop();
                        pictureBox_loader.Visible = false;
                        button_pause.Visible = false;
                        button_start.Visible = true;
                    }));
                }
                else if (panel_urgent.Visible == true)
                {
                    Invoke(new Action(() =>
                    {
                        chromeBrowser.Stop();

                        panel_retry.Visible = true;
                        panel_retry.BringToFront();

                        timer_domain_urgent.Stop();
                        timer_timeout.Stop();
                        pictureBox_loader_urgent.Visible = false;
                        button_pause_urgent.Visible = false;
                        button_start_urgent.Visible = true;
                    }));
                }
            }
        }
        
        private void InitializeChromium()
        {
            try
            {
                CefSettings settings = new CefSettings();
                settings.CefCommandLineArgs.Add("disable-plugins-discovery", "1");
                settings.CefCommandLineArgs.Add("no-proxy-server", "1");
                Cef.Initialize(settings);

                chromeBrowser = new ChromiumWebBrowser(textBox_domain.Text);

                panel_browser.Controls.Add(chromeBrowser);

                chromeBrowser.Dock = DockStyle.Fill;
                
                chromeBrowser.LoadingStateChanged += ChromiumWebBrowser_LoadingStateChangedAsync;
                chromeBrowser.AddressChanged += ChromiumWebBrowser_AddressChanged;
                chromeBrowser.LoadError += ChromiumWebBrowser_BrowserLoadError;
                chromeBrowser.TitleChanged += ChromiumWebBrowser_TitleChanged;
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
                if (panel_main.Visible == true)
                {
                    cefsharp_domain = e.Address;
                }
                else if (panel_urgent.Visible == true)
                {
                    cefsharp_domain = e.Address;
                }
            }));
        }

        private void ChromiumWebBrowser_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                cefsharp_title = e.Title;
            }));
        }

        private void ChromiumWebBrowser_BrowserLoadError(object sender, LoadErrorEventArgs e)
        {
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
                timer_timeout.Stop();
                pictureBox_loader.Visible = false;
            }
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

                    // Date preview
                    start_load = DateTime.Now.ToString("HH:mm:ss.fff");
                    start_load_inaccessible = DateTime.Now;

                    Invoke(new Action(() =>
                    {
                        i = 1;
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
                            webBrowser_new.Navigate(label_domainhide.Text);
                            webBrowser_i++;
                        }
                    }
                    else
                    {
                        int webBrowser_i = 0;
                        while (webBrowser_i <= 2)
                        {
                            webBrowser_new.Navigate(label_domainhide.Text);
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
                    }));

                    if (label_fully_loaded.Text == "1")
                    {
                        Invoke(new Action(() =>
                        {
                            textBox_domain.Text = cefsharp_domain;
                        }));

                        // Inaccessible Status
                        string result = "";
                        string search_replace = label_webtitle.Text;

                        string upper_search = search_replace.ToUpper().ToString();

                        StringBuilder sb = new StringBuilder(upper_search);
                        sb.Replace("-", "");
                        sb.Replace(".", "");
                        sb.Replace(",", "");
                        sb.Replace("!", "");

                        string final_search = Regex.Replace(sb.ToString(), " {2,}", " ");

                        var final_inaccessble_lists = inaccessble_lists.Select(m => m.ToUpper());

                        string[] words = final_search.Split(' ');

                        int i = 0;
                        foreach (string word in words)
                        {
                            i++;

                            if (word != "")
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

                            if (i == 1 && search_replace == "")
                            {
                                result = "match";
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
                                        // Leave blank
                                    }

                                    if (html.Contains("landing_image"))
                                    {
                                        // Timeout Status
                                        if (label_timeout.Text == "timeout")
                                        {
                                            await Task.Run(async () =>
                                            {
                                                await Task.Delay(1000);
                                            });

                                            DataToTextFileTimeout();

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
                                            await Task.Run(async () =>
                                            {
                                                await Task.Delay(1000);
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
                                                webBrowser_new.Stop();
                                                //webBrowser_new.Refresh(WebBrowserRefreshOption.Completely);

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
                                                    await Task.Delay(1000);
                                                });

                                                string datetime = label11.Text;
                                                string datetime_folder = label9.Text;
                                                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                                string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

                                                string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

                                                DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                                using (var pic = new Bitmap(webBrowser_new.Width - 18, webBrowser_new.Height - 18))
                                                {
                                                    webBrowser_new.DrawToBitmap(pic, new Rectangle(0, 0, pic.Width, pic.Height));
                                                    Bitmap resized = new Bitmap(pic, new Size(pic.Width / 2, pic.Height / 2));
                                                    string domain_replace = label_domainhide.Text;
                                                    StringBuilder sb_pic = new StringBuilder(domain_replace);
                                                    sb_pic.Replace("\\", "");
                                                    sb_pic.Replace("/", "");
                                                    sb_pic.Replace("\"", "");
                                                    sb_pic.Replace("*", "");
                                                    sb_pic.Replace(":", "");
                                                    sb_pic.Replace("?", "");
                                                    sb_pic.Replace("<", "");
                                                    sb_pic.Replace(">", "");
                                                    sb_pic.Replace("|", "");
                                                    sb_pic.Replace(" ", "");
                                                    sb_pic.Replace("_", "");
                                                    string full_path = path + "_" + label_macid.Text + "_n_" + sb_pic.ToString() + ".jpeg";
                                                    resized.Save(full_path, ImageFormat.Jpeg);

                                                    var fileLength = new FileInfo(full_path).Length;

                                                    if (fileLength < 3200 && label_webtitle.Text == "Can’t reach this page" || fileLength < 3200 && label_webtitle.Text == "无法访问此页面")
                                                    {
                                                        var access = new Bitmap(Properties.Resources.access);
                                                        access.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                    else if (fileLength < 3200 && label_webtitle.Text == "This site isn’t secure" || fileLength < 3200 && label_webtitle.Text == "此站点不安全")
                                                    {
                                                        var secure = new Bitmap(Properties.Resources.secure);
                                                        secure.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                    else if (fileLength < 3200 && label_webtitle.Text == "Navigation Canceled" || fileLength < 3200 && label_webtitle.Text == "导航已取消")
                                                    {
                                                        var navigation = new Bitmap(Properties.Resources.navigation);
                                                        navigation.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                    else
                                                    {
                                                        resized.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                }

                                                await Task.Run(async () =>
                                                {
                                                    await Task.Delay(1000);
                                                });

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
                                                webBrowser_new.Stop();
                                                //webBrowser_new.Refresh(WebBrowserRefreshOption.Completely);

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
                                                    await Task.Delay(1000);
                                                });

                                                string datetime = label11.Text;
                                                string datetime_folder = label9.Text;
                                                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                                string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

                                                string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

                                                DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                                using (var pic = new Bitmap(webBrowser_new.Width - 18, webBrowser_new.Height - 18))
                                                {
                                                    webBrowser_new.DrawToBitmap(pic, new Rectangle(0, 0, pic.Width, pic.Height));
                                                    Bitmap resized = new Bitmap(pic, new Size(pic.Width / 2, pic.Height / 2));
                                                    string domain_replace = label_domainhide.Text;
                                                    StringBuilder sb_pic = new StringBuilder(domain_replace);
                                                    sb_pic.Replace("\\", "");
                                                    sb_pic.Replace("/", "");
                                                    sb_pic.Replace("\"", "");
                                                    sb_pic.Replace("*", "");
                                                    sb_pic.Replace(":", "");
                                                    sb_pic.Replace("?", "");
                                                    sb_pic.Replace("<", "");
                                                    sb_pic.Replace(">", "");
                                                    sb_pic.Replace("|", "");
                                                    sb_pic.Replace(" ", "");
                                                    sb_pic.Replace("_", "");
                                                    string full_path = path + "_" + label_macid.Text + "_n_" + sb_pic.ToString() + ".jpeg";
                                                    resized.Save(full_path, ImageFormat.Jpeg);

                                                    var fileLength = new FileInfo(full_path).Length;

                                                    if (fileLength < 3200 && label_webtitle.Text == "Can’t reach this page" || fileLength < 3200 && label_webtitle.Text == "无法访问此页面")
                                                    {
                                                        var access = new Bitmap(Properties.Resources.access);
                                                        access.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                    else if (fileLength < 3200 && label_webtitle.Text == "This site isn’t secure" || fileLength < 3200 && label_webtitle.Text == "此站点不安全")
                                                    {
                                                        var secure = new Bitmap(Properties.Resources.secure);
                                                        secure.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                    else if (fileLength < 3200 && label_webtitle.Text == "Navigation Canceled" || fileLength < 3200 && label_webtitle.Text == "导航已取消")
                                                    {
                                                        var navigation = new Bitmap(Properties.Resources.navigation);
                                                        navigation.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                    else
                                                    {
                                                        resized.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                }

                                                await Task.Run(async () =>
                                                {
                                                    await Task.Delay(1000);
                                                });

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
                                            webBrowser_new.Stop();
                                            //webBrowser_new.Refresh(WebBrowserRefreshOption.Completely);

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
                                                await Task.Delay(1000);
                                            });

                                            string datetime = label11.Text;
                                            string datetime_folder = label9.Text;
                                            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                            string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

                                            string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

                                            DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                            using (var pic = new Bitmap(webBrowser_new.Width - 18, webBrowser_new.Height - 18))
                                            {
                                                webBrowser_new.DrawToBitmap(pic, new Rectangle(0, 0, pic.Width, pic.Height));
                                                Bitmap resized = new Bitmap(pic, new Size(pic.Width / 2, pic.Height / 2));
                                                string domain_replace = label_domainhide.Text;
                                                StringBuilder sb_pic = new StringBuilder(domain_replace);
                                                sb_pic.Replace("\\", "");
                                                sb_pic.Replace("/", "");
                                                sb_pic.Replace("\"", "");
                                                sb_pic.Replace("*", "");
                                                sb_pic.Replace(":", "");
                                                sb_pic.Replace("?", "");
                                                sb_pic.Replace("<", "");
                                                sb_pic.Replace(">", "");
                                                sb_pic.Replace("|", "");
                                                sb_pic.Replace(" ", "");
                                                sb_pic.Replace("_", "");
                                                string full_path = path + "_" + label_macid.Text + "_n_" + sb_pic.ToString() + ".jpeg";
                                                resized.Save(full_path, ImageFormat.Jpeg);

                                                var fileLength = new FileInfo(full_path).Length;

                                                if (fileLength < 3200 && label_webtitle.Text == "Can’t reach this page" || fileLength < 3200 && label_webtitle.Text == "无法访问此页面")
                                                {
                                                    var access = new Bitmap(Properties.Resources.access);
                                                    access.Save(full_path, ImageFormat.Jpeg);
                                                }
                                                else if (fileLength < 3200 && label_webtitle.Text == "This site isn’t secure" || fileLength < 3200 && label_webtitle.Text == "此站点不安全")
                                                {
                                                    var secure = new Bitmap(Properties.Resources.secure);
                                                    secure.Save(full_path, ImageFormat.Jpeg);
                                                }
                                                else if (fileLength < 3200 && label_webtitle.Text == "Navigation Canceled" || fileLength < 3200 && label_webtitle.Text == "导航已取消")
                                                {
                                                    var navigation = new Bitmap(Properties.Resources.navigation);
                                                    navigation.Save(full_path, ImageFormat.Jpeg);
                                                }
                                                else
                                                {
                                                    resized.Save(full_path, ImageFormat.Jpeg);
                                                }
                                            }

                                            await Task.Run(async () =>
                                            {
                                                await Task.Delay(1000);
                                            });

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
                                            webBrowser_new.Stop();
                                            //webBrowser_new.Refresh(WebBrowserRefreshOption.Completely);

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
                                                await Task.Delay(1000);
                                            });

                                            string datetime = label11.Text;
                                            string datetime_folder = label9.Text;
                                            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                            string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

                                            string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

                                            DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                            using (var pic = new Bitmap(webBrowser_new.Width - 18, webBrowser_new.Height - 18))
                                            {
                                                webBrowser_new.DrawToBitmap(pic, new Rectangle(0, 0, pic.Width, pic.Height));
                                                Bitmap resized = new Bitmap(pic, new Size(pic.Width / 2, pic.Height / 2));
                                                string domain_replace = label_domainhide.Text;
                                                StringBuilder sb_pic = new StringBuilder(domain_replace);
                                                sb_pic.Replace("\\", "");
                                                sb_pic.Replace("/", "");
                                                sb_pic.Replace("\"", "");
                                                sb_pic.Replace("*", "");
                                                sb_pic.Replace(":", "");
                                                sb_pic.Replace("?", "");
                                                sb_pic.Replace("<", "");
                                                sb_pic.Replace(">", "");
                                                sb_pic.Replace("|", "");
                                                sb_pic.Replace(" ", "");
                                                sb_pic.Replace("_", "");
                                                string full_path = path + "_" + label_macid.Text + "_n_" + sb_pic.ToString() + ".jpeg";
                                                resized.Save(full_path, ImageFormat.Jpeg);

                                                var fileLength = new FileInfo(full_path).Length;

                                                if (fileLength < 3200 && label_webtitle.Text == "Can’t reach this page" || fileLength < 3200 && label_webtitle.Text == "无法访问此页面")
                                                {
                                                    var access = new Bitmap(Properties.Resources.access);
                                                    access.Save(full_path, ImageFormat.Jpeg);
                                                }
                                                else if (fileLength < 3200 && label_webtitle.Text == "This site isn’t secure" || fileLength < 3200 && label_webtitle.Text == "此站点不安全")
                                                {
                                                    var secure = new Bitmap(Properties.Resources.secure);
                                                    secure.Save(full_path, ImageFormat.Jpeg);
                                                }
                                                else if (fileLength < 3200 && label_webtitle.Text == "Navigation Canceled" || fileLength < 3200 && label_webtitle.Text == "导航已取消")
                                                {
                                                    var navigation = new Bitmap(Properties.Resources.navigation);
                                                    navigation.Save(full_path, ImageFormat.Jpeg);
                                                }
                                                else
                                                {
                                                    resized.Save(full_path, ImageFormat.Jpeg);
                                                }
                                            }

                                            await Task.Run(async () =>
                                            {
                                                await Task.Delay(1000);
                                            });

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
                            Invoke(new Action(() =>
                            {
                                label_webtitle.Text = cefsharp_title;
                            }));

                            // inaccessible
                            if (label_webtitle.Text == label_domainhide.Text)
                            {
                                webBrowser_new.Stop();
                                //webBrowser_new.Refresh(WebBrowserRefreshOption.Completely);

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
                                    await Task.Delay(1000);
                                });

                                string datetime = label11.Text;
                                string datetime_folder = label9.Text;
                                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

                                string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

                                DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                using (var pic = new Bitmap(webBrowser_new.Width - 18, webBrowser_new.Height - 18))
                                {
                                    webBrowser_new.DrawToBitmap(pic, new Rectangle(0, 0, pic.Width, pic.Height));
                                    Bitmap resized = new Bitmap(pic, new Size(pic.Width / 2, pic.Height / 2));
                                    string domain_replace = label_domainhide.Text;
                                    StringBuilder sb_pic = new StringBuilder(domain_replace);
                                    sb_pic.Replace("\\", "");
                                    sb_pic.Replace("/", "");
                                    sb_pic.Replace("\"", "");
                                    sb_pic.Replace("*", "");
                                    sb_pic.Replace(":", "");
                                    sb_pic.Replace("?", "");
                                    sb_pic.Replace("<", "");
                                    sb_pic.Replace(">", "");
                                    sb_pic.Replace("|", "");
                                    sb_pic.Replace(" ", "");
                                    sb_pic.Replace("_", "");
                                    string full_path = path + "_" + label_macid.Text + "_n_" + sb_pic.ToString() + ".jpeg";
                                    resized.Save(full_path, ImageFormat.Jpeg);

                                    var fileLength = new FileInfo(full_path).Length;

                                    if (fileLength < 3200 && label_webtitle.Text == "Can’t reach this page" || fileLength < 3200 && label_webtitle.Text == "无法访问此页面")
                                    {
                                        var access = new Bitmap(Properties.Resources.access);
                                        access.Save(full_path, ImageFormat.Jpeg);
                                    }
                                    else if (fileLength < 3200 && label_webtitle.Text == "This site isn’t secure" || fileLength < 3200 && label_webtitle.Text == "此站点不安全")
                                    {
                                        var secure = new Bitmap(Properties.Resources.secure);
                                        secure.Save(full_path, ImageFormat.Jpeg);
                                    }
                                    else if (fileLength < 3200 && label_webtitle.Text == "Navigation Canceled" || fileLength < 3200 && label_webtitle.Text == "导航已取消")
                                    {
                                        var navigation = new Bitmap(Properties.Resources.navigation);
                                        navigation.Save(full_path, ImageFormat.Jpeg);
                                    }
                                    else
                                    {
                                        resized.Save(full_path, ImageFormat.Jpeg);
                                    }
                                }

                                await Task.Run(async () =>
                                {
                                    await Task.Delay(1000);
                                });

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
                                // Timeout Status
                                if (label_timeout.Text == "timeout")
                                {
                                    await Task.Run(async () =>
                                    {
                                        await Task.Delay(1000);
                                    });

                                    DataToTextFileTimeout();

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
                                    await Task.Run(async () =>
                                    {
                                        await Task.Delay(1000);
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
                            }
                            // hijacked
                            else
                            {
                                await Task.Run(async () =>
                                {
                                    await Task.Delay(1000);
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
                    Invoke(new Action(() =>
                    {
                        panel_browser.Controls.Add(chromeBrowser);
                        start_detect++;
                        label_start_detect.Text = start_detect.ToString();
                    }));

                    // Date preview
                    start_load = DateTime.Now.ToString("HH:mm:ss.fff");
                    start_load_inaccessible = DateTime.Now;

                    Invoke(new Action(() =>
                    {
                        i = 1;
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
                    }));

                    if (label_fully_loaded.Text == "1")
                    {
                        Invoke(new Action(() =>
                        {
                            textBox_domain.Text = cefsharp_domain;
                        }));

                        // Inaccessible Status
                        string result = "";
                        string search_replace = label_webtitle.Text;

                        string upper_search = search_replace.ToUpper().ToString();

                        StringBuilder sb = new StringBuilder(upper_search);
                        sb.Replace("-", "");
                        sb.Replace(".", "");
                        sb.Replace(",", "");
                        sb.Replace("!", "");

                        string final_search = Regex.Replace(sb.ToString(), " {2,}", " ");

                        var final_inaccessble_lists = inaccessble_lists.Select(m => m.ToUpper());

                        string[] words = final_search.Split(' ');

                        int i = 0;
                        foreach (string word in words)
                        {
                            i++;

                            if (word != "")
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

                            if (i == 1 && search_replace == "")
                            {
                                result = "match";
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
                                        // Leave blank
                                    }

                                    if (html.Contains("landing_image"))
                                    {
                                        // Timeout Status
                                        if (label_timeout.Text == "timeout")
                                        {
                                            await Task.Run(async () =>
                                            {
                                                await Task.Delay(1000);
                                            });

                                            DataToTextFileTimeout();

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
                                            await Task.Run(async () =>
                                            {
                                                await Task.Delay(1000);
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

                                                //TopMost = false;
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
                                                webBrowser_new.Stop();
                                                //webBrowser_new.Refresh(WebBrowserRefreshOption.Completely);

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
                                                    await Task.Delay(1000);
                                                });

                                                string datetime = label11.Text;
                                                string datetime_folder = label9.Text;
                                                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                                string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

                                                string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

                                                DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                                using (var pic = new Bitmap(webBrowser_new.Width - 18, webBrowser_new.Height - 18))
                                                {
                                                    webBrowser_new.DrawToBitmap(pic, new Rectangle(0, 0, pic.Width, pic.Height));
                                                    Bitmap resized = new Bitmap(pic, new Size(pic.Width / 2, pic.Height / 2));
                                                    string domain_replace = label_domainhide.Text;
                                                    StringBuilder sb_pic = new StringBuilder(domain_replace);
                                                    sb_pic.Replace("\\", "");
                                                    sb_pic.Replace("/", "");
                                                    sb_pic.Replace("\"", "");
                                                    sb_pic.Replace("*", "");
                                                    sb_pic.Replace(":", "");
                                                    sb_pic.Replace("?", "");
                                                    sb_pic.Replace("<", "");
                                                    sb_pic.Replace(">", "");
                                                    sb_pic.Replace("|", "");
                                                    sb_pic.Replace(" ", "");
                                                    sb_pic.Replace("_", "");
                                                    string full_path = path + "_" + label_macid.Text + "_n_" + sb_pic.ToString() + ".jpeg";
                                                    resized.Save(full_path, ImageFormat.Jpeg);

                                                    var fileLength = new FileInfo(full_path).Length;

                                                    if (fileLength < 3200 && label_webtitle.Text == "Can’t reach this page" || fileLength < 3200 && label_webtitle.Text == "无法访问此页面")
                                                    {
                                                        var access = new Bitmap(Properties.Resources.access);
                                                        access.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                    else if (fileLength < 3200 && label_webtitle.Text == "This site isn’t secure" || fileLength < 3200 && label_webtitle.Text == "此站点不安全")
                                                    {
                                                        var secure = new Bitmap(Properties.Resources.secure);
                                                        secure.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                    else if (fileLength < 3200 && label_webtitle.Text == "Navigation Canceled" || fileLength < 3200 && label_webtitle.Text == "导航已取消")
                                                    {
                                                        var navigation = new Bitmap(Properties.Resources.navigation);
                                                        navigation.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                    else
                                                    {
                                                        resized.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                }

                                                await Task.Run(async () =>
                                                {
                                                    await Task.Delay(1000);
                                                });

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
                                                webBrowser_new.Stop();
                                                //webBrowser_new.Refresh(WebBrowserRefreshOption.Completely);

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
                                                    await Task.Delay(1000);
                                                });

                                                string datetime = label11.Text;
                                                string datetime_folder = label9.Text;
                                                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                                string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

                                                string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

                                                DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                                using (var pic = new Bitmap(webBrowser_new.Width - 18, webBrowser_new.Height - 18))
                                                {
                                                    webBrowser_new.DrawToBitmap(pic, new Rectangle(0, 0, pic.Width, pic.Height));
                                                    Bitmap resized = new Bitmap(pic, new Size(pic.Width / 2, pic.Height / 2));
                                                    string domain_replace = label_domainhide.Text;
                                                    StringBuilder sb_pic = new StringBuilder(domain_replace);
                                                    sb_pic.Replace("\\", "");
                                                    sb_pic.Replace("/", "");
                                                    sb_pic.Replace("\"", "");
                                                    sb_pic.Replace("*", "");
                                                    sb_pic.Replace(":", "");
                                                    sb_pic.Replace("?", "");
                                                    sb_pic.Replace("<", "");
                                                    sb_pic.Replace(">", "");
                                                    sb_pic.Replace("|", "");
                                                    sb_pic.Replace(" ", "");
                                                    sb_pic.Replace("_", "");
                                                    string full_path = path + "_" + label_macid.Text + "_n_" + sb_pic.ToString() + ".jpeg";
                                                    resized.Save(full_path, ImageFormat.Jpeg);

                                                    var fileLength = new FileInfo(full_path).Length;

                                                    if (fileLength < 3200 && label_webtitle.Text == "Can’t reach this page" || fileLength < 3200 && label_webtitle.Text == "无法访问此页面")
                                                    {
                                                        var access = new Bitmap(Properties.Resources.access);
                                                        access.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                    else if (fileLength < 3200 && label_webtitle.Text == "This site isn’t secure" || fileLength < 3200 && label_webtitle.Text == "此站点不安全")
                                                    {
                                                        var secure = new Bitmap(Properties.Resources.secure);
                                                        secure.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                    else if (fileLength < 3200 && label_webtitle.Text == "Navigation Canceled" || fileLength < 3200 && label_webtitle.Text == "导航已取消")
                                                    {
                                                        var navigation = new Bitmap(Properties.Resources.navigation);
                                                        navigation.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                    else
                                                    {
                                                        resized.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                }

                                                await Task.Run(async () =>
                                                {
                                                    await Task.Delay(1000);
                                                });

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
                                            webBrowser_new.Stop();
                                            //webBrowser_new.Refresh(WebBrowserRefreshOption.Completely);

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
                                                await Task.Delay(1000);
                                            });

                                            string datetime = label11.Text;
                                            string datetime_folder = label9.Text;
                                            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                            string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

                                            string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

                                            DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                            using (var pic = new Bitmap(webBrowser_new.Width - 18, webBrowser_new.Height - 18))
                                            {
                                                webBrowser_new.DrawToBitmap(pic, new Rectangle(0, 0, pic.Width, pic.Height));
                                                Bitmap resized = new Bitmap(pic, new Size(pic.Width / 2, pic.Height / 2));
                                                string domain_replace = label_domainhide.Text;
                                                StringBuilder sb_pic = new StringBuilder(domain_replace);
                                                sb_pic.Replace("\\", "");
                                                sb_pic.Replace("/", "");
                                                sb_pic.Replace("\"", "");
                                                sb_pic.Replace("*", "");
                                                sb_pic.Replace(":", "");
                                                sb_pic.Replace("?", "");
                                                sb_pic.Replace("<", "");
                                                sb_pic.Replace(">", "");
                                                sb_pic.Replace("|", "");
                                                sb_pic.Replace(" ", "");
                                                sb_pic.Replace("_", "");
                                                string full_path = path + "_" + label_macid.Text + "_n_" + sb_pic.ToString() + ".jpeg";
                                                resized.Save(full_path, ImageFormat.Jpeg);

                                                var fileLength = new FileInfo(full_path).Length;

                                                if (fileLength < 3200 && label_webtitle.Text == "Can’t reach this page" || fileLength < 3200 && label_webtitle.Text == "无法访问此页面")
                                                {
                                                    var access = new Bitmap(Properties.Resources.access);
                                                    access.Save(full_path, ImageFormat.Jpeg);
                                                }
                                                else if (fileLength < 3200 && label_webtitle.Text == "This site isn’t secure" || fileLength < 3200 && label_webtitle.Text == "此站点不安全")
                                                {
                                                    var secure = new Bitmap(Properties.Resources.secure);
                                                    secure.Save(full_path, ImageFormat.Jpeg);
                                                }
                                                else if (fileLength < 3200 && label_webtitle.Text == "Navigation Canceled" || fileLength < 3200 && label_webtitle.Text == "导航已取消")
                                                {
                                                    var navigation = new Bitmap(Properties.Resources.navigation);
                                                    navigation.Save(full_path, ImageFormat.Jpeg);
                                                }
                                                else
                                                {
                                                    resized.Save(full_path, ImageFormat.Jpeg);
                                                }
                                            }

                                            await Task.Run(async () =>
                                            {
                                                await Task.Delay(1000);
                                            });

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
                                            webBrowser_new.Stop();
                                            //webBrowser_new.Refresh(WebBrowserRefreshOption.Completely);

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
                                                await Task.Delay(1000);
                                            });

                                            string datetime = label11.Text;
                                            string datetime_folder = label9.Text;
                                            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                            string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

                                            string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

                                            DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                            using (var pic = new Bitmap(webBrowser_new.Width - 18, webBrowser_new.Height - 18))
                                            {
                                                webBrowser_new.DrawToBitmap(pic, new Rectangle(0, 0, pic.Width, pic.Height));
                                                Bitmap resized = new Bitmap(pic, new Size(pic.Width / 2, pic.Height / 2));
                                                string domain_replace = label_domainhide.Text;
                                                StringBuilder sb_pic = new StringBuilder(domain_replace);
                                                sb_pic.Replace("\\", "");
                                                sb_pic.Replace("/", "");
                                                sb_pic.Replace("\"", "");
                                                sb_pic.Replace("*", "");
                                                sb_pic.Replace(":", "");
                                                sb_pic.Replace("?", "");
                                                sb_pic.Replace("<", "");
                                                sb_pic.Replace(">", "");
                                                sb_pic.Replace("|", "");
                                                sb_pic.Replace(" ", "");
                                                sb_pic.Replace("_", "");
                                                string full_path = path + "_" + label_macid.Text + "_n_" + sb_pic.ToString() + ".jpeg";
                                                resized.Save(full_path, ImageFormat.Jpeg);

                                                var fileLength = new FileInfo(full_path).Length;

                                                if (fileLength < 3200 && label_webtitle.Text == "Can’t reach this page" || fileLength < 3200 && label_webtitle.Text == "无法访问此页面")
                                                {
                                                    var access = new Bitmap(Properties.Resources.access);
                                                    access.Save(full_path, ImageFormat.Jpeg);
                                                }
                                                else if (fileLength < 3200 && label_webtitle.Text == "This site isn’t secure" || fileLength < 3200 && label_webtitle.Text == "此站点不安全")
                                                {
                                                    var secure = new Bitmap(Properties.Resources.secure);
                                                    secure.Save(full_path, ImageFormat.Jpeg);
                                                }
                                                else if (fileLength < 3200 && label_webtitle.Text == "Navigation Canceled" || fileLength < 3200 && label_webtitle.Text == "导航已取消")
                                                {
                                                    var navigation = new Bitmap(Properties.Resources.navigation);
                                                    navigation.Save(full_path, ImageFormat.Jpeg);
                                                }
                                                else
                                                {
                                                    resized.Save(full_path, ImageFormat.Jpeg);
                                                }
                                            }

                                            await Task.Run(async () =>
                                            {
                                                await Task.Delay(1000);
                                            });

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
                            Invoke(new Action(() =>
                            {
                                label_webtitle.Text = cefsharp_title;
                            }));

                            // inaccessible
                            if (label_webtitle.Text == label_domainhide.Text)
                            {
                                webBrowser_new.Stop();
                                //webBrowser_new.Refresh(WebBrowserRefreshOption.Completely);

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
                                    await Task.Delay(1000);
                                });

                                string datetime = label11.Text;
                                string datetime_folder = label9.Text;
                                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

                                string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

                                DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                using (var pic = new Bitmap(webBrowser_new.Width - 18, webBrowser_new.Height - 18))
                                {
                                    webBrowser_new.DrawToBitmap(pic, new Rectangle(0, 0, pic.Width, pic.Height));
                                    Bitmap resized = new Bitmap(pic, new Size(pic.Width / 2, pic.Height / 2));
                                    string domain_replace = label_domainhide.Text;
                                    StringBuilder sb_pic = new StringBuilder(domain_replace);
                                    sb_pic.Replace("\\", "");
                                    sb_pic.Replace("/", "");
                                    sb_pic.Replace("\"", "");
                                    sb_pic.Replace("*", "");
                                    sb_pic.Replace(":", "");
                                    sb_pic.Replace("?", "");
                                    sb_pic.Replace("<", "");
                                    sb_pic.Replace(">", "");
                                    sb_pic.Replace("|", "");
                                    sb_pic.Replace(" ", "");
                                    sb_pic.Replace("_", "");
                                    string full_path = path + "_" + label_macid.Text + "_n_" + sb_pic.ToString() + ".jpeg";
                                    resized.Save(full_path, ImageFormat.Jpeg);

                                    var fileLength = new FileInfo(full_path).Length;

                                    if (fileLength < 3200 && label_webtitle.Text == "Can’t reach this page" || fileLength < 3200 && label_webtitle.Text == "无法访问此页面")
                                    {
                                        var access = new Bitmap(Properties.Resources.access);
                                        access.Save(full_path, ImageFormat.Jpeg);
                                    }
                                    else if (fileLength < 3200 && label_webtitle.Text == "This site isn’t secure" || fileLength < 3200 && label_webtitle.Text == "此站点不安全")
                                    {
                                        var secure = new Bitmap(Properties.Resources.secure);
                                        secure.Save(full_path, ImageFormat.Jpeg);
                                    }
                                    else if (fileLength < 3200 && label_webtitle.Text == "Navigation Canceled" || fileLength < 3200 && label_webtitle.Text == "导航已取消")
                                    {
                                        var navigation = new Bitmap(Properties.Resources.navigation);
                                        navigation.Save(full_path, ImageFormat.Jpeg);
                                    }
                                    else
                                    {
                                        resized.Save(full_path, ImageFormat.Jpeg);
                                    }
                                }

                                await Task.Run(async () =>
                                {
                                    await Task.Delay(1000);
                                });

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

                                    //TopMost = false;
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
                                // Timeout Status
                                if (label_timeout.Text == "timeout")
                                {
                                    await Task.Run(async () =>
                                    {
                                        await Task.Delay(1000);
                                    });

                                    DataToTextFileTimeout();

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
                                    await Task.Run(async () =>
                                    {
                                        await Task.Delay(1000);
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
                            // hijacked
                            else
                            {
                                await Task.Run(async () =>
                                {
                                    await Task.Delay(1000);
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
                            buttonGoWasClicked = false;

                            fully_loaded = 0;
                            start_detect = 0;
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
                    // Detect when stop loads
                    detectnotloading = 0;
                    timer_detectnotloading.Stop();

                    Invoke(new Action(() =>
                    {
                        panel_browser_urgent.Controls.Add(chromeBrowser);
                        start_detect++;
                        label_start_detect.Text = start_detect.ToString();
                    }));

                    // Date preview
                    start_load = DateTime.Now.ToString("HH:mm:ss.fff");
                    start_load_inaccessible = DateTime.Now;

                    Invoke(new Action(() =>
                    {
                        i = 1;
                        timer_timeout.Start();
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
                            Invoke(new Action(() =>
                            {
                                panel_new.Visible = true;
                                panel_new.BringToFront();
                            }));
                            webBrowser_new.Navigate(label_domainhide_urgent.Text);
                            webBrowser_i++;
                        }
                    }
                    else
                    {
                        int webBrowser_i = 0;
                        while (webBrowser_i <= 2)
                        {
                            webBrowser_new.Navigate(label_domainhide_urgent.Text);
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
                    }));

                    if (label_fully_loaded.Text == "1")
                    {
                        Invoke(new Action(() =>
                        {
                            textBox_domain_urgent.Text = cefsharp_domain;
                        }));

                        // Inaccessible Status
                        string result = "";
                        string search_replace = label_webtitle_urgent.Text;

                        string upper_search = search_replace.ToUpper().ToString();

                        StringBuilder sb = new StringBuilder(upper_search);
                        sb.Replace("-", "");
                        sb.Replace(".", "");
                        sb.Replace(",", "");
                        sb.Replace("!", "");

                        string final_search = Regex.Replace(sb.ToString(), " {2,}", " ");

                        var final_inaccessble_lists = inaccessble_lists.Select(m => m.ToUpper());

                        string[] words = final_search.Split(' ');

                        int i = 0;
                        foreach (string word in words)
                        {
                            i++;

                            if (word != "")
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

                            if (i == 1 && search_replace == "")
                            {
                                result = "match";
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
                                        html = new WebClient().DownloadString(textBox_domain_urgent.Text);
                                    }
                                    catch (Exception)
                                    {
                                        // Leave blank
                                    }

                                    if (html.Contains("landing_image"))
                                    {
                                        // Timeout Status
                                        if (label_timeout.Text == "timeout")
                                        {
                                            await Task.Run(async () =>
                                            {
                                                await Task.Delay(1000);
                                            });

                                            DataToTextFileTimeout_Urgent();

                                            Invoke(new Action(() =>
                                            {
                                                // For timeout
                                                i = 1;
                                                timer_timeout.Stop();

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
                                            await Task.Run(async () =>
                                            {
                                                await Task.Delay(1000);
                                            });

                                            DataToTextFileSuccess_Urgent();

                                            Invoke(new Action(() =>
                                            {
                                                // For timeout
                                                i = 1;
                                                timer_timeout.Stop();

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
                                                i = 1;
                                                timer_timeout.Stop();

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
                                            i = 1;
                                            timer_timeout.Stop();

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
                                                webBrowser_new.Stop();
                                                //webBrowser_new.Refresh(WebBrowserRefreshOption.Completely);

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
                                                    await Task.Delay(1000);
                                                });

                                                string datetime = label11.Text;
                                                string datetime_folder = label9.Text;
                                                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                                string path = path_desktop + "\\rainCheck\\" + label_getdatetime_urgent.Text + "_urgent_" + i_timeout + "\\" + label_getdatetime_urgent.Text;

                                                string path_create_rainCheck = path_desktop + "\\rainCheck\\" + label_getdatetime_urgent.Text + "_urgent_" + i_timeout;

                                                DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                                using (var pic = new Bitmap(webBrowser_new.Width - 18, webBrowser_new.Height - 18))
                                                {
                                                    webBrowser_new.DrawToBitmap(pic, new Rectangle(0, 0, pic.Width, pic.Height));
                                                    Bitmap resized = new Bitmap(pic, new Size(pic.Width / 2, pic.Height / 2));
                                                    string domain_replace = label_domainhide_urgent.Text;
                                                    StringBuilder sb_pic = new StringBuilder(domain_replace);
                                                    sb_pic.Replace("\\", "");
                                                    sb_pic.Replace("/", "");
                                                    sb_pic.Replace("\"", "");
                                                    sb_pic.Replace("*", "");
                                                    sb_pic.Replace(":", "");
                                                    sb_pic.Replace("?", "");
                                                    sb_pic.Replace("<", "");
                                                    sb_pic.Replace(">", "");
                                                    sb_pic.Replace("|", "");
                                                    sb_pic.Replace(" ", "");
                                                    sb_pic.Replace("_", "");
                                                    string full_path = path + "_" + label_macid.Text + "_u_" + sb_pic.ToString() + ".jpeg";
                                                    resized.Save(full_path, ImageFormat.Jpeg);

                                                    var fileLength = new FileInfo(full_path).Length;

                                                    if (fileLength < 3200 && label_webtitle_urgent.Text == "Can’t reach this page" || fileLength < 3200 && label_webtitle_urgent.Text == "无法访问此页面")
                                                    {
                                                        var access = new Bitmap(Properties.Resources.access);
                                                        access.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                    else if (fileLength < 3200 && label_webtitle_urgent.Text == "This site isn’t secure" || fileLength < 3200 && label_webtitle_urgent.Text == "此站点不安全")
                                                    {
                                                        var secure = new Bitmap(Properties.Resources.secure);
                                                        secure.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                    else if (fileLength < 3200 && label_webtitle_urgent.Text == "Navigation Canceled" || fileLength < 3200 && label_webtitle_urgent.Text == "导航已取消")
                                                    {
                                                        var navigation = new Bitmap(Properties.Resources.navigation);
                                                        navigation.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                    else
                                                    {
                                                        resized.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                }

                                                await Task.Run(async () =>
                                                {
                                                    await Task.Delay(1000);
                                                });

                                                DataToTextFileInaccessible_Urgent();

                                                // For timeout
                                                i = 1;
                                                timer_timeout.Stop();

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
                                                webBrowser_new.Stop();
                                                //webBrowser_new.Refresh(WebBrowserRefreshOption.Completely);

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
                                                    await Task.Delay(1000);
                                                });

                                                string datetime = label11.Text;
                                                string datetime_folder = label9.Text;
                                                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                                string path = path_desktop + "\\rainCheck\\" + label_getdatetime_urgent.Text + "_urgent_" + i_timeout + "\\" + label_getdatetime_urgent.Text;

                                                string path_create_rainCheck = path_desktop + "\\rainCheck\\" + label_getdatetime_urgent.Text + "_urgent_" + i_timeout;

                                                DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                                using (var pic = new Bitmap(webBrowser_new.Width - 18, webBrowser_new.Height - 18))
                                                {
                                                    webBrowser_new.DrawToBitmap(pic, new Rectangle(0, 0, pic.Width, pic.Height));
                                                    Bitmap resized = new Bitmap(pic, new Size(pic.Width / 2, pic.Height / 2));
                                                    string domain_replace = label_domainhide_urgent.Text;
                                                    StringBuilder sb_pic = new StringBuilder(domain_replace);
                                                    sb_pic.Replace("\\", "");
                                                    sb_pic.Replace("/", "");
                                                    sb_pic.Replace("\"", "");
                                                    sb_pic.Replace("*", "");
                                                    sb_pic.Replace(":", "");
                                                    sb_pic.Replace("?", "");
                                                    sb_pic.Replace("<", "");
                                                    sb_pic.Replace(">", "");
                                                    sb_pic.Replace("|", "");
                                                    sb_pic.Replace(" ", "");
                                                    sb_pic.Replace("_", "");
                                                    string full_path = path + "_" + label_macid.Text + "_u_" + sb_pic.ToString() + ".jpeg";
                                                    resized.Save(full_path, ImageFormat.Jpeg);

                                                    var fileLength = new FileInfo(full_path).Length;

                                                    if (fileLength < 3200 && label_webtitle_urgent.Text == "Can’t reach this page" || fileLength < 3200 && label_webtitle_urgent.Text == "无法访问此页面")
                                                    {
                                                        var access = new Bitmap(Properties.Resources.access);
                                                        access.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                    else if (fileLength < 3200 && label_webtitle_urgent.Text == "This site isn’t secure" || fileLength < 3200 && label_webtitle_urgent.Text == "此站点不安全")
                                                    {
                                                        var secure = new Bitmap(Properties.Resources.secure);
                                                        secure.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                    else if (fileLength < 3200 && label_webtitle_urgent.Text == "Navigation Canceled" || fileLength < 3200 && label_webtitle_urgent.Text == "导航已取消")
                                                    {
                                                        var navigation = new Bitmap(Properties.Resources.navigation);
                                                        navigation.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                    else
                                                    {
                                                        resized.Save(full_path, ImageFormat.Jpeg);
                                                    }
                                                }

                                                await Task.Run(async () =>
                                                {
                                                    await Task.Delay(1000);
                                                });

                                                DataToTextFileInaccessible_Urgent();

                                                // For timeout
                                                i = 1;
                                                timer_timeout.Stop();

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

                                    Invoke(new Action(async () =>
                                    {
                                        label_inaccessible.Text = "inaccessible";

                                        TimeSpan span = end_load_inaccessible - start_load_inaccessible;
                                        int ms = (int)span.TotalMilliseconds;

                                        // for fast load
                                        if (ms < 500)
                                        {
                                            webBrowser_new.Stop();
                                            //webBrowser_new.Refresh(WebBrowserRefreshOption.Completely);

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
                                                await Task.Delay(1000);
                                            });

                                            string datetime = label11.Text;
                                            string datetime_folder = label9.Text;
                                            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                            string path = path_desktop + "\\rainCheck\\" + label_getdatetime_urgent.Text + "_urgent_" + i_timeout + "\\" + label_getdatetime_urgent.Text;

                                            string path_create_rainCheck = path_desktop + "\\rainCheck\\" + label_getdatetime_urgent.Text + "_urgent_" + i_timeout;

                                            DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                            using (var pic = new Bitmap(webBrowser_new.Width - 18, webBrowser_new.Height - 18))
                                            {
                                                webBrowser_new.DrawToBitmap(pic, new Rectangle(0, 0, pic.Width, pic.Height));
                                                Bitmap resized = new Bitmap(pic, new Size(pic.Width / 2, pic.Height / 2));
                                                string domain_replace = label_domainhide_urgent.Text;
                                                StringBuilder sb_pic = new StringBuilder(domain_replace);
                                                sb_pic.Replace("\\", "");
                                                sb_pic.Replace("/", "");
                                                sb_pic.Replace("\"", "");
                                                sb_pic.Replace("*", "");
                                                sb_pic.Replace(":", "");
                                                sb_pic.Replace("?", "");
                                                sb_pic.Replace("<", "");
                                                sb_pic.Replace(">", "");
                                                sb_pic.Replace("|", "");
                                                sb_pic.Replace(" ", "");
                                                sb_pic.Replace("_", "");
                                                string full_path = path + "_" + label_macid.Text + "_u_" + sb_pic.ToString() + ".jpeg";
                                                resized.Save(full_path, ImageFormat.Jpeg);

                                                var fileLength = new FileInfo(full_path).Length;

                                                if (fileLength < 3200 && label_webtitle_urgent.Text == "Can’t reach this page" || fileLength < 3200 && label_webtitle_urgent.Text == "无法访问此页面")
                                                {
                                                    var access = new Bitmap(Properties.Resources.access);
                                                    access.Save(full_path, ImageFormat.Jpeg);
                                                }
                                                else if (fileLength < 3200 && label_webtitle_urgent.Text == "This site isn’t secure" || fileLength < 3200 && label_webtitle_urgent.Text == "此站点不安全")
                                                {
                                                    var secure = new Bitmap(Properties.Resources.secure);
                                                    secure.Save(full_path, ImageFormat.Jpeg);
                                                }
                                                else if (fileLength < 3200 && label_webtitle_urgent.Text == "Navigation Canceled" || fileLength < 3200 && label_webtitle_urgent.Text == "导航已取消")
                                                {
                                                    var navigation = new Bitmap(Properties.Resources.navigation);
                                                    navigation.Save(full_path, ImageFormat.Jpeg);
                                                }
                                                else
                                                {
                                                    resized.Save(full_path, ImageFormat.Jpeg);
                                                }
                                            }

                                            await Task.Run(async () =>
                                            {
                                                await Task.Delay(1000);
                                            });

                                            DataToTextFileInaccessible_Urgent();

                                            // For timeout
                                            i = 1;
                                            timer_timeout.Stop();

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
                                            webBrowser_new.Stop();
                                            //webBrowser_new.Refresh(WebBrowserRefreshOption.Completely);

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
                                                await Task.Delay(1000);
                                            });

                                            string datetime = label11.Text;
                                            string datetime_folder = label9.Text;
                                            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                            string path = path_desktop + "\\rainCheck\\" + label_getdatetime_urgent.Text + "_urgent_" + i_timeout + "\\" + label_getdatetime_urgent.Text;

                                            string path_create_rainCheck = path_desktop + "\\rainCheck\\" + label_getdatetime_urgent.Text + "_urgent_" + i_timeout;

                                            DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                            using (var pic = new Bitmap(webBrowser_new.Width - 18, webBrowser_new.Height - 18))
                                            {
                                                webBrowser_new.DrawToBitmap(pic, new Rectangle(0, 0, pic.Width, pic.Height));
                                                Bitmap resized = new Bitmap(pic, new Size(pic.Width / 2, pic.Height / 2));
                                                string domain_replace = label_domainhide_urgent.Text;
                                                StringBuilder sb_pic = new StringBuilder(domain_replace);
                                                sb_pic.Replace("\\", "");
                                                sb_pic.Replace("/", "");
                                                sb_pic.Replace("\"", "");
                                                sb_pic.Replace("*", "");
                                                sb_pic.Replace(":", "");
                                                sb_pic.Replace("?", "");
                                                sb_pic.Replace("<", "");
                                                sb_pic.Replace(">", "");
                                                sb_pic.Replace("|", "");
                                                sb_pic.Replace(" ", "");
                                                sb_pic.Replace("_", "");
                                                string full_path = path + "_" + label_macid.Text + "_u_" + sb_pic.ToString() + ".jpeg";
                                                resized.Save(full_path, ImageFormat.Jpeg);

                                                var fileLength = new FileInfo(full_path).Length;

                                                if (fileLength < 3200 && label_webtitle_urgent.Text == "Can’t reach this page" || fileLength < 3200 && label_webtitle_urgent.Text == "无法访问此页面")
                                                {
                                                    var access = new Bitmap(Properties.Resources.access);
                                                    access.Save(full_path, ImageFormat.Jpeg);
                                                }
                                                else if (fileLength < 3200 && label_webtitle_urgent.Text == "This site isn’t secure" || fileLength < 3200 && label_webtitle_urgent.Text == "此站点不安全")
                                                {
                                                    var secure = new Bitmap(Properties.Resources.secure);
                                                    secure.Save(full_path, ImageFormat.Jpeg);
                                                }
                                                else if (fileLength < 3200 && label_webtitle_urgent.Text == "Navigation Canceled" || fileLength < 3200 && label_webtitle_urgent.Text == "导航已取消")
                                                {
                                                    var navigation = new Bitmap(Properties.Resources.navigation);
                                                    navigation.Save(full_path, ImageFormat.Jpeg);
                                                }
                                                else
                                                {
                                                    resized.Save(full_path, ImageFormat.Jpeg);
                                                }
                                            }

                                            await Task.Run(async () =>
                                            {
                                                await Task.Delay(1000);
                                            });

                                            DataToTextFileInaccessible_Urgent();

                                            // For timeout
                                            i = 1;
                                            timer_timeout.Stop();

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
                            Invoke(new Action(() =>
                            {
                                label_webtitle_urgent.Text = cefsharp_title;
                            }));

                            // inaccessible
                            if (label_webtitle_urgent.Text == label_domainhide_urgent.Text)
                            {
                                webBrowser_new.Stop();
                                //webBrowser_new.Refresh(WebBrowserRefreshOption.Completely);

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
                                    await Task.Delay(1000);
                                });

                                string datetime = label11.Text;
                                string datetime_folder = label9.Text;
                                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                string path = path_desktop + "\\rainCheck\\" + label_getdatetime_urgent.Text + "_urgent_" + i_timeout + "\\" + label_getdatetime_urgent.Text;

                                string path_create_rainCheck = path_desktop + "\\rainCheck\\" + label_getdatetime_urgent.Text + "_urgent_" + i_timeout;

                                DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

                                using (var pic = new Bitmap(webBrowser_new.Width - 18, webBrowser_new.Height - 18))
                                {
                                    webBrowser_new.DrawToBitmap(pic, new Rectangle(0, 0, pic.Width, pic.Height));
                                    Bitmap resized = new Bitmap(pic, new Size(pic.Width / 2, pic.Height / 2));
                                    string domain_replace = label_domainhide_urgent.Text;
                                    StringBuilder sb_pic = new StringBuilder(domain_replace);
                                    sb_pic.Replace("\\", "");
                                    sb_pic.Replace("/", "");
                                    sb_pic.Replace("\"", "");
                                    sb_pic.Replace("*", "");
                                    sb_pic.Replace(":", "");
                                    sb_pic.Replace("?", "");
                                    sb_pic.Replace("<", "");
                                    sb_pic.Replace(">", "");
                                    sb_pic.Replace("|", "");
                                    sb_pic.Replace(" ", "");
                                    sb_pic.Replace("_", "");
                                    string full_path = path + "_" + label_macid.Text + "_u_" + sb_pic.ToString() + ".jpeg";
                                    resized.Save(full_path, ImageFormat.Jpeg);

                                    var fileLength = new FileInfo(full_path).Length;

                                    if (fileLength < 3200 && label_webtitle_urgent.Text == "Can’t reach this page" || fileLength < 3200 && label_webtitle_urgent.Text == "无法访问此页面")
                                    {
                                        var access = new Bitmap(Properties.Resources.access);
                                        access.Save(full_path, ImageFormat.Jpeg);
                                    }
                                    else if (fileLength < 3200 && label_webtitle_urgent.Text == "This site isn’t secure" || fileLength < 3200 && label_webtitle_urgent.Text == "此站点不安全")
                                    {
                                        var secure = new Bitmap(Properties.Resources.secure);
                                        secure.Save(full_path, ImageFormat.Jpeg);
                                    }
                                    else if (fileLength < 3200 && label_webtitle_urgent.Text == "Navigation Canceled" || fileLength < 3200 && label_webtitle_urgent.Text == "导航已取消")
                                    {
                                        var navigation = new Bitmap(Properties.Resources.navigation);
                                        navigation.Save(full_path, ImageFormat.Jpeg);
                                    }
                                    else
                                    {
                                        resized.Save(full_path, ImageFormat.Jpeg);
                                    }
                                }

                                await Task.Run(async () =>
                                {
                                    await Task.Delay(1000);
                                });

                                DataToTextFileInaccessible_Urgent();

                                Invoke(new Action(() =>
                                {
                                    // For timeout
                                    i = 1;
                                    timer_timeout.Stop();

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
                                // Timeout Status
                                if (label_timeout.Text == "timeout")
                                {
                                    await Task.Run(async () =>
                                    {
                                        await Task.Delay(1000);
                                    });

                                    DataToTextFileTimeout_Urgent();

                                    Invoke(new Action(() =>
                                    {
                                        // For timeout
                                        i = 1;
                                        timer_timeout.Stop();

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
                                    await Task.Run(async () =>
                                    {
                                        await Task.Delay(1000);
                                    });

                                    DataToTextFileSuccess_Urgent();

                                    Invoke(new Action(() =>
                                    {
                                        // For timeout
                                        i = 1;
                                        timer_timeout.Stop();

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
                            // hijacked
                            else
                            {
                                await Task.Run(async () =>
                                {
                                    await Task.Delay(1000);
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
                                    i = 1;
                                    timer_timeout.Stop();

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

        // Main
        private void DataToTextFileSuccess()
        {
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
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        string webtitle_replace = label_webtitle.Text;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        string webtitle_get = webtitle.ToString();
                        if (webtitle_get == "")
                        {
                            webtitle_get = "-";
                        }
                        
                        StringBuilder start_load_replace = new StringBuilder(start_load);
                        start_load_replace.Replace(":", "");
                        start_load_replace.Replace(".", "");

                        StringBuilder end_load_replace = new StringBuilder(end_load);
                        end_load_replace.Replace(":", "");
                        end_load_replace.Replace(".", "");

                        if (Convert.ToInt32(start_load_replace.ToString()) < Convert.ToInt32(end_load_replace.ToString()))
                        {
                            DateTime start_load_timespan = DateTime.ParseExact(start_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            DateTime end_load_timespan = DateTime.ParseExact(end_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            TimeSpan span = end_load_timespan - start_load_timespan;
                            int sec = (int)span.TotalSeconds;

                            int timeout_get = Convert.ToInt32(label13.Text) + 10;

                            if (sec < timeout_get)
                            {
                                StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                                swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + webtitle_get + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + ",-," + datetime + "," + ",N");

                                swww.Close();
                            }
                        }
                    }
                }
                else
                {
                    // Create directory
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        string webtitle_replace = label_webtitle.Text;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        string webtitle_get = webtitle.ToString();
                        if (webtitle_get == "")
                        {
                            webtitle_get = "-";
                        }

                        StringBuilder start_load_replace = new StringBuilder(start_load);
                        start_load_replace.Replace(":", "");
                        start_load_replace.Replace(".", "");

                        StringBuilder end_load_replace = new StringBuilder(end_load);
                        end_load_replace.Replace(":", "");
                        end_load_replace.Replace(".", "");

                        if (Convert.ToInt32(start_load_replace.ToString()) < Convert.ToInt32(end_load_replace.ToString()))
                        {
                            DateTime start_load_timespan = DateTime.ParseExact(start_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            DateTime end_load_timespan = DateTime.ParseExact(end_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            TimeSpan span = end_load_timespan - start_load_timespan;
                            int sec = (int)span.TotalSeconds;

                            int timeout_get = Convert.ToInt32(label13.Text) + 10;

                            if (sec < timeout_get)
                            {
                                StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                                swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + webtitle_get + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + ",-," + datetime + "," + ",N");

                                swww.Close();
                            }
                        }
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
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        string webtitle_replace = label_webtitle.Text;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        StringBuilder start_load_replace = new StringBuilder(start_load);
                        start_load_replace.Replace(":", "");
                        start_load_replace.Replace(".", "");

                        StringBuilder end_load_replace = new StringBuilder(end_load);
                        end_load_replace.Replace(":", "");
                        end_load_replace.Replace(".", "");

                        if (Convert.ToInt32(start_load_replace.ToString()) < Convert.ToInt32(end_load_replace.ToString()))
                        {
                            DateTime start_load_timespan = DateTime.ParseExact(start_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            DateTime end_load_timespan = DateTime.ParseExact(end_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            TimeSpan span = end_load_timespan - start_load_timespan;
                            int sec = (int)span.TotalSeconds;

                            int timeout_get = Convert.ToInt32(label13.Text) + 10;

                            if (sec < timeout_get)
                            {
                                StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                                swww.WriteLine("," + label_domainhide.Text + ",T" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + ",-," + datetime + "," + ",N");

                                swww.Close();
                            }
                        }
                    }
                }
                else
                {
                    // Create directory
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        string webtitle_replace = label_webtitle.Text;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        StringBuilder start_load_replace = new StringBuilder(start_load);
                        start_load_replace.Replace(":", "");
                        start_load_replace.Replace(".", "");

                        StringBuilder end_load_replace = new StringBuilder(end_load);
                        end_load_replace.Replace(":", "");
                        end_load_replace.Replace(".", "");

                        if (Convert.ToInt32(start_load_replace.ToString()) < Convert.ToInt32(end_load_replace.ToString()))
                        {
                            DateTime start_load_timespan = DateTime.ParseExact(start_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            DateTime end_load_timespan = DateTime.ParseExact(end_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            TimeSpan span = end_load_timespan - start_load_timespan;
                            int sec = (int)span.TotalSeconds;

                            int timeout_get = Convert.ToInt32(label13.Text) + 10;

                            if (sec < timeout_get)
                            {
                                StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                                swww.WriteLine("," + label_domainhide.Text + ",T" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + ",-," + datetime + "," + ",N");

                                swww.Close();
                            }
                        }
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
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        //swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + "," + textBox_domain.Text + "," + "," + "," + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

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

                        string webtitle_replace = label_webtitle.Text;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        swww.WriteLine("," + label_domainhide.Text + ",H" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + "," + textBox_domain.Text + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + ",-," + datetime + "," + ",N");
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
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        //swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + "," + textBox_domain.Text + "," + "," + "," + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

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

                        string webtitle_replace = label_webtitle.Text;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        swww.WriteLine("," + label_domainhide.Text + ",H" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + "," + textBox_domain.Text + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + ",-," + datetime + "," + ",N");

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
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

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
                        
                        string domain_replace = label_domainhide.Text;
                        StringBuilder sb_pic = new StringBuilder(domain_replace);
                        sb_pic.Replace("\\", "");
                        sb_pic.Replace("/", "");
                        sb_pic.Replace("\"", "");
                        sb_pic.Replace("*", "");
                        sb_pic.Replace(":", "");
                        sb_pic.Replace("?", "");
                        sb_pic.Replace("<", "");
                        sb_pic.Replace(">", "");
                        sb_pic.Replace("|", "");
                        sb_pic.Replace(" ", "");
                        sb_pic.Replace("_", "");

                        string webtitle_replace = label_webtitle.Text;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        StringBuilder start_load_replace = new StringBuilder(start_load);
                        start_load_replace.Replace(":", "");
                        start_load_replace.Replace(".", "");

                        StringBuilder end_load_replace = new StringBuilder(end_load);
                        end_load_replace.Replace(":", "");
                        end_load_replace.Replace(".", "");

                        if (Convert.ToInt32(start_load_replace.ToString()) < Convert.ToInt32(end_load_replace.ToString()))
                        {
                            DateTime start_load_timespan = DateTime.ParseExact(start_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            DateTime end_load_timespan = DateTime.ParseExact(end_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            TimeSpan span = end_load_timespan - start_load_timespan;
                            int sec = (int)span.TotalSeconds;

                            int timeout_get = Convert.ToInt32(label13.Text) + 10;

                            if (sec < timeout_get)
                            {
                                swww.WriteLine("," + label_domainhide.Text + ",I" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + "," + error_message + "," + datetime_folder + "_" + label_macid.Text + "_n_" + sb_pic.ToString() + "," + isp_get + "," + city_get + ",-," + datetime + "," + ",N");
                            }
                        }

                        swww.Close();
                    }
                }
                else
                {
                    // Create directory
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

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

                        string domain_replace = label_domainhide.Text;
                        StringBuilder sb_pic = new StringBuilder(domain_replace);
                        sb_pic.Replace("\\", "");
                        sb_pic.Replace("/", "");
                        sb_pic.Replace("\"", "");
                        sb_pic.Replace("*", "");
                        sb_pic.Replace(":", "");
                        sb_pic.Replace("?", "");
                        sb_pic.Replace("<", "");
                        sb_pic.Replace(">", "");
                        sb_pic.Replace("|", "");
                        sb_pic.Replace(" ", "");
                        sb_pic.Replace("_", "");

                        string webtitle_replace = label_webtitle.Text;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        StringBuilder start_load_replace = new StringBuilder(start_load);
                        start_load_replace.Replace(":", "");
                        start_load_replace.Replace(".", "");

                        StringBuilder end_load_replace = new StringBuilder(end_load);
                        end_load_replace.Replace(":", "");
                        end_load_replace.Replace(".", "");

                        if (Convert.ToInt32(start_load_replace.ToString()) < Convert.ToInt32(end_load_replace.ToString()))
                        {
                            DateTime start_load_timespan = DateTime.ParseExact(start_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            DateTime end_load_timespan = DateTime.ParseExact(end_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            TimeSpan span = end_load_timespan - start_load_timespan;
                            int sec = (int)span.TotalSeconds;

                            int timeout_get = Convert.ToInt32(label13.Text) + 10;

                            if (sec < timeout_get)
                            {
                                swww.WriteLine("," + label_domainhide.Text + ",I" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + "," + error_message + "," + datetime_folder + "_" + label_macid.Text + "_n_" + sb_pic.ToString() + "," + isp_get + "," + city_get + ",-," + datetime + "," + ",N");
                            }
                        }

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
            try
            {
                string datetime = label11.Text;
                string datetime_folder = label9.Text;
                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                string path = path_desktop + "\\rainCheck\\" + label_getdatetime_urgent.Text + "_urgent_" + i_timeout;

                if (Directory.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path + "\\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide_urgent.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        string webtitle_replace = label_webtitle_urgent.Text;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        string webtitle_get = webtitle.ToString();
                        if (webtitle_get == "")
                        {
                            webtitle_get = "-";
                        }

                        StringBuilder start_load_replace = new StringBuilder(start_load);
                        start_load_replace.Replace(":", "");
                        start_load_replace.Replace(".", "");

                        StringBuilder end_load_replace = new StringBuilder(end_load);
                        end_load_replace.Replace(":", "");
                        end_load_replace.Replace(".", "");

                        if (Convert.ToInt32(start_load_replace.ToString()) < Convert.ToInt32(end_load_replace.ToString()))
                        {
                            DateTime start_load_timespan = DateTime.ParseExact(start_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            DateTime end_load_timespan = DateTime.ParseExact(end_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            TimeSpan span = end_load_timespan - start_load_timespan;
                            int sec = (int)span.TotalSeconds;

                            int timeout_get = Convert.ToInt32(label13.Text) + 10;

                            if (sec < timeout_get)
                            {
                                StreamWriter swww = new StreamWriter(path + @"\result.txt", true, System.Text.Encoding.UTF8);
                                swww.WriteLine("," + label_domainhide_urgent.Text + ",S" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + webtitle_get + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + label_utype.Text + "," + label_datetimetextfile_urgent.Text + "," + ",U");

                                swww.Close();
                            }
                        }
                    }
                }
                else
                {
                    // Create directory
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path + "\\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide_urgent.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        string webtitle_replace = label_webtitle_urgent.Text;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        string webtitle_get = webtitle.ToString();
                        if (webtitle_get == "")
                        {
                            webtitle_get = "-";
                        }

                        StringBuilder start_load_replace = new StringBuilder(start_load);
                        start_load_replace.Replace(":", "");
                        start_load_replace.Replace(".", "");

                        StringBuilder end_load_replace = new StringBuilder(end_load);
                        end_load_replace.Replace(":", "");
                        end_load_replace.Replace(".", "");

                        if (Convert.ToInt32(start_load_replace.ToString()) < Convert.ToInt32(end_load_replace.ToString()))
                        {
                            DateTime start_load_timespan = DateTime.ParseExact(start_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            DateTime end_load_timespan = DateTime.ParseExact(end_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            TimeSpan span = end_load_timespan - start_load_timespan;
                            int sec = (int)span.TotalSeconds;

                            int timeout_get = Convert.ToInt32(label13.Text) + 10;

                            if (sec < timeout_get)
                            {
                                StreamWriter swww = new StreamWriter(path + @"\result.txt", true, System.Text.Encoding.UTF8);
                                swww.WriteLine("," + label_domainhide_urgent.Text + ",S" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + webtitle_get + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + label_utype.Text + "," + label_datetimetextfile_urgent.Text + "," + ",U");

                                swww.Close();
                            }
                        }
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
            try
            {
                string datetime = label11.Text;
                string datetime_folder = label9.Text;
                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                string path = path_desktop + "\\rainCheck\\" + label_getdatetime_urgent.Text + "_urgent_" + i_timeout;

                if (Directory.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path + "\\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide_urgent.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        string webtitle_replace = label_webtitle_urgent.Text;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        StringBuilder start_load_replace = new StringBuilder(start_load);
                        start_load_replace.Replace(":", "");
                        start_load_replace.Replace(".", "");

                        StringBuilder end_load_replace = new StringBuilder(end_load);
                        end_load_replace.Replace(":", "");
                        end_load_replace.Replace(".", "");

                        if (Convert.ToInt32(start_load_replace.ToString()) < Convert.ToInt32(end_load_replace.ToString()))
                        {
                            DateTime start_load_timespan = DateTime.ParseExact(start_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            DateTime end_load_timespan = DateTime.ParseExact(end_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            TimeSpan span = end_load_timespan - start_load_timespan;
                            int sec = (int)span.TotalSeconds;

                            int timeout_get = Convert.ToInt32(label13.Text) + 10;

                            if (sec < timeout_get)
                            {
                                StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                                swww.WriteLine("," + label_domainhide_urgent.Text + ",T" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + label_utype.Text + "," + label_datetimetextfile_urgent.Text + "," + ",U");

                                swww.Close();
                            }
                        }
                    }
                }
                else
                {
                    // Create directory
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path + "\\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_text = label_domainhide_urgent.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        string webtitle_replace = label_webtitle_urgent.Text;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        StringBuilder start_load_replace = new StringBuilder(start_load);
                        start_load_replace.Replace(":", "");
                        start_load_replace.Replace(".", "");

                        StringBuilder end_load_replace = new StringBuilder(end_load);
                        end_load_replace.Replace(":", "");
                        end_load_replace.Replace(".", "");

                        if (Convert.ToInt32(start_load_replace.ToString()) < Convert.ToInt32(end_load_replace.ToString()))
                        {
                            DateTime start_load_timespan = DateTime.ParseExact(start_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            DateTime end_load_timespan = DateTime.ParseExact(end_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            TimeSpan span = end_load_timespan - start_load_timespan;
                            int sec = (int)span.TotalSeconds;

                            int timeout_get = Convert.ToInt32(label13.Text) + 10;

                            if (sec < timeout_get)
                            {
                                StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                                swww.WriteLine("," + label_domainhide_urgent.Text + ",T" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + label_utype.Text + "," + label_datetimetextfile_urgent.Text + "," + ",U");

                                swww.Close();
                            }
                        }
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
            try
            {
                string datetime = label11.Text;
                string datetime_folder = label9.Text;
                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                string path = path_desktop + "\\rainCheck\\" + label_getdatetime_urgent.Text + "_urgent_" + i_timeout;

                if (Directory.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        //swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + "," + textBox_domain.Text + "," + "," + "," + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

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

                        string webtitle_replace = label_webtitle_urgent.Text;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        swww.WriteLine("," + label_domainhide_urgent.Text + ",H" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + "," + textBox_domain_urgent.Text + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + label_utype.Text + "," + label_datetimetextfile_urgent.Text + "," + ",U");
                        swww.Close();
                    }
                }
                else
                {
                    // Create directory
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        //swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + "," + textBox_domain.Text + "," + "," + "," + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

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

                        string webtitle_replace = label_webtitle_urgent.Text;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        swww.WriteLine("," + label_domainhide_urgent.Text + ",H" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + "," + textBox_domain_urgent.Text + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + label_utype.Text + "," + label_datetimetextfile_urgent.Text + "," + ",U");
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
            try
            {
                string datetime = label11.Text;
                string datetime_folder = label9.Text;
                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                string path = path_desktop + "\\rainCheck\\" + label_getdatetime_urgent.Text + "_urgent_" + i_timeout;

                if (Directory.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path + "\\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

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
                        
                        string domain_replace = label_domainhide_urgent.Text;
                        StringBuilder sb_pic = new StringBuilder(domain_replace);
                        sb_pic.Replace("\\", "");
                        sb_pic.Replace("/", "");
                        sb_pic.Replace("\"", "");
                        sb_pic.Replace("*", "");
                        sb_pic.Replace(":", "");
                        sb_pic.Replace("?", "");
                        sb_pic.Replace("<", "");
                        sb_pic.Replace(">", "");
                        sb_pic.Replace("|", "");
                        sb_pic.Replace(" ", "");
                        sb_pic.Replace("_", "");

                        string webtitle_replace = label_webtitle_urgent.Text;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        StringBuilder start_load_replace = new StringBuilder(start_load);
                        start_load_replace.Replace(":", "");
                        start_load_replace.Replace(".", "");

                        StringBuilder end_load_replace = new StringBuilder(end_load);
                        end_load_replace.Replace(":", "");
                        end_load_replace.Replace(".", "");

                        if (Convert.ToInt32(start_load_replace.ToString()) < Convert.ToInt32(end_load_replace.ToString()))
                        {
                            DateTime start_load_timespan = DateTime.ParseExact(start_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            DateTime end_load_timespan = DateTime.ParseExact(end_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            TimeSpan span = end_load_timespan - start_load_timespan;
                            int sec = (int)span.TotalSeconds;

                            int timeout_get = Convert.ToInt32(label13.Text) + 10;

                            if (sec < timeout_get)
                            {
                                swww.WriteLine("," + label_domainhide_urgent.Text + ",I" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + "," + error_message + "," + datetime_folder + "_" + label_macid.Text + "_u_" + sb_pic.ToString() + "," + isp_get + "," + city_get + "," + label_utype.Text + "," + label_datetimetextfile_urgent.Text + "," + ",U");
                            }
                        }

                        swww.Close();
                    }
                }
                else
                {
                    // Create directory
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path + "\\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, System.Text.Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

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

                        string domain_replace = label_domainhide_urgent.Text;
                        StringBuilder sb_pic = new StringBuilder(domain_replace);
                        sb_pic.Replace("\\", "");
                        sb_pic.Replace("/", "");
                        sb_pic.Replace("\"", "");
                        sb_pic.Replace("*", "");
                        sb_pic.Replace(":", "");
                        sb_pic.Replace("?", "");
                        sb_pic.Replace("<", "");
                        sb_pic.Replace(">", "");
                        sb_pic.Replace("|", "");
                        sb_pic.Replace(" ", "");
                        sb_pic.Replace("_", "");

                        string webtitle_replace = label_webtitle_urgent.Text;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        StringBuilder start_load_replace = new StringBuilder(start_load);
                        start_load_replace.Replace(":", "");
                        start_load_replace.Replace(".", "");

                        StringBuilder end_load_replace = new StringBuilder(end_load);
                        end_load_replace.Replace(":", "");
                        end_load_replace.Replace(".", "");

                        if (Convert.ToInt32(start_load_replace.ToString()) < Convert.ToInt32(end_load_replace.ToString()))
                        {
                            DateTime start_load_timespan = DateTime.ParseExact(start_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            DateTime end_load_timespan = DateTime.ParseExact(end_load, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            TimeSpan span = end_load_timespan - start_load_timespan;
                            int sec = (int)span.TotalSeconds;

                            int timeout_get = Convert.ToInt32(label13.Text) + 10;

                            if (sec < timeout_get)
                            {
                                swww.WriteLine("," + label_domainhide_urgent.Text + ",I" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + "," + error_message + "," + datetime_folder + "_" + label_macid.Text + "_u_" + sb_pic.ToString() + "," + isp_get + "," + city_get + "," + label_utype.Text + "," + label_datetimetextfile_urgent.Text + "," + ",U");
                            }
                        }

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
            else
            {
                Cef.Shutdown();
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

                if (SetResult == "Yes")
                {
                    label_domainscount.Text = "Total: " + (index + 1) + " of " + domain_total.ToString();
                    SetResult = "";
                }
                else
                {
                    label_domainscount.Text = "Total: " + (index + 2) + " of " + domain_total.ToString();
                }

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
                    string path_lastcurrentindex = Path.GetTempPath() + @"\raincheck_lastcurrentindex.txt";
                    string path_brand = Path.GetTempPath() + @"\raincheck_brand.txt";
                    string path_datetime = Path.GetTempPath() + @"\raincheck_datetime.txt";

                    if (File.Exists(path_lastcurrentindex))
                    {
                        File.Delete(path_lastcurrentindex);
                    }

                    if (File.Exists(path_brand))
                    {
                        File.Delete(path_brand);
                    }

                    if (File.Exists(path_datetime))
                    {
                        File.Delete(path_datetime);
                    }

                    label_status.Text = "[Loading]";

                    index = 0;
                    label_currentindex.Text = "0";

                    label_domainscount.Text = "Total: " + domain_total.ToString();

                    // else loaded
                    elseloaded_i = 0;
                    timer_elseloaded.Stop();
                    
                    pictureBox_loader.Visible = false;
                    textBox_domain.Text = "";

                    // Enable visible buttons
                    button_start.Visible = true;
                    button_pause.Visible = false;
                    button_start.Enabled = false;
                    button_startover.Enabled = false;
                    pictureBox_loader.Visible = false;

                    timer_domain.Stop();
                    
                    // Detect when stop loads
                    detectnotloading = 0;
                    timer_detectnotloading.Stop();

                    // ms_detect = 0;
                    fully_loaded = 0;
                    start_detect = 0;

                    // set time for next to false
                    timerfornext = false;
                                       
                    string datetime_folder = label9.Text;
                    string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                    string path = path_desktop + "\\rainCheck\\" + datetime_folder;
                    string read = "";

                    // Insert
                    if (File.Exists(path + "\\result.txt"))
                    {
                        read = File.ReadAllText(path + "\\result.txt");
                    }
                    else
                    {
                        string path_autoyes = Path.GetTempPath() + @"\raincheck_autoyes.txt";
                        StreamWriter sw_autoyes = new StreamWriter(path_autoyes, true, Encoding.UTF8);
                        sw_autoyes.Close();

                        can_close = false;
                        Close();
                        Application.Restart();
                    }

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
                            zip.Password = "r@inCh3ckd234b70";
                            zip.AddDirectory(path);
                            zip.Save(outputpath);

                            if (Directory.Exists(path))
                            {
                                Directory.Delete(path, true);
                            }
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1014", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
                    
                    // Random domains
                    dataGridView_domain.DataSource = null;
                    APIGetDomains();
                    dataGridView_domain.ClearSelection();
                    chromeBrowser.Dock = DockStyle.None;
                    dataGridView_domain.Columns["domain_name"].Visible = false;
                    dataGridView_domain.Columns["id"].Visible = false;
                    dataGridView_domain.Columns["text_search"].Visible = false;
                    dataGridView_domain.Columns["website_type"].Visible = false;

                    label_domainscount.Text = "Total: " + dataGridView_domain.RowCount.ToString();

                    webBrowser_new.Refresh(WebBrowserRefreshOption.Completely);
                }
                else
                {
                    string path = Path.GetTempPath() + @"\raincheck_lastcurrentindex.txt";
                    if (File.Exists(path))
                    {
                        File.Delete(path);

                        StreamWriter sw_create = new StreamWriter(path, true, Encoding.UTF8);
                        sw_create.Close();

                        StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8);
                        sw.Write(index);
                        sw.Close();
                    }
                    else
                    {
                        StreamWriter sw_create = new StreamWriter(path, true, Encoding.UTF8);
                        sw_create.Close();

                        StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8);
                        sw.Write(index);
                        sw.Close();
                    }

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
            
            fully_loaded = 0;
            start_detect = 0;
        }

        private void Button_resume_Click(object sender, EventArgs e)
        {
            if (SetResult == "No" || SetResult == "Not Exists")
            {
                if (!buttonDetect)
                {
                    if (label_currentindex.Text == "0")
                    {
                        string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string replace = label_timefor.Text.Replace(":", "");

                        string path = path_desktop + "\\rainCheck\\" + label9.Text + replace;
                        string path_noreplace = path_desktop + "\\rainCheck\\" + label9.Text;

                        if (Directory.Exists(path))
                        {
                            Directory.Delete(path, true);
                        }

                        if (Directory.Exists(path_noreplace))
                        {
                            Directory.Delete(path_noreplace, true);
                        }
                    }
                }
            }

            pictureBox_loader.Visible = true;

            // Set browser panel dock style
            panel_browser.Visible = true;
            panel_browser.BringToFront();
            panel_browser.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;

            timer_blink.Stop();
            label_status.Visible = true;
            label_status.Text = "[Running]";
            timer_domain.Start();

            // For timeout
            i = 1;
            timer_timeout.Start();

            if (SetResult == "Yes")
            {
                string lastcurrentindex = label_currentindex.Text = LastCurrentIndex;
                index = Convert.ToInt32(LastCurrentIndex);
                dataGridView_domain.FirstDisplayedScrollingRowIndex = index-1;
                label_domainscount.Text = "Total: " + lastcurrentindex + " of " + domain_total.ToString();
                int getCurrentIndex = Convert.ToInt32(lastcurrentindex);
                dataGridView_domain.ClearSelection();
                dataGridView_domain.Rows[getCurrentIndex-1].Selected = true;
            }
            else
            {
                label_domainscount.Text = "Total: " + (index + 1) + " of " + domain_total.ToString();
                int getCurrentIndex = Convert.ToInt32(label_currentindex.Text);
                dataGridView_domain.ClearSelection();
                dataGridView_domain.Rows[getCurrentIndex].Selected = true;
            }

            string path_datetime = Path.GetTempPath() + @"\raincheck_datetime.txt";
            if (!File.Exists(path_datetime))
            {
                StreamWriter sw_create = new StreamWriter(path_datetime, true, Encoding.UTF8);
                sw_create.Close();

                StreamWriter sw = new StreamWriter(path_datetime, true, Encoding.UTF8);
                sw.Write(DateTime.Now.ToString("dd MMM ") + label_timefor.Text);
                sw.Close();
            }

            button_pause.Visible = true;
            button_start.Visible = false;

            textBox_domain.Enabled = false;
            button_go.Enabled = false;

            button_startover.Enabled = true;
            
            // textchanged timefor
            textchanged_timefor = true;
        }

        // SELECTED CHANGED
        private void DataGridView_devices_SelectionChanged(object sender, EventArgs e)
        {
            if (label_currentindex.Text == "0")
            {
                pictureBox_loader.Visible = false;
                textBox_domain.Text = "";
            }

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
                        }));
                    }
                    catch (Exception)
                    {
                        // Leave blank
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

            label_time_urgent.Text = time;
        }

        int timer_loader_uploaded = 0;
        int timer_loader_okay = 10;
        private bool buttonGoWasClicked;
        private bool buttonDetect;
        private bool urgentRunning = false;

        private void Timer_loader_Tick(object sender, EventArgs e)
        {
            if (panel_main.Visible == true)
            {
                string color = "#438eb9";
                Color color_change = ColorTranslator.FromHtml(color);
                button_okay.BackColor = color_change;
            }
            else if (panel_urgent.Visible == true)
            {
                string color = "#394557";
                Color color_change = ColorTranslator.FromHtml(color);
                button_okay.BackColor = color_change;
            }

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

                if (panel_main.Visible == true)
                {
                    if (!urgentRunning)
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

                            string date_history = DateTime.Now.ToString("dd MMM ");

                            if (dataGridView_history.RowCount == 12)
                            {
                                dataGridView_history.Rows.RemoveAt(12 - 1);
                            }

                            dataGridView_history.Rows.Insert(0, date_history + label_timeget.Text + " OK");

                            dataGridView_history.ClearSelection();
                            
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
                                //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1016", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (pagesource_history == "ERROR")
                        {
                            if (detectnohistoryyet)
                            {
                                dataGridView_history.Rows.Clear();
                                detectnohistoryyet = false;
                            }

                            string date_history = DateTime.Now.ToString("dd MMM ");

                            if (dataGridView_history.RowCount == 12)
                            {
                                dataGridView_history.Rows.RemoveAt(12 - 1);
                            }

                            dataGridView_history.Rows.Insert(0, date_history + label_timeget.Text + " ERR");

                            dataGridView_history.ClearSelection();

                            // Insert in text file
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
                                //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1016", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        label_timeget.Text = label_timefor.Text;

                        if (server)
                        {
                            string path_autoyes = Path.GetTempPath() + @"\raincheck_autoyes.txt";
                            StreamWriter sw_autoyes = new StreamWriter(path_autoyes, true, Encoding.UTF8);
                            sw_autoyes.Close();

                            can_close = false;
                            Close();
                            Application.Restart();
                        }
                    }
                    else
                    {
                        string color_urgent = "#394557";
                        Color color_change = ColorTranslator.FromHtml(color_urgent);
                        panel_top.BackColor = color_change;

                        if (pagesource_history == "SUCCESS")
                        {
                            if (detectnohistoryyet)
                            {
                                dataGridView_history.Rows.Clear();
                                detectnohistoryyet = false;
                            }

                            string date_history = DateTime.Now.ToString("dd MMM ");

                            if (dataGridView_history.RowCount == 12)
                            {
                                dataGridView_history.Rows.RemoveAt(12 - 1);
                            }

                            dataGridView_history.Rows.Insert(0, date_history + label_timeget.Text + " OK");

                            dataGridView_history.ClearSelection();
                            
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
                                //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1016", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (pagesource_history == "ERROR")
                        {
                            if (detectnohistoryyet)
                            {
                                dataGridView_history.Rows.Clear();
                                detectnohistoryyet = false;
                            }
                            
                            string date_history = DateTime.Now.ToString("dd MMM ");

                            if (dataGridView_history.RowCount == 12)
                            {
                                dataGridView_history.Rows.RemoveAt(12 - 1);
                            }

                            dataGridView_history.Rows.Insert(0, date_history + label_timeget.Text + " ERR");

                            dataGridView_history.ClearSelection();

                            // Insert in text file
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
                                //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1016", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        label_timeget.Text = label_timefor.Text;

                        panel_urgent.Visible = true;
                        panel_main.Visible = false;
                        label_domainscount.Visible = false;
                        label_domain_urgent.Visible = true;
                        textBox_domain_urgent.Text = "";
                        start_detect_button = false;
                        label_domainscount_urgent.Visible = true;

                        button_start_urgent.PerformClick();
                    }
                }
                else if (panel_urgent.Visible == true)
                {
                    string color_normal = "#438eb9";
                    Color color_change = ColorTranslator.FromHtml(color_normal);

                    panel_top.BackColor = color_change;
                    button_start_urgent.BackColor = color_change;
                    button_pause_urgent.BackColor = color_change;
                    button_startover_urgent.BackColor = color_change;
                    label_domainscount_urgent.ForeColor = color_change;

                    label_status_1_urgent.ForeColor = color_change;
                    label_status_urgent.ForeColor = color_change;

                    label_timefor_1_urgent.ForeColor = color_change;
                    label_timefor_urgent.ForeColor = color_change;

                    label_cyclein_1_urgent.ForeColor = color_change;
                    label_cyclein_urgent.ForeColor = color_change;

                    dataGridView_urgent.DefaultCellStyle.SelectionBackColor = color_change;

                    if (pagesource_history == "SUCCESS")
                    {
                        if (detectnohistoryyet)
                        {
                            dataGridView_history.Rows.Clear();
                            detectnohistoryyet = false;
                        }

                        string date_history = DateTime.Now.ToString("dd MMM ");

                        if (dataGridView_history.RowCount == 12)
                        {
                            dataGridView_history.Rows.RemoveAt(12 - 1);
                        }

                        dataGridView_history.Rows.Insert(0, date_history + label_time_urgent.Text + " OK (urgent)");
                        dataGridView_history.ClearSelection();

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
                                sw.WriteLine(date_history + label_time_urgent.Text + " OK (urgent)");
                                sw.WriteLine(oldText);
                                sw.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1016", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (pagesource_history == "ERROR")
                    {
                        if (detectnohistoryyet)
                        {
                            dataGridView_history.Rows.Clear();
                            detectnohistoryyet = false;
                        }

                        string date_history = DateTime.Now.ToString("dd MMM ");

                        if (dataGridView_history.RowCount == 12)
                        {
                            dataGridView_history.Rows.RemoveAt(12 - 1);
                        }

                        dataGridView_history.Rows.Insert(0, date_history + label_time_urgent.Text + " ERR (urgent)");
                        dataGridView_history.ClearSelection();

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
                                sw.WriteLine(date_history + label_time_urgent.Text + " ERR (urgent)");
                                sw.WriteLine(oldText);
                                sw.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1016", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    urgentRunning = false;

                    label_getdatetime_urgent.Text = "";
                    label_datetimetextfile_urgent.Text = "";

                    panel_urgent.Visible = false;
                    panel_main.Visible = true;
                    label_domainscount.Visible = true;
                    label_domain_urgent.Visible = false;
                    label_domainscount_urgent.Visible = false;
                    textBox_domain.Text = "";
                    timer_start_urgent.Start();

                    // Auto start the checking if label time for is not exists in history
                    string path = Path.GetTempPath() + @"\raincheck_history.txt";
                    if (File.Exists(path))
                    {
                        string date_history = DateTime.Now.ToString("dd MMM ");
                        string result_history = "";

                        using (StreamReader sr = File.OpenText(path))
                        {
                            string s = String.Empty;
                            while ((s = sr.ReadLine()) != null)
                            {
                                if (s != "")
                                {
                                    if (s == date_history + label_timefor.Text + " OK" || s == date_history + label_timefor.Text + " ERR" || s == date_history + label_timefor.Text)
                                    {
                                        result_history = "contains";
                                        break;
                                    }
                                    else
                                    {
                                        result_history = "not contains";
                                    }
                                }
                            }
                        }

                        if (result_history == "contains")
                        {
                            textchanged_timefor = true;
                            button_start.Enabled = false;
                        }
                        else
                        {
                            button_start.Enabled = true;
                            button_start.PerformClick();
                            button_start.Enabled = false;
                        }
                    }

                    timer_urgent_detect.Start();
                }
            }
        }

        private void Button_okay_Click(object sender, EventArgs e)
        {
            timer_loader_okay = 10;
            timer_loader_uploaded = 0;
            timer_loader.Stop();
            panel_uploaded.Visible = false;

            if (panel_main.Visible == true)
            {
                if (!urgentRunning)
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

                        string date_history = DateTime.Now.ToString("dd MMM ");

                        if (dataGridView_history.RowCount == 12)
                        {
                            dataGridView_history.Rows.RemoveAt(12 - 1);
                        }

                        dataGridView_history.Rows.Insert(0, date_history + label_timeget.Text + " OK");

                        dataGridView_history.ClearSelection();

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
                            //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1016", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (pagesource_history == "ERROR")
                    {
                        if (detectnohistoryyet)
                        {
                            dataGridView_history.Rows.Clear();
                            detectnohistoryyet = false;
                        }

                        string date_history = DateTime.Now.ToString("dd MMM ");

                        if (dataGridView_history.RowCount == 12)
                        {
                            dataGridView_history.Rows.RemoveAt(12 - 1);
                        }

                        dataGridView_history.Rows.Insert(0, date_history + label_timeget.Text + " ERR");

                        dataGridView_history.ClearSelection();

                        // Insert in text file
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
                            //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1016", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    label_timeget.Text = label_timefor.Text;

                    if (server)
                    {
                        string path_autoyes = Path.GetTempPath() + @"\raincheck_autoyes.txt";
                        StreamWriter sw_autoyes = new StreamWriter(path_autoyes, true, Encoding.UTF8);
                        sw_autoyes.Close();

                        can_close = false;
                        Close();
                        Application.Restart();
                    }
                }
                else
                {
                    string color_urgent = "#394557";
                    Color color_change = ColorTranslator.FromHtml(color_urgent);
                    panel_top.BackColor = color_change;

                    if (pagesource_history == "SUCCESS")
                    {
                        if (detectnohistoryyet)
                        {
                            dataGridView_history.Rows.Clear();
                            detectnohistoryyet = false;
                        }

                        string date_history = DateTime.Now.ToString("dd MMM ");

                        if (dataGridView_history.RowCount == 12)
                        {
                            dataGridView_history.Rows.RemoveAt(12 - 1);
                        }

                        dataGridView_history.Rows.Insert(0, date_history + label_timeget.Text + " OK");

                        dataGridView_history.ClearSelection();

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
                            //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1016", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (pagesource_history == "ERROR")
                    {
                        if (detectnohistoryyet)
                        {
                            dataGridView_history.Rows.Clear();
                            detectnohistoryyet = false;
                        }

                        string date_history = DateTime.Now.ToString("dd MMM ");

                        if (dataGridView_history.RowCount == 12)
                        {
                            dataGridView_history.Rows.RemoveAt(12 - 1);
                        }

                        dataGridView_history.Rows.Insert(0, date_history + label_timeget.Text + " ERR");

                        dataGridView_history.ClearSelection();

                        // Insert in text file
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
                            //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1016", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    label_timeget.Text = label_timefor.Text;

                    panel_urgent.Visible = true;
                    panel_main.Visible = false;
                    label_domainscount.Visible = false;
                    label_domain_urgent.Visible = true;
                    textBox_domain_urgent.Text = "";
                    start_detect_button = false;
                    label_domainscount_urgent.Visible = true;

                    button_start_urgent.PerformClick();
                }
            }
            else if (panel_urgent.Visible == true)
            {
                string color_normal = "#438eb9";
                Color color_change = ColorTranslator.FromHtml(color_normal);

                panel_top.BackColor = color_change;
                button_start_urgent.BackColor = color_change;
                button_pause_urgent.BackColor = color_change;
                button_startover_urgent.BackColor = color_change;
                label_domainscount_urgent.ForeColor = color_change;

                label_status_1_urgent.ForeColor = color_change;
                label_status_urgent.ForeColor = color_change;

                label_timefor_1_urgent.ForeColor = color_change;
                label_timefor_urgent.ForeColor = color_change;

                label_cyclein_1_urgent.ForeColor = color_change;
                label_cyclein_urgent.ForeColor = color_change;

                dataGridView_urgent.DefaultCellStyle.SelectionBackColor = color_change;

                if (pagesource_history == "SUCCESS")
                {
                    if (detectnohistoryyet)
                    {
                        dataGridView_history.Rows.Clear();
                        detectnohistoryyet = false;
                    }

                    string date_history = DateTime.Now.ToString("dd MMM ");

                    if (dataGridView_history.RowCount == 12)
                    {
                        dataGridView_history.Rows.RemoveAt(12 - 1);
                    }

                    dataGridView_history.Rows.Insert(0, date_history + label_time_urgent.Text + " OK (urgent)");
                    dataGridView_history.ClearSelection();

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
                            sw.WriteLine(date_history + label_time_urgent.Text + " OK (urgent)");
                            sw.WriteLine(oldText);
                            sw.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1016", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (pagesource_history == "ERROR")
                {
                    if (detectnohistoryyet)
                    {
                        dataGridView_history.Rows.Clear();
                        detectnohistoryyet = false;
                    }

                    string date_history = DateTime.Now.ToString("dd MMM ");

                    if (dataGridView_history.RowCount == 12)
                    {
                        dataGridView_history.Rows.RemoveAt(12 - 1);
                    }

                    dataGridView_history.Rows.Insert(0, date_history + label_time_urgent.Text + " ERR (urgent)");
                    dataGridView_history.ClearSelection();

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
                            sw.WriteLine(date_history + label_time_urgent.Text + " ERR (urgent)");
                            sw.WriteLine(oldText);
                            sw.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1016", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                urgentRunning = false;

                label_getdatetime_urgent.Text = "";
                label_datetimetextfile_urgent.Text = "";

                panel_urgent.Visible = false;
                panel_main.Visible = true;
                label_domainscount.Visible = true;
                label_domain_urgent.Visible = false;
                label_domainscount_urgent.Visible = false;
                textBox_domain.Text = "";
                timer_start_urgent.Start();

                // Auto start the checking if label time for is not exists in history
                string path = Path.GetTempPath() + @"\raincheck_history.txt";
                if (File.Exists(path))
                {
                    string date_history = DateTime.Now.ToString("dd MMM ");
                    string result_history = "";

                    using (StreamReader sr = File.OpenText(path))
                    {
                        string s = String.Empty;
                        while ((s = sr.ReadLine()) != null)
                        {
                            if (s != "")
                            {
                                if (s == date_history + label_timefor.Text + " OK" || s == date_history + label_timefor.Text + " ERR" || s == date_history + label_timefor.Text)
                                {
                                    result_history = "contains";
                                    break;
                                }
                                else
                                {
                                    result_history = "not contains";
                                }
                            }
                        }
                    }

                    if (result_history == "contains")
                    {
                        textchanged_timefor = true;
                        button_start.Enabled = false;
                    }
                    else
                    {
                        button_start.Enabled = true;
                        button_start.PerformClick();
                        button_start.Enabled = false;
                    }
                }

                timer_urgent_detect.Start();
            }
        }

        private void Timer_blink_Tick(object sender, EventArgs e)
        {
            label_status.Visible = !label_status.Visible;
        }

        private void Button_urgent_Click(object sender, EventArgs e)
        {            
            panel_urgent.Visible = true;
            panel_main.Visible = false;
            label_domainscount.Visible = false;
            label_domain_urgent.Visible = true;
            textBox_domain_urgent.Text = "";
        }

        private void Label_back_Click(object sender, EventArgs e)
        {
            if (label_status_urgent.Text == "[Waiting]")
            {
                panel_urgent.Visible = false;
                panel_main.Visible = true;
                label_domainscount.Visible = true;
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
                        //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1018", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //Close();
                    }

                    if (dataGridView_urgent.Rows.Count > 0)
                    {
                        label_clear.Visible = true;
                    }
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

        private void Button_start_urgent_Click(object sender, EventArgs e)
        {
            if (label_getdatetime_urgent.Text == "")
            {
                label_getdatetime_urgent.Text = label9.Text;
            }

            if (label_datetimetextfile_urgent.Text == "")
            {
                label_datetimetextfile_urgent.Text = DateTime.Now.ToString("yyyy-MM-dd ") + label_timefor.Text + ":00";
            }

            if (label_currentindex_urgent.Text == "0")
            {
                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string path = path_desktop + "\\rainCheck\\" + label_getdatetime_urgent.Text + "_urgent_" + i_timeout;

                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            
            pictureBox_loader_urgent.Visible = true;

            // Set browser panel dock style
            panel_browser_urgent.Visible = true;
            panel_browser_urgent.BringToFront();
            panel_browser_urgent.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;

            label_domainscount_urgent.Text = "Total: " + (index_urgent + 1) + " of " + dataGridView_urgent.RowCount.ToString();

            timer_blink_urgent.Stop();
            label_status_urgent.Visible = true;
            label_status_urgent.Text = "[Running]";

            timer_domain_urgent.Enabled = true;
            timer_domain_urgent.Start();

            int getCurrentIndex = Convert.ToInt32(label_currentindex_urgent.Text);
            dataGridView_urgent.ClearSelection();
            dataGridView_urgent.Rows[getCurrentIndex].Selected = true;

            // For timeout
            i = 1;
            timer_timeout.Start();

            button_pause_urgent.Visible = true;
            button_start_urgent.Visible = false;

            label_inaccessible_error_message.Text = "";

            button_startover_urgent.Enabled = true;

            buttonDetect = true;

            urgentRunning = true;
        }
        
        private void Button_pause_urgent_Click(object sender, EventArgs e)
        {
            // Set browser panel dock style
            chromeBrowser.Dock = DockStyle.None;

            timer_blink_urgent.Start();
            label_status_urgent.Text = "[Paused]";
            timer_domain_urgent.Stop();
            timer_timeout.Stop();
            pictureBox_loader_urgent.Visible = false;

            timer_detectnotloading.Stop();
            detectnotloading = 0;

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
                    string domain = row.Cells[1].Value.ToString();
                    string brand = row.Cells[2].Value.ToString();
                    string text_search = row.Cells[3].Value.ToString();
                    string webtype = row.Cells[4].Value.ToString();

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
                        //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1019", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //Close();
                    }

                    Invoke(new Action(() =>
                    {
                        label_domainhide_urgent.Text = domain;
                        label_brandhide_urgent.Text = brand;
                        label_text_search_urgent.Text = text_search;
                        label_webtype_urgent.Text = webtype;
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

                    // ms_detect = 0;
                    fully_loaded = 0;
                    start_detect = 0;

                    string datetime_folder = label9.Text;
                    string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                    string path = path_desktop + "\\rainCheck\\" + label_getdatetime_urgent.Text + "_urgent_" + i_timeout;
                    string read = "";

                    // Insert
                    if (File.Exists(path + "\\result.txt"))
                    {
                        read = File.ReadAllText(path + "\\result.txt");
                    }
                    else
                    {
                        string path_autoyes = Path.GetTempPath() + @"\raincheck_autoyes.txt";
                        StreamWriter sw_autoyes = new StreamWriter(path_autoyes, true, Encoding.UTF8);
                        sw_autoyes.Close();

                        can_close = false;
                        Close();
                        Application.Restart();
                    }
                    
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

                    string outputpath = "";
                    using (ZipFile zip = new ZipFile())
                    {
                        try
                        {
                            outputpath = path + ".zip";
                            zip.Password = "r@inCh3ckd234b70";
                            zip.AddDirectory(path);
                            zip.Save(outputpath);

                            if (Directory.Exists(path))
                            {
                                Directory.Delete(path, true);
                            }
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1014", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            //Close();
                        }
                    }

                    try
                    {
                        FtpWebRequest req = (FtpWebRequest)WebRequest.Create("ftp://raincheck.ssitex.com/public/zip/" + datetime_folder + "_urgent_" + i_timeout);
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
                        //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1030", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // Enable visible buttons
                    button_start_urgent.Visible = true;
                    button_pause_urgent.Visible = false;
                    button_start_urgent.Enabled = false;
                    button_startover_urgent.Enabled = false;
                    pictureBox_loader_urgent.Visible = false;
                                                            
                    label_currentindex_urgent.Text = "0";

                    label_status_urgent.Text = "[Loading]";
                    timer_domain_urgent.Stop();
                    
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

        private void APIGetDomains()
        {
            if (SetResult == "Yes")
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        string auth = "r@inCh3ckd234b70";
                        string type = "brand_set";
                        string nbrs = BrandIDs;
                        string request = "http://raincheck.ssitex.com/api/api.php";

                        NameValueCollection postData = new NameValueCollection()
                        {
                            { "auth", auth },
                            { "type", type },
                            { "nbrs", nbrs }
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
                    //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1022", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (label_currentindex.Text == "0" && label_status.Text == "[Waiting]")
                    {
                        string path_autoyes = Path.GetTempPath() + @"\raincheck_autoyes.txt";
                        StreamWriter sw_autoyes = new StreamWriter(path_autoyes, true, Encoding.UTF8);
                        sw_autoyes.Close();

                        can_close = false;
                        Close();
                        Application.Restart();
                    }
                    else
                    {
                        server = true;
                    }
                }
            }
            else
            {
                string path = Path.GetTempPath() + @"\raincheck_brand.txt";
                string path_lastchrrentindex = Path.GetTempPath() + @"\raincheck_lastcurrentindex.txt";

                try
                {
                    using (var client = new WebClient())
                    {
                        string auth = "r@inCh3ckd234b70";
                        string type = "brand_get";
                        string request = "http://raincheck.ssitex.com/api/api.php";

                        NameValueCollection postData = new NameValueCollection()
                        {
                            { "auth", auth },
                            { "type", type }
                        };

                        string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));

                        if (File.Exists(path))
                        {
                            File.Delete(path);

                            StreamWriter sw_create = new StreamWriter(path, true, Encoding.UTF8);
                            sw_create.Close();

                            StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8);
                            sw.Write(pagesource);
                            sw.Close();
                        }
                        else
                        {
                            StreamWriter sw_create = new StreamWriter(path, true, Encoding.UTF8);
                            sw_create.Close();

                            StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8);
                            sw.Write(pagesource);
                            sw.Close();
                        }

                        string read = File.ReadAllText(path);
                        BrandIDs = read;
                    }
                }
                catch (Exception ex)
                {
                    var st = new StackTrace(ex, true);
                    var frame = st.GetFrame(0);
                    var line = frame.GetFileLineNumber();
                    //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1022", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    string path_autoyes = Path.GetTempPath() + @"\raincheck_autoyes.txt";
                    StreamWriter sw_autoyes = new StreamWriter(path_autoyes, true, Encoding.UTF8);
                    sw_autoyes.Close();

                    can_close = false;
                    Close();
                    Application.Restart();
                }

                try
                {
                    using (var client = new WebClient())
                    {
                        string auth = "r@inCh3ckd234b70";
                        string type = "domain_main";
                        string nbrs = BrandIDs;
                        string request = "http://raincheck.ssitex.com/api/api.php";

                        NameValueCollection postData = new NameValueCollection()
                        {
                            { "auth", auth },
                            { "type", type },
                            { "nbrs", nbrs }
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

                    if (label_currentindex.Text == "0" && label_status.Text == "[Waiting]")
                    {
                        string path_autoyes = Path.GetTempPath() + @"\raincheck_autoyes.txt";
                        StreamWriter sw_autoyes = new StreamWriter(path_autoyes, true, Encoding.UTF8);
                        sw_autoyes.Close();

                        can_close = false;
                        Close();
                        Application.Restart();
                    }
                    else
                    {
                        server = true;
                    }
                }
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
                        int getCurrentIndex = Convert.ToInt32(label_currentindex_urgent.Text);
                        dataGridView_urgent.ClearSelection();

                        // For timeout
                        i = 1;
                        timer_timeout.Start();

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

            if (label_currentindex.Text == "0" && label_status.Text == "[Waiting]")
            {
                pictureBox_loader.Visible = false;
                textBox_domain.Text = "";
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
        private bool server = false;
        private int estimatedLength;
        private bool domainhide_detect;
        private bool textbox_domain_detect;
        private string cefsharp_title;
        private string cefsharp_domain;

        private void label_timefor_TextChanged(object sender, EventArgs e)
        {
            if (!urgentRunning)
            {
                // Auto start the checking if label time for is not exists in history
                string path = Path.GetTempPath() + @"\raincheck_history.txt";
                if (File.Exists(path))
                {
                    detect_start++;

                    if (detect_start == 1)
                    {
                        string date_history = DateTime.Now.ToString("dd MMM ");
                        string result_history = "";

                        using (StreamReader sr = File.OpenText(path))
                        {
                            string s = String.Empty;
                            while ((s = sr.ReadLine()) != null)
                            {
                                if (s != "")
                                {
                                    if (s == date_history + label_timefor.Text + " OK" || s == date_history + label_timefor.Text + " ERR" || s == date_history + label_timefor.Text)
                                    {
                                        result_history = "contains";
                                        break;
                                    }
                                    else
                                    {
                                        result_history = "not contains";
                                    }
                                }
                            }
                        }

                        if (result_history == "contains")
                        {
                            textchanged_timefor = true;
                            button_start.Enabled = false;
                        }
                        else
                        {
                            label_timeget.Text = label_timefor.Text;
                            button_start.Enabled = true;
                            button_start.PerformClick();
                            button_start.Enabled = false;
                            auto_start = false;
                        }
                    }                    
                }
                
                if (label_status.Text != "[Running]")
                {
                    label_timeget.Text = label_timefor.Text;
                }
                                
                if (textchanged_timefor == true)
                {
                    if (detect_start != 1)
                    {
                        timefor++;
                    }
                    else
                    {
                        auto_start = true;
                    }
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

                if (!File.Exists(path))
                {
                    if (SetResult == "Yes")
                    {
                        button_start.PerformClick();
                    }
                }
            }
            else
            {
                if (label_currentindex.Text != "0")
                {
                    button_pause_urgent.PerformClick();

                    panel_urgent.Visible = false;
                    panel_main.Visible = true;
                    label_domainscount.Visible = true;
                    label_domain_urgent.Visible = false;
                    label_domainscount_urgent.Visible = false;
                    textBox_domain.Text = "";
                    textBox_domain.Enabled = false;
                    button_start.Enabled = false;

                    string color = "#438eb9";
                    Color color_change = ColorTranslator.FromHtml(color);
                    panel_top.BackColor = color_change;

                    timerfornext = true;
                    label_ifloadornot.Text = "1";
                    label_ifloadornot.Text = "0";

                    timer_blink.Stop();
                    label_status.Visible = true;
                    label_status.Text = "[Running]";

                    start_detect_button = true;

                    chromeBrowser.Stop();
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

        private void timer_deviceon_Tick(object sender, EventArgs e)
        {
            try
            {
                using (var client = new WebClient())
                {
                    string auth = "r@inCh3ckd234b70";
                    string type = "running";
                    string mac_id = GetMACAddress();
                    string run_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
                i = 1;

                ms_detect = 0;
                fully_loaded = 0;
                start_detect = 0;
                domain_i = 0;
                label_currentindex_urgent.Text = "0";

                string datetime = label11.Text;
                string datetime_folder = label9.Text;
                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                string path = path_desktop + "\\rainCheck\\" + label_getdatetime_urgent.Text + "_urgent_" + i_timeout;

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

        private void timer_urgent_detect_Tick(object sender, EventArgs e)
        {
            try
            {
                using (var client = new WebClient())
                {
                    string auth = "r@inCh3ckd234b70";
                    string type = "urgent_device";
                    string mac_id = GetMACAddress();
                    string request = "http://raincheck.ssitex.com/api/api.php";

                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "auth", auth },
                        { "type", type },
                        { "mac_id", mac_id }
                    };

                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));
                    string path_urgent_domain_detect = Path.GetTempPath() + @"\raincheck_urgent.txt";

                    JArray jsonObject = JArray.Parse(pagesource);
                    string u_id = jsonObject[0]["u_id"].Value<string>();
                    string u_type = jsonObject[0]["set_type"].Value<string>();
                    
                    label_utype.Text = u_type;

                    if (u_id != label_urgent_detect.Text)
                    {
                        if (label_status.Text == "[Running]")
                        {
                            button_pause.PerformClick();
                        }

                        try
                        {
                            using (var client_1 = new WebClient())
                            {
                                string type_1 = "domain_urgent";
                                string request_1 = "http://raincheck.ssitex.com/api/api.php";

                                NameValueCollection postData_1 = new NameValueCollection()
                                {
                                    { "auth", auth },
                                    { "type", type_1 },
                                    { "mac_id", mac_id }
                                };

                                string pagesource_1 = Encoding.UTF8.GetString(client_1.UploadValues(request_1, postData_1));
                                var arr = JsonConvert.DeserializeObject<JArray>(pagesource_1);
                                dataGridView_urgent.DataSource = arr;
                            }
                        }
                        catch (Exception)
                        {
                            StreamWriter sw_create = new StreamWriter(path_urgent_domain_detect, true, Encoding.UTF8);
                            sw_create.Close();

                            string path_autoyes = Path.GetTempPath() + @"\raincheck_autoyes.txt";
                            StreamWriter sw_autoyes = new StreamWriter(path_autoyes, true, Encoding.UTF8);
                            sw_autoyes.Close();
                            
                            can_close = false;
                            Close();
                            Application.Restart();
                        }

                        if (can_close)
                        {
                            // Urgent
                            string urgent = "#394557";
                            Color color_change = ColorTranslator.FromHtml(urgent);

                            panel_top.BackColor = color_change;
                            button_start_urgent.BackColor = color_change;
                            button_pause_urgent.BackColor = color_change;
                            button_startover_urgent.BackColor = color_change;
                            label_domainscount_urgent.ForeColor = color_change;

                            label_status_1_urgent.ForeColor = color_change;
                            label_status_urgent.ForeColor = color_change;

                            label_timefor_1_urgent.ForeColor = color_change;
                            label_timefor_urgent.ForeColor = color_change;

                            label_cyclein_1_urgent.ForeColor = color_change;
                            label_cyclein_urgent.ForeColor = color_change;

                            // Table UI
                            dataGridView_urgent.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                            string hex = "#438eb9";
                            Color color = ColorTranslator.FromHtml(hex);
                            dataGridView_urgent.DefaultCellStyle.SelectionBackColor = color_change;
                            dataGridView_urgent.DefaultCellStyle.SelectionForeColor = Color.White;
                            dataGridView_urgent.Columns["domain_name"].Visible = false;
                            dataGridView_urgent.Columns["id"].Visible = false;
                            dataGridView_urgent.Columns["text_search"].Visible = false;
                            dataGridView_urgent.Columns["website_type"].Visible = false;
                            label_domainscount_urgent.Visible = true;
                            label_domainscount_urgent.Text = "Total: " + dataGridView_urgent.RowCount.ToString();

                            // Go to urgent panel
                            panel_urgent.Visible = true;
                            panel_main.Visible = false;
                            label_domainscount.Visible = false;
                            label_domain_urgent.Visible = true;
                            textBox_domain_urgent.Text = "";

                            label_urgent_detect.Text = u_id;

                            File.Delete(path_urgent_domain_detect);

                            dataGridView_urgent.ClearSelection();

                            // Start urgent
                            button_start_urgent.Enabled = false;
                            timer_start_urgent.Start();
                            timer_urgent_detect.Stop();
                        }
                    }
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1036", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Close();
            }
        }

        private void timer_start_urgent_Tick(object sender, EventArgs e)
        {
            button_start_urgent.Enabled = true;
            button_start_urgent.PerformClick();
            timer_start_urgent.Stop();
        }

        private void label_domainhide_TextChanged(object sender, EventArgs e)
        {
            domainhide_detect = true;
        }

        private void label_brandhide_TextChanged(object sender, EventArgs e)
        {
            if (label_brandhide.Text == "3")
            {
                timer_deviceon.Stop();
            }
            else
            {
                timer_deviceon.Start();
            }
        }

        private void textBox_domain_TextChanged(object sender, EventArgs e)
        {
            if (domainhide_detect == true)
            {
                if (!textBox_domain.Text.Contains(label_domainhide.Text))
                {
                    textbox_domain_detect = true;
                }
            }

            //if (domainhide_detect == true)
            //{
            //    if (!textBox_domain.Text.Contains(label_domainhide.Text))
            //    {
            //        int webBrowser_i = 0;
            //        while (webBrowser_i <= 2)
            //        {
            //            webBrowser_new.Navigate(label_domainhide.Text);
            //            webBrowser_i++;
            //        }

            //        MessageBox.Show("not contains");

            //        chromeBrowser.Stop();
            //        timer_domain.Stop();
            //        timer_detectnotloading.Stop();
            //        detectnotloading = 0;
            //        fully_loaded = 0;
            //        start_detect = 0;
                    
            //        string datetime_folder = label9.Text;
            //        string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //        string path = path_desktop + "\\result.txt";
            //        var oldLines = File.ReadAllLines(path);
            //        var newLines = oldLines.Where(line => !line.Contains(label_domainhide.Text));
            //        File.WriteAllLines(path, newLines);

            //        string strValue = label_textsearch_brand.Text;
            //        string[] strArray = strValue.Split(',');
            //        string result = "";

            //        foreach (string obj in strArray)
            //        {
            //            bool contains = label_webtitle.Text.Contains(obj);

            //            if (contains == true)
            //            {
            //                Invoke(new Action(() =>
            //                {
            //                    result = "not hijacked";
            //                }));

            //                break;
            //            }
            //            else if (!contains)
            //            {
            //                Invoke(new Action(() =>
            //                {
            //                    result = "hijacked";
            //                }));
            //            }
            //        }

            //        if (result == "hijacked")
            //        {
            //            MessageBox.Show(label_webtitle.Text);
            //            MessageBox.Show("hijacked");
            //            // Data to text file
            //            DataToTextFileHijacked();
            //        }
            //        else
            //        {
            //            MessageBox.Show("not hijacked");
            //            DataToTextFileSuccess();
            //        }

            //        Invoke(new Action(() =>
            //        {
            //            // For timeout
            //            i = 1;
            //            timer_timeout.Stop();

            //            pictureBox_loader.Visible = false;

            //            label_timeout.Text = "";
            //            label_hijacked.Text = "";
            //            label_inaccessible.Text = "";
            //            label_inaccessible_error_message.Text = "";
                        
            //            fully_loaded = 0;
            //            start_detect = 0;
            //            //label_ifloadornot.Text = "1";
            //            //label_ifloadornot.Text = "0";

            //            testonemoretime = 0;
            //            panel_new.Visible = false;
            //        }));
            //    }
            //    else
            //    {
            //        MessageBox.Show("contains");
            //    }

            //    domainhide_detect = false;
            //}
        }
    }
}