using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SmartEMS.API.Middleware;
using SmartEMS.API.Helpers;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SmartEMS.API.Data;
using SmartEMS.API.Repositories.Interfaces;
using SmartEMS.API.Repositories.Implementations;
using SmartEMS.API.Services.Implementations;
using SmartEMS.API.Services.Interfaces;
using SmartEMS.API.Models;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Bind and VALIDATE JwtSettings
var jwtSection = builder.Configuration.GetSection("JwtSettings");

if (!jwtSection.Exists())
{
    throw new Exception("JwtSettings section is missing in appsettings.json");
}

var jwtSettings = jwtSection.Get<JwtSettings>();

if (jwtSettings == null ||
    string.IsNullOrWhiteSpace(jwtSettings.Key) ||
    string.IsNullOrWhiteSpace(jwtSettings.Issuer) ||
    string.IsNullOrWhiteSpace(jwtSettings.Audience))
{
    throw new Exception("Invalid JwtSettings configuration");
}

// Register strongly typed config
builder.Services.Configure<JwtSettings>(jwtSection);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("Database connection string is missing");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(connectionString));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDesignationRepository, DesignationRepository>();
builder.Services.AddScoped<IDesignationService, DesignationService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeSelfService, EmployeeSelfService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();
builder.Services.AddScoped<ILeaveService, LeaveService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICorrectionRepository, CorrectionRepository>();
// ✅ JWT Authentication
var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddSwaggerGen(options =>
{
    // 🔐 Add JWT Authentication
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Token like: Bearer {your_token}"
    });

    // 🔐 Apply globally
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});



builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Users.Any(u => u.Role == UserRole.Admin))
    {
        var admin = new User
        {
            UserId = "ADM001",
            Email = "admin@smartems.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = UserRole.Admin,
            Status = AccountStatus.Active
        };

        db.Users.Add(admin);
        db.SaveChanges();
    }
    if (!db.Departments.Any())
    {
        db.Departments.AddRange(
            new Department { Code = "HR", Name = "Human Resources" },
            new Department { Code = "IT", Name = "Information Technology" },
            new Department { Code = "FINANCE", Name = "Finance" }
        );

        db.SaveChanges();
    }

    if (!db.Designations.Any())
    {
        var hr = db.Departments.First(d => d.Code == "HR");
        var it = db.Departments.First(d => d.Code == "IT");
        var finance = db.Departments.First(d => d.Code == "FINANCE");

        db.Designations.AddRange(
            new Designation { Name = "HR Executive", DepartmentId = hr.Id },
            new Designation { Name = "HR Manager", DepartmentId = hr.Id },

            new Designation { Name = "Software Developer", DepartmentId = it.Id },
            new Designation { Name = "QA Engineer", DepartmentId = it.Id },

            new Designation { Name = "Accountant", DepartmentId = finance.Id }
        );

        db.SaveChanges();
    }
}

app.UseCors("AllowFrontend");


// Middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();