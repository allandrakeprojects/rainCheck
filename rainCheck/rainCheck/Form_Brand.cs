using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class Form_Brand : Form
    {
        MySqlConnection con = new MySqlConnection("server=mysql5018.site4now.net;user id=a3d1a6_check;password=admin12345;database=db_a3d1a6_check;persistsecurityinfo=True;SslMode=none");

        public Form_Brand(string text)
        {
            InitializeComponent();

            label_question.Text = "Select brand for " + text + " domain:";
        }

        private void Form_Brand_Load(object sender, System.EventArgs e)
        {
            using (con)
            {
                try
                {
                    con.Open();

                    MySqlDataAdapter da = new MySqlDataAdapter();

                    DataSet ds = new DataSet();

                    string sql = "SELECT brand_name FROM `brands`";

                    da.SelectCommand = new MySqlCommand(sql, con);

                    da.Fill(ds);

                    //ComboBox comboBox1 = new ComboBox();

                    comboBox_brand.DataSource = ds.Tables[0];
                    comboBox_brand.DisplayMember = "brand_name";
                    
                    con.Close();
                }
                catch (Exception)
                {
                    con.Close();

                    MessageBox.Show("There is a problem with the server! Please contact IT support.", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void Button_start_urgent_Click(object sender, EventArgs e)
        {
            Form_Main.SetValueForTextBrandID = label_brand_id.Text;
            Close();
        }

        private void ComboBox_brand_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = comboBox_brand.Text;

            using (con)
            {
                try
                {
                    con.Close();
                    con.Open();

                    MySqlCommand command = new MySqlCommand("SELECT id FROM `brands` WHERE brand_name = '" + selected + "'", con);
                    command.CommandType = CommandType.Text;
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            label_brand_id.Text = reader["id"].ToString();
                        }
                    }

                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();

                    MessageBox.Show("There is a problem with the server! Please contact IT support. asd123" + ex.Message, "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
