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
    public DbSet<DiscountModel> Discount {get; set; }
    public DbSet<TaxModel> Tax {get; set; }
    public DbSet<OrderItemModel> OrderItem {get; set; }
    public DbSet<BusinessModel> Business {get; set; }
    public DbSet<PaymentModel> Payment {get; set; }
    public DbSet<ProductModel> Product {get; set; }

}