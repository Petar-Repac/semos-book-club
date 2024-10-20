using BookClub.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

var builder = WebApplication.CreateBuilder(args);
 
// Initialize SQLite Batteries
Batteries.Init();
builder.Services.AddControllers();  // Add support for controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 
builder.Services.AddDbContext<BookContext>(options => options.UseSqlite("Data Source=books.db"));

// Add Identity services
builder.Services.AddDbContext<BookContext>(options => options.UseSqlite("Data Source=books.db"));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<BookContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();  // Map controller routes
app.Run();

