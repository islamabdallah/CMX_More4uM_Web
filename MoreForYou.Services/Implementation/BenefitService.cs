using AutoMapper;
using Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MoreForYou.Services.Implementation
{
    public class BenefitService : IBenefitService
    {
        private readonly IRepository<Benefit, long> _repository;
        private readonly ILogger<BenefitService> _logger;
        private readonly IMapper _mapper;
        private readonly IBenefitRequestService _benefitRequestService;
        private readonly IGroupEmployeeService _groupEmployeeService;
        private readonly IEmployeeService _employeeService;
        private readonly IRequestWorkflowService _requestWorkflowService;
        private readonly IBenefitWorkflowService _benefitWorkflowService;

        public BenefitService(IRepository<Benefit, long> benefitRepository,
          ILogger<BenefitService> logger,
          IMapper mapper,
          IBenefitRequestService benefitRequestService,
          IGroupEmployeeService groupEmployeeService,
          IEmployeeService employeeService,
          IRequestWorkflowService requestWorkflowService,
          IBenefitWorkflowService benefitWorkflowService
          )
        {
            _repository = benefitRepository;
            _logger = logger;
            _mapper = mapper;
            _benefitRequestService = benefitRequestService;
            _groupEmployeeService = groupEmployeeService;
            _employeeService = employeeService;
            _requestWorkflowService = requestWorkflowService;
            _benefitWorkflowService = benefitWorkflowService;
        }

        public BenefitModel CreateBenefit(BenefitModel model)
        {
            var benefit = _mapper.Map<Benefit>(model);

            try
            {
                var addedBenefit = _repository.Add(benefit);
                if (addedBenefit != null)
                {
                    BenefitModel addedBenefitModel = new BenefitModel();
                    addedBenefitModel = _mapper.Map<BenefitModel>(addedBenefit);
                    return addedBenefitModel;
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

        public bool DeleteBenefit(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BenefitModel>> GetAllBenefits()
        {
            try
            {
                var benefits = await _repository.Find(i => i.IsVisible == true, false, b => b.BenefitType).ToListAsync();
                var models = new List<BenefitModel>();
                models = _mapper.Map<List<BenefitModel>>(benefits);
                return models;
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public async Task<List<BenefitModel>> GetBenefitsByAddress(string country)
        {
            try
            {
                var benefits = await _repository.Find(i => i.IsVisible == true && i.Country == country, false, b => b.BenefitType).ToListAsync();
                var models = new List<BenefitModel>();
                models = _mapper.Map<List<BenefitModel>>(benefits);
                return models;
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }


        public async Task<BenefitModel> GetBenefit(long id)
        {
            try
            {
                var benefit = await _repository.Find(b => b.Id == id && b.IsVisible == true, false, b => b.BenefitType).FirstOrDefaultAsync();
                BenefitModel benefitModel = _mapper.Map<BenefitModel>(benefit);
                return benefitModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public Task<List<BenefitModel>> GetBenefitByName(BenefitModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateBenefit(BenefitModel model)
        {
            var benefit = _mapper.Map<Benefit>(model);

            try
            {
                bool result = _repository.Update(benefit);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return false;
        }

        public List<BenefitModel> BenefitsUserCanRedeem(List<BenefitModel> benefitModels, EmployeeModel employeeModel)
        {
            int times = 0;
            int HoldedRequests = 0;
            int employeeWorkDuration = 0;
            int employeeAge = 0;
            bool birthDateFlag = true;
            foreach (BenefitModel benefitModel in benefitModels)
            {
                birthDateFlag = true;
                times = 0;
                HoldedRequests = 0;
                employeeWorkDuration = 0;
                employeeAge = 0;
                List<BenefitRequestModel> employeeBenefitRequestModels = new List<BenefitRequestModel>();
                List<GroupEmployeeModel> employeeBenefitGroup = new List<GroupEmployeeModel>();

                employeeWorkDuration = DateTime.Now.Year - employeeModel.JoiningDate.Year;
                employeeAge = DateTime.Now.Year - employeeModel.BirthDate.Year;
                if (benefitModel.BenefitTypeId == (int)CommanData.BenefitTypes.Individual)
                {
                    employeeBenefitRequestModels = _benefitRequestService.GetBenefitRequestByEmployeeId(employeeModel.EmployeeNumber, benefitModel.Id).Result.Where(r => r.RequestStatusId == (int)CommanData.BenefitStatus.Approved || r.RequestStatusId == (int)CommanData.BenefitStatus.Pending || r.RequestStatusId == (int)CommanData.BenefitStatus.InProgress).ToList();
                    if (employeeBenefitRequestModels.Count > 0)
                    {
                        times = employeeBenefitRequestModels.Where(r => r.RequestStatusId == (int)CommanData.BenefitStatus.Approved).Count();

                        HoldedRequests = employeeBenefitRequestModels.Where(r => r.RequestStatusId == (int)CommanData.BenefitStatus.Pending || r.RequestStatusId == (int)CommanData.BenefitStatus.InProgress).Count();
                    }
                }
                else if (benefitModel.BenefitTypeId == (int)CommanData.BenefitTypes.Group)
                {
                    var employeeGroups = _groupEmployeeService.GetGroupsByEmployeeId(employeeModel.EmployeeNumber).Result;
                    employeeBenefitGroup = employeeGroups.Where(eg => eg.Group.BenefitId == benefitModel.Id) != null ? employeeGroups.Where(eg => eg.Group.BenefitId == benefitModel.Id).ToList() : null;
                    if (employeeGroups != null)
                    {
                        if (employeeBenefitGroup != null)
                        {
                            times = employeeBenefitGroup.Where(eg => eg.Group.RequestStatusId == (int)CommanData.BenefitStatus.Approved).ToList().Count;
                            HoldedRequests = employeeBenefitGroup.Where(r => r.Group.RequestStatusId == (int)CommanData.BenefitStatus.Pending || r.Group.RequestStatusId == (int)CommanData.BenefitStatus.InProgress).Count();
                        }

                    }
                }
                benefitModel.UserCanRedeem = false;

                if (benefitModel.DateToMatch == "Birth Date")
                {
                    if ((employeeModel.BirthDate.Month > DateTime.Today.Month))
                    {
                        birthDateFlag = true;
                    }
                    else if (employeeModel.BirthDate.Month == DateTime.Today.Month)
                    {
                        if (employeeModel.BirthDate.Day > DateTime.Today.Day)
                        {
                            birthDateFlag = true;
                        }
                        else
                        {
                            birthDateFlag = false;
                        }
                    }
                    else
                    {
                        birthDateFlag = false;
                    }
                }
                if (birthDateFlag == true)
                {
                    if (benefitModel.Year == DateTime.Now.Year)
                    {
                        if (benefitModel.Times > times && HoldedRequests == 0)
                        {
                            if (benefitModel.Collar == -1 || (benefitModel.Collar != -1 && employeeModel.Collar == benefitModel.Collar))
                            {
                                if (benefitModel.gender == -1 || (benefitModel.gender != -1 && employeeModel.Gender == benefitModel.gender))
                                {
                                    if (benefitModel.MaritalStatus == -1 || (benefitModel.MaritalStatus != -1 && employeeModel.MaritalStatus == benefitModel.MaritalStatus))
                                    {
                                        if (benefitModel.WorkDuration == 0 || (benefitModel.WorkDuration != 0 && employeeWorkDuration >= benefitModel.WorkDuration))
                                        {
                                            if (benefitModel.Age != 0)
                                            {
                                                if ((benefitModel.AgeSign == ">" && employeeAge > benefitModel.Age) ||
                                                    (benefitModel.AgeSign == "<" && employeeAge < benefitModel.Age) ||
                                                    (benefitModel.AgeSign == "=" && employeeAge == benefitModel.Age))
                                                {
                                                    benefitModel.UserCanRedeem = true;
                                                }
                                            }
                                            else
                                            {
                                                benefitModel.UserCanRedeem = true;
                                            }

                                        }
                                    }
                                }

                            }
                        }


                    }
                }
            }
            return benefitModels;
        }

        public BenefitConditionsAndAvailable CreateBenefitConditions(BenefitModel benefitModel, EmployeeModel employeeModel)
        {
            Dictionary<string, string> BenefitConditions = new Dictionary<string, string>();
            Dictionary<string, bool> ConditionsApplicable = new Dictionary<string, bool>();
            BenefitConditions.Add("Type", benefitModel.BenefitType.Name);
            ConditionsApplicable.Add("Type", true);

            if (benefitModel.Age != 0)
            {
                //BenefitConditions.Add("Age " + benefitModel.AgeSign + benefitModel.Age);
                BenefitConditions.Add("Age", "Age " + benefitModel.AgeSign + benefitModel.Age);
                int employeeAge = DateTime.Now.Year - employeeModel.BirthDate.Year;
                if ((benefitModel.AgeSign == ">" && employeeAge > benefitModel.Age) ||
                                                (benefitModel.AgeSign == "<" && employeeAge < benefitModel.Age) ||
                                                (benefitModel.AgeSign == "=" && employeeAge == benefitModel.Age))
                {
                    ConditionsApplicable.Add("Age", true);

                }
                else
                {
                    ConditionsApplicable.Add("Age", false);

                }
            }
            //else
            //{
            //    BenefitConditions.Add("Age : Any");
            //}

            if (benefitModel.WorkDuration != 0)
            {
                BenefitConditions.Add("WorkDuration", "Work Duration >= " + benefitModel.WorkDuration);
                int employeeWorkDuartion = DateTime.Now.Year - employeeModel.JoiningDate.Year;
                if (employeeWorkDuartion >= benefitModel.WorkDuration)
                {
                    ConditionsApplicable.Add("WorkDuration", true);

                }
                else
                {
                    ConditionsApplicable.Add("WorkDuration", false);
                }

            }
            if (benefitModel.gender != (int)CommanData.Gender.Any)
            {
                BenefitConditions.Add("Gender", "" + (CommanData.Gender)benefitModel.gender);
                if (employeeModel.Gender == benefitModel.gender)
                {
                    ConditionsApplicable.Add("Gender", true);

                }
                else
                {
                    ConditionsApplicable.Add("Gender", false);
                }

            }
            if (benefitModel.MaritalStatus != (int)CommanData.MaritialStatus.Any)
            {
                BenefitConditions.Add("MaritalStatus", "" + (CommanData.MaritialStatus)benefitModel.MaritalStatus);
                if (employeeModel.MaritalStatus == benefitModel.MaritalStatus)
                {
                    ConditionsApplicable.Add("MaritalStatus", true);

                }
                else
                {
                    ConditionsApplicable.Add("MaritalStatus", false);
                }
            }
            if (benefitModel.Collar != (int)CommanData.CollarTypes.Any)
            {
                BenefitConditions.Add("PayrollArea", "" + (CommanData.CollarTypes)benefitModel.Collar);
                if (employeeModel.Collar == benefitModel.Collar)
                {
                    ConditionsApplicable.Add("PayrollArea", true);

                }
                else
                {
                    ConditionsApplicable.Add("PayrollArea", false);
                }
            }
            if (benefitModel.DateToMatch != "Any" && (benefitModel.DateToMatch == "Birth Date" || benefitModel.DateToMatch == "Join Date"))
            {
                BenefitConditions.Add("DateToMatch", "Benefit Redeemation date must match with your :" + benefitModel.DateToMatch);
                ConditionsApplicable.Add("DateToMatch", true);

            }
            else if (benefitModel.DateToMatch != "Any" && (benefitModel.DateToMatch == "Certain Date"))
            {
                BenefitConditions.Add("DateToMatch", "Benefit Redeemation date must be at :" + benefitModel.CertainDate);
                ConditionsApplicable.Add("DateToMatch", true);
            }

            //else
            //{
            //    BenefitConditions.Add("Benefit Redeemation can be at any date you desired");
            //}
            if (benefitModel.RequiredDocuments != null)
            {
                BenefitConditions.Add("RequiredDocuments", "Required Documents are " + benefitModel.RequiredDocuments);
                ConditionsApplicable.Add("RequiredDocuments", true);

            }

            if (benefitModel.BenefitTypeId == (int)CommanData.BenefitTypes.Group)
            {
                BenefitConditions.Add("MinParticipant", "" + benefitModel.MinParticipant);
                BenefitConditions.Add("MaxParticipant", "" + benefitModel.MaxParticipant);
                ConditionsApplicable.Add("MinParticipant", true);
                ConditionsApplicable.Add("MaxParticipant", true);

            }
            BenefitConditionsAndAvailable benefitConditionsAndAvailable = new BenefitConditionsAndAvailable();
            benefitConditionsAndAvailable.BenefitApplicable = ConditionsApplicable;
            benefitConditionsAndAvailable.BenefitConditions = BenefitConditions;
            return benefitConditionsAndAvailable;

        }


        public BenefitConditionsAndAvailable CreateArabicBenefitConditions(BenefitModel benefitModel, EmployeeModel employeeModel)
        {
            Dictionary<string, string> BenefitConditions = new Dictionary<string, string>();
            Dictionary<string, bool> ConditionsApplicable = new Dictionary<string, bool>();
            BenefitConditions.Add("Type", benefitModel.BenefitType.ArabicName);
            ConditionsApplicable.Add("Type", true);

            if (benefitModel.Age != 0)
            {
                //BenefitConditions.Add("Age " + benefitModel.AgeSign + benefitModel.Age);
                BenefitConditions.Add("Age", benefitModel.Age + " " + benefitModel.AgeSign + " العمر");
                int employeeAge = DateTime.Now.Year - employeeModel.BirthDate.Year;
                if ((benefitModel.AgeSign == ">" && employeeAge > benefitModel.Age) ||
                                                (benefitModel.AgeSign == "<" && employeeAge < benefitModel.Age) ||
                                                (benefitModel.AgeSign == "=" && employeeAge == benefitModel.Age))
                {
                    ConditionsApplicable.Add("Age", true);

                }
                else
                {
                    ConditionsApplicable.Add("Age", false);

                }
            }
            //else
            //{
            //    BenefitConditions.Add("Age : Any");
            //}

            if (benefitModel.WorkDuration != 0)
            {
                BenefitConditions.Add("WorkDuration", benefitModel.WorkDuration + " عدد سنوات الخبرة بالشركة أكبر من");
                int employeeWorkDuartion = DateTime.Now.Year - employeeModel.JoiningDate.Year;
                if (employeeWorkDuartion >= benefitModel.WorkDuration)
                {
                    ConditionsApplicable.Add("WorkDuration", true);

                }
                else
                {
                    ConditionsApplicable.Add("WorkDuration", false);
                }

            }
            if (benefitModel.gender != (int)CommanData.Gender.Any)
            {
                BenefitConditions.Add("Gender", "" + (CommanData.ArabicGender)benefitModel.gender);
                if (employeeModel.Gender == benefitModel.gender)
                {
                    ConditionsApplicable.Add("Gender", true);

                }
                else
                {
                    ConditionsApplicable.Add("Gender", false);
                }

            }
            if (benefitModel.MaritalStatus != (int)CommanData.MaritialStatus.Any)
            {
                BenefitConditions.Add("MaritalStatus", "" + (CommanData.ArabicMaritialStatus)benefitModel.MaritalStatus);
                if (employeeModel.MaritalStatus == benefitModel.MaritalStatus)
                {
                    ConditionsApplicable.Add("MaritalStatus", true);

                }
                else
                {
                    ConditionsApplicable.Add("MaritalStatus", false);
                }
            }
            if (benefitModel.Collar != (int)CommanData.CollarTypes.Any)
            {
                BenefitConditions.Add("PayrollArea", "" + (CommanData.ArabicCollarTypes)benefitModel.Collar);
                if (employeeModel.Collar == benefitModel.Collar)
                {
                    ConditionsApplicable.Add("PayrollArea", true);

                }
                else
                {
                    ConditionsApplicable.Add("PayrollArea", false);
                }
            }
            if (benefitModel.DateToMatch != "Any" && (benefitModel.DateToMatch == "Birth Date" || benefitModel.DateToMatch == "Join Date"))
            {
                //BenefitConditions.Add("DateToMatch", "Benefit Redeemation date must match with your :" + benefitModel.DateToMatch);
                BenefitConditions.Add("DateToMatch", "تاريخ استرداد هذه الميزة لابد أن يتطابق مع " + benefitModel.ArabicDateToMatch);

                ConditionsApplicable.Add("DateToMatch", true);

            }
            else if (benefitModel.DateToMatch != "Any" && (benefitModel.DateToMatch == "Certain Date"))
            {
                BenefitConditions.Add("DateToMatch", "Benefit Redeemation date must be at :" + benefitModel.CertainDate);
                ConditionsApplicable.Add("DateToMatch", true);
            }

            //else
            //{
            //    BenefitConditions.Add("Benefit Redeemation can be at any date you desired");
            //}
            if (benefitModel.RequiredDocuments != null)
            {
                BenefitConditions.Add("RequiredDocuments", "المستندات المطلوبة هي  " + benefitModel.ArabicRequiredDocuments);
                ConditionsApplicable.Add("RequiredDocuments", true);

            }

            if (benefitModel.BenefitTypeId == (int)CommanData.BenefitTypes.Group)
            {
                BenefitConditions.Add("MinParticipant", "" + benefitModel.MinParticipant);
                BenefitConditions.Add("MaxParticipant", "" + benefitModel.MaxParticipant);
                ConditionsApplicable.Add("MinParticipant", true);
                ConditionsApplicable.Add("MaxParticipant", true);

            }
            BenefitConditionsAndAvailable benefitConditionsAndAvailable = new BenefitConditionsAndAvailable();
            benefitConditionsAndAvailable.BenefitApplicable = ConditionsApplicable;
            benefitConditionsAndAvailable.BenefitConditions = BenefitConditions;
            return benefitConditionsAndAvailable;

        }

        public BenefitAPIModel CreateBenefitAPIModel(BenefitModel model, int languageId)
        {
            try
            {
                BenefitAPIModel benefitAPIModel = new BenefitAPIModel();
                switch (languageId)
                {
                    case (int)CommanData.Languages.English:
                        benefitAPIModel.Name = model.Name;
                        benefitAPIModel.Description = model.Description;
                        benefitAPIModel.BenefitType = model.BenefitType.Name;
                        benefitAPIModel.Title = model.Title;
                        break;
                    case (int)CommanData.Languages.Arabic:
                        benefitAPIModel.Name = model.ArabicName;
                        benefitAPIModel.Description = model.ArabicDescription;
                        benefitAPIModel.BenefitType = model.BenefitType.ArabicName;
                        benefitAPIModel.Title = model.ArabicTitle;
                        break;
                }
                benefitAPIModel.Id = model.Id;
                //benefitAPIModel.Name = model.Name;
                //benefitAPIModel.Description = model.Description;
                benefitAPIModel.BenefitCard = model.BenefitCard;
                benefitAPIModel.BenefitCardAPI = CommanData.Url + CommanData.CardsFolder + model.BenefitCard;
                benefitAPIModel.Times = model.Times;
                benefitAPIModel.numberOfDays = model.numberOfDays;
                benefitAPIModel.DateToMatch = model.DateToMatch;
                benefitAPIModel.MustMatch = model.MustMatch;
                if(model.CertainDate != null)
                {
                    benefitAPIModel.CertainDate = model.CertainDate.Value;
                }
                else
                {
                    benefitAPIModel.CertainDate =null;
                }
                //benefitAPIModel.BenefitType = model.BenefitType.Name;
                benefitAPIModel.BenefitConditions = model.BenefitConditions;
                benefitAPIModel.BenefitApplicable = model.BenefitApplicable;
                benefitAPIModel.BenefitWorkflows = model.BenefitWorkflows;
                benefitAPIModel.UserCanRedeem = model.UserCanRedeem;
                benefitAPIModel.MaxParticipant = model.MaxParticipant;
                benefitAPIModel.MinParticipant = model.MinParticipant;
                benefitAPIModel.IsAgift = model.IsAgift;
                benefitAPIModel.LastStatus = model.LastStatus;
                benefitAPIModel.Title = model.Title;
                //benefitAPIModel.benefitStatses = model.benefitStatses;
                benefitAPIModel.totalRequestsCount = model.totalRequestsCount;
                benefitAPIModel.TimesUserReceiveThisBenefit = model.NumberOfApprovedRequests;
                if (model.RequiredDocumentsArray != null)
                {
                    benefitAPIModel.RequiredDocumentsArray = model.RequiredDocumentsArray;
                }
                string[] benefitDescArray = benefitAPIModel.Description.Split(";");
                benefitAPIModel.BenefitDecriptionList = new List<string>();
                for (int descIndex = 0; descIndex < benefitDescArray.Length; descIndex++)
                {

                    benefitAPIModel.BenefitDecriptionList.Add(benefitDescArray[descIndex]);
                }
                return benefitAPIModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }


        public async Task<List<BenefitAPIModel>> GetMyBenefits(long employeeNumber, int languageId)
        {
            try
            {
                int receivedTimesCount = 0;
                List<BenefitModel> benefitModels = new List<BenefitModel>();
                var benefitRequestModels1 = await _benefitRequestService.GetBenefitRequestByEmployeeId(employeeNumber);
                List<GroupEmployeeModel> groupEmployeeModels = await _groupEmployeeService.GetGroupsByEmployeeId(employeeNumber);
                if ((groupEmployeeModels != null && groupEmployeeModels.Count > 0) || benefitRequestModels1.Count != 0)
                {

                    if (benefitRequestModels1.Count != 0)
                    {
                        benefitRequestModels1 = benefitRequestModels1.OrderByDescending(r => r.CreatedDate).ToList();
                        List<BenefitRequestModel> benefitRequestModels = benefitRequestModels1.Where(r => r.GroupId == null).ToList();

                        var benefitRequestModelsGroup = benefitRequestModels.GroupBy(BR => BR.Benefit.Id).ToList();
                        foreach (var group in benefitRequestModelsGroup)
                        {
                            BenefitModel benefitModel = await GetBenefit(group.Key);

                            List<BenefitRequestModel> benefitRequestModels2 = group.AsEnumerable().ToList();
                            //List<BenefitStats> benefitStats = GetIndividualBenefitStats(benefitRequestModels2).ToList();
                            //benefitModel.benefitStatses = benefitStats;
                            var receivedTimes = benefitRequestModels2.Where(r => r.RequestStatusId == (int)CommanData.BenefitStatus.Approved);
                            if (receivedTimes != null)
                            {
                                receivedTimesCount = receivedTimes.ToList().Count;
                            }
                            benefitModel.totalRequestsCount = benefitRequestModels2.Count;
                            benefitModel.LastStatus = group.First().RequestStatus.Name;
                            benefitModel.LastRequetedDate = group.First().CreatedDate;
                            benefitModel.NumberOfApprovedRequests = receivedTimesCount;
                            benefitModels.Add(benefitModel);
                        }
                    }
                    if (groupEmployeeModels != null)
                    {
                        var groupEmployeeModelGroups = groupEmployeeModels.OrderByDescending(g => g.Group.CreatedDate).GroupBy(g => g.Group.BenefitId).ToList();//.Select(g => g.Select(g => g.Group.Benefit));
                        foreach (var groups in groupEmployeeModelGroups)
                        {
                            List<BenefitRequestModel> GroupbenefitRequestes = new List<BenefitRequestModel>();
                            BenefitModel benefitModel = await GetBenefit(groups.Key);
                            var receivedTimes = groups.Where(g => g.Group.RequestStatusId == (int)CommanData.BenefitStatus.Approved);
                            if (receivedTimes != null)
                            {
                                receivedTimesCount = receivedTimes.ToList().Count;
                            }

                            foreach (var group in groups)
                            {
                                BenefitRequestModel GroupbenefitRequest = _benefitRequestService.GetBenefitRequestByGroupId(group.Group.Id);
                                GroupbenefitRequestes.Add(GroupbenefitRequest);

                            }

                            benefitModel.totalRequestsCount = GroupbenefitRequestes.Count;
                            benefitModel.LastStatus = GroupbenefitRequestes.First().RequestStatus.Name;
                            benefitModel.LastRequetedDate = GroupbenefitRequestes.First().CreatedDate;
                            benefitModel.NumberOfApprovedRequests = receivedTimesCount;

                            benefitModels.Add(benefitModel);
                        }
                    }
                    List<BenefitAPIModel> benefitAPIModels = new List<BenefitAPIModel>();
                    if (benefitModels.Count != 0)
                    {
                        benefitModels = benefitModels.OrderByDescending(b => b.LastRequetedDate).ToList();

                        foreach (BenefitModel model in benefitModels)
                        {
                            BenefitAPIModel benefitAPIModel = CreateBenefitAPIModel(model, languageId);
                            benefitAPIModels.Add(benefitAPIModel);
                        }
                        return benefitAPIModels;
                    }
                    else
                    {
                        return new List<BenefitAPIModel>();
                    }
                }
                else
                {
                    return new List<BenefitAPIModel>();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }


        public async Task<List<Participant>> GetEmployeesCanRedeemThisGroupBenefit(long employeeNumber, long benefitId)
        {
            EmployeeModel employeeModel = _employeeService.GetEmployee(employeeNumber).Result;
            List<Participant> participants = new List<Participant>();

            if (employeeModel != null)
            {
                BenefitModel benefitModel = await GetBenefit(benefitId);
                int EmployeeGroupsCount = 0;
                int EmployeePendingGroupsCount = 0;
                int employeeWorkDuration = 0;
                int employeeAge = 0;
                bool flag = false;
                if (benefitModel.BenefitTypeId == (int)CommanData.BenefitTypes.Group)
                {
                    List<EmployeeModel> employeeModels = _employeeService.GetAllDirectEmployeesSameCountry(employeeModel.EmployeeNumber).Result.ToList();
                    foreach (EmployeeModel employee in employeeModels)
                    {
                        employeeWorkDuration = DateTime.Now.Year - employeeModel.JoiningDate.Year;
                        employeeAge = DateTime.Now.Year - employeeModel.BirthDate.Year;

                        if (benefitModel.Year == DateTime.Now.Year)
                        {
                            if (benefitModel.Collar == -1 || (benefitModel.Collar != -1 && employeeModel.Collar == benefitModel.Collar))
                            {
                                if (benefitModel.gender == -1 || (benefitModel.gender != -1 && employeeModel.Gender == benefitModel.gender))
                                {
                                    if (benefitModel.MaritalStatus == -1 || (benefitModel.MaritalStatus != -1 && employeeModel.MaritalStatus == benefitModel.MaritalStatus))
                                    {
                                        if (benefitModel.WorkDuration == 0 || (benefitModel.WorkDuration != 0 && employeeWorkDuration >= benefitModel.WorkDuration))
                                        {
                                            if (benefitModel.Age != 0)
                                            {
                                                if ((benefitModel.AgeSign == ">" && employeeAge > benefitModel.Age) ||
                                                    (benefitModel.AgeSign == "<" && employeeAge < benefitModel.Age) ||
                                                    (benefitModel.AgeSign == "=" && employeeAge == benefitModel.Age))
                                                {
                                                    flag = true;
                                                }
                                                else
                                                {
                                                    flag = false;
                                                }
                                            }
                                            else
                                            {
                                                flag = true;
                                            }
                                        }
                                    }
                                }

                            }
                        }

                        if (flag == true)
                        {
                            List<GroupEmployeeModel> groupEmployeeModels = _groupEmployeeService.GetGroupsByEmployeeId(employee.EmployeeNumber).Result;

                            var EmployeesGroup = groupEmployeeModels.Where(
                                                         ge => ge.Group.RequestStatusId == (int)CommanData.BenefitStatus.Approved ||
                                                          ge.Group.RequestStatusId == (int)CommanData.BenefitStatus.InProgress ||
                                                          ge.Group.RequestStatusId == (int)CommanData.BenefitStatus.Pending);
                            List<GroupEmployeeModel> EmployeeWithPendingGroup = groupEmployeeModels.Where(g => g.EmployeeId == employee.EmployeeNumber && g.Group.RequestStatusId == (int)CommanData.BenefitStatus.Pending).ToList();
                            if (EmployeesGroup != null)
                            {
                                EmployeeGroupsCount = EmployeesGroup.ToList().Count;
                                EmployeePendingGroupsCount = EmployeeWithPendingGroup.Count();
                                if (EmployeeGroupsCount < benefitModel.Times && EmployeePendingGroupsCount == 0)
                                {
                                    Participant participant = new Participant();
                                    participant.UserNumber = employee.EmployeeNumber;
                                    participant.FullName = employee.FullName;
                                    //participant.ProfilePicture = employee.ProfilePicture;
                                    participants.Add(participant);
                                }
                            }
                            else
                            {
                                Participant participant = new Participant();
                                participant.UserNumber = employee.EmployeeNumber;
                                participant.FullName = employee.FullName;
                                participants.Add(participant);
                            }

                        }
                    }

                    Participant my = participants.Where(p => p.UserNumber == employeeNumber).FirstOrDefault();
                    if (participants.Contains(my))
                    {
                        participants.Remove(my);
                    }

                    return participants;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<HomeModel> ShowAllBenefits(EmployeeModel employeeModel, int languageId)
        {
            try
            {
                HomeModel homeModel = new HomeModel();
                Dictionary<string, string> BenefitConditions = new Dictionary<string, string>();
                Dictionary<string, bool> BenefitApplicale = new Dictionary<string, bool>();
                List<string> BenefitWorkflows = null;

                List<BenefitModel> AllBenefitModels = new List<BenefitModel>();
                List<BenefitAPIModel> AllBenefitAPIModels = new List<BenefitAPIModel>();
                List<BenefitAPIModel> AvailableBenefitAPIModels = new List<BenefitAPIModel>();
                BenefitAPIModel benefitAPIModel = new BenefitAPIModel();
                LoginUser User = new LoginUser();
                BenefitConditionsAndAvailable benefitConditionsAndAvailable = null;
                List<BenefitModel> benefitModels = GetBenefitsByAddress(employeeModel.Country).Result;
                AllBenefitModels = BenefitsUserCanRedeem(benefitModels, employeeModel);

                var requests = await _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(employeeModel.EmployeeNumber);
                if (requests.Count != 0)
                {
                    employeeModel.hasRequests = true;
                    employeeModel.PendingRequestsCount = requests.Where(r => r.RequestStatusId == (int)CommanData.BenefitStatus.Pending).Count();

                }
                else
                {
                    employeeModel.hasRequests = false;
                    employeeModel.PendingRequestsCount = 0;

                }
                User = _employeeService.CreateLoginUser(employeeModel);
                if (User != null)
                {
                    homeModel.user = User;
                }
                else
                {
                    homeModel.user = new LoginUser();
                }
                foreach (BenefitModel benefitModel in AllBenefitModels)
                {
                    switch (languageId)
                    {
                        case (int)CommanData.Languages.English:
                            benefitConditionsAndAvailable = CreateBenefitConditions(benefitModel, employeeModel);
                            if (benefitModel.RequiredDocuments != null)
                            {
                                benefitModel.RequiredDocumentsArray = benefitModel.RequiredDocuments.Split(";");
                            }
                            break;
                        case (int)CommanData.Languages.Arabic:
                            benefitConditionsAndAvailable = CreateArabicBenefitConditions(benefitModel, employeeModel);
                            if (benefitModel.ArabicRequiredDocuments != null)
                            {
                                benefitModel.RequiredDocumentsArray = benefitModel.ArabicRequiredDocuments.Split(";");
                            }
                            break;
                    }
                    //BenefitConditionsAndAvailable benefitConditionsAndAvailable = CreateBenefitConditions(benefitModel, employeeModel);
                    //BenefitConditions = benefitConditionsAndAvailable.BenefitConditions;
                    //BenefitApplicale = benefitConditionsAndAvailable.BenefitApplicable;
                    benefitModel.BenefitConditions = benefitConditionsAndAvailable.BenefitConditions;
                    benefitModel.BenefitApplicable = benefitConditionsAndAvailable.BenefitApplicable;

                    //if (BenefitConditions != null)
                    //{
                    //    benefitModel.BenefitConditions = BenefitConditions;
                    //}
                    //else
                    //{
                    //    benefitModel.BenefitConditions = new Dictionary<string, string>();
                    //}
                    //if (BenefitApplicale != null)
                    //{
                    //    benefitModel.BenefitApplicable = BenefitApplicale;
                    //}
                    //else
                    //{
                    //    benefitModel.BenefitApplicable = new Dictionary<string, bool>();
                    //}
                    if (benefitModel.HasWorkflow == true)
                    {
                        BenefitWorkflows = _benefitWorkflowService.CreateBenefitWorkFlow(benefitModel, languageId);
                        if (BenefitWorkflows != null)
                        {
                            benefitModel.BenefitWorkflows = BenefitWorkflows;
                        }
                        else
                        {
                            benefitModel.BenefitWorkflows = new List<string>();
                        }
                    }
                    else
                    {
                        benefitModel.BenefitWorkflows = new List<string>();
                    }
                    //BenefitWorkflows = _benefitWorkflowService.CreateBenefitWorkFlow(benefitModel);

                }
                if (AllBenefitModels != null)
                {
                    foreach (BenefitModel benefitModel in AllBenefitModels)
                    {
                        benefitAPIModel = CreateBenefitAPIModel(benefitModel, languageId);
                        if (benefitAPIModel != null)
                        {
                            if (benefitAPIModel.BenefitType == "Individual" || benefitAPIModel.BenefitType == "فردية")
                            {
                                benefitAPIModel.HasHoldingRequests = _benefitRequestService.ISEmployeeHasHoldingRequestsForthisBenefit(employeeModel.EmployeeNumber, benefitModel.Id);
                            }
                            else
                            {
                                benefitAPIModel.HasHoldingRequests = _groupEmployeeService.ISEmployeeHasHoldingRequestsForthisGroupBenefit(employeeModel.EmployeeNumber, benefitModel.Id);
                            }


                            AllBenefitAPIModels.Add(benefitAPIModel);
                        }
                    }
                    var AvailableBenefitModels = AllBenefitAPIModels.Where(b => b.UserCanRedeem == true);
                    foreach (var benefit in AllBenefitAPIModels)
                    {
                        if (benefit.BenefitType == "Individual" || benefit.BenefitType == "فردية")
                        {
                            benefit.TimesUserReceiveThisBenefit = _benefitRequestService.GetTimesEmployeeReceieveThisBenefit(employeeModel.EmployeeNumber, benefit.Id);

                        }
                        else
                        {
                            benefit.TimesUserReceiveThisBenefit = _groupEmployeeService.GetTimesEmployeeReceieveThisGroupBenefit(employeeModel.EmployeeNumber, benefit.Id);

                        }
                    }
                    if (AvailableBenefitModels != null)
                    {
                        homeModel.AvailableBenefitModels = AvailableBenefitModels.ToList(); ;

                    }
                    else
                    {
                        homeModel.AvailableBenefitModels = AvailableBenefitAPIModels;

                    }
                    homeModel.AllBenefitModels = AllBenefitAPIModels;

                }
                else
                {
                    homeModel.AllBenefitModels = new List<BenefitAPIModel>();

                }
                return homeModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }

        }

        public async Task<BenefitAPIModel> GetBenefitDetails(long benefitId, EmployeeModel employeeModel, int languageId)
        {
            try
            {
                Dictionary<string, string> BenefitConditions = new Dictionary<string, string>();
                Dictionary<string, bool> BenefitApplicale = new Dictionary<string, bool>();
                BenefitModel benefitModel = await GetBenefit(benefitId);
                BenefitConditionsAndAvailable benefitConditionsAndAvailable = CreateBenefitConditions(benefitModel, employeeModel);
                benefitModel.BenefitConditions = benefitConditionsAndAvailable.BenefitConditions;
                benefitModel.BenefitApplicable = benefitConditionsAndAvailable.BenefitApplicable;
                //if (BenefitConditions != null)
                //{
                //    benefitModel.BenefitConditions = BenefitConditions;
                //}
                //else
                //{
                //    benefitModel.BenefitConditions = new Dictionary<string, string>();
                //}
                //if (BenefitApplicale != null)
                //{
                //    benefitModel.BenefitApplicable = BenefitApplicale;
                //}
                //else
                //{
                //    benefitModel.BenefitApplicable = new Dictionary<string, bool>();
                //}

                benefitModel.BenefitWorkflows = _benefitWorkflowService.CreateBenefitWorkFlow(benefitModel, languageId);
                List<BenefitModel> benefitModels = new List<BenefitModel>();
                benefitModels.Add(benefitModel);
                benefitModels = BenefitsUserCanRedeem(benefitModels, employeeModel);
                benefitModel = benefitModels.First();
                BenefitAPIModel benefitAPIModel = CreateBenefitAPIModel(benefitModel, languageId);
                return benefitAPIModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public async Task<List<Participant>> GetEmployeesWhoCanIGiveThisBenefit(long employeeNumber, long benefitId)
        {
            EmployeeModel employeeModel = _employeeService.GetEmployee(employeeNumber).Result;
            List<Participant> participants = new List<Participant>();

            if (employeeModel != null)
            {
                BenefitModel benefitModel = await GetBenefit(benefitId);
                if (benefitModel.IsAgift == true)
                {
                    var employeeModels = await _employeeService.GetAllEmployeeWhoCanIGive();
                    if (employeeModels != null)
                    {
                        List<EmployeeModel> employeeModels1 = employeeModels.ToList();
                        foreach (var employee in employeeModels)
                        {
                            Participant participant = new Participant();
                            participant.UserNumber = employee.EmployeeNumber;
                            participant.FullName = employee.FullName;
                            participant.ProfilePicture = employee.ProfilePicture;
                            participants.Add(participant);
                        }
                        var my = participants.Where(p => p.UserNumber == employeeNumber).First();
                        participants.Remove(my);
                        return participants;
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }

        public List<BenefitStats> GetIndividualBenefitStats(List<BenefitRequestModel> benefitRequests)
        {
            int pendingCount = benefitRequests.Where(r => r.RequestStatusId == (int)CommanData.BenefitStatus.Pending).Count();
            int InProgressCount = benefitRequests.Where(r => r.RequestStatusId == (int)CommanData.BenefitStatus.InProgress).Count();
            int ApprovedCount = benefitRequests.Where(r => r.RequestStatusId == (int)CommanData.BenefitStatus.Approved).Count();
            int RejectedCount = benefitRequests.Where(r => r.RequestStatusId == (int)CommanData.BenefitStatus.Rejected).Count();
            int CancelledCount = benefitRequests.Where(r => r.RequestStatusId == (int)CommanData.BenefitStatus.Cancelled).Count();
            int Closed = ApprovedCount + RejectedCount + CancelledCount;

            List<BenefitStats> benefitStatses = new List<BenefitStats>(8);
            for (int index = 1; index <= 5; index++)
            {
                BenefitStats benefitStats = new BenefitStats();
                benefitStats.Name = Enum.GetName(typeof(CommanData.BenefitStatus), index);
                benefitStats.Count = benefitRequests.Where(r => r.RequestStatusId == index).Count();
                benefitStatses.Add(benefitStats);
            }

            BenefitStats benefitStats1 = new BenefitStats();
            benefitStats1.Name = "Closed";
            benefitStats1.Count = benefitStatses.Where(s => s.Name == "Cancelled" || s.Name == "Approved" || s.Name == "Rejected").Select(s => s.Count).Sum();
            benefitStatses.Add(benefitStats1);
            BenefitStats benefitStats2 = new BenefitStats();
            benefitStats2.Name = "ALL";
            benefitStats2.Count = benefitStatses.Where(s => s.Name == "Cancelled"
                                                         || s.Name == "Approved"
                                                         || s.Name == "Rejected"
                                                         || s.Name == "Pending"
                                                         || s.Name == "InProgress").Select(s => s.Count).Sum();
            benefitStatses.Add(benefitStats2);

            return benefitStatses;
        }


        public List<BenefitStats> GetGroupBenefitStats(List<GroupEmployeeModel> groupEmployeeModels)
        {

            List<BenefitStats> benefitStatses = new List<BenefitStats>(8);
            for (int index = 1; index <= 5; index++)
            {
                BenefitStats benefitStats = new BenefitStats();
                benefitStats.Name = Enum.GetName(typeof(CommanData.BenefitStatus), index);
                benefitStats.Count = groupEmployeeModels.Where(gr => gr.Group.RequestStatusId == index).Count();
                benefitStatses.Insert(index, benefitStats);
            }

            BenefitStats benefitStats1 = new BenefitStats();
            benefitStats1.Name = "Closed";
            benefitStats1.Count = benefitStatses.Where(s => s.Name == "Cancelled" || s.Name == "Approved" || s.Name == "Rejected").Count();
            benefitStatses.Insert(6, benefitStats1);
            BenefitStats benefitStats2 = new BenefitStats();
            benefitStats2.Name = "ALL";
            benefitStats2.Count = benefitStatses.Where(s => s.Name == "Cancelled"
                                                         || s.Name == "Approved"
                                                         || s.Name == "Rejected"
                                                         || s.Name == "Pending"
                                                         || s.Name == "InProgress").Count();
            benefitStatses.Insert(7, benefitStats2);

            return benefitStatses;
        }


        public async Task<WebRequest> BenefitRedeem(long benefitId, string userId)
        {
            EmployeeModel CurrentEmployee = _employeeService.GetEmployeeByUserId(userId).Result;
            BenefitModel benefitModel = await GetBenefit(benefitId);
            WebRequest request = new WebRequest();
            //benefitModel.BenefitConditions = CreateBenefitConditions(benefitModel);
            benefitModel.UserCanRedeem = true;
            //benefitRequestModel.Benefit = benefitModel;
            if (benefitModel.DateToMatch == "Any")
            {
                request.From = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
                request.To = DateTime.Today.AddDays(benefitModel.numberOfDays - 1).ToString("yyyy-MM-dd");
            }
            else if (benefitModel.DateToMatch != "Any")
            {
                RequestDates requestDates = _requestWorkflowService.CalculateRequestExactDates(benefitModel, CurrentEmployee);
                request.From = requestDates.From;
                request.To = requestDates.To;
                //if (benefitModel.DateToMatch == "Birth Date")
                //{
                //    request.From = new DateTime(DateTime.Today.Year, CurrentEmployee.BirthDate.Month, CurrentEmployee.BirthDate.Day).ToString("yyyy-MM-dd");
                //    request.To = new DateTime(DateTime.Today.Year, CurrentEmployee.BirthDate.Month, CurrentEmployee.BirthDate.Day).ToString("yyyy-MM-dd");
                //}
                //else if (benefitModel.DateToMatch == "Join Date")
                //{
                //    request.From = new DateTime(DateTime.Today.Year, CurrentEmployee.JoiningDate.Month, CurrentEmployee.JoiningDate.Day).ToString("yyyy-MM-dd");
                //    request.To = new DateTime(DateTime.Today.Year, CurrentEmployee.JoiningDate.Month, CurrentEmployee.JoiningDate.Day).ToString("yyyy-MM-dd");
                //}
                //else
                //{
                //    request.From = new DateTime(DateTime.Today.Year, benefitModel.CertainDate.Month, benefitModel.CertainDate.Day).ToString("yyyy-MM-dd");
                //    request.To = new DateTime(DateTime.Today.Year, benefitModel.CertainDate.Month, benefitModel.CertainDate.Day).ToString("yyyy-MM-dd");
                //}
            }
            request.numberOfDays = benefitModel.numberOfDays;
            request.DateToMatch = benefitModel.DateToMatch;
            request.Year = benefitModel.Year;
            request.BenefitName = benefitModel.Name;
            request.BenefitCard = benefitModel.BenefitCard;
            request.IsAgift = benefitModel.IsAgift;
            request.benefitId = benefitModel.Id;
            request.MustMatch = benefitModel.MustMatch;
            request.From1 = request.From;
            request.Title = benefitModel.Title;

            if (benefitModel.RequiredDocuments != null)
            {
                request.RequiredDocuments = benefitModel.RequiredDocuments.Split(";");
            }
            request.BenefitType = benefitModel.BenefitType.Name;
            if (benefitModel.BenefitTypeId == (int)CommanData.BenefitTypes.Individual)
            {
                return request;
            }
            else
            {
                request.MaxParticipant = benefitModel.MaxParticipant;
                request.MinParticipant = benefitModel.MinParticipant;
                request.ParticipantsData = await GetEmployeesCanRedeemThisGroupBenefit(CurrentEmployee.EmployeeNumber, benefitId);
                return request;
            }
        }

        public string CalculateWorkDuration(DateTime joinDate)
        {
            string workDuration = "";
            TimeSpan diff = DateTime.Today - joinDate;
            int days = diff.Days;
            int months = days / 30;
            int years = (int)(days / (356.25));
            if (years < 1)
            {
                if (months < 1)
                {
                    workDuration = days + " days";
                }
                else
                {
                    workDuration = months + " Months";
                }

            }
            else
            {
                workDuration = years + " years";

            }
            return workDuration;
        }



    }
}