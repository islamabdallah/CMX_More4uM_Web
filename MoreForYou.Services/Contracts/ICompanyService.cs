using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
   public interface ICompanyService
    {

        List<CompanyModel> GetAllCompanies();
        CompanyModel CreateCompany(CompanyModel model);
        Task<bool> UpdateCompany(CompanyModel model);
        bool DeleteCompany(int id);
        CompanyModel GetCompany(long Id);
    }
}
