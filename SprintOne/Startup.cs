using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using SprintOne.Data;
using Microsoft.AspNetCore.Identity;
using SprintOne.Models;

namespace SprintOne
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

            services.AddDbContext<MatchContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>(options => {
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<MatchContext>()
             .AddDefaultTokenProviders();

            services.AddMvc();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Book}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //MatchContext.CreateAdminUser(app.ApplicationServices).Wait();
            
            MatchContext.CreateUser(app.ApplicationServices, "first", "first").Wait();
            MatchContext.CreateUser(app.ApplicationServices, "second", "second").Wait();
            MatchContext.CreateUser(app.ApplicationServices, "third", "third").Wait();
            MatchContext.CreateUser(app.ApplicationServices, "fourth", "first").Wait();
            MatchContext.CreateUser(app.ApplicationServices, "fifth", "first").Wait();
            MatchContext.CreateUser(app.ApplicationServices, "sixth", "sixth").Wait();
            MatchContext.CreateUser(app.ApplicationServices, "seventh", "seventh").Wait();
            MatchContext.CreateUser(app.ApplicationServices, "eighth", "eighth").Wait();
            MatchContext.CreateUser(app.ApplicationServices, "ninth", "ninth").Wait();
            MatchContext.CreateUser(app.ApplicationServices, "tenth", "tenth").Wait();
            

        }
    }
}
