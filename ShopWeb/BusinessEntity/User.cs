using System;

namespace BusinessEntity
{
    public class User
    {
        public int UserID { get; set; }
        public int? RoleID { get; set; }
        public int? EmployeeID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Thông tin từ bảng Employees
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }

        // Thông tin từ bảng Roles
        public string RoleName { get; set; }

        // Format hiển thị UserID
        public string DisplayUserID { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}