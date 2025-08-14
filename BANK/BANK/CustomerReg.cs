using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BANK
{
    public partial class CustomerReg : Form
    {
        WelcomePage welcomePage;
        public CustomerReg() 
        {
            InitializeComponent();
        }
        public CustomerReg(WelcomePage welcomePage)
        {
            InitializeComponent();
            this.welcomePage = welcomePage;
        }

        private void CustomerReg_Load(object sender, EventArgs e)
        {
            comboGender.SelectedIndex = 0;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtFullName.Text)) throw new Exception("Full Name is required.");
                if (comboGender.SelectedIndex == -1) throw new Exception("Gender is required.");
                if (dateDob.Value.Date > DateTime.Now.AddYears(-18)) throw new Exception("Customer must be at least 18 years old.");
                if (string.IsNullOrWhiteSpace(txtMobileNo.Text)) throw new Exception("Mobile Number is required.");
                if (!IsValidMobileNumber(txtMobileNo.Text)) throw new Exception("Invalid Mobile Number format.");
                if (string.IsNullOrWhiteSpace(txtAddress.Text)) throw new Exception("Address is required.");
                if (string.IsNullOrWhiteSpace(txtNID.Text)) throw new Exception("NID is required.");
                if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !IsValidEmail(txtEmail.Text)) throw new Exception("Invalid email format.");
                if (!decimal.TryParse(txtInitialDeposit.Text, out decimal initialDeposit) || initialDeposit < 0) throw new Exception("Initial Deposit must be a valid positive amount.");

                byte[] profilePic = picProfile.Image != null ? ImageToByteArray(picProfile.Image) : null;
                byte[] signPic = picSign.Image != null ? ImageToByteArray(picSign.Image) : null;

                using (SqlConnection conn = DBConn.GetConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO Customers (FullName, Gender, DateOfBirth, MobileNo, Address, Occupation, Salary, Age, NID, Email, InitialDeposit, ProfilePicture, SignaturePicture) " +
                                   "VALUES (@FullName, @Gender, @DateOfBirth, @MobileNo, @Address, @Occupation, @Salary, @Age, @NID, @Email, @InitialDeposit, @ProfilePicture, @SignaturePicture)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FullName", txtFullName.Text);
                        cmd.Parameters.AddWithValue("@Gender", comboGender.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@DateOfBirth", dateDob.Value.Date);
                        cmd.Parameters.AddWithValue("@MobileNo", txtMobileNo.Text);
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@Occupation", txtOccupation.Text ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Salary", string.IsNullOrWhiteSpace(txtSalary.Text) ? (object)DBNull.Value : decimal.Parse(txtSalary.Text));
                        cmd.Parameters.AddWithValue("@Age", CalculateAge(dateDob.Value.Date));
                        cmd.Parameters.AddWithValue("@NID", txtNID.Text);
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@InitialDeposit", initialDeposit);
                        cmd.Parameters.AddWithValue("@ProfilePicture", profilePic ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@SignaturePicture", signPic ?? (object)DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Customer registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (this.ParentForm is WelcomePage wp)
                {
                    wp.LoadPanel(new Login(welcomePage));
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUploadProfile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "Image Files|*.jpg;*.jpeg;*.png" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    picProfile.Image = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void btnUploadSign_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "Image Files|*.jpg;*.jpeg;*.png" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    picSign.Image = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtFullName.Clear(); 
            comboGender.SelectedIndex = 0;
            dateDob.Value = DateTime.Now;
            txtMobileNo.Clear();
            txtAddress.Clear();
            txtOccupation.Clear();
            txtSalary.Clear();
            txtNID.Clear();
            txtEmail.Clear();
            txtInitialDeposit.Clear();
            picProfile.Image = null;
            picSign.Image = null;
        }

        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            int age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear) age--;
            return age;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidMobileNumber(string mobileNo)
        {
            return mobileNo.Length == 11 ;
        }
    }
}
