using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
    public class GroupEmployee
    {
        [Required]
        public long GroupId { get; set; }

        [Required]
        public Group Group { get; set; }

        [Required]
        public long EmployeeId { get; set; }
        [Required]
        public Employee Employee { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }
    }
}
