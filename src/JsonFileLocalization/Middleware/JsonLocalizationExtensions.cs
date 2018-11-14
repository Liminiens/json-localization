using JsonFileLocalization.Caching;
using JsonFileLocalization.ObjectLocalization;
using JsonFileLocalization.Resource;
using JsonFileLocalization.StringLocalization;
using JsonFileLocalization.ViewLocalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace JsonFileLocalization.Middleware
{
    public static class JsonLocalizationExtensions
    {
        /// <summary>
        /// Registers types for resource localization via json files
        /// </summary>
        /// <param name="services">service collection</param>
        /// <returns></returns>
        public static IServiceCollection AddJsonFileLocalization(this IServiceCollection services)
        {
            services.AddSingleton<JsonFileLocalizationSettings>();
            services.AddSingleton<IJsonFileResourceManager, JsonFileResourceManager>();

            services.AddTransient<IJsonFileContentCache, JsonFileContentCache>();
            services.AddTransient<IStringLocalizerFactory, JsonFileStringLocalizerFactory>();
            services.AddTransient(typeof(IStringLocalizer<>), typeof(JsonFileStringLocalizer<>));
            services.AddTransient<IObjectLocalizerFactory, JsonFileObjectLocalizerFactory>();
            services.AddTransient(typeof(IObjectLocalizer<>), typeof(JsonFileObjectLocalizer<>));

            services.AddTransient<IHtmlLocalizerFactory, JsonFileHtmlLocalizerFactory>();
            services.AddTransient(typeof(IHtmlLocalizer<>), typeof(JsonFileHtmlLocalizer<>));
            services.AddTransient<IViewLocalizer, JsonViewLocalizer>();
            services.AddTransient<IViewLocalizerExtended, JsonViewLocalizer>();

            return services;
        }
    }
}
