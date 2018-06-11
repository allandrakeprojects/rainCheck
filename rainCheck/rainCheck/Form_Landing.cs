using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class Form_Landing : Form
    {

        Timer timer_authorisation = new Timer();
        Timer timer_main = new Timer();

        public Form_Landing()
        {
            InitializeComponent();
        }

        private void Form_Landing_Load(object sender, EventArgs e)
        {
            goToPanelAuthorisation();
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

        int i_authorisation = 0;
        private void goToPanelAuthorisation() {
            panel_loader.BringToFront();
            timer_authorisation.Enabled = true;
            timer_authorisation.Interval = 500;
            timer_authorisation.Tick += (t, args) =>
            {
                i_authorisation++;
                label4.Text = i_authorisation.ToString();

                if (i_authorisation == 4)
                {
                    timer_authorisation.Stop();
                    if (CheckForInternetConnection())
                    {
                        panel_authorization.BringToFront();
                    }
                    else
                    {
                        panel_retry_authorisation.BringToFront();
                    }
                }
            };
        }

        int i_main = 0;
        private void goToPanelMain()
        {
            if (CheckForInternetConnection())
            {
                Form_Main form_main = new Form_Main();
                this.Hide();
                form_main.ShowDialog();
                this.Close();
            }
            else
            {
                panel_loader.BringToFront();
                timer_main.Interval = 500;
                timer_main.Tick += (t, args) =>
                {
                    i_main++;
                    label4.Text = i_main.ToString();

                    if (i_main == 4)
                    {
                        timer_main.Stop();
                        if (CheckForInternetConnection())
                        {
                            Form_Main form_main = new Form_Main();
                            this.Hide();
                            form_main.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            panel_retry_main.BringToFront();
                        }
                    }
                };
                timer_main.Enabled = true;
            }
        }

        // Button retry
        private void button_retry_authorisation_Click(object sender, EventArgs e)
        {
            panel_loader.BringToFront();
            timer_authorisation.Start();
            timer_authorisation.Interval = 500;
            timer_authorisation.Tick += (t, args) =>
            {
                i_authorisation++;

                if (i_authorisation >= 4)
                {
                    timer_authorisation.Stop();
                    if (CheckForInternetConnection())
                    {
                        panel_authorization.BringToFront();
                    }
                    else
                    {
                        panel_retry_authorisation.BringToFront();
                    }
                }
            };
        }

        private void button_retry_main_Click(object sender, EventArgs e)
        {
            panel_loader.BringToFront();
            timer_main.Start();
            timer_main.Interval = 500;
            timer_main.Tick += (t, args) =>
            {
                i_main++;

                if (i_main >= 4)
                {
                    timer_main.Stop();
                    if (CheckForInternetConnection())
                    {
                        Form_Main form_main = new Form_Main();
                        this.Hide();
                        form_main.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        panel_retry_main.BringToFront();
                    }
                }
            };
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

        private void button1_Click(object sender, EventArgs e)
        {
            goToPanelMain();
        }
    }
}
