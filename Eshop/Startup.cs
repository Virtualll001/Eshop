using Eshop.Business.Interfaces;
using Eshop.Business.Managers;
using Eshop.Data;
using Eshop.Data.Interfaces;
using Eshop.Data.Models;
using Eshop.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Eshop
{
    public class Startup
    {
        public Startup(IConfiguration configuration) //vstupní bod každé aplikace
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services) //dependency container
        {
            //databáze
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies());

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<ICategoryManager, CategoryManager>();
            services.AddScoped<IProductManager, ProductManager>();

            //pøihlašování
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // TODO: zjistit v èem je výhoda založit projekt jako RazorPages a pak ho pøedìlávat na MVC
            services.AddRazorPages();
        }
      

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            //2 režimy chybových hlášek (Devolepment mód)
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
                      
            app.UseHttpsRedirection();  //pøesmìrování z http => https
            app.UseStaticFiles();       //pøístup k souborùm wwwroot

            app.UseRouting();           
            app.UseAuthentication();
            app.UseAuthorization();

            //Pøiøazení uživatele k roli admin (pro úèely 

            //roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
            //ApplicationUser user = userManager.FindByEmailAsync("vera@seznam.cz").Result;
            //userManager.AddToRoleAsync(user, "Admin").Wait();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
                //endpoints.MapRazorPages();
            });
        }
    }
}
