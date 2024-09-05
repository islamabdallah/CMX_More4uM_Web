using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Implementation
{
    public class EmployeeRequestService : IEmployeeRequestService
    {
        private readonly IRepository<EmployeeRequest, long> _repository;
        private readonly ILogger<EmployeeRequestService> _logger;
        private readonly IMapper _mapper;

        public EmployeeRequestService(IRepository<EmployeeRequest, long> Repository,
         ILogger<EmployeeRequestService> logger, IMapper mapper)
        {
            _repository = Repository;
            _logger = logger;
            _mapper = mapper;
        }
        public EmployeeRequestModel CreateEmployeeRequest(EmployeeRequestModel model)
        {
            var employeeRequestModel = _mapper.Map<EmployeeRequest>(model);

            try
            {
                var addedEmployeeRequest = _repository.Add(employeeRequestModel);
                if (addedEmployeeRequest != null)
                {
                    EmployeeRequestModel addedEmployeeRequestModel = new EmployeeRequestModel();
                    addedEmployeeRequestModel = _mapper.Map<EmployeeRequestModel>(addedEmployeeRequest);
                    return addedEmployeeRequestModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public bool DeleteEmployeeRequest(long id)
        {
            throw new NotImplementedException();
        }

        public List<EmployeeRequestModel> GetAllEmployeeRequests()
        {
            throw new NotImplementedException();
        }

        public List<EmployeeRequestModel> GetEmployeeRequest(long requestId)
        {
            try
            {
                List<EmployeeRequest> employeeRequests = _repository.Find(ER => ER.BenefitRequestId == requestId && ER.IsVisible == true, false, ER => ER.Employee, ER => ER.BenefitRequest, ER => ER.BenefitRequest.Benefit, ER => ER.BenefitRequest.Benefit.BenefitType, ER => ER.BenefitRequest.RequestStatus).ToList();
                List<EmployeeRequestModel> employeeRequestModels = _mapper.Map<List<EmployeeRequestModel>>(employeeRequests);
                return employeeRequestModels;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public EmployeeRequestModel GetEmployeeRequestByEmployeeNumber(long employeeNumber)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateEmployeeRequest(EmployeeRequestModel model)
        {
            throw new NotImplementedException();
        }
    }
}
