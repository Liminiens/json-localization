using System.Globalization;

namespace JsonFileLocalization.ObjectLocalization
{
    public class JsonFileObjectLocalizer<TResource> : IObjectLocalizer
    {
        private readonly IObjectLocalizer _localizer;
        
        public JsonFileObjectLocalizer(JsonFileObjectLocalizerFactory factory)
        {
            _localizer = factory.Create(typeof(TResource));
        }

        /// <inheritdoc />
        public LocalizedObject<TValue> GetLocalizedObject<TValue>(string name)
        {
            return _localizer.GetLocalizedObject<TValue>(name);
        }

        /// <inheritdoc />
        public IObjectLocalizer WithCulture(CultureInfo culture)
        {
            return _localizer.WithCulture(culture);
        }
    }
}