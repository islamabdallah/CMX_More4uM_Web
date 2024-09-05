using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Models.Models.MedicalModels
{
    public class MedicalRequest : EntityWithIdentityId<long>
    {
        [ForeignKey("MedicalRequestType")]
        [DisplayName("MedicalRequestType")]
        public int MedicalRequestTypeId { get; set; }

        public virtual MedicalRequestType MedicalRequestType { get; set; }

        public DateTime RequestDate { get; set; }

        public string CreatedBy { get; set; }

        public long RequestedBy { get; set; }

        public int RequestedFor { get; set; }

        public bool MonthlyMedication { get; set; }

        public int? RequestMedicalEntity { get; set; }

        public string? MedicalPurpose { get; set; }

        public string? RequestComment { get; set; }

        public int? ResponseMedicalEntity { get; set; }

        public string? ResponseFeedback { get; set; }

        public string? ResponseReason { get; set; }

        public List<MedicalResponse> MedicalResponse { get; set; }

        public List<MedicalAttachment> MedicalAttachments { get; set; }

        public List<MedicalRequestLog> MedicalRequests { get; set; }
    }
}
