using Microsoft.AspNetCore.Http;
using MoreForYou.Models.Models.MasterModels.MedicalModels;
using MoreForYou.Services.Models.API.Medical;
using MoreForYou.Services.Models.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Models.MedicalRequestViewModels
{
    public class MedicalRequestVM
    {
        public MedicalRequestApiModel mRequest {  get; set; }
        public EmployeeRelativesApiModel RelativesApiModel { get; set; }

        public List<MedicalCategoryModel> CheckupCategoryAPIModels { get; set; }

        public List<MedicalSubCategoryAPIModel> CheckupSubCategoryAPIModels { get; set; }

        public List<MedicalDetailsAPIModel> CheckupDetailsAPIModels { get; set; }
        public List<MedicalDetailsAPIModel>? medicationEntities { get; set; }
     
    }

    public class MedicalNetworkVM
    {
        public List<MedicalCategoryModel> medicalCategories { get; set; }

        public long SelectedSubCategory {  get; set; }
        public long SelectedCategory { get; set; }
        public long SubCategoryId { get; set; }

        public List<MedicalDetails>? medicationEntities { get; set; }

    }

}
