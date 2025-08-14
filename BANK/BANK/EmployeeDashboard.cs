using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BANK
{
    public partial class EmployeeDashboard : Form
    {
        string EmpID;
        string designation;

        public EmployeeDashboard(string empID, string designation)
        {
            InitializeComponent();
            EmpID = empID;
            this.designation = designation;
            LoadFunctions();
        }

        private void LoadFunctions()
        {
            comboFunctions.Items.Clear();
                       

            switch (designation.ToUpper())
            {
                case "CASHIER":
                    comboFunctions.Items.AddRange(new string[] { "ACCOUNT", "TRANSACTION", "PROFILE" });
                    break;
                case "HR":
                    comboFunctions.Items.AddRange(new string[] { "JOB APPLICATIONS", "EMPLOYEE", "PROFILE" });
                    break;
                case "PR":
                    comboFunctions.Items.AddRange(new string[] { "CUSTOMER", "ACCOUNT", "PROFILE" });
                    break;
                case "LOAN INCHARGE":
                    comboFunctions.Items.AddRange(new string[] { "LOANS", "ACCOUNT", "PROFILE" });
                    break;
                case "MANAGER":
                    comboFunctions.Items.AddRange(new string[] { "ACCOUNT", "CUSTOMER", "TRANSACTION" , "JOB APPLICATIONS", "LOANS", "EMPLOYEE","BANK INFO", "PROFILE" });
                    break;
                default:
                    MessageBox.Show("Invalid designation assigned!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
            comboFunctions.Items.Add("LOGOUT");
            comboFunctions.SelectedIndex = 0;
        }

        private void comboFunctions_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboFunctions.Text)
            {
                case "LOGOUT":
                    Logout();
                    break;
                case "PROFILE":
                    LoadPanel(new EmployeeProfile(EmpID));
                    break;
                case "CUSTOMER":
                    LoadPanel(new Customers(EmpID));
                    break;
                case "EMPLOYEE":
                    LoadPanel(new Employees(EmpID));
                    break;
                case "ACCOUNT":
                    LoadPanel(new Accounts(EmpID, designation));
                    break;
                case "JOB APPLICATIONS":
                    LoadPanel(new JobApplications(EmpID));
                    break;
                case "TRANSACTION":
                    LoadPanel(new Transactions(EmpID));
                    break;
                case "LOANS":
                    LoadPanel(new Loans(EmpID));
                    break;
                case "BANK INFO":
                    LoadPanel(new Form1());
                    break;
            }
        }

        private void Logout()
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                WelcomePage welcomePage = new WelcomePage();
                welcomePage.Show();
                this.Close();
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

        private void EmployeeDashboard_Load(object sender, EventArgs e)
        {
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            LoadEmployeeDetails();
        }

        private void LoadEmployeeDetails()
        {
            using (SqlConnection conn = DBConn.GetConnection())
            {
                string query = "SELECT EmployeeName, Designation FROM Employee WHERE EmployeeID = @EmpID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EmpID", EmpID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtEmpId.Text = EmpID;
                    txtEmpName.Text = reader["EmployeeName"].ToString();
                    txtDesignation.Text = reader["Designation"].ToString();
                }
                reader.Close();
            }
        }
    }
}
