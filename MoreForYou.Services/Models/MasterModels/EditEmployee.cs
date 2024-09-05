using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.MasterModels
{
   public class EditEmployee
    {
        public long EmployeeNumber { get; set; }
        public string FullName { get; set; }
        public long SapNumber { get; set; }
        public string Id { get; set; }
        public int Gender { get; set; }
        public DateTime BirthDate { get; set; }

        public DateTime JoinDate { get; set; }
        public int MaritalStatus { get; set; }
        public long DepartmentId { get; set; }
        public long PositionId { get; set; }
        public int CompanyId { get; set; }
        public int Collar { get; set; }
        public bool IsDeptManager { get; set; }

        public bool DirectEmployee { get; set; }
        public string Address { get; set; }

        public long NationalityId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
