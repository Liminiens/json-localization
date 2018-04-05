using System.Globalization;

namespace JsonFileLocalization.ObjectLocalization
{
    /// <summary>
    /// Represents a service that provides localized objects.
    /// </summary>
    public interface IObjectLocalizer
    {
        /// <summary>
        /// Returns localized object
        /// </summary>
        /// <typeparam name="TValue">Type of an object</typeparam>
        /// <param name="name">Object JPath in a resource</param>
        /// <returns>Returns a culture specific <see cref="LocalizedObject{TValue}"/></returns>
        LocalizedObject<TValue> GetLocalizedObject<TValue>(string name);

        /// <summary>
        /// Creates a new <see cref="IObjectLocalizer" /> for a specific <see cref="CultureInfo" />.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo" /> to use.</param>
        /// <returns>A culture-specific <see cref="IObjectLocalizer" />.</returns>
        IObjectLocalizer WithCulture(CultureInfo culture);
    }

}
