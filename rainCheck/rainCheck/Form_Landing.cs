﻿using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: RC1000", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();

                return null;
            }
        }

        // Get MAC Address
        public static string GetMACAddress()
        {
            string macAddress = string.Empty;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up && !nic.Name.Contains("Tunnel") && !nic.Name.Contains("VirtualBox") && !nic.Name.Contains("Adapter") && !nic.Description.Contains("Npcap") && !nic.Description.Contains("Loopback"))
                    macAddress += nic.GetPhysicalAddress().ToString();
            }

            return macAddress;
        }

        int i = 0;
        string city;
        string region;
        string country;
        string isp;

        private void Timer_TickAsync(object sender, EventArgs e)
        {
            panel_loader.BringToFront();
            i++;
            Task task_01 = new Task(delegate
            {
                Invoke(new Action(async () =>
                {
                    try
                    {
                        if (i > 20)
                        {
                            timer.Stop();

                            if (networkIsAvailable)
                            {
                                WebClient wc = new WebClient();
                                wc.Encoding = Encoding.UTF8;
                                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                                byte[] result = await wc.DownloadDataTaskAsync("http://ip-api.com/json/");
                                string responsebody = Encoding.UTF8.GetString(result);
                                var deserialize_object = JsonConvert.DeserializeObject(responsebody);
                                JObject jo = JObject.Parse(deserialize_object.ToString());

                                JToken query = jo.SelectToken("$.query");
                                JToken _city = jo.SelectToken("$.city");
                                JToken _regionName = jo.SelectToken("$.regionName");
                                JToken _country = jo.SelectToken("$.country");
                                JToken _isp = jo.SelectToken("$.isp");

                                label_macid.Text = GetMACAddress();
                                label_ip.Text = query.ToString();
                                label_city.Text = _city.ToString();
                                label_region.Text = _regionName.ToString();
                                label_country.Text = _country.ToString();
                                label_isp.Text = _isp.ToString();

                                city = _city.ToString();
                                country = _country.ToString();
                                isp = _isp.ToString();
                                region = _regionName.ToString();

                                InsertDeviceCondition();
                            }
                            else
                            {
                                panel_retry.BringToFront();
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        label_macid.Text = "DCFE0714662E";
                        label_city.Text = "Chengdu";
                        label_region.Text = "Sichuan";
                        label_country.Text = "China";
                        label_isp.Text = "UNICOM Sichuan";

                        city = "Chengdu";
                        country = "China";
                        isp = "UNICOM Sichuan";
                        region = "Sichuan";

                        InsertDeviceCondition();
                    }
                }));
            });
            task_01.Start();
        }

        // Insert device condition
        private void InsertDeviceCondition()
        {
            try
            {
                Application.DoEvents();
                using (var client = new WebClient())
                {
                    string auth = "r@inCh3ckd234b70";
                    string type = "landing";
                    string request = "http://raincheck.ssitex.com/Api";
                    string mac_id = label_macid.Text;

                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "auth", auth },
                        { "type", type },
                        { "mac_id", mac_id },
                        { "city", city },
                        { "province", region },
                        { "country", country },
                        { "isp", isp }
                    };

                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));
                    if (pagesource != "" && pagesource != "[]")
                    {
                        JArray jsonObject = JArray.Parse(pagesource);
                        string status = jsonObject[0]["status"].Value<string>();
                        string city_get = jsonObject[0]["city"].Value<string>();
                        string province_get = jsonObject[0]["province"].Value<string>();
                        string country_get = jsonObject[0]["country"].Value<string>();
                        string isp_get = jsonObject[0]["isp"].Value<string>();

                        city = city_get;
                        country = country_get;
                        isp = isp_get;
                        region = province_get;

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
                            { "province", region },
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
                //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: RC1001", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Close();
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
                    string request = "http://raincheck.ssitex.com/Api";
                    string mac_id = GetMACAddress();

                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "auth", auth },
                        { "type", type },
                        { "mac_id", mac_id }
                    };

                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));
                    if (pagesource != "" && pagesource != "[]")
                    {
                        if (pagesource != label_apichanges.Text)
                        {
                            JArray jsonObject = JArray.Parse(pagesource);
                            string status = jsonObject[0]["status"].Value<string>();
                            string city_get = jsonObject[0]["city"].Value<string>();
                            string province_get = jsonObject[0]["province"].Value<string>();
                            string country_get = jsonObject[0]["country"].Value<string>();
                            string isp_get = jsonObject[0]["isp"].Value<string>();

                            city = city_get;
                            country = country_get;
                            isp = isp_get;
                            region = province_get;

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
                            { "province", region },
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
                //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: RC1002", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }
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
                string path_autoyes = Path.GetTempPath() + @"\raincheck_autoyes.txt";
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
                                if (File.Exists(path_autoyes))
                                {
                                    File.Delete(path_autoyes);

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

                                    try
                                    {
                                        Form_Main form_main = new Form_Main(city, country, isp);
                                        form_main.ShowDialog();
                                    }
                                    catch (Exception err)
                                    {
                                        MessageBox.Show("No space left on device.", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
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
                        if (File.Exists(path_lastcurrentindex))
                        {
                            if (File.Exists(path))
                            {
                                if (File.Exists(path_autoyes))
                                {
                                    File.Delete(path_autoyes);
                                    
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
    }
}
