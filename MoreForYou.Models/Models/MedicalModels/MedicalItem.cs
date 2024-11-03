using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Models.Models.MedicalModels
{
    public class MedicalItem : EntityWithIdentityId<long>
    {
        public string DrugName { get; set; }

        public string Type { get; set; }

        public string Quantity { get; set; }
        public string Dose { get; set; }
        public string? Category { get; set; }
    }
}
