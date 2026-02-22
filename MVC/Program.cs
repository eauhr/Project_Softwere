using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using ServiceLayer;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DBLibraryContextConnection' not found.");;

//// Connect to database
//builder.Services.AddDbContext<DBLibraryContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Temporary test line
//connection string for SQL Server 
//var connectionString = "Server=(localdb)\\mssqllocaldb;Database=footballteams;Trusted_Connection=True;TrustServerCertificate=True;";
//var connectionString = "Server=localhost;Port=3306;Database=footballteams;User=root;Password=root;";

//var connectionString = "Server=localhost;Port=3306;Database=footballteams;User=root;Password=Maritsa_154;";

var serverVersion = new MySqlServerVersion(new Version(9, 0, 0));

builder.Services.AddDbContext<DBLibraryContext>(options =>
    options.UseMySql(connectionString, serverVersion));

builder.Services.AddIdentity<BusinessLayer.User, Microsoft.AspNetCore.Identity.IdentityRole>()
    .AddEntityFrameworkStores<DBLibraryContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IDb<Footballer, int>, FootballerContext>();
builder.Services.AddScoped<IDb<Team, int>, TeamsContext>();
builder.Services.AddScoped<IDb<Country, string>, CountryContext>();
builder.Services.AddScoped<IDb<Stadium, int>, StadiumContext>();
builder.Services.AddScoped<IDb<Continent, string>, ContinentContext>();
builder.Services.AddScoped<IDb<Trophy, int>, TrophiesContext>();

builder.Services.AddScoped<LibraryManager<Footballer, int>>();
builder.Services.AddScoped<LibraryManager<Team, int>>();
builder.Services.AddScoped<LibraryManager<Country, string>>();
builder.Services.AddScoped<LibraryManager<Stadium, int>>();
builder.Services.AddScoped<LibraryManager<Continent, string>>();
builder.Services.AddScoped<LibraryManager<Trophy, int>>();

builder.Services.AddScoped<IEmailSender, EmailSender>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    if (!await roleManager.RoleExistsAsync("Administrator"))
    {
        await roleManager.CreateAsync(new IdentityRole("Administrator"));
    }

    if (!await roleManager.RoleExistsAsync("User"))
    {
        await roleManager.CreateAsync(new IdentityRole("User"));
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
