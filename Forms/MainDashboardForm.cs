using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SSRMS.Data;
using SSRMS.Data.Models;

namespace SSRMS.Forms
{
    public partial class MainDashboardForm : Form
    {
        private readonly User _user;

        public MainDashboardForm()
        {
            if (SessionInfo.CurrentUser == null)
                throw new InvalidOperationException("Unauthorized access: user session not found.");

            _user = SessionInfo.CurrentUser;
            InitializeComponent();
            ApplyTheme();
            InitializeUserHeader();
            LoadDashboardForRole();
        }

        private void ApplyTheme()
        {
            BackColor = Theme.Background;
            pnlSidebar.BackColor = Theme.Panel;
            pnlTopBar.BackColor = Theme.DarkerPanel;
            pnlContent.BackColor = Theme.Background;
            lblAppTitle.ForeColor = Theme.TextHighlight;
            lblRoleBadge.ForeColor = Color.White;
            lblClearanceBadge.ForeColor = Color.White;
            btnLogout.BackColor = Theme.Secondary;
            btnLogout.ForeColor = Color.White;
            btnHome.ForeColor = Theme.TextPrimary;
            btnHome.BackColor = Theme.Panel;
        }

        private void InitializeUserHeader()
        {
            lblUserName.Text = _user.FullName;
            lblRoleBadge.Text = _user.Role.ToUpperInvariant();
            lblClearanceBadge.Text = $"Clearance { _user.ClearanceLevel }";
            lblRoleBadge.BackColor = Theme.RoleBadge;
            lblClearanceBadge.BackColor = Theme.ClearanceBadge;
            lblSessionStart.Text = $"Session started: {SessionInfo.LoginTimestamp.ToLocalTime():g}";
        }

        private void LoadDashboardForRole()
        {
            lblHomeTitle.Text = $"Welcome back, {_user.FullName}";
            lblHomeSubtitle.Text = "Secure Student Record Management System";

            switch (_user.Role.ToLowerInvariant())
            {
                case "admin":
                    ShowAdminModules();
                    break;
                case "instructor":
                    ShowInstructorModules();
                    break;
                case "ta":
                    ShowTAModules();
                    break;
                case "student":
                    ShowStudentModules();
                    break;
                case "guest":
                    ShowGuestModules();
                    break;
                default:
                    ShowGuestModules();
                    break;
            }
        }

        private void ShowAdminModules()
        {
            AddSidebarButton("Dashboard", btnHome_Click, true);
            AddSidebarButton("Manage Users", btnManageUsers_Click);
            AddSidebarButton("Role Requests", btnRoleRequests_Click);
            AddSidebarButton("Student Records", btnViewStudents_Click);
            AddSidebarButton("Instructor Records", btnViewInstructors_Click);
            AddSidebarButton("Grades & Attendance", btnViewGradesAttendance_Click);
            ShowAdminDashboard();
        }

        private void ShowInstructorModules()
        {
            AddSidebarButton("Dashboard", btnHome_Click, true);
            AddSidebarButton("Students", btnViewStudents_Click);
            AddSidebarButton("Grades", btnGrades_Click);
            AddSidebarButton("Attendance", btnAttendance_Click);
            ShowInstructorDashboard();
        }

        private void ShowTAModules()
        {
            AddSidebarButton("Dashboard", btnHome_Click, true);
            AddSidebarButton("Assigned Students", btnViewStudents_Click);
            AddSidebarButton("Attendance", btnAttendance_Click);
            ShowTADashboard();
        }

        private void ShowStudentModules()
        {
            AddSidebarButton("Dashboard", btnHome_Click, true);
            AddSidebarButton("Profile", btnProfile_Click);
            AddSidebarButton("Grades", btnGrades_Click);
            AddSidebarButton("Attendance", btnAttendance_Click);
            AddSidebarButton("Request Upgrade", btnRoleRequest_Click);
            ShowStudentDashboard();
        }

        private void ShowGuestModules()
        {
            AddSidebarButton("Dashboard", btnHome_Click, true);
            AddSidebarButton("Courses", btnCourses_Click);
            ShowGuestDashboard();
        }

