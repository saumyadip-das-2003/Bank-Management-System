using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BANK
{
    public partial class CustomerProfile : Form
    {
        string acNo;

        public CustomerProfile(string acNo)
        {
            InitializeComponent();
            this.acNo = acNo;
        }

        private void CustomerProfile_Load(object sender, EventArgs e)
        {
            txtACNo.Text = acNo;

            string query = @"
        SELECT c.FullName, c.Gender, c.DateOfBirth, c.MobileNo, c.Address, c.NID, c.Email, 
               a.Balance, a.Password, c.ProfilePicture, c.SignaturePicture 
        FROM dbo.Customers c
        JOIN dbo.Accounts a ON c.CustomerID = a.CustomerID
        WHERE a.AccountNo = @AccountNo AND a.Deleted = 0";

            using (SqlConnection connection = DBConn.GetConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AccountNo", acNo);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        txtFullName.Text = reader["FullName"].ToString();
                        txtGender.Text = reader["Gender"].ToString();
                        txtDOB.Text = Convert.ToDateTime(reader["DateOfBirth"]).ToString("yyyy-MM-dd");
                        txtMobile.Text = reader["MobileNo"].ToString();
                        txtAddress.Text = reader["Address"].ToString();
                        txtNID.Text = reader["NID"].ToString();
                        txtBalance.Text = reader["Balance"].ToString();
                        txtPass.Text = reader["Password"].ToString();

                        // Correctly load ProfilePicture and SignaturePicture
                        if (reader["ProfilePicture"] != DBNull.Value)
                        {
                            byte[] profilePicData = (byte[])reader["ProfilePicture"];
                            using (var ms = new System.IO.MemoryStream(profilePicData))
                            {
                                picProfile.Image = System.Drawing.Image.FromStream(ms);
                            }
                        }
                        if (reader["SignaturePicture"] != DBNull.Value)
                        {
                            byte[] signPicData = (byte[])reader["SignaturePicture"];
                            using (var ms = new System.IO.MemoryStream(signPicData))
                            {
                                picSign.Image = System.Drawing.Image.FromStream(ms);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No data found for this account number.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            txtPass.ReadOnly = false;
            btnSave.Visible = true;
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            string newPassword = txtPass.Text;

            string updateQuery = "UPDATE dbo.Accounts SET Password = @Password WHERE AccountNo = @AccountNo";

            using (SqlConnection connection = DBConn.GetConnection())
            {
                SqlCommand command = new SqlCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@Password", newPassword);
                command.Parameters.AddWithValue("@AccountNo", acNo);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Password updated successfully.");
                        txtPass.ReadOnly = true;
                        btnSave.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Failed to update password.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}
