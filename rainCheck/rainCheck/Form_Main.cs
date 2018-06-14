using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class Form_Main : Form
    {
        MySqlConnection con = new MySqlConnection("server=mysql5018.site4now.net;user id=a3d1a6_check;password=admin12345;database=db_a3d1a6_check;persistsecurityinfo=True;SslMode=none");
        
        public Form_Main()
        {
            InitializeComponent();

            // Design
            this.WindowState = FormWindowState.Maximized;

            DataToGridView("SELECT `domain_name` as 'Domain', `brandname` as 'Brand', `status` as 'Status', `website_type` as 'Website Type', `channel` as 'Channel', `purpose` as 'Purpose', `member` as 'Member', `remark` as 'Remark' FROM `domains`");
            dataGridView_devices.ClearSelection();

            comboBox_brands.SelectedIndex = 0;
            comboBox_member.SelectedIndex = 0;
            comboBox_websitetype.SelectedIndex = 0;
            comboBox_channel.SelectedIndex = 0;
            comboBox_status.SelectedIndex = 0;

            GetBrands();
            GetMember();
            GetWebsiteType();
            GetChannel();
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            
        }

        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            //DialogResult dr = MessageBox.Show("Are you sure you want to exit the program?", "rainCheck", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (dr == DialogResult.No)
            //{
            //    e.Cancel = true;
            //}
        }


        private void Label_bordersearch_Paint(object sender, PaintEventArgs e)
        { 
            ControlPaint.DrawBorder(e.Graphics, label_bordersearch.DisplayRectangle, Color.Gray, ButtonBorderStyle.Solid);
        }

        private void TextBox_search_TextChanged(object sender, EventArgs e)
        {
            DataToGridView("SELECT `domain_name` as 'Domain', `brandname` as 'Brand', `status` as 'Status', `website_type` as 'Website Type', `channel` as 'Channel', `purpose` as 'Purpose', `member` as 'Member', `remark` as 'Remark' FROM `domains` WHERE domain_name LIKE '" + textBox_search.Text + "%'");
        }

        private void GetBrands()
        {
            using (con)
            {
                try
                {
                    string selectQuery = "SELECT distinct(brand_name) FROM `brands` WHERE brand_name IS NOT NULL";
                    con.Open();
                    MySqlCommand command = new MySqlCommand(selectQuery, con);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox_brands.Items.Add(reader.GetString("brand_name"));
                    }
                }
                catch (Exception e)
                {
                    con.Close();

                    //panel_blank.Visible = true;
                    //panel_blank.BringToFront();
                    panel_top.Visible = false;
                    MessageBox.Show("There is a problem with the server! Please contact IT support." + e.Message, "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void GetMember()
        {
            using (con)
            {
                try
                {
                    string selectQuery = "SELECT distinct(member) FROM `domains` WHERE member IS NOT NULL";
                    con.Open();
                    MySqlCommand command = new MySqlCommand(selectQuery, con);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox_member.Items.Add(reader.GetString("member"));
                    }
                }
                catch (Exception e)
                {
                    con.Close();

                    //panel_blank.Visible = true;
                    //panel_blank.BringToFront();
                    panel_top.Visible = false;
                    MessageBox.Show("There is a problem with the server! Please contact IT support." + e.Message, "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void GetWebsiteType()
        {
            using (con)
            {
                try
                {
                    string selectQuery = "SELECT distinct(website_type) FROM `domains` WHERE website_type IS NOT NULL";
                    con.Open();
                    MySqlCommand command = new MySqlCommand(selectQuery, con);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox_websitetype.Items.Add(reader.GetString("website_type"));
                    }
                }
                catch (Exception e)
                {
                    con.Close();

                    //panel_blank.Visible = true;
                    //panel_blank.BringToFront();
                    panel_top.Visible = false;
                    MessageBox.Show("There is a problem with the server! Please contact IT support." + e.Message, "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void GetChannel()
        {
            using (con)
            {
                try
                {
                    string selectQuery = "SELECT distinct(channel) FROM `domains` WHERE channel IS NOT NULL";
                    con.Open();
                    MySqlCommand command = new MySqlCommand(selectQuery, con);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox_channel.Items.Add(reader.GetString("channel"));
                    }
                }
                catch (Exception e)
                {
                    con.Close();

                    //panel_blank.Visible = true;
                    //panel_blank.BringToFront();
                    panel_top.Visible = false;
                    MessageBox.Show("There is a problem with the server! Please contact IT support." + e.Message, "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                finally
                {
                    con.Close();
                }
            }
        }

        // Show domains
        private void DataToGridView(string query)
        {
            using (con)
            {
                try
                {
                    con.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    string sqlSelectAll = query;
                    adapter.SelectCommand = new MySqlCommand(sqlSelectAll, con);

                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    BindingSource source = new BindingSource();
                    source.DataSource = table;

                    dataGridView_devices.DataSource = source;

                    con.Close();

                    dataGridView_devices.ClearSelection();
                }
                catch (Exception e)
                {
                    con.Close();

                    //panel_blank.Visible = true;
                    //panel_blank.BringToFront();
                    panel_top.Visible = false;
                    MessageBox.Show("There is a problem with the server! Please contact IT support." + e.Message, "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
