using System.IO;

namespace JsonFileLocalization.Resources
{
    public class JsonResourceFileNotFoundException : FileNotFoundException
    {
        public JsonResourceFileNotFoundException(string message) : base(message)
        {
        }
    }
}