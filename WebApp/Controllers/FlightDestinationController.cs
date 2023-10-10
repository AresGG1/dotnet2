using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace lab2.Controllers;


[ApiController]
[Route("api/[controller]")]
public class FlightDestinationController : ControllerBase
{
    private readonly FlightDestinationService _service;

    public FlightDestinationController(FlightDestinationService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<FlightDestinationDTO>>> Get()
    {
        return this.Ok(await _service.GetAllFlightDestinations());
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FlightDestinationDTO>> Get([FromRoute] int id)
    {
        try
        {
            var flightDestination = await _service.GetFlightDestination(id);
            
            return Ok(flightDestination);
        }
        catch (Exception e)
        {
            return NotFound($"No such entity with id {id}");
        }
        
    }
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        try
        {
            await _service.GetFlightDestination(id);
        }
        catch (Exception e)
        {
            return NotFound($"No such entity with id {id}");
        }

        try
        {
            await _service.DeleteFlightDestination(id);
        }
        catch (Exception e)
        {
            return Conflict(e.Message);
        }
            
        return NoContent();
    }
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] FlightDestinationDTO flightDestinationDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        try
        {
            await _service.GetFlightDestination(id);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound($"No such entity with id {id}");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
        flightDestinationDto.Id = id;
        await _service.UpdateFlightDestination(flightDestinationDto);

        return NoContent();
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<ActionResult<AircraftDTO>> Create([FromBody] FlightDestinationDTO flightDestinationDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        int id = 0;
        try
        {
            id = await _service.RegisterFlightDestination(flightDestinationDto);

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        flightDestinationDto.Id = id;
        
        return CreatedAtAction(nameof(Get), new { id = id }, flightDestinationDto);
    }
}
