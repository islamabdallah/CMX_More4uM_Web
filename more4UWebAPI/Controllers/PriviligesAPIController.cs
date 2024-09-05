using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MoreForYou.APIController
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PriviligesAPIController : ControllerBase
    {
        private readonly IPrivilegeService _privilegeService;

        public PriviligesAPIController(IPrivilegeService privilegeService)
        {
            _privilegeService = privilegeService;
        }

        // GET: api/<PriviligesAPIController>
        [HttpGet("Getprivilges")]
        public IActionResult Getprivilges()
        {
            try
            {
                List<PrivilegeModel> privilges = _privilegeService.GetAllPrivileges().Result;
                List<PriviligeAPIModel> priviligeAPIModels = _privilegeService.CreatePriviligeAPIModel(privilges);
                return Ok(new { Message = "Success Process", Data = priviligeAPIModels });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Failed Process", Data = false });
            }

        }

        //[HttpPost("UploadedImage")]
        //public async Task<IActionResult> UploadedImageAsync(IFormFile ImageName)
        //{
        //    string uniqueFileName = null;
        //    string filePath = null;

        //    if (ImageName != null)
        //    {
        //        string uploadsFolder = Path.Combine(@"D:\Work\MoreForYou\Project\MoreForeYou\wwwroot\images\", "testUpload");
        //        uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageName.FileName;
        //        uniqueFileName = uniqueFileName.Replace(" ", "");
        //        filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await ImageName.CopyToAsync(fileStream);
        //        }

        //        //using (var dataStream = new MemoryStream())
        //        //{
        //        //    await ImageName.CopyToAsync(dataStream);
        //        //}
        //    }
        //    return Ok(new { Message = "Success Process", Data = filePath });

        //}
        //[HttpPost("UploadFormData")]
        //public async Task<string> UploadFormData(TestModelAPI testModelAPI)
        //{
        //    string uniqueFileName = null;
        //    string filePath = null;

        //    if (testModelAPI.file != null)
        //    {
        //        string uploadsFolder = Path.Combine(@"D:\Work\MoreForYou\Project\MoreForeYou\wwwroot\images\", "testUpload");
        //        uniqueFileName = Guid.NewGuid().ToString() + "_" + testModelAPI.file.FileName;
        //        uniqueFileName = uniqueFileName.Replace(" ", "");
        //        filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await testModelAPI.file.CopyToAsync(fileStream);
        //        }

        //        //using (var dataStream = new MemoryStream())
        //        //{
        //        //    await ImageName.CopyToAsync(dataStream);
        //        //}
        //    }
        //    return uniqueFileName;
        //}


    }
}
