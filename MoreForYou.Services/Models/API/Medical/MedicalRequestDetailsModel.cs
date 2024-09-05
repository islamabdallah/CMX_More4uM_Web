using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Models.API.Medical
{
    public class MedicalRequestDetailsModel
    {
        public string MedicalRequestId { get; set; }

        public string RequestStatus { get; set; }

        public MedicalRequestModel MedicalRequest { get; set; }

        public MedicalResponseModel? MedicalResponse { get; set; }
    }
}
