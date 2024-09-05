using MoreForYou.Models.Models;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.Medical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreForYou.Services.Models.API
{
    public class HomeModel
    {

        //public TokenModel tokenModel { get; set; }
        public List<BenefitAPIModel> AllBenefitModels { get; set; }
        public List<BenefitAPIModel> AvailableBenefitModels { get; set; }

        public LoginUser user { get; set; }

        public List<MedicalCategoryModel> MedicalCategoryModels { get; set; }

        public int UserUnSeenNotificationCount { get; set; }
        public int PriviligesCount { get; set; }
    }


    public class TokenModel
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
