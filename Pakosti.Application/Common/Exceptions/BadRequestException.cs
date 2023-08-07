namespace Pakosti.Application.Common.Exceptions;

public class BadRequestException : Exception
{
    public string Description { get; }
    public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; set; }


    public BadRequestException(string description)
        : base(description)
    {
        Description = description;
    }
}