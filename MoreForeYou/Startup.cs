using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.EntityFramework;
using MoreForYou.Data;
using Microsoft.AspNetCore.Identity;
using MoreForYou.Services.Models.Utilities.Mapping;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Implementation;
using Data.Repository;
using MoreForYou.Models.Models;
using MoreForYou.Service.Implementation.Auth;
using MoreForYou.Service.Contracts.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using MoreForYou.Controllers.hub;
using MoreForYou.Service.Implementation.Email;
using MoreForYou.Service.Models.Email;
using System;
using MoreForYou.Services.Contracts.Email;
using MoreForYou.Services.Implementation.Email;
using MoreForYou.Services.Contracts.Medical;
using MoreForYou.Services.Implementation.MedicalServices;
using Castle.Core.Logging;

namespace MoreForeYou
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public IConfiguration _Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<APPDBContext>(options =>
            options.UseSqlServer(_Configuration.GetConnectionString("SqlCon")));

            var connectionString = _Configuration.GetConnectionString("SqlCon");
            services.AddDbContext<AuthDBContext>(options =>
            {
                options.UseSqlServer(connectionString,
                 b => b.MigrationsAssembly(typeof(AuthDBContext).Assembly.FullName));
            }, ServiceLifetime.Transient);

            services.AddIdentity<AspNetUser, AspNetRole>(options =>
            {
                //options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;

            })
            .AddEntityFrameworkStores<AuthDBContext>()
            .AddRoles<AspNetRole>().AddDefaultUI().AddDefaultTokenProviders();
            services.AddScoped<DbContext, APPDBContext>();
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped<UserManager<AspNetUser>>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IBenefitService, BenefitService>();
            services.AddScoped<IBenefitTypeService, BenefitTypeService>();
            services.AddScoped<IBenefitWorkflowService, BenefitWorkflowService>();
            services.AddScoped<IBenefitRequestService, BenefitRequestService>();
            services.AddScoped<IRequestWorkflowService, RequestWorkflowService>();
            services.AddScoped<IRequestStatusService, RequestStatusService>();
            services.AddScoped<INationalityService, NationalityService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            //services.AddScoped<IEmployeeRequestService, EmployeeRequestService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IGroupEmployeeService, GroupEmployeeService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserNotificationService, UserNotificationService>();
            services.AddScoped<IUserConnectionManager, UserConnectionManager>();
            services.AddScoped<IPrivilegeService, PrivilegeService>();
            services.AddScoped<IRequestDocumentService, RequestDocumentService>();
            services.AddScoped<IFirebaseNotificationService, FirebaseNotificationService>();
            services.AddScoped<IEmailSender, MailKitEmailSenderService>();
            services.AddScoped<IBenefitMailService, BenefitMailService>();
            services.AddScoped<IOutlookSenderService, OutlookSenderService>();
            services.AddScoped<IExcelService, ExcelService>();
            services.AddScoped<IMobileVersionService, MobileVersionService>();
            services.AddScoped<IEmailLogService, EmailLogService>();
            services.AddScoped<IMedicalCategoryService, MedicalCategoryService>();
            services.AddScoped<IMedicalSubCategoryService, MedicalSubCategoryService>();
            services.AddScoped<IMedicalDetailsService, MedicalDetailsService>();
            services.AddScoped<IMGraphMailService, MGraphMailService>();
            services.Configure<MailKitEmailSenderOptions>(options =>
            {
                options.Host_Address = _Configuration["ExternalProviders:MailKit:SMTP:Address"];
                options.Host_Port = Convert.ToInt32(_Configuration["ExternalProviders:MailKit:SMTP:Port"]);
                options.Host_Username = _Configuration["ExternalProviders:MailKit:SMTP:Account"];
                options.Host_Password = _Configuration["ExternalProviders:MailKit:SMTP:Password"];
                options.Sender_EMail = _Configuration["ExternalProviders:MailKit:SMTP:SenderEmail"];
                options.Sender_Name = _Configuration["ExternalProviders:MailKit:SMTP:SenderName"];
                options.Username = _Configuration["ExternalProviders:MailKit:SMTP:Username"];
                options.Host_Username = _Configuration["ExternalProviders:MailKit:SMTP:Host_Username"];

            });
            services.ConfigAutoMapper();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MoreForYou", Version = "v1" });
            });

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddMvc(option =>
            {
                option.CacheProfiles.Add("Default30",
                    new CacheProfile()
                    {
                        Duration = 30
                    });
            });
            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "");
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddMvc();
            services.AddSignalR();
            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
            });

            services.AddMvc().AddMvcOptions(options =>
            {
                options.MaxModelValidationErrors = 999999;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MoreForYou.API");
                c.EnableFilter();
            });
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
       name: "default",
       pattern: "{More4U}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

                endpoints.MapHub<NotificationHub>("/NotificationHub");

            });
        }
    }
}
