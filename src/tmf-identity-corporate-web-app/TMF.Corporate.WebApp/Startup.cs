using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System;
using TMF.Corporate.WebApp.AuthorizationPolicies;
using TMF.Corporate.WebApp.Services;

namespace TMF.Corporate.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMicrosoftIdentityWebAppAuthentication(Configuration)
                    .EnableTokenAcquisitionToCallDownstreamApi()
                    .AddInMemoryTokenCaches();

            services.AddControllersWithViews()
                    .AddMicrosoftIdentityUI();

            services.AddRazorPages();
            services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Employee", configurePolicy =>
                {
                    configurePolicy.AddRequirements(new RoleMemberRequirement("Document.Read"));
                });

                options.AddPolicy("Director", configurePolicy =>
                {
                    configurePolicy.AddRequirements(new RoleMemberRequirement("Document.Edit"));
                });
            });
            services.AddSingleton<IAuthorizationHandler, RoleMemberHandler>();


            services.AddHttpClient<IApiService, ApiService>(configureClient =>
            {
                configureClient.BaseAddress = new Uri(Configuration.GetSection("WebApi:Url").Value);
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
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
