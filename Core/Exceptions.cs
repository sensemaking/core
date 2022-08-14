using System.Collections.Generic;
using System.Linq;

namespace System
{
    public interface IListExceptionErrors 
    {
        string[] Errors { get; }
    }

    public class ValidationException : Exception, IListExceptionErrors
    {
        public ValidationException(params string[] errors) : base("A validation error has occured") { Errors = errors; }
        public string[] Errors { get; }
    }

    public class ConflictException : Exception, IListExceptionErrors
    {
        public ConflictException(params string[] errors) : base("A conflict has occured") { Errors = errors; }
        public string[] Errors { get; }
    }

    public class LegalException : Exception, IListExceptionErrors
    {
        public LegalException(params string[] errors) : base("A legal exception has occured") { Errors = errors; }
        public string[] Errors { get; }
    }

    public class NotFoundException : Exception { public NotFoundException() : base() { } }

    public static class Validation
    {
        public static void BasedOn(Action<IList<string>> validator)
        {
            var errors = new List<string>();
            validator(errors);

            if (errors.Any())
                throw new ValidationException(errors.ToArray());
        }
    }
}