using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BANK
{
    public partial class CustomerDashboard : Form
    {
        string AcNO;

        public CustomerDashboard(string acNO)
        {
            InitializeComponent();
            AcNO = acNO;
        }

        private void CustomerDashboard_Load(object sender, EventArgs e)
        {
            txtAcNo.Text = AcNO;
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadAccountData();
        }

        public void LoadAccountData()
        {
            using (SqlConnection conn = DBConn.GetConnection())
            {
                string query = "SELECT AccountName, Balance FROM [dbo].[Accounts] WHERE AccountNo = @AccountNo AND Deleted = 0";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AccountNo", AcNO);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        txtAcName.Text = reader["AccountName"].ToString();
                        txtBalance.Text = reader["Balance"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Account not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading account data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void comboFunctions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboFunctions.Text == "LOGOUT")
            {
                DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    WelcomePage welcomePage = new WelcomePage();
                    welcomePage.Show();
                    this.Close();
                    
                }
            }
            else if (comboFunctions.Text == "TRANSACTIONS")
            {
                LoadPanel(new CustomerTransactions(AcNO));
                LoadAccountData(); 
                LoadAccountData();

            }

            else if (comboFunctions.Text == "PROFILE")
            {
                LoadPanel(new CustomerProfile(AcNO));
                LoadAccountData(); 
            }
            else if (comboFunctions.Text == "LOAN")
            {
                LoadPanel(new ApplyLoan(AcNO));
                LoadAccountData();
            }
            else if (comboFunctions.Text == "LOAN INSTALLMENTS")
            {
                LoadPanel(new CustomerLoans(AcNO));
                LoadAccountData();
            }
        }

        private void LoadPanel(Form form)
        {
            displayPanel.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            displayPanel.Controls.Add(form);
            form.Show();
        }
    }
}
