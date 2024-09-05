using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.Medical
{
    public class MedicalCategoryModel
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

        public int SubCategoriesCount { get; set; }

        public bool hasOneSubCategory { get; set; }

        public string WebIcon { get; set; }


    }

    public class MedicalMain
    {
        public List<MedicalCategoryModel> MedicalCategoryModels { get; set; }
    }
}
