using BLL.DTO;
using BLL.Services;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace lab2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AircraftController : ControllerBase
{
    private readonly AircraftService _service;

    public AircraftController(AircraftService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AircraftDTO>>> Get()
    {
        return this.Ok(await _service.GetAllAircrafts());
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AircraftDTO>> Get([FromRoute] int id)
    {
        try
        {
            var aircraft = await _service.GetAircraft(id);
            
            return Ok(aircraft);
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
            await _service.GetAircraft(id);
        }
        catch (Exception e)
        {
            return NotFound($"No such entity with id {id}");
        }

        try
        {
            await _service.DeleteAircraft(id);
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
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] AircraftDTO aircraftDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        try
        {
            await _service.GetAircraft(id);
        }
        catch
        {
            return NotFound($"No such entity with id {id}");
        }
        
        aircraftDto.Id = id;
        await _service.UpdateAircraft(aircraftDto);

        return NoContent();
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<ActionResult<AircraftDTO>> Create([FromBody] AircraftDTO aircraftDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        int id = 0;
        
        try
        {
            id = await _service.RegisterAircraft(aircraftDto);
            aircraftDto.Id = id;
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
        return CreatedAtAction(nameof(Get), new { id = id }, aircraftDto);
    }
}
