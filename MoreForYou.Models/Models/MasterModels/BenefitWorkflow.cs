using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
    public class BenefitWorkflow
    {
      
        [Required]
        public Benefit Benefit { get; set; }

        [Key]
        public long BenefitId { get; set; }

        [Key]
        public string RoleId { get; set; }

        [Required]
        public int Order { get; set; }

        public bool IsDelted { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
