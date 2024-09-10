using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Models.MasterModels
{
    public class BenefitFilterModel
    {
        public List<BenefitRequestModel> BenefitRequests { get; set; }

        public long? EmployeeNumber { get; set; }

        public long? SelectedBenefit { get; set; }

        [Required(ErrorMessage = "You must Select Area")]
        public string SelectedCountry { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
    }
}
