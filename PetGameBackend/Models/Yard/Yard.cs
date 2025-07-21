using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGameBackend.Models
{
    public class Yard
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PlayerId { get; set; }

        public int Level { get; set; } = 1;

        public string Style { get; set; } = "Grass";

        public int DecorationScore { get; set; } = 0;

        [ForeignKey("PlayerId")]
        public PlayerData? Player { get; set; }
    }
}
