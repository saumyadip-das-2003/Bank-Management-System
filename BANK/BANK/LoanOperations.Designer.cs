namespace BANK
{
    partial class LoanOperations
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoanOperations));
            this.txtMonthlyInstallment = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboDuration = new System.Windows.Forms.ComboBox();
            this.comboLoanAmount = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtInterestRate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.comboLoanType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtMonthlyInstallment
            // 
            this.txtMonthlyInstallment.BackColor = System.Drawing.Color.White;
            this.txtMonthlyInstallment.Font = new System.Drawing.Font("NSimSun", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMonthlyInstallment.Location = new System.Drawing.Point(390, 320);
            this.txtMonthlyInstallment.Name = "txtMonthlyInstallment";
            this.txtMonthlyInstallment.ReadOnly = true;
            this.txtMonthlyInstallment.Size = new System.Drawing.Size(260, 41);
            this.txtMonthlyInstallment.TabIndex = 39;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label5.Font = new System.Drawing.Font("MingLiU_MSCS-ExtB", 19.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(62, 327);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(306, 27);
            this.label5.TabIndex = 38;
            this.label5.Text = "Monthly Installement:";
            // 
            // comboDuration
            // 
            this.comboDuration.Font = new System.Drawing.Font("PMingLiU-ExtB", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboDuration.FormattingEnabled = true;
            this.comboDuration.Items.AddRange(new object[] {
            ""});
            this.comboDuration.Location = new System.Drawing.Point(390, 233);
            this.comboDuration.Margin = new System.Windows.Forms.Padding(2);
            this.comboDuration.Name = "comboDuration";
            this.comboDuration.Size = new System.Drawing.Size(260, 30);
            this.comboDuration.TabIndex = 37;
            this.comboDuration.Text = "Select Duration (Year)";
            this.comboDuration.SelectedIndexChanged += new System.EventHandler(this.comboDuration_SelectedIndexChanged);
            // 
            // comboLoanAmount
            // 
            this.comboLoanAmount.Font = new System.Drawing.Font("PMingLiU-ExtB", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboLoanAmount.FormattingEnabled = true;
            this.comboLoanAmount.Items.AddRange(new object[] {
            ""});
            this.comboLoanAmount.Location = new System.Drawing.Point(390, 186);
            this.comboLoanAmount.Margin = new System.Windows.Forms.Padding(2);
            this.comboLoanAmount.Name = "comboLoanAmount";
            this.comboLoanAmount.Size = new System.Drawing.Size(260, 30);
            this.comboLoanAmount.TabIndex = 36;
            this.comboLoanAmount.Text = "Select Amount";
            this.comboLoanAmount.SelectedIndexChanged += new System.EventHandler(this.comboLoanAmount_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label4.Font = new System.Drawing.Font("MingLiU_MSCS-ExtB", 19.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(160, 280);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(208, 27);
            this.label4.TabIndex = 35;
            this.label4.Text = "Interest Rate:";
            // 
            // txtInterestRate
            // 
            this.txtInterestRate.BackColor = System.Drawing.Color.White;
            this.txtInterestRate.Font = new System.Drawing.Font("NSimSun", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInterestRate.Location = new System.Drawing.Point(390, 273);
            this.txtInterestRate.Name = "txtInterestRate";
            this.txtInterestRate.ReadOnly = true;
            this.txtInterestRate.Size = new System.Drawing.Size(260, 41);
            this.txtInterestRate.TabIndex = 34;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label3.Font = new System.Drawing.Font("MingLiU_MSCS-ExtB", 19.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(230, 233);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 27);
            this.label3.TabIndex = 33;
            this.label3.Text = "Duration:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label1.Font = new System.Drawing.Font("MingLiU_MSCS-ExtB", 19.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(188, 186);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 27);
            this.label1.TabIndex = 32;
            this.label1.Text = "Loan Amount:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label2.Font = new System.Drawing.Font("MingLiU_MSCS-ExtB", 19.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(216, 144);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 27);
            this.label2.TabIndex = 31;
            this.label2.Text = "Loan Type:";
            // 
            // btnApply
            // 
            this.btnApply.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnApply.Font = new System.Drawing.Font("Palatino Linotype", 21.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.Location = new System.Drawing.Point(519, 381);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(131, 41);
            this.btnApply.TabIndex = 30;
            this.btnApply.Text = "Add Loan";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // comboLoanType
            // 
            this.comboLoanType.Font = new System.Drawing.Font("PMingLiU-ExtB", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboLoanType.FormattingEnabled = true;
            this.comboLoanType.Items.AddRange(new object[] {
            ""});
            this.comboLoanType.Location = new System.Drawing.Point(390, 144);
            this.comboLoanType.Margin = new System.Windows.Forms.Padding(2);
            this.comboLoanType.Name = "comboLoanType";
            this.comboLoanType.Size = new System.Drawing.Size(260, 30);
            this.comboLoanType.TabIndex = 29;
            this.comboLoanType.Text = "Select Type";
            this.comboLoanType.SelectedIndexChanged += new System.EventHandler(this.comboLoanType_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Monotype Corsiva", 36F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(318, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 57);
            this.label6.TabIndex = 40;
            this.label6.Text = "Loans";
            // 
            // LoanOperations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.OldLace;
            this.ClientSize = new System.Drawing.Size(784, 461);
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
            this.Name = "LoanOperations";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GLOBAL TRUST BANK | Loan Operation";
            this.Load += new System.EventHandler(this.LoanOperations_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMonthlyInstallment;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboDuration;
        private System.Windows.Forms.ComboBox comboLoanAmount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtInterestRate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.ComboBox comboLoanType;
        private System.Windows.Forms.Label label6;
    }
}