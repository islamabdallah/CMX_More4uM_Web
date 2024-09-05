using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface IMobileVersionService
    {
        Task<bool> CreateMobileVersion(MobileVersionModel mobileVersionModel );

        Task<MobileVersionModel> GetLastMobileVersion();

        Task<List<MobileVersionModel>> GetAllMobileVersion();
    }
}
