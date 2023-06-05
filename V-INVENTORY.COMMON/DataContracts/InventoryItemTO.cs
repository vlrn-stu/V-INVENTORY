using System.Runtime.Serialization;

namespace V_INVENTORY.MODEL.DataContracts
{
    [DataContract]
    public class InventoryItemTO
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string? Name { get; set; }

        [DataMember]
        public string? Description { get; set; }

        [DataMember]
        public Guid LocationId { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public decimal OriginalPrice { get; set; }

        [DataMember]
        public DateTime BuyDate { get; set; }
    }
}