namespace AvalonMessageBox
{
    internal struct Optional<T>
    {
        public Optional(T value)
        {
            Value = value;
            HasValue = true;
        }

        public static Optional<T> None { get; } = new Optional<T>();
        
        public T Value { get; }
        
        public bool HasValue { get; }
    }
}