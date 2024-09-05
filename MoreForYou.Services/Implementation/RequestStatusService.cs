using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Implementation
{
    public class RequestStatusService : IRequestStatusService
    {
        private readonly IRepository<RequestStatus, int> _repository;
        private readonly ILogger<RequestStatusService> _logger;
        private readonly IMapper _mapper;

        public RequestStatusService(IRepository<RequestStatus, int> requestRepository,
         ILogger<RequestStatusService> logger, IMapper mapper)
        {
            _repository = requestRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public List<RequestStatusModel> GetAllStatuses()
        {
            try
            {
               List<RequestStatus> requestStatuses = _repository.Findlist().Result;
                List<RequestStatusModel> requestStatusModels = _mapper.Map<List<RequestStatusModel>>(requestStatuses);
                return requestStatusModels;
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }
    }
}
