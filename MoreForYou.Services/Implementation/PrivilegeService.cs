using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Implementation
{
    public class PrivilegeService : IPrivilegeService
    {
        private readonly IRepository<Privilege, long> _repository;
        private readonly ILogger<PrivilegeService> _logger;
        private readonly IMapper _mapper;

        public PrivilegeService(IRepository<Privilege, long> repository,
            ILogger<PrivilegeService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<PrivilegeModel> CreatePrivilege(PrivilegeModel model)
        {
            var privilege = _mapper.Map<Privilege>(model);

            try
            {
                var addedPrivilege = _repository.Add(privilege);
                if (addedPrivilege != null)
                {
                    PrivilegeModel addedPrivilegeModel = new PrivilegeModel();
                    addedPrivilegeModel = _mapper.Map<PrivilegeModel>(addedPrivilege);
                    return addedPrivilegeModel;
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

        public bool DeletePrivilege(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PrivilegeModel>> GetAllPrivileges()
        {
            try
            {
                var privileges = _repository.Find(i => i.IsVisible == true).ToList();
                var models = new List<PrivilegeModel>();
                models = _mapper.Map<List<PrivilegeModel>>(privileges);
                return models;
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public PrivilegeModel GetPrivilege(long Id)
        {
            try
            {
                var privilege = _repository.Find(p => p.Id == Id && p.IsVisible == true).First();
                PrivilegeModel privilegeModel = _mapper.Map<PrivilegeModel>(privilege);
                return privilegeModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public PrivilegeModel GetPrivilegeByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePrivilege(PrivilegeModel model)
        {
            var privilege = _mapper.Map<Privilege>(model);

            try
            {
                _repository.Update(privilege);

                return Task<bool>.FromResult<bool>(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return Task<bool>.FromResult<bool>(false);
        }

        public List<PriviligeAPIModel> CreatePriviligeAPIModel(List<PrivilegeModel> privilegeModels)
        {
            List<PriviligeAPIModel> priviligeAPIModels = new List<PriviligeAPIModel>();
            foreach (var privilige in privilegeModels)
            {
                PriviligeAPIModel priviligeAPIModel = new PriviligeAPIModel();
                priviligeAPIModel.Name = privilige.Name;
                priviligeAPIModel.Description = privilige.Description;
                priviligeAPIModel.Image = privilige.Image;
                priviligeAPIModel.Priority = privilige.Priority;
                priviligeAPIModels.Add(priviligeAPIModel);
            }

            return priviligeAPIModels;
        }
    }
}
