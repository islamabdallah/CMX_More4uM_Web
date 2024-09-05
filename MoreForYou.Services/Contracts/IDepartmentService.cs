using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface IDepartmentService:IGenericService
    {
        List<DepartmentModel> GetAllDepartments();
        Task<bool> CreateDepartment(DepartmentModel model);
        Task<bool> UpdateDepartment(DepartmentModel model);
        bool DeleteDepartment(long id);
        DepartmentModel GetDepartment(long id);
        DepartmentModel GetDepartmentByName(string deptName);
        Task<List<DepartmentModel>> GetDepartmentsByName(string deptName);
    }
}