        private void AddSidebarButton(string text, EventHandler handler, bool active = false)
        {
            var button = new Button
            {
                Text = text,
                Dock = DockStyle.Top,
                Height = 45,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Theme.TextPrimary,
                BackColor = Theme.Panel,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            button.FlatAppearance.BorderSize = 0;
            button.Click += handler;
            pnlSidebarButtons.Controls.Add(button);

            if (active)
            {
                button.BackColor = Theme.Primary;
                button.ForeColor = Color.White;
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            ShowDashboardHome();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            SessionInfo.EndSession();
            var loginForm = new LoginForm();
            loginForm.Show();
            Close();
        }

        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            LoadAdminUserManagement();
        }

        private void btnRoleRequests_Click(object sender, EventArgs e)
        {
            LoadRoleRequestPanel();
        }

        private void btnViewStudents_Click(object sender, EventArgs e)
        {
            LoadStudentRecords();
        }

        private void btnViewInstructors_Click(object sender, EventArgs e)
        {
            LoadInstructorRecords();
        }

        private void btnViewGradesAttendance_Click(object sender, EventArgs e)
        {
            LoadGradesAttendanceOverview();
        }

        private void btnGrades_Click(object sender, EventArgs e)
        {
            LoadGradesOverview();
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            LoadAttendanceOverview();
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            LoadStudentProfile();
        }

        private void btnRoleRequest_Click(object sender, EventArgs e)
        {
            LoadRoleRequestForm();
        }

        private void btnCourses_Click(object sender, EventArgs e)
        {
            LoadCourseInformation();
        }

        private void ShowDashboardHome()
        {
            pnlContent.Controls.Clear();
            var homePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.Background
            };

            var title = new Label
            {
                Text = "Secure Student Records Dashboard",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Theme.TextHighlight,
                Dock = DockStyle.Top,
                Height = 45,
                Padding = new Padding(20, 10, 0, 0)
            };

            var subTitle = new Label
            {
                Text = "Access controls adapt to your role and clearance level.",
                Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Theme.TextSecondary,
                Dock = DockStyle.Top,
                Height = 30,
                Padding = new Padding(20, 0, 0, 0)
            };

            var statGrid = new TableLayoutPanel
            {
                ColumnCount = 4,
                RowCount = 1,
                Dock = DockStyle.Top,
                Height = 160,
                Padding = new Padding(20, 20, 20, 0)
            };
            statGrid.ColumnStyles.Clear();
            for (var i = 0; i < 4; i++)
                statGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            foreach (var stat in DatabaseHelper.LoadDashboardStats())
            {
                statGrid.Controls.Add(CreateStatCard("Students", stat.StudentCount.ToString(), "fa-users"));
                statGrid.Controls.Add(CreateStatCard("Instructors", stat.InstructorCount.ToString(), "fa-chalkboard-teacher"));
                statGrid.Controls.Add(CreateStatCard("Grades", stat.GradeCount.ToString(), "fa-graduation-cap"));
                statGrid.Controls.Add(CreateStatCard("Attendance", stat.AttendanceCount.ToString(), "fa-calendar-check"));
            }

            homePanel.Controls.Add(statGrid);
            homePanel.Controls.Add(subTitle);
            homePanel.Controls.Add(title);
            pnlContent.Controls.Add(homePanel);
        }

