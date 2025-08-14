using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BANK
{
    public partial class Login : Form
    {
        private WelcomePage welcomePage; 

        public Login(WelcomePage welcomePage)
        {
            InitializeComponent();
            this.welcomePage = welcomePage; 
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DBConn.GetConnection())
            {
                conn.Open();

                if (AuthenticateAdmin(conn, txtUsername.Text, txtPassword.Text))
                {
                    OpenDashboard(new AdminDashboard(txtUsername.Text));
                }
                else if (AuthenticateCustomer(conn, txtUsername.Text, txtPassword.Text))
                {
                    OpenDashboard(new CustomerDashboard(txtUsername.Text));
                }
                else if (AuthenticateEmployee(conn, txtUsername.Text, txtPassword.Text, out string designation))
                {
                    OpenDashboard(new EmployeeDashboard(txtUsername.Text, designation));
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool AuthenticateAdmin(SqlConnection conn, string username, string password)
        {
            string query = "SELECT Username FROM [dbo].[Admin] WHERE Username = @Username AND Password = @Password";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        private bool AuthenticateCustomer(SqlConnection conn, string accountNo, string password)
        {
            string query = "SELECT AccountNo FROM [dbo].[Accounts] WHERE AccountNo = @AccountNo AND Password = @Password";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@AccountNo", accountNo);
                cmd.Parameters.AddWithValue("@Password", password);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        private bool AuthenticateEmployee(SqlConnection conn, string employeeID, string password, out string designation)
        {
            designation = string.Empty;
            string query = "SELECT Designation FROM [dbo].[Employee] WHERE EmployeeID = @EmployeeID AND Password = @Password";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                cmd.Parameters.AddWithValue("@Password", password);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        designation = reader["Designation"].ToString();
                        return true;
                    }
                }
            }
            return false;
        }

        private void OpenDashboard(Form dashboard)
        {
            
            welcomePage.Hide();            
            dashboard.Show();
        }

        private void btnOpenEye_Click(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '\0';
            btnOpenEye.Visible = false;
            btnCloseEye.Visible = true;
        }

        private void btnCloseEye_Click(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '*';
            btnOpenEye.Visible = true;
            btnCloseEye.Visible = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtPassword.Text = null;
            txtUsername.Text = null;    
        }
    }
}
