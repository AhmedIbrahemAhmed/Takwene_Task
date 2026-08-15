using Microsoft.EntityFrameworkCore;
using MusicDistribution.DAL.Entities;

namespace MusicDistribution.DAL.Repositories
{
    public class DspRepository : IDspRepository
    {
        private readonly MusicDistributionDbContext _db;

        public DspRepository(MusicDistributionDbContext db)
        {
            _db = db;
        }

        public async Task<List<Dsp>> GetAllAsync()
        {
            return await _db.Dsps.ToListAsync();
        }

        public async Task<Dsp?> GetByIdAsync(int id)
        {
            return await _db.Dsps.FindAsync(id);
        }

        public async Task AddAsync(Dsp dsp)
        {
            await _db.Dsps.AddAsync(dsp);
            await _db.SaveChangesAsync();
        }
    }
}