using CefSharp;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class Form_Landing : Form
    {
        MySqlConnection con = new MySqlConnection("server= ;user id=ssimecgp_ssiit;password=p0w3r@SSI;database=ssimecgp_raincheck;persistsecurityinfo=True;SslMode=none");

        static bool networkIsAvailable = false;

        public Form_Landing()
        {
            InitializeComponent();

            var culture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        private void Form_Landing_Load(object sender, EventArgs e)
        {
            GetTime();

            // Checking internet connection
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface nic in nics)
            {
                if ((nic.NetworkInterfaceType != NetworkInterfaceType.Loopback && nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel) && nic.OperationalStatus == OperationalStatus.Up)
                {
                    networkIsAvailable = true;
                }
            }

            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);
        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            networkIsAvailable = e.IsAvailable;

            if (networkIsAvailable)
            {
                Invoke(new Action(() =>
                {
                    authorisation = 0;
                    label_timer.Text = "";

                    timer_apichanges.Stop();

                    i = 0;
                    timer.Start();
                }));
            }
            else
            {
                Invoke(new Action(() =>
                {
                    panel_retry.Visible = true;
                    panel_retry.BringToFront();

                    panel_authorization.Visible = false;

                    authorisation = 0;
                    timer_authorisation.Stop();

                    timer_apichanges.Stop();

                    i = 0;
                    timer.Stop();
                }));
            }
        }

        // Get external IP
        private string GetExternalIp()
        {
            try
            {
                string externalIP;
                externalIP = (new WebClient()).DownloadString("https://canihazip.com/s");
                externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                             .Matches(externalIP)[0].ToString();
                return externalIP;
            }
            catch(Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: RC1000", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();

                return null;
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

        int i = 0;
        string city;
        string region;
        string country;
        string isp;

        private void Timer_Tick(object sender, EventArgs e)
        {
            panel_loader.BringToFront();
            i++;

            if (i > 20)
            {
                timer.Stop();

                if (networkIsAvailable)
                {
                    var API_PATH_IP_API = "http://ip-api.com/json/" + GetExternalIp();

                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        client.BaseAddress = new Uri(API_PATH_IP_API);
                        HttpResponseMessage response = client.GetAsync(API_PATH_IP_API).GetAwaiter().GetResult();
                        if (response.IsSuccessStatusCode)
                        {
                            var locationDetails = response.Content.ReadAsAsync<IpInfo>().GetAwaiter().GetResult();
                            if (locationDetails != null)
                            {
                                label_macid.Text = GetMACAddress();
                                label_ip.Text = GetExternalIp();
                                label_city.Text = locationDetails.city;
                                label_region.Text = locationDetails.regionName;
                                label_country.Text = locationDetails.country;
                                label_isp.Text = locationDetails.isp;

                                city = locationDetails.city;
                                country = locationDetails.country;
                                isp = locationDetails.isp; 
                                region = locationDetails.regionName;

                                string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                // INSERT device
                                //InsertDeviceCondition("INSERT INTO `devices`(`id`, `device_id`, `status`, `city`, `province`, `country`, `isp`, `date_received`, `updated_by`, `updated_date`) VALUES (null,'" + GetMACAddress() + "','X','" + locationDetails.city + "','" + locationDetails.regionName + "','" + locationDetails.country + "','" + locationDetails.isp + "','" + datetime + "',null,null)"); 

                                // Test get
                                //TestGet();

                                InsertDeviceCondition();
                            }
                        }
                    }
                }
                else
                {
                    panel_retry.BringToFront();
                }
            }
        }

        // Insert device condition
        private void InsertDeviceCondition()
        {
            try
            {
                using (var client = new WebClient())
                {
                    string auth = "r@inCh3ckd234b70";
                    string type = "landing";
                    string request = "http://raincheck.ssitex.com/api/api.php";
                    string mac_id = GetMACAddress();

                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "auth", auth },
                        { "type", type },
                        { "mac_id", mac_id },
                        { "city", city },
                        { "region", region },
                        { "country", country },
                        { "isp", isp }
                    };

                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));

                    if (pagesource != "")
                    {
                        JArray jsonObject = JArray.Parse(pagesource);
                        string status = jsonObject[0]["status"].Value<string>();

                        if (status == "A")
                        {
                            timer_authorisation.Stop();
                            timer_apichanges.Stop();
                            timer_apichanges.Enabled = false;

                            panel_verified.BringToFront();

                            timer_gotomain.Start();
                        }
                        else if (status == "P")
                        {
                            timer_authorisation.Start();
                            panel_authorization.BringToFront();
                        }
                        else if (status == "R")
                        {
                            timer_authorisation.Stop();
                            timer_apichanges.Stop();
                            timer_apichanges.Enabled = false;

                            panel_blank.BringToFront();
                            MessageBox.Show("You're rejected to the system! Please contact IT support.", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Close();
                        }
                        else if (status == "X")
                        {
                            timer_authorisation.Stop();
                            timer_apichanges.Stop();
                            timer_apichanges.Enabled = false;

                            panel_blank.BringToFront();
                            MessageBox.Show("You're removed to the system! Please contact IT support.", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Close();
                        }
                        else
                        {
                            timer_authorisation.Stop();
                            timer_apichanges.Stop();
                            timer_apichanges.Enabled = false;

                            MessageBox.Show("There is a problem! Please contact IT support.", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Close();
                        }

                        label_apichanges.Text = pagesource;
                        timer_apichanges.Start();
                    }
                    else
                    {
                        // Insert
                        type = "device_insert";

                        NameValueCollection postData_new = new NameValueCollection()
                        {
                            { "auth", auth },
                            { "type", type },
                            { "mac_id", mac_id },
                            { "city", city },
                            { "region", region },
                            { "country", country },
                            { "isp", isp }
                        };

                        string pagesource_new = Encoding.UTF8.GetString(client.UploadValues(request, postData_new));

                        timer_authorisation.Start();
                        panel_authorization.BringToFront();

                        label_apichanges.Text = pagesource;
                        timer_apichanges.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                timer.Stop();
                timer_apichanges.Stop();
                timer_authorisation.Stop();
                timer_gotomain.Stop();

                panel_blank.BringToFront();

                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: RC1001", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Close();
                Application.Restart();
            }
        }

        private void Timer_apichanges_Tick(object sender, EventArgs e)
        {
            try
            {
                using (var client = new WebClient())
                {
                    string auth = "r@inCh3ckd234b70";
                    string type = "landing";
                    string request = "http://raincheck.ssitex.com/api/api.php";
                    string mac_id = GetMACAddress();

                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "auth", auth },
                        { "type", type },
                        { "mac_id", mac_id }
                    };

                    // client.UploadValues returns page's source as byte array (byte[])
                    // so it must be transformed into a string
                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));

                    //MessageBox.Show(pagesource);

                    if (pagesource != "")
                    {
                        if (pagesource != label_apichanges.Text)
                        {
                            JArray jsonObject = JArray.Parse(pagesource);
                            string status = jsonObject[0]["status"].Value<string>();

                            if (status == "A")
                            {
                                timer_authorisation.Stop();
                                timer_apichanges.Stop();
                                timer_apichanges.Enabled = false;

                                panel_verified.BringToFront();

                                timer_gotomain.Start();
                            }
                            else if (status == "P")
                            {
                                timer_authorisation.Start();
                                panel_authorization.BringToFront();
                            }
                            else if (status == "R")
                            {
                                timer_authorisation.Stop();
                                timer_apichanges.Stop();
                                timer_apichanges.Enabled = false;

                                panel_blank.BringToFront();
                                MessageBox.Show("You're rejected to the system! Please contact IT support.", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Close();
                            }
                            else if (status == "X")
                            {
                                timer_authorisation.Stop();
                                timer_apichanges.Stop();
                                timer_apichanges.Enabled = false;

                                panel_blank.BringToFront();
                                MessageBox.Show("You're removed to the system! Please contact IT support.", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Close();
                            }
                            else
                            {
                                timer_authorisation.Stop();
                                timer_apichanges.Stop();
                                timer_apichanges.Enabled = false;

                                MessageBox.Show("There is a problem! Please contact IT support.", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Close();
                            }

                            label_apichanges.Text = pagesource;
                        }
                    }
                    else
                    {
                        // Insert
                        type = "device_insert";

                        NameValueCollection postData_new = new NameValueCollection()
                        {
                            { "auth", auth },
                            { "type", type },
                            { "mac_id", mac_id },
                            { "city", city },
                            { "region", region },
                            { "country", country },
                            { "isp", isp }
                        };

                        string pagesource_new = Encoding.UTF8.GetString(client.UploadValues(request, postData_new));

                        timer_authorisation.Start();
                        panel_authorization.BringToFront();

                        label_apichanges.Text = pagesource;
                        timer_apichanges.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: RC1002", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            InsertDeviceCondition();
        }

        int gotomain = 0;
        private void Timer_gotomain_Tick(object sender, EventArgs e)
        {
            gotomain++;
            if (gotomain == 2)
            {
                timer_apichanges.Stop();
                timer.Stop();
                timer_authorisation.Stop();
                timer_gotomain.Stop();
                
                timer_apichanges.Enabled = false;
                timer.Enabled = false;
                timer_authorisation.Enabled = false;
                timer_gotomain.Enabled = false;
                
                Hide();
                Close();

                //city, country, isp
                string path = Path.GetTempPath() + @"\raincheck_brand.txt";
                string path_history = Path.GetTempPath() + @"\raincheck_history.txt";
                string path_lastcurrentindex = Path.GetTempPath() + @"\raincheck_lastcurrentindex.txt";
                string result = "";

                string path_datetime = Path.GetTempPath() + @"\raincheck_datetime.txt";
                if (File.Exists(path_datetime))
                {
                    string read = File.ReadAllText(path_datetime);
                    string datetime = DateTime.Now.ToString("dd MMM ") + label_timefor.Text;
                    
                    if (read != datetime)
                    {
                        result = "not equal";
                    }
                    else
                    {
                        result = "equal";
                    }
                }
                              
                if (File.Exists(path_history))
                {
                    string date_history = DateTime.Now.ToString("dd MMM ");
                    string result_history = "";

                    using (StreamReader sr = File.OpenText(path_history))
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
                        Form_Main.SetResult = "No";

                        if (File.Exists(path_lastcurrentindex))
                        {
                            File.Delete(path_lastcurrentindex);
                        }

                        if (File.Exists(path_datetime))
                        {
                            File.Delete(path_datetime);
                        }

                        Form_Main form_main = new Form_Main(city, country, isp);
                        form_main.ShowDialog();
                    }
                    else
                    {
                        if (result == "not equal")
                        {
                            Form_Main.SetResult = "No";

                            if (File.Exists(path_lastcurrentindex))
                            {
                                File.Delete(path_lastcurrentindex);
                            }

                            if (File.Exists(path))
                            {
                                File.Delete(path);
                            }

                            if (File.Exists(path_datetime))
                            {
                                File.Delete(path_datetime);
                            }

                            Form_Main form_main = new Form_Main(city, country, isp);
                            form_main.ShowDialog();
                        }
                        else
                        {
                            if (File.Exists(path))
                            {
                                DialogResult dr = MessageBox.Show("Do you want to continue the previous checking?", "rainCheck", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (dr == DialogResult.Yes)
                                {
                                    Form_Main.SetResult = "Yes";

                                    string read = File.ReadAllText(path);
                                    Form_Main.BrandIDs = read;

                                    if (File.Exists(path_lastcurrentindex))
                                    {
                                        string read_lastcurrentindex = File.ReadAllText(path_lastcurrentindex);
                                        Form_Main.LastCurrentIndex = read_lastcurrentindex;
                                    }
                                    else
                                    {
                                        Form_Main.LastCurrentIndex = "1";
                                    }

                                    Form_Main form_main = new Form_Main(city, country, isp);
                                    form_main.ShowDialog();
                                }
                                else
                                {
                                    Form_Main.SetResult = "No";

                                    if (File.Exists(path_lastcurrentindex))
                                    {
                                        File.Delete(path_lastcurrentindex);
                                    }

                                    if (File.Exists(path_datetime))
                                    {
                                        File.Delete(path_datetime);
                                    }

                                    Form_Main form_main = new Form_Main(city, country, isp);
                                    form_main.ShowDialog();
                                }
                            }
                            else
                            {
                                Form_Main.SetResult = "Not Exists";

                                if (File.Exists(path_lastcurrentindex))
                                {
                                    File.Delete(path_lastcurrentindex);
                                }

                                if (File.Exists(path_datetime))
                                {
                                    File.Delete(path_datetime);
                                }

                                Form_Main form_main = new Form_Main(city, country, isp);
                                form_main.ShowDialog();
                            }
                        }
                    }
                }
                else
                {
                    Form_Main.SetResult = "Not Exists";

                    if (File.Exists(path_lastcurrentindex))
                    {
                        File.Delete(path_lastcurrentindex);
                    }

                    if (File.Exists(path_datetime))
                    {
                        File.Delete(path_datetime);
                    }

                    Form_Main form_main = new Form_Main(city, country, isp);
                    form_main.ShowDialog();
                }                
            }
        }

        int authorisation = 0;
        private void Timer_authorisation_Tick(object sender, EventArgs e)
        {
            authorisation++;
            label_timer.Text = authorisation.ToString();

            if (authorisation > 60)
            {
                Application.Exit();
                Close();
            }
        }

        private void GetTime()
        {
            string time = DateTime.Now.ToString("HH:mm");
            string result = time.Replace(":", ".");

            if (Convert.ToDouble(result) >= 0 && Convert.ToDouble(result) <= 1.59)
            {
                label_timefor.Text = "00:00";
            }
            else if (Convert.ToDouble(result) >= 2 && Convert.ToDouble(result) <= 3.59)
            {
                label_timefor.Text = "02:00";
            }
            else if (Convert.ToDouble(result) >= 4 && Convert.ToDouble(result) <= 5.59)
            {
                label_timefor.Text = "04:00";
            }
            else if (Convert.ToDouble(result) >= 6 && Convert.ToDouble(result) <= 7.59)
            {
                label_timefor.Text = "06:00";
            }
            else if (Convert.ToDouble(result) >= 8 && Convert.ToDouble(result) <= 9.59)
            {
                label_timefor.Text = "08:00";
            }
            else if (Convert.ToDouble(result) >= 10 && Convert.ToDouble(result) <= 11.59)
            {
                label_timefor.Text = "10:00";
            }
            else if (Convert.ToDouble(result) >= 12 && Convert.ToDouble(result) <= 13.59)
            {
                label_timefor.Text = "12:00";
            }
            else if (Convert.ToDouble(result) >= 14 && Convert.ToDouble(result) <= 15.59)
            {
                label_timefor.Text = "14:00";
            }
            else if (Convert.ToDouble(result) >= 16 && Convert.ToDouble(result) <= 17.59)
            {
                label_timefor.Text = "16:00";
            }
            else if (Convert.ToDouble(result) >= 18 && Convert.ToDouble(result) <= 19.59)
            {
                label_timefor.Text = "18:00";
            }
            else if (Convert.ToDouble(result) >= 20 && Convert.ToDouble(result) <= 21.59)
            {
                label_timefor.Text = "20:00";
            }
            else if (Convert.ToDouble(result) >= 22 && Convert.ToDouble(result) <= 23.59)
            {
                label_timefor.Text = "22:00";
            }
        }

        private void Form_Landing_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }
    }
}
