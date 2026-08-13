using MusicDistribution.BLL.DTOs;

namespace MusicDistribution.BLL.Services
{
    public interface ITrackService
    {
        Task<TrackResponse> CreateAsync(CreateTrackRequest request);
        Task<List<TrackResponse>> GetFilteredAsync(TrackFilterRequest filter);
        Task<TrackDetailResponse> GetByIdAsync(int id);
        Task UpdateStatusAsync(int trackId, UpdateTrackStatusRequest request);

    }
}