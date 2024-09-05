using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
   public interface IGroupEmployeeService
    {
        Task<List<GroupEmployeeModel>> GetAllGroupEmployees();
        GroupEmployeeModel CreateGroupEmployee(GroupEmployeeModel model);
        Task<bool> UpdateGroupEmployee (GroupEmployeeModel model);
        Task<List<GroupEmployeeModel>> GetGroupParticipants(long id);
        Task<List<GroupEmployeeModel>> GetGroupsByEmployeeId(long employeeId);

        Task<List<GroupEmployeeModel>> GetGroupsByBenefitId(long benefitId);

        int GetTimesEmployeeReceieveThisGroupBenefit(long employeeNumber, long benefitId);
        bool ISEmployeeHasHoldingRequestsForthisGroupBenefit(long employeeNumber, long benefitId);
        public Task<List<string>> GetGroupParticipantsMails(long id);

        public  Task<List<GroupEmployeeModel>> GetGroupParticipantsBasicData(long id);



    }
}
