using System;
using System.Collections.Generic;
using System.Text;

namespace MoreForYou.Services.Models.MasterModels
{
    public class RequestModel
    {
        public int Id { get; set; }
        public int numberOfDays { get; set; }

        public int requiredDocuments { get; set; }
        public bool IsAgift { get; set; }

        public DateTime ExpectedDateFrom { get; set; }
    }
}
