namespace SSRMS.Forms
{
    partial class MainDashboardForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Panel pnlSidebarButtons;
        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.Label lblAppTitle;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblRoleBadge;
        private System.Windows.Forms.Label lblClearanceBadge;
        private System.Windows.Forms.Label lblSessionStart;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel pnlContent;

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
            pnlSidebar = new System.Windows.Forms.Panel();
            lblLogo = new System.Windows.Forms.Label();
            pnlSidebarButtons = new System.Windows.Forms.Panel();
            btnLogout = new System.Windows.Forms.Button();
            pnlTopBar = new System.Windows.Forms.Panel();
            lblAppTitle = new System.Windows.Forms.Label();
            lblUserName = new System.Windows.Forms.Label();
            lblRoleBadge = new System.Windows.Forms.Label();
            lblClearanceBadge = new System.Windows.Forms.Label();
            lblSessionStart = new System.Windows.Forms.Label();
            pnlContent = new System.Windows.Forms.Panel();
            pnlSidebar.SuspendLayout();
            pnlTopBar.SuspendLayout();
            SuspendLayout();
            //
            // pnlSidebar
            //
            pnlSidebar.BackColor = System.Drawing.Color.FromArgb(28, 36, 54);
            pnlSidebar.Controls.Add(btnLogout);
            pnlSidebar.Controls.Add(pnlSidebarButtons);
            pnlSidebar.Controls.Add(lblLogo);
            pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            pnlSidebar.Location = new System.Drawing.Point(0, 0);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new System.Drawing.Size(220, 640);
            pnlSidebar.TabIndex = 0;
            //
            // lblLogo
            //
            lblLogo.AutoSize = true;
            lblLogo.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblLogo.ForeColor = System.Drawing.Color.FromArgb(153, 193, 241);
            lblLogo.Location = new System.Drawing.Point(20, 22);
            lblLogo.Name = "lblLogo";
            lblLogo.Size = new System.Drawing.Size(186, 25);
            lblLogo.TabIndex = 0;
            lblLogo.Text = "SSRMS University";
            //
            // pnlSidebarButtons
            //
            pnlSidebarButtons.AutoScroll = true;
            pnlSidebarButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlSidebarButtons.Location = new System.Drawing.Point(0, 98);
            pnlSidebarButtons.Name = "pnlSidebarButtons";
            pnlSidebarButtons.Size = new System.Drawing.Size(220, 472);
            pnlSidebarButtons.TabIndex = 1;
            //
            // btnLogout
            //
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnLogout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            btnLogout.ForeColor = System.Drawing.Color.White;
            btnLogout.Location = new System.Drawing.Point(20, 586);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new System.Drawing.Size(180, 34);
            btnLogout.TabIndex = 2;
            btnLogout.Text = "Logout";
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += new System.EventHandler(btnLogout_Click);
            //
            // pnlTopBar
            //
            pnlTopBar.BackColor = System.Drawing.Color.FromArgb(22, 30, 45);
            pnlTopBar.Controls.Add(lblSessionStart);
            pnlTopBar.Controls.Add(lblClearanceBadge);
            pnlTopBar.Controls.Add(lblRoleBadge);
            pnlTopBar.Controls.Add(lblUserName);
            pnlTopBar.Controls.Add(lblAppTitle);
            pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTopBar.Location = new System.Drawing.Point(220, 0);
            pnlTopBar.Name = "pnlTopBar";
            pnlTopBar.Size = new System.Drawing.Size(990, 100);
            pnlTopBar.TabIndex = 1;
            //
            // lblAppTitle
            //
            lblAppTitle.AutoSize = true;
            lblAppTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblAppTitle.ForeColor = System.Drawing.Color.FromArgb(153, 193, 241);
            lblAppTitle.Location = new System.Drawing.Point(30, 22);
            lblAppTitle.Name = "lblAppTitle";
            lblAppTitle.Size = new System.Drawing.Size(322, 30);
            lblAppTitle.TabIndex = 0;
            lblAppTitle.Text = "Secure Student Records Portal";
            //
            // lblUserName
            //
            lblUserName.AutoSize = true;
            lblUserName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblUserName.ForeColor = System.Drawing.Color.FromArgb(238, 241, 246);
            lblUserName.Location = new System.Drawing.Point(30, 60);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new System.Drawing.Size(79, 19);
            lblUserName.TabIndex = 1;
            lblUserName.Text = "User Name";
            //
            // lblRoleBadge
            //
            lblRoleBadge.AutoSize = true;
            lblRoleBadge.BackColor = System.Drawing.Color.FromArgb(74, 145, 255);
            lblRoleBadge.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblRoleBadge.ForeColor = System.Drawing.Color.White;
            lblRoleBadge.Location = new System.Drawing.Point(520, 28);
            lblRoleBadge.Name = "lblRoleBadge";
            lblRoleBadge.Padding = new System.Windows.Forms.Padding(10, 4, 10, 4);
            lblRoleBadge.Size = new System.Drawing.Size(88, 26);
            lblRoleBadge.TabIndex = 2;
            lblRoleBadge.Text = "ADMIN";
            //
            // lblClearanceBadge
            //
            lblClearanceBadge.AutoSize = true;
            lblClearanceBadge.BackColor = System.Drawing.Color.FromArgb(108, 197, 168);
            lblClearanceBadge.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblClearanceBadge.ForeColor = System.Drawing.Color.White;
            lblClearanceBadge.Location = new System.Drawing.Point(520, 60);
            lblClearanceBadge.Name = "lblClearanceBadge";
            lblClearanceBadge.Padding = new System.Windows.Forms.Padding(10, 4, 10, 4);
            lblClearanceBadge.Size = new System.Drawing.Size(114, 26);
            lblClearanceBadge.TabIndex = 3;
            lblClearanceBadge.Text = "Clearance 0";
            //
            // lblSessionStart
            //
            lblSessionStart.AutoSize = true;
            lblSessionStart.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblSessionStart.ForeColor = System.Drawing.Color.FromArgb(156, 168, 186);
            lblSessionStart.Location = new System.Drawing.Point(760, 40);
            lblSessionStart.Name = "lblSessionStart";
            lblSessionStart.Size = new System.Drawing.Size(164, 13);
            lblSessionStart.TabIndex = 4;
            lblSessionStart.Text = "Session started: --/--/---- --:--";
            //
            // pnlContent
            //
            pnlContent.BackColor = System.Drawing.Color.FromArgb(18, 24, 34);
            pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlContent.Location = new System.Drawing.Point(220, 100);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new System.Drawing.Size(990, 540);
            pnlContent.TabIndex = 2;
            //
            // MainDashboardForm
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(18, 24, 34);
            ClientSize = new System.Drawing.Size(1210, 640);
            Controls.Add(pnlContent);
            Controls.Add(pnlTopBar);
            Controls.Add(pnlSidebar);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainDashboardForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "SSRMS Dashboard";
            pnlSidebar.ResumeLayout(false);
            pnlSidebar.PerformLayout();
            pnlTopBar.ResumeLayout(false);
            pnlTopBar.PerformLayout();
            ResumeLayout(false);
        }
    }
}
