using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BANK
{
    public partial class CustomerTransactions : Form
    {
        private string ACNO;

        public CustomerTransactions(string aCNO)
        {
            InitializeComponent();
            ACNO = aCNO;
        }

        private void CustomerTransactions_Load_1(object sender, EventArgs e)
        {
            LoadTransactions();   
            comboAccountsToTransfer.Visible = false;
            lblTo.Visible = false;
        }

        private void LoadTransactions()
        {
            string query = @"
                SELECT TransactionID, TransactionType, Amount, TransferAccount, Status, CreatedAt 
                FROM dbo.[Transaction] 
                WHERE AccountID = @AccountID
                ORDER BY CreatedAt DESC";

            using (SqlConnection connection = DBConn.GetConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AccountID", ACNO);

                try
                {
                    connection.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable transactionTable = new DataTable();
                    dataAdapter.Fill(transactionTable);
                    dataGridView1.DataSource = transactionTable;

                    PopulateColumnComboBox();
                    PopulateAccountComboBox();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadTransactions(); 
            txtSearch.Text = null;
            comboColumns.SelectedIndex = 0;
            dateTo.Value = DateTime.Now;    
            dateFrom.Value = DateTime.Now;  
        }

        private bool IsSufficientBalance(string accountNo, decimal amount)
        {
            string query = "SELECT Balance FROM dbo.Accounts WHERE AccountNo = @AccountNo AND Deleted = 0";
            using (SqlConnection connection = DBConn.GetConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AccountNo", accountNo);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    return result != null && Convert.ToDecimal(result) >= amount;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return false;
                }
            }
        }

        private bool IsAccountExist(string accountNo)
        {
            string query = "SELECT COUNT(*) FROM dbo.Accounts WHERE AccountNo = @AccountNo AND Deleted = 0";
            using (SqlConnection connection = DBConn.GetConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AccountNo", accountNo);

                try
                {
                    connection.Open();
                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return false;
                }
            }
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid amount.");
                return;
            }

            string operation = comboOperation.SelectedItem.ToString();
            string transferAccount = comboAccountsToTransfer.SelectedItem?.ToString();
            string status = "Pending";

            txtAmount.Clear();
            comboOperation.SelectedIndex = 0;
            comboAccountsToTransfer.Text = null;

            if ((operation == "Withdraw" || operation == "Transfer") && !IsSufficientBalance(ACNO, amount))
            {
                MessageBox.Show("Insufficient balance for this operation.");
                return;
            }

            if (operation == "Transfer")
            {
                if (string.IsNullOrEmpty(transferAccount))
                {
                    MessageBox.Show("Please select a valid transfer account.");
                    return;
                }
                if (!IsAccountExist(transferAccount))
                {
                    MessageBox.Show("The target transfer account does not exist or is inactive.");
                    return;
                }
            }

            string query = @"
                INSERT INTO dbo.[Transaction] (TransactionType, AccountID, TransferAccount, Status, Amount) 
                VALUES (@TransactionType, @AccountID, @TransferAccount, @Status, @Amount)";

            using (SqlConnection connection = DBConn.GetConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TransactionType", operation);
                command.Parameters.AddWithValue("@AccountID", ACNO);
                command.Parameters.AddWithValue("@TransferAccount", transferAccount ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Status", status);
                command.Parameters.AddWithValue("@Amount", amount);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Transaction submitted successfully.");
                    LoadTransactions(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                if (this.Parent is CustomerDashboard dashboard)
                {
                    dashboard.LoadAccountData();
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (comboColumns.SelectedItem == null || string.IsNullOrEmpty(txtSearch.Text)) return;
            string column = comboColumns.SelectedItem.ToString();
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"[{column}] LIKE '%{txtSearch.Text.Trim()}%'";
        }

        private void PopulateColumnComboBox()
        {
            comboColumns.Items.Clear();
            if (dataGridView1.DataSource == null) return;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
                comboColumns.Items.Add(column.Name);
        }

        private void PopulateAccountComboBox()
        {
            comboAccountsToTransfer.Items.Clear();
            string query = "SELECT AccountNo FROM dbo.Accounts WHERE AccountNo <> @CurrentAccount AND Deleted = 0";
            using (SqlConnection connection = DBConn.GetConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CurrentAccount", ACNO);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        comboAccountsToTransfer.Items.Add(reader["AccountNo"].ToString());
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void comboOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isTransfer = comboOperation.SelectedItem.ToString() == "Transfer";
            comboAccountsToTransfer.Visible = isTransfer;
            lblTo.Visible = isTransfer;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = DBConn.GetConnection())
            {
                string query = "SELECT * FROM [Transaction] WHERE CreatedAt BETWEEN @DateFrom AND @DateTo";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@DateFrom", dateFrom.Value.Date);
                da.SelectCommand.Parameters.AddWithValue("@DateTo", dateTo.Value.Date.AddDays(1).AddSeconds(-1));
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
    }
}
