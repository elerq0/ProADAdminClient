using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ADAdmin
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
            //services.AddControllersWithViews();
            services.AddControllers();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "wwwroot/dist";
            });
            services.AddAuthentication(IISDefaults.AuthenticationScheme);

            /*
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ADException", policy => policy.RequireUserName(@"kglsa\w.golab"));
                options.AddPolicy("ADUsers", policy => policy.RequireRole(@"kglsa\PRO ADAdmin FrontEnd"));
                //options.AddPolicy("ADUsers", policy => policy.RequireRole(@"BUILTIN\Administratorzy"));

            });
            */
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });

            //services.AddCors();
            services.AddSingleton<Models.Users>();
            services.AddSingleton<Models.Whitelist>();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.ApplicationServices.GetService<Models.Users>();
            app.ApplicationServices.GetService<Models.Whitelist>();

            //app.UseCors(builder => builder.WithOrigins("http://www"));

            
           
             app.UseSpa(spa =>
             {
                 // To learn more about options for serving an Angular SPA from ASP.NET Core,
                 // see https://go.microsoft.com/fwlink/?linkid=864501

                 spa.Options.SourcePath = "ClientApp";
                 if (env.IsDevelopment())
                 {
                     spa.Options.StartupTimeout = new TimeSpan(0, 0, 60);
                     spa.UseAngularCliServer(npmScript: "start");
                 }
             });
      
            /*
           */
        }
        }
}
