using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static MoreForYou.Services.Models.CommanData;

namespace MoreForYou.Services.Models.MasterModels
{
    public class RequestWokflowModel
    {
        public long Id { get; set; }
        [Required]
        public BenefitRequestModel BenefitRequest { get; set; }
        public long BenefitRequestId { get; set; }
        [Required]
        public EmployeeModel Employee { get; set; }
        public long EmployeeId { get; set; }

        [Required]
        public string RoleId { get; set; }

        [Required]
        public int Status { get; set; }

        public string Notes { get; set; }

        public DateTime ReplayDate { get; set; }

        public bool IsDelted { get; set; }
        public bool IsVisible { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public bool canResponse { get; set; }
        public List<RequestStatusModel> RequestStatusModels { get; set; }
        public int RequestStatusSelectedId { get; set; }

        public List<ResonseStatus> ResonseStatuses { get; set; }

        [Required]
        public int RequestStatusId { get; set; }
        [Required]
        public RequestStatusModel RequestStatus { get; set; }
        public string ConfirmedDateFromString { get; set; }
        public string ConfirmedDateToString { get; set; }
        public string RoleName { get; set; }
        public string[] Documents { get; set; }
        public bool HasDocuments { get; set; }
        public long? WhoResponseId { get; set; }

    }
}
