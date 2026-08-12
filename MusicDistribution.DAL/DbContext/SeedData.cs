using Microsoft.EntityFrameworkCore;
using MusicDistribution.DAL.Entities;
using MusicDistribution.DAL.Enums;

namespace MusicDistribution.DAL
{
    public static class SeedData
    {
        public static async Task InitializeAsync(MusicDistributionDbContext db)
        {
            if (db == null) return;

            // ensure database
            await db.Database.MigrateAsync();

            // Insert artists if missing (do not set identity values explicitly)
            if (!await db.Artists.AnyAsync())
            {
                db.Artists.AddRange(
                    new Artist { Name = "Alice Records", Email = "contact@alicerecords.com", Country = "USA" },
                    new Artist { Name = "Blue Wave", Email = "info@bluewave.co", Country = "UK" },
                    new Artist { Name = "Crescent Beats", Email = "hello@crescentbeats.io", Country = "Canada" }
                );
                await db.SaveChangesAsync();
            }

            // Insert dsps if missing
            if (!await db.Dsps.AnyAsync())
            {
                db.Dsps.AddRange(
                    new Dsp { Name = "Spotify" },
                    new Dsp { Name = "Apple Music" },
                    new Dsp { Name = "YouTube" }
                );
                await db.SaveChangesAsync();
            }

            // Insert tracks if missing. Use existing artists to set ArtistId.
            if (!await db.Tracks.AnyAsync())
            {
                var alice = await db.Artists.FirstAsync(a => a.Name == "Alice Records");
                var blue = await db.Artists.FirstAsync(a => a.Name == "Blue Wave");
                var crescent = await db.Artists.FirstAsync(a => a.Name == "Crescent Beats");

                db.Tracks.AddRange(
                    new Track { Title = "Sunrise", ArtistId = alice.Id, ISRC = "US-ALC-21-00001", ReleaseDate = new DateTime(2023, 1, 10), Genre = "Pop", Status = TrackStatus.Draft },
                    new Track { Title = "Moonlight", ArtistId = alice.Id, ISRC = "US-ALC-21-00002", ReleaseDate = new DateTime(2023, 3, 5), Genre = "Jazz", Status = TrackStatus.Submitted },
                    new Track { Title = "Ocean Drive", ArtistId = blue.Id, ISRC = "GB-BLW-22-00001", ReleaseDate = new DateTime(2022, 6, 20), Genre = "Electronic", Status = TrackStatus.Distributed },
                    new Track { Title = "Northern Lights", ArtistId = blue.Id, ISRC = "GB-BLW-22-00002", ReleaseDate = new DateTime(2022, 9, 1), Genre = "Indie", Status = TrackStatus.Submitted },
                    new Track { Title = "Desert Rose", ArtistId = crescent.Id, ISRC = "CA-CRB-23-00001", ReleaseDate = new DateTime(2023, 5, 18), Genre = "World", Status = TrackStatus.Draft },
                    new Track { Title = "City Lights", ArtistId = crescent.Id, ISRC = "CA-CRB-23-00002", ReleaseDate = new DateTime(2023, 7, 30), Genre = "Pop", Status = TrackStatus.Submitted },
                    new Track { Title = "Starlight", ArtistId = alice.Id, ISRC = "US-ALC-21-00003", ReleaseDate = new DateTime(2021, 11, 11), Genre = "Pop", Status = TrackStatus.Distributed },
                    new Track { Title = "Rainfall", ArtistId = blue.Id, ISRC = "GB-BLW-22-00003", ReleaseDate = new DateTime(2020, 2, 14), Genre = "Folk", Status = TrackStatus.Draft }
                );
                await db.SaveChangesAsync();
            }

            // Insert track distributions if missing. Use existing tracks and dsps to set FK values.
            if (!await db.TrackDistributions.AnyAsync())
            {
                var spotify = await db.Dsps.FirstAsync(d => d.Name == "Spotify");
                var apple = await db.Dsps.FirstAsync(d => d.Name == "Apple Music");
                var youtube = await db.Dsps.FirstAsync(d => d.Name == "YouTube");

                var oceanDrive = await db.Tracks.FirstAsync(t => t.Title == "Ocean Drive");
                var starlight = await db.Tracks.FirstAsync(t => t.Title == "Starlight");
                var moonlight = await db.Tracks.FirstAsync(t => t.Title == "Moonlight");

                db.TrackDistributions.AddRange(
                    new TrackDistribution { TrackId = oceanDrive.Id, DspId = spotify.Id, SubmittedAt = new DateTime(2022, 6, 21), Status = DistributionStatus.Live },
                    new TrackDistribution { TrackId = starlight.Id, DspId = apple.Id, SubmittedAt = new DateTime(2021, 11, 12), Status = DistributionStatus.Live },
                    new TrackDistribution { TrackId = moonlight.Id, DspId = youtube.Id, SubmittedAt = new DateTime(2023, 3, 6), Status = DistributionStatus.Pending }
                );
                await db.SaveChangesAsync();
            }
        }
    }
}