        private Panel CreateStatCard(string title, string value, string iconKey)
        {
            var card = new Panel
            {
                BackColor = Theme.Card,
                Margin = new Padding(10),
                Dock = DockStyle.Fill
            };
            var labelTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Theme.TextSecondary,
                Dock = DockStyle.Top,
                Height = 24,
                Padding = new Padding(12, 8, 0, 0)
            };
            var labelValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Theme.TextPrimary,
                Dock = DockStyle.Top,
                Height = 60,
                Padding = new Padding(12, 0, 0, 0)
            };
            card.Controls.Add(labelValue);
            card.Controls.Add(labelTitle);
            return card;
        }

        private void ShowAdminDashboard()
        {
            ShowDashboardHome();
        }

        private void ShowInstructorDashboard()
        {
            ShowDashboardHome();
        }

        private void ShowTADashboard()
        {
            ShowDashboardHome();
        }

        private void ShowStudentDashboard()
        {
            ShowDashboardHome();
        }

        private void ShowGuestDashboard()
        {
            ShowDashboardHome();
        }

        private void LoadAdminUserManagement()
        {
            var panel = CreatePanelFrame("Manage Users", "Administrators can create, edit, and remove user accounts.");
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Theme.Background,
                ForeColor = Theme.TextPrimary,
                EnableHeadersVisualStyles = false,
                ColumnHeadersDefaultCellStyle = { BackColor = Theme.DarkerPanel, ForeColor = Theme.TextHighlight, Font = new Font("Segoe UI", 9F, FontStyle.Bold) }
            };
            grid.DataSource = DatabaseHelper.GetAllUsers();
            panel.Controls.Add(grid);
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(panel);
        }

        private void LoadRoleRequestPanel()
        {
            var panel = CreatePanelFrame("Pending Role Requests", "Review pending role upgrade requests and authorize access changes.");
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Theme.Background,
                ForeColor = Theme.TextPrimary,
                EnableHeadersVisualStyles = false,
                ColumnHeadersDefaultCellStyle = { BackColor = Theme.DarkerPanel, ForeColor = Theme.TextHighlight, Font = new Font("Segoe UI", 9F, FontStyle.Bold) }
            };
            grid.DataSource = DatabaseHelper.GetPendingRoleRequests();
            panel.Controls.Add(grid);
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(panel);
        }

        private void LoadStudentRecords()
        {
            var panel = CreatePanelFrame("Student Records", "Search and review student profiles based on role privileges.");
            var searchBox = new TextBox
            {
                PlaceholderText = "Search students by name, program, or ID...",
                Dock = DockStyle.Top,
                Margin = new Padding(20),
                BackColor = Theme.InputBackground,
                ForeColor = Theme.TextPrimary
            };
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Theme.Background,
                ForeColor = Theme.TextPrimary,
                EnableHeadersVisualStyles = false,
                ColumnHeadersDefaultCellStyle = { BackColor = Theme.DarkerPanel, ForeColor = Theme.TextHighlight, Font = new Font("Segoe UI", 9F, FontStyle.Bold) }
            };
            searchBox.TextChanged += (sender, e) => grid.DataSource = DatabaseHelper.SearchStudents(searchBox.Text.Trim());
            grid.DataSource = DatabaseHelper.SearchStudents(string.Empty);
            panel.Controls.Add(grid);
            panel.Controls.Add(searchBox);
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(panel);
        }

        private void LoadInstructorRecords()
        {
            var panel = CreatePanelFrame("Instructor Records", "Review instructor assignments and department credentials.");
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Theme.Background,
                ForeColor = Theme.TextPrimary,
                EnableHeadersVisualStyles = false,
                ColumnHeadersDefaultCellStyle = { BackColor = Theme.DarkerPanel, ForeColor = Theme.TextHighlight, Font = new Font("Segoe UI", 9F, FontStyle.Bold) }
            };
            grid.DataSource = new List<object> { new { InstructorId = 1, FullName = "Dr. Amina Yusuf", Department = "Computer Science", ClearanceLevel = 4 } };
            panel.Controls.Add(grid);
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(panel);
        }

        private void LoadGradesAttendanceOverview()
        {
            var panel = CreatePanelFrame("Grades & Attendance", "Comprehensive overview for secure academic management.");
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Theme.Background,
                ForeColor = Theme.TextPrimary,
                EnableHeadersVisualStyles = false,
                ColumnHeadersDefaultCellStyle = { BackColor = Theme.DarkerPanel, ForeColor = Theme.TextHighlight, Font = new Font("Segoe UI", 9F, FontStyle.Bold) }
            };
            grid.DataSource = new List<object> { new { StudentId = 1001, FullName = "Aisha Bello", Course = "Data Structures", Grade = "A", Attendance = "96%" } };
            panel.Controls.Add(grid);
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(panel);
        }

        private void LoadGradesOverview()
        {
            var panel = CreatePanelFrame("Grades", "Enter and update grades where your clearance permits.");
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Theme.Background,
                ForeColor = Theme.TextPrimary,
                EnableHeadersVisualStyles = false,
                ColumnHeadersDefaultCellStyle = { BackColor = Theme.DarkerPanel, ForeColor = Theme.TextHighlight, Font = new Font("Segoe UI", 9F, FontStyle.Bold) }
            };
            grid.DataSource = new List<object> { new { StudentId = 1001, Course = "Algorithms", Grade = "B+", Term = "Spring 2026" } };
            panel.Controls.Add(grid);
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(panel);
        }

        private void LoadAttendanceOverview()
        {
            var panel = CreatePanelFrame("Attendance", "Track attendance records in secure view-only mode.");
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Theme.Background,
                ForeColor = Theme.TextPrimary,
                EnableHeadersVisualStyles = false,
                ColumnHeadersDefaultCellStyle = { BackColor = Theme.DarkerPanel, ForeColor = Theme.TextHighlight, Font = new Font("Segoe UI", 9F, FontStyle.Bold) }
            };
            grid.DataSource = new List<object> { new { StudentId = 1001, CourseCode = "CS202", Date = DateTime.Today.AddDays(-1), Status = "Present" } };
            panel.Controls.Add(grid);
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(panel);
        }

        private void LoadStudentProfile()
        {
            var panel = CreatePanelFrame("Profile", "Review and update your student profile information.");
            var profileGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Theme.Background,
                ForeColor = Theme.TextPrimary,
                EnableHeadersVisualStyles = false,
                ColumnHeadersDefaultCellStyle = { BackColor = Theme.DarkerPanel, ForeColor = Theme.TextHighlight, Font = new Font("Segoe UI", 9F, FontStyle.Bold) }
            };
            profileGrid.DataSource = new List<object> { new { Field = "Full Name", Value = _user.FullName }, new { Field = "Department", Value = _user.Department }, new { Field = "Email", Value = _user.Email }, new { Field = "Role", Value = _user.Role } };
            panel.Controls.Add(profileGrid);
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(panel);
        }

        private void LoadRoleRequestForm()
        {
            var panel = CreatePanelFrame("Request Role Upgrade", "Submit a secure role change request for review.");
            var formPanel = new Panel { Dock = DockStyle.Top, Height = 300, Padding = new Padding(20), BackColor = Theme.Background };
            var lblCurrentRole = CreateFormLabel("Current Role");
            var txtCurrentRole = new TextBox { Text = _user.Role, ReadOnly = true, BackColor = Theme.InputBackground, ForeColor = Theme.TextPrimary };
            var lblRequestedRole = CreateFormLabel("Requested Role");
            var cbRequestedRole = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, BackColor = Theme.InputBackground, ForeColor = Theme.TextPrimary };
            cbRequestedRole.Items.AddRange(new object[] { "Instructor", "TA", "Student", "Guest" });
            cbRequestedRole.SelectedIndex = 0;
            var lblJustification = CreateFormLabel("Justification");
            var txtJustification = new TextBox { Multiline = true, Height = 100, BackColor = Theme.InputBackground, ForeColor = Theme.TextPrimary };            
            var btnSubmit = new Button { Text = "Submit Request", BackColor = Theme.Primary, ForeColor = Color.White, Height = 40, Dock = DockStyle.Bottom };
            var lblStatus = new Label { ForeColor = Theme.TextSecondary, Dock = DockStyle.Bottom, Height = 24 };
            btnSubmit.Click += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtJustification.Text))
                {
                    lblStatus.Text = "Please provide a clear justification for your request.";
                    return;
                }

                var success = DatabaseHelper.AddRoleRequest(_user.Username, _user.Role, cbRequestedRole.Text, txtJustification.Text.Trim());
                lblStatus.Text = success ? "Your request has been submitted and is pending review." : "Unable to submit request. Please try again.";
            };

            formPanel.Controls.Add(btnSubmit);
            formPanel.Controls.Add(lblStatus);
            formPanel.Controls.Add(txtJustification);
            formPanel.Controls.Add(lblJustification);
            formPanel.Controls.Add(cbRequestedRole);
            formPanel.Controls.Add(lblRequestedRole);
            formPanel.Controls.Add(txtCurrentRole);
            formPanel.Controls.Add(lblCurrentRole);
            panel.Controls.Add(formPanel);
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(panel);
        }

        private void LoadCourseInformation()
        {
            var panel = CreatePanelFrame("Public Course Information", "Access public course listings available to guests.");
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Theme.Background,
                ForeColor = Theme.TextPrimary,
                EnableHeadersVisualStyles = false,
                ColumnHeadersDefaultCellStyle = { BackColor = Theme.DarkerPanel, ForeColor = Theme.TextHighlight, Font = new Font("Segoe UI", 9F, FontStyle.Bold) }
            };
            grid.DataSource = new List<object> { new { CourseCode = "CS101", Title = "Introduction to Computing", Instructor = "Dr. Amina Yusuf" } };
            panel.Controls.Add(grid);
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(panel);
        }

        private Panel CreatePanelFrame(string title, string subtitle)
        {
            var panel = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Background, Padding = new Padding(20) };
            var titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 16F, FontStyle.SemiBold, GraphicsUnit.Point),
                ForeColor = Theme.TextHighlight,
                Dock = DockStyle.Top,
                Height = 36
            };
            var subtitleLabel = new Label
            {
                Text = subtitle,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Theme.TextSecondary,
                Dock = DockStyle.Top,
                Height = 24,
                Padding = new Padding(0, 0, 0, 12)
            };
            panel.Controls.Add(subtitleLabel);
            panel.Controls.Add(titleLabel);
            return panel;
        }

        private Label CreateFormLabel(string text)
        {
            return new Label
            {
                Text = text,
                ForeColor = Theme.TextSecondary,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                Height = 22,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 6, 0, 6)
            };
        }
    }
}
