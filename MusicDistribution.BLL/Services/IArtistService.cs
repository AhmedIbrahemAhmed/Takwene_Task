using MusicDistribution.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDistribution.BLL.Services
{
    public interface IArtistService
    {
        Task<ArtistResponse> CreateAsync(CreateArtistRequest request);
        Task<List<ArtistResponse>> GetAllAsync();
    }
}
