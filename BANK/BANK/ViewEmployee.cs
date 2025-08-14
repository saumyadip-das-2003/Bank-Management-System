using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BANK
{
    public partial class ViewEmployee : Form
    {
        private string EmpID;

        // Constructor that receives the Employee ID to load the data
        public ViewEmployee(string empID)
        {
            InitializeComponent();
            EmpID = empID;
        }

        private void ViewEmployee_Load(object sender, EventArgs e)
        {
            // Initially hide the Save button and disable the input fields
            btnSave.Visible = false;
            txtDesignation.Enabled = false;
            txtEmpID.Enabled = false;
            txtEmpName.Enabled = false;
            txtPass.Enabled = false;
            txtSalary.Enabled = false;

            // Load data for the selected employee
            LoadEmployeeData();
        }

        // Method to load employee data using the Employee ID
        private void LoadEmployeeData()
        {
            using (SqlConnection conn = DBConn.GetConnection())
            {
                string query = "SELECT * FROM [dbo].[Employee] WHERE EmployeeID = @EmployeeID AND Deleted = 0";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@EmployeeID", EmpID); 
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtEmpID.Text = row["EmployeeID"].ToString();
                    txtEmpName.Text = row["EmployeeName"].ToString();
                    txtDesignation.Text = row["Designation"].ToString();
                    txtPass.Text = row["Password"].ToString();
                    txtSalary.Text = row["Salary"].ToString();
                }
                else
                {
                    MessageBox.Show("Employee not found.");
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            txtDesignation.Enabled = true;
            txtEmpName.Enabled = true;
            txtPass.Enabled = true;
            txtSalary.Enabled = true;

            btnSave.Visible = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmpID.Text) || string.IsNullOrEmpty(txtEmpName.Text) ||
                string.IsNullOrEmpty(txtDesignation.Text) || string.IsNullOrEmpty(txtSalary.Text) ||
                string.IsNullOrEmpty(txtPass.Text))
            {
                MessageBox.Show("Please fill in all the fields.");
                return;
            }

            using (SqlConnection conn = DBConn.GetConnection()) 
            {
                string query = "UPDATE [dbo].[Employee] SET EmployeeName = @EmployeeName, Designation = @Designation, " +
                               "Salary = @Salary, Password = @Password WHERE EmployeeID = @EmployeeID AND Deleted = 0";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EmployeeID", txtEmpID.Text);
                cmd.Parameters.AddWithValue("@EmployeeName", txtEmpName.Text);
                cmd.Parameters.AddWithValue("@Designation", txtDesignation.Text);
                cmd.Parameters.AddWithValue("@Salary", Convert.ToDecimal(txtSalary.Text));
                cmd.Parameters.AddWithValue("@Password", txtPass.Text);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Employee details updated successfully.");
                    LoadEmployeeData(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating employee details: " + ex.Message);
                }
            }

            txtDesignation.Enabled = false;
            txtEmpName.Enabled = false;
            txtPass.Enabled = false;
            txtSalary.Enabled = false;

            btnSave.Visible = false;
        }
    }
}
