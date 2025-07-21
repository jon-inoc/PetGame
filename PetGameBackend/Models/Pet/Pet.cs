using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGameBackend.Models
{
    public class Pet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PlayerId { get; set; }

        [Required]
        public int CatTypeId { get; set; }

        public string? Name { get; set; }

        public int BondLevel { get; set; } = 0;

        public bool IsOwned { get; set; } = true;

        public int Happiness { get; set; } = 50;

        [ForeignKey("PlayerId")]
        public PlayerData? Player { get; set; }

        [ForeignKey("CatTypeId")]
        public CatType? CatType { get; set; }
    }
}
