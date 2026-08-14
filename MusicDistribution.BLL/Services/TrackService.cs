using MusicDistribution.BLL.DTOs;
using MusicDistribution.BLL.Exceptions;
using MusicDistribution.DAL.Entities;
using MusicDistribution.DAL.Enums;
using MusicDistribution.DAL.Repositories;

namespace MusicDistribution.BLL.Services
{
    public class TrackService : ITrackService
    {
        private readonly ITrackRepository _trackRepository;
        private readonly IArtistRepository _artistRepository;

        public TrackService(ITrackRepository trackRepository, IArtistRepository artistRepository)
        {
            _trackRepository = trackRepository;
            _artistRepository = artistRepository;
        }

        public async Task<TrackResponse> CreateAsync(CreateTrackRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ValidationException("Title is required.");
            if (string.IsNullOrWhiteSpace(request.Isrc) || request.Isrc.Length != 12)
                throw new ValidationException("ISRC must be exactly 12 characters.");
            if (string.IsNullOrWhiteSpace(request.Genre))
                throw new ValidationException("Genre is required.");

            var artist = await _artistRepository.GetByIdAsync(request.ArtistId);
            if (artist is null)
                throw new NotFoundException($"Artist {request.ArtistId} not found.");

            if (await _trackRepository.ExistsByIsrcAsync(request.Isrc))
                throw new ConflictException($"A track with ISRC {request.Isrc} already exists.");

            var track = new Track
            {
                Title = request.Title,
                ArtistId = request.ArtistId,
                ISRC = request.Isrc,
                ReleaseDate = request.ReleaseDate,
                Genre = request.Genre,
                Status = TrackStatus.Draft
            };
            await _trackRepository.AddAsync(track);

            return MapToResponse(track, artist.Name);
        }

        public async Task<List<TrackResponse>> GetFilteredAsync(TrackFilterRequest filter)
        {
            TrackStatus? status = null;

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                if (Enum.TryParse<TrackStatus>(
                    filter.Status,
                    true,
                    out var parsedStatus))
                {
                    status = parsedStatus;
                }
            }

            var tracks = await _trackRepository.GetAllAsync(
                filter.ArtistId,
                filter.Genre,
                status
            );

            return tracks
                .Select(t => MapToResponse(t, t.Artist.Name))
                .ToList();
        }

        public async Task<TrackDetailResponse> GetByIdAsync(int id)
        {
            var track = await _trackRepository.GetByIdAsync(id);
            if (track is null)
                throw new NotFoundException($"Track {id} not found.");

            return new TrackDetailResponse
            {
                Id = track.Id,
                Title = track.Title,
                ArtistId = track.ArtistId,
                ArtistName = track.Artist.Name,
                Isrc = track.ISRC,
                ReleaseDate = track.ReleaseDate,
                Genre = track.Genre,
                Status = track.Status.ToString(),
                Distributions = track.Distributions.Select(d => new TrackDistributionResponse
                {
                    DspId = d.DspId,
                    DspName = d.Dsp.Name,
                    Status = d.Status.ToString(),
                    SubmittedAt = d.SubmittedAt
                }).ToList()
            };
        }

        private static TrackResponse MapToResponse(Track t, string artistName) => new()
        {
            Id = t.Id,
            Title = t.Title,
            ArtistId = t.ArtistId,
            ArtistName = artistName,
            Isrc = t.ISRC,
            ReleaseDate = t.ReleaseDate,
            Genre = t.Genre,
            Status = t.Status.ToString()
        };

        public async Task UpdateStatusAsync(int trackId, UpdateTrackStatusRequest request)
        {
            var track = await _trackRepository.GetByIdAsync(trackId);
            if (track is null)
                throw new NotFoundException($"Track {trackId} not found.");

            if (!Enum.TryParse<TrackStatus>(request.Status, true, out var newStatus))
                throw new ValidationException($"Invalid status: {request.Status}");

            track.Status = newStatus;
            await _trackRepository.UpdateAsync(track);
        }
    }
}
