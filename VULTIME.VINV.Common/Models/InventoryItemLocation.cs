using System.ComponentModel.DataAnnotations;
using VULTIME.VINV.Common.Interfaces;

namespace VULTIME.VINV.Common.Models
{
    public class InventoryItemLocation : IInventoryItemLocation
    {
        [Key]
        public Guid Id { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required]
        public string Name { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}