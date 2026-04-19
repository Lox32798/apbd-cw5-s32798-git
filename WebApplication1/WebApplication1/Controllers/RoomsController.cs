using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController(RoomRepository roomService, ReservationRepository reservationService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector, [FromQuery] bool? activeOnly)
    {
        var rooms = roomService.Filter(minCapacity, hasProjector, activeOnly);
        return Ok(rooms);
    }
    
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var room = roomService.GetById(id);
        if (room == null)
            return NotFound();

        return Ok(room);
    }
    
    [HttpGet("building/{buildingCode}")]
    public IActionResult GetByBuilding(string buildingCode)
    {
        var rooms = roomService.GetByBuilding(buildingCode);
        return Ok(rooms);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Room room)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = roomService.Add(room);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
    
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Room room)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!roomService.Update(id, room))
            return NotFound();

        return Ok(room);
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var room = roomService.GetById(id);
        if (room == null)
            return NotFound();
        
        var hasReservations = reservationService
            .GetAll()
            .Any(r => r.RoomId == id);

        if (hasReservations)
            return Conflict("Room has reservations");

        roomService.Delete(id);
        return NoContent();
    }
}