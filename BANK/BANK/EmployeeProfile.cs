using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BANK
{
    public partial class EmployeeProfile : Form
    {
        string EmpID;
        public EmployeeProfile(string empID)
        {
            InitializeComponent();
            EmpID = empID;
        }

        private void EmployeeProfile_Load(object sender, EventArgs e)
        {
            btnSave.Visible = false;
            txtPass.ReadOnly = true;
            LoadEmployeeDetails();
        }

        private void LoadEmployeeDetails()
        {
            using (SqlConnection con = DBConn.GetConnection())
            {
                string query = @"SELECT E.EmployeeID, E.EmployeeName, E.Salary, E.Designation, E.Password, 
                                        J.DateOfBirth, J.Gender, J.MobileNo, J.Address, J.NID, J.ProfilePic, J.SignaturePic
                                 FROM Employee E
                                 JOIN JobApplications J ON E.ApplicationID = J.ApplicationID
                                 WHERE E.EmployeeID = @EmpID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@EmpID", EmpID);
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            txtEmpID.Text = dr["EmployeeID"].ToString();
                            txtEmpName.Text = dr["EmployeeName"].ToString();
                            txtSalary.Text = dr["Salary"].ToString();
                            txtDesignation.Text = dr["Designation"].ToString();
                            txtPass.Text = dr["Password"].ToString();
                            txtDOB.Text = Convert.ToDateTime(dr["DateOfBirth"]).ToString("yyyy-MM-dd");
                            txtGender.Text = dr["Gender"].ToString();
                            txtMobile.Text = dr["MobileNo"].ToString();
                            txtAddress.Text = dr["Address"].ToString();
                            txtNID.Text = dr["NID"].ToString();

                            if (dr["ProfilePic"] != DBNull.Value)
                            {
                                byte[] imageBytes = (byte[])dr["ProfilePic"];
                                using (MemoryStream ms = new MemoryStream(imageBytes))
                                {
                                    picProfile.Image = Image.FromStream(ms);
                                }
                            }

                            if (dr["SignaturePic"] != DBNull.Value)
                            {
                                byte[] signBytes = (byte[])dr["SignaturePic"];
                                using (MemoryStream ms = new MemoryStream(signBytes))
                                {
                                    picSign.Image = Image.FromStream(ms);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtPass.ReadOnly = false;
            btnSave.Visible = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = DBConn.GetConnection())
            {
                string query = "UPDATE Employee SET Password = @Password WHERE EmployeeID = @EmpID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Password", txtPass.Text);
                    cmd.Parameters.AddWithValue("@EmpID", EmpID);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Password updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPass.ReadOnly = true;
                    btnSave.Visible = false;
                }
            }
        }
    }
}
