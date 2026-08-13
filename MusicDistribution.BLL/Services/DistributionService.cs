using MusicDistribution.BLL.DTOs;
using MusicDistribution.BLL.Exceptions;
using MusicDistribution.DAL.Entities;
using MusicDistribution.DAL.Enums;
using MusicDistribution.DAL.Repositories;


namespace MusicDistribution.BLL.Services
{
    public class DistributionService : IDistributionService
    {
        private readonly ITrackRepository _trackRepository;
        private readonly IDspRepository _dspRepository;
        private readonly ITrackDistributionRepository _distributionRepository;

        public DistributionService(
            ITrackRepository trackRepository,
            IDspRepository dspRepository,
            ITrackDistributionRepository distributionRepository)
        {
            _trackRepository = trackRepository;
            _dspRepository = dspRepository;
            _distributionRepository = distributionRepository;
        }

        public async Task DistributeAsync(int trackId, DistributeTrackRequest request)
        {
            var track = await _trackRepository.GetByIdAsync(trackId);
            if (track is null)
                throw new NotFoundException($"Track {trackId} not found.");

            if (track.Status == TrackStatus.Draft)
                throw new ValidationException("Cannot distribute a track that is still in Draft status.");

            if (request.DspIds is null || !request.DspIds.Any())
                throw new ValidationException("At least one DSP must be specified.");

            foreach (var dspId in request.DspIds)
            {
                var dsp = await _dspRepository.GetByIdAsync(dspId);
                if (dsp is null)
                    throw new NotFoundException($"DSP {dspId} not found.");

                var alreadyExists = await _distributionRepository.ExistsAsync(trackId, dspId);
                if (alreadyExists)
                    continue; // skip duplicates rather than error

                await _distributionRepository.AddAsync(new TrackDistribution
                {
                    TrackId = trackId,
                    DspId = dspId,
                    SubmittedAt = DateTime.UtcNow,
                    Status = DistributionStatus.Pending
                });
            }

            track.Status = TrackStatus.Submitted;
            await _trackRepository.UpdateAsync(track);
        }
    }
}
