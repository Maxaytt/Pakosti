namespace Pakosti.Application.Exceptions;

public class ConflictException : ApplicationException
{
    public string EntityName { get; }
    public object EntityId { get; }

    public ConflictException(string entityName, object entityId)
        : base($"Entity \"{entityName}\" ({entityId}) calls conflict.")
    {
        EntityName = entityName;
        EntityId = entityId;
    }
}