using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Text;
using static MoreForYou.Services.Models.CommanData;

namespace MoreForYou.Services.Models.MasterModels
{
    public class RequestFilterModel
    {
        public List<RequestWokflowModel> RequestWokflowModels { get; set; }

        public List<BenefitRequestModel> BenefitRequestModels { get; set; }

        public List<BenefitTypeModel> BenefitTypeModels { get; set; }

        public int SelectedBenefitType { get; set; }
        public int SelectedTimingId { get; set; }

        public List<RequestStatusModel> RequestStatusModels { get; set; }

        public int SelectedRequestStatus { get; set; }

        public string benefitNameSearch { get; set; }
        public long RequestNumberSearch { get; set; }
        public List<DepartmentModel> DepartmentModels { get; set; }
        public long SelectedDepartmentId { get; set; }

        public DateTime SelectedRequestDate { get; set; }
        public long employeeNumberSearch { get; set; }
        public List<ResonseStatus> ResonseStatuses { get; set; }
        public int AllRequestsStatus { get; set; }
        public bool SelectedAll { get; set; }
        public List<TimingModel> TimingModels { get; set; }
        public List<GroupModel> GroupModels { get; set; }

    }
}
