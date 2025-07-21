using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGameBackend.Models
{
    public class PlayerData
    {
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public int Level { get; set; } = 1;
        public int Coins { get; set; } = 100;

        public string PlayerJsonData { get; set; } = "{}";

        public User? User { get; set; }
    }
}
