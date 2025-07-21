using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGameBackend.Models
{
    public class DungeonRun
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PlayerId { get; set; }

        public string Biome { get; set; } = "Forest";

        public string LootGained { get; set; } = "{}";

        public bool Completed { get; set; } = false;

        public DateTime RunTimestamp { get; set; } = DateTime.UtcNow;

        [ForeignKey("PlayerId")]
        public PlayerData? Player { get; set; }
    }
}
