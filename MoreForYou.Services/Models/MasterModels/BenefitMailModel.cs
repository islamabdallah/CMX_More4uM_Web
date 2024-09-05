using MoreForYou.Models.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.MasterModels
{
    public class BenefitMailModel
    {
        public string SendTo { get; set; }

        public string Subject { get; set; }

        public int Id { get; set; }
        public bool IsDelted { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        [Required]
        public long BenefitId { get; set; }
        [Required]
        public Benefit Benefit { get; set; }

        public string CarbonCopies { get; set; }
    }
}
