using System.ComponentModel.DataAnnotations;

namespace PetGameBackend.Models
{
    public class ToyType
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string AttractsPersonality { get; set; } = string.Empty;

        public int HappinessBonus { get; set; } = 0;

        public string CraftingRecipe { get; set; } = "{}";
    }
}
