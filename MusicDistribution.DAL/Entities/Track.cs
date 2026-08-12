using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MusicDistribution.DAL.Enums;

namespace MusicDistribution.DAL.Entities
{
    public class Track
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Artist))]
        public int ArtistId { get; set; }

        public Artist? Artist { get; set; }

        [Required]
        [MaxLength(50)]
        public string ISRC { get; set; } = null!;

        [Required]
        public DateTime ReleaseDate { get; set; }

        [MaxLength(100)]
        public string? Genre { get; set; }

        [Required]
        public TrackStatus Status { get; set; } = TrackStatus.Draft;

        public ICollection<TrackDistribution>? Distributions { get; set; }
    }
}
