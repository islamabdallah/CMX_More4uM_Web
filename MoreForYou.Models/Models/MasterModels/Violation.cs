using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
    public class Violation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public int TruckID { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
    }
}
