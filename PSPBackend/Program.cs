var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("connectionStrings.json", optional: true, reloadOnChange: true);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000")
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

var app = builder.Build();

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