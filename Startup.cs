using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using robert_brands_com.Filter;
using System.Security.Claims;
using robert_brands_com.Repositories;

namespace robert_brands_com
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
            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options => Configuration.Bind("AzureAd", options));

            services.AddRazorPages().AddMvcOptions(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.Filters.Add(new RejectFilter()); // rbrands: Filter a list of rejected browsers
            });

            // rbrands Policy based on claims: https://docs.microsoft.com/en-us/aspnet/core/security/authorization/claims
            // and https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy(KnownRoles.PolicyIsBlogAuthor, policy => policy.RequireRole(KnownRoles.BlogAuthorRoles));
                options.AddPolicy(KnownRoles.PolicyIsCalendarCoordinator, policy => policy.RequireRole(KnownRoles.CalendarCoordinatorRoles));
            });
            // rbrands: Configure standard services.
            // For dependency injection in ASP.NET Core see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection

            // Inject IOptions<DbConfig> for Cosmos DB
            services.Configure<DbConfig>(Configuration.GetSection("SiteDB"));
            DbConfig dbConfig = Configuration.GetSection("SiteDB").Get<DbConfig>();

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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
