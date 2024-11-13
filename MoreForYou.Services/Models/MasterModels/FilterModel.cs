using MoreForYou.Models.Models.MedicalModels;
using MoreForYou.Services.Models.API.Medical;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreForYou.Services.Models.MasterModels
{
   public class FilterModel
    {
        public EmployeeModel EmployeeModel { get; set; }
        public List<EmployeeModel> EmployeeModels { get; set; }
        public List<DepartmentModel> DepartmentModels { get; set; }
        public List<PositionModel> PositionModels { get; set; }

        public List<CompanyModel> CompanyModels { get; set; }

        public List<NationalityModel> NationalityModels { get; set; }

        public List<GenderModel> genderModels { get; set; }
        public List<MartialStatusModel> MartialStatusModels { get; set; }
        public List<Collar> collars { get; set; }
        public long EmployeeNumber { get; set; }

        public long SelectedDepartmentId { get; set; }

        public long SelectedPositionId { get; set; }

        public string EmployeeName { get; set; }

        public long SapNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public long SelectedNationalityId { get; set; }

        public string Id { get; set; }

        public DateTime JoinDate { get; set; }

        public bool DirectEmployee { get; set; }

        public int SelectedGenderId { get; set; }

        //public string Email { get; set; }



    }

    public class RelativeFilterModel
    {
        public EmployeeRelativesApiModel EmployeeRelativeModel { get; set; }

        public List<Relative>relatives { get; set; }
        public string EmployeePicrure { get; set; }
        public EmployeeModel employeeModel { get; set; }

        public long EmployeeNumber { get; set; }
    }
    public class MedicalItemsFilterModel
    {
        public List<MedicalItem> items { get; set; }
        public string text { get; set; }
        public string type { get; set; }

    }
}
