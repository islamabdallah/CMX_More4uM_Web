using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.MasterModels
{
    public class EmployeeRequestModel
    {
        [Required]
        public long BenefitRequestId { get; set; }

        [Required]
        public BenefitRequestModel BenefitRequest { get; set; }

        [Required]
        public long EmployeeId { get; set; }

        [Required]
        public EmployeeModel Employee { get; set; }
        [Required]
        public int RequestStatusId { get; set; }
        [Required]
        public RequestStatusModel RequestStatus { get; set; }

        public DateTime joinDate { get; set; }

        public bool IsDelted { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
