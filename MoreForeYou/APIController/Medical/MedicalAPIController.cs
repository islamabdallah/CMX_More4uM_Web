//using Microsoft.AspNetCore.Mvc;
//using MoreForYou.Services.Contracts;
//using MoreForYou.Services.Contracts.Medical;
//using MoreForYou.Services.Models.API.Medical;
//using MoreForYou.Services.Models.Medical;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace MoreForYou.APIController.Medical
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class MedicalAPIController : ControllerBase
//    {
//        private readonly IMedicalCategoryService _medicalCategoryService;

//        private readonly IMedicalSubCategoryService _medicalSubCategoryService;

//        private readonly IMedicalDetailsService _medicalDetailsService;
//        public MedicalAPIController(IMedicalCategoryService medicalCategoryService,
//            IMedicalSubCategoryService medicalSubCategoryService,
//            IMedicalDetailsService medicalDetailsService)
//        {
//            _medicalCategoryService = medicalCategoryService;
//            _medicalSubCategoryService = medicalSubCategoryService;
//            _medicalDetailsService= medicalDetailsService;
//        }
//        [HttpGet("MedicalDataTest")]
//        public async Task<ActionResult> GetMedicalData()
//        {

//            //Medical Data Category 
//            var category = new List<DataTemp>()
//            {
//                 new DataTemp(){  Name_AR = "المستشفيات" , Name_EN = "Hospitals"},
//                 new DataTemp(){  Name_AR = "مراكز البصريات" , Name_EN = "Optometry Centers"},
//                 new DataTemp(){  Name_AR = "الأشعة" , Name_EN = "Radiology Centers'"},
//                 new DataTemp(){  Name_AR = "المعامل" , Name_EN = "Laboratories"},
//                 new DataTemp(){  Name_AR = "العيادات" , Name_EN = "Clinics"},
//                 new DataTemp(){  Name_AR = "الصيدليات" , Name_EN = "Pharmacies"},
//            };

//            //Medical Data Sub Category 
//            var subCategory = new List<DataTemp>() {

//                 new DataTemp(){  Name_AR = "الرمد" , Name_EN = "Eyes" , Category= "Clinics"},
//                 new DataTemp(){  Name_AR = "العظام" , Name_EN = "Bones" , Category= "Clinics"},
//                 new DataTemp(){  Name_AR = "الأسنان" , Name_EN = "Teeth" , Category= "Clinics"},
//                 new DataTemp(){  Name_AR = "الجراحة العامة" , Name_EN = "Surgery" , Category= "Clinics"},
//                 new DataTemp(){  Name_AR = "مراكز البصريات" , Name_EN = "Optometry Centers", Category="Optometry Centers"},
//                 new DataTemp(){  Name_AR = "الأشعة" , Name_EN = "Radiology Centers'" , Category ="Radiology Centers"},
//                 new DataTemp(){  Name_AR = "الصيدليات" , Name_EN = "Pharmacies" , Category ="Pharmacies"},
//                 new DataTemp(){  Name_AR = "المعامل" , Name_EN = "Laboratories" , Category ="Laboratories"},

//            };

//            var medicalDetails = new List<MedicalVM>()
//            {
//                //Hospitals
//                new MedicalVM(){  Name = "مسشفي الزهراء التخصصي", Address = " شارع الجلاء شركة فريال ", Mobile = " 2326023-2325360", WorkingHours = "العمل 24 ساعه", Category = "Hospitals", SubCategory = "Hospitals"},
//                new MedicalVM(){  Name = "مستشفي الحياة", Address = "شارع محمود رشوان بجوار مسجد مكه", Mobile = "2285301", WorkingHours = "العمل 24 ساعه", Category = "Hospitals", SubCategory = "Hospitals" }, 
//                new MedicalVM(){  Name = "م اسيوط الجامعى - صحة المرأه - الأطفال ",Address = "جامعة اسيوط ",Mobile = "088/2413063", WorkingHours = "العمل 24 ساعه",Category = "Hospitals",SubCategory = "Hospitals" },
//                new MedicalVM(){  Name = "مستشفى سانت ماريا ",Address = "شارع الحكمدار شركة فريال",Mobile = "0882322610 & 0882310515 &1281483018",WorkingHours = "العمل 24 ساعه",Category = "Hospitals", SubCategory = "Hospitals"},
               
                
//                //Clinics - Eyes
//                  new MedicalVM(){  Name = " هاني عمر الصدفي", Address = " شارع يسري راغب – الدور الثاني برج التجاريين ", Mobile = "882331520", WorkingHours = " من الثانية حتي السابعه مساءا ماعدا الخميس والجمعة ", Category = "Clinics", SubCategory = "Eyes"},
//                  new MedicalVM(){  Name = " سمير يحيي صالح", Address = "شارع يسري راغب – السنتر التجاري – الدورالاول ", Mobile = "882330229", WorkingHours = " من السبت الي الثلاثاء من 3 الى 10 مساءا _ يوم الاربعاء من 6 الى 9 _ يوم الخميس من 1 الى 5 مساءا", Category = "Clinics", SubCategory = "Eyes"},
//                  new MedicalVM(){  Name = " محمد سعد عبدالرحمن", Address = " أول السادات ابراج التوحيد الدور الاول  ", Mobile = " 0882349191 & 01062201644", WorkingHours = "من الساعة 2 الى 7 مساءا ماعدا الجمعة ", Category = "Clinics", SubCategory = "Eyes"},
          
