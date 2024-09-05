using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.API
{
    public class UpdateProfile
    {
        [MaxLength]
        public string Photo { get; set; }
    }
}
