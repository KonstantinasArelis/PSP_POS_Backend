using PSPBackend.Model;

public class ReservationRepository
{
    private readonly AppDbContext _context;

    public ReservationRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<ReservationModel> GetReservation(int page_nr, int limit, int employee_id, 
            int client_name, int  client_phone, DateTime created_before, DateTime created_after, 
            DateTime last_modified_before, DateTime last_modified_after,
            DateTime appointment_time_before, DateTime appointment_time_after, 
            int duration_less_than, int duration_more_than, int status, int service_id) 
    {
        var query = _context.Reservation.AsQueryable();

        Console.WriteLine("LOG: repository returns Reservation");
        return query; 
    }

    public int CreateReservation(ReservationModel reservation)
    {
        Console.WriteLine("CreateReservation repository");
        _context.Reservation.Add(reservation);
        int rowsAffected = _context.SaveChanges(); 

        return rowsAffected;
    }
}