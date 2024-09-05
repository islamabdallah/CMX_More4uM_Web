using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace MoreForYou.Services.Contracts
{
    public interface IExcelService
    {
        public DataTable ReadExcelData(string filePath, string excelConnectionString);

        public MemoryStream ExportDriversDataToExcel(List<EmployeeUserData> employeeDatas);
    }
}
