using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.MasterModels
{
   public class GroupEmployeeModel
    {
        [Required]
        public long GroupId { get; set; }

        [Required]
        public GroupModel Group { get; set; }

        [Required]
        public long EmployeeId{ get; set; }

        [Required]
        public EmployeeModel Employee { get; set; }

        public DateTime JoinDate { get; set; }
    }
}
