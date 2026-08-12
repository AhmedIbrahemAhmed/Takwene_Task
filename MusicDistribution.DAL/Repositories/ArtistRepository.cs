using Microsoft.EntityFrameworkCore;
using MusicDistribution.DAL.Entities;

namespace MusicDistribution.DAL.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly MusicDistributionDbContext _db;

        public ArtistRepository(MusicDistributionDbContext db)
        {
            _db = db;
        }

        public async Task<Artist> AddAsync(Artist artist)
        {
            _db.Artists.Add(artist);
            await _db.SaveChangesAsync();
            return artist;
        }

        public async Task<List<Artist>> GetAllAsync()
        {
            return await _db.Artists.ToListAsync();
        }

        public async Task<Artist?> GetByIdAsync(int id)
        {
            return await _db.Artists.FindAsync(id);
        }
    }
}
