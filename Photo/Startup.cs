using Photo.Controllers;
using Photo.Data;
using Photo.Infrastructure.Extensions;
using Photo.Model;
using Photo.Services.Ads;
using Photo.Services.Cameras;
using Photo.Services.Dealers;
using Photo.Services.Images;

namespace Photo
{
  
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        public Startup(IConfiguration configuration) 
            => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<ApplicationDbContext>(options => options
                    .UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<ICamerasServices,CamerasService>();
            services.AddTransient<IDealerService, DealerService>();
            services.AddTransient<IAdsServices, AdsService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<ApplicationDbContext>();
            // services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddScoped<DealerService>();

            services
                .AddDefaultIdentity<User>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

          services.AddAutoMapper(typeof(Startup));

            services.AddMemoryCache();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            });

            


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDatabase();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
             //   app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultAreaRoute();

                    endpoints.MapControllerRoute(
                        name: "Camera Details",
                        pattern: "/Cameras/Details/{id}",
                        defaults: new 
                        { 
                            controller = typeof(CamerasController).GetControllerName(), 
                            action = nameof(CamerasController.Details) 
                        });

                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapRazorPages();
                });
        }
    }
}
