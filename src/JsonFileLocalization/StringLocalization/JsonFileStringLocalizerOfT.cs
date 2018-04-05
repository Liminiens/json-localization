using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Localization;

namespace JsonFileLocalization.StringLocalization
{
    public class JsonFileStringLocalizer<TResource> : IStringLocalizer<TResource>
    {
        private readonly IStringLocalizer _localizer;

        /// <inheritdoc />
        public JsonFileStringLocalizer(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create(typeof(TResource));
        }

        /// <inheritdoc />
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _localizer.GetAllStrings(includeParentCultures);
        }

        /// <inheritdoc />
        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return _localizer.WithCulture(culture);
        }

        /// <inheritdoc />
        public LocalizedString this[string name]
            => _localizer[name];

        /// <inheritdoc />
        public LocalizedString this[string name, params object[] arguments]
            => _localizer[name, arguments];
    }
}
