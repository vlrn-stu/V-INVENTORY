using System.ComponentModel.DataAnnotations;
using VCS.V_INVENTORY.MODEL.Interfaces;

namespace VCS.V_INVENTORY.MODEL.Models
{
    public class ItemLocation : IItemLocation
    {
        [Key]
        public Guid Id { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required]
        public string Name { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}