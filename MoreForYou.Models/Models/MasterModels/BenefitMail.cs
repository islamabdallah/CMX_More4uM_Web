using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
    public class BenefitMail: EntityWithIdentityId<int>
    {
        [Required]
        public string SendTo { get; set; }

        public string CarbonCopy { get; set; }

      
        //public string MailDescription { get; set; }


        [Required]
        public long BenefitId { get; set; }
        [Required]
        public Benefit Benefit { get; set; }
    }
}
