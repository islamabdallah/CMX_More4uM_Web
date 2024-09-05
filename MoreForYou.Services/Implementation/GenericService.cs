using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Implementation
{
    public class GenericService
    {
        protected  IRepository<Entity<long>, long> _baserepository;

        public GenericService(IRepository<Entity<long>, long> repository)
        {
            _baserepository = repository;
        }


        public long GetLatestId()
        {
            long lastId = -1;
            try
            {
                lastId = _baserepository.Find(e => e.IsVisible == true).OrderBy(e => e.CreatedDate).LastOrDefault().Id;
            }
            catch (Exception e)
            {
            }
            return lastId;
        }

    }
}
