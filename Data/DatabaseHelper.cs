using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using SSRMS.Data.Models;

namespace SSRMS.Data
{
    public static class DatabaseHelper
    {
        // Update this connection string to match your SQL Server environment.
        private const string ConnectionString = "Data Source=.;Initial Catalog=SSRMS;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";

        public static User? AuthenticateUser(string username, string password)
        {
            const string sql = @"SELECT Username, PasswordHash, PasswordSalt, Role, ClearanceLevel, FullName, Department, Email
                                 FROM Users
                                 WHERE Username = @Username";

            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Username", username);
            connection.Open();

            using var reader = command.ExecuteReader();
            if (!reader.Read())
                return null;

            var storedHash = reader.GetString(reader.GetOrdinal("PasswordHash"));
            var storedSalt = reader.GetString(reader.GetOrdinal("PasswordSalt"));
            if (!VerifyPassword(password, storedSalt, storedHash))
                return null;

            return new User
            {
                Username = reader.GetString(reader.GetOrdinal("Username")),
                Role = reader.GetString(reader.GetOrdinal("Role")),
                ClearanceLevel = reader.GetInt32(reader.GetOrdinal("ClearanceLevel")),
                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                Department = reader.GetString(reader.GetOrdinal("Department")),
                Email = reader.GetString(reader.GetOrdinal("Email"))
            };
        }

        public static bool CreateAccount(string username, string password, string role, int clearance, string fullName, string department, string email)
        {
            var (hash, salt) = CreatePasswordHash(password);
            const string sql = @"INSERT INTO Users (Username, PasswordHash, PasswordSalt, Role, ClearanceLevel, FullName, Department, Email)
                                 VALUES (@Username, @Hash, @Salt, @Role, @ClearanceLevel, @FullName, @Department, @Email)";

            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Hash", hash);
            command.Parameters.AddWithValue("@Salt", salt);
            command.Parameters.AddWithValue("@Role", role);
            command.Parameters.AddWithValue("@ClearanceLevel", clearance);
            command.Parameters.AddWithValue("@FullName", fullName);
            command.Parameters.AddWithValue("@Department", department);
            command.Parameters.AddWithValue("@Email", email);

            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }

        public static bool AddRoleRequest(string username, string currentRole, string requestedRole, string justification)
        {
            const string sql = @"INSERT INTO RoleRequests (Username, CurrentRole, RequestedRole, Justification, RequestDate, Status)
                                 VALUES (@Username, @CurrentRole, @RequestedRole, @Justification, @RequestDate, 'Pending')";

            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@CurrentRole", currentRole);
            command.Parameters.AddWithValue("@RequestedRole", requestedRole);
            command.Parameters.AddWithValue("@Justification", justification);
            command.Parameters.AddWithValue("@RequestDate", DateTime.UtcNow);
            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }

        public static IEnumerable<RoleRequest> GetPendingRoleRequests()
        {
            const string sql = @"SELECT RequestId, Username, CurrentRole, RequestedRole, Justification, RequestDate, Status
                                 FROM RoleRequests
                                 WHERE Status = 'Pending'
                                 ORDER BY RequestDate DESC";

            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand(sql, connection);
            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new RoleRequest
                {
                    RequestId = reader.GetInt32(reader.GetOrdinal("RequestId")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    CurrentRole = reader.GetString(reader.GetOrdinal("CurrentRole")),
                    RequestedRole = reader.GetString(reader.GetOrdinal("RequestedRole")),
                    Justification = reader.GetString(reader.GetOrdinal("Justification")),
                    RequestDate = reader.GetDateTime(reader.GetOrdinal("RequestDate")),
                    Status = reader.GetString(reader.GetOrdinal("Status"))
                };
            }
        }

        public static bool UpdateRoleRequestStatus(int requestId, string status)
        {
            const string sql = @"UPDATE RoleRequests SET Status = @Status WHERE RequestId = @RequestId";
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Status", status);
            command.Parameters.AddWithValue("@RequestId", requestId);
            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }

        public static IEnumerable<UserSummary> GetAllUsers()
        {
            const string sql = @"SELECT Username, Role, ClearanceLevel, FullName, Department, Email FROM Users ORDER BY Username";
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand(sql, connection);
            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new UserSummary
                {
                    Username = reader.GetString(0),
                    Role = reader.GetString(1),
                    ClearanceLevel = reader.GetInt32(2),
                    FullName = reader.GetString(3),
                    Department = reader.GetString(4),
                    Email = reader.GetString(5)
                };
            }
        }

        public static IEnumerable<DashboardStats> LoadDashboardStats()
        {
            const string sql = @"SELECT
                                     (SELECT COUNT(*) FROM Students) AS StudentCount,
                                     (SELECT COUNT(*) FROM Instructors) AS InstructorCount,
                                     (SELECT COUNT(*) FROM Grades) AS GradeCount,
                                     (SELECT COUNT(*) FROM Attendance) AS AttendanceCount";
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand(sql, connection);
            connection.Open();

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                yield return new DashboardStats
                {
                    StudentCount = reader.GetInt32(0),
                    InstructorCount = reader.GetInt32(1),
                    GradeCount = reader.GetInt32(2),
                    AttendanceCount = reader.GetInt32(3)
                };
            }
        }

        public static IEnumerable<StudentRecord> SearchStudents(string filter)
        {
            const string sql = @"SELECT StudentId, FullName, Program, YearLevel, ClearanceLevel FROM Students
                                 WHERE FullName LIKE @Filter OR Program LIKE @Filter OR StudentId LIKE @Filter";
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Filter", "%" + filter + "%");
            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new StudentRecord
                {
                    StudentId = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    Program = reader.GetString(2),
                    YearLevel = reader.GetInt32(3),
                    ClearanceLevel = reader.GetInt32(4)
                };
            }
        }

        public static IEnumerable<Grade> LoadGradesForStudent(int studentId)
        {
            const string sql = @"SELECT GradeId, StudentId, CourseCode, CourseTitle, Score, Term FROM Grades
                                 WHERE StudentId = @StudentId";
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@StudentId", studentId);
            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new Grade
                {
                    GradeId = reader.GetInt32(0),
                    StudentId = reader.GetInt32(1),
                    CourseCode = reader.GetString(2),
                    CourseTitle = reader.GetString(3),
                    Score = reader.GetDecimal(4),
                    Term = reader.GetString(5)
                };
            }
        }

        public static IEnumerable<AttendanceRecord> LoadAttendanceForStudent(int studentId)
        {
            const string sql = @"SELECT AttendanceId, StudentId, CourseCode, Date, Status FROM Attendance
                                 WHERE StudentId = @StudentId";
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@StudentId", studentId);
            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new AttendanceRecord
                {
                    AttendanceId = reader.GetInt32(0),
                    StudentId = reader.GetInt32(1),
                    CourseCode = reader.GetString(2),
                    Date = reader.GetDateTime(3),
                    Status = reader.GetString(4)
                };
            }
        }

        private static (string Hash, string Salt) CreatePasswordHash(string password)
        {
            var saltBytes = RandomNumberGenerator.GetBytes(16);
            using var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256);
            var hashBytes = deriveBytes.GetBytes(32);
            return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
        }

        private static bool VerifyPassword(string password, string saltBase64, string hashBase64)
        {
            var saltBytes = Convert.FromBase64String(saltBase64);
            using var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256);
            var computedHash = deriveBytes.GetBytes(32);
            return CryptographicOperations.FixedTimeEquals(computedHash, Convert.FromBase64String(hashBase64));
        }
    }
}
