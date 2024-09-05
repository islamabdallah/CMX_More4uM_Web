using Data.Repository;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Contracts.Email;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Implementation.Email
{
    public class EmailLogService : IEmailLogService
    {
        private readonly IRepository<EmailLog, long> _repository;
        private readonly ILogger<EmailLogService> _logger;

        public EmailLogService(IRepository<EmailLog, long> repository, 
            ILogger<EmailLogService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public Task<bool> CreateEmailLog(EmailLog model)
        {
            try
            {
                var log = _repository.Add(model);
                if (log != null)
                {
                    return Task<bool>.FromResult<bool>(true);
                }
                else
                {
                    return Task<bool>.FromResult<bool>(false);
                }
            }
            catch(Exception e)
            {
                return Task<bool>.FromResult<bool>(false);
            }
        }

        public Task<List<EmailLog>> GetAllEmailLogs()
        {
            throw new NotImplementedException();
        }
    }
}
