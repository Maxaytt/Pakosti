namespace Pakosti.Application.Exceptions;

public class InternalServerException : ApplicationException
{
    public string? EntityName { get; }
    public object? EntityId { get; }

    public InternalServerException(string entityName, object entityId, string massage)
        : base(massage)
    {
        EntityName = entityName;
        EntityId = entityId;
    }
    
    public InternalServerException(string massage)
        : base(massage)
    {
        EntityName = null;
        EntityId = null;
    }
}