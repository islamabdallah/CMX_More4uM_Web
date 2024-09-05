using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
   public interface IGroupService
    {
        Task<List<GroupModel>> GetAllGroups();
        GroupModel CreateGroup(GroupModel model);
        bool UpdateGroup(GroupModel model);
        bool DeleteGroup(long id);
        GroupModel GetGroup(long id);
        Task<List<GroupModel>> GetGroupByBenefitId(long Id);
    }
}
