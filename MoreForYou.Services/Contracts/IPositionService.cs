using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface IPositionService
    {
        Task<List<PositionModel>> GetAllPositions();
        Task<bool> CreatePosition(PositionModel model);
        Task<bool> UpdatePosition(PositionModel model);
        bool DeletePosition(int id);
        PositionModel GetPosition(int id);
        Task<PositionModel> GetPositionByName(string name);
    }
}
