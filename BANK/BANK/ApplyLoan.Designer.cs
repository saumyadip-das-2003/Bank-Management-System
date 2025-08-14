namespace BANK
{
    partial class ApplyLoan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplyLoan));
            this.comboLoanType = new System.Windows.Forms.ComboBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtInterestRate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboLoanAmount = new System.Windows.Forms.ComboBox();
            this.comboDuration = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMonthlyInstallment = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboLoanType
            // 
            this.comboLoanType.Font = new System.Drawing.Font("PMingLiU-ExtB", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboLoanType.FormattingEnabled = true;
            this.comboLoanType.Items.AddRange(new object[] {
            ""});
            this.comboLoanType.Location = new System.Drawing.Point(498, 164);
            this.comboLoanType.Margin = new System.Windows.Forms.Padding(2);
            this.comboLoanType.Name = "comboLoanType";
            this.comboLoanType.Size = new System.Drawing.Size(260, 30);
            this.comboLoanType.TabIndex = 1;
            this.comboLoanType.Text = "Select Type";
            this.comboLoanType.SelectedIndexChanged += new System.EventHandler(this.comboLoanType_SelectedIndexChanged);
            // 
            // btnApply
            // 
            this.btnApply.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnApply.Font = new System.Drawing.Font("Palatino Linotype", 21.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.Location = new System.Drawing.Point(627, 401);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(131, 41);
            this.btnApply.TabIndex = 18;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label2.Font = new System.Drawing.Font("MingLiU_MSCS-ExtB", 19.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(324, 164);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 27);
            this.label2.TabIndex = 19;
            this.label2.Text = "Loan Type:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label1.Font = new System.Drawing.Font("MingLiU_MSCS-ExtB", 19.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(296, 206);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 27);
            this.label1.TabIndex = 20;
            this.label1.Text = "Loan Amount:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label3.Font = new System.Drawing.Font("MingLiU_MSCS-ExtB", 19.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(338, 253);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 27);
            this.label3.TabIndex = 21;
            this.label3.Text = "Duration:";
            // 
            // txtInterestRate
            // 
            this.txtInterestRate.BackColor = System.Drawing.Color.White;
            this.txtInterestRate.Font = new System.Drawing.Font("NSimSun", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInterestRate.Location = new System.Drawing.Point(498, 293);
            this.txtInterestRate.Name = "txtInterestRate";
            this.txtInterestRate.ReadOnly = true;
            this.txtInterestRate.Size = new System.Drawing.Size(260, 41);
            this.txtInterestRate.TabIndex = 23;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label4.Font = new System.Drawing.Font("MingLiU_MSCS-ExtB", 19.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(268, 300);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(208, 27);
            this.label4.TabIndex = 24;
            this.label4.Text = "Interest Rate:";
            // 
            // comboLoanAmount
            // 
            this.comboLoanAmount.Font = new System.Drawing.Font("PMingLiU-ExtB", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboLoanAmount.FormattingEnabled = true;
            this.comboLoanAmount.Items.AddRange(new object[] {
            ""});
            this.comboLoanAmount.Location = new System.Drawing.Point(498, 206);
            this.comboLoanAmount.Margin = new System.Windows.Forms.Padding(2);
            this.comboLoanAmount.Name = "comboLoanAmount";
            this.comboLoanAmount.Size = new System.Drawing.Size(260, 30);
            this.comboLoanAmount.TabIndex = 25;
            this.comboLoanAmount.Text = "Select Amount";
            this.comboLoanAmount.SelectedIndexChanged += new System.EventHandler(this.comboLoanAmount_SelectedIndexChanged);
            // 
            // comboDuration
            // 
            this.comboDuration.Font = new System.Drawing.Font("PMingLiU-ExtB", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboDuration.FormattingEnabled = true;
            this.comboDuration.Items.AddRange(new object[] {
            ""});
            this.comboDuration.Location = new System.Drawing.Point(498, 253);
            this.comboDuration.Margin = new System.Windows.Forms.Padding(2);
            this.comboDuration.Name = "comboDuration";
            this.comboDuration.Size = new System.Drawing.Size(260, 30);
            this.comboDuration.TabIndex = 26;
            this.comboDuration.Text = "Select Duration (Year)";
            this.comboDuration.SelectedIndexChanged += new System.EventHandler(this.comboDuration_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label5.Font = new System.Drawing.Font("MingLiU_MSCS-ExtB", 19.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(170, 347);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(306, 27);
            this.label5.TabIndex = 27;
            this.label5.Text = "Monthly Installement:";
            // 
            // txtMonthlyInstallment
            // 
            this.txtMonthlyInstallment.BackColor = System.Drawing.Color.White;
            this.txtMonthlyInstallment.Font = new System.Drawing.Font("NSimSun", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMonthlyInstallment.Location = new System.Drawing.Point(498, 340);
            this.txtMonthlyInstallment.Name = "txtMonthlyInstallment";
            this.txtMonthlyInstallment.ReadOnly = true;
            this.txtMonthlyInstallment.Size = new System.Drawing.Size(260, 41);
            this.txtMonthlyInstallment.TabIndex = 28;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Monotype Corsiva", 48F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(376, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(314, 79);
            this.label6.TabIndex = 29;
            this.label6.Text = "Apply Loan";
            // 
            // ApplyLoan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.OldLace;
            this.ClientSize = new System.Drawing.Size(1009, 550);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtMonthlyInstallment);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboDuration);
            this.Controls.Add(this.comboLoanAmount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtInterestRate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.comboLoanType);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ApplyLoan";
            this.Text = "GLOBAL TRUST BANK | Apply For Loan";
            this.Load += new System.EventHandler(this.ApplyLoan_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboLoanType;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtInterestRate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboLoanAmount;
        private System.Windows.Forms.ComboBox comboDuration;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMonthlyInstallment;
        private System.Windows.Forms.Label label6;
    }
}