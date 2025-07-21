using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGameBackend.Models
{
    public class Tank
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PlayerId { get; set; }

        public int ArmorLevel { get; set; } = 1;

        public string WeaponType { get; set; } = "Cannon";

        public string SupportModules { get; set; } = "[]";

        public int? HeadPartId { get; set; }
        public int? BodyPartId { get; set; }
        public int? MobilityPartId { get; set; }

        [ForeignKey("HeadPartId")]
        public virtual TankPart? HeadPart { get; set; }

        [ForeignKey("BodyPartId")]
        public virtual TankPart? BodyPart { get; set; }

        [ForeignKey("MobilityPartId")]
        public virtual TankPart? MobilityPart { get; set; }

        [ForeignKey("PlayerId")]
        public PlayerData? Player { get; set; }
    }
}
