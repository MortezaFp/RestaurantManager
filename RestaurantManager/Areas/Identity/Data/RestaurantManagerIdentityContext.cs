using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Areas.Identity.Data;

namespace RestaurantManager.Data;

public class RestaurantManagerIdentityContext : IdentityDbContext<RestaurantManagerUser>
{
    public RestaurantManagerIdentityContext(DbContextOptions<RestaurantManagerIdentityContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}