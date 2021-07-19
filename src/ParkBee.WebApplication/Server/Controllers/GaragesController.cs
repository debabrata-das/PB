using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkBee.Domain.AggregatesModel;
using ParkBee.WebApplication.Server.Commands;
using ParkBee.WebApplication.Server.Queries;
using ParkBee.WebApplication.Shared;

namespace ParkBee.WebApplication.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GaragesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IMediator _mediator;

        public GaragesController(ILoggerManager logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Get all <see cref="Garage">Garages details</see> along with its <see cref="Door">Door</see> statuses
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<GarageDto>>> GetAllGaragesWithDoorDetails()
        {
            var res = await _mediator.Send(new GetAllGaragesWithDoorDetailsQuery());
            return Ok(res);
        }

        /// <summary>
        /// Get Garage by its identifier
        /// </summary>
        /// <returns><see cref="GarageDto"/></returns>
        [HttpGet("{identifier:guid}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GarageDto>> GetGarageByIdentifier(Guid identifier)
        {
            try
            {
                GetGarageDetailsQuery garageDetailsQuery = new GetGarageDetailsQuery() {Identifier = identifier};
                var garageDto = await _mediator.Send(garageDetailsQuery);
                if (garageDto == null)
                {
                    return NotFound();
                }

                _logger.Debug($"Found Garage with identifier - {garageDto.Identifier}");
                return Ok(garageDto);
            }
            catch (Exception exc)
            {
                return BadRequest(exc);
            }
        }

        /// <summary>
        /// Creates a Garage
        /// </summary>
        /// <returns>Created Garage</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GarageDto>> CreateGarage(SaveGarageCommand command)
        {
            try
            {
                var garageDto = await _mediator.Send(command);
                _logger.Info($"Garage '{garageDto.Name}' with Identifier '{garageDto.Identifier}' has been created");
                return Ok(garageDto);
            }
            catch (Exception exc)
            {
                return BadRequest(exc);
            }
        }
    }
}
