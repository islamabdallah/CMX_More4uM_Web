using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MoreForYou.Models.Models;
using MoreForYou.Service.Contracts.Auth;
using MoreForYou.Service.Implementation.Auth;
using MoreForYou.Service.Implementation.Email;
using MoreForYou.Service.Models.Email;
using MoreForYou.Services.Contracts.Email;
using MoreForYou.Services.Contracts.Medical;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Implementation.Email;
using MoreForYou.Services.Implementation.MedicalServices;
using MoreForYou.Services.Implementation;
using Repository.EntityFramework;
using More4UWebAPI.Data;
using MoreForYou.Services.Models.Utilities.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MoreForYou.Services.Contracts.TermsConditions;
using MoreForYou.Services.Implementation.TermsConditions;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SqlCon") ?? throw new InvalidOperationException("Connection string 'AuthDBContextConnection' not found.");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<APPDBContext>(options =>
options.UseSqlServer(connectionString));

builder.Services.AddDbContext<AuthDBContext>(options =>
{
    options.UseSqlServer(connectionString,
     b => b.MigrationsAssembly(typeof(AuthDBContext).Assembly.FullName));
}, ServiceLifetime.Transient);

builder.Services.AddIdentity<AspNetUser, AspNetRole>(options =>
{
    //options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true;

})
.AddEntityFrameworkStores<AuthDBContext>()
.AddRoles<AspNetRole>().AddDefaultUI().AddDefaultTokenProviders();

var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();
builder.Services.AddAuthentication();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(authenticationScheme: JwtBearerDefaults.AuthenticationScheme,
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddScoped<DbContext, APPDBContext>();
builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
builder.Services.AddScoped<UserManager<AspNetUser>>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IBenefitService, BenefitService>();
builder.Services.AddScoped<IBenefitTypeService, BenefitTypeService>();
builder.Services.AddScoped<IBenefitWorkflowService, BenefitWorkflowService>();
builder.Services.AddScoped<IBenefitRequestService, BenefitRequestService>();
builder.Services.AddScoped<IRequestWorkflowService, RequestWorkflowService>();
builder.Services.AddScoped<IRequestStatusService, RequestStatusService>();
builder.Services.AddScoped<INationalityService, NationalityService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
//services.AddScoped<IEmployeeRequestService, EmployeeRequestService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IGroupEmployeeService, GroupEmployeeService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IUserNotificationService, UserNotificationService>();
builder.Services.AddScoped<IUserConnectionManager, UserConnectionManager>();
builder.Services.AddScoped<IPrivilegeService, PrivilegeService>();
builder.Services.AddScoped<IRequestDocumentService, RequestDocumentService>();
builder.Services.AddScoped<IFirebaseNotificationService, FirebaseNotificationService>();
builder.Services.AddScoped<IEmailSender, MailKitEmailSenderService>();
builder.Services.AddScoped<IBenefitMailService, BenefitMailService>();
builder.Services.AddScoped<IOutlookSenderService, OutlookSenderService>();
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<IMobileVersionService, MobileVersionService>();
builder.Services.AddScoped<IEmailLogService, EmailLogService>();
builder.Services.AddScoped<IMedicalCategoryService, MedicalCategoryService>();
builder.Services.AddScoped<IMedicalSubCategoryService, MedicalSubCategoryService>();
builder.Services.AddScoped<IMedicalDetailsService, MedicalDetailsService>();
builder.Services.AddScoped<IMGraphMailService, MGraphMailService>();
builder.Services.AddScoped<ITermsConditionsService, TermsConditionsService>();

builder.Services.Configure<MailKitEmailSenderOptions>(options =>
{
    options.Host_Address = builder.Configuration["ExternalProviders:MailKit:SMTP:Address"];
    options.Host_Port = Convert.ToInt32(builder.Configuration["ExternalProviders:MailKit:SMTP:Port"]);
    options.Host_Username = builder.Configuration["ExternalProviders:MailKit:SMTP:Account"];
    options.Host_Password = builder.Configuration["ExternalProviders:MailKit:SMTP:Password"];
    options.Sender_EMail = builder.Configuration["ExternalProviders:MailKit:SMTP:SenderEmail"];
    options.Sender_Name = builder.Configuration["ExternalProviders:MailKit:SMTP:SenderName"];
    options.Username = builder.Configuration["ExternalProviders:MailKit:SMTP:Username"];
    options.Host_Username = builder.Configuration["ExternalProviders:MailKit:SMTP:Host_Username"];

});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "More4UWebAPI", Version = "v1" });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigAutoMapper();
builder.Services.AddSignalR();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHsts();
//app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});
app.Run();