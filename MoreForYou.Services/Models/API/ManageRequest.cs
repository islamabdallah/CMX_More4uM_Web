using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreForYou.Services.Models.API
{
    public class ManageRequest
    {
        public List<Request> Requests { get; set; }

        public List<BenefitTypeModel> BenefitTypeModels { get; set; }

        public int SelectedBenefitType { get; set; }
        public int SelectedTimingId { get; set; }

        public List<RequestStatusModelAPI> RequestStatusModels { get; set; }

        public int SelectedRequestStatus { get; set; }

        //public string benefitNameSearch { get; set; }
        //public long RequestNumberSearch { get; set; }
        public List<DepartmentAPI> DepartmentModels { get; set; }
        public long SelectedDepartmentId { get; set; }
        public long employeeNumberSearch { get; set; }
        public bool SelectedAll { get; set; }
        public List<TimingModel> TimingModels { get; set; }

        public DateTime SearchDateFrom { get; set; }

        public DateTime SearchDateTo { get; set; }
        public bool HasWarningMessage { get; set; }

        public bool IsAdmin { get; set; }
    }

    //public class ReuestToApprove
    //{
    //    public long RequestNumber { get; set; }
    //    public string Requiredat { get; set; }
    //    public string RequiredTo { get; set; }

    //    public string Requestedat { get; set; }
    //    public string Message { get; set; }

    //    public string GroupName { get; set; }

    //    public List<long> Participants { get; set; }

    //    public List<LoginUser> ParticipantsData { get; set; }
    //    public LoginUser SendTo { get; set; }

    //    public long benefitId { get; set; }

    //    public string BenefitName { get; set; }

    //    public string BenefitType { get; set; }
    //    public string status { get; set; }

    //    public LoginUser CreatedBy { get; set; }
    //    public string  WariningMessage { get; set; }

    //    public bool EmployeeCanResponse { get; set; }


    //}

    public class RequestSearch
    {
        public int SelectedBenefitType { get; set; }
        public int SelectedTimingId { get; set; }
        public int SelectedRequestStatus { get; set; }
        public long SelectedDepartmentId { get; set; }
        public long userNumberSearch { get; set; }
        public bool SelectedAll { get; set; }

        public long userNumber { get; set; }

        public DateTime SearchDateFrom { get; set; }

        public DateTime SearchDateTo { get; set; }

        public bool HasWarningMessage { get; set; }

        public int languageId { get; set; }
    }

    public class DepartmentAPI
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }

}
