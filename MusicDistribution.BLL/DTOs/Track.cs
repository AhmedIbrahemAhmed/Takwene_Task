using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDistribution.BLL.DTOs
{
    public class CreateTrackRequest
    {
        public string Title { get; set; }
        public int ArtistId { get; set; }
        public string Isrc { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
    }

    public class TrackResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ArtistId { get; set; }
        public string ArtistName { get; set; } // flattened for list view
        public string Isrc { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public string Status { get; set; }
    }

    public class TrackDetailResponse : TrackResponse
    {
        public List<TrackDistributionResponse> Distributions { get; set; } = new();
    }

    public class TrackDistributionResponse
    {
        public int DspId { get; set; }
        public string DspName { get; set; }
        public string Status { get; set; }
        public DateTime SubmittedAt { get; set; }
    }

    public class TrackFilterRequest
    {
        public int? ArtistId { get; set; }
        public string? Genre { get; set; }
        public string? Status { get; set; }
    }

    public class DistributeTrackRequest
    {
        public List<int> DspIds { get; set; }
    }

    public class UpdateTrackStatusRequest
    {
        public string Status { get; set; }
    }
}
