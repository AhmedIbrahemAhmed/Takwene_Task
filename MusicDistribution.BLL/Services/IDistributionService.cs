using MusicDistribution.BLL.DTOs;

namespace MusicDistribution.BLL.Services
{
    public interface IDistributionService
    {
        Task DistributeAsync(int trackId, DistributeTrackRequest request);
    }
}