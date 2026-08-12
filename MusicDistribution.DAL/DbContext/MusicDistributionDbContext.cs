using Microsoft.EntityFrameworkCore;
using MusicDistribution.DAL.Entities;
using MusicDistribution.DAL.Enums;

namespace MusicDistribution.DAL
{
    public class MusicDistributionDbContext : DbContext
    {
        public MusicDistributionDbContext(DbContextOptions<MusicDistributionDbContext> options) : base(options)
        {
        }

        public DbSet<Artist> Artists { get; set; } = null!;
        public DbSet<Track> Tracks { get; set; } = null!;
        public DbSet<Dsp> Dsps { get; set; } = null!;
        public DbSet<TrackDistribution> TrackDistributions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Track>()
                .HasIndex(t => t.ISRC)
                .IsUnique();

            modelBuilder.Entity<Artist>()
                .HasMany(a => a.Tracks)
                .WithOne(t => t.Artist)
                .HasForeignKey(t => t.ArtistId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Dsp>()
                .HasMany(d => d.TrackDistributions)
                .WithOne(td => td.Dsp)
                .HasForeignKey(td => td.DspId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Track>()
                .HasMany(t => t.Distributions)
                .WithOne(td => td.Track)
                .HasForeignKey(td => td.TrackId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure enum properties to be stored as strings in the database
            modelBuilder.Entity<Track>()
                .Property(t => t.Status)
                .HasConversion<string>();

            // Configure enum properties to be stored as strings in the database
            modelBuilder.Entity<TrackDistribution>()
                .Property(td => td.Status)
                .HasConversion<string>();
        }
    }
}
