using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BANK
{
    public partial class ApplyLoan : Form
    {
        string AccountNo; 
        public ApplyLoan(string accountNo)
        {
            InitializeComponent();
            AccountNo = accountNo;
        }

        private void ApplyLoan_Load(object sender, EventArgs e)
        {
            // Populate combo boxes
            PopulateLoanTypes();
            PopulateLoanAmounts();
            PopulateLoanDuration();

            // Set default selections
            comboDuration.SelectedIndex = 0;
            comboLoanAmount.SelectedIndex = 0;
            comboLoanType.SelectedIndex = 0;
            txtInterestRate.Text = string.Empty;
            txtMonthlyInstallment.Text = string.Empty;

            // Load the loan data based on default loan type
            UpdateLoanAmountsAndInterestRate("Home Loan"); // Default loan type as Home Loan
        }

        private void PopulateLoanTypes()
        {
            comboLoanType.Items.Clear();
            comboLoanType.Items.Add("Home Loan");
            comboLoanType.Items.Add("Personal Loan");
            comboLoanType.Items.Add("Car Loan");
        }

        private void PopulateLoanAmounts()
        {
            comboLoanAmount.Items.Clear();
            comboLoanAmount.Items.Add(100000);
            comboLoanAmount.Items.Add(200000);
            comboLoanAmount.Items.Add(300000);
            comboLoanAmount.Items.Add(400000);
            comboLoanAmount.Items.Add(500000);
        }

        private void PopulateLoanDuration()
        {
            comboDuration.Items.Clear();
            comboDuration.Items.Add("1");
            comboDuration.Items.Add("2");
            comboDuration.Items.Add("3");
            comboDuration.Items.Add("4");
            comboDuration.Items.Add("5");
        }

        private void comboLoanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboLoanType.SelectedItem != null)
            {
                string selectedLoanType = comboLoanType.SelectedItem.ToString();
                UpdateLoanAmountsAndInterestRate(selectedLoanType);
            }
        }

        private void comboLoanAmount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboLoanAmount.SelectedItem != null)
            {
                UpdateInterestRateAndInstallment();
            }
        }

        private void comboDuration_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboDuration.SelectedItem != null)
            {
                UpdateInterestRateAndInstallment();
            }
        }

        private void UpdateLoanAmountsAndInterestRate(string loanType)
        {
            comboLoanAmount.Items.Clear();
            txtInterestRate.Text = string.Empty;
            txtMonthlyInstallment.Text = string.Empty;

            if (loanType == "Home Loan")
            {
                comboLoanAmount.Items.AddRange(new object[] { 100000, 200000, 300000, 400000, 500000 });
                comboLoanAmount.SelectedIndex = 0; 
                txtInterestRate.Text = "5.5"; 
            }
            else if (loanType == "Personal Loan")
            {
                comboLoanAmount.Items.AddRange(new object[] { 50000, 100000, 150000 });
                comboLoanAmount.SelectedIndex = 0; 
                txtInterestRate.Text = "10.0";
            }
            else if (loanType == "Car Loan")
            {
                comboLoanAmount.Items.AddRange(new object[] { 100000, 150000, 200000 });
                comboLoanAmount.SelectedIndex = 0;
                txtInterestRate.Text = "7.5"; 
            }

            UpdateInterestRateAndInstallment(); 
        }

        private void UpdateInterestRateAndInstallment()
        {
            if (comboLoanType.SelectedItem == null || comboLoanAmount.SelectedItem == null || comboDuration.SelectedItem == null)
            {
                return; 
            }

            string selectedLoanType = comboLoanType.SelectedItem.ToString();
            int selectedAmount = 0;
            int selectedDuration = 0;

            if (!int.TryParse(comboLoanAmount.SelectedItem.ToString(), out selectedAmount))
            {
                MessageBox.Show("Please select a valid loan amount.");
                return;
            }

            if (!int.TryParse(comboDuration.SelectedItem.ToString(), out selectedDuration))
            {
                MessageBox.Show("Please select a valid loan duration.");
                return;
            }

            decimal interestRate = 0;

            if (selectedLoanType == "Home Loan")
            {
                interestRate = 5.5m;
            }
            else if (selectedLoanType == "Personal Loan")
            {
                interestRate = 10.0m;
            }
            else if (selectedLoanType == "Car Loan")
            {
                interestRate = 7.5m;
            }

            txtInterestRate.Text = interestRate.ToString("F2");

            decimal monthlyRate = interestRate / 100 / 12;
            int months = selectedDuration * 12;
            decimal principal = selectedAmount;

            decimal monthlyInstallment = principal * monthlyRate * (decimal)Math.Pow((double)(1 + monthlyRate), months) /
                                 ((decimal)Math.Pow((double)(1 + monthlyRate), months) - 1);

            txtMonthlyInstallment.Text = monthlyInstallment.ToString("F2");
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (comboLoanType.SelectedItem == null || comboLoanAmount.SelectedItem == null || comboDuration.SelectedItem == null)
            {
                MessageBox.Show("Please select all the required fields.");
                return;
            }

            string loanType = comboLoanType.SelectedItem.ToString();
            int loanAmount = Convert.ToInt32(comboLoanAmount.SelectedItem.ToString());
            int duration = Convert.ToInt32(comboDuration.SelectedItem.ToString());
            decimal interestRate = Convert.ToDecimal(txtInterestRate.Text);
            decimal monthlyInstallment = Convert.ToDecimal(txtMonthlyInstallment.Text);
            decimal remainingBalance = monthlyInstallment * duration * 12; 


            string query = @"
        INSERT INTO [dbo].[Loan] 
        ([LoanType], [LoanAmount], [Duration], [InterestRate], [MonthlyInstallment], [Remaining], [Status], [AccountNo], [ApprovedBy]) 
        VALUES 
        (@LoanType, @LoanAmount, @Duration, @InterestRate, @MonthlyInstallment, @Remaining, @Status, @AccountNo, NULL)";

            try
            {
                using (SqlConnection conn = DBConn.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@LoanType", loanType);
                        cmd.Parameters.AddWithValue("@LoanAmount", loanAmount);
                        cmd.Parameters.AddWithValue("@Duration", duration);
                        cmd.Parameters.AddWithValue("@InterestRate", interestRate);
                        cmd.Parameters.AddWithValue("@MonthlyInstallment", monthlyInstallment);
                        cmd.Parameters.AddWithValue("@Remaining", remainingBalance);
                        cmd.Parameters.AddWithValue("@Status", "Pending"); 
                        cmd.Parameters.AddWithValue("@AccountNo", AccountNo); 

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("Loan application submitted successfully.");
                        ResetFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while applying for loan: " + ex.Message);
            }
        }

        private void ResetFields()
        {
            comboLoanType.SelectedIndex = 0;
            comboLoanAmount.SelectedIndex = 0;
            comboDuration.SelectedIndex = 0;

            txtInterestRate.Clear();
            txtMonthlyInstallment.Clear();
        }

    }
}