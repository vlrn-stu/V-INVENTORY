using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace V_INVENTORY.MODEL.DataContracts
{
    [DataContract]
    public class InventoryItemTO
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name is too long.")]
        public string? Name { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Description is required.")]
        public string? Description { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Location is required.")]
        public Guid LocationId { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity should be greater than 0")]
        public int Quantity { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Original Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price should be greater than 0")]
        public decimal OriginalPrice { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Buy Date is required.")]
        public DateOnly BuyDate { get; set; }
    }
}