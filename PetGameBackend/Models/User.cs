using System.ComponentModel.DataAnnotations;

namespace PetGameBackend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // Navigation property - no default initialization to avoid recursion
        public PlayerData? PlayerData { get; set; }
    }
}
