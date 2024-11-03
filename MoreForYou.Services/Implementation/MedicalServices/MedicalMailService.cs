using AutoMapper;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MedicalModels;
using MoreForYou.Services.Contracts.Email;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Contracts.Medical;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repository;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Models;
using System.IO;
using MoreForYou.Services.Models.API.Medical;
using Repository.EntityFramework;

namespace MoreForYou.Services.Implementation.MedicalServices
{
    public class MedicalMailService : IMedicalMailService
    {
        private readonly ILogger<BenefitMailService> _logger;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IEmployeeService _employeeService;
        private readonly IOutlookSenderService _outlookSenderService;
        private readonly IMGraphMailService _mGraphMailService;
        public APPDBContext _context;

        public MedicalMailService(ILogger<BenefitMailService> logger,
            IMapper mapper,
            IEmailSender emailSender,
            IOutlookSenderService outlookSenderService,
            IEmployeeService employeeService,
            IMGraphMailService mGraphMailService, APPDBContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _emailSender = emailSender;
            _outlookSenderService = outlookSenderService;
            _employeeService = employeeService;
            _mGraphMailService = mGraphMailService;
            _context = context;
        }
        public bool SendToMailList(MedicalRequestDetailsModel model, List<string> groupMails = null, List<GroupEmployeeModel> groupEmployeeModels = null)
        {
            if (model.MedicalRequest.medicalPurpose == "Medication")
            {
                List<string> sendToMails = new List<string>()
          {
            "lamia.mousa@ext.cemex.com",
           // "eman.rasmy @cemex.com",
           // "islammohamed.abdallah@cemex.com"
          };
                //  mailBody = _outlookSenderService.PrapareMailBody(benefitRequestModel);
                string body = string.Empty;
                string fileName = "Medication.html";
                string path = Path.Combine(@"D:\_cemex\_projects\_AzureMore4U\More4UAzure\MoreForYou.Services\MailTemplate\", fileName);
                using (StreamReader reader = new StreamReader(path))
                {
                    body = reader.ReadToEnd();
                }
                string items = "";
                if (model.MedicalResponse != null)
                {
                    if (model.MedicalResponse.medicalItems != null)
                    {
                        if (model.MedicalResponse.medicalItems.Count > 0)
                        {
                            foreach (var item in model.MedicalResponse.medicalItems)
                            {
                                items += "<tr><td>" + item.itemName + "," + item.itemType + "," + item.itemDose + "</td>";
                                items += "<td>" + item.itemQuantity + "</td></tr>";
                            }
                        }

                    }
                }
                body = body.Replace("{Items}", items);
                var txt=_context.Relatives.Where(t=>t.EmployeeNumber == Convert.ToInt64(model.MedicalRequest.requestedByNumber) && t.Relation=="Self").FirstOrDefault();
                if (txt != null)
                {
                    if (txt.ArabicRelatives == "")
                    {
                        body = body.Replace("{EmployeeName}", txt.Relatives);
                    }
                    else
                    {
                        body = body.Replace("{EmployeeName}", txt.ArabicRelatives);
                    }
                }
                else
                {
                    body = body.Replace("{EmployeeName}", model.MedicalRequest.requestedBy);
                }
               
                body = body.Replace("{EmployeeNumber}", model.MedicalRequest.requestedByNumber);//for emp coverage              
                body = body.Replace("{RelCoverage}", model.MedicalRequest.relativeCoverage);
                body = body.Replace("{Relation}", model.MedicalRequest.relation);
                body = body.Replace("{RequestDate}", Convert.ToDateTime(model.MedicalResponse.responseDate).Date.ToString("yyyy-MM-dd"));

                _emailSender.SendEmailAsync(body, sendToMails, "Medical Approve", Convert.ToInt64(model.MedicalRequest.requestedByNumber), "Medication", null);
            }
           else if (model.MedicalRequest.requestType==2 && model.MedicalResponse.medicalItems.Count>0)
            {
                List<string> sendToMails = new List<string>()
          {
            "lamia.mousa@ext.cemex.com",
           // "eman.rasmy @cemex.com",
           // "islammohamed.abdallah@cemex.com"
          };
                //  mailBody = _outlookSenderService.PrapareMailBody(benefitRequestModel);
                string body = string.Empty;
                string fileName = "LapWithX_rays.html";
                string path = Path.Combine(@"D:\_cemex\_projects\_AzureMore4U\More4UAzure\MoreForYou.Services\MailTemplate\", fileName);
                using (StreamReader reader = new StreamReader(path))
                {
                    body = reader.ReadToEnd();
                }
                string items = "";
                if (model.MedicalResponse != null)
                {
                    if (model.MedicalResponse.medicalItems != null)
                    {
                        if (model.MedicalResponse.medicalItems.Count > 0)
                        {
                            foreach (var item in model.MedicalResponse.medicalItems)
                            {
                                items += "<tr><td>" + item.itemName + "</td></tr>";
                             
                            }
                        }

                    }
                }
                body = body.Replace("{Items}", items);
                var txt = _context.Relatives.Where(t => t.EmployeeNumber == Convert.ToInt64(model.MedicalRequest.requestedByNumber) && t.Relation == "Self").FirstOrDefault();
                if (txt != null)
                {
                    if (txt.ArabicRelatives == "")
                    {
                        body = body.Replace("{EmployeeName}", txt.Relatives);
                    }
                    else
                    {
                        body = body.Replace("{EmployeeName}", txt.ArabicRelatives);
                    }
                }
                else
                {
                    body = body.Replace("{EmployeeName}", model.MedicalRequest.requestedBy);
                }
                // body = body.Replace("{EmployeeName}", model.MedicalRequest.requestedBy);
                body = body.Replace("{EmployeeNumber}", model.MedicalRequest.requestedByNumber);//for emp coverage
                body = body.Replace("{Relation}", model.MedicalRequest.relation);
                body = body.Replace("{RelCoverage}", model.MedicalRequest.relativeCoverage);
                body = body.Replace("{Coverage}", model.MedicalRequest.employeeCoverage);
                body = body.Replace("{RequestDate}", Convert.ToDateTime(model.MedicalResponse.responseDate).Date.ToString("yyyy-MM-dd"));
                //body = body.Replace("{RequestDate}", model.MedicalResponse.responseDate);
                body = body.Replace("{EntityName}", model.MedicalResponse.medicalEntity);
                body = body.Replace("{RelativeName}", model.MedicalRequest.requestedFor);

                _emailSender.SendEmailAsync(body, sendToMails, "Medical Approve", Convert.ToInt64(model.MedicalRequest.requestedByNumber), "Medical transfer", null);
            }
            else if (model.MedicalRequest.medicalPurpose == "hospital Admission" || model.MedicalRequest.medicalPurpose == "اجراء عملية")
            {
                List<string> sendToMails = new List<string>()
          {
            "lamia.mousa@ext.cemex.com",
           // "eman.rasmy @cemex.com",
           // "islammohamed.abdallah@cemex.com"
          };
                //  mailBody = _outlookSenderService.PrapareMailBody(benefitRequestModel);
                string body = string.Empty;
                string fileName = "HospitalAdmition.html";
                string path = Path.Combine(@"D:\_cemex\_projects\_AzureMore4U\More4UAzure\MoreForYou.Services\MailTemplate\", fileName);
                using (StreamReader reader = new StreamReader(path))
                {
                    body = reader.ReadToEnd();
                }
                var txt = _context.Relatives.Where(t => t.EmployeeNumber == Convert.ToInt64(model.MedicalRequest.requestedByNumber) && t.Relation == "Self").FirstOrDefault();
                if (txt != null)
                {
                    if (txt.ArabicRelatives == "")
                    {
                        body = body.Replace("{EmployeeName}", txt.Relatives);
                    }
                    else
                    {
                        body = body.Replace("{EmployeeName}", txt.ArabicRelatives);
                    }
                }
                else
                {
                    body = body.Replace("{EmployeeName}", model.MedicalRequest.requestedBy);
                }
                body = body.Replace("{RelativeName}", model.MedicalRequest.requestedFor);
                body = body.Replace("{EmployeeNumber}", model.MedicalRequest.requestedByNumber);//for emp coverage
                body = body.Replace("{Relation}", model.MedicalRequest.relation);
                body = body.Replace("{RelCoverage}", model.MedicalRequest.relativeCoverage);
                body = body.Replace("{EntityName}", model.MedicalResponse.medicalEntity);
                body = body.Replace("{RequestDate}", Convert.ToDateTime(model.MedicalResponse.responseDate).Date.ToString("yyyy-MM-dd"));
                _emailSender.SendEmailAsync(body, sendToMails, "Medical Approve", Convert.ToInt64(model.MedicalRequest.requestedByNumber), "Medical transfer", null);
            }

            else
            {
                List<string> sendToMails = new List<string>()
          {
            "lamia.mousa@ext.cemex.com",
           // "eman.rasmy @cemex.com",
           // "islammohamed.abdallah@cemex.com"
          };
                //  mailBody = _outlookSenderService.PrapareMailBody(benefitRequestModel);
                string body = string.Empty;
                string fileName = "CheckUp.html";
                string path = Path.Combine(@"D:\_cemex\_projects\_AzureMore4U\More4UAzure\MoreForYou.Services\MailTemplate\", fileName);
                using (StreamReader reader = new StreamReader(path))
                {
                    body = reader.ReadToEnd();
                }

                var txt = _context.Relatives.Where(t => t.EmployeeNumber == Convert.ToInt64(model.MedicalRequest.requestedByNumber) && t.Relation == "Self").FirstOrDefault();
                if (txt != null)
                {
                    if (txt.ArabicRelatives == "")
                    {
                        body = body.Replace("{EmployeeName}", txt.Relatives);
                    }
                    else
                    {
                        body = body.Replace("{EmployeeName}", txt.ArabicRelatives);
                    }
                }
                else
                {
                    body = body.Replace("{EmployeeName}", model.MedicalRequest.requestedBy);
                }
                //body = body.Replace("{EmployeeName}", model.MedicalRequest.requestedBy);
                body = body.Replace("{EmployeeNumber}", model.MedicalRequest.requestedByNumber);//for emp coverage

                body = body.Replace("{RelativeName}", model.MedicalRequest.requestedFor);
                body = body.Replace("{Relation}", model.MedicalRequest.relation);
              //  body = body.Replace("{RequestDate}", model.MedicalResponse.responseDate);
                body = body.Replace("{RequestDate}", Convert.ToDateTime(model.MedicalResponse.responseDate).Date.ToString("yyyy-MM-dd"));
                body = body.Replace("{EntityName}", model.MedicalResponse.medicalEntity);

                _emailSender.SendEmailAsync(body, sendToMails, "Medical Approve", Convert.ToInt64(model.MedicalRequest.requestedByNumber), "Medical transfer", null);
            }

            // throw new NotImplementedException();
            return true;
        }
    }
}
