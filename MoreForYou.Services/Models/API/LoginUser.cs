using System;
using System.Collections.Generic;
using System.Text;

namespace MoreForYou.Services.Models.API
{
    public class LoginUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public long UserNumber { get; set; }

        public string PositionName { get; set; }
        public string DepartmentName { get; set; }
        public string BirthDate { get; set; }
        public string JoinDate { get; set; }
        public string Gender { get; set; }

        public string MaritalStatus { get; set; }
        public string Entity { get; set; }
        public string Nationality { get; set; }

        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public string Collar { get; set; }

        public long SapNumber { get; set; }

        public string Id { get; set; }
        public string SupervisorName { get; set; }
        public string ProfilePicture { get; set; }

        public string WorkDuration { get; set; }
        public bool hasRequests { get; set; }
        public long PendingRequestsCount { get; set; }

        public bool IsTheGroupCreator { get; set; }

        public bool IsAdmin { get; set; }

        public bool HasRoles { get; set; }
        public string Country { get; set; }

        public string State { get; set; }
        public string ProfilePictureAPI { get; set; }

        //public string? Token { get; set; }
    }


    public class EmployeeData
    {
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public long EmployeeNumber { get; set; }

        public string PositionName { get; set; }
        public string DepartmentName { get; set; }
        public string BirthDate { get; set; }
        public string JoinDate { get; set; }

        public string Company { get; set; }

        public string PhoneNumber { get; set; }

        public string Collar { get; set; }

        public long SapNumber { get; set; }

        public string ProfilePicture { get; set; }

        public int WorkDuration { get; set; }
        public string Country { get; set; }

        public string State { get; set; }
        public string ProfilePictureAPI { get; set; }
    }

    public class EmployeeUserData
    {
        public long Number { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class UserData
    {
        public long UserNumber { get; set; }
        public TokenModel TokenModel { get; set; }

    }
}
