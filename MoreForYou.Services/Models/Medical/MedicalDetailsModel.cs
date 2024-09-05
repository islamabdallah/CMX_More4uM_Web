using MoreForYou.Models.Models.MasterModels.MedicalModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.Medical
{
    public class MedicalDetailsModel
    {
        public long Id { get; set; }
        public bool IsDelted { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Name_EN { get; set; }

        public string Address_EN { get; set; }

        public string Mobile { get; set; }

        public string WorkingHours_EN { get; set; }

        public string Image { get; set; }

        public MedicalCategory MedicalCategory { get; set; }

        public MedicalSubCategory MedicalSubCategory { get; set; }


        public string Name_AR { get; set; }

        public string Address_AR { get; set; }


        public string WorkingHours_AR { get; set; }

    }

    public class MedicalDetailsViewModel
    {
        public List<MedicalDetailsModel> MedicalDetailsModels { get; set; }
    }
}
