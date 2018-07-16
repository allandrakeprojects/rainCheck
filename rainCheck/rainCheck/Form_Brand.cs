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
            linkLabel_question.Text = "Select brand for " + domain_urgent + " domain:";

            linkLabel_question.Links.Clear();
            linkLabel_question.Links.Add(17, text_length).Enabled = false;

            string hex = "#438eb9";
            Color color = ColorTranslator.FromHtml(hex);
            linkLabel_question.ForeColor = color;
            linkLabel_question.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
        }



















        public class RootObject
        {
            public string brand_name { get; set; }
        }

        private void Form_Brand_Load(object sender, System.EventArgs e)
        {
            using (var client = new WebClient())
            {
                string auth = "r@inCh3ckd234b70";
                string type = "brand_get";
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
                    //comboBox_brand.DisplayMember = brand.brand_name;
                }

                //MessageBox.Show(x.ToString());
                //foreach (var player in x.brand_name)
                //{
                //    //ComboboxItem item = new ComboboxItem();
                //    //item.Text = player.f + " " + player.l;
                //    //item.Value = player.id;

                //    //comboBox1.Items.Add(item);
                //}
















                //var s = new JavaScriptSerializer();


                //var blah = s.Deserialize<List<Workout>>(pagesource);
                //var response = ServicePOST<Workout>("AddUserWorkout", workout);
                //MessageBox.Show(blah.ToString());

















                //MessageBox.Show(pagesource);

                //Friends facebookFriends = new JavaScriptSerializer().Deserialize<List<Friends>>(pagesource);

                //foreach (var item in facebookFriends.data)
                //{
                //    MessageBox.Show(item.brand_name);
                //}

                //using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(pagesource)))
                //{
                //    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(RootObject));
                //    RootObject obj = (RootObject)deserializer.ReadObject(ms);

                //    MessageBox.Show(obj.brand_name);

                //    // Assign data to combo box
                //    foreach (var name in obj.brand_name)
                //    {
                //        ComboBox item = new ComboBox();
                //        item.Text = obj.brand_name;
                //        comboBox_brand.Items.Add(item);
                //    }
                //}


                //var arr = JsonConvert.DeserializeObject<RootObject>(pagesource);
                //foreach (var player in arr.Players)
                //{
                //    ComboBox item = new ComboBox();
                //    item.Text = player.brand_name;

                //    comboBox_brand.Items.Add(item);
                //}
                ////comboBox_brand.DataSource = arr;

                //MessageBox.Show(pagesource);
            }

            //using (con)
            //{
            //    try
            //    {
            //        con.Open();

            //        MySqlDataAdapter da = new MySqlDataAdapter();

            //        DataSet ds = new DataSet();

            //        string sql = "SELECT brand_name FROM `brands`";

            //        da.SelectCommand = new MySqlCommand(sql, con);

            //        da.Fill(ds);

            //        //ComboBox comboBox1 = new ComboBox();

            //        comboBox_brand.DataSource = ds.Tables[0];
            //        comboBox_brand.DisplayMember = "brand_name";

            //        con.Close();
            //    }
            //    catch (Exception)
            //    {
            //        con.Close();

            //        MessageBox.Show("There is a problem with the server! Please contact IT support.", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        Application.Exit();
            //    }
            //    finally
            //    {
            //        con.Close();
            //    }
            //}
        }

        private void Button_start_urgent_Click(object sender, EventArgs e)
        {
            Form_Main.SetValueForTextBrandID = label_brand_id.Text;
            Form_Main.SetValueForTextSearch = label_text_search.Text;
            Close();
        }

        private void ComboBox_brand_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = comboBox_brand.Text;

            // API Brand
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
                MessageBox.Show(pagesource);
            }
            
            //using (con)
            //{
            //    try
            //    {
            //        con.Close();
            //        con.Open();

            //        MySqlCommand command = new MySqlCommand("SELECT id, text_search FROM `brands` WHERE brand_name = '" + selected + "'", con);
            //        command.CommandType = CommandType.Text;
            //        MySqlDataReader reader = command.ExecuteReader();

            //        if (reader.HasRows)
            //        {
            //            while (reader.Read())
            //            {
            //                label_brand_id.Text = reader["id"].ToString();
            //                label_text_search.Text = reader["text_search"].ToString();
            //            }
            //        }

            //        con.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        con.Close();

            //        MessageBox.Show("There is a problem with the server! Please contact IT support. asd123" + ex.Message, "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        Application.Exit();
            //    }
            //    finally
            //    {
            //        con.Close();
            //    }
            //}
        }
    }
}
