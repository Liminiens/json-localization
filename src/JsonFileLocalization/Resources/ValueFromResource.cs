namespace JsonFileLocalization.Resources
{
    /// <summary>
    /// Value from a resource
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public readonly struct ValueFromResource<TValue>
    {
        /// <summary>
        /// Value
        /// </summary>
        public readonly TValue Value;
        /// <summary>
        /// Is value parsed successfully
        /// </summary>
        public readonly bool ParseSuccess;

        public ValueFromResource(in TValue value, in bool parseSuccess)
        {
            Value = value;
            ParseSuccess = parseSuccess;
        }
    }
}