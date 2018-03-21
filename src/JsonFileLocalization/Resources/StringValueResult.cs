namespace JsonFileLocalization.Resources
{
    /// <summary>
    /// String resource from a file
    /// </summary>
    public class StringValueResult
    {
        /// <summary>
        /// Path in a resource
        /// </summary>
        public readonly string Path;

        /// <summary>
        /// Name of a string
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Value of a string
        /// </summary>
        public readonly string Value;

        public StringValueResult(string path, string name, string value)
        {
            Path = path;
            Name = name;
            Value = value;
        }
    }
}