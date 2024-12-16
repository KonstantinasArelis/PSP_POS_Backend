using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PSPBackend.Model;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

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
        builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddControllers();

builder.Services.AddTransient<AppDbContext>();
builder.Services.AddTransient<OrderRepository>();
builder.Services.AddTransient<OrderService>();
builder.Services.AddTransient<ReservationRepository>();
builder.Services.AddTransient<ReservationService>();
// builder.Services.AddTransient<UserRepository>();
// builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<DiscountRepository>();
builder.Services.AddTransient<DiscountService>();
builder.Services.AddTransient<TaxRepository>();
builder.Services.AddTransient<TaxService>();
builder.Services.AddTransient<BusinessRepository>();
builder.Services.AddTransient<BusinessService>();
builder.Services.AddTransient<PaymentService>();
builder.Services.AddTransient<PaymentRepository>();

builder.Services.AddAuthorization();

builder.Services
    .AddIdentity<UserModel, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services
    .AddIdentityCore<UserModel>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddSignInManager<SignInManager<UserModel>>()
    .AddDefaultTokenProviders();

var key = builder.Configuration.GetValue<string>("JwtSettings:Secret");
builder.Services
    .AddAuthentication(u =>
    {
        u.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        u.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(u =>
    {
        u.RequireHttpsMetadata = false;
        u.SaveToken = true;
        u.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        JwtBearerDefaults.AuthenticationScheme,
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Description =
                "JWT Authorization header using the Bearer scheme. \r\n\r\n "
                + "Enter 'Bearer' [space] and then your token in the text input below. \r\n\r\n"
                + "Example: Bearer 12345abcdefgh",
            BearerFormat = "JWT",
            Scheme = JwtBearerDefaults.AuthenticationScheme
        }
    );
    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        }
    );
});

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Add Authentication Middleware
app.UseAuthorization(); // Add Authorization Middleware

app.MapControllers();

app.Run();
