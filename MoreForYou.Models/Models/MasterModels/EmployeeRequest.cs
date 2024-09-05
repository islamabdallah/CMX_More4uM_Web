using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{ 
    public class EmployeeRequest
    {
        [Required]
        public long BenefitRequestId { get; set; }

        [Required]
        public BenefitRequest BenefitRequest { get; set; }

        [Required]
        public long EmployeeId { get; set; }

        [Required]
        public Employee Employee { get; set; }
        [Required]
        public int RequestStatusId { get; set; }
        [Required]
        public RequestStatus RequestStatus { get; set; }

        public DateTime joinDate { get; set; }

        public bool IsDelted { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
