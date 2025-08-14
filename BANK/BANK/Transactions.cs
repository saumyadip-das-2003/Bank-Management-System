using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BANK
{
    public partial class Transactions : Form
    {
        string username;

        public Transactions(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void Transactions_Load(object sender, EventArgs e)
        {
            LoadTransactions();
            PopulateComboColumns();
        }

        private void LoadTransactions()
        {
            using (SqlConnection con = DBConn.GetConnection())
            {
                string query = "SELECT * FROM [Transaction]";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void PopulateComboColumns()
        {
            comboColumns.Items.Clear();
            comboColumns.Items.AddRange(new string[] { "TransactionID", "TransactionType", "AccountID", "TransferAccount", "Status", "ApprovedBy", "Amount" });
            comboColumns.SelectedIndex = 0;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (comboColumns.SelectedItem == null || string.IsNullOrWhiteSpace(txtSearch.Text))
                return;

            using (SqlConnection con = DBConn.GetConnection())
            {
                string column = comboColumns.SelectedItem.ToString();
                string query = $"SELECT * FROM [Transaction] WHERE {column} LIKE @Search";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@Search", "%" + txtSearch.Text + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void btnProceed_Click(object sender, EventArgs e)
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

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string transactionID = dataGridView1.SelectedRows[0].Cells["TransactionID"].Value.ToString();
                string transactionType = dataGridView1.SelectedRows[0].Cells["TransactionType"].Value.ToString();
                string accountID = dataGridView1.SelectedRows[0].Cells["AccountID"].Value.ToString();
                string transferAccount = dataGridView1.SelectedRows[0].Cells["TransferAccount"].Value?.ToString();
                decimal amount = Convert.ToDecimal(dataGridView1.SelectedRows[0].Cells["Amount"].Value);
                string status = dataGridView1.SelectedRows[0].Cells["Status"].Value.ToString();

                // Check if the transaction is already approved
                if (status == "Approved")
                {
                    MessageBox.Show("This transaction has already been approved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection con = DBConn.GetConnection())
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    try
                    {
                        // Handle different transaction types
                        if (transactionType == "Withdraw")
                        {
                            string balanceCheckQuery = "SELECT Balance FROM Accounts WHERE AccountNo = @AccountNo";
                            SqlCommand balanceCmd = new SqlCommand(balanceCheckQuery, con, transaction);
                            balanceCmd.Parameters.AddWithValue("@AccountNo", accountID);
                            decimal currentBalance = Convert.ToDecimal(balanceCmd.ExecuteScalar());

                            if (currentBalance < amount)
                            {
                                MessageBox.Show("Insufficient balance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                transaction.Rollback();
                                return;
                            }

                            string updateBalance = "UPDATE Accounts SET Balance = Balance - @Amount WHERE AccountNo = @AccountNo";
                            SqlCommand updateCmd = new SqlCommand(updateBalance, con, transaction);
                            updateCmd.Parameters.AddWithValue("@Amount", amount);
                            updateCmd.Parameters.AddWithValue("@AccountNo", accountID);
                            updateCmd.ExecuteNonQuery();
                        }
                        else if (transactionType == "Deposit")
                        {
                            string updateBalance = "UPDATE Accounts SET Balance = Balance + @Amount WHERE AccountNo = @AccountNo";
                            SqlCommand updateCmd = new SqlCommand(updateBalance, con, transaction);
                            updateCmd.Parameters.AddWithValue("@Amount", amount);
                            updateCmd.Parameters.AddWithValue("@AccountNo", accountID);
                            updateCmd.ExecuteNonQuery();
                        }
                        else if (transactionType == "Transfer")
                        {
                            if (string.IsNullOrEmpty(transferAccount))
                            {
                                MessageBox.Show("Invalid transfer account.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                transaction.Rollback();
                                return;
                            }

                            string balanceCheckQuery = "SELECT Balance FROM Accounts WHERE AccountNo = @AccountNo";
                            SqlCommand balanceCmd = new SqlCommand(balanceCheckQuery, con, transaction);
                            balanceCmd.Parameters.AddWithValue("@AccountNo", accountID);
                            decimal currentBalance = Convert.ToDecimal(balanceCmd.ExecuteScalar());

                            if (currentBalance < amount)
                            {
                                MessageBox.Show("Insufficient balance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                transaction.Rollback();
                                return;
                            }

                            // Deduct from source account
                            string deductAmount = "UPDATE Accounts SET Balance = Balance - @Amount WHERE AccountNo = @AccountNo";
                            SqlCommand deductCmd = new SqlCommand(deductAmount, con, transaction);
                            deductCmd.Parameters.AddWithValue("@Amount", amount);
                            deductCmd.Parameters.AddWithValue("@AccountNo", accountID);
                            deductCmd.ExecuteNonQuery();

                            // Add to destination account
                            string addAmount = "UPDATE Accounts SET Balance = Balance + @Amount WHERE AccountNo = @TransferAccount";
                            SqlCommand addCmd = new SqlCommand(addAmount, con, transaction);
                            addCmd.Parameters.AddWithValue("@Amount", amount);
                            addCmd.Parameters.AddWithValue("@TransferAccount", transferAccount);
                            addCmd.ExecuteNonQuery();
                        }

                        // Update the transaction status to 'Approved'
                        string updateTransaction = "UPDATE [Transaction] SET Status = 'Approved', ApprovedBy = @ApprovedBy WHERE TransactionID = @TransactionID";
                        SqlCommand updateTransCmd = new SqlCommand(updateTransaction, con, transaction);
                        updateTransCmd.Parameters.AddWithValue("@ApprovedBy", username);
                        updateTransCmd.Parameters.AddWithValue("@TransactionID", transactionID);
                        updateTransCmd.ExecuteNonQuery();

                        transaction.Commit();
                        MessageBox.Show("Transaction approved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadTransactions();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error: " + ex.Message, "Transaction Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string transactionID = dataGridView1.SelectedRows[0].Cells["TransactionID"].Value.ToString();
                string status = dataGridView1.SelectedRows[0].Cells["Status"].Value.ToString();

                if (status == "Approved")
                {
                    MessageBox.Show("Approved transactions cannot be canceled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection con = DBConn.GetConnection())
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    try
                    {
                        string cancelQuery = "UPDATE [Transaction] SET Status = 'Canceled', ApprovedBy = @ApprovedBy WHERE TransactionID = @TransactionID";
                        SqlCommand cmd = new SqlCommand(cancelQuery, con, transaction);
                        cmd.Parameters.AddWithValue("@ApprovedBy", username);
                        cmd.Parameters.AddWithValue("@TransactionID", transactionID);
                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                        MessageBox.Show("Transaction canceled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadTransactions();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error: " + ex.Message, "Transaction Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadTransactions();
        }
    }
}