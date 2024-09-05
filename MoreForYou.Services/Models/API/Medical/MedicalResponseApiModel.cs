using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Models.API.Medical
{
    public class MedicalResponseApiModel
    {
        public string requestId { get; set; }

        public string createdBy { get; set; }

        public string status { get; set; }

        public string? responseDate { get; set; }

        public List<IFormFile>? attachment { get; set; }

        public string? medicalEntity { get; set; }

        public string? feedback { get; set; }

        public string? responseComment { get; set; }

        public List<MedicalItemsAPIModel> medicalItems { get; set; }

        public string? LanguageId { get; set; }
    }

    public class MedicalResponseModel
    {
        public string? createdBy { get; set; }

        public string? responseDate { get; set; }

        public List<string>? attachment { get; set; }

        public string? medicalEntity { get; set; }

        public string? feedback { get; set; }

        public string? responseComment { get; set; }

        public List<MedicalDetailsAPIModel>? medicalEntities{ get; set; }

        public List<MedicalItemsAPIModel> medicalItems { get; set; }

        public List<string>? feedbackCollection { get; set; }
    }
}
