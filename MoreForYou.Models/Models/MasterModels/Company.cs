using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
   public class Company: EntityWithIdentityId<int>
    {
        [Required]
        public string Code { get; set; }
    }
}
