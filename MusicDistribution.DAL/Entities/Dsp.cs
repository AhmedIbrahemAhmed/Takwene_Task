using System.ComponentModel.DataAnnotations;

namespace MusicDistribution.DAL.Entities
{
    public class Dsp
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        public ICollection<TrackDistribution>? TrackDistributions { get; set; }
    }
}
