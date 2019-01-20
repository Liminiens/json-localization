using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;

namespace JsonFileLocalization.Example.Localization
{
    public class LocalizedValidationAttributeAdapterProvider : ValidationAttributeAdapterProvider,
        IValidationAttributeAdapterProvider
    {
        public IAttributeAdapter GetAttributeAdapter(
            ValidationAttribute attribute,
            IStringLocalizer stringLocalizer)
        {
            stringLocalizer = stringLocalizer.WithCulture(CultureInfo.CurrentUICulture);
            return base.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }
}