namespace BANK
{
    partial class AdminDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminDashboard));
            this.displayPanel = new System.Windows.Forms.Panel();
            this.comboFunctions = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAdminId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDate = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // displayPanel
            // 
            this.displayPanel.BackColor = System.Drawing.Color.OldLace;
            this.displayPanel.BackgroundImage = global::BANK.Properties.Resources.LOGO;
            this.displayPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.displayPanel.Location = new System.Drawing.Point(14, 129);
            this.displayPanel.Margin = new System.Windows.Forms.Padding(2);
            this.displayPanel.Name = "displayPanel";
            this.displayPanel.Size = new System.Drawing.Size(1155, 609);
            this.displayPanel.TabIndex = 1;
            // 
            // comboFunctions
            // 
            this.comboFunctions.Font = new System.Drawing.Font("PMingLiU-ExtB", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboFunctions.FormattingEnabled = true;
            this.comboFunctions.Items.AddRange(new object[] {
            "CUSTOMERS",
            "ACCOUNTS",
            "JOB APPLICATIONS",
            "EMPLOYEE",
            "TRANSACTION",
            "LOAN",
            "BANK INFO",
            "LOGOUT"});
            this.comboFunctions.Location = new System.Drawing.Point(884, 28);
            this.comboFunctions.Margin = new System.Windows.Forms.Padding(2);
            this.comboFunctions.Name = "comboFunctions";
            this.comboFunctions.Size = new System.Drawing.Size(284, 30);
            this.comboFunctions.TabIndex = 0;
            this.comboFunctions.Text = "MENU";
            this.comboFunctions.SelectedIndexChanged += new System.EventHandler(this.comboFunctions_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label2.Font = new System.Drawing.Font("MingLiU_MSCS-ExtB", 19.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 31);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 27);
            this.label2.TabIndex = 2;
            this.label2.Text = "Admin. ID:";
            // 
            // txtAdminId
            // 
            this.txtAdminId.Font = new System.Drawing.Font("NSimSun", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAdminId.Location = new System.Drawing.Point(165, 25);
            this.txtAdminId.Name = "txtAdminId";
            this.txtAdminId.ReadOnly = true;
            this.txtAdminId.Size = new System.Drawing.Size(274, 35);
            this.txtAdminId.TabIndex = 41;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label1.Font = new System.Drawing.Font("MingLiU_MSCS-ExtB", 19.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(798, 75);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 27);
            this.label1.TabIndex = 43;
            this.label1.Text = "Date:";
            // 
            // txtDate
            // 
            this.txtDate.Font = new System.Drawing.Font("NSimSun", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDate.Location = new System.Drawing.Point(885, 72);
            this.txtDate.Name = "txtDate";
            this.txtDate.ReadOnly = true;
            this.txtDate.Size = new System.Drawing.Size(283, 35);
            this.txtDate.TabIndex = 44;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DimGray;
            this.panel1.Controls.Add(this.txtDate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtAdminId);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.comboFunctions);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1181, 120);
            this.panel1.TabIndex = 0;
            // 
            // AdminDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1180, 757);
            this.Controls.Add(this.displayPanel);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AdminDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GLOBAL TRUST BANK | Admin Dashboard";
            this.Load += new System.EventHandler(this.AdminDashboard_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel displayPanel;
        private System.Windows.Forms.ComboBox comboFunctions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAdminId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDate;
        private System.Windows.Forms.Panel panel1;
    }
}

