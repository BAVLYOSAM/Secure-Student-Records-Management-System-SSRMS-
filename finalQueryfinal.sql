Create database finalproject3;




--======================
--encrption
--======================
--master key
CREATE MASTER KEY
ENCRYPTION BY PASSWORD = 'Bavly@123';
GO
--certificate
CREATE CERTIFICATE SRMSCert
WITH SUBJECT = 'SRMS Encryption Certificate';
GO


CREATE SYMMETRIC KEY SRMSSymKey
WITH ALGORITHM = AES_256
ENCRYPTION BY CERTIFICATE SRMSCert;
GO


--======================
--create tables
--======================


--create student tabel
CREATE TABLE Student (
    StudentID INT IDENTITY PRIMARY KEY,
    EncryptedStudentID VARBINARY(MAX) NOT NULL, -- AES
    FullName NVARCHAR(100),
    Email NVARCHAR(100),
    Phone VARBINARY(MAX), -- AES
    DOB DATE,
    Department NVARCHAR(50),
    ClearanceLevel INT -- MLS
);
---INSERT 
OPEN SYMMETRIC KEY SRMSSymKey
DECRYPTION BY CERTIFICATE SRMSCert;
GO
INSERT INTO Student (
    EncryptedStudentID,
    FullName,
    Email,
    Phone,
    DOB,
    Department,
    ClearanceLevel
)
VALUES (
    EncryptByKey(Key_GUID('SRMSSymKey'), CAST(1 AS NVARCHAR(20))), -- student logical ID
    'Sara Amgad',
    'sara@mail.com',
    EncryptByKey(Key_GUID('SRMSSymKey'), '01012345678'),
    '2005-08-03',
    'CS',
    1
);
GO
CLOSE SYMMETRIC KEY SRMSSymKey;
GO
EXEC Instructor_InsertGrade
    @Username  = 'instructor1',
    @StudentID = 4,
    @CourseID  = 1,
    @Grade     = 95;


  SELECT name
FROM sys.procedures
WHERE name LIKE '%Grade%';
  --create instructor table 
CREATE TABLE Instructor (
    InstructorID INT IDENTITY PRIMARY KEY,
    FullName NVARCHAR(100),
    Email NVARCHAR(100),
    ClearanceLevel INT
);

--INSERT
INSERT INTO Instructor (
    FullName,
    Email,
    ClearanceLevel
)
VALUES
('Dr. Ahmed Hassan', 'ahmed@uni.edu', 3),
('Dr. Mona Ali', 'mona@uni.edu', 3);
GO

--create course table
CREATE TABLE Course (
    CourseID INT IDENTITY PRIMARY KEY,
    CourseName NVARCHAR(100),
    Description NVARCHAR(MAX),
    PublicInfo NVARCHAR(MAX)
);

--INSERT
INSERT INTO Course (CourseName, Description, PublicInfo)
VALUES
(
    'Database Security',
    'Advanced database security concepts including RBAC and MLS',
    'Open for all students'
),
(
    'Operating Systems',
    'Processes, memory management, scheduling',
    'Basic course info'
);
GO


--create grandes table 
CREATE TABLE Grades (
    GradeID INT IDENTITY PRIMARY KEY,
    StudentID INT,
    CourseID INT,
    GradeValue VARBINARY(MAX), -- AES
    DateEntered DATETIME,
    FOREIGN KEY (StudentID) REFERENCES Student(StudentID),
    FOREIGN KEY (CourseID) REFERENCES Course(CourseID)
);

--insert and use encrption and dycrption
OPEN SYMMETRIC KEY SRMSSymKey
DECRYPTION BY CERTIFICATE SRMSCert;

INSERT INTO Grades (StudentID, CourseID, GradeValue, DateEntered, ClassificationLevel)
VALUES (
    1,
    1,
    EncryptByKey(Key_GUID('SRMSSymKey'), CAST(95 AS NVARCHAR)),
    GETDATE(),
    3
);

CLOSE SYMMETRIC KEY SRMSSymKey;



--create attendance table 
CREATE TABLE Attendance (
    AttendanceID INT IDENTITY PRIMARY KEY,
    StudentID INT,
    CourseID INT,
    Status BIT,
    DateRecorded DATETIME,
    FOREIGN KEY (StudentID) REFERENCES Student(StudentID),
    FOREIGN KEY (CourseID) REFERENCES Course(CourseID)
);

INSERT INTO Attendance (StudentID, CourseID, Status, DateRecorded)
VALUES (3,1, 1, GETDATE());

