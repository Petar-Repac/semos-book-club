using System.Text;
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

// Create roles at startup
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    await CreateRolesAsync(roleManager, userManager);  // Call the role creation function
}

async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
{
    var roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Create an admin user
    var adminEmail = "admin@example.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var newAdmin = new IdentityUser { UserName = adminEmail, Email = adminEmail };
        await userManager.CreateAsync(newAdmin, "AdminPassword123!");
        await userManager.AddToRoleAsync(newAdmin, "Admin");
    }
    
    // Create a basic user
    var basicUserEmail = "user@example.com";
    var basicUser = await userManager.FindByEmailAsync(basicUserEmail);
    if (basicUser == null)
    {
        var newUser = new IdentityUser { UserName = basicUserEmail, Email = basicUserEmail };
        await userManager.CreateAsync(newUser, "UserPassword123!");
        await userManager.AddToRoleAsync(newUser, "User");
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();  // Enable authentication middleware
app.UseAuthorization();
app.MapControllers();  // Map controller routes
app.Run();

