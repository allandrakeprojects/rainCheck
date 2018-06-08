using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Xml;

namespace rainCheck
{
    public partial class Form_Landing : Form
    {

        public Form_Landing()
        {
            InitializeComponent();            
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

            if (i == 20)
            {
                timer.Stop();
                if (CheckForInternetConnection())
                {
                    panel_authorization.BringToFront();
                }
                else
                {
                    panel_retry.BringToFront();
                }
            }     
        }

        // Button retry
        private void button_retry_Click(object sender, EventArgs e)
        {
            i = 0;
            timer.Start();
        }

        private void button_authorize_Click(object sender, EventArgs e)
        {
            
            label1.Text = GetUserCountryByIp("43.226.5.59");

        }

        // Country
        public static string GetUserCountryByIp(string ip)
        {
            IpInfo ipInfo = new IpInfo();
            try
            {
                string info = new WebClient().DownloadString("http://ipinfo.io/" + ip);
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName;
            }
            catch (Exception)
            {
                ipInfo.Country = null;
            }

            return ipInfo.Country;
        }
    }
}
