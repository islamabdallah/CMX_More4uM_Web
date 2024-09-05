using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Models.API.Medical
{
    public class PendingRequestApiModel
    {
        public PendingRequestCount requestsCount { get; set; }

        public List<PendingRequestSummeyModel> requests { get; set; }
    }
    public class PendingRequestCount
    {
        public string medications { get; set; }

        public string checkups { get; set; }

        public string sickleave { get; set; }

        public string totalRequest { get; set; }
    }
    public class PendingRequestModel
    {
        public EmployeeRelativesModel relatives { get; set; }

        public MedicalRequestSearchModel searchModel { get; set; }

        public PendingRequestCount requestsCount { get; set; }

        public List<PendingRequestSummeyModel> requests { get; set; }
    }
    public class PendingRequestSummeyModel
    {
        public string requestID { get; set; }

        public string employeeName { get; set; }

        public string employeeNumber { get; set; }

        public string employeeImageUrl { get; set; }

        public DateTime requestDate { get; set; }

        public string createdBy { get; set; }

        public string requestTypeID { get; set; }

        public bool selfRequest { get; set; }

        public string? requestComment { get; set; }

        public string? requestStatus { get; set; }

        public string? requestMedicalEntity { get; set; }
    }
}
