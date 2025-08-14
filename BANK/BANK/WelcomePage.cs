using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BANK
{
    public partial class WelcomePage : Form
    {
        public WelcomePage()
        {
            InitializeComponent();
            LoadPanel(new Home());
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        public void LoadPanel(Form form)
        {
            displayPanel.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            displayPanel.Controls.Add(form);
            form.Show();
        }

        private void comboFunctions_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboFunctions.Text)
            {
                case "REGISTER CUSTOMER":
                    LoadPanel(new CustomerReg(this));
                    break;

                case "HOME":
                    LoadPanel(new Home());
                    break;

                case "LOGIN":
                    LoadPanel(new Login(this));
                    break;

                case "APPLLY FOR JOB":
                    LoadPanel(new ApplyForJob(this));
                    break;
                case "EXIT":
                    Application.Exit();
                    break;


                default:
                    MessageBox.Show("Invalid selection!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {
            LoadPanel(new Home()); 
        }

        private void WelcomePage_Load(object sender, EventArgs e)
        {
            LoadPanel(new Home());
        }
    }
}
