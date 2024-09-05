using Microsoft.EntityFrameworkCore;
using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
    [Index(nameof(Nationality.Name), IsUnique = true)]
    public class Nationality: EntityWithIdentityId<long>
    {
        [Required]
        public string Name { get; set; }
    }
}
