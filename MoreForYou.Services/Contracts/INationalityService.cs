using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface INationalityService
    {
        Task<List<NationalityModel>> GetAllNationalities();
        Task<bool> CreateNationality(NationalityModel model);
        Task<bool> UpdateNationality(NationalityModel model);
        bool DeleteNationality(int id);
        NationalityModel GetNationality(int id);
        Task<List<NationalityModel>> GetNationalityByName(NationalityModel model);
    }
}
