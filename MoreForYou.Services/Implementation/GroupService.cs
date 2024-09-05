using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MoreForYou.Models.Models.MasterModels;
using System.Linq;

namespace MoreForYou.Services.Implementation
{
    public class GroupService : IGroupService
    {
        private readonly IRepository<Group, long> _repository;
        private readonly ILogger<GroupService> _logger;
        private readonly IMapper _mapper;

        public GroupService(IRepository<Group, long> benefitRepository,
          ILogger<GroupService> logger, IMapper mapper)
        {
            _repository = benefitRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public GroupModel CreateGroup(GroupModel model)
        {
            Group group = _mapper.Map<Group>(model);

            try
            {
                var addedGroup = _repository.Add(group);
                if (addedGroup != null)
                {
                    GroupModel addedGroupModel = new GroupModel();
                    addedGroupModel = _mapper.Map<GroupModel>(addedGroup);
                    return addedGroupModel;
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

        public bool DeleteGroup(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<GroupModel>> GetAllGroups()
        {
            throw new NotImplementedException();
        }

        public async Task<List<GroupModel>> GetGroupByBenefitId(long Id)
        {
            try
            {
               var groups = _repository.Find(g => g.BenefitId == Id && g.IsVisible == true, false);
               List<GroupModel> groupModels = _mapper.Map<List<GroupModel>>(groups);
                return groupModels;
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public GroupModel GetGroup(long id)
        {
            try
            {
                Group group = _repository.Find(g=>g.Id == id && g.IsVisible == true, false, g=>g.Benefit, global=>global.RequestStatus).First();
                GroupModel groupModel = _mapper.Map<GroupModel>(group);
                return groupModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public bool UpdateGroup(GroupModel model)
        {
            Group group = _mapper.Map<Group>(model);
            try
            {

                _repository.Update(group);

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return false;
        }
    }
}
