using Microsoft.EntityFrameworkCore;

namespace MboaPrice.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Commodity> Commodities => Set<Commodity>();
    public DbSet<Market> Markets => Set<Market>();
    public DbSet<PriceReport> Reports => Set<PriceReport>();
    public DbSet<FavoriteCommodity> FavoriteCommodities => Set<FavoriteCommodity>();
    public DbSet<FavoriteMarket> FavoriteMarkets => Set<FavoriteMarket>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<User>().HasIndex(u => u.Email).IsUnique();
        b.Entity<PriceReport>().HasIndex(r => new { r.CommodityId, r.ObservedAt });
        b.Entity<PriceReport>().HasIndex(r => r.UserId);
        b.Entity<FavoriteCommodity>().HasKey(f => new { f.UserId, f.CommodityId });
        b.Entity<FavoriteMarket>().HasKey(f => new { f.UserId, f.MarketId });
    }
}
