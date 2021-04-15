using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationService;
using Banks.ApplicationServiceInterfaces;
using Banks.ApplicationServices;
using Core.ApplicationService;
using Domain.Repositories;
using Infrastructure.DataAccess.EFDataAccess;
using Infrastructure.DataAccess.EFDataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using WebClient.Filters;
using WebClient.Helpers;

namespace WebClient
{
    public class Startup
    {
        private readonly IWebHostEnvironment WebHostEnvironment;
        private readonly IConfiguration Configuration;

        public Startup(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            WebHostEnvironment = webHostEnvironment;
            Configuration = configuration;
            InitializeEntityFrameworkConnection(configuration);
        }

        public void InitializeEntityFrameworkConnection(IConfiguration config)
        {
            string entityFrameworkContextConnectionString = config["PSDSQL:ConnectionString"];
            EfConnectionSettings.ConfigureSqlServerConnection(entityFrameworkContextConnectionString);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string adminPass = Configuration["Admin:Pass"];
            AdminHelper.AdminPass = adminPass;
            services.AddTransient<PSDDbContext>();
            services.AddTransient<IUnitOfWork, EfUnitOfWork>();
            services.AddTransient<IUnitOfWorkFactory, EfUnitOfWorkFactory>();
            services.AddScoped<IUserRepository, EFUserRepository>();
            services.AddScoped<IBankServiceProvider, BankServiceProvider>(sp =>
            {
                var bsp = new BankServiceProvider();
                bsp.Add("dummy", new DummyBankService());
                return bsp;
            });
            services.AddScoped<UserService>();
            services.AddTransient<CustomExceptionFilterAttribute>();
            services.AddControllersWithViews();

            services.AddMvc((MvcOptions options) =>
            {
                options.Filters.Add(typeof(CustomExceptionFilterAttribute));
            });
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
