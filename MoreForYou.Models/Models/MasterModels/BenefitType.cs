using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
    public class BenefitType: EntityWithIdentityId<long>
    {
        [Required]
        public String Name { get; set; }

        [Required]
        public String ArabicName { get; set; }
    }
}
