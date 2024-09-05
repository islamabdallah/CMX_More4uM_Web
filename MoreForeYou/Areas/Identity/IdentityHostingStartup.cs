using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoreForYou.Data;
using MoreForYou.Models.Models;

//[assembly: HostingStartup(typeof(WebApplication9.Areas.Identity.IdentityHostingStartup))]
namespace WebApplication9.Areas.Identity
{
    public class IdentityHostingStartup //: IHostingStartup
    {
        //public void Configure(IWebHostBuilder builder)
        //{
        //    builder.ConfigureServices((context, services) =>
        //    {
        //        services.AddDbContext<AuthDBContext>(options =>
        //            options.UseSqlServer(
        //                context.Configuration.GetConnectionString("SqlCon")));

        //        services.AddIdentity<AspNetUser, IdentityRole>(
        //            options => options.SignIn.RequireConfirmedAccount = true)
        //            .AddEntityFrameworkStores<AuthDBContext>()
        //            .AddRoles<IdentityRole>()
        //            .AddDefaultUI().AddDefaultTokenProviders();

        //    });
        //}
    }
}