using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using VULTIME.VINV.Common.Models;

namespace VULTIME.VINV.Common.DataContracts
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
        [Required(ErrorMessage = "Status is required.")]
        public InventoryItemStatus Status { get; set; } = InventoryItemStatus.Stored;

        [DataMember]
        [StringLength(500, ErrorMessage = "Description is too long.")]
        public string? Description { get; set; } = "";

        [DataMember]
        [Required(ErrorMessage = "Location is required.")]
        public Guid? LocationId { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity should be greater than 0")]
        public int Quantity { get; set; } = 1;

        [DataMember]
        [Required(ErrorMessage = "Original Price is required.")]
        [Range(0.00, double.MaxValue, ErrorMessage = "Price should be greater than or equal 0")]
        public decimal OriginalPrice { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Buy Date is required.")]
        public DateTimeOffset BuyDate { get; set; } = DateTimeOffset.UtcNow;
    }
}