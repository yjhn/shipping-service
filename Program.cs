using Microsoft.EntityFrameworkCore;

using shipping_service.Persistence.Database;
using shipping_service.Repositories;
        using shipping_service.Services;
using shipping_service.Models;
using shipping_service.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
// Configure services
builder.Configuration.AddJsonFile("appsettings.json");
string dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
bool autoApplyMigrations = builder.Configuration.GetValue<bool>("AutomaticallyApplyMigrations");
bool addSeedData = builder.Configuration.GetValue<bool>("AddSeedDataIfDBEmpty");
bool detailedDbLogging = builder.Configuration.GetValue<bool>("DetailedDbLogging");
double authenticationCookieLifetimeDays = builder.Configuration.GetValue<double>("AuthenticationCookieLifetimeDays");

builder.Services.AddDbContext<DatabaseContext>(option =>
{
    option.UseNpgsql(dbConnectionString);
    if (detailedDbLogging)
    {
        // enable logging for debugging
        option.EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            .LogTo(Console.WriteLine);
    }
});
builder.Services.AddScoped<ICourierRepository, CourierRepository>();
builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();
builder.Services.AddScoped<IShipmentService, ShipmentService>();
builder.Services.AddScoped<ICourierService, CourierService>();
builder.Services.AddScoped<IPostMachineRepository, PostMachineRepository>();
builder.Services.AddScoped<IPostMachineService, PostMachineService>();
string generatorInterface = builder.Configuration.GetValue<string>("GeneratorInterface");
if (generatorInterface == "CodeGenerator")
{
builder.Services.AddScoped<ICodeGenerator, CodeGenerator>();
}
else
{
builder.Services.AddScoped<ICodeGenerator, CodeGeneratorByDigit>();
}
builder.Services.AddScoped<ISenderRepository, SenderRepository>();
builder.Services.AddScoped<IShipmentService, ShipmentService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(authenticationCookieLifetimeDays);
    options.Cookie.Name = "auth";
    options.Cookie.SameSite = SameSiteMode.Strict;
});
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddHttpContextAccessor();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<TokenProvider>();
builder.Services.AddControllers();

//Build
WebApplication app = builder.Build();

if (autoApplyMigrations)
{
    Console.WriteLine("Creating the DB and applying DB migrations");
    // create DB with all migrations applied on startup
    using IServiceScope serviceScope = app.Services.CreateScope();
    DatabaseContext context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
    context.Database.Migrate();
}

if (addSeedData)
{
    Console.WriteLine("Adding seed data to DB");
    SeedData.PopulateIfEmpty(app);
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
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapControllers();

});

app.Run();
