using Microsoft.EntityFrameworkCore;

public class NewsCollectorDbContext : DbContext
{
    public NewsCollectorDbContext(DbContextOptions<NewsCollectorDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<News>().Navigation(e => e.Source).AutoInclude();
        modelBuilder.Entity<Sentiment>().Navigation(e => e.News).AutoInclude();

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Coin> Coins { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<NewsSource> NewsSources { get; set; }
    public DbSet<Prediction> Predictions { get; set; }
    public DbSet<Sentiment> Sentiments { get; set; }
}