using Microsoft.EntityFrameworkCore;
using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
    //[Index(nameof(EmployeeDependent.Id), IsUnique = true)]
    //[Index(nameof(EmployeeDependent.PhoneNumber), IsUnique = true)]
    public class EmployeeDependent
    {
        [Required]
        public int relation { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }
        public long PhoneNumber { get; set; }
        public string job { get; set; }
        public Employee Employee { get; set; }

        [Key]
        public long EmployeeNumber { get; set; }

        [Key]
        [MinLength(14)]
        [MaxLength(14)]
        public string Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Name { get; set; }
        public bool IsDelted { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
