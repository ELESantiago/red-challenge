using System;

namespace REDChallenge.Domain.Exceptions
{
    public class InvalidLoginAttemptException : Exception
    {
        public int StatusCode { get; } = 401;
        public InvalidLoginAttemptException() : base()
        {
        }

        public InvalidLoginAttemptException(string message) : base(message)
        {
        }

        public InvalidLoginAttemptException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
