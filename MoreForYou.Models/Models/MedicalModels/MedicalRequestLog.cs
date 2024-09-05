using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Models.Models.MedicalModels
{
    public class MedicalRequestLog : EntityWithIdentityId<long>
    {
        [ForeignKey("MedicalRequest")]
        [DisplayName("MedicalRequest")]
        public long MedicalRequestId { get; set; }

        public virtual MedicalRequest MedicalRequest { get; set; }

        public string Status { get; set; }

        public string CreatedBy { get; set; }

        public bool IsActive { get; set; }
    }
}
