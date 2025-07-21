using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGameBackend.Models
{
    public class Toy
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int YardId { get; set; }

        [Required]
        public int ToyTypeId { get; set; }

        public float PositionX { get; set; }

        public float PositionY { get; set; }

        [ForeignKey("YardId")]
        public Yard? Yard { get; set; }

        [ForeignKey("ToyTypeId")]
        public ToyType? ToyType { get; set; }
    }
}
