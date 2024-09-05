using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
    class EmployeeRequestNotification:EntityWithIdentityId<long>
    {
        public string Message { get; set; }

        public Employee Employee { get; set; }
        public long EmployeeId { get; set; }

        public BenefitRequest BenefitRequest { get; set; }
        public long BenefitRequestId { get; set; }

        public bool Seen { get; set; }

    }
}
