using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Models.Models
{
    public class AspNetRole:IdentityRole
    {
        [Required]
        public string ArabicRoleName { get; set; }
    }
}
