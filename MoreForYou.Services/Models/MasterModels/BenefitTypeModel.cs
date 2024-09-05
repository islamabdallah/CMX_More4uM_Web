using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.MasterModels
{
    public class BenefitTypeModel
    {
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }

        [Required]
        public String ArabicName { get; set; }
    }
}
