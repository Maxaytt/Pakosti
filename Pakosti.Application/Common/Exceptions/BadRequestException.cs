namespace Pakosti.Application.Common.Exceptions;

public class BadRequestException : Exception
{
    public string Description { get; }

    public BadRequestException(string description)
        : base(description)
    {
        Description = description;
    }
}