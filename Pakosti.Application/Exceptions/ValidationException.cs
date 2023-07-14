using ApplicationException = Pakosti.Domain.Exceptions.ApplicationException;

namespace Pakosti.Application.Exceptions;

public class ValidationException : ApplicationException
{
    public ValidationException(IReadOnlyDictionary<string, string[]> errorsDictionary)
        : base("Validation failure", "One or more validation errors occurred.") =>
        ErrorsDictionary = errorsDictionary;

    public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; }
}