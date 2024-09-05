using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels.MedicalModels
{
    public class MedicalSubCategory: lookupIdentityEntity
    {
        public MedicalCategory MedicalCategory { get; set; }
        //[Required]
        // public int MedicalCategoryId { get; set; }

        public string WebIcon { get; set; }


    }
}
