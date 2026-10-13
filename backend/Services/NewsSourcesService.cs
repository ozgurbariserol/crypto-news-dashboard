public interface INewsSourcesRepository
{
    NewsSource? Get(int id);
    List<NewsSource> GetAll();
    void Add(NewsSource news);
    void Update(NewsSource news);
    void Delete(int id);
}

public class NewsSourcesRepository : INewsSourcesRepository
{
    private readonly NewsCollectorDbContext _dbContext;

    public NewsSourcesRepository(NewsCollectorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public NewsSource? Get(int id)
    {
        return _dbContext.NewsSources.FirstOrDefault(n => n.Id == id);
    }

    public List<NewsSource> GetAll()
    {
        return _dbContext.NewsSources.ToList();
    }

    public void Add(NewsSource news)
    {
        _dbContext.NewsSources.Add(news);
        _dbContext.SaveChanges();
    }

    public void Update(NewsSource news)
    {
        _dbContext.NewsSources.Update(news);
        _dbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        var news = _dbContext.NewsSources.FirstOrDefault(n => n.Id == id);
        if (news != null)
        {
            _dbContext.NewsSources.Remove(news);
            _dbContext.SaveChanges();
        }
    }
}