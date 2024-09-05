using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreForYou.Services.Models.MasterModels
{
    public class GroupModel
    {
        public string Code { get; set; }

        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int RequestStatusId { get; set; }
        [Required]
        public RequestStatusModel RequestStatus { get; set; }

        public string GroupStatus { get; set; }

        [Required]

        public DateTime ExpectedDateFrom { get; set; }
        [Required]

        public DateTime ExpectedDateTo { get; set; }
       // [Required]
        [MaxLength(600)]
        public string Message { get; set; }
        public DateTime ConfirmedDateFrom { get; set; }
        public DateTime ConfirmedDateTo { get; set; }

        [Required]
        public long BenefitId { get; set; }
        [Required]
        public BenefitModel Benefit { get; set; }

        public string CreatedBy { get; set; }

        [NotMapped]
        public BenefitRequestModel BenefitRequestModel { get; set; }

        public long Id { get; set; }
        public bool IsDelted { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool CanResponse { get; set; }

        public EmployeeModel CreatedEmployee { get; set; }
       public List<Participant> Participants { get; set; }

        public int SelectedRequestStatusId { get; set; }

        public int groupMembersCount { get; set; }

        public List<GroupEmployeeModel> groupEmployeeModels { get; set; }

    }
}
