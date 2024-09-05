using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Models.API.Medical
{
    public class MedicalRequestSearchApi
    {
        public int SelectedMedicalRequestType { get; set; }

        public int SelectedRequestStatus { get; set; }

        public bool SelectedAll { get; set; }

        public DateTime SearchDateFrom { get; set; }

        public DateTime SearchDateTo { get; set; }

        public string languageId { get; set; }
    }
}
