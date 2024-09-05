using System;
using System.Collections.Generic;
using System.Text;

namespace MoreForYou.Services.Models.API
{
    public class Gift
    {
        public long RequestNumber { get; set; }

        public long UserNumber { get; set; }

        public string UserName { get; set; }

        public string BenefitName { get; set; }
        public string BenefitCard { get; set; }
        public string UserDepartment { get; set; }
        public string UserEmail { get; set; }
        public DateTime Date { get; set; }
    }
}
