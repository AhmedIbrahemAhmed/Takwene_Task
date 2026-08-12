using Microsoft.EntityFrameworkCore;
using MusicDistribution.DAL.Entities;
using MusicDistribution.DAL.Enums;

namespace MusicDistribution.DAL.Repositories
{
    public class TrackRepository : ITrackRepository
    {
        private readonly MusicDistributionDbContext _db;

        public TrackRepository(MusicDistributionDbContext db)
        {
            _db = db;
        }

        public async Task<Track> AddAsync(Track track)
        {
            _db.Tracks.Add(track);
            await _db.SaveChangesAsync();
            return track;
        }

        public async Task<List<Track>> GetAllAsync(int? artistId = null, string? genre = null, object? status = null)
        {
            var q = _db.Tracks.Include(t => t.Artist).AsQueryable();

            if (artistId.HasValue)
                q = q.Where(t => t.ArtistId == artistId.Value);

            if (!string.IsNullOrWhiteSpace(genre))
                q = q.Where(t => t.Genre == genre);

            if (status is TrackStatus s)
                q = q.Where(t => t.Status == s);
            else if (status is int si)
                q = q.Where(t => (int)t.Status == si);

            return await q.ToListAsync();
        }

        public async Task<Track?> GetByIdAsync(int id)
        {
            return await _db.Tracks
                .Include(t => t.Artist)
                .Include(t => t.Distributions!)
                .ThenInclude(d => d.Dsp)
                .FirstOrDefaultAsync(t => t.Id == id);
            // Formatting change for consistency
        }

        public async Task UpdateAsync(Track track)
        {
            _db.Tracks.Update(track);
            await _db.SaveChangesAsync();
        }
    }
}
