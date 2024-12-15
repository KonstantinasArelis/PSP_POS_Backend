using PSPBackend.Repository;
using PSPBackend.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("connectionStrings.json", optional: true, reloadOnChange: true);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin() // Permissive: allows requests from any origin
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

builder.Services.AddTransient<AppDbContext>();
builder.Services.AddTransient<OrderRepository>();
builder.Services.AddTransient<OrderService>();
builder.Services.AddTransient<ReservationRepository>();
builder.Services.AddTransient<ReservationService>();
builder.Services.AddTransient<DiscountRepository>();
builder.Services.AddTransient<DiscountService>();
builder.Services.AddTransient<TaxRepository>();
builder.Services.AddTransient<TaxService>();
builder.Services.AddTransient<BusinessRepository>();
builder.Services.AddTransient<BusinessService>();
builder.Services.AddTransient<PaymentService>();
builder.Services.AddTransient<PaymentRepository>();
builder.Services.AddTransient<MenuRepository>();
builder.Services.AddTransient<MenuService>();

var app = builder.Build();

// Ensure CORS middleware is applied BEFORE routing and other middleware
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Console.WriteLine("BACKEND LOGAI ATSIRANDA CIA");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
