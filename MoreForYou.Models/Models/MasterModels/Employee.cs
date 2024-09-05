using Microsoft.EntityFrameworkCore;
using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
    //[Index(nameof(Employee.SapNumber), IsUnique = true)]
    //[Index(nameof(Employee.Id), IsUnique = true)]
    //[Index(nameof(Employee.PhoneNumber), IsUnique = true)]

    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long EmployeeNumber { get; set; }
        [Required]
        [MaxLength(500)]
        public string FullName { get; set; }

        [Required]
        public long SapNumber { get; set; }

        [Required]
        [MinLength(14)]
        [MaxLength(14)]
        public string Id { get; set; }

        [Required]
        public int Gender { get; set; }

        [Required]
        public DateTime JoiningDate { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public int MaritalStatus { get; set; }

        [Required]
        public long DepartmentId { get; set; }

        [Required]
        public Department Department { get; set; }

        [Required]
        public long PositionId { get; set; }

        [Required]
        public Position Position { get; set; }

        public string UserId { get; set; } 

        public Employee Supervisor { get; set; }

        //[Required]
        public long? SupervisorId { get; set; }

        [Required]
        public bool isDeptManager { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [MinLength(11)]
        [MaxLength(11)]
        public string PhoneNumber { get; set; }

        [Required]
        public Nationality Nationality { get; set; }

        [Required]
        public long NationalityId { get; set; }

        public string ProfilePicture { get; set; }


        [Required]
        public long HRId { get; set; }

        public bool IsDelted { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public Company Company { get; set; }

        [Required]
        public int Collar { get; set; }
        [Required]
        public string UserToken { get; set; }
        [Required]
        public string Country { get; set; }

        [Required]
        public bool HasChilderen { get; set; }

        [Required]
        public bool IsDirectEmployee { get; set; }
    }
}
