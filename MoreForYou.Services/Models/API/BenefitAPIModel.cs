using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.API
{
    public class BenefitAPIModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string BenefitCard { get; set; }

        public int Times { get; set; }

        public string BenefitType { get; set; }

        public bool UserCanRedeem { get; set; }

        /// Data to used in Redeem
        public bool IsAgift { get; set; }

        public int MinParticipant { get; set; }

        public int MaxParticipant { get; set; }

        public string[] RequiredDocumentsArray { get; set; }

        [Required]
        public int numberOfDays { get; set; }

        public string DateToMatch { get; set; }
        public DateTime? CertainDate { get; set; }

        public string LastStatus { get; set; }
        public int TimesUserReceiveThisBenefit { get; set; }

        public List<string> BenefitWorkflows { get; set; }
        //public List<string> BenefitConditions { get; set; }

        //public List<BenefitStats> benefitStatses { get; set; }

        public Dictionary<string, string> BenefitConditions { get; set; }
        public Dictionary<string, bool> BenefitApplicable { get; set; }

        public int totalRequestsCount { get; set; }
        public bool HasHoldingRequests { get; set; }

        public string BenefitCardAPI { get; set; }
        public bool MustMatch { get; set; }

        public string Title { get; set; }
        public List<string> BenefitDecriptionList { get; set; }
    }

    public class BenefitStats
    {
        public string Name { get; set; }
        public int Count { get; set; }

    }

    public class BenefitConditionsAndAvailable
    {
        public Dictionary<string, string> BenefitConditions { get; set; }

        public Dictionary<string, bool> BenefitApplicable { get; set; }


    }

    public class BenefitDecription
    {
        public int Id { get; set; }

        public string Description { get; set; }
    }

    public class MyBenefitsModel
    {
        public List<BenefitAPIModel> PendingBenefits { get; set; }

        public List<BenefitAPIModel> InprogressBenefits { get; set; }

        public List<BenefitAPIModel> ApprovedBenefits { get; set; }

        public List<BenefitAPIModel> RejectedBenefits { get; set; }


        public List<BenefitAPIModel> CancelledBenefits { get; set; }



    }

    public class MyBenefitsViewModel
    {
        public List<BenefitAPIModel> myBenefits { get; set; }
    }


}
