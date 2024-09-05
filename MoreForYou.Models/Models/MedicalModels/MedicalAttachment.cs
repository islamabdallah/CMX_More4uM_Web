using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Models.Models.MedicalModels
{
    public class MedicalAttachment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("MedicalRequest")]
        [DisplayName("MedicalRequest")]
        public long MedicalRequestId { get; set; }

        public virtual MedicalRequest MedicalRequest { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string? Status { get; set; }
    }
}
