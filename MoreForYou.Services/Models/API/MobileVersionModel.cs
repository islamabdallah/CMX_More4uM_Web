using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.API
{
    public class MobileVersionAPIModel
    {
        [Required]
        public string AndroidVersion { get; set; }

        [Required]
        public string IOSVersion { get; set; }

        [Required]
        [Url]
        public string AndroidLink { get; set; }
        [Required]
        [Url]
        public string IOSLink { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
