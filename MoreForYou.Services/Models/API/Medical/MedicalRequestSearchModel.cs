using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Models.API.Medical
{
    public class MedicalRequestSearchModel
    {
        public string selectedRequestType { get; set; }

        public string selectedRequestStatus { get; set; }

        public long requestId { get; set; }

        public long userNumberSearch { get; set; }

        public int relativeId { get; set; }

        public long userNumber { get; set; }

        public DateTime searchDateFrom { get; set; }

        public DateTime searchDateTo { get; set; }
    }
}