SELECT * FROM Attendance;


--create user table
CREATE TABLE Users (
    Username NVARCHAR(50) PRIMARY KEY,
    Password VARBINARY(MAX), -- Hashed / Encrypted
    Role NVARCHAR(20),
    ClearanceLevel INT
);
---INSERT 
INSERT INTO Users (Username, Password, Role, ClearanceLevel)
VALUES
('AdminUser', HASHBYTES('SHA2_256','admin123'), 'Admin', 4),
('InstructorUser', HASHBYTES('SHA2_256','-'), 'Instructor', 3),
('TAUser', HASHBYTES('SHA2_256','ta123'), 'TA', 2),
('StudentUser', HASHBYTES('SHA2_256','stud123'), 'Student', 1),
('GuestUser', HASHBYTES('SHA2_256','guest123'), 'Guest', 0);
GO


-- Create new student user for demo
INSERT INTO Users (Username, Password, Role, ClearanceLevel)
VALUES (
    'student_demo',
    HASHBYTES('SHA2_256', 'demo123'),
    'Student',
    1
);

--======================
--create roles
--======================
CREATE ROLE AdminRole;
CREATE ROLE InstructorRole;
CREATE ROLE TARole;
CREATE ROLE StudentRole;
CREATE ROLE GuestRole;
GO
--login
CREATE LOGIN AdminUser WITH PASSWORD = 'Admin@123';
CREATE LOGIN InstructorUser WITH PASSWORD = 'Inst@123';
CREATE LOGIN TAUser WITH PASSWORD = 'TA@123';
CREATE LOGIN StudentUser WITH PASSWORD = 'Stud@123';
CREATE LOGIN GuestUser WITH PASSWORD = 'Guest@123';
GO
--users
CREATE USER AdminUser FOR LOGIN AdminUser;
CREATE USER InstructorUser FOR LOGIN InstructorUser;
CREATE USER TAUser FOR LOGIN TAUser;
CREATE USER StudentUser FOR LOGIN StudentUser;
CREATE USER GuestUser FOR LOGIN GuestUser;
GO
--exec
EXEC sp_addrolemember 'AdminRole', 'AdminUser';
EXEC sp_addrolemember 'InstructorRole', 'InstructorUser';
EXEC sp_addrolemember 'TARole', 'TAUser';
EXEC sp_addrolemember 'StudentRole', 'StudentUser';
EXEC sp_addrolemember 'GuestRole', 'GuestUser';

--======================
--access control
--======================

--deny for public
DENY SELECT,INSERT,UPDATE,DELETE ON Student TO PUBLIC;
DENY SELECT,INSERT,UPDATE,DELETE ON Grades TO PUBLIC;
DENY SELECT,INSERT,UPDATE,DELETE ON Attendance TO PUBLIC;
DENY SELECT,INSERT,UPDATE,DELETE ON Users TO PUBLIC;

--Admin
GRANT SELECT, INSERT, UPDATE, DELETE ON Student TO AdminRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON Instructor TO AdminRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON Course TO AdminRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON Grades TO AdminRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON Attendance TO AdminRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON Users TO AdminRole;
--instructor
GRANT SELECT, INSERT, UPDATE ON Grades TO InstructorRole;
GRANT SELECT ON Attendance TO InstructorRole;
GRANT SELECT ON Course TO InstructorRole;

DENY DELETE ON Users TO InstructorRole;
DENY INSERT ON Users TO InstructorRole;
DENY UPDATE ON Users TO InstructorRole;
--TA
GRANT SELECT, INSERT, UPDATE ON Attendance TO TARole;
GRANT SELECT ON Student TO TARole;
GRANT SELECT ON Course TO TARole;

DENY SELECT, INSERT, UPDATE, DELETE ON Grades TO TARole;
--student

GRANT SELECT ON Student TO StudentRole;
GRANT SELECT ON Attendance TO StudentRole;
GRANT SELECT ON Grades TO StudentRole;
--guest
GRANT SELECT ON Course TO GuestRole;

DENY SELECT ON Student TO GuestRole;
DENY SELECT ON Grades TO GuestRole;
DENY SELECT ON Attendance TO GuestRole;
--=============================
--VIEWS
--=============================
--ADMIN 
GO
CREATE VIEW vw_Admin_Students
AS
SELECT StudentID, FullName, Email, Department, ClearanceLevel
FROM Student;
GO

GO
CREATE VIEW vw_Admin_Grades
AS
SELECT *
FROM Grades;
GO

