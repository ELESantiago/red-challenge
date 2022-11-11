using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using REDChallenge.Domain.ErrorObjects;

namespace REDChallenge.Domain.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Failures = new List<InvalidParameter>();
        }

        public ValidationException(List<ValidationFailure> failures)
            : this()
        {
            var propertyNames = failures
                .OrderBy(f => f.PropertyName)
                .Select(e => new InvalidParameter
                {
                    Name = e.PropertyName,
                    Reason = e.ErrorMessage
                });
        }

        public IEnumerable<InvalidParameter> Failures { get; }
    }
}
