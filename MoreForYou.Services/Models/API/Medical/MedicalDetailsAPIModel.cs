using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.API.Medical
{
    public class MedicalDetailsAPIModel
    {
        public string MedicalDetailsName { get; set; }

        public string MedicalDetailsAddress { get; set; }

        public string MedicalDetailsMobile { get; set; }

        public string MedicalDetailsWorkingHours { get; set; }

        public string MedicalDetailsImage { get; set; }

        public string SubCategoryName { get; set; }

        //public string SubCategoryName_AR { get; set; }
        //public string SubCategoryName_EN { get; set; }
        public string SubCategoryImage { get; set; }

        public string CategoryName { get; set; }


        //public string CategoryName_AR { get; set; }
        //public string CategoryName_EN { get; set; }
        public string CategoryImage { get; set; }
    }
}