CREATE VIEW vw_Admin_Attendance
AS
SELECT *
FROM Attendance;
GO

CREATE VIEW vw_Admin_Users
AS
SELECT Username, Role, ClearanceLevel
FROM Users;
GO
--====================================================
--INTSRUCTOR 

CREATE VIEW vw_Instructor_Grades
AS
SELECT g.GradeID, g.CourseID, g.GradeValue, g.DateEntered
FROM Grades g;
GO

CREATE VIEW vw_Instructor_Attendance
AS
SELECT a.AttendanceID, a.StudentID, a.CourseID, a.Status, a.DateRecorded
FROM Attendance a;
GO

CREATE VIEW vw_Instructor_Courses
AS
SELECT CourseID, CourseName, Description
FROM Course;
GO

--====================================================================
--TA

CREATE VIEW vw_TA_Students
AS
SELECT StudentID, FullName, Department
FROM Student;
GO

CREATE VIEW vw_TA_Attendance
AS
SELECT AttendanceID, StudentID, CourseID, Status, DateRecorded
FROM Attendance;
GO
--===========================================================
--STUDENT 
CREATE VIEW vw_Student_OwnProfile
AS
SELECT 
    s.StudentID,
    s.FullName,
    s.Email,
    s.Department
FROM Student s
JOIN Users u
    ON u.ClearanceLevel = s.ClearanceLevel;
GO

CREATE VIEW vw_Student_OwnGrades
AS
SELECT g.GradeID, g.CourseID, g.GradeValue
FROM Grades g
JOIN Student s
    ON g.StudentID = s.StudentID;
GO

CREATE VIEW vw_Student_OwnAttendance
AS
SELECT a.AttendanceID, a.CourseID, a.Status, a.DateRecorded
FROM Attendance a
JOIN Student s
    ON a.StudentID = s.StudentID;
GO
--======================================================
--GUEST
CREATE VIEW vw_Guest_Courses
AS
SELECT CourseID, CourseName, PublicInfo
FROM Course;
GO

--============================
--ACCESS CONTROL TO VIEWS
--============================
GRANT SELECT ON vw_Admin_Users TO Adminrole;
GRANT SELECT ON vw_Admin_Attendance TO Adminrole;
GRANT SELECT ON vw_Admin_Grades TO Adminrole;
GRANT SELECT ON vw_Admin_Students TO Adminrole;


GRANT SELECT ON vw_Instructor_Grades TO InstructorRole;
GRANT SELECT ON vw_Instructor_Attendance TO InstructorRole;
GRANT SELECT ON vw_Instructor_Courses TO InstructorRole;

-- TA
GRANT SELECT ON vw_TA_Students TO TARole;
GRANT SELECT ON vw_TA_Attendance TO TARole;

-- Student
GRANT SELECT ON vw_Student_OwnProfile TO StudentRole;
GRANT SELECT ON vw_Student_OwnGrades TO StudentRole;
GRANT SELECT ON vw_Student_OwnAttendance TO StudentRole;

-- Guest
GRANT SELECT ON vw_Guest_Courses TO GuestRole;
--===================================================
--DENY
--===================================================
-- ADMIN VIEWS
REVOKE SELECT ON vw_Admin_Students    TO PUBLIC;
REVOKE SELECT ON vw_Admin_Grades      TO PUBLIC;
REVOKE SELECT ON vw_Admin_Attendance  TO PUBLIC;
REVOKE SELECT ON vw_Admin_Users       TO PUBLIC;
DENY SELECT ON vw_Admin_Students TO StudentRole, TARole, InstructorRole, GuestRole;
DENY SELECT ON vw_Admin_Grades TO StudentRole, TARole, InstructorRole, GuestRole;
DENY SELECT ON vw_Admin_Attendance TO StudentRole, TARole, InstructorRole, GuestRole;


-- INSTRUCTOR VIEWS
REVOKE SELECT ON vw_Instructor_Grades      TO PUBLIC;
REVOKE SELECT ON vw_Instructor_Attendance  TO PUBLIC;
REVOKE SELECT ON vw_Instructor_Courses     TO PUBLIC;

-- TA VIEWS
REVOKE SELECT ON vw_TA_Students     TO PUBLIC;
REVOKE SELECT ON vw_TA_Attendance   TO PUBLIC;

-- STUDENT VIEWS
REVOKE SELECT ON vw_Student_OwnProfile     TO PUBLIC;
REVOKE SELECT ON vw_Student_OwnGrades      TO PUBLIC;
REVOKE SELECT ON vw_Student_OwnAttendance  TO PUBLIC;

