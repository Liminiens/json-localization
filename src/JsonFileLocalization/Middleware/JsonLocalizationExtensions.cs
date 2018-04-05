using JsonFileLocalization.Caching;
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
        public static IServiceCollection AddJsonFileLocalization(
            this IServiceCollection services, JsonLocalizationOptions options)
        {
            services.AddSingleton<IJsonFileLocalizationSettings, JsonFileLocalizationSettings>(provider =>
                new JsonFileLocalizationSettings(
                    provider.GetService<IHostingEnvironment>(),
                    options.CultureSuffixStrategy,
                    options.ResourceRelativePath));
            services.AddSingleton<IJsonFileResourceManager, JsonFileResourceManager>();

            services.AddTransient<IJsonFileContentCache, JsonFileContentCache>();
            services.AddTransient<IStringLocalizerFactory, JsonFileStringLocalizerFactory>();
            services.AddTransient(typeof(IStringLocalizer<>), typeof(JsonFileStringLocalizer<>));

            return services;
        }

        public static IServiceCollection AddJsonFileViewLocalication(this IServiceCollection services)
        {
            services.AddTransient<IViewLocalizer, JsonViewLocalizer>();
            services.AddTransient<JsonViewLocalizer>();

            services.AddTransient<IHtmlLocalizerFactory, JsonFileHtmlLocalizerFactory>();
            services.AddTransient(typeof(IHtmlLocalizer<>), typeof(JsonFileHtmlLocalizer<>));
            return services;
        }
    }
}
