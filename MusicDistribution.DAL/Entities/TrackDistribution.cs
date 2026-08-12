using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MusicDistribution.DAL.Enums;

namespace MusicDistribution.DAL.Entities
{
    public class TrackDistribution
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Track))]
        public int TrackId { get; set; }
        public Track? Track { get; set; }

        [Required]
        [ForeignKey(nameof(Dsp))]
        public int DspId { get; set; }
        public Dsp? Dsp { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DistributionStatus Status { get; set; } = DistributionStatus.Pending;
    }
}
