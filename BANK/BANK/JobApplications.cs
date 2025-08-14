using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BANK
{
    public partial class JobApplications : Form
    {
        private SqlConnection conn;
        string Username;
        string role; 

        public JobApplications(string username)
        {
            InitializeComponent();
            Username = username;

            button2.Visible = false;
        }

        public JobApplications(string username, string role)
        {
            InitializeComponent();
            Username = username;
        }
        private void JobApplications_Load(object sender, EventArgs e)
        {
            conn = DBConn.GetConnection();
            PopulateColumnNames();
            LoadJobApplications();
        }

        private void PopulateColumnNames()
        {
            comboColumns.Items.Clear();
            comboColumns.Items.AddRange(new string[] { "Status", "FullName", "Gender", "DateOfBirth", "MobileNo", "Address", "JobExperience", "PreferredDesignation", "Email", "NID" });
            comboColumns.SelectedIndex = 0;
        }

        private void LoadJobApplications(string filterColumn = "", string filterValue = "")
        {
            string query = "SELECT ApplicationID, Status, FullName, Gender, DateOfBirth, MobileNo, Address, JobExperience, PreferredDesignation, Email, NID " +
                           "FROM JobApplications WHERE Deleted IS NULL";

            if (!string.IsNullOrEmpty(filterColumn) && !string.IsNullOrEmpty(filterValue))
                query += $" AND {filterColumn} LIKE @FilterValue";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (!string.IsNullOrEmpty(filterValue))
                    cmd.Parameters.AddWithValue("@FilterValue", "%" + filterValue + "%");

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.Columns["Status"].DisplayIndex = 0;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (comboColumns.SelectedItem != null)
            {
                LoadJobApplications(comboColumns.SelectedItem.ToString(), txtSearch.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var dialogResult = MessageBox.Show("Are you sure you want to delete this job application?", "Confirm Deletion", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    int applicationID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ApplicationID"].Value);

                    using (SqlCommand cmdDeleteEmployee = new SqlCommand("UPDATE Employee SET Deleted = 1 WHERE ApplicationID = @ApplicationID", conn))
                    {
                        cmdDeleteEmployee.Parameters.AddWithValue("@ApplicationID", applicationID);
                        conn.Open();
                        cmdDeleteEmployee.ExecuteNonQuery();
                        conn.Close();
                    }

                    using (SqlCommand cmdDeleteApplication = new SqlCommand("UPDATE JobApplications SET Deleted = 1 WHERE ApplicationID = @ApplicationID", conn))
                    {
                        cmdDeleteApplication.Parameters.AddWithValue("@ApplicationID", applicationID);
                        conn.Open();
                        cmdDeleteApplication.ExecuteNonQuery();
                        conn.Close();
                    }

                    LoadJobApplications(); 
                }
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int applicationID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ApplicationID"].Value);
                ViewApplications viewAppForm = new ViewApplications(applicationID, Username);
                viewAppForm.ShowDialog();
                LoadJobApplications();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ApplyForJob ap = new ApplyForJob();
            ap.ShowDialog();
            LoadJobApplications();
        }
    }
}
