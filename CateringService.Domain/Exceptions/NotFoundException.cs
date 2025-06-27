namespace CateringService.Domain.Exceptions;

/// <summary>
/// Exception that is thrown when an entity with the specified type and identifier is not found.
/// </summary>
public class NotFoundException : Exception
{
    public string Type { get; }
    public string Id { get; }
    public override string Message => $"Entity {Type} with {Id} wasn't found.";

    public NotFoundException(string type, string id)
    {
        Type = type;
        Id = id;
    }
}