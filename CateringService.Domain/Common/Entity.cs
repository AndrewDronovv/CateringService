namespace CateringService.Domain.Common;

public abstract class Entity<TPrimaryKey>
{
    public required TPrimaryKey Id { get; set; }
}

public abstract class UlidEntity : Entity<Ulid>
{
    protected UlidEntity()
    {
        Id = Ulid.NewUlid();
    }
}