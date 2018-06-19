using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class Form_Landing : Form
    {
        MySqlConnection con = new MySqlConnection("server=mysql5018.site4now.net;user id=a3d1a6_check;password=admin12345;database=db_a3d1a6_check;persistsecurityinfo=True;SslMode=none");

        public Form_Landing()
        {
            InitializeComponent();
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
        private void Timer_Tick(object sender, EventArgs e)
        {
            panel_loader.BringToFront();
            i++;

            if (i >= 20)
            {
                timer.Stop();
                if (CheckForInternetConnection())
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

                                string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                // INSERT device
                                InsertDeviceCondition("INSERT INTO `devices`(`id`, `device_id`, `status`, `city`, `province`, `country`, `isp`, `date_received`, `updated_by`, `updated_date`) VALUES (null,'" + GetMACAddress() + "','X','" + locationDetails.city + "','" + locationDetails.regionName + "','" + locationDetails.country + "','" + locationDetails.isp + "','" + datetime + "',null,null)"); 
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
        private void InsertDeviceCondition(string query)
        {
            using (con)
            {
                try
                {
                    con.Open();
                    MySqlCommand command_chck = new MySqlCommand("SELECT * FROM `devices` WHERE device_id = '" + GetMACAddress() + "'", con);
                    command_chck.CommandType = CommandType.Text;
                    MySqlDataReader reader_chck = command_chck.ExecuteReader();

                    if (!reader_chck.HasRows)
                    {
                        con.Close();
                        con.Open();
                        MySqlCommand command = new MySqlCommand(query, con);
                        MySqlDataReader reader = command.ExecuteReader();
                        con.Close();

                        panel_authorization.BringToFront();
                        button_retry.Enabled = false;
                    }
                    else
                    {
                        con.Close();
                        con.Open();
                        MySqlCommand command_approved = new MySqlCommand("SELECT * FROM `devices` WHERE device_id = '" + GetMACAddress() + "' AND status = 'A'", con);
                        command_approved.CommandType = CommandType.Text;
                        MySqlDataReader reader_approved = command_approved.ExecuteReader();

                        if (reader_approved.HasRows)
                        {
                            con.Close();

                            Form_Main form_main = new Form_Main();
                            this.Hide();
                            form_main.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            con.Close();

                            panel_authorization.BringToFront();
                            button_retry.Enabled = false;
                        }
                    }
                    con.Close();
                }
                catch (Exception)
                {
                    con.Close();

                    panel_blank.BringToFront();
                    MessageBox.Show("There is a problem with the server! Please contact IT support.", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}
