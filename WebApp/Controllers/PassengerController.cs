using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace lab2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PassengerController : ControllerBase
{
    private readonly PassengerService _service;

    public PassengerController(PassengerService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PassengerDTO>>> Get()
    {
        return this.Ok(await _service.GetALlPassengers());
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PassengerDTO>> Get([FromRoute] int id)
    {
        try
        {
            var passenger = await _service.GetPassenger(id);
            
            return Ok(passenger);
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
            await _service.GetPassenger(id);
        }
        catch (Exception e)
        {
            return NotFound($"No such entity with id {id}");
        }

        try
        {
            await _service.DeletePassenger(id);
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
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] PassengerDTO passenger)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        try
        {
            await _service.GetPassenger(id);
        }
        catch
        {
            return NotFound($"No such entity with id {id}");
        }
        
        passenger.Id = id;
        await _service.UpdatePassenger(passenger);

        return NoContent();
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<ActionResult<AircraftDTO>> Create([FromBody] PassengerDTO passenger)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        int id = await _service.RegisterPassenger(passenger);
        passenger.Id = id;
        
        return CreatedAtAction(nameof(Get), new { id = id }, passenger);
    }
}
