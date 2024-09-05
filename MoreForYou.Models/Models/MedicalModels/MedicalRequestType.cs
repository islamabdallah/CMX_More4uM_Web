using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Models.Models.MedicalModels
{
    public class MedicalRequestType : EntityWithIdentityId<int>
    {
        public string Name { get; set; }

        public List<MedicalRequest> medicalRequests { get; set; }
    }
}
