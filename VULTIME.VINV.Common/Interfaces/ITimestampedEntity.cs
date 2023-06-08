namespace VULTIME.VINV.Common.Interfaces
{
    public interface ITimestampedEntity
    {
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset UpdatedAt { get; set; }
    }
}