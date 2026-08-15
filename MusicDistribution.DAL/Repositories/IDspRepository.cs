using MusicDistribution.DAL.Entities;

namespace MusicDistribution.DAL.Repositories
{
    public interface IDspRepository
    {
        Task<List<Dsp>> GetAllAsync();
        Task<Dsp?> GetByIdAsync(int id);
        Task AddAsync(Dsp dsp);
    }
}
