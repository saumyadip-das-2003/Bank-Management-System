using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BANK
{
    public partial class Customers : Form
    {
        private SqlConnection conn;
        string username;
        string role; 

        public Customers(string username)
        {
            InitializeComponent();
            this.username = username;
            button2.Visible = false;
        }

        public Customers(string username, string role)
        {
            InitializeComponent();
            this.username = username;
            this.role = role;

            
        }

        private void Customers_Load(object sender, EventArgs e)
        {
            conn = DBConn.GetConnection();
            PopulateColumnNames();
            LoadCustomerData();
        }

        private void PopulateColumnNames()
        {
            comboColumns.Items.Clear();
            comboColumns.Items.AddRange(new string[] { "AccountCount", "FullName", "Gender", "DateOfBirth", "MobileNo", "Address", "Occupation", "Salary", "Age", "NID", "Email", "InitialDeposit" });
            comboColumns.SelectedIndex = 0;
        }

        private void LoadCustomerData(string filterColumn = "", string filterValue = "")
        {
            string query = "SELECT CustomerID, AccountCount, FullName, Gender, DateOfBirth, MobileNo, Address, Occupation, Salary, Age, NID, Email, InitialDeposit " +
                           "FROM Customers WHERE Deleted IS NULL";

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
                dataGridView1.Columns["AccountCount"].DisplayIndex = 0;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (comboColumns.SelectedItem != null)
            {
                LoadCustomerData(comboColumns.SelectedItem.ToString(), txtSearch.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var dialogResult = MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Deletion", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    int customerID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["CustomerID"].Value);

                    using (SqlCommand cmdDeleteAccounts = new SqlCommand("UPDATE Accounts SET Deleted = 1 WHERE CustomerID = @CustomerID", conn))
                    {
                        cmdDeleteAccounts.Parameters.AddWithValue("@CustomerID", customerID);
                        conn.Open();
                        cmdDeleteAccounts.ExecuteNonQuery();
                        conn.Close();
                    }

                    using (SqlCommand cmdDeleteCustomer = new SqlCommand("UPDATE Customers SET Deleted = 1 WHERE CustomerID = @CustomerID", conn))
                    {
                        cmdDeleteCustomer.Parameters.AddWithValue("@CustomerID", customerID);
                        conn.Open();
                        cmdDeleteCustomer.ExecuteNonQuery();
                        conn.Close();
                    }

                    LoadCustomerData(); 
                }
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int customerID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["CustomerID"].Value);
                ViewCustomers viewCustomerForm = new ViewCustomers(customerID, username);
                viewCustomerForm.ShowDialog();
                LoadCustomerData(); 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CustomerReg cg = new CustomerReg();
            cg.ShowDialog();
            LoadCustomerData();
        }
    }
}
