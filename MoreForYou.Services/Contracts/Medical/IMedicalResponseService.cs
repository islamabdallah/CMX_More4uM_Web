﻿using MoreForYou.Models.Models.MedicalModels;
using MoreForYou.Services.Models.API.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts.Medical
{
    public interface IMedicalResponseService
    {
        string addMedicalResponseAsync(MedicalResponseApiModel model);
        Task<bool> SendDoctorResponseToEmployeeAsync(MedicalRequest model, long empNumber, string status);
    }
}
