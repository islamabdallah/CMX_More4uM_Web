using MoreForYou.Models.Models.MasterModels.MedicalModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.Medical
{
    public class MedicalSubCategoryModel
    {
        public long Id { get; set; }
        public bool IsDelted { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        [Required]
        [MaxLength(500)]
        public string Name_AR { get; set; }

        [Required]
        [MaxLength(500)]
        public string Name_EN { get; set; }

        [Required]
        public string Image { get; set; }
        public MedicalCategory MedicalCategory { get; set; }

        public List<MedicalDetailsModel> MedicalDetailsModels { get; set; }

        public long MedicalDetailsCount { get; set; }

        public string WebIcon { get; set; }

    }
}
