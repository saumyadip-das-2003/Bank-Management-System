using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BANK
{
    public partial class ViewAccount : Form
    {
        string AcNo;
        public ViewAccount(string acNo)
        {
            InitializeComponent();
            AcNo = acNo;
        }

        private void ViewAccount_Load(object sender, EventArgs e)
        {
            txtAcNo.Enabled = false;
            txtAcName.Enabled = false;
            txtBalance.Enabled = false;
            txtPass.Enabled = false;
            btnSave.Visible = false;
            LoadAccountDetails();
        }

        private void LoadAccountDetails()
        {
            using (SqlConnection con = DBConn.GetConnection())
            {
                con.Open();
                string query = "SELECT AccountName, Balance, Password FROM Accounts WHERE AccountNo = @AcNo AND Deleted = 0";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@AcNo", AcNo);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtAcNo.Text = AcNo;
                            txtAcName.Text = reader["AccountName"].ToString();
                            txtBalance.Text = reader["Balance"].ToString();
                            txtPass.Text = reader["Password"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Account not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Close();
                        }
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            txtAcName.Enabled = true;
            txtBalance.Enabled = true;
            txtPass.Enabled = true;
            btnSave.Visible = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = DBConn.GetConnection())
            {
                con.Open();
                string query = "UPDATE Accounts SET AccountName = @Name, Balance = @Balance, Password = @Pass WHERE AccountNo = @AcNo";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", txtAcName.Text);
                    cmd.Parameters.AddWithValue("@Balance", Convert.ToDecimal(txtBalance.Text));
                    cmd.Parameters.AddWithValue("@Pass", txtPass.Text);
                    cmd.Parameters.AddWithValue("@AcNo", AcNo);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Account details updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Update failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            txtAcName.Enabled = false;
            txtBalance.Enabled = false;
            txtPass.Enabled = false;
            btnSave.Visible = false;
        }
    }
}