using Common.MessagingService;
using Common.MessagingService.QueuesConfig;
using Domain.Contact.Service;
using Integration;
using Integration.Region;
using Integration.Region.Dtos;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TechChallenge.Controllers.Contacts.Dto;
using TechChallenge1.Controllers.Contacts.Dto;
using TechChallenge1.Helpers.Mappers;

namespace TechChallenge1.Controllers.Contacts.Http
{
    [Route("api/[controller]")]
    [SwaggerTag("Endpoints to manage contacts")]
    [Produces("application/json")]
    public class ContactsController : Controller
    {
        private readonly IContactService _contactsService;
        private readonly IRegionIntegration _regionService;
        private readonly IIntegrationService _integrationService;
        private readonly IRabbitMqService _rabbitMqService;

        public ContactsController(IContactService contactsService,
            IRegionIntegration regionService,
            IRabbitMqService rabbitMqService,
            IIntegrationService integrationService)
        {
            _contactsService = contactsService;
            _regionService = regionService;
            _rabbitMqService = rabbitMqService;
            _integrationService = integrationService;
        }

        /// <summary>
        /// Add a new contact
        /// </summary>
        /// <param name="dto">Contact DTO</param>
        /// <response code="201">Contact created with success</response>
        /// <response code="400">Bad Request</response>
        [HttpPost("")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddContact([FromBody] ContactPostDto dto)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);

            Func<Task<RegionRequestGetDto>> call = () => { return _regionService.GetById(dto.RegionId); };
            var region = await _integrationService.SendRequestWithPolicy(call);

            if (region == null)
                return StatusCode(StatusCodes.Status400BadRequest, "region not found");

            var contactEntity = ContactMapper.ContactMapPostDto(dto);

            var wasSent = await _rabbitMqService.SendMessage(QueueNames.ContactInsert, QueueNames.ContactInsert, contactEntity);

            if (!wasSent)
                return StatusCode(StatusCodes.Status400BadRequest, "error on sent insert");

            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        /// Get contacts by region
        /// </summary>
        /// <param name="ddd">Region ddd</param>
        /// <response code="200">Contacts by region</response>
        /// <response code="400">Bad Request</response>
        [HttpGet("by-region/{ddd}")]
        [ProducesResponseType(typeof(List<ContactsGetDto>),200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetContactsByRegion([FromRoute] string ddd)
        {
            Func<Task<RegionRequestGetDto>> call = () => { return _regionService.GetByDDD(ddd); };
            var region = await _integrationService.SendRequestWithPolicy(call);

            if (region == null)
                return StatusCode(StatusCodes.Status400BadRequest, "region not found");

            var contacts = await _contactsService.GetByRegionId(region.Id);

            if(!contacts.Any())
                return StatusCode(StatusCodes.Status400BadRequest, "No contact found for the ddd provided");

            var contactsDto = ContactMapper.MapContactGetDto(contacts, region);

            return StatusCode(StatusCodes.Status200OK, contactsDto);
        }
    }
}
