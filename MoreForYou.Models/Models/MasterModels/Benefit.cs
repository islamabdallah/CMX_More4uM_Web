using MoreForYou.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
    public class Benefit:EntityWithIdentityId<long>
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string ArabicName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ArabicDescription { get; set; }

        [Required]
        public BenefitType BenefitType { get; set; }


        [Required]
        public long BenefitTypeId { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public bool HasWorkflow { get; set; }

        public int gender { get; set; }

        public int WorkDuration { get; set; }

        public int Age { get; set; }

        public int MaritalStatus { get; set; }

        public int MinParticipant { get; set; }

        public int MaxParticipant { get; set; }

        public char AgeSign { get; set; }

        [Required]
        public string BenefitCard { get; set; }

        [Required]
        public int Times { get; set; }

        [Required]
        public int Collar { get; set; }

        public string RequiredDocuments { get; set; }

        [Required]
        public int numberOfDays { get; set; }

        public string DateToMatch { get; set; }

        public bool MustMatch { get; set; }

        public bool IsAgift { get; set; }

        public bool HasMails { get; set; }

        public string Country { get; set; }

        public bool HasChilderen { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string ArabicTitle { get; set; }

        public string ArabicDateToMatch { get; set; }

        public string ArabicRequiredDocuments { get; set; }

        public DateTime? CertainDate { get; set; }


    }
}
