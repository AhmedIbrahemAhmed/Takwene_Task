using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDistribution.BLL.DTOs
{
    public class CreateArtistRequest
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Country { get; set; }
    }

    public class ArtistResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Country { get; set; }
    }
}
