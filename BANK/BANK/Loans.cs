using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BANK
{
    public partial class Loans : Form
    {
        string username;

        public Loans(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void Loans_Load(object sender, EventArgs e)
        {
            LoadLoanData();
        }

        private void LoadLoanData()
        {
            string query = "SELECT * FROM [dbo].[Loan]";
            using (SqlConnection conn = DBConn.GetConnection())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a loan to approve.");
                return;
            }

            int loanID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["LoanID"].Value);
            decimal loanAmount = Convert.ToDecimal(dataGridView1.SelectedRows[0].Cells["LoanAmount"].Value);
            string accountNo = dataGridView1.SelectedRows[0].Cells["AccountNo"].Value.ToString();

            string updateLoanQuery = "UPDATE [dbo].[Loan] SET Status = 'Approved', ApprovedBy = @Username WHERE LoanID = @LoanID";
            string updateBalanceQuery = "UPDATE [dbo].[Accounts] SET Balance = Balance + @LoanAmount WHERE AccountNo = @AccountNo";

            using (SqlConnection conn = DBConn.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmdLoan = new SqlCommand(updateLoanQuery, conn, transaction))
                        {
                            cmdLoan.Parameters.AddWithValue("@Username", username);
                            cmdLoan.Parameters.AddWithValue("@LoanID", loanID);
                            cmdLoan.ExecuteNonQuery();
                        }

                        using (SqlCommand cmdBalance = new SqlCommand(updateBalanceQuery, conn, transaction))
                        {
                            cmdBalance.Parameters.AddWithValue("@LoanAmount", loanAmount);
                            cmdBalance.Parameters.AddWithValue("@AccountNo", accountNo);
                            cmdBalance.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("Loan approved successfully.");
                        LoadLoanData();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a loan to cancel.");
                return;
            }

            int loanID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["LoanID"].Value);
            string query = "UPDATE [dbo].[Loan] SET Status = 'Canceled' WHERE LoanID = @LoanID";

            using (SqlConnection conn = DBConn.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@LoanID", loanID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Loan canceled successfully.");
                    LoadLoanData();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadLoanData();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            comboColumns.Items.Clear(); 
            // write the dynamic search functionality here. 
        }
    }
}
