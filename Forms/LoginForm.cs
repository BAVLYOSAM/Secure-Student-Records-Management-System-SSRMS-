using System;
using System.Windows.Forms;
using SSRMS.Data;
using SSRMS.Data.Models;

namespace SSRMS.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            BackColor = Theme.Background;
            pnlCard.BackColor = Theme.Panel;
            lblHeader.ForeColor = Theme.TextHighlight;
            lblSubHeader.ForeColor = Theme.TextSecondary;
            txtUsername.BackColor = Theme.InputBackground;
            txtUsername.ForeColor = Theme.TextPrimary;
            txtPassword.BackColor = Theme.InputBackground;
            txtPassword.ForeColor = Theme.TextPrimary;
            btnLogin.BackColor = Theme.Primary;
            btnLogin.ForeColor = Color.White;
            lblFeedback.ForeColor = Theme.Error;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            lblFeedback.Text = string.Empty;
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                lblFeedback.Text = "Please enter both username and password.";
                return;
            }

            var user = DatabaseHelper.AuthenticateUser(username, password);
            if (user == null)
            {
                lblFeedback.Text = "Invalid credentials. Double-check your username and password.";
                return;
            }

            SessionInfo.StartSession(user);
            var dashboard = new MainDashboardForm();
            dashboard.Show();
            Hide();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
        }
    }
}
