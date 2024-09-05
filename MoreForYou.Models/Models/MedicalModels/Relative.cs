using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Models.Models.MedicalModels
{
    public class Relative : EntityWithIdentityId<int>
    {
        public long EmployeeNumber { get; set; }

        public string Relatives { get; set; }

        public string Relation { get; set; }

        public DateTime BDate { get; set; }

        public int Order { get; set; }

        public string CoverPercentage { get; set; }

        public bool IsActive { get; set; }

        public
#nullable enable
        string? ArabicRelatives
        { get; set; }

        public string? ArabicRelation { get; set; }
    }
}
