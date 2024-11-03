using AutoMapper;
using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MedicalModels;
using MoreForYou.Models.Models;
using MoreForYou.Service.Contracts.Auth;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Contracts.Medical;
using MoreForYou.Services.Models.API.Medical;
using MoreForYou.Services.Models.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MoreForYou.Services.Implementation.MedicalServices
{
    public class RelativeService : IRelativeService
    {
        private readonly
#nullable disable
    IRepository<Relative, long> _repository;
        private readonly ILogger<RelativeService> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IRoleService _roleService;
        private readonly IConfiguration _configuration;
        private readonly IMedicalCategoryService _medicalCategoryService;
        private readonly IMedicalSubCategoryService _medicalSubCategoryService;
        private readonly IMedicalDetailsService _medicalDetailsService;
        private readonly IEmployeeService _employeeService;

        public RelativeService(
          IRepository<Relative, long> employeeRepository,
          ILogger<RelativeService> logger,
          IMapper mapper,
          UserManager<AspNetUser> userManager,
          IRoleService roleService,
          IConfiguration configuration,
          IMedicalCategoryService medicalCategoryService,
          IMedicalSubCategoryService medicalSubCategoryService,
          IMedicalDetailsService medicalDetailsService,
          IEmployeeService employeeService)
        {
            this._repository = employeeRepository;
            this._logger = logger;
            this._mapper = mapper;
            this._userManager = userManager;
            this._roleService = roleService;
            this._configuration = configuration;
            this._medicalCategoryService = medicalCategoryService;
            this._medicalSubCategoryService = medicalSubCategoryService;
            this._medicalDetailsService = medicalDetailsService;
            this._employeeService = employeeService;
        }

        public Task<bool> CreateEmployeeRelative(Relative model)
        {
            try
            {
                return this._repository.Add(model) != null ? Task.FromResult<bool>(true) : Task.FromResult<bool>(false);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.ToString());
            }
            return Task.FromResult<bool>(false);
        }

        public async Task<List<Relative>> GetEmployeeRelatives(long employeeNumber)
        {
            try
            {
                List<Relative> listAsync = await this._repository.Find((Expression<Func<Relative, bool>>)(i => i.IsVisible == true && i.IsActive == true && i.EmployeeNumber == employeeNumber)).ToListAsync();
                if (listAsync != null)
                    return listAsync;
                else return null;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.ToString());
            }
            return (List<Relative>)null;
        }

        public async Task<EmployeeRelativesApi> GetEmployeeRelativesApi(
          long userNumber,
          int languageCode,
          int type,
          string Country)
        {
            List<Relative> employeeRelatives = await this.GetEmployeeRelatives(userNumber);
            if (employeeRelatives != null)
            {
                if (employeeRelatives.Count > 0)
                {
                    EmployeeRelativesApi employeeRelativesApi = new EmployeeRelativesApi();
                    EmployeeRelativesApiModel relativesApiModel = new EmployeeRelativesApiModel();
                    List<RelativeApiModel> relativeApiModelList = new List<RelativeApiModel>();
                    if (languageCode == 2)
                    {
                        foreach (Relative relative in employeeRelatives)
                        {
                            if (relative.Relation == "Self")
                            {
                                relativesApiModel.RelativeId = relative.Id;
                                relativesApiModel.CemexId = (int)relative.EmployeeNumber;
                                relativesApiModel.EmployeeName = relative.Relatives;
                                relativesApiModel.BirthDate = relative.BDate;
                                relativesApiModel.MedicalCoverage = relative.CoverPercentage;
                            }
                            else
                                relativeApiModelList.Add(new RelativeApiModel()
                                {
                                    RelativeId = relative.Id,
                                    RelativeName = relative.ArabicRelatives,
                                    Relation = relative.ArabicRelation,
                                    BirthDate = relative.BDate,
                                    Order = relative.Order,
                                    MedicalCoverage = relative.CoverPercentage
                                });
                        }
                    }
                    else
                    {
                        foreach (Relative relative in employeeRelatives)
                        {
                            if (relative.Relation == "Self")
                            {
                                relativesApiModel.RelativeId = relative.Id;
                                relativesApiModel.CemexId = (int)relative.EmployeeNumber;
                                relativesApiModel.EmployeeName = relative.Relatives;
                                relativesApiModel.BirthDate = relative.BDate;
                                relativesApiModel.MedicalCoverage = relative.CoverPercentage;
                            }
                            else
                                relativeApiModelList.Add(new RelativeApiModel()
                                {
                                    RelativeId = relative.Id,
                                    RelativeName = relative.Relatives,
                                    Relation = relative.Relation,
                                    BirthDate = relative.BDate,
                                    Order = relative.Order,
                                    MedicalCoverage = relative.CoverPercentage
                                });
                        }
                    }
                    relativesApiModel.Relatives = relativeApiModelList;
                    employeeRelativesApi.RelativesApiModel = relativesApiModel;
                    List<MedicalCategoryModel> medicalCategoryModels = await this._medicalCategoryService.GetAllMedicalCategories();
                    medicalCategoryModels = type != 1 ? medicalCategoryModels.Where<MedicalCategoryModel>((Func<MedicalCategoryModel, bool>)(a => a.Id != 4L)).ToList<MedicalCategoryModel>() : medicalCategoryModels.Where<MedicalCategoryModel>((Func<MedicalCategoryModel, bool>)(a => a.Id == 4L)).ToList<MedicalCategoryModel>();
                    if (medicalCategoryModels != null && medicalCategoryModels.Count > 0)
                        employeeRelativesApi.medicalCategoryAPIModels = this._medicalCategoryService.ConvertMedicalCategoriesModelToMedicalCategoriesAPIModel(medicalCategoryModels, languageCode);
                    List<MedicalSubCategoryModel> medicalSubCategoryModels = await this._medicalSubCategoryService.GetAllMedicalSubCategories();
                    long[] lineIDs = medicalCategoryModels.Select<MedicalCategoryModel, long>((Func<MedicalCategoryModel, long>)(l => l.Id)).ToArray<long>();
                    medicalSubCategoryModels = medicalSubCategoryModels.Where<MedicalSubCategoryModel>((Func<MedicalSubCategoryModel, bool>)(t => ((IEnumerable<long>)lineIDs).Contains<long>(t.MedicalCategory.Id))).ToList<MedicalSubCategoryModel>();
                    if (medicalSubCategoryModels != null && medicalSubCategoryModels.Count > 0)
                        employeeRelativesApi.medicalSubCategoryAPIModels = this._medicalSubCategoryService.ConvertMedicalSubCategoriesModelToMedicalSubCategoriesAPIModel(medicalSubCategoryModels, languageCode);
                    List<MedicalDetailsModel> detailsForCountry = await this._medicalDetailsService.GetMedicalDetailsForCountry(Country);
                    long[] lineID = medicalSubCategoryModels.Select<MedicalSubCategoryModel, long>((Func<MedicalSubCategoryModel, long>)(l => l.Id)).ToArray<long>();
                    List<MedicalDetailsModel> list = detailsForCountry.Where<MedicalDetailsModel>((Func<MedicalDetailsModel, bool>)(t => ((IEnumerable<long>)lineID).Contains<long>(t.MedicalSubCategory.Id))).ToList<MedicalDetailsModel>();
                    if (list != null && list.Count > 0)
                        employeeRelativesApi.medicalDetailsAPIModels = this._medicalDetailsService.ConvertMedicalDetailsModelToMedicalDetailsAPIModelWithId(list, languageCode);
                    return employeeRelativesApi;
                }
                    return (EmployeeRelativesApi)null;
            }  
            else
            {
               return null;                 
            }
        }

        public async Task<EmployeeRelativesApiModel> GetEmployeeRelativesApiModel(
          long userNumber,
          int languageCode)
        {
            List<Relative> result = this.GetEmployeeRelatives(userNumber).Result;
            if (result == null || result.Count <= 0)
                return (EmployeeRelativesApiModel)null;
            EmployeeRelativesApiModel relativesApiModel = new EmployeeRelativesApiModel();
            List<RelativeApiModel> relativeApiModelList = new List<RelativeApiModel>();
            if (languageCode == 2)
            {
                foreach (Relative relative in result)
                {
                    if (relative.Relation == "Self")
                    {
                        relativesApiModel.RelativeId = relative.Id;
                        relativesApiModel.CemexId = (int)relative.EmployeeNumber;
                        relativesApiModel.EmployeeName = relative.ArabicRelatives;
                        relativesApiModel.BirthDate = relative.BDate;
                        relativesApiModel.MedicalCoverage = relative.CoverPercentage;
                    }
                    else
                        relativeApiModelList.Add(new RelativeApiModel()
                        {
                            RelativeId = relative.Id,
                            RelativeName = relative.ArabicRelatives,
                            Relation = relative.ArabicRelation,
                            BirthDate = relative.BDate,
                            Order = relative.Order,
                            MedicalCoverage = relative.CoverPercentage
                        });
                }
            }
            else
            {
                foreach (Relative relative in result)
                {
                    if (relative.Relation == "Self")
                    {
                        relativesApiModel.RelativeId = relative.Id;
                        relativesApiModel.CemexId = (int)relative.EmployeeNumber;
                        relativesApiModel.EmployeeName = relative.Relatives;
                        relativesApiModel.BirthDate = relative.BDate;
                        relativesApiModel.MedicalCoverage = relative.CoverPercentage;
                    }
                    else
                        relativeApiModelList.Add(new RelativeApiModel()
                        {
                            RelativeId = relative.Id,
                            RelativeName = relative.Relatives,
                            Relation = relative.Relation,
                            BirthDate = relative.BDate,
                            Order = relative.Order,
                            MedicalCoverage = relative.CoverPercentage
                        });
                }
            }
            relativesApiModel.Relatives = relativeApiModelList;
            return relativesApiModel;
        }

        public async Task<List<Relative>> GetPendingRelative()
        {
            try
            {
                List<Relative> listAsync = await this._repository.Find((Expression<Func<Relative, bool>>)(i => i.IsVisible == false && i.IsDelted == false)).ToListAsync<Relative>();
                if (listAsync != null)
                    return listAsync;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.ToString());
            }
            return null;
        }

        public async Task<int> GetPendingRelativeCountAsync()
        {
            try
            {
                List<Relative> listAsync = await this._repository.Find((Expression<Func<Relative, bool>>)(i => i.IsVisible == false && i.IsDelted == false)).ToListAsync<Relative>();
                if (listAsync != null)
                    return listAsync.Count;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.ToString());
            }
            return 0;
        }
    }
}
