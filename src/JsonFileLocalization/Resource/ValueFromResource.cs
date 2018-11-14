namespace JsonFileLocalization.Resource
{
    /// <summary>
    /// Value from a resource
    /// </summary>
    /// <typeparam name="TValue">Value type</typeparam>
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

        public ValueFromResource(TValue value, bool parseSuccess)
        {
            Value = value;
            ParseSuccess = parseSuccess;
        }
    }
}