//                  //Clinics - Bones
//                  new MedicalVM(){  Name = "  أسامه أحمد فاروق", Address = "ميدان المحطه الدور الاول أمام المحاريث والهندسه  ", Mobile = "882356741", WorkingHours = "من الساعة الثانية مساءا حتي الثامنة من السبت الي الثلاثاء الحجز الساعة 11 ص ", Category = "Clinics", SubCategory = "Bones"},
//                  new MedicalVM(){  Name = "  محمد جمال حسن ", Address = "برج معونة الشتاء الدور الثاني  ", Mobile = "882334234", WorkingHours = "من الساعه الثالثة حتي السادسة مساءا ماعدا الخميس والجمعة ", Category = "Clinics", SubCategory = "Bones"},
//                  new MedicalVM(){  Name = "خالد محمد حسن يونس", Address = " برج الزهور الدور الخامس  ", Mobile = "0882373431 & 01020386226", WorkingHours = "من الساعة 2 الى 7 مساءا ماعدا الجمعة ", Category = "Clinics", SubCategory = "Bones"},

//                     //Clinics - Teeth
//                  new MedicalVM(){  Name = "محمد سيد ثابت ", Address = "بجوار عصير العربى أعلى محلات كنز", Mobile = "882336606", WorkingHours = " من 2 مساءا حتي 11 مساءا", Category = "Clinics", SubCategory = "Teeth"},
//                  new MedicalVM(){  Name = "أبو المواهب ابراهيم", Address = "شارع الجمهوريه_ابراج عثمان ابن عفان مدخل أ  الدور الثالث  ", Mobile = "  0882298229 -0115493331", WorkingHours = "من الساعه 12 ظهرا حتي الساعه 10 مساءا", Category = "Clinics", SubCategory = "Teeth"},
               
//            };

//            if(medicalDetails!=null)
//                return Ok(new { Flag = true, Message ="Done",Category = category , SubCategory = subCategory ,  Data = medicalDetails });
//            return BadRequest(new { Flag = false,  Message = "Error", Data = 0 }); 

//        }


//        [HttpGet("MedicalData")]
//        public async Task<ActionResult> GetAllMedicalData()
//        {
//           List<MedicalCategoryAPIModel> medicalCategoryAPIModels = new List<MedicalCategoryAPIModel>();
//           List<MedicalSubCategoryAPIModel> medicalSubCategoryAPIModels = new List<MedicalSubCategoryAPIModel>();
//           List<MedicalDetailsAPIModel> medicalDetailsAPIModels = new List<MedicalDetailsAPIModel>(); 


//           List<MedicalCategoryModel> medicalCategoryModels =_medicalCategoryService.GetAllMedicalCategories().Result;
//            if(medicalCategoryModels != null && medicalCategoryModels.Count > 0)
//            {
//              medicalCategoryAPIModels = _medicalCategoryService.ConvertMedicalCategoriesModelToMedicalCategoriesAPIModel(medicalCategoryModels);
//            }
//            List<MedicalSubCategoryModel> medicalSubCategoryModels = _medicalSubCategoryService.GetAllMedicalSubCategories().Result;
            
//            if(medicalSubCategoryModels != null && medicalSubCategoryModels.Count > 0)
//            {
//                medicalSubCategoryAPIModels = _medicalSubCategoryService.ConvertMedicalSubCategoriesModelToMedicalSubCategoriesAPIModel(medicalSubCategoryModels); 
//            }
//                List <MedicalDetailsModel>medicalDetailModels = _medicalDetailsService.GetAllMedicalDetails().Result;
//            if(medicalDetailModels != null && medicalDetailModels.Count > 0)
//            {
//                medicalDetailsAPIModels = _medicalDetailsService.ConvertMedicalDetailsModelToMedicalDetailsAPIModel(medicalDetailModels);
//            }
//            return Ok(new { Flag = true, Message ="Done", Category = medicalCategoryAPIModels, SubCategory = medicalSubCategoryAPIModels, MedicalDetails = medicalDetailsAPIModels});
//        }
//        }
//}