-- GUEST VIEW
REVOKE SELECT ON vw_Guest_Courses TO PUBLIC;
GO
--======================================================================================
--TEST ACCESS CONTROL
--========================================================
EXECUTE AS USER = 'AdminUser';
SELECT * FROM vw_Admin_Students;   -- ✅
SELECT * FROM vw_Admin_Grades;     -- ✅
SELECT * FROM vw_Guest_Courses;    
REVERT;

EXECUTE AS USER = 'StudentUser';
SELECT * FROM vw_Admin_Students; --  Permission Denied
SELECT * FROM vw_Student_OwnProfile; -- 
REVERT;

EXECUTE AS USER = 'GuestUser';
SELECT * FROM vw_Guest_Courses; -- ✔
SELECT * FROM vw_Student_OwnGrades; -- Permission Denied
REVERT;

EXECUTE AS USER = 'InstructorUser';

--  Allowed
SELECT * FROM vw_Instructor_Grades;
SELECT * FROM vw_Instructor_Attendance;
SELECT * FROM vw_Instructor_Courses;

-- Not Allowed
SELECT * FROM vw_Admin_Students;     -- Permission Denied
SELECT * FROM vw_TA_Students;        -- Permission Denied
SELECT * FROM vw_Student_OwnProfile; -- Permission Denied

REVERT;

EXECUTE AS USER = 'TAUser';

-- Allowed
SELECT * FROM vw_TA_Students;
SELECT * FROM vw_TA_Attendance;

-- Not Allowed
SELECT * FROM vw_Instructor_Grades;  -- Permission Denied
SELECT * FROM vw_Admin_Grades;       -- Permission Denied
SELECT * FROM vw_Admin_Users;        -- Permission Denied

REVERT;

--======================
--poredcure
--======================
--GetUserRole
GO
CREATE PROCEDURE GetUserRole
    @Username NVARCHAR(50),
    @Role NVARCHAR(20) OUTPUT,
    @Clearance INT OUTPUT
AS
BEGIN
    SELECT 
        @Role = Role,
        @Clearance = ClearanceLevel
    FROM Users
    WHERE Username = @Username;
END;
GO



--View Own Profile (Admin / Instructor / TA / Student)
GO
CREATE PROCEDURE ViewOwnProfile
    @Username NVARCHAR(50),
    @StudentID INT
AS
BEGIN
    DECLARE @Role NVARCHAR(20), @Clearance INT;

    EXEC GetUserRole @Username, @Role OUTPUT, @Clearance OUTPUT;

    IF @Role IN ('Admin', 'Instructor', 'TA', 'Student')
        SELECT StudentID, FullName, Email, DOB, Department
        FROM Student
        WHERE StudentID = @StudentID;
    ELSE
        RAISERROR('Access Denied', 16, 1);
END;
GO


--View Grades (Admin / Instructor)
GO
CREATE OR ALTER PROCEDURE ViewGrades
    @Username NVARCHAR(50)
AS
BEGIN
    DECLARE @Role NVARCHAR(20), @Clearance INT;

    EXEC GetUserRole @Username, @Role OUTPUT, @Clearance OUTPUT;

    IF @Role IN ('Admin', 'Instructor')
    BEGIN
        OPEN SYMMETRIC KEY SRMSSymKey
        DECRYPTION BY CERTIFICATE SRMSCert;

        SELECT 
            GradeID,
            StudentID,
            CourseID,
            CONVERT(DECIMAL, DecryptByKey(GradeValue)) AS GradeValue,
            DateEntered
        FROM Grades;

        CLOSE SYMMETRIC KEY SRMSSymKey;
    END
    ELSE
        RAISERROR('Access Denied: Cannot view grades', 16, 1);
END;
GO

EXEC ViewGrades @Username = 'AdminUser';      -- ✅
EXEC ViewGrades @Username = 'InstructorUser';-- ✅
EXEC ViewGrades @Username = 'TAUser';         -- ❌ Access Denied
EXEC ViewGrades @Username = 'StudentUser';    -- ❌ Access Denied







--view Own Grades (Student)
GO
CREATE PROCEDURE ViewMyGrades
    @Username NVARCHAR(50),
    @StudentID INT
