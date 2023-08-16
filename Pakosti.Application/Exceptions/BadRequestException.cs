namespace Pakosti.Application.Exceptions;

public class BadRequestException : Exception
{
    public string Description { get; }

    public BadRequestException(string description)
        : base(description)
    {
        Description = description;
    }
}