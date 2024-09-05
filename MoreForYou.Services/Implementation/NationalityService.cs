using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Implementation
{
    public class NationalityService : INationalityService
    {
        private readonly IRepository<Nationality, long> _repository;
        private readonly ILogger<NationalityService> _logger;
        private readonly IMapper _mapper;

        public NationalityService(IRepository<Nationality, long> nationalityRepository,
          ILogger<NationalityService> logger, IMapper mapper)
        {
            _repository = nationalityRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public Task<bool> CreateNationality(NationalityModel model)
        {
            var nationality = _mapper.Map<Nationality>(model);
            try
            {
                _repository.Add(nationality);

                return Task<bool>.FromResult<bool>(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return Task<bool>.FromResult<bool>(false);
        }

        public bool DeleteNationality(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<NationalityModel>> GetAllNationalities()
        {
            try
            {
                var nationalities = _repository.Find(i => i.IsVisible == true).ToList();
                var models = new List<NationalityModel>();
                models = _mapper.Map<List<NationalityModel>>(nationalities);
                return models;
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public NationalityModel GetNationality(int id)
        {
            try
            {
                var nationality = _repository.Find(i => i.IsVisible == true && i.Id == id);
                if(nationality.Any() == true)
                {
                    var model = new NationalityModel();
                    model = _mapper.Map<NationalityModel>(nationality.First());
                    return model;
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

        public Task<List<NationalityModel>> GetNationalityByName(NationalityModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateNationality(NationalityModel model)
        {
            var nationality = _mapper.Map<Nationality>(model);

            try
            {
               var response = _repository.Update(nationality);

                return Task<bool>.FromResult<bool>(response);

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return Task<bool>.FromResult<bool>(false);
        }
    }
}
