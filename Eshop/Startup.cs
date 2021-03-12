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
        public Startup(IConfiguration configuration) //vstupn� bod ka�d� aplikace
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services) //dependency container
        {
            //datab�ze
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies());

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<ICategoryManager, CategoryManager>();
            services.AddScoped<IProductManager, ProductManager>();

            //p�ihla�ov�n�
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // TODO: zjistit v �em je v�hoda zalo�it projekt jako RazorPages a pak ho p�ed�l�vat na MVC
            services.AddRazorPages();
        }
      

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            //2 re�imy chybov�ch hl�ek (Devolepment m�d)
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
                      
            app.UseHttpsRedirection();  //p�esm�rov�n� z http => https
            app.UseStaticFiles();       //p��stup k soubor�m wwwroot

            app.UseRouting();           
            app.UseAuthentication();
            app.UseAuthorization();

            //P�i�azen� u�ivatele k roli admin (pro ��ely 

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
