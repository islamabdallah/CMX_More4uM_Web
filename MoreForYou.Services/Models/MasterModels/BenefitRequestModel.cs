using Microsoft.AspNetCore.Http;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreForYou.Services.Models.MasterModels
{
    public class BenefitRequestModel
    {
        public string BenefitName { get; set; }
        //public List<> MyProperty { get; set; }
        [Required]
        public long EmployeeId { get; set; }
        [Required]
        public EmployeeModel Employee { get; set; }
        [Required]
        public long BenefitId { get; set; }
        [Required]
        public BenefitModel Benefit { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ExpectedDateFrom { get; set; }
        public DateTime ExpectedDateTo { get; set; }

        [MaxLength(600)]
        public string Message { get; set; }
        public DateTime ConfirmedDateFrom { get; set; }
        public DateTime ConfirmedDateTo { get; set; }
        public long Id { get; set; }
        public bool IsDelted { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public List<RequestWokflowModel> RequestWokflowModels { get; set; }
        public bool CanCancel { get; set; }
        public bool CanEdit { get; set; }

        public List<EmployeeModel> Participants { get; set; }

        public List<long> SelectedParticipants { get; set; }
        public string SelectedEmployeeNumbers { get; set; }

        [Required]
        public int RequestStatusId { get; set; }
        [Required]
        public RequestStatusModel RequestStatus { get; set; }

        public string RequestDateString { get; set; }
        public string ExpectedDateFromString { get; set; }
        public string ExpectedDateToString { get; set; }

        public long BenefitTypeId { get; set; }

        public GroupModel Group { get; set; }

        public long? GroupId { get; set; }

        public List<BenefitRequestModel> BenefitRequestModels { get; set; }

        public string RequiredDocuments { get; set; }

        [NotMapped]
        [Required]
        public IFormFile[] RequiredDocumentsfiles { get; set; }
        public RequestWokflowModel requestWokflowModel { get; set; }

        public string WarningMessage { get; set; }

        public long SendTo { get; set; }

        public List<RequestDocumentModel> RequestDocumentModels { get; set; }

        public EmployeeModel CurrentMember { get; set; }
    }
}
