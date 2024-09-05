using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MoreForYou.Models.Auth;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.MaterModels
{
    [Index(nameof(EmployeeModel.SapNumber), IsUnique = true)]
    [Index(nameof(EmployeeModel.Id), IsUnique = true)]
    [Index(nameof(EmployeeModel.EmployeeNumber), IsUnique = true)]
    [Index(nameof(EmployeeModel.PhoneNumber), IsUnique = true)]

    public class EmployeeModel
    {
        [Required]
        [MaxLength(500) ]
        public string FullName { get; set; }

        [Required(ErrorMessage ="You must enter Sap Number For the employee")]
        public long SapNumber { get; set; }

        [Required]
        [Range(1, 2, ErrorMessage = "You must select a gender For the employee")]
        public int Gender { get; set; }
        public string GenderString { get; set; }


        [Required]
        public DateTime JoiningDate { get; set; }
        public string JoiningDateString { get; set; }


        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [Range(1,4, ErrorMessage = "You must select a Marital status For the employee")]
        public int MaritalStatus { get; set; }
        public string MaritalStatusString { get; set; }

        [Required]
        [Range(1,1000, ErrorMessage = "You must select a Departement For the employee")]
        public long DepartmentId { get; set; }

        [Required]
        public DepartmentModel Department { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "You must select a Position For the employee")]
        public long PositionId { get; set; }

        public PositionModel Position { get; set; }

        public string UserId { get; set; }

        public EmployeeModel Supervisor { get; set; }

        [Required]
        public long? SupervisorId { get; set; }

        public bool isDeptManager { get; set; }

        [Required]
        public string Address { get; set; }

        public NationalityModel Nationality { get; set; }

        [Required]
        [Range(1, 2, ErrorMessage = "You must select a Nationality for the employee")]
        public long NationalityId { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public long EmployeeNumber { get; set; }

        public bool IsDelted { get; set; }

        public bool IsVisible { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        [Required]
        [MinLength(14)]
        [MaxLength(14)]
        public string Id { get; set; }
        public List<DepartmentModel> DepartmentModels { get; set; }
        public List<NationalityModel> NationalityModels { get; set; }
        public List<EmployeeModel> EmployeeModels { get; set; }
        public List<PositionModel> PositionModels { get; set; }
        public List<GenderModel> genderModels { get; set; }
        public List<MartialStatusModel> MartialStatusModels { get; set; }

        public List<CompanyModel> companyModels { get; set; }


        [RegularExpression("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$",ErrorMessage ="Invalid Mail format")]
        [Required]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",ErrorMessage ="Passord must consits of 8 characters with at least one Cpaital letter, one small letter and one special character")]
        public string Password { get; set; }


        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }

        public EmployeeDependent EmployeeDependent { get; set; } 

        public List<RoleModel> RoleModels { get; set; }
                public string RoleId { get; set; }

        public RoleModel RoleModel { get; set; }
        public string[] AsignedRolesNames { get; set; }

        [Required]
        public long HRId { get; set; }
        public string SupervisorName { get; set; }

        public string ProfilePicture { get; set; }

        public FilterModel FilterModel { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "You must select a Company For the employee")]
        public int CompanyId { get; set; }

        [Required]
        public CompanyModel Company { get; set; }

        [Required]
        [Range(1, 2, ErrorMessage = "You must select a Collar For the employee")]
        public int Collar { get; set; }
        public string CollarString { get; set; }


        public List<Collar> Collars { get; set; }

        public List<CompanyModel> Companies { get; set; }

        public int WorkDuration { get; set; }
        public string BirthDateString { get; set; }
        public string CollarText { get; set; }

        public int PendingRequestsCount { get; set; }

        public bool hasRequests { get; set; }

        public bool IsTheGroupCreator { get; set; }

        [Required]
        public string UserToken { get; set; }
        public UserSetting userSetting { get; set; }

        public string oldPassword { get; set; }

        [RegularExpression(@"^(?=.*\d)(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z]).{8,32}$")]
        public string newPassword { get; set; }

        [RegularExpression(@"^(?=.*\d)(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z]).{8,32}$")]
        [Compare("newPassword")]
        public string confirmPassword { get; set; }
        public IFormFile ProfilePicturePath { get; set; }
        [Required]
        public string Country { get; set; }

        [Required]
        public bool HasChilderen { get; set; }

        [Required]
        public bool IsDirectEmployee { get; set; }
    }

    public class Participant
    {
        public long UserNumber { get; set; }

        public string FullName { get; set; }

        public string ProfilePicture { get; set; }
    }
}
