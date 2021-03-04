namespace Billy.CodeReadability
{
    public sealed class FindResult<T>
    {
        public FindResult(Index index, T value)
        {
            Index = index;
            Value = value;
        }

        public Index Index { get; }
        public T Value { get; }
    }
}