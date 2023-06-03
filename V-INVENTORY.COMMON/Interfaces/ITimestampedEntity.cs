namespace VCS.V_INVENTORY.MODEL.Interfaces
{
    public interface ITimestampedEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}