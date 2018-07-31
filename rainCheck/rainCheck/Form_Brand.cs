using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class Form_Brand : Form
    {
        MySqlConnection con = new MySqlConnection("server=mysql5018.site4now.net;user id=a3d1a6_check;password=admin12345;database=db_a3d1a6_check;persistsecurityinfo=True;SslMode=none");

        public Form_Brand(string domain_urgent)
        {
            InitializeComponent();

            int text_length = domain_urgent.Length;
            linkLabel_question.Text = "Select brand and website type for " + domain_urgent + " domain:";

            linkLabel_question.Links.Clear();
            linkLabel_question.Links.Add(34, text_length).Enabled = false;

            string hex = "#438eb9";
            Color color = ColorTranslator.FromHtml(hex);
            linkLabel_question.ForeColor = color;
            linkLabel_question.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
        }

        public class RootObject
        {
            public string brand_name { get; set; }
        }

        public class RootObjectWebsiteType
        {
            public string category_name { get; set; }
        }

        private void Form_Brand_Load(object sender, System.EventArgs e)
        {
            try
            {
                using (var client = new WebClient())
                {
                    string auth = "r@inCh3ckd234b70";
                    string type = "brand_get_popup";
                    string request = "http://raincheck.ssitex.com/api/api.php";

                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "auth", auth },
                        { "type", type }
                    };

                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));

                    var x = JsonConvert.DeserializeObject<List<RootObject>>(pagesource);

                    foreach (var brand in x)
                    {
                        comboBox_brand.Items.Add(brand.brand_name);
                    }

                    comboBox_brand.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is a problem with the server! Please contact IT support.", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                using (var client = new WebClient())
                {
                    string auth = "r@inCh3ckd234b70";
                    string type = "websitetype_get";
                    string request = "http://raincheck.ssitex.com/api/api.php";

                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "auth", auth },
                        { "type", type }
                    };

                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));

                    var x = JsonConvert.DeserializeObject<List<RootObjectWebsiteType>>(pagesource);

                    foreach (var websitetype in x)
                    {
                        comboBox_websitetype.Items.Add(websitetype.category_name);
                    }

                    comboBox_websitetype.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is a problem with the server! Please contact IT support. " + ex.Message, "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Button_start_urgent_Click(object sender, EventArgs e)
        {
            Form_Main.SetValueForTextBrandID = label_brand_id.Text;
            Form_Main.SetValueForTextSearch = label_text_search.Text;
            Form_Main.SetValueForWebsiteType = label_websitetype.Text;
            Close();
        }

        private void ComboBox_brand_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = comboBox_brand.Text;

            // API Brand
            try
            {
                using (var client = new WebClient())
                {
                    string auth = "r@inCh3ckd234b70";
                    string type = "brand_selected";
                    string request = "http://raincheck.ssitex.com/api/api.php";
                    string brand_selected = selected;

                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "auth", auth },
                        { "type", type },
                        { "brand_selected", brand_selected }
                    };

                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));

                    JArray jsonObject = JArray.Parse(pagesource);

                    string brand_id = jsonObject[0]["id"].Value<string>();
                    string text_search = jsonObject[0]["text_search"].Value<string>();

                    label_brand_id.Text = brand_id;
                    label_text_search.Text = text_search;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is a problem with the server! Please contact IT support.", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox_websitetype_SelectedIndexChanged(object sender, EventArgs e)
        {
            label_websitetype .Text = comboBox_websitetype.Text;

        }
    }
}
