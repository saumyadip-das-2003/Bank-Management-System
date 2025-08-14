using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BANK
{
    public partial class ViewCustomers : Form
    {
        int customerID;
        string username; 

        public ViewCustomers(int customerID, string username)
        {
            InitializeComponent();
            this.customerID = customerID;
            this.username = username;
        }

        private void ViewCustomers_Load(object sender, EventArgs e)
        {
            LoadCustomerDetails(); 
            EnableFields(false);
            btnChangeProfile.Visible = false;
            btnChangeSign.Visible = false;  
         }

        private void LoadCustomerDetails()
        {
            using (SqlConnection conn = DBConn.GetConnection())
            {
                string query = "SELECT * FROM Customers WHERE CustomerID = @CustomerID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerID", customerID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtFullName.Text = reader["FullName"].ToString();
                    comboGender.Text = reader["Gender"].ToString();
                    dateDob.Value = Convert.ToDateTime(reader["DateOfBirth"]);
                    txtMobileNo.Text = reader["MobileNo"].ToString();
                    txtAddress.Text = reader["Address"].ToString();
                    txtOccupation.Text = reader["Occupation"].ToString();
                    txtSalary.Text = reader["Salary"].ToString();
                    txtNID.Text = reader["NID"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                    txtInitialDeposit.Text = reader["InitialDeposit"].ToString();

                    if (reader["ProfilePicture"] != DBNull.Value)
                        picProfile.Image = ByteArrayToImage((byte[])reader["ProfilePicture"]);

                    if (reader["SignaturePicture"] != DBNull.Value)
                        picSign.Image = ByteArrayToImage((byte[])reader["SignaturePicture"]);
                }
                conn.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EnableFields(true);
            btnSave.Visible = true; 
            btnChangeProfile.Visible = true;
            btnChangeSign.Visible = true;

        }
        

        private void EnableFields(bool enable)
        {
            txtFullName.ReadOnly = !enable;
            comboGender.Enabled = enable;
            dateDob.Enabled = enable;
            txtMobileNo.ReadOnly = !enable;
            txtAddress.ReadOnly = !enable;
            txtOccupation.ReadOnly = !enable;
            txtSalary.ReadOnly = !enable;
            txtNID.ReadOnly = !enable;
            txtEmail.ReadOnly = !enable;
            txtInitialDeposit.ReadOnly = !enable;
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            using (SqlConnection conn = DBConn.GetConnection())
            {
                string query = @"UPDATE Customers SET FullName = @FullName, Gender = @Gender, DateOfBirth = @DateOfBirth, 
                                  MobileNo = @MobileNo, Address = @Address, Occupation = @Occupation, Salary = @Salary, 
                                  NID = @NID, Email = @Email, InitialDeposit = @InitialDeposit WHERE CustomerID = @CustomerID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FullName", txtFullName.Text);
                cmd.Parameters.AddWithValue("@Gender", comboGender.Text);
                cmd.Parameters.AddWithValue("@DateOfBirth", dateDob.Value);
                cmd.Parameters.AddWithValue("@MobileNo", txtMobileNo.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@Occupation", txtOccupation.Text);
                cmd.Parameters.AddWithValue("@Salary", txtSalary.Text);
                cmd.Parameters.AddWithValue("@NID", txtNID.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@InitialDeposit", txtInitialDeposit.Text);
                cmd.Parameters.AddWithValue("@CustomerID", customerID);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Customer details updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                EnableFields(false);
                btnSave.Visible = false;
            }
        }

        private void btnChangeProfile_Click(object sender, EventArgs e)
        {
            UploadImage(picProfile, "ProfilePicture");
        }

        private void btnChangeSign_Click(object sender, EventArgs e)
        {
            UploadImage(picSign, "SignaturePicture");
        }

        private void UploadImage(PictureBox pictureBox, string column)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    byte[] imageBytes = File.ReadAllBytes(ofd.FileName);
                    pictureBox.Image = Image.FromFile(ofd.FileName);
                    UpdateImageInDatabase(imageBytes, column);
                }
            }
        }

        private void UpdateImageInDatabase(byte[] imageBytes, string column)
        {
            using (SqlConnection conn = DBConn.GetConnection())
            {
                string query = $"UPDATE Customers SET {column} = @Image WHERE CustomerID = @CustomerID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Image", imageBytes);
                cmd.Parameters.AddWithValue("@CustomerID", customerID);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }

        private void btnCreateAc_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DBConn.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Insert the account
                    string insertAccountQuery = @"
                INSERT INTO Accounts (AccountNo, Password, AccountName, Balance, CustomerID, CreatedBy)
                VALUES (NULL, @Password, @AccountName, @Balance, @CustomerID, @CreatedBy)";

                    SqlCommand cmdAccount = new SqlCommand(insertAccountQuery, conn, transaction);
                    cmdAccount.Parameters.AddWithValue("@Password", "1234");
                    cmdAccount.Parameters.AddWithValue("@AccountName", txtFullName.Text);
                    cmdAccount.Parameters.AddWithValue("@Balance", Convert.ToDecimal(txtInitialDeposit.Text));
                    cmdAccount.Parameters.AddWithValue("@CustomerID", customerID);
                    cmdAccount.Parameters.AddWithValue("@CreatedBy", username);
                    cmdAccount.ExecuteNonQuery();

                    // Update the AccountCount in Customers table
                    string updateCustomerQuery = "UPDATE Customers SET AccountCount = AccountCount + 1 WHERE CustomerID = @CustomerID";
                    SqlCommand cmdCustomer = new SqlCommand(updateCustomerQuery, conn, transaction);
                    cmdCustomer.Parameters.AddWithValue("@CustomerID", customerID);
                    cmdCustomer.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error creating account: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

    }
}
