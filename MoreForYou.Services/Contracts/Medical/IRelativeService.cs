using MoreForYou.Models.Models.MedicalModels;
using MoreForYou.Services.Models.API.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts.Medical
{
    public interface IRelativeService
    {
        Task<List<Relative>> GetEmployeeRelatives(long employeeNumber);

        Task<int> GetPendingRelativeCountAsync();

        Task<EmployeeRelativesApi> GetEmployeeRelativesApi(
          long userNumber,
          int languageCode,
          int type,
          string Country);

        Task<EmployeeRelativesApiModel> GetEmployeeRelativesApiModel(long userNumber, int languageCode);

        Task<bool> CreateEmployeeRelative(Relative model);
    }
}
