using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
    public class EmailLog
    {
        public long Id { get; set; }

        [Required]
        public string Subject { get; set; }
        [Required]
        public string To { get; set; }

        public long EmployeeNumber { get; set; }

        [Required]
        public string benefitName { get; set; }
        public string ExceptionType { get; set; }
        public bool Done { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