AS
BEGIN
    DECLARE @Role NVARCHAR(20), @Clearance INT;

    EXEC GetUserRole @Username, @Role OUTPUT, @Clearance OUTPUT;

    IF @Role = 'Student'
    BEGIN
        OPEN SYMMETRIC KEY SRMSSymKey
        DECRYPTION BY CERTIFICATE SRMSCert;

        SELECT 
            CourseID,
            CONVERT(DECIMAL, DecryptByKey(GradeValue)) AS GradeValue
        FROM Grades
        WHERE StudentID = @StudentID;

        CLOSE SYMMETRIC KEY SRMSSymKey;
    END
    ELSE
        RAISERROR('Access Denied', 16, 1);
END;
GO



--Manage Attendance (Admin / Instructor / TA)
GO
CREATE PROCEDURE UpdateAttendance
    @Username NVARCHAR(50),
    @AttendanceID INT,
    @Status BIT
AS
BEGIN
    DECLARE @Role NVARCHAR(20), @Clearance INT;

    EXEC GetUserRole @Username, @Role OUTPUT, @Clearance OUTPUT;

    IF @Role IN ('Admin', 'Instructor', 'TA')
        UPDATE Attendance
        SET Status = @Status
        WHERE AttendanceID = @AttendanceID;
    ELSE
        RAISERROR('Access Denied', 16, 1);
END;
GO



--View Public Course Info
GO
CREATE PROCEDURE ViewPublicCourses
AS
BEGIN
    SELECT CourseID, CourseName, PublicInfo
    FROM Course;
END;
GO

--instructor insert
CREATE PROCEDURE Instructor_InsertGrade
    @Username NVARCHAR(50),
    @StudentID INT,
    @CourseID INT,
    @Grade INT
AS
BEGIN
    DECLARE @Clearance INT;

    SELECT @Clearance = ClearanceLevel
    FROM Users
    WHERE Username = @Username;

    -- Grades = Secret (3)
    IF @Clearance < 3
    BEGIN
        RAISERROR('Access Denied: MLS No Write Down', 16, 1);
        RETURN;
    END

    OPEN SYMMETRIC KEY SRMSSymKey
    DECRYPTION BY CERTIFICATE SRMSCert;

    INSERT INTO Grades (StudentID, CourseID, GradeValue, DateEntered, ClassificationLevel)
    VALUES (
        @StudentID,
        @CourseID,
        EncryptByKey(Key_GUID('SRMSSymKey'), CAST(@Grade AS NVARCHAR)),
        GETDATE(),
        3
    );

    CLOSE SYMMETRIC KEY SRMSSymKey;
END;
-----------------------------------------------------
--procedure for gui
CREATE PROCEDURE SubmitRoleRequest
AS
EXEC RequestRoleUpgrade


--istructor view
CREATE PROCEDURE Instructor_ViewGrades
    @Username NVARCHAR(50)
AS
BEGIN
    DECLARE @Clearance INT;

    SELECT @Clearance = ClearanceLevel
    FROM Users
    WHERE Username = @Username;

    IF @Clearance < 3
    BEGIN
        RAISERROR('Access Denied: No Read Up', 16, 1);
        RETURN;
    END

    OPEN SYMMETRIC KEY SRMSSymKey
    DECRYPTION BY CERTIFICATE SRMSCert;

    SELECT 
        StudentID,
        CourseID,
        CONVERT(INT, DecryptByKey(GradeValue)) AS Grade,
        DateEntered
    FROM Grades;

    CLOSE SYMMETRIC KEY SRMSSymKey;
END;
--istructor view attendance
CREATE PROCEDURE Instructor_ViewAttendance
AS
BEGIN
    SELECT StudentID, CourseID, Status, DateRecorded
    FROM Attendance;
END;

--student view grade
CREATE PROCEDURE Student_ViewGrades
    @Username NVARCHAR(50)
AS
BEGIN
    DECLARE @StudentID INT;

    SELECT @StudentID = s.StudentID
    FROM Student s
    JOIN Users u ON u.ClearanceLevel = s.ClearanceLevel
    WHERE u.Username = @Username;

    OPEN SYMMETRIC KEY SRMSSymKey
    DECRYPTION BY CERTIFICATE SRMSCert;

    SELECT 
        CourseID,
        CONVERT(INT, DecryptByKey(GradeValue)) AS Grade
    FROM Grades
    WHERE StudentID = @StudentID;

    CLOSE SYMMETRIC KEY SRMSSymKey;
END;
----student view attendance
CREATE PROCEDURE Student_ViewAttendance
    @Username NVARCHAR(50)
