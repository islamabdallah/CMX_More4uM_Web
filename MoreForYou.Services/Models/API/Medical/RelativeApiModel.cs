using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Models.API.Medical
{
    public class RelativeApiModel
    {
        public int RelativeId { get; set; }

        public string RelativeName { get; set; }

        public string Relation { get; set; }

        public DateTime BirthDate { get; set; }

        public int Order { get; set; }

        public string MedicalCoverage { get; set; }

        public bool? IsActive { get; set; }
    }
}
