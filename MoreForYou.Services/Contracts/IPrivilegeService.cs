using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface IPrivilegeService
    {
        Task<List<PrivilegeModel>> GetAllPrivileges();
        Task<PrivilegeModel> CreatePrivilege(PrivilegeModel model);
        Task<bool> UpdatePrivilege(PrivilegeModel model);
        bool DeletePrivilege(int id);
        PrivilegeModel GetPrivilege(long Id);
        public PrivilegeModel GetPrivilegeByName(string name);
        List<PriviligeAPIModel> CreatePriviligeAPIModel(List<PrivilegeModel> privilegeModels);

    }
}
