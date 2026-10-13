public interface INewsRepository
{
    News? Get(int id);
    List<News> GetAll();
    void Add(News news);
    void Update(News news);
    void Delete(int id);
}

public class NewsRepository : INewsRepository
{
    private readonly NewsCollectorDbContext _dbContext;

    public NewsRepository(NewsCollectorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public News? Get(int id)
    {
        return _dbContext.News.FirstOrDefault(n => n.Id == id);
    }

    public List<News> GetAll()
    {
        return _dbContext.News.ToList();
    }

    public void Add(News news)
    {
        _dbContext.News.Add(news);
        _dbContext.SaveChanges();
    }

    public void Update(News news)
    {
        _dbContext.News.Update(news);
        _dbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        var news = _dbContext.News.FirstOrDefault(n => n.Id == id);
        if (news != null)
        {
            _dbContext.News.Remove(news);
            _dbContext.SaveChanges();
        }
    }
}