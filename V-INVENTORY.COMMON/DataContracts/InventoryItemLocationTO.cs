using System.Runtime.Serialization;

namespace V_INVENTORY.MODEL.DataContracts
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