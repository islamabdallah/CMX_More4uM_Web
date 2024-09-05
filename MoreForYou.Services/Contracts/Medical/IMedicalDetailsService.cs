using MoreForYou.Models.Models.MasterModels.MedicalModels;
using MoreForYou.Services.Models.API.Medical;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.Medical;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts.Medical
{
    public interface IMedicalDetailsService
    {
        Task<List<MedicalDetailsModel>> GetAllMedicalDetails();

        Task<List<MedicalDetailsModel>> GetMedicalDetailsBySubCategoryId(long SubCategoryId, string Country);

        Task<List<MedicalDetailsModel>> GetMedicalDetailsForCountry(string Country);

        List<MedicalDetailsAPIModel> ConvertMedicalDetailsModelToMedicalDetailsAPIModel(List<MedicalDetailsModel> MedicalDetailsModels, int languageId);

        Task<MedicalDetailsModel> GetMedicalDetailsById(int MedicalDetailsId, string Country);

    }
}
