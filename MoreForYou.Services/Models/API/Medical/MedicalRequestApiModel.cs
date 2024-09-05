using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Models.API.Medical
{
    public class MedicalRequestApiModel
    {
        public
#nullable disable
    string createdBy { get; set; }

        public int requestedBy { get; set; }

        public int requestedFor { get; set; }

        public int requestType { get; set; }

        public DateTime requestDate { get; set; }

        public bool monthlyMedication { get; set; }

        public List<IFormFile>? attachment{ get; set; }

        public bool selfRequest { get; set; }

        public int? medicalEntityId { get; set; }

        public string? medicalPurpose { get; set; }

        public string comment { get; set; }

        public string? LanguageId { get; set; }
    }
}
