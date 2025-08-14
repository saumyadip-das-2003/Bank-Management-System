using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BANK
{
    public partial class ApplyForJob : Form
    {
        WelcomePage welcomePage;
        public ApplyForJob()
        {
            InitializeComponent();
        }

        public ApplyForJob(WelcomePage welcomePage)
        {
            InitializeComponent();
            this.welcomePage = welcomePage;
        }

        private void ApplyForJob_Load(object sender, EventArgs e)
        {
            comboGender.SelectedIndex = 0; 
            comboDesignation.SelectedIndex = 0; 
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    MessageBox.Show("Full Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (comboGender.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select a gender.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (dateDob.Value > DateTime.Now.AddYears(-18))
                {
                    MessageBox.Show("Applicant must be at least 18 years old.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtMobile.Text) || txtMobile.Text.Length < 10 || txtMobile.Text.Length > 15)
                {
                    MessageBox.Show("Enter a valid mobile number (10-15 characters).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtAddress.Text))
                {
                    MessageBox.Show("Address is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txtJobExperience.Text, out int yearsOfExperience) || yearsOfExperience < 0)
                {
                    MessageBox.Show("Enter a valid number of years for Job Experience (0 or greater).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (comboDesignation.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select a preferred designation.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
                {
                    MessageBox.Show("Enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNID.Text))
                {
                    MessageBox.Show("NID is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCV.Text))
                {
                    MessageBox.Show("Please upload a CV.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Insert into database
                using (SqlConnection conn = DBConn.GetConnection())
                {
                    string query = @"
                INSERT INTO JobApplications 
                (FullName, Gender, DateOfBirth, MobileNo, Address, JobExperience, PreferredDesignation, Email, NID, ProfilePic, SignaturePic, CV, Status) 
                VALUES 
                (@FullName, @Gender, @DateOfBirth, @MobileNo, @Address, @JobExperience, @PreferredDesignation, @Email, @NID, @ProfilePic, @SignaturePic, @CV, 'Not Hired')";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FullName", txtFullName.Text);
                        cmd.Parameters.AddWithValue("@Gender", comboGender.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@DateOfBirth", dateDob.Value);
                        cmd.Parameters.AddWithValue("@MobileNo", txtMobile.Text);
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@JobExperience", yearsOfExperience);
                        cmd.Parameters.AddWithValue("@PreferredDesignation", comboDesignation.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@NID", txtNID.Text);

                        // Convert profile picture to binary
                        if (picProfile.Image != null)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                picProfile.Image.Save(ms, picProfile.Image.RawFormat);
                                cmd.Parameters.AddWithValue("@ProfilePic", ms.ToArray());
                            }
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@ProfilePic", DBNull.Value);
                        }

                        // Convert signature picture to binary
                        if (picSign.Image != null)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                picSign.Image.Save(ms, picSign.Image.RawFormat);
                                cmd.Parameters.AddWithValue("@SignaturePic", ms.ToArray());
                            }
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@SignaturePic", DBNull.Value);
                        }

                        // Read CV file as binary
                        if (!string.IsNullOrWhiteSpace(txtCV.Text))
                        {
                            byte[] fileData = File.ReadAllBytes(txtCV.Text);
                            cmd.Parameters.AddWithValue("@CV", fileData);
                        }

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Application submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (this.ParentForm != null)
                        {
                            WelcomePage wp = (WelcomePage)this.ParentForm;
                            wp.LoadPanel(new Login(wp));
                        }
                        else
                        {
                            this.Close();
                        }

                    }
                }

                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtFullName.Text = string.Empty;
            comboGender.SelectedIndex = 0;
            dateDob.Value = DateTime.Now;
            txtMobile.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtJobExperience.Text = string.Empty;
            comboDesignation.SelectedIndex = 0;
            txtEmail.Text = string.Empty;
            txtNID.Text = string.Empty;
            txtCV.Text = string.Empty;
            picProfile.Image = null;
            picSign.Image = null;
        }

        private void btnUploadProfile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    picProfile.Image = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void btnUploadSign_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    picSign.Image = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void btnUploadCV_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "PDF Files|*.pdf";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtCV.Text = ofd.FileName;
                }
            }
        }

        private void btnUploadProfile_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        picProfile.Image = Image.FromFile(ofd.FileName);
                        MessageBox.Show("Profile picture uploaded successfully.", "Upload Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while uploading the profile picture: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnUploadSign_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        picSign.Image = Image.FromFile(ofd.FileName);
                        MessageBox.Show("Signature uploaded successfully.", "Upload Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while uploading the signature: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "PDF Files|*.pdf";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        txtCV.Text = ofd.FileName;
                        MessageBox.Show("CV uploaded successfully.", "Upload Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while uploading the CV: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

    }
}
