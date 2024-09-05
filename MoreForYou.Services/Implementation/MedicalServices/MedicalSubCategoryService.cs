using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MasterModels.MedicalModels;
using MoreForYou.Services.Contracts.Medical;
using MoreForYou.Services.Models.API.Medical;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;

namespace MoreForYou.Services.Implementation.MedicalServices
{
    public class MedicalSubCategoryService : IMedicalSubCategoryService
    {
        private readonly IRepository<MedicalSubCategory, long> _repository;
        private readonly ILogger<MedicalSubCategoryService> _logger;
        private readonly IMapper _mapper;

        public MedicalSubCategoryService(IRepository<MedicalSubCategory, long> repository,
        ILogger<MedicalSubCategoryService> logger,
        IMapper mapper
        )
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<MedicalSubCategoryModel>> GetAllMedicalSubCategories()
        {
            try
            {
                var medicalCategories = await _repository.Find(s => s.IsVisible == true, false, s=>s.MedicalCategory).ToListAsync();
                if (medicalCategories != null)
                {
                    List<MedicalSubCategoryModel> MedicalSubCategoriesModels = _mapper.Map<List<MedicalSubCategoryModel>>(medicalCategories);
                    return MedicalSubCategoriesModels;
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

        public async Task<List<MedicalSubCategoryModel>> GetMedicalSubCategoryModelsByCategoryId(int CategoryId)
        {
            try
            {
                var medicalCategories = await _repository.Find(s => s.IsVisible == true && s.MedicalCategory.Id ==CategoryId, false, s => s.MedicalCategory).ToListAsync();
                if (medicalCategories != null)
                {
                    List<MedicalSubCategoryModel> MedicalSubCategoriesModels = _mapper.Map<List<MedicalSubCategoryModel>>(medicalCategories);
                 
                    return MedicalSubCategoriesModels;
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


        public List<MedicalSubCategoryAPIModel> ConvertMedicalSubCategoriesModelToMedicalSubCategoriesAPIModel(List<MedicalSubCategoryModel> MedicalSubCategoryModels, int languageId)
        {
            try
            {
                string CategoryName = "";
                string SubCategoryName = "";
                List<MedicalSubCategoryAPIModel> medicalSubCategoryAPIModels = new List<MedicalSubCategoryAPIModel>();
                if (MedicalSubCategoryModels != null)
                {
                    foreach (var submodel in MedicalSubCategoryModels)
                    {
                        if (languageId == (int)CommanData.Languages.English)
                        {
                            CategoryName = submodel.MedicalCategory.Name_EN;
                            SubCategoryName = submodel.Name_EN;
                        }
                        else if (languageId == (int)CommanData.Languages.Arabic)
                        {
                            CategoryName = submodel.MedicalCategory.Name_AR;
                            SubCategoryName = submodel.Name_AR;
                        }
                        MedicalSubCategoryAPIModel medicalSubCategoryAPIModel = new MedicalSubCategoryAPIModel
                        {
                            SubCategoryName = SubCategoryName,
                            SubCategoryImage = CommanData.Url + CommanData.MedicalSubCategoryFolder + submodel.Image,
                            CategoryName = CategoryName,
                            CategoryImage = CommanData.Url + CommanData.MedicalCategoryFolder + submodel.MedicalCategory.Image,

                        };
                        medicalSubCategoryAPIModels.Add(medicalSubCategoryAPIModel);
                    }
                    return medicalSubCategoryAPIModels;
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

    }
}
