using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
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

            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);
        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            networkIsAvailable = e.IsAvailable;

            if (networkIsAvailable)
            {
                Invoke(new Action(() =>
                {
                    panel_authorization.Visible = true;
                    panel_authorization.BringToFront();

                    panel_retry.Visible = false;
                }));
            }
            else
            {
                Invoke(new Action(() =>
                {
                    panel_authorization.Visible = false;

                    panel_retry.Visible = true;
                    panel_retry.BringToFront();
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

            if (i >= 20)
            {
                timer.Stop();
                if (networkIsAvailable == true)
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
                    button_retry.Enabled = true;
                }
            }
        }

        // Button retry
        private void Button_retry_Click(object sender, EventArgs e)
        {
            i = 0;
            timer.Start();
        }

        // F5
        private void Button_retry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                if(i >= 20)
                {
                    i = 0;
                    timer.Start();
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
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: RC1001", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
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
                timer_apichanges.Enabled = false;
                timer_gotomain.Stop();
                //city, country, isp
                Form_Main form_main = new Form_Main(city, country, isp);

                Hide();
                form_main.ShowDialog();
                Close();
            }
        }

        int authorisation = 0;
        private void Timer_authorisation_Tick(object sender, EventArgs e)
        {
            authorisation++;
            label_timer.Text = authorisation.ToString();

            if (authorisation > 60)
            {
                Close();
            }
        }

        private void Form_Landing_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                using (var client = new WebClient())
                {
                    string auth = "r@inCh3ckd234b70";
                    string type = "closing";
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
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: RC1003", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                //Close();
            }
        }
    }
}
