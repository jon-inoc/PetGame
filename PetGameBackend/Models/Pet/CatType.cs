using System.ComponentModel.DataAnnotations;

namespace PetGameBackend.Models
{
    public class CatType
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Rarity { get; set; } = "Common";

        public string Personality { get; set; } = string.Empty;

        public string PassiveEffect { get; set; } = "{}";
    }
}
