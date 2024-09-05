using System;
using System.Collections.Generic;
using System.Text;

namespace MoreForYou.Services.Models.MasterModels
{
    public class SearchBenefits
    {
        public int SelectedBenefitTypeId { get; set; }

        public List<BenefitTypeModel> BenefitTypeModels { get; set; }

        public List<int> Years { get; set; }

        public int SelectedYear { get; set; }

        public List<BenefitModel> AllBenefitModels { get; set; }

        public List<BenefitModel> AvailableBenefitModels { get; set; }

        public bool hasRequests { get; set; }

    }
}
