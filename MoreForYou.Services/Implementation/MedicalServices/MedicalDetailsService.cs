using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Models.Models.MasterModels.MedicalModels;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Contracts.Medical;
using MoreForYou.Services.Models.API.Medical;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MoreForYou.Services.Implementation.MedicalServices
{
    public class MedicalDetailsService : IMedicalDetailsService
    {
        private readonly IRepository<MedicalDetails, long> _repository;
        private readonly ILogger<MedicalDetailsService> _logger;
        private readonly IMapper _mapper;

        public MedicalDetailsService(IRepository<MedicalDetails, long> repository,
        ILogger<MedicalDetailsService> logger,
        IMapper mapper
        )
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<List<MedicalDetailsModel>> GetAllMedicalDetails()
        {
            try
            {
                var medicalDetails = _repository.Find(m => m.IsVisible == true, false, m => m.MedicalSubCategory, mbox => mbox.MedicalSubCategory.MedicalCategory).ToList();
                if (medicalDetails != null)
                {
                    List<MedicalDetailsModel> MedicalDetailsModels = _mapper.Map<List<MedicalDetailsModel>>(medicalDetails);
                    return MedicalDetailsModels;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<MedicalDetailsModel>> GetMedicalDetailsForCountry(string Country)
        {
            try
            {
                var medicalDetails = _repository.Find(m => m.IsVisible == true && m.Country == Country, false, m => m.MedicalSubCategory, mbox => mbox.MedicalSubCategory.MedicalCategory).ToList();
                if (medicalDetails != null)
                {
                    List<MedicalDetailsModel> MedicalDetailsModels = _mapper.Map<List<MedicalDetailsModel>>(medicalDetails);
                    return MedicalDetailsModels;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<MedicalDetailsAPIModel> ConvertMedicalDetailsModelToMedicalDetailsAPIModel(List<MedicalDetailsModel> MedicalDetailsModels, int languageId)
        {
            try
            {
                List<MedicalDetailsAPIModel> medicalDetailsAPIModels = new List<MedicalDetailsAPIModel>();
                if (MedicalDetailsModels != null)
                {
                    foreach (var model in MedicalDetailsModels)
                    {
                        MedicalDetailsAPIModel medicalDetailsAPIModel = new MedicalDetailsAPIModel();
                        //{
                        if (languageId == (int)CommanData.Languages.English)
                        {
                            medicalDetailsAPIModel.MedicalDetailsName = model.Name_EN;
                            medicalDetailsAPIModel.MedicalDetailsAddress = model.Address_EN;
                            medicalDetailsAPIModel.MedicalDetailsMobile = model.Mobile;
                            medicalDetailsAPIModel.MedicalDetailsWorkingHours = model.WorkingHours_EN;
                            medicalDetailsAPIModel.SubCategoryName = model.MedicalSubCategory.Name_EN;
                            medicalDetailsAPIModel.CategoryName = model.MedicalSubCategory.MedicalCategory.Name_EN;
                        }
                        else if (languageId == (int)CommanData.Languages.Arabic)
                        {
                            medicalDetailsAPIModel.MedicalDetailsName = model.Name_AR;
                            medicalDetailsAPIModel.MedicalDetailsAddress = model.Address_AR;
                            medicalDetailsAPIModel.MedicalDetailsMobile = model.Mobile;
                            medicalDetailsAPIModel.MedicalDetailsWorkingHours = model.WorkingHours_AR;
                            medicalDetailsAPIModel.SubCategoryName = model.MedicalSubCategory.Name_AR;
                            medicalDetailsAPIModel.CategoryName = model.MedicalSubCategory.MedicalCategory.Name_AR;
                        }
                        medicalDetailsAPIModel.MedicalDetailsImage = CommanData.Url + CommanData.MedicalDetailsFolder + model.Image;
                        medicalDetailsAPIModels.Add(medicalDetailsAPIModel);
                    }
                    return medicalDetailsAPIModels;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<List<MedicalDetailsModel>> GetMedicalDetailsBySubCategoryId(long SubCategoryId, string Country)
        {
            try
            {
                var medicalDetails = await _repository.Find(m => m.IsVisible == true && m.MedicalSubCategory.Id == SubCategoryId && m.Country == Country, false, m => m.MedicalSubCategory).ToListAsync();
                if (medicalDetails != null)
                {
                    List<MedicalDetailsModel> MedicalDetailsModels = _mapper.Map<List<MedicalDetailsModel>>(medicalDetails);
                    return MedicalDetailsModels;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<MedicalDetailsModel> GetMedicalDetailsById(int MedicalDetailsId, string Country)
        {
            try
            {
                var medicalDetail = await _repository.Find(m => m.IsVisible == true && m.Id == MedicalDetailsId && m.Country == Country, false).FirstOrDefaultAsync();
                if (medicalDetail != null)
                {
                    MedicalDetailsModel MedicalDetailsModel = _mapper.Map<MedicalDetailsModel>(medicalDetail);
                    return MedicalDetailsModel;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<MedicalDetailsModel> GetMedicalEntityById(int? MedicalDetailsId)
        {
            if (MedicalDetailsId != null)
            {
                //return new MedicalDetailsModel();
                try
                {
                    MedicalDetails source = await _repository.Find(m => m.IsVisible == true && m.Id == MedicalDetailsId, false, m => m.MedicalSubCategory).FirstOrDefaultAsync();
                    return source == null ? null : _mapper.Map<MedicalDetailsModel>(source);
                }
                catch (Exception ex)
                {
                    return null;
                }
                //throw new NotImplementedException();
            }
            else { return null; }
        }

        public List<MedicalDetailsAPIModel> ConvertMedicalDetailsModelToMedicalDetailsAPIModelWithId(List<MedicalDetailsModel> MedicalDetailsModels, int languageId)
        {
            try
            {
                List<MedicalDetailsAPIModel> detailsApiModelWithId = new List<MedicalDetailsAPIModel>();
                if (MedicalDetailsModels == null)
                    return (List<MedicalDetailsAPIModel>)null;
                foreach (MedicalDetailsModel medicalDetailsModel in MedicalDetailsModels)
                {
                    MedicalDetailsAPIModel medicalDetailsApiModel = new MedicalDetailsAPIModel();
                    switch (languageId)
                    {
                        case 1:
                            medicalDetailsApiModel.MedicalEntityId = medicalDetailsModel.Id.ToString();
                            medicalDetailsApiModel.MedicalDetailsName = medicalDetailsModel.Name_EN;
                            medicalDetailsApiModel.MedicalDetailsAddress = medicalDetailsModel.Address_EN;
                            medicalDetailsApiModel.MedicalDetailsMobile = medicalDetailsModel.Mobile;
                            medicalDetailsApiModel.MedicalDetailsWorkingHours = medicalDetailsModel.WorkingHours_EN;
                            medicalDetailsApiModel.SubCategoryName = medicalDetailsModel.MedicalSubCategory.Name_EN;
                            medicalDetailsApiModel.CategoryName = medicalDetailsModel.MedicalSubCategory.MedicalCategory.Name_EN;
                            break;
                        case 2:
                            medicalDetailsApiModel.MedicalEntityId = medicalDetailsModel.Id.ToString();
                            medicalDetailsApiModel.MedicalDetailsName = medicalDetailsModel.Name_AR;
                            medicalDetailsApiModel.MedicalDetailsAddress = medicalDetailsModel.Address_AR;
                            medicalDetailsApiModel.MedicalDetailsMobile = medicalDetailsModel.Mobile;
                            medicalDetailsApiModel.MedicalDetailsWorkingHours = medicalDetailsModel.WorkingHours_AR;
                            medicalDetailsApiModel.SubCategoryName = medicalDetailsModel.MedicalSubCategory.Name_AR;
                            medicalDetailsApiModel.CategoryName = medicalDetailsModel.MedicalSubCategory.MedicalCategory.Name_AR;
                            break;
                    }
                    medicalDetailsApiModel.MedicalDetailsImage = CommanData.Url + CommanData.MedicalDetailsFolder + medicalDetailsModel.Image;
                    detailsApiModelWithId.Add(medicalDetailsApiModel);
                }
                return detailsApiModelWithId;
            }
            catch (Exception ex)
            {
                return (List<MedicalDetailsAPIModel>)null;
            }
            //throw new NotImplementedException();
        }
    }
}
