using Microsoft.EntityFrameworkCore;
using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
    [Index(nameof(Department.Name), IsUnique = true)]
    public class Department: EntityWithIdentityId<long>
    {
        [Required]
        [MaxLength(500)]
        public string Name { get; set; }

    }
}
