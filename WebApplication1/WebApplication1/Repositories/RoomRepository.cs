namespace WebApplication1.Repositories;
using WebApplication1.Models;
public class RoomRepository
{
    private static int _nextId = 1;
    private static List<Room> _rooms = new();

    public RoomRepository()
    {
        if (!_rooms.Any())
        {
            _rooms.AddRange(new List<Room>
            {
                new Room { Id = _nextId++, Name = "A101", BuildingCode = "A", Floor = 1, Capacity = 20, HasProjector = true, IsActive = true },
                new Room { Id = _nextId++, Name = "B202", BuildingCode = "B", Floor = 2, Capacity = 30, HasProjector = false, IsActive = true },
                new Room { Id = _nextId++, Name = "C303", BuildingCode = "C", Floor = 3, Capacity = 15, HasProjector = true, IsActive = false }
            });
        }
    }

    public IEnumerable<Room> GetAll()
    {
        return _rooms;
    }

    public Room? GetById(int id)
    {
        return _rooms.FirstOrDefault(r => r.Id == id);
    }

    public IEnumerable<Room> Filter(int? minCapacity, bool? hasProjector, bool? activeOnly)
    {
        var query = _rooms.AsQueryable();

        if (minCapacity.HasValue)
            query = query.Where(r => r.Capacity >= minCapacity.Value);

        if (hasProjector.HasValue)
            query = query.Where(r => r.HasProjector == hasProjector.Value);

        if (activeOnly == true)
            query = query.Where(r => r.IsActive);

        return query.ToList();
    }

    public IEnumerable<Room> GetByBuilding(string buildingCode)
    {
        return _rooms.Where(r => r.BuildingCode == buildingCode).ToList();
    }

    public Room Add(Room room)
    {
        room.Id = _nextId++;
        _rooms.Add(room);
        return room;
    }

    public bool Update(int id, Room updated)
    {
        var existing = GetById(id);
        if (existing == null) return false;

        existing.Name = updated.Name;
        existing.BuildingCode = updated.BuildingCode;
        existing.Floor = updated.Floor;
        existing.Capacity = updated.Capacity;
        existing.HasProjector = updated.HasProjector;
        existing.IsActive = updated.IsActive;

        return true;
    }

    public bool Delete(int id)
    {
        var room = GetById(id);
        if (room == null) return false;

        _rooms.Remove(room);
        return true;
    }
}