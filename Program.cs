using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Models;
using LibraryApi.Repo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));


//Services
builder.Services.AddScoped<IGenericRepository<Author>, GenericRepository<Author>>();
builder.Services.AddScoped<IGenericRepository<Customer>, GenericRepository<Customer>>();
builder.Services.AddScoped<IGenericRepository<LibraryBranch>, GenericRepository<LibraryBranch>>();
builder.Services.AddScoped<IGenericRepository<Book>, GenericRepository<Book>>();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Library API Project",
        Description = "This is an API Project with CRUD operations for FDU Project",
        Contact = new OpenApiContact
        {
            Name = "Eduardo Flores",
            Email = "e.flores@student.fdu.edu",
            Url = new Uri("http://www.eduardoflores.name")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API Project v1");
        c.RoutePrefix = "";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
