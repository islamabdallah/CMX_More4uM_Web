using AutoMapper;
using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Implementation
{
    public class GroupEmployeeService : IGroupEmployeeService
    {
            private readonly IRepository<GroupEmployee, long> _repository;
            private readonly ILogger<GroupEmployeeService> _logger;
            private readonly IMapper _mapper;
            private readonly UserManager<AspNetUser> _userManager;

        public GroupEmployeeService(IRepository<GroupEmployee, long> Repository,
              ILogger<GroupEmployeeService> logger, IMapper mapper,
              UserManager<AspNetUser> userManager)
            {
                _repository = Repository;
                _logger = logger;
                _mapper = mapper;
                _userManager = userManager;
            }
            public GroupEmployeeModel CreateGroupEmployee(GroupEmployeeModel model)
        {
            GroupEmployee groupEmployee = _mapper.Map<GroupEmployee>(model);

            try
            {
                var addedGroupEmployee = _repository.Add(groupEmployee);
                if (addedGroupEmployee != null)
                {
                    GroupEmployeeModel addedGroupEmployeeModel = new GroupEmployeeModel();
                    addedGroupEmployeeModel = _mapper.Map<GroupEmployeeModel>(addedGroupEmployee);
                    return addedGroupEmployeeModel;
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

        public async Task<List<GroupEmployeeModel>> GetAllGroupEmployees()
        {
            try
            {
                var employeeGroups = await _repository.Find(ge=>ge.EmployeeId != 0, false, ge=>ge.Group, ge => ge.Employee).ToListAsync();
                var models = new List<GroupEmployeeModel>();
                models = _mapper.Map<List<GroupEmployeeModel>>(employeeGroups);
                return models;
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public async Task<List<GroupEmployeeModel>> GetGroupParticipants(long id)
        {
            try
            {
               var groupEmployees = await _repository.Find(ge => ge.GroupId == id, false, ge => ge.Employee, ge => ge.Employee.Department, ge => ge.Employee.Position, ge => ge.Employee.Company, ge => ge.Employee.Nationality, ge => ge.Employee.Supervisor).ToListAsync();
                List<GroupEmployeeModel> groupEmployeeModels = _mapper.Map<List<GroupEmployeeModel>>(groupEmployees);

                return groupEmployeeModels;

            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public Task<bool> UpdateGroupEmployee(GroupEmployeeModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<GroupEmployeeModel>> GetGroupsByEmployeeId(long employeeId)
        {
            try
            {
                var groupEmployees = await _repository.Find(ge => ge.EmployeeId == employeeId, false, ge => ge.Employee, ge => ge.Employee.Department, ge => ge.Employee.Position, ge => ge.Employee.Nationality, ge => ge.Employee.Supervisor, ge => ge.Employee.Company, ge => ge.Group, ge => ge.Group.RequestStatus).ToListAsync();
                List<GroupEmployeeModel> groupEmployeeModels = _mapper.Map<List<GroupEmployeeModel>>(groupEmployees);

                return groupEmployeeModels;

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }


        public async Task<List<GroupEmployeeModel>> GetGroupsByBenefitId(long benefitId)
        {
            try
            {
                var groupEmployees = await _repository.Find(ge => ge.EmployeeId != 0 && ge.Group.BenefitId == benefitId, false, ge => ge.Employee, ge => ge.Group, ge => ge.Group.RequestStatus).ToListAsync();
                List<GroupEmployeeModel> groupEmployeeModels = _mapper.Map<List<GroupEmployeeModel>>(groupEmployees);

                return groupEmployeeModels;

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public int GetTimesEmployeeReceieveThisGroupBenefit(long employeeNumber, long benefitId)
        {
            try
            {
                int times = 0;

                List<GroupEmployee> groupEmployees = _repository.Find(ge => ge.EmployeeId == employeeNumber && ge.Group.BenefitId == benefitId && ge.Group.RequestStatusId == (int)CommanData.BenefitStatus.Approved).ToList();
                if(groupEmployees != null)
                {
                    times = groupEmployees.Count;
                }
                else
                {
                    times = 0;
                }
                return times;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return -1;
            }
        }
        public bool ISEmployeeHasHoldingRequestsForthisGroupBenefit(long employeeNumber, long benefitId)
        {
            try
            {
                var times = _repository.Find(ge => ge.EmployeeId == employeeNumber && ge.Group.BenefitId == benefitId && (ge.Group.RequestStatusId == (int)CommanData.BenefitStatus.Pending || ge.Group.RequestStatusId == (int)CommanData.BenefitStatus.InProgress));
                if (times.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return false;
            }
        }

        public async Task<List<string>> GetGroupParticipantsMails(long id)
        {
            try
            {
                var groupEmployees = _repository.Find(ge => ge.GroupId == id, false, ge => ge.Employee).Select(e=>e.Employee);
                List<string> groupEmployeeMails = new List<string>();
                AspNetUser user = new AspNetUser();
                if (groupEmployees != null)
                {
                    foreach(var employee in groupEmployees)
                    {
                        user = await _userManager.FindByIdAsync(employee.UserId);
                        groupEmployeeMails.Add(user.Email);
                    }
                }

                return groupEmployeeMails;

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public async Task<List<GroupEmployeeModel>> GetGroupParticipantsBasicData(long id)
        {
            try
            {
                var groupEmployees = await _repository.Find(ge => ge.GroupId == id, false, ge => ge.Employee, ge => ge.Employee.Department, ge => ge.Employee.Position, ge => ge.Employee.Company, ge => ge.Employee.Nationality, ge => ge.Employee.Supervisor).ToListAsync();
                List<GroupEmployeeModel> groupEmployeeModels = _mapper.Map<List<GroupEmployeeModel>>(groupEmployees);
                return groupEmployeeModels;

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }
    }
}
