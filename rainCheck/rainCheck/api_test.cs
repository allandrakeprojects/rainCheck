using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class api_test : Form
    {
        private static readonly HttpClient client = new HttpClient();

        public api_test()
        {
            InitializeComponent();
        }

        private void api_test_Load(object sender, EventArgs e)
        {
            string auth = "http://raincheck.ssitex.com/Adminlogin/verify?username=admin&password=1243";
            AuthAPI(auth);
        }

        private void AuthAPI(string url)
        {
            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                if ((webResponse.StatusCode == HttpStatusCode.OK))
                {
                    WebClient wc = new WebClient();
                    var result = wc.DownloadString(url);
                    bool next = result.Contains("success");

                    if (next)
                    {
                        MessageBox.Show("authorize");
                        
                        string url_test = "http://raincheck.ssitex.com/api/response.html";
                        WebClient wc_test = new WebClient();
                        var result_test = wc_test.DownloadString(url_test);
                        MessageBox.Show(result_test);
                        //GetRESTData(result_test);
                    }
                    else
                    {
                        MessageBox.Show("ops");

                        string url_test = "http://raincheck.ssitex.com/api/response.html";
                        WebClient wc_test = new WebClient();
                        var result_test = wc_test.DownloadString(url_test);
                        MessageBox.Show(result_test);
                    }
                }
                else
                {
                    MessageBox.Show(string.Format("Status code == {0}", webResponse.StatusCode));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetRESTData(string url)
        {
            try
            {

                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                if ((webResponse.StatusCode == HttpStatusCode.OK) && (webResponse.ContentLength > 0))
                {
                    var reader = new StreamReader(webResponse.GetResponseStream());
                    string s = reader.ReadToEnd();
                    var arr = JsonConvert.DeserializeObject<JArray>(s);
                    label_api_test.Text = arr.ToString();
                    dataGridView_api_test.DataSource = arr;

                    webResponse.Close();
                    reader.Close();
                }
                else
                {
                    MessageBox.Show(string.Format("Status code == {0}", webResponse.StatusCode));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timer_api_test_Tick(object sender, EventArgs e)
        {
            try
            {
                string url = "http://raincheck.ssitex.com/Admin/api";
                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                if ((webResponse.StatusCode == HttpStatusCode.OK) && (webResponse.ContentLength > 0))
                {
                    var reader = new StreamReader(webResponse.GetResponseStream());
                    string s = reader.ReadToEnd();
                    var arr = JsonConvert.DeserializeObject<JArray>(s);

                    if (arr.ToString() != label_api_test.Text)
                    {
                        MessageBox.Show("changes detected");
                        label_api_test.Text = arr.ToString();
                        dataGridView_api_test.DataSource = arr;
                    }

                    webResponse.Close();
                    reader.Close();
                }
                else
                {
                    MessageBox.Show(string.Format("Status code == {0}", webResponse.StatusCode));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_post_Click_1(object sender, EventArgs e)
        {
            using (var client = new WebClient())
            {
                string auth = "r@inCh3ckd234b70";
                string type = "domain_main";
                string request = "http://raincheck.ssitex.com/api/api.php";

                NameValueCollection postData = new NameValueCollection()
                {
                    { "auth", auth },
                    { "type", type }
                };

                // client.UploadValues returns page's source as byte array (byte[])
                // so it must be transformed into a string
                string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));

                var arr = JsonConvert.DeserializeObject<JArray>(pagesource);
                
                dataGridView_api_test.DataSource = arr;

                MessageBox.Show(pagesource);

                //if (pagesource == "domain_main")
                //{
                //    WebClient wc = new WebClient();
                //    var result = wc.DownloadString(request);
                //    MessageBox.Show(result);
                //}
                
            }
        }
    }
}
