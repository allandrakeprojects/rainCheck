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
using System.Reactive.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class Form_Main : Form
    {
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
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1000", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1001", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1002", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1003", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1004", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                can_close = false;
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

                    string result = pagesource.Replace("\"", "") + "000";
                    label13.Text = result;
                    timer_handler.Interval = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1005", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                can_close = false;
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
                                timer_handler.Start();

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
                                timer_handler.Start();

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
                        panel_retry.Visible = true;
                        panel_retry.BringToFront();

                        timer_domain.Stop();
                        timer_handler.Stop();
                        pictureBox_loader.Visible = false;
                        button_pause.Visible = false;
                        button_start.Visible = true;
                    }));
                }
                else if (panel_urgent.Visible == true)
                {
                    Invoke(new Action(() =>
                    {
                        panel_retry.Visible = true;
                        panel_retry.BringToFront();

                        timer_domain_urgent.Stop();
                        timer_handler.Stop();
                        pictureBox_loader_urgent.Visible = false;
                        button_pause_urgent.Visible = false;
                        button_start_urgent.Visible = true;
                    }));
                }
            }
        }

        public int i = 1;
        private void Timer_timeout_Tick(object sender, EventArgs e)
        {
            if (InvokeRequired) { Invoke(new Action(() => { Timer_timeout_Tick(sender, e); })); return; }
            label3.Text = i++.ToString();
            
            if (label3.Text == label13.Text)
            {
                label_timeout.Text = "timeout";
                timer_handler.Stop();
                pictureBox_loader.Visible = false;
            }
        }

        string start_load = "";
        string end_load = "";

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

                    // Delete when line of domain when exists
                    var oldLines = File.ReadAllLines(path + "\\result.txt");
                    var newLines = oldLines.Where(line => !line.Contains(label_domainhide.Text));
                    File.WriteAllLines(path + "\\result.txt", newLines);

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(isp_get))
                        {
                            isp_get = "-";
                        }

                        if (string.IsNullOrEmpty(city_get))
                        {
                            city_get = "-";
                        }

                        string webtitle_replace = webbrowser_handler_title;

                        if (String.IsNullOrEmpty(webtitle_replace))
                        {
                            webtitle_replace = "-";
                        }

                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);
                        swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + ",-," + datetime + "," + ",N");

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

                    // Delete when line of domain when exists
                    var oldLines = File.ReadAllLines(path + "\\result.txt");
                    var newLines = oldLines.Where(line => !line.Contains(label_domainhide.Text));
                    File.WriteAllLines(path + "\\result.txt", newLines);

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(isp_get))
                        {
                            isp_get = "-";
                        }

                        if (string.IsNullOrEmpty(city_get))
                        {
                            city_get = "-";
                        }

                        string webtitle_replace = webbrowser_handler_title;

                        if (String.IsNullOrEmpty(webtitle_replace))
                        {
                            webtitle_replace = "-";
                        }

                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);
                        swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + ",-," + datetime + "," + ",N");

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

                    // Delete when line of domain when exists
                    var oldLines = File.ReadAllLines(path + "\\result.txt");
                    var newLines = oldLines.Where(line => !line.Contains(label_domainhide.Text));
                    File.WriteAllLines(path + "\\result.txt", newLines);

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(isp_get))
                        {
                            isp_get = "-";
                        }

                        if (string.IsNullOrEmpty(city_get))
                        {
                            city_get = "-";
                        }

                        string webtitle_replace = webbrowser_handler_title;

                        if (String.IsNullOrEmpty(webtitle_replace))
                        {
                            webtitle_replace = "-";
                        }
                        
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);
                        swww.WriteLine("," + label_domainhide.Text + ",T" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + ",-," + datetime + "," + ",N");
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

                    // Delete when line of domain when exists
                    var oldLines = File.ReadAllLines(path + "\\result.txt");
                    var newLines = oldLines.Where(line => !line.Contains(label_domainhide.Text));
                    File.WriteAllLines(path + "\\result.txt", newLines);

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(isp_get))
                        {
                            isp_get = "-";
                        }

                        if (string.IsNullOrEmpty(city_get))
                        {
                            city_get = "-";
                        }

                        string webtitle_replace = webbrowser_handler_title;

                        if (String.IsNullOrEmpty(webtitle_replace))
                        {
                            webtitle_replace = "-";
                        }

                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);
                        swww.WriteLine("," + label_domainhide.Text + ",T" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + ",-," + datetime + "," + ",N");
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
            try
            {
                string datetime = label11.Text;
                string datetime_folder = label9.Text;
                string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                string path = path_desktop + "\\rainCheck\\" + datetime_folder;

                if (Directory.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);
                        //swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + "," + textBox_domain.Text + "," + "," + "," + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

                        swww.Close();
                    }

                    // Delete when line of domain when exists
                    var oldLines = File.ReadAllLines(path + "\\result.txt");
                    var newLines = oldLines.Where(line => !line.Contains(label_domainhide.Text));
                    File.WriteAllLines(path + "\\result.txt", newLines);

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);
                        
                        if (string.IsNullOrEmpty(isp_get))
                        {
                            isp_get = "-";
                        }

                        if (string.IsNullOrEmpty(city_get))
                        {
                            city_get = "-";
                        }

                        string webtitle_replace = webbrowser_handler_title;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        swww.WriteLine("," + label_domainhide.Text + ",H" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + "," + webbrowser_handler_url + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + ",-," + datetime + "," + ",N");
                        swww.Close();
                    }
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);
                        //swww.WriteLine("," + label_domainhide.Text + ",S" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + label_webtitle.Text + "," + textBox_domain.Text + "," + "," + "," + "," + isp_get + "," + city_get + "," + datetime + "," + ",N");
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

                        swww.Close();
                    }

                    // Delete when line of domain when exists
                    var oldLines = File.ReadAllLines(path + "\\result.txt");
                    var newLines = oldLines.Where(line => !line.Contains(label_domainhide.Text));
                    File.WriteAllLines(path + "\\result.txt", newLines);

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);

                        if (string.IsNullOrEmpty(isp_get))
                        {
                            isp_get = "-";
                        }

                        if (string.IsNullOrEmpty(city_get))
                        {
                            city_get = "-";
                        }

                        string webtitle_replace = webbrowser_handler_title;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        swww.WriteLine("," + label_domainhide.Text + ",H" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + "," + webbrowser_handler_url + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + ",-," + datetime + "," + ",N");
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

                    // Delete when line of domain when exists
                    var oldLines = File.ReadAllLines(path + "\\result.txt");
                    var newLines = oldLines.Where(line => !line.Contains(label_domainhide.Text));
                    File.WriteAllLines(path + "\\result.txt", newLines);

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);
                        
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

                        string webtitle_replace = webbrowser_handler_title;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        swww.WriteLine("," + label_domainhide.Text + ",I" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + "," + webtitle.ToString() + "," + datetime_folder + "_" + label_macid.Text + "_n_" + sb_pic.ToString() + "," + isp_get + "," + city_get + ",-," + datetime + "," + ",N");
                        swww.Close();
                    }
                }
                else
                {
                    // Create directory
                    DirectoryInfo di = Directory.CreateDirectory(path);

                    StreamWriter sw = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);
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

                    // Delete when line of domain when exists
                    var oldLines = File.ReadAllLines(path + "\\result.txt");
                    var newLines = oldLines.Where(line => !line.Contains(label_domainhide.Text));
                    File.WriteAllLines(path + "\\result.txt", newLines);

                    string contain_text = label_domainhide.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);

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

                        string webtitle_replace = webbrowser_handler_title;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        swww.WriteLine("," + label_domainhide.Text + ",I" + "," + label_brandhide.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + "," + webtitle.ToString() + "," + datetime_folder + "_" + label_macid.Text + "_n_" + sb_pic.ToString() + "," + isp_get + "," + city_get + ",-," + datetime + "," + ",N");
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

                    // Delete when line of domain when exists
                    var oldLines = File.ReadAllLines(path + "\\result.txt");
                    var newLines = oldLines.Where(line => !line.Contains(label_domainhide_urgent.Text));
                    File.WriteAllLines(path + "\\result.txt", newLines);

                    string contain_text = label_domainhide_urgent.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(isp_get))
                        {
                            isp_get = "-";
                        }

                        if (string.IsNullOrEmpty(city_get))
                        {
                            city_get = "-";
                        }

                        string webtitle_replace = webbrowser_handler_title;

                        if (String.IsNullOrEmpty(webtitle_replace))
                        {
                            webtitle_replace = "-";
                        }

                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);
                        swww.WriteLine("," + label_domainhide_urgent.Text + ",S" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + label_utype.Text + "," + label_datetimetextfile_urgent.Text + "," + ",U");
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

                    // Delete when line of domain when exists
                    var oldLines = File.ReadAllLines(path + "\\result.txt");
                    var newLines = oldLines.Where(line => !line.Contains(label_domainhide_urgent.Text));
                    File.WriteAllLines(path + "\\result.txt", newLines);

                    string contain_text = label_domainhide_urgent.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(isp_get))
                        {
                            isp_get = "-";
                        }

                        if (string.IsNullOrEmpty(city_get))
                        {
                            city_get = "-";
                        }

                        string webtitle_replace = webbrowser_handler_title;

                        if (String.IsNullOrEmpty(webtitle_replace))
                        {
                            webtitle_replace = "-";
                        }

                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);
                        swww.WriteLine("," + label_domainhide_urgent.Text + ",S" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + label_utype.Text + "," + label_datetimetextfile_urgent.Text + "," + ",U");
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

                    // Delete when line of domain when exists
                    var oldLines = File.ReadAllLines(path + "\\result.txt");
                    var newLines = oldLines.Where(line => !line.Contains(label_domainhide_urgent.Text));
                    File.WriteAllLines(path + "\\result.txt", newLines);

                    string contain_text = label_domainhide_urgent.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {                        
                        string webtitle_replace = webbrowser_handler_title;

                        if (String.IsNullOrEmpty(webtitle_replace))
                        {
                            webtitle_replace = "-";
                        }

                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);
                        swww.WriteLine("," + label_domainhide_urgent.Text + ",T" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + label_utype.Text + "," + label_datetimetextfile_urgent.Text + "," + ",U");
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

                    // Delete when line of domain when exists
                    var oldLines = File.ReadAllLines(path + "\\result.txt");
                    var newLines = oldLines.Where(line => !line.Contains(label_domainhide_urgent.Text));
                    File.WriteAllLines(path + "\\result.txt", newLines);

                    string contain_text = label_domainhide_urgent.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        string webtitle_replace = webbrowser_handler_title;

                        if (String.IsNullOrEmpty(webtitle_replace))
                        {
                            webtitle_replace = "-";
                        }

                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);
                        swww.WriteLine("," + label_domainhide_urgent.Text + ",T" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + ",-" + ",-" + "," + isp_get + "," + city_get + "," + label_utype.Text + "," + label_datetimetextfile_urgent.Text + "," + ",U");
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

                    // Delete when line of domain when exists
                    var oldLines = File.ReadAllLines(path + "\\result.txt");
                    var newLines = oldLines.Where(line => !line.Contains(label_domainhide_urgent.Text));
                    File.WriteAllLines(path + "\\result.txt", newLines);

                    string contain_text = label_domainhide_urgent.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);

                        if (string.IsNullOrEmpty(isp_get))
                        {
                            isp_get = "-";
                        }

                        if (string.IsNullOrEmpty(city_get))
                        {
                            city_get = "-";
                        }

                        string webtitle_replace = webbrowser_handler_title;
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

                    // Delete when line of domain when exists
                    var oldLines = File.ReadAllLines(path + "\\result.txt");
                    var newLines = oldLines.Where(line => !line.Contains(label_domainhide_urgent.Text));
                    File.WriteAllLines(path + "\\result.txt", newLines);

                    string contain_text = label_domainhide_urgent.Text;
                    if (File.ReadLines(path + @"\result.txt").Any(line => line.Contains(contain_text)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);

                        if (string.IsNullOrEmpty(isp_get))
                        {
                            isp_get = "-";
                        }

                        if (string.IsNullOrEmpty(city_get))
                        {
                            city_get = "-";
                        }

                        string webtitle_replace = webbrowser_handler_title;
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
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);

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

                        string webtitle_replace = webbrowser_handler_title;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        swww.WriteLine("," + label_domainhide_urgent.Text + ",I" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + "," + webtitle.ToString() + "," + datetime_folder + "_" + label_macid.Text + "_u_" + sb_pic.ToString() + "," + isp_get + "," + city_get + "," + label_utype.Text + "," + label_datetimetextfile_urgent.Text + "," + ",U");
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
                        StreamWriter swww = new StreamWriter(path + "\\result.txt", true, Encoding.UTF8);

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

                        string webtitle_replace = webbrowser_handler_title;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        swww.WriteLine("," + label_domainhide_urgent.Text + ",I" + "," + label_brandhide_urgent.Text + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + "," + webtitle.ToString() + "," + datetime_folder + "_" + label_macid.Text + "_u_" + sb_pic.ToString() + "," + isp_get + "," + city_get + "," + label_utype.Text + "," + label_datetimetextfile_urgent.Text + "," + ",U");
                        swww.Close();
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
            }
        }

        private void Button_go_Click(object sender, EventArgs e)
        {
            // asdasd
            start_load = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            webBrowser_handler.Visible = true;
            webBrowser_handler.BringToFront();
            pictureBox_loader.Visible = true;
            pictureBox_loader.Enabled = true;

            timer_handler.Stop();
            timer_handler.Start();

            completed = true;
            timeout = true;
            buttonGoWasClicked = true;
            buttonDetect = true;
            domain = textBox_domain.Text;
            label_domainhide.Text = domain;
            
            StringBuilder domain_replace = new StringBuilder(domain);
            domain_replace.Replace("https://", "");
            domain_replace.Replace("http://", "");
            domain_replace.Replace("/", "");

            // API Brand
            try
            {
                using (var client = new WebClient())
                {
                    string auth = "r@inCh3ckd234b70";
                    string type = "brand";
                    string request = "http://raincheck.ssitex.com/api/api.php";

                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "auth", auth },
                        { "type", type },
                        { "domain", domain_replace.ToString() }
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

                        Form_Brand form_brand = new Form_Brand(domain);
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
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1014", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }

            webBrowser_handler.Navigate(domain);

            //// Set browser panel dock style
            //chromeBrowser.Dock = DockStyle.Fill;

            //i = 1;

            //label_domainhide.Text = textBox_domain.Text;
            //string domain_urgent = label_domainhide.Text;
                        
            //if (label_brandhide.Text != "3")
            //{
            //    chromeBrowser.Load(textBox_domain.Text);
            //}
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
                }
                else
                {
                    index = dataGridView_domain.SelectedRows[0].Index + 1;
                    label_currentindex.Text = index.ToString();
                }

                if (index == domain_total)
                {
                    if (upload_one_time)
                    {
                        button_start_fires = false;
                        webBrowser_handler.Stop();
                        upload_one_time = false;
                        webBrowser_handler.Visible = false;

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

                        pictureBox_loader.Visible = false;
                        textBox_domain.Text = "";

                        // Enable visible buttons
                        button_start.Visible = true;
                        button_pause.Visible = false;
                        button_start.Enabled = false;
                        button_startover.Enabled = false;
                        pictureBox_loader.Visible = false;

                        timer_domain.Stop();
                        
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
                            MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1015", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        dataGridView_domain.Columns["domain_name"].Visible = false;
                        dataGridView_domain.Columns["id"].Visible = false;
                        dataGridView_domain.Columns["text_search"].Visible = false;
                        dataGridView_domain.Columns["website_type"].Visible = false;

                        label_domainscount.Text = "Total: " + dataGridView_domain.RowCount.ToString();

                        webBrowser_handler.Refresh(WebBrowserRefreshOption.Completely);
                    }
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
                // For timeout
                i = 1;
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
            button_start_fires = false;

            timer_blink.Start();
            label_status.Text = "[Paused]";
            timer_domain.Stop();
            timer_handler.Stop();
            pictureBox_loader.Visible = false;
            
            button_pause.Visible = false;
            button_start.Visible = true;
            button_start.Enabled = true;

            textBox_domain.Enabled = true;
            button_go.Enabled = true;

            label_inaccessible_error_message.Text = "";
            textBox_domain.Text = "";
            ActiveControl = textBox_domain;
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

            button_start_fires = true;

            pictureBox_loader.Visible = true;

            timer_blink.Stop();
            label_status.Visible = true;
            label_status.Text = "[Running]";
            timer_domain.Start();

            // For timeout
            i = 1;
            timer_handler.Start();

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
                    string brand;
                    string text_search;
                    string webtype;

                    try
                    {
                        domain = row.Cells[1].Value.ToString();
                        brand = row.Cells[2].Value.ToString();
                        text_search = row.Cells[3].Value.ToString();
                        webtype = row.Cells[4].Value.ToString();
                        
                        if (button_start_fires)
                        {
                            start_load = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            webBrowser_handler.Visible = true;
                            webBrowser_handler.BringToFront();
                            pictureBox_loader.Visible = true;
                            pictureBox_loader.Enabled = true;

                            timer_handler.Stop();
                            timer_handler.Start();

                            completed = true;
                            timeout = true;

                            webBrowser_handler.Navigate(domain);
                        }

                        Invoke(new Action(() =>
                        {
                            textBox_domain.Text = domain;
                            pictureBox_loader.Visible = true;
                            pictureBox_loader.Enabled = true;
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

        // asd123      
        private async void webBrowser_handler_DocumentCompletedAsync(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // Success/Hijacked/Inaccessible
            if (upload_one_time)
            {
                if (completed)
                {
                    if (webBrowser_handler.ReadyState == WebBrowserReadyState.Complete)
                    {
                        if (e.Url == webBrowser_handler.Url)
                        {
                            // handlers
                            end_load = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            webbrowser_handler_title = webBrowser_handler.DocumentTitle;
                            webbrowser_handler_url = webBrowser_handler.Url;
                            timeout = false;
                            timer_handler.Stop();

                            if (panel_main.Visible == true)
                            {
                                pictureBox_loader.Visible = false;
                                pictureBox_loader.Enabled = false;
                            }
                            else if (panel_urgent.Visible == true)
                            {
                                pictureBox_loader_urgent.Visible = false;
                                pictureBox_loader_urgent.Enabled = false;
                            }

                            textBox_domain.Text = webBrowser_handler.Url.ToString();

                            // -------- Data To Text File
                            string search_replace = webbrowser_handler_title;
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
                                        isInaccessible = true;
                                        break;
                                    }
                                    else
                                    {
                                        isInaccessible = false;
                                    }
                                }

                                if (i == 1 && search_replace == "")
                                {
                                    isInaccessible = true;
                                }
                            }

                            if (isInaccessible)
                            {
                                if (label_webtype.Text == "Landing Page" || label_webtype.Text == "Landing page" || webbrowser_handler_title == "")
                                {
                                    var html = "";

                                    if (!domain.Contains("http"))
                                    {
                                        try
                                        {
                                            replace_domain_get = "http://" + domain;
                                            html = new WebClient().DownloadString(replace_domain_get);
                                        }
                                        catch (Exception)
                                        {
                                            html = "";
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            html = new WebClient().DownloadString(domain);
                                        }
                                        catch (Exception)
                                        {
                                            html = "";
                                        }
                                    }

                                    if (html.Contains("landing_image"))
                                    {
                                        if (panel_main.Visible == true)
                                        {
                                            DataToTextFileSuccess();
                                        }
                                        else if (panel_urgent.Visible == true)
                                        {
                                            DataToTextFileSuccess_Urgent();
                                        }
                                    }
                                    else
                                    {
                                        if (panel_main.Visible == true)
                                        {
                                            TakeScreenShot();
                                            DataToTextFileInaccessible();
                                        }
                                        else if (panel_urgent.Visible == true)
                                        {
                                            TakeScreenShot_Urgent();
                                            DataToTextFileInaccessible_Urgent();
                                        }
                                    }
                                }
                                else
                                {
                                    if (panel_main.Visible == true)
                                    {
                                        TakeScreenShot();
                                        DataToTextFileInaccessible();
                                    }
                                    else if (panel_urgent.Visible == true)
                                    {
                                        TakeScreenShot_Urgent();
                                        DataToTextFileInaccessible_Urgent();
                                    }
                                }
                            }
                            else
                            {
                                string strValue = label_textsearch_brand.Text;
                                string[] strArray = strValue.Split(',');

                                foreach (string obj in strArray)
                                {
                                    bool contains = webbrowser_handler_title.Contains(obj);

                                    if (contains == true)
                                    {
                                        Invoke(new Action(() =>
                                        {
                                            isHijacked = false;
                                        }));

                                        break;
                                    }
                                    else if (!contains)
                                    {
                                        Invoke(new Action(() =>
                                        {
                                            isHijacked = true;
                                        }));
                                    }
                                }

                                if (isHijacked)
                                {
                                    if (panel_main.Visible == true)
                                    {
                                        DataToTextFileHijacked();
                                    }
                                    else if (panel_urgent.Visible == true)
                                    {
                                        DataToTextFileHijacked_Urgent();
                                    }
                                }
                                else
                                {
                                    if (panel_main.Visible == true)
                                    {
                                        DataToTextFileSuccess();
                                    }
                                    else if (panel_urgent.Visible == true)
                                    {
                                        DataToTextFileSuccess_Urgent();
                                    }
                                }
                            }
                            // -------- End of Data To Text File

                            await Task.Run(async () =>
                            {
                                await Task.Delay(3000);
                            });

                            if (panel_main.Visible == true)
                            {
                                if (button_start_fires)
                                {
                                    label_ifloadornot.Text = "1";
                                    label_ifloadornot.Text = "0";
                                    buttonGoWasClicked = false;
                                }
                            }
                            else if (panel_urgent.Visible == true)
                            {
                                if (button_start_urgent_fires)
                                {
                                    label_ifloadornot_urgent.Text = "1";
                                    label_ifloadornot_urgent.Text = "0";
                                }
                            }
                        }
                    }
                }
            }
        }

        // Timeout/Hijacked
        private async void timer_handler_TickAsync(object sender, EventArgs e)
        {
            if (upload_one_time)
            {
                if (timeout)
                {
                    // handlers
                    end_load = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    webbrowser_handler_title = webBrowser_handler.DocumentTitle;
                    webbrowser_handler_url = webBrowser_handler.Url;
                    completed = false;
                    pictureBox_loader.Visible = false;
                    pictureBox_loader.Enabled = false;
                    webBrowser_handler.Stop();
                    timer_handler.Stop();

                    // -------- Data To Text File
                    string strValue = label_textsearch_brand.Text;
                    string[] strArray = strValue.Split(',');

                    foreach (string obj in strArray)
                    {
                        bool contains = webbrowser_handler_title.Contains(obj);

                        if (contains == true)
                        {
                            Invoke(new Action(() =>
                            {
                                isHijacked = false;
                            }));

                            break;
                        }
                        else if (!contains)
                        {
                            Invoke(new Action(() =>
                            {
                                isHijacked = true;
                            }));
                        }
                    }

                    if (isHijacked)
                    {
                        if (label_webtype.Text == "Landing Page" || label_webtype.Text == "Landing page")
                        {
                            var html = "";

                            if (!domain.Contains("http"))
                            {
                                try
                                {
                                    replace_domain_get = "http://" + domain;
                                    html = new WebClient().DownloadString(replace_domain_get);
                                }
                                catch (Exception)
                                {
                                    html = "";
                                }
                            }
                            else
                            {
                                try
                                {
                                    html = new WebClient().DownloadString(domain);
                                }
                                catch (Exception)
                                {
                                    html = "";
                                }
                            }

                            if (html.Contains("landing_image"))
                            {
                                if (panel_main.Visible == true)
                                {
                                    DataToTextFileSuccess();
                                }
                                else if (panel_urgent.Visible == true)
                                {
                                    DataToTextFileSuccess_Urgent();
                                }
                            }
                            else
                            {
                                if (panel_main.Visible == true)
                                {
                                    DataToTextFileHijacked();
                                }
                                else if (panel_urgent.Visible == true)
                                {
                                    DataToTextFileHijacked_Urgent();
                                }
                            }
                        }
                        else
                        {
                            if (panel_main.Visible == true)
                            {
                                DataToTextFileHijacked();
                            }
                            else if (panel_urgent.Visible == true)
                            {
                                DataToTextFileHijacked_Urgent();
                            }
                        }
                    }
                    else
                    {
                        if (panel_main.Visible == true)
                        {
                            DataToTextFileTimeout();
                        }
                        else if (panel_urgent.Visible == true)
                        {
                            DataToTextFileTimeout_Urgent();
                        }
                    }
                    // -------- End of Data To Text File

                    await Task.Run(async () =>
                    {
                        await Task.Delay(3000);
                    });
                    
                    if (panel_main.Visible == true)
                    {
                        label_ifloadornot.Text = "1";
                        label_ifloadornot.Text = "0";
                        buttonGoWasClicked = false;
                    }
                    else if (panel_urgent.Visible == true)
                    {
                        label_ifloadornot_urgent.Text = "1";
                        label_ifloadornot_urgent.Text = "0";
                    }
                }
            }
        }

        private void TakeScreenShot()
        {
            string datetime = label11.Text;
            string datetime_folder = label9.Text;
            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string path = path_desktop + "\\rainCheck\\" + datetime_folder + "\\" + datetime_folder;

            string path_create_rainCheck = path_desktop + "\\rainCheck\\" + datetime_folder;

            DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

            using (var pic = new Bitmap(webBrowser_handler.Width - 18, webBrowser_handler.Height - 18))
            {
                webBrowser_handler.DrawToBitmap(pic, new Rectangle(0, 0, pic.Width, pic.Height));
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

                if (fileLength < 3200 && webbrowser_handler_title == "Can’t reach this page" || fileLength < 3200 && webbrowser_handler_title == "无法访问此页面")
                {
                    var access = new Bitmap(Properties.Resources.access);
                    access.Save(full_path, ImageFormat.Jpeg);
                }
                else if (fileLength < 3200 && webbrowser_handler_title == "This site isn’t secure" || fileLength < 3200 && webbrowser_handler_title == "此站点不安全")
                {
                    var secure = new Bitmap(Properties.Resources.secure);
                    secure.Save(full_path, ImageFormat.Jpeg);
                }
                else if (fileLength < 3200 && webbrowser_handler_title == "Navigation Canceled" || fileLength < 3200 && webbrowser_handler_title == "导航已取消")
                {
                    var navigation = new Bitmap(Properties.Resources.navigation);
                    navigation.Save(full_path, ImageFormat.Jpeg);
                }
                else
                {
                    resized.Save(full_path, ImageFormat.Jpeg);
                }
            }
        }

        private void TakeScreenShot_Urgent()
        {
            string datetime = label11.Text;
            string datetime_folder = label9.Text;
            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string path = path_desktop + "\\rainCheck\\" + label_getdatetime_urgent.Text + "_urgent_" + i_timeout + "\\" + label_getdatetime_urgent.Text;

            string path_create_rainCheck = path_desktop + "\\rainCheck\\" + label_getdatetime_urgent.Text + "_urgent_" + i_timeout;

            DirectoryInfo di = Directory.CreateDirectory(path_create_rainCheck);

            using (var pic = new Bitmap(webBrowser_handler.Width - 18, webBrowser_handler.Height - 18))
            {
                webBrowser_handler.DrawToBitmap(pic, new Rectangle(0, 0, pic.Width, pic.Height));
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
                button_start_fires = false;
                button_start_urgent_fires = false;
                upload_one_time = true;
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
            button_start_fires = false;
            button_start_urgent_fires = false;
            upload_one_time = true;
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
            
            if (!button_start_fires && label_status.Text == "[Paused]")
            {
                pictureBox_loader.Visible = false;
                pictureBox_loader.Enabled = false;
                //textBox_domain.Text = "";
            }
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

            button_start_urgent_fires = true;
            pictureBox_loader_urgent.Visible = true;

            // Set browser panel dock style
            panel_browser_urgent.Visible = true;
            panel_browser_urgent.BringToFront();

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
            timer_handler.Start();

            button_pause_urgent.Visible = true;
            button_start_urgent.Visible = false;

            label_inaccessible_error_message.Text = "";

            button_startover_urgent.Enabled = true;

            buttonDetect = true;

            urgentRunning = true;
        }
        
        private void Button_pause_urgent_Click(object sender, EventArgs e)
        {
            timer_blink_urgent.Start();
            label_status_urgent.Text = "[Paused]";
            timer_domain_urgent.Stop();
            timer_handler.Stop();
            pictureBox_loader_urgent.Visible = false;

            button_start_urgent_fires = false;

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
                    domain = row.Cells[1].Value.ToString();
                    string brand = row.Cells[2].Value.ToString();
                    string text_search = row.Cells[3].Value.ToString();
                    string webtype = row.Cells[4].Value.ToString();
                    
                    if (button_start_urgent_fires)
                    {
                        start_load = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        webBrowser_handler.Visible = true;
                        webBrowser_handler.BringToFront();
                        pictureBox_loader_urgent.Visible = true;
                        pictureBox_loader_urgent.Enabled = true;

                        timer_handler.Stop();
                        timer_handler.Start();

                        completed = true;
                        timeout = true;

                        webBrowser_handler.Navigate(domain);
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
                    if (upload_one_time)
                    {
                        button_start_urgent_fires = false;
                        webBrowser_handler.Stop();
                        upload_one_time = false;
                        webBrowser_handler.Visible = false;
                        
                        textBox_domain.Text = "";

                        dataGridView_urgent.ClearSelection();

                        index_urgent = 0;
                        label_domainscount_urgent.Text = "Total: " + domain_total.ToString();
                        
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
                            MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1016", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1017", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

            //if (label_currentindex.Text == "0" && label_status.Text == "[Waiting]")
            //{
            //    pictureBox_loader.Visible = false;
            //    textBox_domain.Text = "";
            //}
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
        private bool button_start_fires = false;
        private string domain;
        private bool upload_one_time = true;
        private bool timeout = true;
        private bool completed = true;
        private string webbrowser_handler_title;
        private Uri webbrowser_handler_url;
        private bool isHijacked;
        private bool isInaccessible;
        private string replace_domain_get;
        private bool button_start_urgent_fires;

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
                }
            }
        }

        private void label_inaccessible_error_message_TextChanged(object sender, EventArgs e)
        {
            if (label_inaccessible_error_message.Text == "ERR_INTERNET_DISCONNECTED")
            {
                timer_domain.Stop();
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
                //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1036", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

        public bool IsChinese(string text)
        {
            return text.Any(c => (uint)c >= 0x4E00 && (uint)c <= 0x2FA1F);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(GetMacAddress());
        }

        private string GetMacAddress()
        {
            //const int MIN_MAC_ADDR_LENGTH = 12;
            //string macAddress = string.Empty;
            //long maxSpeed = -1;

            //foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            //{
            //    //MessageBox.Show(
            //    //    "Found MAC Address: " + nic.GetPhysicalAddress() +
            //    //    " Type: " + nic.NetworkInterfaceType);

            //    string tempMac = nic.GetPhysicalAddress().ToString();
            //    if (nic.Speed > maxSpeed &&
            //        !string.IsNullOrEmpty(tempMac) &&
            //        tempMac.Length >= MIN_MAC_ADDR_LENGTH)
            //    {
            //        //MessageBox.Show("New Max Speed = " + nic.Speed + ", MAC: " + tempMac);
            //        //maxSpeed = nic.Speed;
            //        macAddress = tempMac;
            //    }
            //}

            //return macAddress;

            string macAddresses = string.Empty;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces, thereby ignoring any
                // loopback devices etc.
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                nic.OperationalStatus == OperationalStatus.Up && !nic.Name.Contains("Tunnel") && !nic.Name.Contains("VirtualBox"))
                    macAddresses += nic.GetPhysicalAddress().ToString();
            }

            return macAddresses;
        }
    }
}