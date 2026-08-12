using MusicDistribution.DAL.Entities;

namespace MusicDistribution.DAL.Repositories
{
    public interface ITrackRepository
    {
        Task<Track> AddAsync(Track track);
        Task<List<Track>> GetAllAsync(int? artistId = null, string? genre = null, object? status = null);
        Task<Track?> GetByIdAsync(int id);
        Task UpdateAsync(Track track);
    }
}
