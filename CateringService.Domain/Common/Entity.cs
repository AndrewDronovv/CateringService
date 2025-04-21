namespace CateringService.Domain.Common;

public abstract class Entity<TPrimaryKey>
{
    public TPrimaryKey Id { get; set; }
}

public abstract class Entity : Entity<int>
{
}

public abstract class UlidEntity : Entity<Ulid>
{
    protected UlidEntity()
    {
        Id = Ulid.NewUlid();
    }
}