AS
BEGIN
    DECLARE @StudentID INT;

    SELECT @StudentID = s.StudentID
    FROM Student s
    JOIN Users u ON u.ClearanceLevel = s.ClearanceLevel
    WHERE u.Username = @Username;

    SELECT CourseID, Status, DateRecorded
    FROM Attendance
    WHERE StudentID = @StudentID;
END;
--virew profile
CREATE PROCEDURE Student_ViewProfile
    @Username NVARCHAR(50)
AS
BEGIN
    SELECT s.StudentID, s.FullName, s.Email, s.DOB, s.Department
    FROM Student s
    JOIN Users u ON u.ClearanceLevel = s.ClearanceLevel
    WHERE u.Username = @Username;
END;

--ta
GO
CREATE PROCEDURE TA_ViewAttendance
    @Username NVARCHAR(50)
AS
BEGIN
    DECLARE @Role NVARCHAR(20)

    SELECT @Role = Role
    FROM Users
    WHERE Username = @Username

    IF @Role <> 'TA'
    BEGIN
        RAISERROR('Access Denied',16,1)
        RETURN
    END

    SELECT StudentID, CourseID, Status, DateRecorded
    FROM Attendance
END
GO
--
GO
CREATE PROCEDURE Guest_ViewCourses
AS
BEGIN
    SELECT CourseID, CourseName, PublicInfo
    FROM Course
END
GO


