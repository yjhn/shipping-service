using Microsoft.EntityFrameworkCore;

using shipping_service.Persistence;
using shipping_service.Persistence.DatabaseContext;
using shipping_service.Persistence.Entities;
using shipping_service.Repositories;
using shipping_service.Services;

var builder = WebApplication.CreateBuilder(args);
// Configure services
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddDbContext<DatabaseContext>(option =>
    option.UseNpgsql("Server=::1;Port=5432;Database=shipping_service_db;User Id=admin;Password=password")
        // enable logging for debugging
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
        .LogTo(Console.WriteLine));
    // option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ICourierRepository, CourierRepository>();
builder.Services.AddScoped<IPackageRepository, PackageRepository>();
builder.Services.AddScoped<IPostMachineRepository, PostMachineRepository>();
builder.Services.AddScoped<ISenderRepository, SenderRepository>();
builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); This one could come in handy.
builder.Services.AddServerSideBlazor();

//Build
var app = builder.Build();

// create DB with all migrations applied on startup
using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
    context.Database.Migrate();
}

//Configure
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});
app.Run();