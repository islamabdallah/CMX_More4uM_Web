using MoreForYou.Services.Models.API.Medical;
using MoreForYou.Services.Models.Medical;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts.Medical
{
    public interface IMedicalCategoryService
    {
        Task<List<MedicalCategoryModel>> GetAllMedicalCategories();

       List<MedicalCategoryAPIModel> ConvertMedicalCategoriesModelToMedicalCategoriesAPIModel(List<MedicalCategoryModel> MedicalCategoryModels, int languageId);
    }
}
