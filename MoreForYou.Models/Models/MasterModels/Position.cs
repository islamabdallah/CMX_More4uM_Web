using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
    public class Position:EntityWithIdentityId<long>
    {
        [Required]
        public string Name { get; set; }
    }
}
