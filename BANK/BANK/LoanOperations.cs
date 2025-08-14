using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BANK
{
    public partial class LoanOperations : Form
    {
        string accountNo;
        string username; 

        public LoanOperations(string accountNo, string username)
        {
            InitializeComponent();
            this.accountNo = accountNo;
            this.username = username;
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

        // Button click handler for applying for a loan
        private void btnApply_Click(object sender, EventArgs e)
        {
            // Get the selected values from the form
            string loanType = comboLoanType.SelectedItem?.ToString();
            decimal loanAmount = Convert.ToDecimal(comboLoanAmount.SelectedItem?.ToString());
            int duration = Convert.ToInt32(comboDuration.SelectedItem?.ToString());
            decimal interestRate = 0m;
            decimal monthlyInstallment = 0m;

            // Validate inputs
            if (string.IsNullOrEmpty(loanType))
            {
                MessageBox.Show("Please select a loan type.");
                return;
            }

            if (!decimal.TryParse(txtInterestRate.Text, out interestRate) || interestRate <= 0)
            {
                MessageBox.Show("Please enter a valid interest rate.");
                return;
            }

            if (!decimal.TryParse(txtMonthlyInstallment.Text, out monthlyInstallment) || monthlyInstallment <= 0)
            {
                MessageBox.Show("Please enter a valid monthly installment.");
                return;
            }

            ApplyForLoan(accountNo, loanType, loanAmount, duration, interestRate, monthlyInstallment);
        }

        private void ApplyForLoan(string accountNo, string loanType, decimal loanAmount, int duration, decimal interestRate, decimal monthlyInstallment)
        {
            using (var conn = DBConn.GetConnection())
            {
                conn.Open();

                // Insert loan details into the Loan table
                string query = "INSERT INTO Loan (LoanType, LoanAmount, Duration, InterestRate, MonthlyInstallment, Remaining, Status, AccountNo, ApprovedBy) " +
                               "VALUES (@LoanType, @LoanAmount, @Duration, @InterestRate, @MonthlyInstallment, @LoanAmount, 'Approved', @AccountNo, @ApprovedBy)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@LoanType", loanType);
                    cmd.Parameters.AddWithValue("@LoanAmount", loanAmount);
                    cmd.Parameters.AddWithValue("@Duration", duration);
                    cmd.Parameters.AddWithValue("@InterestRate", interestRate);
                    cmd.Parameters.AddWithValue("@MonthlyInstallment", monthlyInstallment);
                    cmd.Parameters.AddWithValue("@AccountNo", accountNo);
                    cmd.Parameters.AddWithValue("@ApprovedBy", username);


                    cmd.ExecuteNonQuery();
                }

                // Update the Account balance by adding the loan amount
                string updateAccountQuery = "UPDATE Accounts SET Balance = Balance + @LoanAmount WHERE AccountNo = @AccountNo";
                using (var cmd = new SqlCommand(updateAccountQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@LoanAmount", loanAmount);
                    cmd.Parameters.AddWithValue("@AccountNo", accountNo);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Loan approved successfully.");
        }

        // Event handler when the loan type is selected
        private void comboLoanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboLoanType.SelectedItem != null)
            {
                string selectedLoanType = comboLoanType.SelectedItem.ToString();
                UpdateLoanAmountsAndInterestRate(selectedLoanType);
            }
        }

        // Event handler when the loan amount is selected
        private void comboLoanAmount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboLoanAmount.SelectedItem != null)
            {
                UpdateInterestRateAndInstallment();
            }
        }

        // Event handler when the loan duration is selected
        private void comboDuration_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboDuration.SelectedItem != null)
            {
                UpdateInterestRateAndInstallment();
            }
        }

        // Method to update loan amounts and interest rate based on selected loan type
        private void UpdateLoanAmountsAndInterestRate(string loanType)
        {
            comboLoanAmount.Items.Clear();
            txtInterestRate.Text = string.Empty;
            txtMonthlyInstallment.Text = string.Empty;

            if (loanType == "Home Loan")
            {
                comboLoanAmount.Items.AddRange(new object[] { 100000, 200000, 300000, 400000, 500000 });
                comboLoanAmount.SelectedIndex = 0;
                txtInterestRate.Text = "5.50"; // Set as decimal (5.50%).
            }
            else if (loanType == "Personal Loan")
            {
                comboLoanAmount.Items.AddRange(new object[] { 50000, 100000, 150000 });
                comboLoanAmount.SelectedIndex = 0;
                txtInterestRate.Text = "10.00"; // Set as decimal (10.00%).
            }
            else if (loanType == "Car Loan")
            {
                comboLoanAmount.Items.AddRange(new object[] { 100000, 150000, 200000 });
                comboLoanAmount.SelectedIndex = 0;
                txtInterestRate.Text = "7.50"; // Set as decimal (7.50%).
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
            decimal selectedAmount = 0;
            int selectedDuration = 0;

            // Ensure valid loan amount selection
            if (!decimal.TryParse(comboLoanAmount.SelectedItem.ToString(), out selectedAmount))
            {
                MessageBox.Show("Please select a valid loan amount.");
                return;
            }

            // Ensure valid loan duration selection
            if (!int.TryParse(comboDuration.SelectedItem.ToString(), out selectedDuration))
            {
                MessageBox.Show("Please select a valid loan duration.");
                return;
            }

            decimal interestRate = 0;

            if (selectedLoanType == "Home Loan")
            {
                interestRate = 5.50m;
            }
            else if (selectedLoanType == "Personal Loan")
            {
                interestRate = 10.00m;
            }
            else if (selectedLoanType == "Car Loan")
            {
                interestRate = 7.50m;
            }

            txtInterestRate.Text = interestRate.ToString("F2");

            decimal monthlyRate = interestRate / 100 / 12;
            int months = selectedDuration * 12;
            decimal principal = selectedAmount;

            decimal monthlyInstallment = principal * monthlyRate * (decimal)Math.Pow((double)(1 + monthlyRate), months) /
                                         ((decimal)Math.Pow((double)(1 + monthlyRate), months) - 1);

            txtMonthlyInstallment.Text = monthlyInstallment.ToString("F2");
        }

        private void LoanOperations_Load(object sender, EventArgs e)
        {
            PopulateLoanDuration();
            PopulateLoanAmounts();
            PopulateLoanTypes();
        }
    }
}
