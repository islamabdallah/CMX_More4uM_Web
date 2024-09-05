using MoreForYou.Models.Models.MedicalModels;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Models.API.Medical
{
    public class SearchApiModel
    {
        public List<MedicalRequest> Requests { get; set; }

        public List<MedicalRequestType> MedicalRequestTypes { get; set; }

        public int SelectedMedicalRequestType { get; set; }

        public List<RequestStatusModelAPI> RequestStatusModels { get; set; }

        public int SelectedRequestStatus { get; set; }

        public bool SelectedAll { get; set; }

        public DateTime SearchDateFrom { get; set; }

        public DateTime SearchDateTo { get; set; }
    }
}
