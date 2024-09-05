using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreForYou.Services.Models.API
{
    public class TestModelAPI
    {
        public int Id { get; set; }

        public string  Name { get; set; }

        public IFormFile file { get; set; }
    }
}
