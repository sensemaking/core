namespace System.Serialization
{
    public class SerializationException : Exception, IListExceptionErrors
    {
        public SerializationException(params string[] errors) : base("A serialization error has occured")
        {
            Errors = errors;
        }

        public string[] Errors { get; }
    }
}