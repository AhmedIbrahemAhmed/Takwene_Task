using Microsoft.AspNetCore.Mvc;
using MusicDistribution.BLL.DTOs;
using MusicDistribution.BLL.Exceptions;
using MusicDistribution.BLL.Services;

namespace MusicDistribution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistsController(IArtistService artistService) => _artistService = artistService;

        [HttpPost]
        public async Task<IActionResult> Create(CreateArtistRequest request)
        {
            try
            {
                var result = await _artistService.CreateAsync(request);
                return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _artistService.GetAllAsync();
            return Ok(result);
        }
    }
}
