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
    public class CompanyService : ICompanyService
    {
        private readonly IRepository<Company, int> _repository;
        private readonly ILogger<CompanyService> _logger;
        private readonly IMapper _mapper;

        public CompanyService(IRepository<Company, int> Repository,
          ILogger<CompanyService> logger, 
          IMapper mapper)
        {
            _repository = Repository;
            _logger = logger;
            _mapper = mapper;
        }
        public CompanyModel CreateCompany(CompanyModel model)
        {
            try
            {
               var company = _mapper.Map<Company>(model);
               var addedCompany = _repository.Add(company);
               CompanyModel companyModel = _mapper.Map<CompanyModel>(addedCompany);

                return companyModel;
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public bool DeleteCompany(int id)
        {
            throw new NotImplementedException();
        }

        public List<CompanyModel> GetAllCompanies()
        {
            try
            {
                var companies = _repository.Find(i => i.IsVisible == true);
                var models = new List<CompanyModel>();
                models = _mapper.Map<List<CompanyModel>>(companies);
                return models;
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public CompanyModel GetCompany(long Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateCompany(CompanyModel model)
        {
            throw new NotImplementedException();
        }
    }
}
