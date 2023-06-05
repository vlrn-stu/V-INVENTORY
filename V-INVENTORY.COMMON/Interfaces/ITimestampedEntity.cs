namespace V_INVENTORY.MODEL.Interfaces
{
    public interface ITimestampedEntity
    {
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset UpdatedAt { get; set; }
    }
}