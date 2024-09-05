using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreForYou.Services.Models.MasterModels
{
   public class GroupRequestModel
    {
        public List<EmployeeModel> EmployeeModels { get; set; }

        public string  GroupName { get; set; }

        public string GroupDescription { get; set; }

        public  BenefitRequestModel BenefitRequestModel { get; set; }


    }
}
