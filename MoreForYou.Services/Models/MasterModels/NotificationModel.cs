using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.MasterModels
{
   public class NotificationModel
    {
        public long? MedicalRequestId { get; set; }

        [Required]
        public string Message { get; set; }
        public long Id { get; set; }
        public bool IsDelted { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
       
        public long? BenefitRequestId { get; set; }
        
        public BenefitRequestModel BenefitRequest { get; set; }
        public string Type { get; set; }

        public long? ResponsedBy { get; set; }
        public long RequestWorkflowId { get; set; }

        [Required]
        public string ArabicMessage { get; set; }
    }
}
