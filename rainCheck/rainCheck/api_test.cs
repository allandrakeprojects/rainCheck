using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class api_test : Form
    {
        public api_test()
        {
            InitializeComponent();
            

            string url = "http://raincheck.ssitex.com/api.txt";
            GetRESTData(url);
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
























        void get_data(string response)
        {
            dataGridView_api_test.AutoGenerateColumns = false;
            dataGridView_api_test.Rows.Clear();
            JArray fetch = JArray.Parse(response);
            if (fetch.Count() > 0)
            {
                for (int i = 0; dataGridView_api_test.Rows.Count > i; i++)
                {
                    int n = dataGridView_api_test.Rows.Add();
                    dataGridView_api_test.Rows[n].Cells[0].Value = fetch[i]["JsonObjectName1"].ToString();
                }
            }
        }

        private void button_getmaindomains_Click(object sender, EventArgs e)
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    string api = "http://raincheck.ssitex.com/api.txt";
                    var result = JsonConvert.DeserializeObject<List<MainDomainsResult>>(api);
                    dataGridView_api_test.DataSource = result;
                    //MessageBox.Show(result.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timer_api_test_Tick(object sender, EventArgs e)
        {

        }
    }
}
