using Common.MessagingService;
using Common.MessagingService.QueuesConfig;
using Domain.Contact.Entity;
using Domain.Region.Entity;
using Domain.Region.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TechChallenge.Controllers.Regions.Dto;
using TechChallenge.Helpers.Mappers;

namespace TechChallenge.Controllers.Regions.Http
{
    [Route("api/[controller]")]
    [SwaggerTag("Endpoints to manage regions")]
    [Produces("application/json")]
    public class RegionsController : Controller
    {
        private readonly IRegionService _regionService;
        private readonly IRabbitMqService _rabbitMqService;

        public RegionsController(
            IRegionService regionService,
            IRabbitMqService rabbitMqService)
        {
            _regionService = regionService;
            _rabbitMqService = rabbitMqService;
        }

        /// <summary>
        /// Add a new region
        /// </summary>
        /// <param name="dto">Region DTO</param>
        /// <response code="201">Region created with success</response>
        /// <response code="400">Bad Request</response>
        [HttpPost("")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddRegion([FromBody] RegionPostDto dto)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);

            var region = await _regionService.GetByDDD(dto.DDD);

            if (region != null)
                return StatusCode(StatusCodes.Status400BadRequest, "region already exist");

            var regionEntity = RegionMapper.MapRegionPost(dto);

            var wasSent = await _rabbitMqService.SendMessage(QueueNames.RegionInsert, QueueNames.RegionInsert, regionEntity);

            if (!wasSent)
                return StatusCode(StatusCodes.Status400BadRequest, "error on sent insert");

            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        /// Get all regions
        /// </summary>
        /// <response code="200">All Regions</response>
        /// <response code="400">Bad Request</response>
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<RegionEntity>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetContactsByRegion()
        {
            var region = await _regionService.GetAllRegions();

            return StatusCode(StatusCodes.Status200OK, region);
        }

        /// <summary>
        /// Get region by Id
        /// </summary>
        /// <response code="200">All Regions</response>
        /// <response code="400">Bad Request</response>
        [HttpGet("id/{id}")]
        [ProducesResponseType(typeof(List<RegionEntity>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            var region = await _regionService.GetById(id);
            if (region == null)
                return StatusCode(StatusCodes.Status400BadRequest, "region not found");

            return StatusCode(StatusCodes.Status200OK, region);
        }

        /// <summary>
        /// Get region by DDD
        /// </summary>
        /// <response code="200">All Regions</response>
        /// <response code="400">Bad Request</response>
        [HttpGet("ddd/{ddd}")]
        [ProducesResponseType(typeof(List<RegionEntity>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetRegionByDDD([FromRoute] string ddd)
        {
            var region = await _regionService.GetByDDD(ddd);
            if (region == null)
                return StatusCode(StatusCodes.Status400BadRequest, "region not found");

            return StatusCode(StatusCodes.Status200OK, region);
        }
    }
}
