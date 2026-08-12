using Microsoft.EntityFrameworkCore;
using MusicDistribution.DAL.Entities;

namespace MusicDistribution.DAL.Repositories
{
    public class TrackDistributionRepository : ITrackDistributionRepository
    {
        private readonly MusicDistributionDbContext _db;

        public TrackDistributionRepository(MusicDistributionDbContext db)
        {
            _db = db;
        }

        public async Task<TrackDistribution> AddAsync(TrackDistribution distribution)
        {
            _db.TrackDistributions.Add(distribution);
            await _db.SaveChangesAsync();
            return distribution;
        }

        public async Task<List<TrackDistribution>> GetByTrackIdAsync(int trackId)
        {
            return await _db.TrackDistributions
                .Include(td => td.Dsp)
                .Where(td => td.TrackId == trackId)
                .ToListAsync();
        }
    }
}
