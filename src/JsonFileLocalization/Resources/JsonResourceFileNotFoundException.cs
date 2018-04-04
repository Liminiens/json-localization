using System.IO;

namespace JsonFileLocalization.Resources
{
    /// <summary>
    /// Exception for a situation when an invalid path for a resource was provided
    /// </summary>
    public class JsonResourceFileNotFoundException : FileNotFoundException
    {
        public JsonResourceFileNotFoundException(string message) : base(message)
        {
        }
    }
}