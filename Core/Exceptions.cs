using System.Collections.Generic;
using System.Linq;

namespace System
{
    public interface IListExceptionErrors
    {
        string[] Errors { get; }
    }

    public class Validation
    {
        public static void BasedOn(Action<IList<string>> validator)
        {
            var errors = new List<string>();
            validator(errors);

            if (errors.Any())
                throw new ValidationException(errors.ToArray());
        }
    }

    public class ValidationException : Exception, IListExceptionErrors
    {
        public ValidationException(params string[] errors) : base("A validation error has occured")
        {
            Errors = errors;
        }

        public string[] Errors { get; }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}