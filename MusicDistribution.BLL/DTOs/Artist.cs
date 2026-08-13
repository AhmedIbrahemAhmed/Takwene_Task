using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDistribution.BLL.DTOs
{
    public class CreateArtistRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
    }

    public class ArtistResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
    }
}
