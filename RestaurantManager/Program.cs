using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Areas.Identity.Data;
using RestaurantManager.Data;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<RestaurantManagerIdentityContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDbContext<RestaurantManagerContext>(options =>
    options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<RestaurantManagerUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<RestaurantManagerIdentityContext>();
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
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

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
              name: "areas",
              pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );
        });

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        #region CreateRolesAndDefaultUsers

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Admin", "Cashier", "Order Taker" };
            IdentityResult roleResult;

            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<RestaurantManagerUser>>();

            var users = new Dictionary<string, string>()
            {{ "Admin" , "admin@res.com" } ,  { "Cashier" , "cashier@res.com"},{"Order Taker" , "ordertaker@res.com" }};
            string password = "@Morteza973155";

            foreach (KeyValuePair<string, string> kvp in users)
                if (await userManager.FindByEmailAsync(kvp.Value) == null)
                {
                    var user = new RestaurantManagerUser
                    {
                        UserName = kvp.Value,
                        Email = kvp.Value,
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(user, password);
                    await userManager.AddToRoleAsync(user, kvp.Key);
                }
        }

        #endregion CreateRolesAndDefaultUsers

        app.Run();
    }
}