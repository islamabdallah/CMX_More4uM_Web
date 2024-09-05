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
    public class BenefitTypeService : IBenefitTypeService
    {
        private readonly IRepository<BenefitType, long> _repository;
        private readonly ILogger<BenefitTypeService> _logger;
        private readonly IMapper _mapper;

        public BenefitTypeService(IRepository<BenefitType, long> benefitTypeRepository,
          ILogger<BenefitTypeService> logger, IMapper mapper)
        {
            _repository = benefitTypeRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Create Benefit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public BenefitTypeModel CreateBenefitType(BenefitTypeModel model)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBenefitType(long id)
        {
            throw new NotImplementedException();
        }

        public List<BenefitTypeModel> GetAllBenefitTypes()
        {
            try
            {
                var benefits = _repository.Find(i => i.IsVisible == true).ToList();
                var models = new List<BenefitTypeModel>();
                models = _mapper.Map<List<BenefitTypeModel>>(benefits);
                return models;
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public BenefitTypeModel GetBenefitType(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BenefitTypeModel>> GetBenefitTypeByName(BenefitTypeModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateBenefitType(BenefitTypeModel model)
        {
            throw new NotImplementedException();
        }
    }
}
