using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace lab2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AirportController : ControllerBase
{
    private readonly AirportService _service;

    public AirportController(AirportService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AirportDTO>>> Get()
    {
        return this.Ok(await _service.GetAirports());
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AirportDTO>> Get([FromRoute] int id)
    {
        try
        {
            var flightDestination = await _service.GetAirport(id);
            
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
            await _service.GetAirport(id);
        }
        catch (Exception e)
        {
            return NotFound($"No such entity with id {id}");
        }

        try
        {
            await _service.DeleteAirport(id);
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
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] AirportDTO airportDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        try
        {
            await _service.GetAirport(id);
        }
        catch
        {
            return NotFound($"No such entity with id {id}");
        }
        
        airportDto.Id = id;
        await _service.UpdateAirport(airportDto);

        return NoContent();
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<ActionResult<AircraftDTO>> Create([FromBody] AirportDTO airportDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        int id = await _service.RegisterAirport(airportDto);
        airportDto.Id = id;
        
        return CreatedAtAction(nameof(Get), new { id = id }, airportDto);
    }
}
