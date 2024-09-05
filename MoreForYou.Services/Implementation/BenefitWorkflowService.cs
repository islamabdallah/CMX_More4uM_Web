using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Service.Contracts.Auth;
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
    public class BenefitWorkflowService: IBenefitWorkflowService
    {

        private readonly IRepository<BenefitWorkflow, long> _repository;
        private readonly ILogger<BenefitWorkflowService> _logger;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        public BenefitWorkflowService(IRepository<BenefitWorkflow, long> benefitRepository,
          ILogger<BenefitWorkflowService> logger, IMapper mapper,
          IRoleService roleService)
        {
            _repository = benefitRepository;
            _logger = logger;
            _mapper = mapper;
            _roleService = roleService;
        }

        public Task<bool> CreateBenefitWorkflow(BenefitWorkflowModel model)
        {
            var benefitworkflow = _mapper.Map<BenefitWorkflow>(model);

            try
            {
                var addedBenefit = _repository.Add(benefitworkflow);

                return Task<bool>.FromResult<bool>(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return Task<bool>.FromResult<bool>(false);
        }

        public bool DeleteBenefitWorkflow(long id)
        {
            bool response = false;
            try
            {
                response = _repository.DeleteById(id);
                return response;
            }
            catch(Exception e)
            {
                return response;
            }
        }

        public Task<List<BenefitWorkflowModel>> GetAllBenefitWorkflows()
        {
            throw new NotImplementedException();
        }

        public BenefitWorkflowModel GetBenefitWorkflow(long id)
        {
            throw new NotImplementedException();
        }

        public List<BenefitWorkflowModel> GetBenefitWorkflowS(long BenefitId)
        {
            try
            {
                var benefitWorkflows = _repository.Find(w => w.BenefitId == BenefitId && w.IsVisible == true).ToList();
                List<BenefitWorkflowModel> benefitWorkflowModels = _mapper.Map<List<BenefitWorkflowModel>>(benefitWorkflows);
                return benefitWorkflowModels;
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }
        public Task<List<BenefitWorkflowModel>> GetBenefitWorkflowByName(BenefitWorkflowModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateBenefitWorkflow(BenefitWorkflowModel model)
        {
            var benefitworkflow = _mapper.Map<BenefitWorkflow>(model);

            try
            {
                var addedBenefit = _repository.Update(benefitworkflow);

                return Task<bool>.FromResult<bool>(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return Task<bool>.FromResult<bool>(false);
        }

        public List<string> CreateBenefitWorkFlow(BenefitModel model, int languageId)
        {
            try
            {
                List<BenefitWorkflowModel> benefitWorkflowModels = GetBenefitWorkflowS(model.Id);
                List<string> benefitWorkflowString = new List<string>();
                string workflow = "";
                benefitWorkflowModels = benefitWorkflowModels.OrderBy(w => w.Order).ToList();
                foreach (BenefitWorkflowModel benefitWorkflowModel in benefitWorkflowModels)
                {
                    if(languageId == (int)CommanData.Languages.Arabic)
                    {
                        workflow = _roleService.GetRole(benefitWorkflowModel.RoleId).Result.ArabicRoleName;
                    }
                    else if (languageId == (int)CommanData.Languages.English)
                    {
                        workflow = _roleService.GetRole(benefitWorkflowModel.RoleId).Result.Name;
                    }
                    benefitWorkflowString.Add(workflow);
                }

                return benefitWorkflowString;
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
                
        }

        public BenefitWorkflowModel GetBenefitWorkflow(long benefitId, string roleId)
        {
            try
            {
                var benefitWorkflow = _repository.Find(w => w.BenefitId == benefitId && w.RoleId == roleId);
                if(benefitWorkflow.Any())
                {
                    BenefitWorkflowModel benefitWorkflowModel = _mapper.Map<BenefitWorkflowModel>(benefitWorkflow.First());
                    return benefitWorkflowModel;
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }
    }
}
