using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
   public class Notification: EntityWithIdentityId<long>
    {
        public long? MedicalRequestId { get; set; }
        public long? BenefitRequestId { get; set; }
        public BenefitRequest BenefitRequest { get; set; }
        [Required]
        public string Message { get; set; }

        [Required]
        public string ArabicMessage { get; set; }

        [Required]
        public string Type { get; set; }
        public long? ResponsedBy { get; set; }

        public long RequestWorkflowId { get; set; }
    }
}
