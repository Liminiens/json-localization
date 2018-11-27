using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace JsonFileLocalization.ViewLocalization
{
    public class JsonFileHtmlLocalizer : IHtmlLocalizer
    {
        private readonly IStringLocalizer _localizer;

        public JsonFileHtmlLocalizer(IStringLocalizer localizer)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// Creates a new <see cref="LocalizedHtmlString"/> for a <see cref="LocalizedString"/>.
        /// </summary>
        /// <param name="result">The <see cref="LocalizedString"/>.</param>
        protected virtual LocalizedHtmlString ToHtmlString(LocalizedString result) =>
            new LocalizedHtmlString(result.Name, result.Value, result.ResourceNotFound);

        /// <summary>
        /// Creates a new <see cref="LocalizedHtmlString"/> for a <see cref="LocalizedString"/>.
        /// </summary>
        /// <param name="result">The <see cref="LocalizedString"/>.</param>
        /// <param name="arguments">Format arguments</param>
        protected virtual LocalizedHtmlString ToHtmlString(LocalizedString result, object[] arguments) =>
            new LocalizedHtmlString(result.Name, result.Value, result.ResourceNotFound, arguments);

        /// <inheritdoc />
        public LocalizedString GetString(string name)
        {
            return _localizer.GetString(name);
        }

        /// <inheritdoc />
        public LocalizedString GetString(string name, params object[] arguments)
        {
            return _localizer[name, arguments];
        }

        /// <inheritdoc />
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _localizer.GetAllStrings(includeParentCultures);
        }

        /// <inheritdoc />
        public IHtmlLocalizer WithCulture(CultureInfo culture)
        {
            var stringLocalizer = _localizer.WithCulture(culture);
            if (stringLocalizer != null)
            {
                return new JsonFileHtmlLocalizer(stringLocalizer);
            }

            return null;
        }

        /// <inheritdoc />
        public LocalizedHtmlString this[string name]
            => ToHtmlString(_localizer[name]);

        /// <inheritdoc />
        public LocalizedHtmlString this[string name, params object[] arguments]
            => ToHtmlString(_localizer[name, arguments], arguments);
    }
}