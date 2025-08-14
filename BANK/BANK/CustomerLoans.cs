using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BANK
{
    public partial class CustomerLoans : Form
    {
        string Ac;

        public CustomerLoans(string ac)
        {
            InitializeComponent();
            Ac = ac;
        }

        private void CustomerLoans_Load(object sender, EventArgs e)
        {
            LoadLoanIDs();
            LoadInstallments();
            LoadLoanData();
            LoadColumns();
        }

        private void LoadColumns()
        {
            comboColumns.Items.Clear();
            comboColumns.Items.Add("LoanID");
            comboColumns.Items.Add("LoanAmount");
            comboColumns.Items.Add("Remaining");
            comboColumns.Items.Add("MonthlyInstallment");
            comboColumns.Items.Add("Status");
            comboColumns.SelectedIndex = 0; 
        }


        private void LoadLoanIDs()
        {
            comboLoanID.Items.Clear();
            string query = "SELECT LoanID FROM [dbo].[Loan] WHERE AccountNo = @AccountNo AND Status LIKE 'Approved'";

            using (SqlConnection conn = DBConn.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@AccountNo", Ac);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboLoanID.Items.Add(reader["LoanID"].ToString());
                    }
                }
            }
        }


        private void LoadInstallments()
        {
            for (int i = 1; i <= 5; i++)
            {
                comboNumberofInstallments.Items.Add(i.ToString());
            }
        }

        private void LoadLoanData()
        {
            string query = "SELECT * FROM [dbo].[Loan] WHERE AccountNo = @AccountNo";
            using (SqlConnection conn = DBConn.GetConnection())
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@AccountNo", Ac);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string column = comboColumns.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(column)) return;

            string query = $"SELECT * FROM [dbo].[Loan] WHERE AccountNo = @AccountNo AND {column} LIKE @Search";
            using (SqlConnection conn = DBConn.GetConnection())
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@AccountNo", Ac);
                adapter.SelectCommand.Parameters.AddWithValue("@Search", $"%{txtSearch.Text}%");
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            if (comboLoanID.SelectedItem == null || comboNumberofInstallments.SelectedItem == null)
            {
                MessageBox.Show("Please select a loan and number of installments.");
                return;
            }

            int loanID = Convert.ToInt32(comboLoanID.SelectedItem);
            int installments = Convert.ToInt32(comboNumberofInstallments.SelectedItem);

            string checkQuery = "SELECT LoanAmount, Remaining, MonthlyInstallment FROM [dbo].[Loan] WHERE LoanID = @LoanID";
            string balanceQuery = "SELECT Balance FROM [dbo].[Accounts] WHERE AccountNo = @AccountNo";

            decimal remaining, monthlyInstallment, balance;
            using (SqlConnection conn = DBConn.GetConnection())
            using (SqlCommand cmd = new SqlCommand(checkQuery, conn))
            {
                cmd.Parameters.AddWithValue("@LoanID", loanID);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return;
                    remaining = Convert.ToDecimal(reader["Remaining"]);
                    monthlyInstallment = Convert.ToDecimal(reader["MonthlyInstallment"]);
                }
            }

            using (SqlConnection conn = DBConn.GetConnection())
            using (SqlCommand cmd = new SqlCommand(balanceQuery, conn))
            {
                cmd.Parameters.AddWithValue("@AccountNo", Ac);
                conn.Open();
                balance = Convert.ToDecimal(cmd.ExecuteScalar());
            }

            decimal totalPayment = monthlyInstallment * installments;
            if (totalPayment > balance)
            {
                MessageBox.Show("Insufficient balance to make this payment.");
                return;
            }

            if (totalPayment > remaining)
            {
                MessageBox.Show("Installment exceeds remaining loan amount.");
                return;
            }

            using (SqlConnection conn = DBConn.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string updateBalanceQuery = "UPDATE [dbo].[Accounts] SET Balance = Balance - @Amount WHERE AccountNo = @AccountNo";
                        string updateLoanQuery = "UPDATE [dbo].[Loan] SET Remaining = Remaining - @Amount, Status = CASE WHEN Remaining - @Amount <= 0 THEN 'Completed' ELSE Status END WHERE LoanID = @LoanID";

                        using (SqlCommand cmdBalance = new SqlCommand(updateBalanceQuery, conn, transaction))
                        {
                            cmdBalance.Parameters.AddWithValue("@Amount", totalPayment);
                            cmdBalance.Parameters.AddWithValue("@AccountNo", Ac);
                            cmdBalance.ExecuteNonQuery();
                        }

                        using (SqlCommand cmdLoan = new SqlCommand(updateLoanQuery, conn, transaction))
                        {
                            cmdLoan.Parameters.AddWithValue("@Amount", totalPayment);
                            cmdLoan.Parameters.AddWithValue("@LoanID", loanID);
                            cmdLoan.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("Loan payment successful.");
                        LoadLoanData();
                        if (this.Parent is CustomerDashboard dashboard)
                        {
                            dashboard.LoadAccountData();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadLoanData();
        }

        private void comboNumberofInstallments_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboLoanID.SelectedItem == null || comboNumberofInstallments.SelectedItem == null) return;

            int loanID = Convert.ToInt32(comboLoanID.SelectedItem);
            int installments = Convert.ToInt32(comboNumberofInstallments.SelectedItem);

            string query = "SELECT MonthlyInstallment FROM [dbo].[Loan] WHERE LoanID = @LoanID";
            using (SqlConnection conn = DBConn.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@LoanID", loanID);
                conn.Open();
                decimal monthlyInstallment = Convert.ToDecimal(cmd.ExecuteScalar());
                txtAmount.Text = (monthlyInstallment * installments).ToString("F2");
            }
        }
    }
}
