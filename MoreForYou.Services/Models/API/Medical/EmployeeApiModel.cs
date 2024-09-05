using MoreForYou.Services.Models.MaterModels;
using MoreForYou.Services.Models.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Models.API.Medical
{
    public class EmployeeApiModel
    {
        public string name { get; set; }

        public string employeeNumber { get; set; }
    }
    public class EmployeeMedicalRequestModel
    {
        public MedicalRequestApiModel medicalRequestApiModel { get; set; }

        public EmployeeRelativesModel relatives { get; set; }

        public List<MedicalDetailsModel> Medications { get; set; }

        public List<MedicalDetailsModel> Checkup { get; set; }

        public string userId { get; set; }

        public MedicalMain medicalMain { get; set; }

        public List<MedicalSubCategoryModel> medicalSubCategory { get; set; }
    }
    public class EmployeeRelativesApi
    {
        public EmployeeRelativesApiModel RelativesApiModel { get; set; }

        public List<MedicalCategoryAPIModel> medicalCategoryAPIModels { get; set; }

        public List<MedicalSubCategoryAPIModel> medicalSubCategoryAPIModels { get; set; }

        public List<MedicalDetailsAPIModel> medicalDetailsAPIModels { get; set; }
    }
    public class EmployeeRelativesApiModel
    {
        public int CemexId { get; set; }

        public int RelativeId { get; set; }

        public string EmployeeName { get; set; }

        public DateTime BirthDate { get; set; }

        public string MedicalCoverage { get; set; }

        public string ProfilePicture { get; set; }

        public string EmployeeDepartment { get; set; }

        public List<RelativeApiModel> Relatives { get; set; }
    }
    public class EmployeeRelativesModel
    {
        public int CemexId { get; set; }

        public int RelativeId { get; set; }

        public string EmployeeName { get; set; }

        public DateTime BirthDate { get; set; }

        public string MedicalCoverage { get; set; }

        public bool IsActive { get; set; }

        public List<RelativeApiModel> Relatives { get; set; }

        public EmployeeModel employeeModel { get; set; }
    }
}
