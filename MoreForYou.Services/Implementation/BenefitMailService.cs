using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Service.Implementation.Email;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Contracts.Email;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Implementation
{
    public class BenefitMailService : IBenefitMailService
    {
        private readonly IRepository<BenefitMail, int> _repository;
        private readonly ILogger<BenefitMailService> _logger;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IEmployeeService _employeeService;
        private readonly IOutlookSenderService _outlookSenderService;
        private readonly IMGraphMailService _mGraphMailService;

        public BenefitMailService(IRepository<BenefitMail, int> repository,
            ILogger<BenefitMailService> logger,
            IMapper mapper,
            IEmailSender emailSender,
            IOutlookSenderService outlookSenderService,
            IEmployeeService employeeService,
            IMGraphMailService mGraphMailService)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _emailSender = emailSender;
            _outlookSenderService = outlookSenderService;
            _employeeService = employeeService;
            _mGraphMailService = mGraphMailService;
        }
        public bool CreateBenefitMail(BenefitMailModel model)
        {
            try
            {
                BenefitMail benefitMail = _mapper.Map<BenefitMail>(model);
                var addedBenefitMail = _repository.Add(benefitMail);
                if(addedBenefitMail != null)
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

        public bool DeleteBenefitMail(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BenefitMailModel>> GetAllBenefitMails()
        {
            throw new NotImplementedException();
        }

        public BenefitMailModel GetBenefitMailsByBenefitId(long benefitId)
        {
            try
            {
               var mails = _repository.Find(bm => bm.IsVisible == true && bm.BenefitId == benefitId).First();
                if(mails != null)
                {
                  BenefitMailModel benefitMailModels = _mapper.Map<BenefitMailModel>(mails);
                    return benefitMailModels;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public BenefitMailModel GetBenefitMail(long id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateBenefitMail(BenefitMailModel model)
        {
            throw new NotImplementedException();
        }

        public bool SendToMailList(BenefitRequestModel benefitRequestModel, List<string> groupMails = null, List<GroupEmployeeModel> groupEmployeeModels = null)
        {
            string subject = "";
            string mailMessage = "";
            string mailBody = "";
            EmployeeModel employeeModel = new EmployeeModel();
            BenefitMailModel benefitMailModel = GetBenefitMailsByBenefitId(benefitRequestModel.Benefit.Id);
            if (benefitMailModel != null)
            {
                List<string> sendToMails = benefitMailModel.SendTo.Split(";").ToList();
                if(benefitRequestModel.SendTo !=0)
                {
                   string giftTo = _employeeService.GetEmailOfEmployee(benefitRequestModel.SendTo);
                    sendToMails.Add(giftTo);
                    employeeModel = _employeeService.GetEmployee(benefitRequestModel.EmployeeId).Result;
                }
                else
                {
                    employeeModel = _employeeService.GetEmployee(benefitRequestModel.EmployeeId).Result;
                }
                if (groupMails != null)
                {
                    sendToMails.AddRange(groupMails);
                }
                if (sendToMails.Contains("time.keeping"))
                {
                   string supervisorEmail = _employeeService.GetSupervisorEmailOfEmployee(benefitRequestModel.EmployeeId);
                    if(!supervisorEmail.Contains("NotProvided"))
                    {
                        if (benefitMailModel.CarbonCopies == null)
                        {
                            benefitMailModel.CarbonCopies = supervisorEmail;
                        }
                        else
                        {
                            benefitMailModel.CarbonCopies += ";" + supervisorEmail;
                        }
                    }
                    else
                    {
                        string managerEmail = _employeeService.GetManagerEmailOfEmployee(benefitRequestModel.EmployeeId);
                        if (!managerEmail.Contains("NotProvided"))
                        {
                            if (benefitMailModel.CarbonCopies == null)
                            {
                                benefitMailModel.CarbonCopies = supervisorEmail;
                            }
                            else
                            {
                                benefitMailModel.CarbonCopies += ";" + supervisorEmail;
                            }
                        }
                    }
                }
                string employeeMail = _employeeService.GetEmailOfEmployee(benefitRequestModel.EmployeeId);
                if (!employeeMail.Contains("NotProvided"))
                {
                    if (benefitMailModel.CarbonCopies == null)
                    {
                        benefitMailModel.CarbonCopies = employeeMail;
                    }
                    else
                    {
                        benefitMailModel.CarbonCopies += ";" + employeeMail;
                    }
                }
                if(groupEmployeeModels != null)
                {
                    mailBody = _outlookSenderService.PrapareGroupMailBody(benefitRequestModel.Benefit.BenefitCard, benefitRequestModel.Benefit.Name, benefitRequestModel.ExpectedDateFrom, benefitRequestModel.ExpectedDateTo, groupEmployeeModels);
                   // mailBody = "testtt";
                }
                else
                {
                    mailBody = _outlookSenderService.PrapareMailBody(benefitRequestModel);
                    //mailBody = "testtt";
                }
                
                
                if (benefitMailModel.CarbonCopies == null)
                {
                   // _outlookSenderService.SendFromOutlook(mailBody, sendToMails, null);
                    _emailSender.SendEmailAsync(mailBody, sendToMails, "More4U Request", benefitRequestModel.EmployeeId, benefitRequestModel.Benefit.Name, null);

                   // _mGraphMailService.SendAsync(mailBody, sendToMails, "More4U Request", benefitRequestModel.EmployeeId, benefitRequestModel.Benefit.Name, null);

                }
                else
                {
                    string[] CCMails = benefitMailModel.CarbonCopies.Split(";");
                   // _outlookSenderService.SendFromOutlook(mailBody, sendToMails, "More4U Request",CCMails);
                    _emailSender.SendEmailAsync(mailBody, sendToMails, "More4U Request", benefitRequestModel.EmployeeId, benefitRequestModel.Benefit.Name,CCMails);
                    //_mGraphMailService.SendAsync(mailBody, sendToMails, "More4U Request", benefitRequestModel.EmployeeId, benefitRequestModel.Benefit.Name, CCMails);

                }

            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
