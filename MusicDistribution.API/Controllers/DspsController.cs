using Microsoft.AspNetCore.Mvc;
using MusicDistribution.BLL.DTOs;
using MusicDistribution.BLL.Exceptions;
using MusicDistribution.BLL.Services;

namespace MusicDistribution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DspsController : ControllerBase
    {
        private readonly IDspService _dspService;

        public DspsController(IDspService dspService) => _dspService = dspService;

        [HttpPost]
        public async Task<IActionResult> Create(CreateDspRequest request)
        {
            try
            {
                var result = await _dspService.CreateAsync(request);
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
            var result = await _dspService.GetAllAsync();
            return Ok(result);
        }
    }
}
