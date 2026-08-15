using MusicDistribution.BLL.DTOs;

namespace MusicDistribution.BLL.Services
{
    public interface IDspService
    {
        Task<DspResponse> CreateAsync(CreateDspRequest request);
        Task<List<DspResponse>> GetAllAsync();
    }
}
