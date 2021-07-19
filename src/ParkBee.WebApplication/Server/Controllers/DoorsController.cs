using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkBee.Domain.AggregatesModel;
using ParkBee.WebApplication.Server.Commands;
using ParkBee.WebApplication.Shared;

namespace ParkBee.WebApplication.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DoorsController : ControllerBase
    {
        private readonly IDoorRepository _doorRepository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public DoorsController(IDoorRepository doorRepository, ILoggerManager logger, IMapper mapper, IMediator mediator)
        {
            _doorRepository = doorRepository;
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
        }

        /// <summary>
        /// Get Door by its identifier
        /// </summary>
        /// <param name="identifier"><see cref="Door.Identifier">Door identifier</see></param>
        /// <returns><see cref="Door"/></returns>
        [HttpGet("{identifier:guid}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DoorDto>> GetDoorByIdentifier(Guid identifier)
        {
            try
            {
                var door = await _doorRepository.GetByIdentifier(identifier);
                if (door == null)
                {
                    return NotFound();
                }
                
                _logger.Debug($"Found Door with identifier - {identifier}");
                
                var doorDto = _mapper.Map<Door, DoorDto>(door);
                return Ok(doorDto);
            }
            catch (Exception exc)
            {
                return BadRequest(exc);
            }
        }

        /// <summary>
        /// Create Door
        /// </summary>
        /// <returns>Created Door</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DoorDto>> CreateDoor(SaveDoorCommand command)
        {
            try
            {
                var doorDto = await _mediator.Send(command);
                _logger.Info($"Door '{doorDto.Identifier}' for Garage {doorDto.GarageIdentifier} has been created");
                return Ok(doorDto);
            }
            catch (Exception exc)
            {
                return BadRequest(exc);
            }
        }

        /// <summary>
        /// Update Door
        /// </summary>
        /// <returns>Updated Door</returns>
        [HttpPatch("")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DoorDto>> UpdateDoor(SaveDoorCommand command)
        {
            try
            {
                var doorDto = await _mediator.Send(command);
                _logger.Info($"Door '{doorDto.Identifier}' for Garage {doorDto.GarageIdentifier} has been updated");
                return Ok(doorDto);
            }
            catch (Exception exc)
            {
                return BadRequest(exc);
            }
        }

        /// <summary>
        /// Removing <see cref="Door">Door</see> 
        /// </summary>
        /// <param name="identifier"><see cref="Door.Identifier">Garage Identifier</see></param>
        [HttpDelete("{identifier:guid}")]
        public async Task<ActionResult> RemoveDoor(Guid identifier)
        {
            try
            {
                await _doorRepository.RemoveDoor(identifier);
                return Ok();
            }
            catch (Exception exc)
            {
                return BadRequest(exc);
            }
        }
    }
}
