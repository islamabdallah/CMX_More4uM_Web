using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.API.Medical
{
    public class MedicalCategoryAPIModel
    {
        //public string CategoryName_AR { get; set; }
        //public string CategoryName_EN { get; set; }

        public string CategoryName { get; set; }
        public string CategoryImage { get; set; }
    }
}
