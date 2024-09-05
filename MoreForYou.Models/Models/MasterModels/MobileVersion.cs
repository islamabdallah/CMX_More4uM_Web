using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
    public class MobileVersion: EntityWithIdentityId<int>
    {
        [Required]
        public string AndroidVersion { get; set; }
        [Required]
        public string IOSVersion { get; set; }

        [Required]
        public string AndroidLink { get; set; }
        [Required]
        public string IOSLink { get; set; }

    }
}
