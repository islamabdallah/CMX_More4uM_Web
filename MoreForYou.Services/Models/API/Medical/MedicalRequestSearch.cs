using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Models.API.Medical
{
    public class MedicalRequestSearch
    {
        public string selectedRequestType { get; set; }

        public string selectedRequestStatus { get; set; }

        public string requestId { get; set; }

        public string userNumberSearch { get; set; }

        public string relativeId { get; set; }

        public string userNumber { get; set; }

        public string searchDateFrom { get; set; }

        public string searchDateTo { get; set; }

        public string languageId { get; set; }
    }
}
