namespace CateringService.Domain.Exceptions;

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