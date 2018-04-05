using System.Globalization;

namespace JsonFileLocalization.Resource
{
    public interface IJsonFileResourceManager
    {
        /// <summary>
        /// Gets an already existing json resource or creates a new one if it doesnt exist yet
        /// </summary>
        /// <param name="baseName">name of a resource</param>
        /// <param name="location">assembly name or empty string</param>
        /// <param name="culture">resource culture</param>
        /// <returns>Json resource</returns>
        JsonFileResource GetResource(string baseName, string location, CultureInfo culture);
    }
}