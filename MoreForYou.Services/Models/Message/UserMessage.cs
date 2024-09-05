using System;
using System.Collections.Generic;
using System.Text;

namespace MoreForYou.Services.Models.Message
{
    public class Message
    {
        public int LanguageId { get; set; }
        public string Text { get; set; }
    }
    public static class UserMessage
    {
        public static string[] Done = new string[3]
        {
            " ",
            "Done",
            "العملية تمت بنجاح"
        };

        public static string[] SuccessfulProcess = new string[3]
        {
            "",
            "Successful Process",
            "العملية تمت بنجاح"
        };

        public static string[] LoginDone = new string[3]
        {
            "",
            "Sucessful login",
            "تم الدخول بنجاح"
        };

        public static string[] LoginFailed = new string[3]
        {
            "",
            "Invalid Email Or Password",
           "البريد الالكتروني أو كلمة المرور خطأ"
        };


        public static string[] EmailNotFound = new string[3]
        {
            "",
           "Email not Exist",
           "البريد الالكتروني غير موجود"
        };

        public static string[] LoginIndirect = new string[3]
        {
            "",
           "Sorry, Service not available for this user",
           "هذه الخدمة ليست متاحة لهذا المستخدم"
           // "هذه الخدمة ليست متاحة الا للموظفين التابعين لشركة سيمكس"
        };

        public static string[] LoginInvalidNumber = new string[3]
        {
            "",
            "User Number not Exist",
            "رقم المستخدم خطأ"
        };

        public static string[] SuccessfulPasswordChange = new string[3]
        {
            "",
            "Sucessful process, Your Password has been changed",
            "تم تغيير كلمة المرور بنجاح"
        };

        public static string[] ChangePasswordFailed = new string[]
        {
            "",
            "Wrong old password",
           "عملية فاشلة ، كلمة المرور السابقة خطأ"
        };

        public static string[] InvalidEmployeeData = new string[]
        {
            "",
            "Failed Process, Invalid User Data",
            "عملية فاشلة ، بيانات المستخدم خطأ"
        };

        public static string[] InValidData = new string[]
        {
            "",
            "Invaild data",
            "بيانات خاطئة"
        };

        public static string[] FailedProcess = new string[]
        {
            "",
            "Failed Process",
            "عملية فاشلة"
        };

        public static string[] WrongToken = new string[]
            {
                "",
                "session has been expired!",
                "يرجى تسجيل الدخول مرة أخرى!"
            };

        //public static List<Message> Done = new List<Message>
        //{
        //    new Message {LanguageId=2, Text="Done"},
        //    new Message {LanguageId=1, Text="تم بنجاح"}
        //};

        //public static List<Message> SuccessfulProcess = new List<Message>
        //{
        //    new Message {LanguageId=2, Text="Successful Process"},
        //    new Message {LanguageId=1, Text="العملية تمت بنجاح"}
        //};

        //public static List<Message> LoginDone = new List<Message>
        //{
        //    new Message {LanguageId=2, Text="Sucessful login"},
        //    new Message {LanguageId=1, Text="تم الدخول بنجاح"}
        //};

        //public static List<Message> LoginFailed = new List<Message>
        //{
        //    new Message {LanguageId=2, Text="Invalid Email Or Password"},
        //    new Message {LanguageId=1, Text="البريد الالكتروني أو كلمة السر خطأ"}
        //};


        //public static List<Message> EmailNotFound= new List<Message>
        //{
        //    new Message {LanguageId=2, Text="Email not Exist"},
        //    new Message {LanguageId=1, Text="البريد الالكتروني غير موجود"}
        //};

        //public static List<Message> LoginIndirect = new List<Message>
        //{
        //    new Message {LanguageId=2, Text="Sorry, Service not available for non-direct employee"},
        //    new Message {LanguageId=1, Text="هذه الخدمة ليست متاحة الا للموظفين التابعين لشركة سيمكس"}
        //};

        //public static List<Message> LoginInvalidNumber = new List<Message>
        //{
        //    new Message {LanguageId=2, Text="Employee Number not Exist"},
        //    new Message {LanguageId=1, Text="رقم الموظف خطأ"}
        //};

        //public static List<Message> SuccessfulPasswordChange = new List<Message>
        //{
        //    new Message {LanguageId=2, Text="Sucessful process, Your Password has been changed"},
        //    new Message {LanguageId=1, Text="تم تغيير كلمة السر بنجاح"}
        //};

        //public static List<Message> ChangePasswordFailed = new List<Message>
        //{
        //    new Message {LanguageId=2, Text="Can not change to old password"},
        //    new Message {LanguageId=1, Text="عملية فاشلة ، لاتستطيع تغيير كلمة السر"}
        //};

        //public static List<Message> InvalidEmployeeData = new List<Message>
        //{
        //    new Message {LanguageId=2, Text="Failed Process, invalid Employee Data"},
        //    new Message {LanguageId=1, Text="عملية فاشلة ، بيانات الموظف خطأ"}
        //};

        //public static List<Message> InValidData = new List<Message>
        //{
        //    new Message {LanguageId=2, Text="Invaild data"},
        //    new Message {LanguageId=1, Text="بيانات خاطئة"}
        //};
    }
}
