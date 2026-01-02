using System.Text;
using System.Xml;
using LibraryCore.Entities;
using LibraryDataAccess;
using LibraryDataAccess.Repositories;
using LibraryService.Interfaces;
using LibraryService.MapProfile;
using LibraryService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;

// Log yapýlandýrmasý
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Jwt yapýlandýrmasý
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

var isReadFromJwt = builder.Configuration.GetSection("Jwt:Key");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Kutuphane API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<DatabaseConnection>();

builder.Services.AddAutoMapper(typeof(MapProfile));

// Service Implamantations
// AddTransient: Her istekte yeni bir instance oluþturur.
// AddScoped: Her istekte bir instance oluþturur ve isteðin sonunda yok eder.
builder.Services.AddScoped<IGenericRepository<Author>, Repository<Author>>();
builder.Services.AddScoped<IAuthorService, AuthorService>();

builder.Services.AddScoped<IGenericRepository<Book>, Repository<Book>>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<IGenericRepository<Category>, Repository<Category>>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IGenericRepository<User>, Repository<User>>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
