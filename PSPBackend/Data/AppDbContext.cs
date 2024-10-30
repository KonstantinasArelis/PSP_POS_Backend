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
        Console.WriteLine("TESTAVIMAS: " + _config.GetConnectionString("DatabaseConnection"));
        optionsBuilder.UseSqlServer(_config.GetConnectionString("DatabaseConnection"));
    }

    public DbSet<OrderModel> _Order {get; set; }
}