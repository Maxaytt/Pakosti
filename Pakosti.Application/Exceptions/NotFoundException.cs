namespace Pakosti.Application.Exceptions;

public class NotFoundException : Exception
{
    public string EntityName { get; }
    public object EntityId { get; }

    public NotFoundException(string entityName, object entityId)
        : base($"Entity \"{entityName}\" ({entityId}) not found.")
    {
        EntityName = entityName;
        EntityId = entityId;
    }
}