using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.MasterModels
{
    public class EmployeeDependentModel
    {
        [Required]
        public string relation { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }
        public long PhoneNumber { get; set; }
        public string job { get; set; }
        public EmployeeModel Employee { get; set; }

        public long EmployeeNumber { get; set; }

        public string Id { get; set; }
        public bool IsDelted { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        [Required]
        [MaxLength(500)]
        public string Name { get; set; }
    }
}
