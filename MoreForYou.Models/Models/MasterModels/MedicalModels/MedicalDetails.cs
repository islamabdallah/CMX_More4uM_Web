using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace MoreForYou.Models.Models.MasterModels.MedicalModels
{
    public class MedicalDetails : EntityWithIdentityId<long>
    {
        [Required]
        public string Name_AR { get; set; }

        [Required]
        public string Name_EN { get; set; }


        [Required]

        public string Address_AR { get; set; }

        [Required]

        public string Address_EN { get; set; }

        [Required]
        public string Mobile { get; set; }

        [Required]
        public string WorkingHours_EN { get; set; }

        [Required]
        public string WorkingHours_AR { get; set; }

        [Required]
        public string Image { get; set; }

        public MedicalSubCategory MedicalSubCategory { get; set; }
        // [Required]
        //public long MedicalSubCategoryId { get; set; }

        [Required]
        [DefaultValue("Assiut")]
        public string Country { get; set; }

    }
}
