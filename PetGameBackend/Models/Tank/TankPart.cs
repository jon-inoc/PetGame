using System.ComponentModel.DataAnnotations;

namespace PetGameBackend.Models
{
    public class TankPart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PartType { get; set; } = "Head";

        [Required]
        public string Name { get; set; }

        public int Damage { get; set; } = 0;
        public int Armor { get; set; } = 0;
        public int Speed { get; set; } = 0;
    }
}
