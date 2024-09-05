using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Models.API.Medical
{
    public class MedicalItemsAPIModel
    {
        public string itemId
        { get; set; }

        public string itemName { get; set; }

        public string itemType { get; set; }

        public string itemQuantity { get; set; }

        public string? itemDateFrom
        { get; set; }

        public string? itemDateTo { get; set; }
    }
}
