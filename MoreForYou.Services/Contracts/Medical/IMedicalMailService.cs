using MoreForYou.Models.Models.MedicalModels;
using MoreForYou.Services.Models.API.Medical;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts.Medical
{
    public interface IMedicalMailService
    {
        bool SendToMailList(MedicalRequestDetailsModel model, List<string> groupMails = null, List<GroupEmployeeModel> groupEmployeeModels = null);

    }
}
