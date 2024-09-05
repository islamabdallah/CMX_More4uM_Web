using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.Entity;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Implementation
{
    public class DepartmentService : GenericService, IDepartmentService
    {
        private readonly IRepository<Department, long> _repository;
        private readonly ILogger<DepartmentService> _logger;
        private readonly IMapper _mapper;

        public DepartmentService(IRepository<Entity<long>, long> repository, IRepository<Department, long> departmentRepository,
          ILogger<DepartmentService> logger, IMapper mapper):base(repository) 
        {
            _repository = departmentRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public Task<bool> CreateDepartment(DepartmentModel model)
        {
            var department = _mapper.Map<Department>(model);

            try
            {
                _repository.Add(department);

                return Task<bool>.FromResult<bool>(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return Task<bool>.FromResult<bool>(false);
        }

        public bool DeleteDepartment(long id)
        {
            throw new NotImplementedException();
        }

        public List<DepartmentModel> GetAllDepartments()
        {
            try
            {
                var departments = _repository.Find(i => i.IsVisible == true).ToList();
                var models = new List<DepartmentModel>();
                models = _mapper.Map<List<DepartmentModel>>(departments);
                return models;
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public DepartmentModel GetDepartment(long id)
        {
            try
            {
                Department department = _repository.Find(d => d.IsVisible == true && d.Id == id).First();
                DepartmentModel departmentModel = _mapper.Map<DepartmentModel>(department);
                return departmentModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public DepartmentModel GetDepartmentByName(string deptName)
        {
            try
            {
               Department department = _repository.Find(d => d.IsVisible == true && d.Name == deptName).First();
                DepartmentModel departmentModel = _mapper.Map<DepartmentModel>(department);
                return departmentModel;
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public Task<List<DepartmentModel>> GetDepartmentsByName(string deptName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDepartment(DepartmentModel model)
        {
            var department = _mapper.Map<Department>(model);

            try
            {
                _repository.Update(department);

                return Task<bool>.FromResult<bool>(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return Task<bool>.FromResult<bool>(false);
        }

    }
}
