namespace ResilientOrderEngine.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTimeOffset.UtcNow;  
    }

    public void SetUpdatedAt()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}