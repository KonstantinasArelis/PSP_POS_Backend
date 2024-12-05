using Microsoft.EntityFrameworkCore;
using PSPBackend.Model;
public class AppDbContext : DbContext
{
    public IConfiguration _config {get; set; }
    public AppDbContext(IConfiguration config)
    {
        _config = config;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_config.GetConnectionString("DatabaseConnection"));
    }

    public DbSet<OrderModel> Order {get; set; }
    public DbSet<ReservationModel> Reservation {get; set; }
    public DbSet<UserModel> User {get; set; }
    public DbSet<OrderItemModel> OrderItem {get; set; }
    public DbSet<BusinessModel> Business {get; set; }
}