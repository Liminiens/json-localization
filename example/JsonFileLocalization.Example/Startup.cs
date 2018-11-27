using System.Collections.Generic;
using System.Globalization;
using JsonFileLocalization.Example.Localization;
using JsonFileLocalization.Middleware;
using JsonFileLocalization.Resource;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JsonFileLocalization.Example
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
            services.AddJsonFileLocalization();
            services.Configure<JsonLocalizationOptions>(opt =>
            {
                opt.CultureSuffixStrategy = CultureSuffixStrategy.TwoLetterISO6391;
                opt.ResourceRelativePath = "CustomResourcesFolder";
                opt.WatchForChanges = true;
            });
            services.AddRouting(x => x.LowercaseUrls = true);
            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru")
                };
                opts.FallBackToParentCultures = false;
                opts.FallBackToParentUICultures = false;
                opts.DefaultRequestCulture = new RequestCulture("ru", "ru");
                // Formatting numbers, dates, etc.
                opts.SupportedCultures = supportedCultures;
                // UI strings that we have localized.
                opts.SupportedUICultures = supportedCultures;
                var routeDataProvider = new RouteDataRequestCultureProvider()
                {
                    Options = opts,
                    RouteDataStringKey = "lang",
                    UIRouteDataStringKey = "lang"
                };
                opts.RequestCultureProviders.Clear();
                opts.RequestCultureProviders.Insert(0, routeDataProvider);
            });
            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("lang", typeof(LanguageRouteConstraint));
            });
            services
                .AddMvc(s => s.Filters.Add(new MiddlewareFilterAttribute(typeof(LocalizationPipeline))))
                .AddDataAnnotationsLocalization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();
            app.UseRequestLocalization();
            app.UseMvc(ConfigureRoutes);
        }

        private void ConfigureRoutes(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "LocalizedDefault",
                template: "{lang:lang}/{controller=Home}/{action=Index}/{id?}"
            );
            routes.MapRoute(
                name: "default",
                template: "{*catchall}",
                defaults: new { controller = "Home", action = "RedirectToDefaultLanguage" });
        }
    }
}
