using MoreForYou.Services.Models.API.Medical;
using MoreForYou.Services.Models.Medical;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts.Medical
{
    public interface IMedicalSubCategoryService
    {
        Task<List<MedicalSubCategoryModel>> GetAllMedicalSubCategories();

   
        List<MedicalSubCategoryAPIModel> ConvertMedicalSubCategoriesModelToMedicalSubCategoriesAPIModel(List<MedicalSubCategoryModel> MedicalSubCategoryModels, int languageId);

         Task<List<MedicalSubCategoryModel>> GetMedicalSubCategoryModelsByCategoryId(int CategoryId);
    }
}
