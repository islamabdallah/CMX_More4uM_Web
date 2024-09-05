using MoreForYou.Models.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts.Email
{
    public interface IEmailLogService
    {
        Task<List<EmailLog>> GetAllEmailLogs();
        Task<bool> CreateEmailLog(EmailLog model);
    }
}
