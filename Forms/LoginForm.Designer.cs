namespace SSRMS.Forms
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlCard;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblSubHeader;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblFeedback;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pnlCard = new System.Windows.Forms.Panel();
            lblHeader = new System.Windows.Forms.Label();
            lblSubHeader = new System.Windows.Forms.Label();
            txtUsername = new System.Windows.Forms.TextBox();
            txtPassword = new System.Windows.Forms.TextBox();
            btnLogin = new System.Windows.Forms.Button();
            lblFeedback = new System.Windows.Forms.Label();
            pnlCard.SuspendLayout();
            SuspendLayout();
            //
            // pnlCard
            //
            pnlCard.BackColor = System.Drawing.Color.FromArgb(28, 36, 54);
            pnlCard.Controls.Add(lblFeedback);
            pnlCard.Controls.Add(btnLogin);
            pnlCard.Controls.Add(txtPassword);
            pnlCard.Controls.Add(txtUsername);
            pnlCard.Controls.Add(lblSubHeader);
            pnlCard.Controls.Add(lblHeader);
            pnlCard.Location = new System.Drawing.Point(56, 42);
            pnlCard.Name = "pnlCard";
            pnlCard.Size = new System.Drawing.Size(418, 320);
            pnlCard.TabIndex = 0;
            //
            // lblHeader
            //
            lblHeader.AutoSize = true;
            lblHeader.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblHeader.Location = new System.Drawing.Point(32, 30);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new System.Drawing.Size(210, 32);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "Secure SSRMS Login";
            //
            // lblSubHeader
            //
            lblSubHeader.AutoSize = true;
            lblSubHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblSubHeader.Location = new System.Drawing.Point(35, 74);
            lblSubHeader.Name = "lblSubHeader";
            lblSubHeader.Size = new System.Drawing.Size(329, 15);
            lblSubHeader.TabIndex = 1;
            lblSubHeader.Text = "Access student records securely using your assigned university role.";
            //
            // txtUsername
            //
            txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtUsername.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtUsername.Location = new System.Drawing.Point(35, 112);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Username";
            txtUsername.Size = new System.Drawing.Size(348, 25);
            txtUsername.TabIndex = 2;
            //
            // txtPassword
            //
            txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtPassword.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtPassword.Location = new System.Drawing.Point(35, 155);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Password";
            txtPassword.Size = new System.Drawing.Size(348, 25);
            txtPassword.TabIndex = 3;
            //
            // btnLogin
            //
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnLogin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            btnLogin.Location = new System.Drawing.Point(35, 205);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new System.Drawing.Size(348, 42);
            btnLogin.TabIndex = 4;
            btnLogin.Text = "Sign In";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += new System.EventHandler(btnLogin_Click);
            //
            // lblFeedback
            //
            lblFeedback.AutoSize = true;
            lblFeedback.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblFeedback.Location = new System.Drawing.Point(35, 265);
            lblFeedback.Name = "lblFeedback";
            lblFeedback.Size = new System.Drawing.Size(0, 15);
            lblFeedback.TabIndex = 5;
            //
            // LoginForm
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(18, 24, 34);
            ClientSize = new System.Drawing.Size(530, 420);
            Controls.Add(pnlCard);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "SSRMS Login";
            Load += new System.EventHandler(LoginForm_Load);
            pnlCard.ResumeLayout(false);
            pnlCard.PerformLayout();
            ResumeLayout(false);
        }
    }
}
