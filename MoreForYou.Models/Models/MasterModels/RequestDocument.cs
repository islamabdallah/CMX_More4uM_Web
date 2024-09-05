using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
    public class RequestDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        //[MaxLength(100)]
        //public string Name { get; set; }
        [MaxLength(100)]
        public string FileType { get; set; }
        //[MaxLength]
        //public byte[] DataFiles { get; set; }
        [Required]
        [MaxLength]
        public string fileName { get; set; }

        public long BenefitRequestId { get; set; }
        public BenefitRequest BenefitRequest { get; set; }
    }
}
