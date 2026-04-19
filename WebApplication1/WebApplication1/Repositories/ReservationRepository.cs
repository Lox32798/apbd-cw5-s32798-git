using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class ReservationRepository
{
    private static int _nextId = 1;
    private static List<Reservation> _reservation = new();

    public ReservationRepository()
    {
        if (!_reservation.Any())
        {
            _reservation.Add(new Reservation
            {
                Id = _nextId++,
                RoomId = 1,
                OrganizerName = "Test",
                Topic = "API",
                Date = DateTime.Today,
                StartTime = new TimeSpan(10,0,0),
                EndTime = new TimeSpan(12,0,0),
                Status = "confirmed"
            });
        }
    }
    public IEnumerable<Reservation> GetAll()
    {
        return _reservation;
    }
    
    public Reservation? GetById(int id)
    {
        return _reservation.FirstOrDefault(reservation => reservation.Id == id);
    }

    public IEnumerable<Reservation> GetByAttributes(DateTime? date, string? status, int? roomId)
    {
        var query = _reservation.AsQueryable();

        if (date.HasValue)
            query = query.Where(r => r.Date == date.Value);

        if (!string.IsNullOrEmpty(status))
            query = query.Where(r => r.Status == status);

        if (roomId.HasValue)
            query = query.Where(r => r.RoomId == roomId.Value);

        return query.ToList();
    }

    public Reservation Add(Reservation reservation)
    {
        var res = reservation;
        res.Id = _nextId++;
        _reservation.Add(res);
        return res;
    }

    public bool Update(int id,Reservation reservation)
    {
        var existing = GetById(id);
        if (existing is null)
        {
            return false;
        }
        
        existing.RoomId = reservation.RoomId;
        existing.Date = reservation.Date;
        existing.Status = reservation.Status;
        existing.OrganizerName = reservation.OrganizerName;
        existing.StartTime = reservation.StartTime;
        existing.EndTime = reservation.EndTime;
        return true;
    }

    public bool Delete(int id)
    {
        var existing = GetById(id);
        if (existing is null)
        {
            return false;
        }
        _reservation.Remove(existing);
        return true;
    }
}