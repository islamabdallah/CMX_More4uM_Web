using Microsoft.AspNetCore.Http;
using MoreForYou.Services.Models.API.Medical;
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

        public List<MedicalCategoryAPIModel> CheckupCategoryAPIModels { get; set; }

        public List<MedicalSubCategoryAPIModel> CheckupSubCategoryAPIModels { get; set; }

        public List<MedicalDetailsAPIModel> CheckupDetailsAPIModels { get; set; }
        public List<MedicalDetailsAPIModel>? medicationEntities { get; set; }
     
    }

}
