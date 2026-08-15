namespace MusicDistribution.BLL.DTOs
{
    public class CreateDspRequest
    {
        public string Name { get; set; } = null!;
    }

    public class DspResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
