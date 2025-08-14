using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BANK
{
    public partial class Accounts : Form
    {
        string username;
        string role; 
        public Accounts(string username)
        {            
            InitializeComponent();
            this.username = username;
            btnDelete.Visible = false;            
        }

        public Accounts(string username, string role)
        {
            InitializeComponent();
            this.username = username;
            this.role = role;

            btnDelete.Visible = false;
            btnCreateNew.Visible = false;
            btnLoan.Visible = false;
            btnTransaction.Visible = false;

            if (role == "admin")
            {
                btnDelete.Visible = true;
                btnCreateNew.Visible = true;
                btnLoan.Visible = true;
                btnTransaction.Visible = true;
            }
            else if (role == "Manager")
            {
                btnCreateNew.Visible = true;
                btnLoan.Visible = true;
                btnTransaction.Visible = true;
            }
            else if (role == "PR")
            {
                btnCreateNew.Visible = true;
            }
            else if (role == "Loan Incharge")
            {
                btnLoan.Visible = true;
            }
            else if (role == "Cashier")
            {
                btnTransaction.Visible = true;
            }
        }


        private void Accounts_Load(object sender, EventArgs e)
        {
            LoadAccounts();
            PopulateComboFunctions();
        }

        private void PopulateComboFunctions()
        {
            comboFunctions.Items.Clear();
            comboFunctions.Items.AddRange(new string[] { "AccountNo", "AccountName", "Balance", "CustomerID", "CreatedBy" });
            comboFunctions.SelectedIndex = 0;
        }

        private void LoadAccounts()
        {
            using (SqlConnection con = DBConn.GetConnection())
            {
                string query = "SELECT AccountNo, AccountName, Balance, CustomerID, CreatedBy FROM Accounts WHERE Deleted = 0";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (comboFunctions.SelectedItem == null || string.IsNullOrWhiteSpace(txtSearch.Text))
                return;

            using (SqlConnection con = DBConn.GetConnection())
            {
                string column = comboFunctions.SelectedItem.ToString();
                string query = $"SELECT AccountNo, AccountName, Balance, CustomerID, CreatedBy FROM Accounts WHERE Deleted = 0 AND {column} LIKE @Search";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@Search", "%" + txtSearch.Text + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string accountNo = dataGridView1.SelectedRows[0].Cells["AccountNo"].Value.ToString();
                string customerID = dataGridView1.SelectedRows[0].Cells["CustomerID"].Value.ToString(); 

                using (SqlConnection con = DBConn.GetConnection())
                {
                    string query = "UPDATE Accounts SET Deleted = 1 WHERE AccountNo = @AccountNo";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@AccountNo", accountNo);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    string updateCustomerQuery = "UPDATE Customers SET AccountCount = AccountCount - 1 WHERE CustomerID = @CustomerID";
                    SqlCommand updateCustomerCmd = new SqlCommand(updateCustomerQuery, con);
                    updateCustomerCmd.Parameters.AddWithValue("@CustomerID", customerID);
                    con.Open();
                    updateCustomerCmd.ExecuteNonQuery();
                    con.Close();
                }

                LoadAccounts();
            }
        }


        private void btnCreateNew_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Go to Customer Page for creating account.",
                            "Customer Page Required",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string accountNo = dataGridView1.SelectedRows[0].Cells["AccountNo"].Value.ToString();
                TransactionOperations transactionForm = new TransactionOperations(accountNo, username);
                transactionForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select an account for transactions.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string accountNo = dataGridView1.SelectedRows[0].Cells["AccountNo"].Value.ToString();
                ViewAccount viewAccountForm = new ViewAccount(accountNo);
                viewAccountForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select an account to view.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLoan_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string accountNo = dataGridView1.SelectedRows[0].Cells["AccountNo"].Value.ToString();
                LoanOperations l = new LoanOperations(accountNo, username);
                l.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select an account to view.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadAccounts(); 
        }
    }
}
