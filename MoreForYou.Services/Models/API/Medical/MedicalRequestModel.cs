using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Models.API.Medical
{
    public class MedicalRequestModel
    {
        public string createdBy { get; set; }

        public string requestedBy { get; set; }

        public string requestedByNumber { get; set; }

        public string employeeCoverage { get; set; }

        public string requestedFor { get; set; }

        public string? relativeCoverage { get; set; }

        public string? relation { get; set; }

        public string? order { get; set; }

        public int requestType { get; set; }

        public DateTime requestDate { get; set; }

        public bool monthlyMedication { get; set; }

        public List<string>? attachment { get; set; }

        public bool selfRequest { get; set; }

        public string MedicalEntity { get; set; }

        public string MedicalEntityId { get; set; }

        public string? medicalPurpose { get; set; }

        public string comment { get; set; }
    }
}
