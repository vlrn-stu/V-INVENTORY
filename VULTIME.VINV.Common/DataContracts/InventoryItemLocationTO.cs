using System.Runtime.Serialization;

namespace VULTIME.VINV.Common.DataContracts
{
    [DataContract]
    public class InventoryItemLocationTO
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string? Name { get; set; }
    }
}