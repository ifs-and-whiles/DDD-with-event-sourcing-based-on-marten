namespace Billy.CodeReadability
{
    public sealed class Index
    {
        private readonly int _index;

        public Index(int index)
        {
            _index = index;
        }
        
        public static implicit operator int(Index index) => index._index;
        public static implicit operator Index(int index) => new Index(index);

        private bool Equals(Index other)
        {
            return _index == other._index;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Index other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _index;
        }

        public override string ToString()
        {
            return _index.ToString();
        }
    }
}