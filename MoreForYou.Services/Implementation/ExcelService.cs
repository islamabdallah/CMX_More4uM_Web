using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;

namespace MoreForYou.Services.Implementation
{
    public class ExcelService : IExcelService
    {
        private readonly ILogger<ExcelService> _logger;

        public ExcelService(ILogger<ExcelService> logger)
        {
            _logger = logger;
        }

        public DataTable ReadExcelData(string filePath, string excelConnectionString)
        {
            try
            {
                DataTable dt = new DataTable();
                excelConnectionString = string.Format(excelConnectionString, filePath);

                using (OleDbConnection connExcel = new OleDbConnection(excelConnectionString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                           // Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();
                        }
                    }
                }
                return dt;
            }
            catch (Exception e)
            {

            }
            return null;
        }


        public MemoryStream ExportDriversDataToExcel(List<EmployeeUserData> employeeDatas)
        {
            try
            {
                DataTable dt = new DataTable("EmployeeData");
                dt.Columns.AddRange(new DataColumn[4] {
                                            new DataColumn("Employee Number"),
                                            new DataColumn("FullName"),
                                            new DataColumn("Employee Mail"),
                                            new DataColumn("Password"),});


                foreach (var employee in employeeDatas)
                {
                    dt.Rows.Add(employee.Number, employee.FullName, employee.Email, employee.Password);
                }
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return stream;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

    }
}
