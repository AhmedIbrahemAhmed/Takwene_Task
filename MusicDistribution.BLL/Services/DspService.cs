using MusicDistribution.BLL.DTOs;
using MusicDistribution.BLL.Exceptions;
using MusicDistribution.DAL.Entities;
using MusicDistribution.DAL.Repositories;

namespace MusicDistribution.BLL.Services
{
    public class DspService : IDspService
    {
        private readonly IDspRepository _dspRepository;

        public DspService(IDspRepository dspRepository) => _dspRepository = dspRepository;

        public async Task<DspResponse> CreateAsync(CreateDspRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ValidationException("DSP name is required.");

            var dsp = new Dsp { Name = request.Name };
            await _dspRepository.AddAsync(dsp);

            return new DspResponse { Id = dsp.Id, Name = dsp.Name };
        }

        public async Task<List<DspResponse>> GetAllAsync()
        {
            var dsps = await _dspRepository.GetAllAsync();
            return dsps.Select(d => new DspResponse { Id = d.Id, Name = d.Name }).ToList();
        }
    }
}
