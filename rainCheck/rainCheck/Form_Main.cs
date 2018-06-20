using CefSharp;
using CefSharp.WinForms;
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

        public ChromiumWebBrowser chromeBrowser { get; private set; }

        //MySqlConnection con = new MySqlConnection("server=localhost;user id=root;password=;persistsecurityinfo=True;port=;database=raincheck;SslMode=none");

        public Form_Main()
        {
            InitializeComponent();

            // Design
            this.WindowState = FormWindowState.Maximized;

            DataToGridView("SELECT `domain_name` as 'Domain(s) List' FROM `domains`");
        }

        private void dataGridView_devices_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_devices.CurrentCell == null || dataGridView_devices.CurrentCell.Value == null || e.RowIndex == -1)
            {
                return;
            }
            else
            {
                // Gets a collection that contains all the rows
                DataGridViewRow row = this.dataGridView_devices.Rows[e.RowIndex];
                // Populate the textbox from specific value of the coordinates of column and row.
                string domain = dataGridView_devices.Rows[e.RowIndex].Cells[0].Value.ToString();

                InitializeChromium(domain);
            }
        }

        public void InitializeChromium(string domain)
        {
            try
            {
                if (Cef.IsInitialized == true)
                {
                    MessageBox.Show("asd");
                    MessageBox.Show(domain);


                }
                else
                {
                    MessageBox.Show("asd1");
                    MessageBox.Show(domain);

                    CefSettings settings = new CefSettings();

                    // Initialize cef with the provided settings
                    Cef.Initialize(settings);

                    // Create a browser component
                    chromeBrowser = new ChromiumWebBrowser(domain);
                    // Add it to the form and fill it to the form window.
                    panel_browser.Controls.Add(chromeBrowser);
                    chromeBrowser.Dock = DockStyle.Fill;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
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

            Cef.Shutdown();
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