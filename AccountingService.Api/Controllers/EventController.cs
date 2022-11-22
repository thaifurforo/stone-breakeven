using AccountingService.Api.Contracts.v1.Requests;
using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace TransactioningService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediator _mediator;

   
        public EventController(IEventStoreRepository eventStoreRepository, IMediator mediator)
        {
            this._eventStoreRepository = eventStoreRepository;
            this._mediator = mediator;
        }

        // GET: api/event
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventStoreRepository.GetAllEvents();
            if (events.IsNullOrEmpty())
            {
                return NoContent();
            }
            return Ok(events);
        }

        // GET: api/event/id/5
        [HttpGet("id/{Id}")]
        public async Task<IActionResult> GetEventById([FromRoute] GetByEventId request)
        {
            var eventId = Guid.Parse(request.Id);
            var @event = await _eventStoreRepository.GetEventById(eventId);

            if (@event is null)
            {
                return NotFound(new { message = "The given Id does not exist in the Event Store Database" });
            }
            return Ok(@event);
        }
        
        // GET: api/event/account/5
        [HttpGet("account/{Id}")]
        public async Task<IActionResult> GetEventByAccountId([FromRoute] GetByAccountId request)
        {
            var events = await _eventStoreRepository.GetEventsByAccountId(request.Id);

            if (events.IsNullOrEmpty())
            {
                return NotFound(new { message = "The given Account Id has no events registered in the Event Store Database" });
            }

            return Ok(events);
        }
        
        // GET: api/event/name/CreatedAccountEvent
        [HttpGet("name/{EventName}")]
        public async Task<IActionResult> GetEventByName([FromRoute] GetByEventName request)
        {
            var events = await _eventStoreRepository.GetEventsByName(request.EventName);

            if (events.IsNullOrEmpty())
            {
                return NotFound(new { message = "There's no event under that name in the database" });
            }

            return Ok(events);
        }

    }
}