--login
CREATE OR ALTER PROCEDURE sp_Login
    @Username NVARCHAR(50),
    @hashedPassword VARBINARY(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Role,
        ClearanceLevel
    FROM Users
    WHERE Username = @Username
      AND Password = @hashedPassword;
END;
GO
-------------------------------------------
UPDATE Users SET ClearanceLevel = 4 WHERE Role = 'Admin';
UPDATE Users SET ClearanceLevel = 3 WHERE Role = 'Instructor';
UPDATE Users SET ClearanceLevel = 2 WHERE Role = 'TA';
UPDATE Users SET ClearanceLevel = 1 WHERE Role = 'Student';
UPDATE Users SET ClearanceLevel = 0 WHERE Role = 'Guest';

--======================
--multilevel-security
--======================
--Grades = Secret
ALTER TABLE Grades
ADD ClassificationLevel INT DEFAULT 3;
--Attendance = Secret
ALTER TABLE Attendance
ADD ClassificationLevel INT DEFAULT 3;
--Student = Confidential
ALTER TABLE Student
ADD ClassificationLevel INT DEFAULT 2;
 --Course = Unclassified
ALTER TABLE Course
ADD ClassificationLevel INT DEFAULT 1;


--no read up
GO
CREATE PROCEDURE MLS_ViewGrades
    @Username NVARCHAR(50)
AS
BEGIN
    DECLARE @UserClearance INT;

    SELECT @UserClearance = ClearanceLevel
    FROM Users
    WHERE Username = @Username;

    -- No Read Up
    IF @UserClearance < 3
    BEGIN
        RAISERROR('MLS Violation: No Read Up', 16, 1);
        RETURN;
    END

    OPEN SYMMETRIC KEY SRMSSymKey
    DECRYPTION BY CERTIFICATE SRMSCert;

    SELECT 
        StudentID,
        CourseID,
        CONVERT(DECIMAL, DecryptByKey(GradeValue)) AS Grade
    FROM Grades;

    CLOSE SYMMETRIC KEY SRMSSymKey;
END;
GO

--no write down
GO
CREATE PROCEDURE MLS_InsertGrade
    @Username NVARCHAR(50),
    @StudentID INT,
    @CourseID INT,
    @Grade DECIMAL
AS
BEGIN
    DECLARE @UserClearance INT;
    DECLARE @DataLevel INT = 3; -- Secret

    SELECT @UserClearance = ClearanceLevel
    FROM Users
    WHERE Username = @Username;

    -- No Write Down
    IF @UserClearance > @DataLevel
    BEGIN
        RAISERROR('MLS Violation: No Write Down', 16, 1);
        RETURN;
    END

    OPEN SYMMETRIC KEY SRMSSymKey
    DECRYPTION BY CERTIFICATE SRMSCert;

    INSERT INTO Grades (StudentID, CourseID, GradeValue, DateEntered, ClassificationLevel)
    VALUES (
        @StudentID,
        @CourseID,
        EncryptByKey(Key_GUID('SRMSSymKey'), CAST(@Grade AS NVARCHAR)),
        GETDATE(),
        3
    );

    CLOSE SYMMETRIC KEY SRMSSymKey;
END;
GO

--view
GO
CREATE VIEW V_MLS_StudentData
AS
SELECT *
FROM Student
WHERE ClassificationLevel <=
    (SELECT ClearanceLevel FROM Users WHERE Username = USER_NAME());

--======================
--Inference Control
--======================
--Implement Query Set Size Control (minimum group size = 3)
GO
CREATE PROCEDURE IC_ViewAttendance
    @Username NVARCHAR(50),
    @CourseID INT
AS
BEGIN
    DECLARE @Count INT;

    SELECT @Count = COUNT(*)
    FROM Attendance
    WHERE CourseID = @CourseID;

    IF @Count < 3
    BEGIN
        RAISERROR('Inference Control Violation: Query Set Size < 3', 16, 1);
        RETURN;
    END

    SELECT StudentID, Status, DateRecorded
    FROM Attendance
    WHERE CourseID = @CourseID;
END;
GO

--Student Attendance View
GO
CREATE VIEW V_StudentAttendance
AS
SELECT 
    StudentID,
    CourseID,
    Status,
    DateRecorded
FROM Attendance
WHERE StudentID =
(
    SELECT StudentID
    FROM Student
    WHERE StudentID = CAST(SESSION_CONTEXT(N'StudentID') AS INT)
);
--in gui
EXEC sys.sp_set_session_context 'StudentID', @StudentID;

 
 --Create restricted views for TA/Student
GO
CREATE VIEW V_TA_Students
AS
SELECT StudentID, FullName, Department
FROM Student;

--Block aggregate results that reveal identity
GO
CREATE PROCEDURE IC_AvgGrades
    @Username NVARCHAR(50),
    @CourseID INT
AS
BEGIN
    RAISERROR('Inference Control: Aggregate queries are restricted', 16, 1);
END;
GO
--failur test
SELECT AVG(GradeValue)
FROM Grades
WHERE CourseID = 5 AND StudentID <> 101;

--======================
--flow control
--======================
--deny leak information
GO
CREATE PROCEDURE FC_ExportGrades
    @Username NVARCHAR(50)
AS
BEGIN
    DECLARE @UserClearance INT;

    SELECT @UserClearance = ClearanceLevel
    FROM Users
    WHERE Username = @Username;

    -- Grades = Secret (3)
    IF @UserClearance < 3
    BEGIN
        RAISERROR('Flow Control Violation: Downward Flow Blocked', 16, 1);
        RETURN;
    END

    RAISERROR('Export of Secret data is not allowed', 16, 1);
END;
GO
--======================
--test cases
--======================
--login
--======================

--Login Form
GO
CREATE PROCEDURE sp_Login
    @username NVARCHAR(50),
    @hashedPassword VARBINARY(MAX)
AS
BEGIN
    SELECT Role, ClearanceLevel
    FROM Users
    WHERE Username = @username
      AND Password = @hashedPassword;
END;
GO
--======================
--create RoleRequests Table
--======================
GO
CREATE TABLE RoleRequests (
    RequestID INT IDENTITY PRIMARY KEY,
    Username NVARCHAR(50),
    CurrentRole NVARCHAR(20),
    RequestedRole NVARCHAR(20),
    Reason NVARCHAR(255),
    Comments NVARCHAR(255),
    Status NVARCHAR(20) DEFAULT 'Pending',
    RequestDate DATETIME DEFAULT GETDATE()
);



--======================
--create dashboard
--======================
GO
CREATE VIEW AdminRoleRequestDashboard AS
SELECT 
    rr.RequestID,
    rr.Username,
    u.Role AS CurrentRole,
    rr.RequestedRole,
    rr.Reason,
    rr.Comments,
    rr.RequestDate,
    rr.Status
FROM RoleRequests rr
JOIN Users u ON rr.Username = u.Username;
--test
SELECT * FROM AdminRoleRequestDashboard;

--requestrole
EXEC RequestRoleUpgrade
 'student1', 'TA', 'Lab assistance', 'Good GPA';
 --insert student
 EXEC RequestRoleUpgrade
    'student1',
    'TA',
    'I help in labs',
    'Good GPA';

revert;


--======================
--Request Role Upgrade (Student / TA)
--======================
--create poredcure for request role
--using for block student from touch user and role 
GO
CREATE PROCEDURE RequestRoleUpgrade
    @Username NVARCHAR(50),
    @RequestedRole NVARCHAR(20),
    @Reason NVARCHAR(255),
    @Comments NVARCHAR(255)
AS
BEGIN
    DECLARE @CurrentRole NVARCHAR(20);

    SELECT @CurrentRole = Role
    FROM Users
    WHERE Username = @Username;

    INSERT INTO RoleRequests
    (Username, CurrentRole, RequestedRole, Reason, Comments)
    VALUES
    (@Username, @CurrentRole, @RequestedRole, @Reason, @Comments);
END;


--======================
--Admin Dashboard
--======================
GO
CREATE VIEW AdminPendingRoleRequests AS
SELECT *
FROM AdminRoleRequestDashboard
WHERE Status = 'Pending';


CREATE PROCEDURE GetPendingRoleRequests
AS
BEGIN
    SELECT *
    FROM AdminPendingRoleRequests;
END;


--admin stored porudcure
GO
GO
CREATE OR ALTER PROCEDURE ApproveRoleRequest
    @RequestID INT
AS
BEGIN
    DECLARE @Username NVARCHAR(50);
    DECLARE @CurrentRole NVARCHAR(20);
    DECLARE @NewRole NVARCHAR(20);

    -- Get request info
    SELECT 
        @Username = Username,
        @CurrentRole = CurrentRole,
        @NewRole = RequestedRole
    FROM RoleRequests
    WHERE RequestID = @RequestID;

    -- Update role in Users table
    UPDATE Users
    SET Role = @NewRole
    WHERE Username = @Username;

    -- Update SQL Roles
    IF @CurrentRole = 'Student' AND @NewRole = 'TA'
    BEGIN
        EXEC sp_droprolemember 'StudentRole', @Username;
        EXEC sp_addrolemember 'TARole', @Username;
    END

    IF @CurrentRole = 'TA' AND @NewRole = 'Instructor'
    BEGIN
        EXEC sp_droprolemember 'TARole', @Username;
        EXEC sp_addrolemember 'InstructorRole', @Username;
    END

    -- Mark request as approved
    UPDATE RoleRequests
    SET Status = 'Approved'
    WHERE RequestID = @RequestID;
END;
GO


--for deny stored poreduce
GO
CREATE PROCEDURE DenyRoleRequest
    @RequestID INT
AS
BEGIN
    UPDATE RoleRequests
    SET Status = 'Denied'
    WHERE RequestID = @RequestID;
END;


--test dashboard
--insert
EXEC RequestRoleUpgrade
    @Username = 'student1',
    @RequestedRole = 'TA',
    @Reason = 'I assist instructors in labs',
    @Comments = 'Completed prerequisite courses';

	SELECT * FROM RoleRequests;

SELECT 
    RequestID,
    Username,
    CurrentRole,
    RequestedRole,
    Reason,
    RequestDate,
    Status
FROM RoleRequests
WHERE Status = 'Pending';
--accept from admin
EXEC ApproveRoleRequest @RequestID = 1;
--deny from admin
EXEC DenyRoleRequest @RequestID = 1;

--now he is admin 
SELECT Username, Role FROM Users WHERE Username = 'student1';

------------------------------
--test gui


SELECT * FROM RoleRequests;



EXEC SubmitRoleRequest
    @Username = 'student1',
    @RequestedRole = 'TA',
    @Reason = 'Lab assistance',
    @Comments = 'Available';


	SELECT * 
FROM RoleRequests
WHERE Status = 'Pending';



EXEC SubmitRoleRequest
  @Username = 'student1',
  @RequestedRole = 'TA',
  @Reason = 'Lab assistance',
  @Comments = 'Available this term';





SELECT name
FROM sys.procedures
WHERE name LIKE '%Attendance%';



CREATE PROCEDURE sp_TA_InsertAttendance
    @StudentID INT,
    @CourseID INT,
    @Status BIT
AS
BEGIN
    INSERT INTO Attendance
    (
        StudentID,
        CourseID,
        Status,
        DateRecorded
    )
    VALUES
    (
        @StudentID,
        @CourseID,
        @Status,
        GETDATE()
    );
END;
GO



EXEC sp_TA_InsertAttendance
    @StudentID = 1,
    @CourseID = 1,
    @Status = 1;




  ('AdminUser', HASHBYTES('SHA2_256','admin123'), 'Admin', 4),
('InstructorUser', HASHBYTES('SHA2_256','inst123'), 'Instructor', 3),
('TAUser', HASHBYTES('SHA2_256','ta123'), 'TA', 2),
('StudentUser', HASHBYTES('SHA2_256','stud123'), 'Student', 1),
('GuestUser', HASHBYTES('SHA2_256','guest123'), 'Guest', 0);