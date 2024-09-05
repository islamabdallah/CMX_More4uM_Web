using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.MasterModels
{
    public class MobileVersionModel
    {
        public int Id { get; set; }
        public bool IsDelted { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
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
