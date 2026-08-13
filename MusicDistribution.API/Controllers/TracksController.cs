using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicDistribution.BLL.DTOs;
using MusicDistribution.BLL.Exceptions;
using MusicDistribution.BLL.Services;

namespace MusicDistribution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TracksController : ControllerBase
    {
        private readonly ITrackService _trackService;
        private readonly IDistributionService _distributionService;

        public TracksController(ITrackService trackService, IDistributionService distributionService)
        {
            _trackService = trackService;
            _distributionService = distributionService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrackRequest request)
        {
            try
            {
                var result = await _trackService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (ValidationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (ConflictException ex) { return Conflict(new { message = ex.Message }); }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? artistId, [FromQuery] string? genre, [FromQuery] string? status)
        {
            var result = await _trackService.GetFilteredAsync(new TrackFilterRequest { ArtistId = artistId, Genre = genre, Status = status });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _trackService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
        }

        [Authorize]
        [HttpPost("{id}/distribute")]
        public async Task<IActionResult> Distribute(int id, DistributeTrackRequest request)
        {
            try
            {
                await _distributionService.DistributeAsync(id, request);
                return Ok(new { message = "Track submitted to selected DSPs." });
            }
            catch (ValidationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
        }

        [Authorize]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateTrackStatusRequest request)
        {
            try
            {
                await _trackService.UpdateStatusAsync(id, request);
                return Ok(new { message = "Status updated." });
            }
            catch (ValidationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
        }
    }
}
