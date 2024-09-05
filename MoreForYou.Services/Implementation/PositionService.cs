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
    public class PositionService : IPositionService
    {
        private readonly IRepository<Position, long> _repository;
        private readonly ILogger<PositionService> _logger;
        private readonly IMapper _mapper;

        public PositionService(IRepository<Position, long> PositionRepository,
          ILogger<PositionService> logger, IMapper mapper)
        {
            _repository = PositionRepository;
            _logger = logger;
            _mapper = mapper;
        }


        public Task<bool> CreatePosition(PositionModel model)
        {
            var position = _mapper.Map<Position>(model);
            try
            {
                _repository.Add(position);

                return Task<bool>.FromResult<bool>(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return Task<bool>.FromResult<bool>(false);
        }

        public bool DeletePosition(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PositionModel>> GetAllPositions()
        {
            try
            {
                var positions = _repository.Find(i => i.IsVisible == true).ToList();
                var models = new List<PositionModel>();
                models = _mapper.Map<List<PositionModel>>(positions);
                return models;
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public PositionModel GetPosition(int id)
        {
            try
            {
                var position = _repository.Find(d => d.IsVisible == true && d.Id == id);
                if(position.Any() == true)
                {
                    PositionModel positionModel = _mapper.Map<PositionModel>(position.First());
                    return positionModel;
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

        public async Task<PositionModel> GetPositionByName(string name)
        {
            try
            {
                var position = _repository.Find(d => d.IsVisible == true && d.Name == name);
                if (position.Any() == true)
                {
                    PositionModel positionModel = _mapper.Map<PositionModel>(position);
                    return positionModel;
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

        public Task<bool> UpdatePosition(PositionModel model)
        {
            var position = _mapper.Map<Position>(model);

            try
            {
                _repository.Update(position);

                return Task<bool>.FromResult<bool>(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return Task<bool>.FromResult<bool>(false);
        }
    }
}
