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
    public class MobileVersionService : IMobileVersionService
    {
        private readonly IRepository<MobileVersion, int> _repository;
        private readonly ILogger<MobileVersionService> _logger;
        private readonly IMapper _mapper;

        public MobileVersionService(IRepository<MobileVersion, int> repository,
           ILogger<MobileVersionService> logger,
           IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<bool> CreateMobileVersion(MobileVersionModel mobileVersionModel)
        {
            try
            {
                MobileVersion model= _mapper.Map<MobileVersion>(mobileVersionModel);
                var addedModel =_repository.Add(model);
                if(addedModel != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public async Task<List<MobileVersionModel>> GetAllMobileVersion()
        {
            try
            {
                List<MobileVersionModel> mobileVersionModels = new List<MobileVersionModel>();
                var mobileVersions = _repository.Find(m=>m.IsVisible == true);
                if (mobileVersions != null)
                {
                    mobileVersionModels = _mapper.Map<List<MobileVersionModel>>(mobileVersions.ToList());
                    return mobileVersionModels;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<MobileVersionModel> GetLastMobileVersion()
        {
            try
            {
                MobileVersionModel mobileVersionModel = new MobileVersionModel();
                List<MobileVersion> mobileVersions = _repository.Find(m => m.IsVisible == true).ToList();
                if (mobileVersions != null)
                {
                    var lastVersion = mobileVersions.OrderByDescending(m => m.CreatedDate).FirstOrDefault();
                    mobileVersionModel = _mapper.Map<MobileVersionModel>(lastVersion);
                    return mobileVersionModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
