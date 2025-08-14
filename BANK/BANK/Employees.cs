using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BANK
{
    public partial class Employees : Form
    {
        String empId;
        string role; 
        public Employees(string empId)
        {
            InitializeComponent();
            this.empId = empId;
            button2.Visible = false;
        }

        public Employees(string empId, string role)
        {
            InitializeComponent();
            this.empId = empId;
            this.role = role;

        }

        private void Employees_Load(object sender, EventArgs e)
        {
            LoadEmployeeData();
            PopulateComboBox();
        }

        private void LoadEmployeeData()
        {
            using (SqlConnection conn = DBConn.GetConnection())
            {
                string query = "SELECT EmployeeID, EmployeeName, Salary, Designation, HiredBy FROM [dbo].[Employee] WHERE Deleted = 0";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void PopulateComboBox()
        {
            comboFunctions.Items.Clear();
            comboFunctions.Items.Add("EmployeeID");
            comboFunctions.Items.Add("EmployeeName");
            comboFunctions.Items.Add("Salary");
            comboFunctions.Items.Add("Designation");
            comboFunctions.Items.Add("HiredBy");
            comboFunctions.SelectedIndex = 0;  
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string selectedColumn = comboFunctions.SelectedItem.ToString();
            string searchValue = txtSearch.Text;
            SearchEmployee(selectedColumn, searchValue);
        }

        private void SearchEmployee(string selectedColumn, string searchValue)
        {
            using (SqlConnection conn = DBConn.GetConnection())
            {
                string query = $"SELECT EmployeeID, EmployeeName, Salary, Designation, HiredBy FROM [dbo].[Employee] WHERE Deleted = 0 AND {selectedColumn} LIKE @SearchValue";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchValue", "%" + searchValue + "%");

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.Width = dataGridView1.Width / dataGridView1.Columns.Count;
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string employeeID = dataGridView1.SelectedRows[0].Cells["EmployeeID"].Value.ToString();
                MarkEmployeeAsDeleted(employeeID);
            }
            else
            {
                MessageBox.Show("Please select an employee to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MarkEmployeeAsDeleted(string employeeID)
        {
            using (SqlConnection conn = DBConn.GetConnection())
            {
                SqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    // Get the ApplicationID of the employee to update the JobApplications table
                    string getApplicationIdQuery = "SELECT ApplicationID FROM [dbo].[Employee] WHERE EmployeeID = @EmployeeID";
                    SqlCommand getAppIdCmd = new SqlCommand(getApplicationIdQuery, conn, transaction);
                    getAppIdCmd.Parameters.AddWithValue("@EmployeeID", employeeID);

                    int? applicationID = (int?)getAppIdCmd.ExecuteScalar();
                    if (applicationID.HasValue)
                    {
                        // Mark employee as deleted
                        string updateEmployeeQuery = "UPDATE [dbo].[Employee] SET Deleted = 1 WHERE EmployeeID = @EmployeeID";
                        SqlCommand updateEmployeeCmd = new SqlCommand(updateEmployeeQuery, conn, transaction);
                        updateEmployeeCmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                        int rowsAffected = updateEmployeeCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Mark the related job application as 'Not Hired'
                            string updateJobAppQuery = "UPDATE [dbo].[JobApplications] SET Status = 'Not Hired' WHERE ApplicationID = @ApplicationID";
                            SqlCommand updateJobAppCmd = new SqlCommand(updateJobAppQuery, conn, transaction);
                            updateJobAppCmd.Parameters.AddWithValue("@ApplicationID", applicationID.Value);
                            updateJobAppCmd.ExecuteNonQuery();

                            transaction.Commit();

                            MessageBox.Show("Employee marked as deleted and application status updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadEmployeeData();
                        }
                        else
                        {
                            transaction.Rollback();
                            MessageBox.Show("Error occurred while deleting the employee.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        MessageBox.Show("Employee not found in JobApplications.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                        transaction.Rollback();
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }
            }
        }



        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string employeeID = dataGridView1.SelectedRows[0].Cells["EmployeeID"].Value.ToString();

                ViewEmployee viewEmployeeForm = new ViewEmployee(employeeID);
                viewEmployeeForm.ShowDialog();  
            }
            else
            {
                MessageBox.Show("Please select an employee to view.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCreateNew_Click(object sender, EventArgs e)
        {
            MessageBox.Show("First Create a Job Application.",
                            "Job Application Required",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }
    }
}
