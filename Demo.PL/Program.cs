using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.BLL;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Demo.PL.Helper;

namespace Demo.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var Builder = WebApplication.CreateBuilder(args);
            
            
            #region Configure Services That Allow Dependancy Injection 

            Builder.Services.AddControllersWithViews(); // Register Built In MVC Services
            // services.AddScoped<AppDbContext>(); // services.AddDbContext »‰” Œœ„ DbContext»” ›Ì «·⁄«œÌ „⁄ «· 
            Builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Builder.Configuration.GetConnectionString("DefaultConnection"));
            }); //MEF.Sql Ã«ÌÂ „‰  Extension Method ÊœÌ ⁄·Ì ›ﬂ—… ÂÌ«  services.AddDbContext

            Builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            Builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            Builder.Services.AddAutoMapper(M => M.AddProfile(new MapProfile()));
            Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //                    Class -> user   Class -> IdentityRole
            Builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Option =>
            {
                Option.Password.RequireNonAlphanumeric = true;
                Option.Password.RequireDigit = true;
                Option.Password.RequireLowercase = true;
                Option.Password.RequireUppercase = true;
            })
                    .AddEntityFrameworkStores<AppDbContext>() //«·Ì ÂÌ« „Õ «Ã«Â«  UserManager «·Ì ÃÊÂ CreateAsync «·Ì Add Services 
                    .AddDefaultTokenProviders();//Token «·Ì » ”„Õ·Ì «⁄„·  

            //services.AddScoped<UserManager<ApplicationUser>>(); //Ê«ÕœÂ Ê«ÕœÂ  services ﬂœÂ Âﬁ⁄œ «÷ÌﬁÂ„ 

            Builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(option =>
                    {
                        option.LoginPath = "Account/Login";
                        option.AccessDeniedPath = "Home/Error";
                    }); //Add All Service ->  1. UserManager  - 2. SignInManager - 3. RoleManager 

            #endregion

            var app = Builder.Build();
            
            #region Configure HTTP Request Pipeline Or Middlewares

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });

            #endregion

            app.Run();
        }


    }
}
