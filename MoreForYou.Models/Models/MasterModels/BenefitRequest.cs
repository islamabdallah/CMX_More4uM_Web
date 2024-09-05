using Microsoft.AspNetCore.Http;
using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
    public class BenefitRequest: EntityWithIdentityId<long>
    {

        [Required]
        public long BenefitId { get; set; }
        [Required]
        public Benefit Benefit { get; set; }

        [Required]
        public long EmployeeId { get; set; }

        [Required]
        public Employee Employee { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ExpectedDateFrom { get; set; }
        public DateTime ExpectedDateTo { get; set; }

        [MaxLength(600)]
        public string Message { get; set; }
        [Required]
        public int RequestStatusId { get; set; }
        [Required]
        public RequestStatus RequestStatus { get; set; }

        public Group Group { get; set; }

        public long? GroupId { get; set; }

        public string RequiredDocuments { get; set; }

        public long? SendTo { get; set; }



    }
}
