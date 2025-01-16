using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.BusinessLogic.Interfaces;
using Demo.BusinessLogic.Repositories;
using Demo.DataAccess.Contexts;
using Demo.Presentation.MappingProfiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            ///Allow Dependenncy Injection
            services.AddDbContext<MVCAppDbContext>(Options =>
            {
                //Options.UseSqlServer("Server = .; Database = MVCAppDb; Trusted_Connection = True");   el makan el s7 ely yt7t feh el connection string hwa el ((appsetting.json)) 
                Options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });            ///needs package EFCore.SQlServer in Data access layer so need to build only

            services.AddScoped<IDepartmentRepository, DepartmentRepository>(); //allow dependency injection
            services.AddScoped<IEmployeeRepository, EmployeeRepository>(); //allow dependency injection
            services.AddAutoMapper(m=>m.AddProfile(new EmployeeProfile())); //Transient
            services.AddAutoMapper(d => d.AddProfile(new DepartmentProfile())); //Transient

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
