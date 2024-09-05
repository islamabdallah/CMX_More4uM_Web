using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
    public class UserNotification
    {
        [Required]
        public long NotificationId { get; set; }
        public Notification Notification { get; set; }
        [Required]

        public long EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Required]
        public bool Seen { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
