using MoreForYou.Services.Models.TermsConditionsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts.TermsConditions
{
    public interface ITermsConditionsService
    {
        public Task<TermsOfConditionsModel> LoadEnglishTerms();


        public Task<TermsOfConditionsModel> LoadArabicTerms();

    }
}
