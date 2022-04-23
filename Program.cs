using Microsoft.EntityFrameworkCore;

using shipping_service.Persistence.Database;
using shipping_service.Repositories;
using shipping_service.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
// Configure services
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddDbContext<DatabaseContext>(option =>
    option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        // enable logging for debugging
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
        .LogTo(Console.WriteLine));
builder.Services.AddScoped<ICourierRepository, CourierRepository>();
builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();
builder.Services.AddScoped<IPostMachineRepository, PostMachineRepository>();
builder.Services.AddScoped<ISenderRepository, SenderRepository>();
builder.Services.AddScoped<IShipmentService, ShipmentService>();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); This one could come in handy.
builder.Services.AddServerSideBlazor();

//Build
WebApplication app = builder.Build();

// create DB with all migrations applied on startup
using (IServiceScope serviceScope = app.Services.CreateScope())
{
    DatabaseContext context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
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
SeedData.PopulateIfEmpty(app);
app.Run();