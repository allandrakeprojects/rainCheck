using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace rainCheck
{
    public partial class Form_Main : Form
    {
        MySqlConnection con = new MySqlConnection("server=mysql5018.site4now.net;user id=a3d1a6_check;password=admin12345;database=db_a3d1a6_check;persistsecurityinfo=True;SslMode=none");
        //MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;persistsecurityinfo=True;port=;database=raincheck;SslMode=none");

        public Form_Main()
        {
            InitializeComponent();

            // Design
            this.WindowState = FormWindowState.Maximized;
            
            //DataToGridView("SELECT `domain_name` as 'Domain', `brandname` as 'Brand', IF(status = 'A', 'Active', 'Inactive') as 'Status', `website_type` as 'Website Type', `channel` as 'Channel', `purpose` as 'Purpose', `member` as 'Member', `remark` as 'Remark' FROM `domains`");

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
            dataGridView_devices.ClearSelection();
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
            DataToGridView("SELECT `domain_name` as 'Domain', `brandname` as 'Brand', IF(status = 'A', 'Active', 'Inactive') as 'Status', `website_type` as 'Website Type', `channel` as 'Channel', `purpose` as 'Purpose', `member` as 'Member', `remark` as 'Remark' FROM `domains` WHERE domain_name LIKE '" + textBox_search.Text + "%'");
        }

        private void GetBrands()
        {
            using (con)
            {
                try
                {
                    string selectQuery = "SELECT distinct(brand_name) FROM `brands` WHERE brand_name IS NOT NULL AND brand_name != ''";
                    con.Open();
                    MySqlCommand command = new MySqlCommand(selectQuery, con);
                    MySqlDataReader reader = command.ExecuteReader();
                    
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            comboBox_brands.Items.Add(reader.GetString("brand_name"));
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows!!!");
                    }
                }
                catch (Exception e)
                {
                    con.Close();

                    //panel_blank.Visible = true;
                    //panel_blank.BringToFront();
                    panel_top.Visible = false;
                    MessageBox.Show("There is a problem with the server! Please contact IT support. brands " + e.Message, "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    string selectQuery = "SELECT GROUP_CONCAT(member) as member FROM `domains` WHERE member IS NOT NULL OR member != ''";
                    con.Open();
                    MySqlCommand command = new MySqlCommand(selectQuery, con);
                    MySqlDataReader reader = command.ExecuteReader();
                    
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            //string[] namesArray = reader.GetString("member").Split(',');
                            //List<string> namesList = new List<string>(namesArray.Length);
                            //namesList.AddRange(namesArray);
                            //namesList.Reverse();

                            string value = reader.GetString("member");
                            string[] lines = Regex.Split(value, ",");
                            foreach (string line in lines)
                            {
                                if (!comboBox_member.Items.Contains(line))
                                {
                                    comboBox_member.Items.Add(line);

                                    string str = comboBox_member.Items[0].ToString();
                                    comboBox_member.Items.RemoveAt(0);
                                    //comboBox_member.Sorted = true;
                                    comboBox_member.Sorted = false;
                                    comboBox_member.Items.Insert(0, str);
                                }
                            }


                            //comboBox_member.Items.Add(reader.GetString("member"));
                        }
                    }
                }
                catch (Exception e)
                {
                    con.Close();

                    //panel_blank.Visible = true;
                    //panel_blank.BringToFront();
                    panel_top.Visible = false;
                    MessageBox.Show("There is a problem with the server! Please contact IT support. member " + e.Message, "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    string selectQuery = "SELECT distinct(website_type) FROM `domains` WHERE website_type IS NOT NULL AND website_type != ''";
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
                    MessageBox.Show("There is a problem with the server! Please contact IT support. website type " + e.Message, "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    string selectQuery = "SELECT distinct(channel) FROM `domains` WHERE channel IS NOT NULL AND channel != ''";
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
                    MessageBox.Show("There is a problem with the server! Please contact IT support. channel " + e.Message, "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                finally
                {
                    con.Close();
                }
            }
        }

        string brands;
        string member;
        string websitetype;
        string channel;
        string status;

        private void ComboBox_brands_SelectedIndexChanged(object sender, EventArgs e)
        {
            brands = comboBox_brands.GetItemText(comboBox_brands.SelectedItem);
            ComboBox_All(brands, member, websitetype, channel, status);
        }

        private void ComboBox_member_SelectedIndexChanged(object sender, EventArgs e)
        {
            member = comboBox_member.GetItemText(comboBox_member.SelectedItem);
            ComboBox_All(brands, member, websitetype, channel, status);
        }

        private void ComboBox_websitetype_SelectedIndexChanged(object sender, EventArgs e)
        {
            websitetype = comboBox_websitetype.GetItemText(comboBox_websitetype.SelectedItem);
            ComboBox_All(brands, member, websitetype, channel, status);
        }

        private void ComboBox_channel_SelectedIndexChanged(object sender, EventArgs e)
        {
            channel = comboBox_channel.GetItemText(comboBox_channel.SelectedItem);
            ComboBox_All(brands, member, websitetype, channel, status);
        }

        private void ComboBox_status_SelectedIndexChanged(object sender, EventArgs e)
        {
            status = comboBox_status.GetItemText(comboBox_status.SelectedItem);
            ComboBox_All(brands, member, websitetype, channel, status);
        }

        public void ComboBox_All(string brands, string member, string websitetype, string channel, string status)
        {

            //MessageBox.Show("Website Type: " + websitetype + " Channel: " + channel + " Status: " + status);
            //MessageBox.Show("Brands: " + brands);
            if (brands == "All")
            {
                brands = "";
            }

            if (member == "All") {
                member = "";
            }

            if (websitetype == "All")
            {
                websitetype = "";
            }

            if (channel == "All")
            {
                channel = "";
            }

            if (status == "All")
            {
                status = "";
            }

            if (status == "Active")
            {
                status = "A";
            }

            if (status == "Inactive")
            {
                status = "X";
            }

            DataToGridView("SELECT `domain_name` as 'Domain', `brandname` as 'Brand', IF(status = 'A', 'Active', 'Inactive') as 'Status', `website_type` as 'Website Type', `channel` as 'Channel', `purpose` as 'Purpose', `member` as 'Member', `remark` as 'Remark' FROM `domains` WHERE brandname LIKE '" + brands + "%' AND member LIKE '%" + member + "%' AND website_type LIKE '" + websitetype + "%' AND channel LIKE '" + channel + "%' AND status LIKE '" + status + "%'");
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            comboBox_brands.SelectedIndex = 0;
            comboBox_member.SelectedIndex = 0;
            comboBox_websitetype.SelectedIndex = 0;
            comboBox_channel.SelectedIndex = 0;
            comboBox_status.SelectedIndex = 0;

            string brands = "";
            string member = "";
            string websitetype = "";
            string channel = "";
            string status = "";

            DataToGridView("SELECT `domain_name` as 'Domain', `brandname` as 'Brand', IF(status = 'A', 'Active', 'Inactive') as 'Status', `website_type` as 'Website Type', `channel` as 'Channel', `purpose` as 'Purpose', `member` as 'Member', `remark` as 'Remark' FROM `domains` WHERE brandname LIKE '" + brands + "%' AND member LIKE '%" + member + "%' AND website_type LIKE '" + websitetype + "%' AND channel LIKE '" + channel + "%' AND status LIKE '" + status + "%'");  
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
                    adapter.SelectCommand = new MySqlCommand(query, con);

                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    if (table.Rows.Count == 0)
                    {
                        BindingSource source = new BindingSource();
                        source.DataSource = table;
                        
                        table.Rows.Add("No data available in table");

                        dataGridView_devices.DataSource = source;

                        con.Close();

                        dataGridView_devices.ClearSelection();

                        dataGridView_devices.CellBorderStyle = DataGridViewCellBorderStyle.None;
                        dataGridView_devices.DefaultCellStyle.SelectionBackColor = dataGridView_devices.DefaultCellStyle.BackColor;
                        dataGridView_devices.DefaultCellStyle.SelectionForeColor = dataGridView_devices.DefaultCellStyle.ForeColor;
                    }
                    else
                    {
                        BindingSource source = new BindingSource();
                        source.DataSource = table;

                        dataGridView_devices.DataSource = source;

                        con.Close();

                        dataGridView_devices.ClearSelection();

                        dataGridView_devices.CellBorderStyle = DataGridViewCellBorderStyle.Single;

                        string hex = "#438eb9";
                        Color color = ColorTranslator.FromHtml(hex);
                        dataGridView_devices.DefaultCellStyle.SelectionBackColor = color;
                        dataGridView_devices.DefaultCellStyle.SelectionForeColor = Color.White;
                    }

                }
                catch (Exception e)
                {
                    con.Close();

                    //panel_blank.Visible = true;
                    //panel_blank.BringToFront();
                    panel_top.Visible = false;
                    MessageBox.Show("There is a problem with the server! Please contact IT support. main " + e.Message, "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
