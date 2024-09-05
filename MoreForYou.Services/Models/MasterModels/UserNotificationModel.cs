using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.MasterModels
{
   public class UserNotificationModel
    {
        [Required]
        public long NotificationId { get; set; }
        public NotificationModel Notification { get; set; }
        [Required]

        public long EmployeeId { get; set; }
        public EmployeeModel Employee { get; set; }

        [Required]
        public bool Seen { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public EmployeeModel ResponsedByEmployee { get; set; }

    }
}
