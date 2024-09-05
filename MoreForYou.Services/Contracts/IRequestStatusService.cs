using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface IRequestStatusService
    {
        List<RequestStatusModel> GetAllStatuses();
        
    }
}
