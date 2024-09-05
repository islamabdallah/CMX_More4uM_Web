using Microsoft.EntityFrameworkCore;
using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
    [Index(nameof(Group.Code), IsUnique = true)]
    public class Group: EntityWithIdentityId<long>
    {
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public int RequestStatusId { get; set; }
        [Required]
        public RequestStatus RequestStatus { get; set; }

        public string  GroupStatus { get; set; }

        public DateTime ExpectedDateFrom { get; set; }
        public DateTime ExpectedDateTo { get; set; }
        //[Required]
        [MaxLength(600)]
        public string Message { get; set; }

        [Required]
        public long BenefitId { get; set; }
        [Required]
        public Benefit Benefit { get; set; }

        public string CreatedBy { get; set; }

    }
}
