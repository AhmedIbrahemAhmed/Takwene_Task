using MusicDistribution.DAL.Entities;

namespace MusicDistribution.DAL.Repositories
{
    public interface IArtistRepository
    {
        Task<Artist> AddAsync(Artist artist);
        Task<List<Artist>> GetAllAsync();
        Task<Artist?> GetByIdAsync(int id);
    }
}
