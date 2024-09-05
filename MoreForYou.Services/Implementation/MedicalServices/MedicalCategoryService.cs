
using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MasterModels.MedicalModels;
using MoreForYou.Services.Contracts.Medical;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.API.Medical;
using MoreForYou.Services.Models.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Implementation.MedicalServices
{
    public class MedicalCategoryService: IMedicalCategoryService
    {
        private readonly IRepository<MedicalCategory, long> _repository;
        private readonly ILogger<MedicalCategoryService> _logger;
        private readonly IMapper _mapper;

        public MedicalCategoryService(IRepository<MedicalCategory, long> repository,
        ILogger<MedicalCategoryService> logger,
        IMapper mapper
        )
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }


        public List<MedicalCategoryAPIModel> ConvertMedicalCategoriesModelToMedicalCategoriesAPIModel(List<MedicalCategoryModel> MedicalCategoryModels, int languageId)
        {
            try
            {
                string CategoryName = "";
                List<MedicalCategoryAPIModel> medicalCategoryAPIModels= new List<MedicalCategoryAPIModel>();
               if(MedicalCategoryModels != null )
                {
                    foreach (var model in MedicalCategoryModels)
                    {
                        if(languageId == (int)CommanData.Languages.English)
                        {
                            CategoryName = model.Name_EN;
                        }
                        else if (languageId == (int)CommanData.Languages.Arabic)
                        {
                            CategoryName = model.Name_AR;
                        }
                        MedicalCategoryAPIModel medicalCategoryAPIModel = new MedicalCategoryAPIModel
                        {
                            CategoryName = CategoryName,
                            CategoryImage = CommanData.Url + CommanData.MedicalCategoryFolder + model.Image
                        };
                        medicalCategoryAPIModels.Add(medicalCategoryAPIModel);
                    }
                    return medicalCategoryAPIModels;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<List<MedicalCategoryModel>> GetAllMedicalCategories()
        {
            try
            {
                var medicalCategories = _repository.Find(m => m.IsVisible == true).ToList();
                if (medicalCategories != null)
                {
                    List<MedicalCategoryModel> MedicalCategoriesModels = _mapper.Map<List<MedicalCategoryModel>>(medicalCategories);
                    return MedicalCategoriesModels;
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
