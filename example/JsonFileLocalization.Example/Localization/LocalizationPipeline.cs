using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace JsonFileLocalization.Example.Localization
{
    public class LocalizationPipeline
    {
        public void Configure(IApplicationBuilder app, IOptions<RequestLocalizationOptions> options)
        {
            app.UseRequestLocalization(options.Value);
        }
    }
}