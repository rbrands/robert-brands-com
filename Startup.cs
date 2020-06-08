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
using robert_brands_com.Repositories;
using System.Security.Claims;
using robert_brands_com.Models;
using Microsoft.AspNetCore.Mvc;
using NetEscapades.AspNetCore.SecurityHeaders;


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
            // rbrands: For Application Insights see https://docs.microsoft.com/en-us/azure/azure-monitor/app/asp-net-core
            services.AddApplicationInsightsTelemetry();

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

            //
            // rbrands: Configure standard services
            //
            // For dependency injection in ASP.NET Core see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
            services.AddSingleton(typeof(IConfigurationRoot), Configuration);
            // Inject IOptions<DbConfig>
            services.Configure<DbConfig>(Configuration.GetSection("SiteDB"));
            DbConfig dbConfig = Configuration.GetSection("SiteDB").Get<DbConfig>();
            // Inject IOption<TinyMCEConfig>
            services.Configure<TinyMCEConfig>(Configuration.GetSection("TinyMCE"));
            // Inject IOptions<FunctionSiteToolsConfig>
            services.Configure<FunctionSiteToolsConfig>(Configuration.GetSection("FunctionSiteTools"));
            FunctionSiteToolsConfig functionsConfig = Configuration.GetSection("FunctionSiteTools").Get<FunctionSiteToolsConfig>();
            // rbrands: FunctionSiteTools to access Azure Functions
            FunctionSiteTools functionSiteTools = new FunctionSiteTools(functionsConfig);
            services.AddSingleton(typeof(IFunctionSiteTools), functionSiteTools);
            // Repositories for ActivityLogging - one for writing one for reading because of different interfaces. 
            services.AddSingleton(typeof(ICosmosDBRepository<ActivityLogItem>), new CosmosDBRepository<ActivityLogItem>(dbConfig));
            services.AddSingleton(typeof(IActivityLog), new ActivityLogDBRepository(Configuration, dbConfig));
            // All data repositories
            services.AddSingleton(typeof(ICosmosDBRepository<Shortcut>), new CosmosDBRepository<Shortcut>(dbConfig));
            services.AddSingleton(typeof(ICosmosDBRepository<CommentedLinkItem>), new CosmosDBRepository<CommentedLinkItem>(dbConfig));
            services.AddSingleton(typeof(ICosmosDBRepository<ListCategory>), new CosmosDBRepository<ListCategory>(dbConfig));
            services.AddSingleton(typeof(ICosmosDBRepository<TrackItem>), new CosmosDBRepository<TrackItem>(dbConfig));
            services.AddSingleton(typeof(ICosmosDBRepository<Article>), new CosmosDBRepository<Article>(dbConfig));
            services.AddSingleton(typeof(ICosmosDBRepository<ArticleTag>), new CosmosDBRepository<ArticleTag>(dbConfig));
            services.AddSingleton(typeof(ICosmosDBRepository<CalendarItem>), new CosmosDBRepository<CalendarItem>(dbConfig));
            // rbrands: Example for creating for every request services.AddScoped<ICosmosDBRepository<CalendarItem>>(provider => new CosmosDBRepository<CalendarItem>(Configuration, "SiteDB"));

            // Application Insights
            services.AddApplicationInsightsTelemetry();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // rbrands: For error handling see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-3.1
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
            app.UseStatusCodePages();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // rbrands: Custom headers for security (rbrands)
            // see https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders
            var policyCollection = new HeaderPolicyCollection()
                .AddDefaultSecurityHeaders()
                .RemoveCustomHeader("X-Frame-Options");
            // rbrands: Example for custom header:    .AddCustomHeader("X-My-Test-Header", "Header value");
            app.UseSecurityHeaders(policyCollection);


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                // rbrands: Add endpoints for shortcuts
                // See https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-3.1
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}"
                    );
                endpoints.MapControllerRoute(
                    name: "categoryLink",
                    pattern: "{category}/{nickname}",
                    defaults: new { Controller = "Shortcuts", Action = "Link" }
                   );
                endpoints.MapControllerRoute(
                    name: "link",
                    pattern: "{nickname}",
                    defaults: new { Controller = "Shortcuts", Action = "Link", Category = "Default" }
                    );
            });

        }
    }
}
