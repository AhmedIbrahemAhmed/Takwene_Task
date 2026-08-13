using MusicDistribution.DAL.Entities;

namespace MusicDistribution.DAL.Repositories
{
    public interface ITrackDistributionRepository
    {
        Task<TrackDistribution> AddAsync(TrackDistribution distribution);
        Task<List<TrackDistribution>> GetByTrackIdAsync(int trackId);
        Task<bool> ExistsAsync(int trackId, int dspId);
    }
}
