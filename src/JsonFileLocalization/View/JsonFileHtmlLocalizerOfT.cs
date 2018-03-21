using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace JsonFileLocalization.View
{
    public class JsonFileHtmlLocalizer<TResource> : IHtmlLocalizer<TResource>
    {
        private readonly IHtmlLocalizer _localizer;

        public JsonFileHtmlLocalizer(in IHtmlLocalizerFactory factory)
        {
            _localizer = factory.Create(typeof(TResource)) ?? throw new ArgumentException(nameof(factory));
        }

        /// <inheritdoc />
        public LocalizedString GetString(string name)
            => _localizer.GetString(name);

        /// <inheritdoc />
        public LocalizedString GetString(string name, params object[] arguments)
            => _localizer.GetString(name, arguments);

        /// <inheritdoc />
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
            => _localizer.GetAllStrings(includeParentCultures);

        /// <inheritdoc />
        public IHtmlLocalizer WithCulture(CultureInfo culture)
            => _localizer.WithCulture(culture);

        /// <inheritdoc />
        public LocalizedHtmlString this[string name]
            => _localizer[name];

        /// <inheritdoc />
        public LocalizedHtmlString this[string name, params object[] arguments]
            => _localizer[name, arguments];
    }
}