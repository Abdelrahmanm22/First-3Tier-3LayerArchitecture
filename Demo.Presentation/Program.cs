using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.BusinessLogic.Interfaces;
using Demo.BusinessLogic.Repositories;
using Demo.DataAccess.Contexts;
using Demo.DataAccess.Models;
using Demo.Presentation.MappingProfiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Demo.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var Builder = WebApplication.CreateBuilder(args);
            #region Configure Services That allow Dependancy Injection
            Builder.Services.AddControllersWithViews();
            ///Allow Dependenncy Injection
            Builder.Services.AddDbContext<MVCAppDbContext>(Options =>
            {
                //Options.UseSqlServer("Server = .; Database = MVCAppDb; Trusted_Connection = True");   el makan el s7 ely yt7t feh el connection string hwa el ((appsetting.json)) 
                Options.UseSqlServer(Builder.Configuration.GetConnectionString("DefaultConnection"));
            });            ///needs package EFCore.SQlServer in Data access layer so need to build only

            Builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); //allow dependency injection
            //services.AddScoped<IEmployeeRepository, EmployeeRepository>(); //allow dependency injection
            Builder.Services.AddAutoMapper(m => m.AddProfile(new EmployeeProfile())); //Transient
            Builder.Services.AddAutoMapper(d => d.AddProfile(new DepartmentProfile())); //Transient
            Builder.Services.AddAutoMapper(d => d.AddProfile(new UserProfile())); //Transient
            Builder.Services.AddAutoMapper(d => d.AddProfile(new RoleProfile())); //Transient
            Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //services.AddScoped<UserManager<User>>();
            //services.AddScoped<SignInManager<User>>();

            Builder.Services.AddIdentity<User, IdentityRole>(Options =>
            {
                Options.Password.RequireNonAlphanumeric = true; //@ # $
                Options.Password.RequireDigit = true; //1345
                Options.Password.RequireLowercase = true; //sld
                Options.Password.RequireUppercase = true; //SLD
                //P@ssw0rd
            })
            .AddEntityFrameworkStores<MVCAppDbContext>()
            .AddDefaultTokenProviders();


            Builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(Options =>
            {
                Options.LoginPath = "Account/Login";
                Options.AccessDeniedPath = "Home/Error";

            });
            #endregion
            var app = Builder.Build();
            #region Configure Http Request Pipeliens
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

            ///Security
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
