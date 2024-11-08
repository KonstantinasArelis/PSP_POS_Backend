using PSPBackend.Model;

public class ReservationRepository
{
    private readonly AppDbContext _context;

    public ReservationRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<ReservationModel> GetReservation(int page_nr, int limit, int? id, int? business_id, int? employee_id, 
            string? client_name, string?  client_phone, DateTime? created_before, DateTime? created_after, 
            DateTime? last_modified_before, DateTime? last_modified_after,
            DateTime? appointment_time_before, DateTime? appointment_time_after, 
            int? duration_less_than, int? duration_more_than, int? status, int? service_id) 
    {
        var query = _context.Reservation.AsQueryable();


        if(id != null)
        {
            query = query.Where(c => c.id == id);
        }
        if(business_id != null)
        {
            query = query.Where(c => c.business_id == business_id);
        }
        if(employee_id != null)
        {
            query = query.Where(c => c.employee_id == employee_id);
        }
        if(client_name != null)
        {
            query = query.Where(c => c.client_name == client_name);
        }
        if(client_phone != null)
        {
            query = query.Where(c => c.client_phone == client_phone);
        }
        if(created_before != null)
        {
            query = query.Where(c => c.created_at < created_before);
        }
        if(created_after != null)
        {
            query = query.Where(c => c.created_at > created_after);
        }
        if(created_before != null)
        {
            query = query.Where(c => c.last_modified < last_modified_before);
        }
        if(created_after != null)
        {
            query = query.Where(c => c.last_modified > last_modified_after);
        }
        if(appointment_time_before != null)
        {
            query = query.Where(c => c.appointment_time < appointment_time_before);
        }
        if(appointment_time_after != null)
        {
            query = query.Where(c => c.appointment_time > appointment_time_after);
        }
        if(duration_less_than != null)
        {
            query = query.Where(c => c.duration < duration_less_than);
        }
        if(duration_more_than != null)
        {
            query = query.Where(c => c.duration > duration_more_than);
        }
        if(status != null)
        {
            query = query.Where(c => c.ReservationStatus == status);
        }
        if(service_id != null)
        {
            query = query.Where(c => c.service_id == service_id);
        }

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