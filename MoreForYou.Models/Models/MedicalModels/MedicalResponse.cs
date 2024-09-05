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
    public class MedicalResponse : EntityWithIdentityId<long>
    {
        [ForeignKey("MedicalRequest")]
        [DisplayName("MedicalRequest")]
        public long MedicalRequestId { get; set; }

        public virtual MedicalRequest MedicalRequest { get; set; }

        public long? MedicalItemId { get; set; }

        public int? Quantity { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
    }
}
