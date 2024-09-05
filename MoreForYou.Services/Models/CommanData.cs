using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreForYou.Services.Models
{
    public class CommanData
    {
        public static int Individual = 2;
        public static int Group = 3;
        //public static string UploadMainFolder = @"D:\Work\MoreForYou\Project_26_12_2022 (2)\Project\MoreForeYou\wwwroot/";
        public static string UploadMainFolder = @"C:\inetpub\wwwroot\_more4u\wwwroot/";
        //public static string UploadMainFolder = @"D:\Work\MoreForYou\Project_26_12_2022 (2)\Project\MoreForeYou\wwwroot/";
        public static string DocumentsFolder = "BenefitRequestFiles/";
        public static string ProfileFolder = @"images/userProfile/";
        public static string CardsFolder = @"images/BenefitCards/";
        public static string MedicalSubCategoryFolder = @"images/MedicalSubCategory/";
        public static string MedicalCategoryFolder = @"images/MedicalCategory/";

        public static string MedicalDetailsFolder = @"images/MedicalDetails/";

        public static string Url = "https://more4u.cemex.com.eg/more4u/";
        public static string APIUrl = "https://more4u.cemex.com.eg/More4uAPI/";


        public static string TermsPath = @"D:\_cemex\_projects\_AzureMore4U\More4UAzure\MoreForYou.Services\Implementation\TermsConditions\StaticFile\TermsConditionsFiles\";

     //Azure configuration
        public static string AzureBlobUrl = "https://westdmfup001.blob.core.windows.net/";

        public static string AzureDocumentsFolder = "benefitfiles";
        public static string AzureProfileFolder = "userProfile";
        public static string AzureCardsFolder = "BenefitCards";
        public static string AzureMedicalSubCategoryFolder = "MedicalSubCategory";
        public static string AzureMedicalCategoryFolder = "MedicalCategory";

        public static string AzureMedicalDetailsFolder = "MedicalDetails";
        public enum Languages
        {
            Arabic = 2,
            English = 1
        };

        public enum MaritialStatus
        {
            Single = 1,
            Married = 2,
            Divorced = 3,
            Widower = 4,
            Any = -1
        };

        public enum ArabicMaritialStatus
        {
            أعزب = 1,
            متزوج = 2,
            مطلق = 3,
            أرمل = 4,
            لايوجد = -1
        };


        public enum Gender
        {
            Any = -1,
            Male = 1,
            Female = 2
        };

        public enum ArabicGender
        {
            لايوجد = -1,
            ذكر = 1,
            أنثي = 2
        };

        public enum BenefitStatus
        {
            Pending = 1,
            InProgress = 2,
            Approved = 3,
            Rejected = 4,
            Cancelled = 5,
            NotStartedYet = 6
        };


        public enum CollarTypes
        {
            Any = -1,
            WhiteCollar = 1,
            blueCollar = 2,
        };

        public enum ArabicCollarTypes
        {
            لايوجد = -1,
            WhiteCollar = 1,
            blueCollar = 2,
        };

        public enum BenefitTypes
        {
            Individual = 2,
            Group = 3,
        };


        public List<GenderModel> genderList = new List<GenderModel>()
            {
                new GenderModel {  Name= "Male"},
                new GenderModel {  Name="Female"}
            };

        public static List<GenderModel> genderModels = new List<GenderModel>()
            {
                new GenderModel {Id=1 , Name="Male"},
                new GenderModel {Id=2 ,  Name="Female"}
            };

        public List<ResonseStatus> resonseStatuses = new List<ResonseStatus>()
        {
            new ResonseStatus {Id =-1, Name ="None"},
            new ResonseStatus {Id =1, Name ="Approve"},
            new ResonseStatus {Id =1, Name ="Disapprove"},
        };

        public static List<RequestStatusModelAPI> whoIsConcernRequestStatusModels = new List<RequestStatusModelAPI>()
        {
            new RequestStatusModelAPI {Id =-1, Name ="Select Status"},
            new RequestStatusModelAPI {Id =1, Name ="Pending"},
            new RequestStatusModelAPI {Id =3, Name ="Approved"},
            new RequestStatusModelAPI {Id =4, Name ="Rejected"},

        };

        public static List<RequestStatusModelAPI> RequestStatusModels = new List<RequestStatusModelAPI>()
        {
            new RequestStatusModelAPI {Id =2, Name ="InProgress"},
            new RequestStatusModelAPI {Id =1, Name ="Pending"},
            new RequestStatusModelAPI {Id =3, Name ="Approved"},
            new RequestStatusModelAPI {Id =4, Name ="Rejected"},
            new RequestStatusModelAPI {Id =5, Name ="Cancelled"},
            new RequestStatusModelAPI {Id =0, Name =""},



        };

        public string MailBody = "Dears </br> <p></p>";

        public static List<TimingModel> timingModels = new List<TimingModel>()
        {
             new TimingModel{Id=-1, Name="Date"},
             new TimingModel{Id=1, Name="Today"},
            new TimingModel{Id=2, Name="Last Day"},
            new TimingModel{Id=3, Name="Current Week"},
            new TimingModel{Id=4, Name="Current Month"},
        };

        public static List<Collar> Collars = new List<Collar>()
        {
            new Collar { Id = -1, Name = "Any" },
            new Collar { Id = 1, Name = "White Collar" },
            new Collar { Id = 2, Name = "Blue Collar" }

        };

        public static List<BenefitTypeModel> BenefitTypeModels = new List<BenefitTypeModel>()
        {
               new BenefitTypeModel { Id = -1, Name = "Any" },
            new BenefitTypeModel { Id = 2, Name = "Individual" },
            new BenefitTypeModel { Id = 3, Name = "Group" }

        };

        public static List<MartialStatusModel> martialStatusModels = new List<MartialStatusModel>()
        {
            new MartialStatusModel {Id =-1 , Name="Any"},
            new MartialStatusModel {Id =1 , Name="Single"},
            new MartialStatusModel {Id=2, Name="Married" },
            new MartialStatusModel {Id=3, Name="Divorced" },
            new MartialStatusModel {Id=4, Name="Widower"}
        };

        public List<string> AgeSigns = new List<string>()
        {
           ">",
           "<",
           "="
        };

        public List<string> DatesToMatch = new List<string>()
        {
            "Any",
           "Birth Date",
           "Join Date",
           "certain Date"
        };

    }

    public class ResonseStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Collar
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class MartialStatusModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
