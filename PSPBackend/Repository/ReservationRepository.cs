using Microsoft.EntityFrameworkCore;
using PSPBackend.Model;

public class ReservationRepository
{
    private readonly AppDbContext _context;

    public ReservationRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<ReservationModel> GetReservations(ReservationGetDto reservationGetDto) 
    {
        var query = _context.Reservation.AsQueryable();


        if(reservationGetDto.id != null)
        {
            query = query.Where(c => c.id == reservationGetDto.id);
        }
        if(reservationGetDto.business_id != null)
        {
            query = query.Where(c => c.business_id == reservationGetDto.business_id);
        }
        if(reservationGetDto.employee_id != null)
        {
            query = query.Where(c => c.employee_id == reservationGetDto.employee_id);
        }
        if(reservationGetDto.client_name != null)
        {
            query = query.Where(c => c.client_name == reservationGetDto.client_name);
        }
        if(reservationGetDto.client_phone != null)
        {
            query = query.Where(c => c.client_phone == reservationGetDto.client_phone);
        }
        if(reservationGetDto.created_before != null)
        {
            query = query.Where(c => c.created_at < reservationGetDto.created_before);
        }
        if(reservationGetDto.created_after != null)
        {
            query = query.Where(c => c.created_at > reservationGetDto.created_after);
        }
        if(reservationGetDto.created_before != null)
        {
            query = query.Where(c => c.last_modified < reservationGetDto.last_modified_before);
        }
        if(reservationGetDto.created_after != null)
        {
            query = query.Where(c => c.last_modified > reservationGetDto.last_modified_after);
        }
        if(reservationGetDto.appointment_time_before != null)
        {
            query = query.Where(c => c.appointment_time < reservationGetDto.appointment_time_before);
        }
        if(reservationGetDto.appointment_time_after != null)
        {
            query = query.Where(c => c.appointment_time > reservationGetDto.appointment_time_after);
        }
        if(reservationGetDto.duration_less_than != null)
        {
            query = query.Where(c => c.duration < reservationGetDto.duration_less_than);
        }
        if(reservationGetDto.duration_more_than != null)
        {
            query = query.Where(c => c.duration > reservationGetDto.duration_more_than);
        }
        if(reservationGetDto.status != null)
        {
            query = query.Where(c => c.ReservationStatus == reservationGetDto.status);
        }
        if(reservationGetDto.service_id != null)
        {
            query = query.Where(c => c.service_id == reservationGetDto.service_id);
        }

        return query; 
    }

    public ReservationModel GetReservationById(int id)
    {
        ReservationModel? gottenReservation = _context.Reservation.Single(c => c.id == id);

        if(gottenReservation == null)
        {
            throw new KeyNotFoundException("Order with id " + id + " was not found");
        }
        return gottenReservation;
    }

    public ReservationModel CreateReservation(ReservationModel reservation)
    {
        _context.Reservation.Add(reservation);
        int rowsAffected = _context.SaveChanges(); 
         
        if(rowsAffected == 0)
        {
            throw new DbUpdateException("Failed to create a new reservation");
        }
        return reservation;
    }

    public ReservationModel UpdateReservation(int id, ReservationPatchDto reservationDto)
    {
        ReservationModel? reservation;
        try
        {
            reservation = GetReservationById(id);
        } catch (KeyNotFoundException ex) {
            throw;
        }


        foreach (var property in typeof(ReservationPatchDto).GetProperties())
        {
            var dtoValue = property.GetValue(reservationDto);
            if(dtoValue != null)
            {
                
                var reservationProperty = typeof(ReservationModel).GetProperty(property.Name);
                reservationProperty?.SetValue(reservation, dtoValue);
            }
        }
        reservation.last_modified = DateTime.Now;
        int result = _context.SaveChanges();
        if(result == 0)
        {
            throw new DbUpdateException("Could not save updated reservation");
        }

        return reservation;
    }

    public int GetNewOrderId()
    {
        return _context.Reservation.Select(o => o.id).ToList().OrderByDescending(a => a).FirstOrDefault() + 1;
    }
}