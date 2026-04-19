using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController(ReservationRepository service) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(service.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var reservation = service.GetById(id);
        if (reservation == null)
            return NotFound();
        return Ok(reservation);
    }

    [HttpGet]
    public IActionResult Filter([FromQuery] DateTime? date, [FromQuery] string? status, int? roomId)
    {
        var res = service.GetByAttributes(date, status, roomId);
        return Ok(res);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Reservation reservation)
    {
        var conflict = service.GetAll().Any(r =>
            r.RoomId == reservation.RoomId &&
            r.Date == reservation.Date &&
            reservation.StartTime < r.EndTime &&
            reservation.EndTime > r.StartTime);

        if (conflict)
            return Conflict("Time conflict");
        
        var res = service.Add(reservation);
        return CreatedAtAction(nameof(GetById), new { id = res.Id }, res);
    }

    [HttpPut("{id}")]
    public IActionResult Put([FromRoute] int id, [FromBody] Reservation reservation)
    {
        if (service.Update(id, reservation))
            return NoContent();
        return NotFound();
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if(service.Delete(id))
            return NoContent();
        return NotFound();
    }
}
