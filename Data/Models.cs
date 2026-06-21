using System;

namespace SSRMS.Data.Models
{
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int ClearanceLevel { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public static class SessionInfo
    {
        public static User? CurrentUser { get; set; }
        public static DateTime LoginTimestamp { get; set; }

        public static void StartSession(User user)
        {
            CurrentUser = user;
            LoginTimestamp = DateTime.UtcNow;
        }

        public static void EndSession()
        {
            CurrentUser = null;
        }
    }

    public class RoleRequest
    {
        public int RequestId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string CurrentRole { get; set; } = string.Empty;
        public string RequestedRole { get; set; } = string.Empty;
        public string Justification { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class UserSummary
    {
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int ClearanceLevel { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class DashboardStats
    {
        public int StudentCount { get; set; }
        public int InstructorCount { get; set; }
        public int GradeCount { get; set; }
        public int AttendanceCount { get; set; }
    }

    public class StudentRecord
    {
        public int StudentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Program { get; set; } = string.Empty;
        public int YearLevel { get; set; }
        public int ClearanceLevel { get; set; }
    }

    public class Grade
    {
        public int GradeId { get; set; }
        public int StudentId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseTitle { get; set; } = string.Empty;
        public decimal Score { get; set; }
        public string Term { get; set; } = string.Empty;
    }

    public class AttendanceRecord
    {
        public int AttendanceId { get; set; }
        public int StudentId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
