using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BANK
{
    public partial class ViewApplications : Form
    {
        string Username; 
        int ApplicationID;
        SqlConnection conn;

        public ViewApplications(int applicationID, string username)
        {
            InitializeComponent();
            ApplicationID = applicationID;
            Username = username;
        }

        private void ViewApplications_Load(object sender, EventArgs e)
        {
            conn = DBConn.GetConnection();
            DisableFields();
            LoadApplicationData();

            string checkHireStatusQuery = "SELECT Status FROM JobApplications WHERE ApplicationID = @ApplicationID";

            using (SqlCommand cmd = new SqlCommand(checkHireStatusQuery, conn))
            {
                cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                conn.Open();

                var hiredStatus = cmd.ExecuteScalar()?.ToString();

                if (hiredStatus != null && hiredStatus.Equals("Hired", StringComparison.OrdinalIgnoreCase))
                {
                    btnHire.Visible = false;
                }
                conn.Close();
            }
        }


        private void LoadApplicationData()
        {
            string query = "SELECT FullName, Gender, DateOfBirth, MobileNo, Address, JobExperience, PreferredDesignation, Email, NID, ProfilePic, SignaturePic, CV FROM JobApplications WHERE ApplicationID = @ApplicationID";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtFullName.Text = reader["FullName"].ToString();
                    comboGender.SelectedItem = reader["Gender"].ToString();
                    dateDob.Value = Convert.ToDateTime(reader["DateOfBirth"]);
                    txtMobile.Text = reader["MobileNo"].ToString();
                    txtAddress.Text = reader["Address"].ToString();
                    txtJobExperience.Text = reader["JobExperience"].ToString();
                    comboDesignation.SelectedItem = reader["PreferredDesignation"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                    txtNID.Text = reader["NID"].ToString();

                    byte[] profilePic = reader["ProfilePic"] as byte[];
                    if (profilePic != null)
                        picProfile.Image = ByteArrayToImage(profilePic);

                    byte[] signaturePic = reader["SignaturePic"] as byte[];
                    if (signaturePic != null)
                        picSign.Image = ByteArrayToImage(signaturePic);

                    byte[] cv = reader["CV"] as byte[];
                }
                conn.Close();
            }
        }

        private void DisableFields()
        {
            txtAddress.Enabled = false;
            txtCV.Enabled = false;
            txtEmail.Enabled = false;
            txtFullName.Enabled = false;
            txtJobExperience.Enabled = false;
            txtMobile.Enabled = false;
            txtNID.Enabled = false;
            comboDesignation.Enabled = false;
            comboGender.Enabled = false;
            picProfile.Enabled = false;
            picSign.Enabled = false;
            dateDob.Enabled = false;

            btnSave.Visible = false;
            btnEdit.Visible = true;
            btnUploadSign.Visible = false;
            btnUploadProfile.Visible = false;
            button1.Visible = false;
        }

        private void EnableFields()
        {
            txtAddress.Enabled = true;
            txtCV.Enabled = true;
            txtEmail.Enabled = true;
            txtFullName.Enabled = true;
            txtJobExperience.Enabled = true;
            txtMobile.Enabled = true;
            txtNID.Enabled = true;
            comboDesignation.Enabled = true;
            comboGender.Enabled = true;
            picProfile.Enabled = true;
            picSign.Enabled = true;
            dateDob.Enabled = true;

            btnSave.Visible = true;
            btnEdit.Visible = false;
            btnUploadSign.Visible = true;
            btnUploadProfile.Visible = true;
            button1.Visible = true;
        }

        private void btnUploadSign_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    picSign.Image = Image.FromFile(dlg.FileName);

                    // Update the signature in the database
                    using (SqlCommand cmd = new SqlCommand("UPDATE JobApplications SET SignaturePic = @SignaturePic WHERE ApplicationID = @ApplicationID", conn))
                    {
                        cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        cmd.Parameters.AddWithValue("@SignaturePic", ImageToByteArray(picSign.Image));
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
        }

        private void btnUploadProfile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    picProfile.Image = Image.FromFile(dlg.FileName);

                    // Update the profile picture in the database
                    using (SqlCommand cmd = new SqlCommand("UPDATE JobApplications SET ProfilePic = @ProfilePic WHERE ApplicationID = @ApplicationID", conn))
                    {
                        cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        cmd.Parameters.AddWithValue("@ProfilePic", ImageToByteArray(picProfile.Image));
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Update the CV in the database
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "PDF Files|*.pdf";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    byte[] cvBytes = File.ReadAllBytes(dlg.FileName);

                    using (SqlCommand cmd = new SqlCommand("UPDATE JobApplications SET CV = @CV WHERE ApplicationID = @ApplicationID", conn))
                    {
                        cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        cmd.Parameters.AddWithValue("@CV", cvBytes);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
        }

        private void btnDownloadCV_Click(object sender, EventArgs e)
        {
            // Download the CV
            string query = "SELECT CV FROM JobApplications WHERE ApplicationID = @ApplicationID";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                conn.Open();
                byte[] cvBytes = cmd.ExecuteScalar() as byte[];
                conn.Close();

                if (cvBytes != null)
                {
                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.Filter = "PDF Files|*.pdf";
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(dlg.FileName, cvBytes);
                    }
                }
                else
                {
                    MessageBox.Show("No CV available to download.");
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EnableFields();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Save the updated data to the database
            string query = "UPDATE JobApplications SET FullName = @FullName, Gender = @Gender, DateOfBirth = @DateOfBirth, MobileNo = @MobileNo, Address = @Address, " +
                           "JobExperience = @JobExperience, PreferredDesignation = @PreferredDesignation, Email = @Email, NID = @NID WHERE ApplicationID = @ApplicationID";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                cmd.Parameters.AddWithValue("@FullName", txtFullName.Text);
                cmd.Parameters.AddWithValue("@Gender", comboGender.SelectedItem);
                cmd.Parameters.AddWithValue("@DateOfBirth", dateDob.Value);
                cmd.Parameters.AddWithValue("@MobileNo", txtMobile.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@JobExperience", txtJobExperience.Text);
                cmd.Parameters.AddWithValue("@PreferredDesignation", comboDesignation.SelectedItem);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@NID", txtNID.Text);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            DisableFields();
            MessageBox.Show("Changes saved successfully.");
        }

        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }

        private void btnHire_Click(object sender, EventArgs e)
        {
            string name = txtFullName.Text;
            string designation = comboDesignation.SelectedItem.ToString();
            int applicationID = ApplicationID;

            SelectFollowing sf = new SelectFollowing(name, designation, applicationID, Username);
            sf.ShowDialog();
            this.Close(); 
        }
    }
}
