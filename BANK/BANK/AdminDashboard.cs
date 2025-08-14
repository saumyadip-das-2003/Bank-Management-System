using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BANK
{
    public partial class AdminDashboard : Form
    {
        string username;
        public AdminDashboard(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            txtAdminId.Text = username;
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }


        private void comboFunctions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboFunctions.Text == "LOGOUT")
            {
                DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    WelcomePage welcomePage = new WelcomePage();
                    welcomePage.Show();
                    this.Close();
                }
            }
            else if (comboFunctions.Text == "CUSTOMERS")
            {
                LoadPanel(new Customers(username, "admin"));
            }
            else if (comboFunctions.Text == "JOB APPLICATIONS")
            {
                LoadPanel(new JobApplications(username, "admin"));
            }
            else if (comboFunctions.Text == "ACCOUNTS")
            {
                LoadPanel(new Accounts(username, "admin"));
            }
            else if (comboFunctions.Text == "EMPLOYEE")
            {
                LoadPanel(new Employees(username, "admin"));
            }
            else if (comboFunctions.Text == "TRANSACTION")
            {
                LoadPanel(new Transactions(username));
            }
            else if (comboFunctions.Text == "LOAN")
            {
                LoadPanel(new Loans(username));
            }
            else if (comboFunctions.Text == "BANK INFO")
            {
                LoadPanel(new Form1());
            }
        }

        private void LoadPanel(Form form)
        {
            displayPanel.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            displayPanel.Controls.Add(form);
            form.Show();
        }


    }
}
