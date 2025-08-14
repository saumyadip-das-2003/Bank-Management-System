using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BANK
{
    public partial class TransactionOperations : Form
    {
        string acNo;
        string username;

        public TransactionOperations(string accountNo, string username)
        {
            InitializeComponent();
            this.acNo = accountNo;
            this.username = username;
        }

        private void TransactionOperations_Load(object sender, EventArgs e)
        {
            lblTo.Visible = false;
            comboAccountsToTransfer.Visible = false;
            comboOperation.Items.AddRange(new string[] { "Deposit", "Withdraw", "Transfer" });
            comboOperation.SelectedIndex = 0;
            LoadTransferAccounts();
        }

        private void comboOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboOperation.SelectedItem.ToString() == "Transfer")
            {
                lblTo.Visible = true;
                comboAccountsToTransfer.Visible = true;
            }
            else
            {
                lblTo.Visible = false;
                comboAccountsToTransfer.Visible = false;
            }
        }

        private void LoadTransferAccounts()
        {
            using (SqlConnection con = DBConn.GetConnection())
            {
                string query = "SELECT AccountNo FROM Accounts WHERE Deleted = 0 AND AccountNo <> @AccountNo";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@AccountNo", acNo);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboAccountsToTransfer.Items.Add(reader["AccountNo"].ToString());
                }
                con.Close();
            }
            if (comboAccountsToTransfer.Items.Count > 0)
                comboAccountsToTransfer.SelectedIndex = 0;
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAmount.Text) || !decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid amount.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string operation = comboOperation.SelectedItem.ToString();

            if (operation == "Deposit")
                Deposit(amount);
            else if (operation == "Withdraw")
                Withdraw(amount);
            else if (operation == "Transfer")
                Transfer(amount);
        }

        private void Deposit(decimal amount)
        {
            using (SqlConnection con = DBConn.GetConnection())
            {
                string query = "UPDATE Accounts SET Balance = Balance + @Amount WHERE AccountNo = @AccountNo AND Deleted = 0";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Amount", amount);
                cmd.Parameters.AddWithValue("@AccountNo", acNo);
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    LogTransaction("Deposit", amount);
                    MessageBox.Show("Deposit successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Account not found or invalid operation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                con.Close();
            }
        }

        private void Withdraw(decimal amount)
        {
            using (SqlConnection con = DBConn.GetConnection())
            {
                // Check if account has sufficient balance
                string balanceQuery = "SELECT Balance FROM Accounts WHERE AccountNo = @AccountNo AND Deleted = 0";
                SqlCommand balanceCmd = new SqlCommand(balanceQuery, con);
                balanceCmd.Parameters.AddWithValue("@AccountNo", acNo);
                con.Open();
                decimal currentBalance = (decimal)balanceCmd.ExecuteScalar();
                con.Close();

                if (currentBalance >= amount)
                {
                    string query = "UPDATE Accounts SET Balance = Balance - @Amount WHERE AccountNo = @AccountNo AND Deleted = 0";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.Parameters.AddWithValue("@AccountNo", acNo);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        LogTransaction("Withdraw", amount);
                        MessageBox.Show("Withdrawal successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Account not found or invalid operation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    con.Close();
                }
                else
                {
                    MessageBox.Show("Insufficient balance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Transfer(decimal amount)
        {
            string toAccount = comboAccountsToTransfer.SelectedItem.ToString();

            using (SqlConnection con = DBConn.GetConnection())
            {
                // Check if source account has sufficient balance
                string balanceQuery = "SELECT Balance FROM Accounts WHERE AccountNo = @AccountNo AND Deleted = 0";
                SqlCommand balanceCmd = new SqlCommand(balanceQuery, con);
                balanceCmd.Parameters.AddWithValue("@AccountNo", acNo);
                con.Open();
                decimal currentBalance = (decimal)balanceCmd.ExecuteScalar();
                con.Close();

                if (currentBalance >= amount)
                {
                    // Check if the destination account exists
                    string checkToAccountQuery = "SELECT COUNT(*) FROM Accounts WHERE AccountNo = @ToAccount AND Deleted = 0";
                    SqlCommand checkToAccountCmd = new SqlCommand(checkToAccountQuery, con);
                    checkToAccountCmd.Parameters.AddWithValue("@ToAccount", toAccount);
                    con.Open();
                    int accountExists = (int)checkToAccountCmd.ExecuteScalar();
                    con.Close();

                    if (accountExists > 0)
                    {
                        // Update source account balance
                        string debitQuery = "UPDATE Accounts SET Balance = Balance - @Amount WHERE AccountNo = @AccountNo AND Deleted = 0";
                        SqlCommand debitCmd = new SqlCommand(debitQuery, con);
                        debitCmd.Parameters.AddWithValue("@Amount", amount);
                        debitCmd.Parameters.AddWithValue("@AccountNo", acNo);
                        con.Open();
                        debitCmd.ExecuteNonQuery();
                        con.Close();

                        // Update destination account balance
                        string creditQuery = "UPDATE Accounts SET Balance = Balance + @Amount WHERE AccountNo = @ToAccount AND Deleted = 0";
                        SqlCommand creditCmd = new SqlCommand(creditQuery, con);
                        creditCmd.Parameters.AddWithValue("@Amount", amount);
                        creditCmd.Parameters.AddWithValue("@ToAccount", toAccount);
                        con.Open();
                        creditCmd.ExecuteNonQuery();
                        con.Close();

                        // Log the transaction
                        LogTransaction("Transfer", amount, toAccount);
                        MessageBox.Show("Transfer successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Destination account not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Insufficient balance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LogTransaction(string transactionType, decimal amount, string toAccount = null)
        {
            using (SqlConnection con = DBConn.GetConnection())
            {
                string query = "INSERT INTO [Transaction] (TransactionType, AccountID, TransferAccount, Status, ApprovedBy, Amount) " +
                               "VALUES (@TransactionType, @AccountID, @TransferAccount, @Status, @ApprovedBy, @Amount)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@TransactionType", transactionType);
                cmd.Parameters.AddWithValue("@AccountID", acNo);
                cmd.Parameters.AddWithValue("@TransferAccount", toAccount ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", "Approved");  
                cmd.Parameters.AddWithValue("@ApprovedBy", username);
                cmd.Parameters.AddWithValue("@Amount", amount);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
