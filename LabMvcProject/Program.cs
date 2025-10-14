using Microsoft.EntityFrameworkCore;
using LabMvcProject.Data; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
var dbFile = connStr.Replace("Data Source=", "");
Console.WriteLine($"[DEBUG] SQLite DB file (relative): {dbFile}");
Console.WriteLine($"[DEBUG] SQLite DB full path: {Path.GetFullPath(dbFile)}");



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Comment out HTTPS redirection for Codespaces
// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
