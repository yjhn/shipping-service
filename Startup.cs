using Microsoft.EntityFrameworkCore;

using shipping_service.Entities;
using shipping_service.Repositories;
using shipping_service.Services;

namespace shipping_service
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ServiceDbContext>(option =>
                option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<ICourierRepository, CourierRepository>();
            services.AddScoped<IPackageRepository, PackageRepository>();
            services.AddScoped<IPostMachineRepository, PostMachineRepository>();
            services.AddScoped<ISenderRepository, SenderRepository>();
            services.AddScoped<IPackageService, PackageService>();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); This one could come in handy.
            services.AddServerSideBlazor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
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
        }

    }

}