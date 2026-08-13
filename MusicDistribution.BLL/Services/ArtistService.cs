using MusicDistribution.BLL.DTOs;
using MusicDistribution.DAL.Entities;
using MusicDistribution.DAL.Repositories;
using System.ComponentModel.DataAnnotations;
namespace MusicDistribution.BLL.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;

        public ArtistService(IArtistRepository artistRepository) => _artistRepository = artistRepository;

        public async Task<ArtistResponse> CreateAsync(CreateArtistRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ValidationException("Artist name is required.");
            if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains('@'))
                throw new ValidationException("A valid email is required.");
            if (string.IsNullOrWhiteSpace(request.Country))
                throw new ValidationException("Country is required.");

            var artist = new Artist { Name = request.Name, Email = request.Email, Country = request.Country };
            await _artistRepository.AddAsync(artist);

            return new ArtistResponse { Id = artist.Id, Name = artist.Name, Email = artist.Email, Country = artist.Country };
        }

        public async Task<List<ArtistResponse>> GetAllAsync()
        {
            var artists = await _artistRepository.GetAllAsync();
            return artists.Select(a => new ArtistResponse { Id = a.Id, Name = a.Name, Email = a.Email, Country = a.Country }).ToList();
        }
    }
}
