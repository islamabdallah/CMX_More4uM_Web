using MoreForYou.Services.Contracts.TermsConditions;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.TermsConditionsModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Implementation.TermsConditions
{
    public class TermsConditionsService : ITermsConditionsService
    {
        public async Task<TermsOfConditionsModel> LoadArabicTerms()
        {
            try
            {
                TermsOfConditionsModel termsOfConditionsModel = new TermsOfConditionsModel();
                string path = Path.Combine(CommanData.TermsPath, "ArabicTermsOfConditions.txt");
                if (File.Exists(path))
                        {
                    using (var sr = new StreamReader(path))
                    {
                        termsOfConditionsModel.Text = await sr.ReadToEndAsync();
                    }
                }
                else
                {
                    termsOfConditionsModel.Text = "لاتوجد معلومات حاليا";
                }

                return termsOfConditionsModel;
            }
            catch (Exception ex) 
            {
                return null;
            }
        }

        public async Task<TermsOfConditionsModel> LoadEnglishTerms()
        {
            try
            {
                string path = Path.Combine(CommanData.TermsPath, "EnglishTermsOfConditions.txt");
                TermsOfConditionsModel termsOfConditionsModel = new TermsOfConditionsModel();
                if (File.Exists(path))
                {

                    using (var sr = new StreamReader(path))
                    {
                        termsOfConditionsModel.Text = await sr.ReadToEndAsync();
                    }
                }
                else
                {
                    termsOfConditionsModel.Text = "No data available";
                }
                return termsOfConditionsModel;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
