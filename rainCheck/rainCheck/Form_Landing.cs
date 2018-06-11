using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class Form_Landing : Form
    {
        public Form_Landing()
        {
            InitializeComponent();
        }

        // Get external IP
        private string getExternalIp()
        {
            try
            {
                string externalIP;
                externalIP = (new WebClient()).DownloadString("https://canihazip.com/s");
                externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                             .Matches(externalIP)[0].ToString();
                return externalIP;
            }
            catch { return null; }
        }

        // Checking if connected to internet
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // Get MAC Address
        public void GetMACAddress()
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
            //label_mac_address.Text = sMacAddress;
        }

        int i = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            panel_loader.BringToFront();
            i++;

            //label4.Text = i.ToString();

            if (i >= 20)
            {
                timer.Stop();
                if (CheckForInternetConnection())
                {
                    label_ip.Text = getExternalIp();

                    var API_PATH_IP_API = "http://ip-api.com/json/" + getExternalIp();

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
                                label_city.Text = locationDetails.city;
                                label_region.Text = locationDetails.regionName;
                                label_country.Text = locationDetails.country;
                                label_isp.Text = locationDetails.isp;
                            }
                        }
                    }

                    panel_authorization.BringToFront();
                    button_retry.Enabled = false;
                }
                else
                {
                    panel_retry.BringToFront();
                    button_retry.Enabled = true;
                }
            }
        }

        // Button retry
        private void button_retry_Click(object sender, EventArgs e)
        {
            i = 0;
            timer.Start();
        }

        private void button_retry_KeyDown(object sender, KeyEventArgs e)
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
    }
}
