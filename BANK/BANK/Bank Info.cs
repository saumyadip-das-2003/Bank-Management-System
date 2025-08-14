using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BANK
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtEmpNo.Text = "";
            txtNoAc.Text = "";
            txtNoCustomer.Text = "";
            txtTotalAmount.Text = "";

            LoadBankData();
        }

        private void LoadBankData()
        {
            try
            {
                using (SqlConnection conn = DBConn.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT TotalBalance, TotalEmployees, TotalCustomers, TotalAccounts FROM [dbo].[Bank] WHERE BankID = 1";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtTotalAmount.Text = reader["TotalBalance"].ToString();
                        txtEmpNo.Text = reader["TotalEmployees"].ToString();
                        txtNoCustomer.Text = reader["TotalCustomers"].ToString();
                        txtNoAc.Text = reader["TotalAccounts"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("No data found in the Bank table.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading bank data: " + ex.Message);
            }
        }
    }
}
